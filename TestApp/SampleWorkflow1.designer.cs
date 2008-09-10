using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace TestApp
{
	public sealed partial class SampleWorkflow1
	{
		#region Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
         this.CanModifyActivities = true;
         System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
         this.MessageReceived = new System.Workflow.Activities.CodeActivity();
         this.ReceiveMessage = new Winterdom.Workflow.Activities.Msmq.MsmqReceiveActivity();
         this.DelayDone = new System.Workflow.Activities.CodeActivity();
         this.ReceiveTimeout = new System.Workflow.Activities.DelayActivity();
         this.SendMessage = new Winterdom.Workflow.Activities.Msmq.MsmqSendActivity();
         this.eventDrivenActivity2 = new System.Workflow.Activities.EventDrivenActivity();
         this.eventDrivenActivity1 = new System.Workflow.Activities.EventDrivenActivity();
         this.MessageSent = new System.Workflow.Activities.CodeActivity();
         this.SendScope = new System.Workflow.ComponentModel.TransactionScopeActivity();
         this.DelaySend = new System.Workflow.Activities.DelayActivity();
         this.ConfigureObject = new System.Workflow.Activities.CodeActivity();
         this.listenActivity1 = new System.Workflow.Activities.ListenActivity();
         this.sequenceActivity2 = new System.Workflow.Activities.SequenceActivity();
         this.sequenceActivity1 = new System.Workflow.Activities.SequenceActivity();
         this.parallelActivity1 = new System.Workflow.Activities.ParallelActivity();
         // 
         // MessageReceived
         // 
         this.MessageReceived.Name = "MessageReceived";
         this.MessageReceived.ExecuteCode += new System.EventHandler(this.MessageReceived_ExecuteCode);
         activitybind2.Name = "SampleWorkflow1";
         activitybind2.Path = "QUEUE";
         activitybind3.Name = "SampleWorkflow1";
         activitybind3.Path = "_customerReceived";
         // 
         // ReceiveMessage
         // 
         activitybind1.Name = "SampleWorkflow1";
         activitybind1.Path = "_labelReceived";
         this.ReceiveMessage.MessageType = typeof(TestApp.Customer);
         this.ReceiveMessage.Name = "ReceiveMessage";
         this.ReceiveMessage.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqReceiveActivity.QueueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
         this.ReceiveMessage.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqReceiveActivity.MessageReceivedProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
         this.ReceiveMessage.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqReceiveActivity.LabelProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
         // 
         // DelayDone
         // 
         this.DelayDone.Name = "DelayDone";
         this.DelayDone.ExecuteCode += new System.EventHandler(this.DelayDone_ExecuteCode);
         // 
         // ReceiveTimeout
         // 
         this.ReceiveTimeout.Name = "ReceiveTimeout";
         this.ReceiveTimeout.TimeoutDuration = System.TimeSpan.Parse("00:00:10");
         activitybind6.Name = "SampleWorkflow1";
         activitybind6.Path = "QUEUE";
         // 
         // SendMessage
         // 
         this.SendMessage.IsTransactionalQueue = true;
         activitybind4.Name = "SampleWorkflow1";
         activitybind4.Path = "_labelToSend";
         activitybind5.Name = "SampleWorkflow1";
         activitybind5.Path = "_customerToSend";
         this.SendMessage.Name = "SendMessage";
         this.SendMessage.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqSendActivity.MessageToSendProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
         this.SendMessage.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqSendActivity.QueueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
         this.SendMessage.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqSendActivity.LabelProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
         // 
         // eventDrivenActivity2
         // 
         this.eventDrivenActivity2.Activities.Add(this.ReceiveMessage);
         this.eventDrivenActivity2.Activities.Add(this.MessageReceived);
         this.eventDrivenActivity2.Name = "eventDrivenActivity2";
         // 
         // eventDrivenActivity1
         // 
         this.eventDrivenActivity1.Activities.Add(this.ReceiveTimeout);
         this.eventDrivenActivity1.Activities.Add(this.DelayDone);
         this.eventDrivenActivity1.Name = "eventDrivenActivity1";
         // 
         // MessageSent
         // 
         this.MessageSent.Name = "MessageSent";
         this.MessageSent.ExecuteCode += new System.EventHandler(this.MessageSent_ExecuteCode);
         // 
         // SendScope
         // 
         this.SendScope.Activities.Add(this.SendMessage);
         this.SendScope.Name = "SendScope";
         this.SendScope.TransactionOptions.IsolationLevel = System.Transactions.IsolationLevel.Serializable;
         // 
         // DelaySend
         // 
         this.DelaySend.Name = "DelaySend";
         this.DelaySend.TimeoutDuration = System.TimeSpan.Parse("00:00:00");
         // 
         // ConfigureObject
         // 
         this.ConfigureObject.Name = "ConfigureObject";
         this.ConfigureObject.ExecuteCode += new System.EventHandler(this.ConfigureObject_ExecuteCode);
         // 
         // listenActivity1
         // 
         this.listenActivity1.Activities.Add(this.eventDrivenActivity1);
         this.listenActivity1.Activities.Add(this.eventDrivenActivity2);
         this.listenActivity1.Name = "listenActivity1";
         // 
         // sequenceActivity2
         // 
         this.sequenceActivity2.Activities.Add(this.ConfigureObject);
         this.sequenceActivity2.Activities.Add(this.DelaySend);
         this.sequenceActivity2.Activities.Add(this.SendScope);
         this.sequenceActivity2.Activities.Add(this.MessageSent);
         this.sequenceActivity2.Name = "sequenceActivity2";
         // 
         // sequenceActivity1
         // 
         this.sequenceActivity1.Activities.Add(this.listenActivity1);
         this.sequenceActivity1.Name = "sequenceActivity1";
         // 
         // parallelActivity1
         // 
         this.parallelActivity1.Activities.Add(this.sequenceActivity1);
         this.parallelActivity1.Activities.Add(this.sequenceActivity2);
         this.parallelActivity1.Name = "parallelActivity1";
         // 
         // SampleWorkflow1
         // 
         this.Activities.Add(this.parallelActivity1);
         this.Name = "SampleWorkflow1";
         this.CanModifyActivities = false;

		}

		#endregion

      private DelayActivity DelaySend;
      private CodeActivity DelayDone;
      private ParallelActivity parallelActivity1;
      private SequenceActivity sequenceActivity1;
      private ListenActivity listenActivity1;
      private EventDrivenActivity eventDrivenActivity1;
      private DelayActivity ReceiveTimeout;
      private EventDrivenActivity eventDrivenActivity2;
      private SequenceActivity sequenceActivity2;
      private TransactionScopeActivity SendScope;
      private CodeActivity ConfigureObject;
      private CodeActivity MessageSent;
      private CodeActivity MessageReceived;
      private Winterdom.Workflow.Activities.Msmq.MsmqSendActivity SendMessage;
      private Winterdom.Workflow.Activities.Msmq.MsmqReceiveActivity ReceiveMessage;















































   }
}
