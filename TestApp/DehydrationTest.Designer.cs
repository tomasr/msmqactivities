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
	partial class DehydrationTest
	{
		#region Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
		private void InitializeComponent()
		{
         this.CanModifyActivities = true;
         System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
         this.msmqReceiveActivity1 = new Winterdom.Workflow.Activities.Msmq.MsmqReceiveActivity();
         this.delayActivity2 = new System.Workflow.Activities.DelayActivity();
         this.eventDrivenActivity2 = new System.Workflow.Activities.EventDrivenActivity();
         this.eventDrivenActivity1 = new System.Workflow.Activities.EventDrivenActivity();
         this.msmqSendActivity1 = new Winterdom.Workflow.Activities.Msmq.MsmqSendActivity();
         this.listenActivity1 = new System.Workflow.Activities.ListenActivity();
         this.transactionScopeActivity1 = new System.Workflow.ComponentModel.TransactionScopeActivity();
         this.delayActivity1 = new System.Workflow.Activities.DelayActivity();
         this.sequenceActivity2 = new System.Workflow.Activities.SequenceActivity();
         this.sequenceActivity1 = new System.Workflow.Activities.SequenceActivity();
         this.codeActivity1 = new System.Workflow.Activities.CodeActivity();
         this.parallelActivity1 = new System.Workflow.Activities.ParallelActivity();
         activitybind1.Name = "DehydrationTest";
         activitybind1.Path = "LabelReceived";
         activitybind2.Name = "DehydrationTest";
         activitybind2.Path = "QUEUE";
         activitybind3.Name = "DehydrationTest";
         activitybind3.Path = "CustomerReceived";
         // 
         // msmqReceiveActivity1
         // 
         this.msmqReceiveActivity1.MessageType = typeof(TestApp.Customer);
         this.msmqReceiveActivity1.Name = "msmqReceiveActivity1";
         this.msmqReceiveActivity1.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqBaseReceiveActivity.LabelProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
         this.msmqReceiveActivity1.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqBaseReceiveActivity.QueueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
         this.msmqReceiveActivity1.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqBaseReceiveActivity.MessageReceivedProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
         // 
         // delayActivity2
         // 
         this.delayActivity2.Name = "delayActivity2";
         this.delayActivity2.TimeoutDuration = System.TimeSpan.Parse("00:05:00");
         // 
         // eventDrivenActivity2
         // 
         this.eventDrivenActivity2.Activities.Add(this.msmqReceiveActivity1);
         this.eventDrivenActivity2.Name = "eventDrivenActivity2";
         // 
         // eventDrivenActivity1
         // 
         this.eventDrivenActivity1.Activities.Add(this.delayActivity2);
         this.eventDrivenActivity1.Name = "eventDrivenActivity1";
         activitybind6.Name = "DehydrationTest";
         activitybind6.Path = "QUEUE";
         // 
         // msmqSendActivity1
         // 
         this.msmqSendActivity1.IsTransactionalQueue = true;
         activitybind4.Name = "DehydrationTest";
         activitybind4.Path = "LabelToSend";
         activitybind5.Name = "DehydrationTest";
         activitybind5.Path = "CustomerToSend";
         this.msmqSendActivity1.Name = "msmqSendActivity1";
         this.msmqSendActivity1.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqSendActivity.LabelProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
         this.msmqSendActivity1.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqSendActivity.MessageToSendProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
         this.msmqSendActivity1.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqSendActivity.QueueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
         // 
         // listenActivity1
         // 
         this.listenActivity1.Activities.Add(this.eventDrivenActivity1);
         this.listenActivity1.Activities.Add(this.eventDrivenActivity2);
         this.listenActivity1.Name = "listenActivity1";
         // 
         // transactionScopeActivity1
         // 
         this.transactionScopeActivity1.Activities.Add(this.msmqSendActivity1);
         this.transactionScopeActivity1.Name = "transactionScopeActivity1";
         this.transactionScopeActivity1.TransactionOptions.IsolationLevel = System.Transactions.IsolationLevel.Serializable;
         // 
         // delayActivity1
         // 
         this.delayActivity1.Name = "delayActivity1";
         this.delayActivity1.TimeoutDuration = System.TimeSpan.Parse("00:01:00");
         // 
         // sequenceActivity2
         // 
         this.sequenceActivity2.Activities.Add(this.listenActivity1);
         this.sequenceActivity2.Name = "sequenceActivity2";
         // 
         // sequenceActivity1
         // 
         this.sequenceActivity1.Activities.Add(this.delayActivity1);
         this.sequenceActivity1.Activities.Add(this.transactionScopeActivity1);
         this.sequenceActivity1.Name = "sequenceActivity1";
         // 
         // codeActivity1
         // 
         this.codeActivity1.Name = "codeActivity1";
         this.codeActivity1.ExecuteCode += new System.EventHandler(this.MessageReceived_ExecuteCode);
         // 
         // parallelActivity1
         // 
         this.parallelActivity1.Activities.Add(this.sequenceActivity1);
         this.parallelActivity1.Activities.Add(this.sequenceActivity2);
         this.parallelActivity1.Name = "parallelActivity1";
         // 
         // DehydrationTest
         // 
         this.Activities.Add(this.parallelActivity1);
         this.Activities.Add(this.codeActivity1);
         this.Name = "DehydrationTest";
         this.CanModifyActivities = false;

		}

		#endregion

      private TransactionScopeActivity transactionScopeActivity1;
      private SequenceActivity sequenceActivity2;
      private SequenceActivity sequenceActivity1;
      private ParallelActivity parallelActivity1;
      private EventDrivenActivity eventDrivenActivity2;
      private EventDrivenActivity eventDrivenActivity1;
      private ListenActivity listenActivity1;
      private DelayActivity delayActivity2;
      private Winterdom.Workflow.Activities.Msmq.MsmqReceiveActivity msmqReceiveActivity1;
      private DelayActivity delayActivity1;
      private Winterdom.Workflow.Activities.Msmq.MsmqSendActivity msmqSendActivity1;
      private CodeActivity codeActivity1;










   }
}
