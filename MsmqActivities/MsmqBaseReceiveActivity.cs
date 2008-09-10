
//
// MsmqBaseReceive.cs
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
   /// Base class for our receive activities
   /// </summary>
   public partial class MsmqBaseReceiveActivity 
      : Activity
   {
      private Type _messageType;
      private MessageFormatterKind _formatterKind;

      #region Dependency Properties
      //
      // Dependency Properties
      // 

      /// <summary>
      /// The path to the queue is a regular dependency property
      /// </summary>
      public static readonly DependencyProperty QueueProperty =
         DependencyProperty.Register("Queue", typeof(string),
         typeof(MsmqBaseReceiveActivity),
         new PropertyMetadata(new Attribute[] { 
            new ValidationOptionAttribute(ValidationOption.Required) }));

      /// <summary>
      /// The MessageReceived property is an attached
      /// dependency property so that we can bind it to 
      /// an external storage. This allows us easy to leave
      /// the result of receiving the message somewhere else.
      /// </summary>
      public static readonly DependencyProperty MessageReceivedProperty = 
         DependencyProperty.RegisterAttached("MessageReceived", typeof(object), 
         typeof(MsmqBaseReceiveActivity), 
         new PropertyMetadata(DependencyPropertyOptions.NonSerialized, 
         new Attribute[] { new BrowsableAttribute(false), 
            new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content), 
            new ValidationOptionAttribute(ValidationOption.Required) }));
      
      public static object GetMessageReceived(DependencyObject obj)
      {
         return obj.GetValue(MessageReceivedProperty);
      }
      public static void SetMessageReceived(DependencyObject obj, object value)
      {
         obj.SetValue(MessageReceivedProperty, value);
      }

      /// <summary>
      /// Label that the message had when it was received
      /// </summary>
      public static readonly DependencyProperty LabelProperty =
         DependencyProperty.RegisterAttached("Label", typeof(string),
         typeof(MsmqBaseReceiveActivity),
         new PropertyMetadata(DependencyPropertyOptions.Optional,
         new Attribute[] { new BrowsableAttribute(true), 
            new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible), 
            new ValidationOptionAttribute(ValidationOption.Optional) }));

      public static string GetLabel(DependencyObject obj)
      {
         return (string)obj.GetValue(LabelProperty);
      }
      public static void SetLabel(DependencyObject obj, string value)
      {
         obj.SetValue(LabelProperty, value);
      }
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
      [SRDescription("Recv_Queue")]
      [Category("Activity")]
      [ValidationOption(ValidationOption.Required)]
      public string Queue
      {
         get { return (string)base.GetValue(QueueProperty); }
         set { base.SetValue(QueueProperty, value); }
      }


      /// <summary>
      /// Message that was received from the queue.
      /// </summary>
      /// <remarks>
      /// The object will be deserialized using the
      /// specified MessageFormatter
      /// </remarks>
      [SRDescription("Recv_MessageReceived")]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      [Browsable(false)]
      [ValidationOption(ValidationOption.Required)]
      public object MessageReceived
      {
         get { return base.GetValue(MessageReceivedProperty); }
         set { base.SetValue(MessageReceivedProperty, value); }
      }


      /// <summary>
      /// Label that the message received had in the queue
      /// </summary>
      [SRDescription("Recv_Label")]
      [Category("Activity")]
      [ValidationOption(ValidationOption.Optional)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      public string Label
      {
         get { return (string)base.GetValue(LabelProperty); }
         set { base.SetValue(LabelProperty, value); }
      }

      /// <summary>
      /// Type of the message object to expect
      /// </summary>
      [DefaultValue(typeof(string))]
      [SRDescription("Recv_MessageType")]
      [Category("Activity")]
      [Editor(typeof(System.Workflow.ComponentModel.Design.TypeBrowserEditor), 
         typeof(System.Drawing.Design.UITypeEditor))]
      [ValidationOption(ValidationOption.Required)]
      public Type MessageType
      {
         get { return _messageType; }
         set { _messageType = value; }
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

      #endregion // Public Properties

      /// <summary>
      /// Initializes a new instance
      /// </summary>
      public MsmqBaseReceiveActivity()
      {
         Queue = null;
         _messageType = typeof(string);
         _formatterKind = MessageFormatterKind.XmlFormatter;
      }


      #region Protected Methods
      //
      // Private Methods
      //

      /// <summary>
      /// Reads the message body using
      /// the appropriate formatter
      /// </summary>
      /// <param name="msg">Message</param>
      protected void ReadMessageBody(Message msg)
      {
         switch ( FormatterKind )
         {
         case MessageFormatterKind.XmlFormatter:
            msg.Formatter = new XmlMessageFormatter(new Type[] { MessageType });
            MessageReceived = msg.Body;
            break;
         case MessageFormatterKind.BinaryFormatter:
            msg.Formatter = new BinaryMessageFormatter();
            MessageReceived = msg.Body;
            break;
         case MessageFormatterKind.ActiveXFormatter:
            msg.Formatter = new ActiveXMessageFormatter();
            MessageReceived = msg.Body;
            break;
         case MessageFormatterKind.Raw:
            // not supported yet
            throw new NotImplementedException();
         }
      }

      #endregion // Protected Methods

   } // class MsmqBaseReceive

} // namespace Winterdom.Workflow.Activities.Msmq
