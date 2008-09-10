
//
// MsmqDirectReceive.cs
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
using System.Transactions;
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
   /// Receives a message from an MSMQ Queue directly, without
   /// using the underlying MsmqListenerService. This allows you to
   /// do a synchronous and/or transacted receive.
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
   [SRDescription("DirectRecv_Description")]
   [ToolboxItem(typeof(MsmqActivityToolboxItem))]
   [Designer(typeof(MsmqReceiveActivityDesigner), typeof(IDesigner))]
   [ToolboxBitmap(typeof(MsmqDirectReceiveActivity), "Resources.MessageQueuing.bmp")]
   [ActivityValidator(typeof(MsmqReceiveActivityValidator))]
   public partial class MsmqDirectReceiveActivity 
      : MsmqBaseReceiveActivity
   {
      private bool _isTransactionalQueue;


      #region Public Properties
      //
      // Public Properties
      //

      /// <summary>
      /// The kind of MSMQ transaction to use
      /// when receiving the message
      /// </summary>
      /// <remarks>
      /// You should use this option when receiving
      /// messages from a transactional queues. You can 
      /// set it to Single to receive the message transactionally
      /// or set it to Automatic and then go ahead
      /// and wrap the MsmqDirectReceive activity inside an 
      /// atomic transaction scope, and that will be used when
      /// receiving the message.
      /// </remarks>
      [DefaultValue(false)]
      [SRDescription("Send_IsTransactionalQueue")]
      [Category("Activity")]
      public bool IsTransactionalQueue
      {
         get { return _isTransactionalQueue; }
         set { _isTransactionalQueue = value; }
      }

      #endregion // Public Properties

      /// <summary>
      /// Initializes a new instance
      /// </summary>
      public MsmqDirectReceiveActivity()
      {
      }

      #region Activity Overrides
      //
      // Activity Overrides
      //

      /// <summary>
      /// Executes the activity
      /// </summary>
      /// <param name="context">Execution Context</param>
      /// <returns>The activity Status</returns>
      protected override ActivityExecutionStatus Execute(ActivityExecutionContext context)
      {
         if ( context == null )
            throw new ArgumentNullException("context");

         TraceUtil.WriteInfo("MsmqDirectReceiveActivity::Execute()");

         ProcessMessageFromQueue(context);
         return ActivityExecutionStatus.Closed;
      }

      #endregion // Activity Overrides

      #region Private Methods
      //
      // Private Methods
      //

      private void ProcessMessageFromQueue(ActivityExecutionContext context)
      {
         using ( MessageQueue queue = new MessageQueue(Queue) )
         {
            MessageQueueTransactionType txType = MessageQueueTransactionType.None;
            if ( IsTransactionalQueue )
               txType = MessageQueueTransactionType.Automatic;

            Message msg = queue.Receive(txType);
            TraceUtil.WriteInfo("MsmqDirectReceiveActivity::ProcessMessageFromQueue() - Message Received");
            Label = msg.Label;
            ReadMessageBody(msg);
         }
      }

      #endregion // Private Methods

   } // class MsmqDirectReceive

} // namespace Winterdom.Workflow.Activities.Msmq
