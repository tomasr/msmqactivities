
//
// MsmsqListenerService.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Transactions;

using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;

namespace Winterdom.Workflow.Activities.Msmq
{
   using Properties;

   /// <summary>
   /// Service class that implements the listener
   /// service for the MSMQ Queues for the Receive
   /// activity.
   /// </summary>
   /// <remarks>
   /// An instance of this service should be present 
   /// on the Services collection of the WorkflowRuntime
   /// for every AppDomain that wants to use the 
   /// MsmqReceiveActivity. 
   /// 
   /// <para>You can add it like this by code:</para>
   /// <example><code><![CDATA[
   /// WorkflowRuntime workflowRuntime = new WorkflowRuntime();
   /// workflowRuntime.AddService(new MsmqListenerService());
   /// ]]>
   /// </code></example>
   /// </remarks>
   public class MsmqListenerService : WorkflowRuntimeService
   {
      private const string DEFAULT_HOST = "default";
      private Dictionary<Guid, MsmqSubscription> _subscriptionsByID;
      private Dictionary<string, Receiver> _queueReceivers;
      private ReaderWriterLock _lock;
      private string _hostname;

      public MsmqListenerService()
      {
         _hostname = DEFAULT_HOST;
      }

      public MsmqListenerService(NameValueCollection parameters)
      {
         if ( parameters == null )
            throw new ArgumentNullException("parameters");

         _hostname = parameters["hostname"];
         if ( String.IsNullOrEmpty(_hostname) )
            _hostname = DEFAULT_HOST;
      }



      /// <summary>
      /// Subscribe to messages received from an MSMQ Queue
      /// </summary>
      /// <param name="wfQueueName">Workflow Queue to post received messages to</param>
      /// <param name="msmqQueue">MSMQ Queue to monitor</param>
      /// <returns>The subscription ID</returns>
      internal Guid Subscribe(IComparable wfQueueName, string msmqQueue)
      {
         if ( wfQueueName == null )
            throw new ArgumentNullException("wfQueueName");
         if ( String.IsNullOrEmpty(msmqQueue) )
            throw new ArgumentNullException("msmqQueue");

         MessageQueue queue = new MessageQueue(msmqQueue);

         MsmqSubscription subscription = new MsmqSubscription();
         subscription.WfQueueName = wfQueueName;
         subscription.MsmqQueue = queue.FormatName;
         subscription.WorkflowInstance = 
            WorkflowEnvironment.WorkflowInstanceId;

         using ( TransactionScope scope = new TransactionScope() )
         {
            PersistSubscription(subscription);
            ActivateSubscription(queue, subscription);
            scope.Complete();
            return subscription.ID;
         }
      }

      /// <summary>
      /// Remove a subscription to messages being received
      /// from an MSMQ Queue
      /// </summary>
      /// <param name="subscriptionID">ID of Subscription to remove</param>
      /// <remarks>
      /// If there is no more pending subscriptions, we stop
      /// listening to the queue
      /// </remarks>
      internal void Unsubscribe(Guid subscriptionID)
      {
         try
         {
            _lock.AcquireWriterLock(Timeout.Infinite);

            if ( _subscriptionsByID.ContainsKey(subscriptionID) )
            {
               MsmqSubscription subscription = 
                  _subscriptionsByID[subscriptionID];
               _subscriptionsByID.Remove(subscriptionID);

               Receiver receiver = _queueReceivers[subscription.MsmqQueue];
               receiver.Release();
               RemoveSubscription(subscription);
            }
         } finally
         {
            _lock.ReleaseWriterLock();
         }
      }

      #region Overrides
      //
      // Overrides
      //

      /// <summary>
      /// Initialize our service when the Workflow
      /// Runtime starts it.
      /// </summary>
      protected override void OnStarted()
      {
         base.OnStarted();
         _subscriptionsByID = new Dictionary<Guid, MsmqSubscription>();
         _queueReceivers = new Dictionary<string, Receiver>();
         _lock = new ReaderWriterLock();

         IMsmqSubscriptionPersistenceService svc =
            Runtime.GetService<IMsmqSubscriptionPersistenceService>();

         if ( svc == null )
            throw new InvalidOperationException(Resources.IMSPSNotConfigured);

         foreach ( MsmqSubscription subscription in svc.LoadAll(_hostname) )
         {
            string name = "FormatName:" + subscription.MsmqQueue;
            MessageQueue queue = new MessageQueue(name);
            ActivateSubscription(queue, subscription);
         }
      }

      #endregion // Overrides

      #region Private Methods
      //
      // Private Methods
      //

      /// <summary>
      /// Persist the specified subscription
      /// </summary>
      /// <param name="subscription">Subscription to Persist</param>
      private void PersistSubscription(MsmqSubscription subscription)
      {
         IMsmqSubscriptionPersistenceService svc =
            Runtime.GetService<IMsmqSubscriptionPersistenceService>();
         svc.Persist(_hostname, subscription);
      }

      /// <summary>
      /// Remove the specified susbcription
      /// </summary>
      /// <param name="subscription">Subscription to remove</param>
      private void RemoveSubscription(MsmqSubscription subscription)
      {
         IMsmqSubscriptionPersistenceService svc =
            Runtime.GetService<IMsmqSubscriptionPersistenceService>();
         svc.Remove(subscription);
      }


      /// <summary>
      /// Activates a subscription by starting
      /// to listen to the specified queue
      /// </summary>
      /// <param name="queue">Queue to activate</param>
      /// <param name="subscription">Subscription to activate</param>
      private void ActivateSubscription(MessageQueue queue, MsmqSubscription subscription)
      {
         try
         {
            _lock.AcquireWriterLock(Timeout.Infinite);
            _subscriptionsByID.Add(subscription.ID, subscription);
            // do we already have suscriptions to this queue?
            Receiver receiver = null;
            if ( !_queueReceivers.ContainsKey(queue.FormatName) )
            {
               receiver = new Receiver(queue, OnMessageReceived);
               _queueReceivers.Add(queue.FormatName, receiver);
            } else
            {
               receiver = _queueReceivers[queue.FormatName];
               receiver.AddRef();
               queue.Dispose();
            }
         } finally
         {
            _lock.ReleaseWriterLock();
         }
      }

      /// <summary>
      /// A message was received on monitored MSMQ queue
      /// </summary>
      /// <param name="receiver">MSMQ Receiver that received the message</param>
      /// <param name="message">MSMQ Message received</param>
      /// <remarks>
      /// We want to now notify the original workflow that subscribed to this
      /// MSMQ queue that a message was received. For this we need to first copy
      /// all the relevant data in the MSMQ message into a serializable format
      /// so we can put it into the WorkflowQueue given to us when
      /// the subscription was done.
      /// </remarks>
      private void OnMessageReceived(Receiver receiver, Message message)
      {
         if ( receiver == null )
            throw new ArgumentNullException("receiver");
         if ( message == null )
            throw new ArgumentNullException("message");

         try
         {
            _lock.AcquireReaderLock(Timeout.Infinite);

            foreach ( MsmqSubscription subscription in _subscriptionsByID.Values )
            {
               StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
               if ( comparer.Compare(subscription.MsmqQueue, message.DestinationQueue.FormatName) == 0 )
               {
                  // ensure that each instance gets it's own copy
                  MessageDataEventArgs data = new MessageDataEventArgs(message);
                  NotifyWorkflow(subscription, data);
               }
            }
         } finally
         {
            _lock.ReleaseReaderLock();
         }
      }

      private void NotifyWorkflow(MsmqSubscription subscription, MessageDataEventArgs data)
      {
         TraceUtil.WriteInfo("MsmqListenerService::NotifyWorkflow({0}, {1})", subscription.MsmqQueue, subscription.WfQueueName);

         WorkflowInstance instance = 
            Runtime.GetWorkflow(subscription.WorkflowInstance);
         instance.EnqueueItem(subscription.WfQueueName, data, null, null);
      }

      #endregion // Private Methods


      #region MSMQ Receiver
      //
      // MSMQ Receiver
      //

      delegate void ReceiveMessageHandler(Receiver receiver, Message message);

      /// <summary>
      /// Internal receiver class that listens to messages from
      /// an MSMQ Queue
      /// </summary>
      class Receiver
      {
         private MessageQueue _queue;
         private int _refcount;
         private ReceiveMessageHandler _handler;

         /// <summary>
         /// Message queue we listen to
         /// </summary>
         public MessageQueue Queue
         {
            get { return _queue; }
         }
         /// <summary>
         /// This receiver reference count
         /// </summary>
         public int Refcount
         {
            get { return _refcount; }
         }

         /// <summary>
         /// Initialize a new receiver
         /// </summary>
         /// <param name="queue">MSMQ Queue to listen to</param>
         /// <param name="handler">Delegate to call once we receive a message</param>
         public Receiver(MessageQueue queue, ReceiveMessageHandler handler)
         {
            _refcount++;
            _queue = queue;
            _queue.ReceiveCompleted += OnMessageReceived;
            _queue.MessageReadPropertyFilter.DestinationQueue = true;
            _handler = handler;

            ReceiveMessage();
         }

         public void AddRef()
         {
            _refcount++;
         }
         public void Release()
         {
            _refcount--;
            if ( _refcount == 0 )
            {
               _queue.Dispose();
            }
         }

         private void ReceiveMessage()
         {
            IAsyncResult res = _queue.BeginReceive();
         }

         private void OnMessageReceived(object sender, ReceiveCompletedEventArgs e)
         {
            try
            {
               Message msg = _queue.EndReceive(e.AsyncResult);
               _handler(this, msg);
               ReceiveMessage();
            } catch ( MessageQueueException ex )
            {
               // if queue was closed, all is cool
               if ( (int)ex.MessageQueueErrorCode != -1073741536 )
               {
                  TraceUtil.WriteException(ex);
                  throw;
               }
            }
         }

      } // class Receiver

      #endregion // MSMQ Receiver


   } // class MsmqListenerService

} // namespace Winterdom.Workflow.Activities.Msmq
