
//
// MsmqReceive.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Messaging;
using System.IO;
using System.Text;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;



namespace Winterdom.Workflow.Activities.Msmq
{
   using Design;
   using Properties;
   using Validation;

   /// <summary>
   /// Receives a message from an MSMQ Queue
   /// </summary>
   /// <remarks>
   /// To receive a message, you need to set the MessageType
   /// property to the .NET type of the object you want to deserialize
   /// the received message as. You then need to bind a property of
   /// that type to the MessageReceived property, and optionally 
   /// bind another to the Label property. When the receive is complete
   /// those properties will contain the actual values of the message received
   /// and its label.
   /// 
   /// <para>The path or format name of the queue to send
   /// the message to needs to be specified in the Queue property
   /// either as a value or by binding it to another property.</para>
   /// </remarks>
   [SRDescription("Recv_Description")]
   [ToolboxItem(typeof(MsmqActivityToolboxItem))]
   [Designer(typeof(MsmqReceiveActivityDesigner), typeof(IDesigner))]
   [ToolboxBitmap(typeof(MsmqReceiveActivity), "Resources.MessageQueuing.bmp")]
   [ActivityValidator(typeof(MsmqReceiveActivityValidator))]
   public partial class MsmqReceiveActivity 
      : MsmqBaseReceiveActivity, IEventActivity, IActivityEventListener<QueueEventArgs>
   {
      private Guid _subscriptionID;
      private bool _activitySubscribed;
      private string _wfQueueName;

      /// <summary>
      /// Initializes a new instance
      /// </summary>
      public MsmqReceiveActivity()
      {
      }

      #region Activity Overrides
      //
      // Activity Overrides
      //

      /// <summary>
      /// Initialize this activity.
      /// </summary>
      /// <param name="provider">Service provider</param>
      protected override void Initialize(IServiceProvider provider)
      {
         if ( provider == null )
            throw new ArgumentNullException("provider");

         TraceUtil.WriteInfo("MsmqReceiveActivity::Initialize()");
         base.Initialize(provider);

         _wfQueueName = "MSMQ_" + Guid.NewGuid();
         //
         // validate all necessary services are configured
         //
         if ( provider.GetService(typeof(MsmqListenerService)) == null )
            throw new InvalidOperationException(Resources.MLServiceNotConfigured);
      }

      protected override void Uninitialize(IServiceProvider provider)
      {
         if ( provider == null )
            throw new ArgumentNullException("provider");

         TraceUtil.WriteInfo("MsmqReceiveActivity::Uninitialize()");
         DeleteWorkflowQueue(provider);
         base.Uninitialize(provider);
      }

      /// <summary>
      /// Executes the activity
      /// </summary>
      /// <param name="context">Execution Context</param>
      /// <returns>The activity Status</returns>
      protected override ActivityExecutionStatus Execute(ActivityExecutionContext context)
      {
         if ( context == null )
            throw new ArgumentNullException("context");

         TraceUtil.WriteInfo("MsmqReceiveActivity::Execute()");
         // 
         // If there are messages pending in the WorkflowQueue
         // it means we are inside an EventDrivenActivity, since we 
         // previously subscribed to the msmq queue as requested by
         // our parent activity. We can just process it and exit.
         //
         // If not, then we reuse our existing mechanism by subcribing
         // to messages (thus requesting the MsmqListenerService to
         // monitor the queue), and waiting for the notification to
         // end. We appear to execute asynchronously to the runtime
         // because of this.
         //
         if ( ProcessMessageFromQueue(context) )
         {
            return ActivityExecutionStatus.Closed;
         }
         ((IEventActivity)this).Subscribe(context, this);
         _activitySubscribed = true;
         return ActivityExecutionStatus.Executing;
      }

      /// <summary>
      /// Cancel the activity execution
      /// </summary>
      /// <param name="executionContext">Execution Context</param>
      /// <returns>The activity status</returns>
      protected override ActivityExecutionStatus Cancel(ActivityExecutionContext executionContext)
      {
         if ( executionContext == null )
            throw new ArgumentNullException("executionContext");
         //
         // tell the MsmqListenerService that we are
         // no longer interested in messages arriving
         // in the queue
         //
         TraceUtil.WriteInfo("MsmqReceiveActivity::Cancel()");
         if ( _activitySubscribed )
         {
            ((IEventActivity)this).Unsubscribe(executionContext, this);
         }
         DeleteWorkflowQueue(executionContext);
         return ActivityExecutionStatus.Closed;
      }

      #endregion // Activity Overrides


      #region IEventActivity Members
      //
      // IEventActivity Members
      //

      IComparable IEventActivity.QueueName
      {
         get { return _wfQueueName; }
      }

      /// <summary>
      /// Subscribe to events being received into the workflow
      /// queue associated with this activity instance. That
      /// will allow us to receive the messages from the 
      /// MsmqListenerService when a message is received
      /// </summary>
      /// <param name="parentContext">Execution Context</param>
      /// <param name="parentEventHandler">Event handler that is subscribed</param>
      void IEventActivity.Subscribe(ActivityExecutionContext parentContext, IActivityEventListener<QueueEventArgs> parentEventHandler)
      {
         if ( parentContext == null )
            throw new ArgumentNullException("parentContext");
         if ( parentEventHandler == null )
            throw new ArgumentNullException("parentEventHandler");

         TraceUtil.WriteInfo("MsmqReceiveActivity::Subscribe({0})", parentContext.Activity.Name);

         WorkflowQueue queue = GetWorkflowQueue(parentContext);
         queue.RegisterForQueueItemAvailable(parentEventHandler, QualifiedName);

         MsmqListenerService msmqSvc = 
            parentContext.GetService<MsmqListenerService>();
         _subscriptionID = msmqSvc.Subscribe(_wfQueueName, Queue);
      }

      /// <summary>
      /// Unsubscribe to events being received into the workflow
      /// queue associated with this activity instance. 
      /// </summary>
      /// <param name="parentContext">Parent Activity Context</param>
      /// <param name="parentEventHandler">Parent Activity Event Handler</param>
      void IEventActivity.Unsubscribe(ActivityExecutionContext parentContext, IActivityEventListener<QueueEventArgs> parentEventHandler)
      {
         if ( parentContext == null )
            throw new ArgumentNullException("parentContext");
         if ( parentEventHandler == null )
            throw new ArgumentNullException("parentEventHandler");

         TraceUtil.WriteInfo("MsmqReceiveActivity::Unsubscribe({0})", parentContext.Activity.Name);

         //
         // Tell our runtime Service that we are not interested
         // in messages arriving at the MSMQ Queue anymore.
         //
         MsmqListenerService msmqSvc = (MsmqListenerService)
            parentContext.GetService(typeof(MsmqListenerService));
         msmqSvc.Unsubscribe(_subscriptionID);
         _subscriptionID = Guid.Empty;

         WorkflowQueue queue = GetWorkflowQueue(parentContext);
         queue.UnregisterForQueueItemAvailable(parentEventHandler);
         // 
         // Notice we do *not* delete the queue here, generally.
         // This is because:
         // a) Unsubscribe can be called multiple times (because of
         //    how we are implemented
         // b) If we close the queue, we won't be able to 
         //    retrieve the message data from it when Execute()
         //    is finally called by our parent EventDrivenActivity
         //    (if we are running event-driven)
         //
      }

      #endregion // IEventActivity Members
      

      #region IActivityEventListener<QueueEventArgs> Members
      //
      // IActivityEventListener<QueueEventArgs> Members
      //

      /// <summary>
      /// Receive notification of an event in our WorkflowQueue
      /// </summary>
      /// <param name="sender">Sender of the event</param>
      /// <param name="e">Event arguments</param>
      void IActivityEventListener<QueueEventArgs>.OnEvent(object sender, QueueEventArgs e)
      {
         //
         // An event has been triggered at our workflow queue.
         // However, this is only triggered if the direct subscriber
         // to the event is ourselves (and not our parent activity), which
         // only happens for us when running outside of an EventDrivenActivity.
         //
         TraceUtil.WriteInfo("MsmqReceiveActivity::OnEvent() - {0}", ExecutionStatus);
         if ( ExecutionStatus == ActivityExecutionStatus.Executing )
         {
             ActivityExecutionContext context = (ActivityExecutionContext)sender;
             if ( ProcessMessageFromQueue(context) )
             {
                 context.CloseActivity();
             }
         }
      }

      #endregion // IActivityEventListener<QueueEventArgs> Members


      #region Private Methods
      //
      // Private Methods
      //

      /// <summary>
      /// Gets the workflow queue to use
      /// or creates it if needed
      /// </summary>
      /// <param name="provider">Service Provider</param>
      /// <returns>A Workflow Queue instance</returns>
      private WorkflowQueue GetWorkflowQueue(IServiceProvider provider)
      {
         WorkflowQueuingService queueSvc = (WorkflowQueuingService)
            provider.GetService(typeof(WorkflowQueuingService));

         WorkflowQueue queue = null;
         if ( queueSvc.Exists(_wfQueueName) )
         {
            queue = queueSvc.GetWorkflowQueue(_wfQueueName);
         } else
         {
            queue = queueSvc.CreateWorkflowQueue(_wfQueueName, true);
            TraceUtil.WriteInfo("MsmqReceiveActivity::Subscribe() - Created Queue {0}", _wfQueueName);
         }
         return queue;
      }

      /// <summary>
      /// Delete the workflow queue associated with
      /// this instance
      /// </summary>
      /// <param name="provider">Service Provider</param>
      private void DeleteWorkflowQueue(IServiceProvider provider)
      {
         WorkflowQueuingService queueSvc = (WorkflowQueuingService)
            provider.GetService(typeof(WorkflowQueuingService));

         if ( queueSvc.Exists(_wfQueueName) )
         {
            queueSvc.DeleteWorkflowQueue(_wfQueueName);
         } 
      }


      /// <summary>
      /// Process a pending event on the workflow queue
      /// </summary>
      /// <param name="context">Execution Context</param>
      /// <returns>true if an event was found and was 
      /// processed, false otherwise</returns>
      private bool ProcessMessageFromQueue(ActivityExecutionContext context)
      {
         WorkflowQueuingService queueSvc = (WorkflowQueuingService)
            context.GetService(typeof(WorkflowQueuingService));
         //
         // If the workflow queue exists, and it contains an event
         // then we extract it and process it. It should contain the 
         // MessageDataEventArgs sent by the MsmqListenerService
         // when it received the message from the MSMQ Queue.
         //
         if ( queueSvc.Exists(_wfQueueName) )
         {
            WorkflowQueue queue = queueSvc.GetWorkflowQueue(_wfQueueName);
            if ( queue.Count > 0 )
            {
               MessageDataEventArgs data = (MessageDataEventArgs)queue.Dequeue();
               OnMessageReceived(this, data);
               if ( _activitySubscribed )
               {
                  ((IEventActivity)this).Unsubscribe(context, this);
               }
               queueSvc.DeleteWorkflowQueue(_wfQueueName);
               return true;
            }
         }
         return false;
      }

      private void OnMessageReceived(object sender, MessageDataEventArgs e)
      {
         Message msg = new Message();
         msg.BodyStream = e.MessageStream;
         msg.BodyType = e.BodyType;

         Label = e.Label;
         ReadMessageBody(msg);
         // todo: fire event
      }
      #endregion // Private Methods

   } // class MsmqReceive

} // namespace Winterdom.Workflow.Activities.Msmq
