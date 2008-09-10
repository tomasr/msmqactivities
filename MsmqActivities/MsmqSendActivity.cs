
//
// MsmqSendActivity.cs
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
   using Validation;

   /// <summary>
   /// Sends a message to an MSMQ Queue.
   /// </summary>
   /// <remarks>
   /// The message can be represented by an instance
   /// of any type that can be sent through one of the supported
   /// MSMQ Message Formatters, as specified by the 
   /// <c>MessageFormatterKind</c> enumeration.
   /// 
   /// <para>The path or format name of the queue to send
   /// the message to needs to be specified in the Queue property
   /// either as a value or by binding it to another property. 
   /// You can also specify the label to apply to the message when 
   /// sending it by binding or setting the Label property.</para>
   /// </remarks>
   [SRDescription("Send_Description")]
   [ToolboxItem(typeof(MsmqActivityToolboxItem))]
   [Designer(typeof(MsmqSendActivityDesigner), typeof(IDesigner))]
   [ToolboxBitmap(typeof(MsmqSendActivity), "Resources.MessageQueuing.bmp")]
   [ActivityValidator(typeof(MsmqSendActivityValidator))]
   public partial class MsmqSendActivity : Activity, IPendingWork
   {
      private MessageFormatterKind _formatterKind;
      private bool _isTransactionalQueue;

      #region Dependency Properties
      //
      // Dependency Properties
      //

      /// <summary>
      /// The path to the queue is a regular dependency property
      /// </summary>
      public static readonly DependencyProperty QueueProperty =
         DependencyProperty.Register("Queue", typeof(string),
         typeof(MsmqSendActivity),
         new PropertyMetadata(new Attribute[] { 
            new ValidationOptionAttribute(ValidationOption.Required) }));

      /// <summary>
      /// Message Object to send to the queue
      /// </summary>
      public static readonly DependencyProperty MessageToSendProperty = 
         DependencyProperty.Register("MessageToSend", typeof(object), 
         typeof(MsmqSendActivity),
         new PropertyMetadata(new Attribute[] { 
            new ValidationOptionAttribute(ValidationOption.Required) }));

      /// <summary>
      /// Label to give to the message sent
      /// </summary>
      public static readonly DependencyProperty LabelProperty =
         DependencyProperty.Register("Label", typeof(string),
         typeof(MsmqSendActivity),
         new PropertyMetadata(new Attribute[] { 
            new ValidationOptionAttribute(ValidationOption.Optional) }));
      #endregion // Dependency Properties


      #region Public Properties
      //
      // Public Properties
      //

      /// <summary>
      /// Path to the queue that you want to
      /// receive the message from.
      /// </summary>
      [DefaultValue(null)]
      [SRDescription("Send_Queue")]
      [Category("Activity")]
      [ValidationOption(ValidationOption.Required)]
      public string Queue
      {
         get { return (string)base.GetValue(QueueProperty); }
         set { base.SetValue(QueueProperty, value); }
      }

      /// <summary>
      /// Label that the message received had in the queue
      /// </summary>
      [DefaultValue("")]
      [SRDescription("Send_Label")]
      [Category("Activity")]
      [ValidationOptionAttribute(ValidationOption.Optional)]
      public string Label
      {
         get { return (string)base.GetValue(LabelProperty); }
         set { base.SetValue(LabelProperty, value); }
      }

      /// <summary>
      /// Message to send to the queue
      /// </summary>
      [Browsable(true)]
      [ValidationOption(ValidationOption.Required)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [SRDescription("Send_MessageToSend")]
      [Category("Activity")]
      public object MessageToSend
      {
         get { return base.GetValue(MsmqSendActivity.MessageToSendProperty); }
         set { base.SetValue(MsmqSendActivity.MessageToSendProperty, value); }
      }

      /// <summary>
      /// Specifies the kind of MSMQ Message
      /// Formatter to use
      /// </summary>
      [DefaultValue(MessageFormatterKind.XmlFormatter)]
      [SRDescription("Gen_FormatterKind")]
      [Category("Activity")]
      public MessageFormatterKind FormatterKind
      {
         get { return _formatterKind; }
         set { _formatterKind = value; }
      }

      /// <summary>
      /// The kind of MSMQ transaction to use
      /// when sending the message
      /// </summary>
      /// <remarks>
      /// You should use this option when sending
      /// messages to transactional queues. You can 
      /// set it to Single to send the message transactionally
      /// or set it to Automatic and then go ahead
      /// and wrap the MsmqSend activity inside an 
      /// atomic transaction scope, and that will be used when
      /// sending the message.
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
      public MsmqSendActivity()
      {
         Queue = null;
         Label = "";
         _formatterKind = MessageFormatterKind.XmlFormatter;
         _isTransactionalQueue = false;
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

         TraceUtil.WriteInfo("MsmqSendActivity::Execute()");

         if ( this.MessageToSend != null )
         {
            Message message = new Message();
            message.Label = Label;
            WriteMessageBody(message);
            MsmqSendWorkItem item = new MsmqSendWorkItem(Queue, message);

            if ( IsTransactionalQueue )
            {
               WorkflowEnvironment.WorkBatch.Add(this, item);
            } else
            {
               DoSend(item, false);
            }
         }
         return ActivityExecutionStatus.Closed;
      }

      /// <summary>
      /// Does the actual send of a message to a queue
      /// </summary>
      /// <param name="item">Work item to send</param>
      /// <param name="transactional">True if we need a transactional send</param>
      private void DoSend(MsmqSendWorkItem item, bool transactional)
      {
         TraceUtil.WriteInfo("MsmqSendActivity::DoSend({0})", item.Queue);
         using ( MessageQueue queue = new MessageQueue(item.Queue) )
         {
            MessageQueueTransactionType txType = MessageQueueTransactionType.None;
            if ( transactional )
               txType = MessageQueueTransactionType.Automatic;

            queue.Send(item.Message, txType);
         }
      }

      /// <summary>
      /// Write the message contents into the Msmq body
      /// according to the selected formatter
      /// </summary>
      /// <param name="msg">Msmq Message to write to</param>
      private void WriteMessageBody(Message msg)
      {
         switch ( FormatterKind )
         {
         case MessageFormatterKind.ActiveXFormatter:
            msg.Formatter = new ActiveXMessageFormatter();
            break;
         case MessageFormatterKind.BinaryFormatter:
            msg.Formatter = new BinaryMessageFormatter();
            break;
         case MessageFormatterKind.XmlFormatter:
            Type type = MessageToSend.GetType();
            msg.Formatter = new XmlMessageFormatter(new Type[] { type });
            break;
         case MessageFormatterKind.Raw:
            throw new NotImplementedException();
         }
         msg.Body = MessageToSend;
      }


      #region IPendingWork Members

      void IPendingWork.Commit(System.Transactions.Transaction transaction, ICollection items)
      {
         TraceUtil.WriteInfo("IPendingWork::Commit()");
         using ( TransactionScope scope = new TransactionScope(transaction) )
         {
            foreach ( MsmqSendWorkItem item in items )
            {
               DoSend(item, true);
            }
            scope.Complete();
         }
      }

      void IPendingWork.Complete(bool succeeded, ICollection items)
      {
         TraceUtil.WriteInfo("IPendingWork::Complete({0})", succeeded);
         // we really don't care, so don't do anything.
      }

      bool IPendingWork.MustCommit(ICollection items)
      {
         TraceUtil.WriteInfo("IPendingWork::MustCommit()");
         // yes, we do want to commit whatever we have pending
         return true;
      }

      #endregion // IPendingWork implementation


      #region class MsmqSendWorkItem
      //
      // class MsmqSendWorkItem
      //

      /// <summary>
      /// Work item type used
      /// for transactional batching
      /// </summary>
      private class MsmqSendWorkItem
      {
         private Message _message;
         private string _queue;

         public Message Message
         {
            get { return _message; }
         }

         public string Queue
         {
            get { return _queue; }
         }

         public MsmqSendWorkItem(string queue, Message message)
         {
            _queue = queue;
            _message = message;
         }
      } // class MsmqSendWorkItem

      #endregion // class MsmqSendWorkItem

   } // class MsmqSendActivity

} // namespace Winterdom.Workflow.Activities.Msmq
