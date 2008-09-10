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
	partial class SimpleSyncSendReceive
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
         System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
         System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
         this.messageReceived = new System.Workflow.Activities.CodeActivity();
         this.receiveMessage = new Winterdom.Workflow.Activities.Msmq.MsmqDirectReceiveActivity();
         this.messageSent = new System.Workflow.Activities.CodeActivity();
         this.sendMessage = new Winterdom.Workflow.Activities.Msmq.MsmqSendActivity();
         this.ConfigureObjects = new System.Workflow.Activities.CodeActivity();
         this.receiveTransaction = new System.Workflow.ComponentModel.TransactionScopeActivity();
         this.sendTransaction = new System.Workflow.ComponentModel.TransactionScopeActivity();
         // 
         // messageReceived
         // 
         this.messageReceived.Name = "messageReceived";
         this.messageReceived.ExecuteCode += new System.EventHandler(this.MessageReceived_ExecuteCode);
         activitybind2.Name = "SimpleSyncSendReceive";
         activitybind2.Path = "QUEUE";
         activitybind3.Name = "SimpleSyncSendReceive";
         activitybind3.Path = "CustomerReceived";
         // 
         // receiveMessage
         // 
         this.receiveMessage.IsTransactionalQueue = true;
         activitybind1.Name = "SimpleSyncSendReceive";
         activitybind1.Path = "LabelReceived";
         this.receiveMessage.MessageType = typeof(TestApp.Customer);
         this.receiveMessage.Name = "receiveMessage";
         this.receiveMessage.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqBaseReceiveActivity.LabelProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
         this.receiveMessage.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqBaseReceiveActivity.QueueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
         this.receiveMessage.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqBaseReceiveActivity.MessageReceivedProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
         // 
         // messageSent
         // 
         this.messageSent.Name = "messageSent";
         this.messageSent.ExecuteCode += new System.EventHandler(this.MessageSent_ExecuteCode);
         activitybind6.Name = "SimpleSyncSendReceive";
         activitybind6.Path = "QUEUE";
         // 
         // sendMessage
         // 
         this.sendMessage.IsTransactionalQueue = true;
         activitybind4.Name = "SimpleSyncSendReceive";
         activitybind4.Path = "LabelToSend";
         activitybind5.Name = "SimpleSyncSendReceive";
         activitybind5.Path = "CustomerToSend";
         this.sendMessage.Name = "sendMessage";
         this.sendMessage.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqSendActivity.MessageToSendProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
         this.sendMessage.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqSendActivity.QueueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
         this.sendMessage.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqSendActivity.LabelProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
         // 
         // ConfigureObjects
         // 
         this.ConfigureObjects.Name = "ConfigureObjects";
         this.ConfigureObjects.ExecuteCode += new System.EventHandler(this.ConfigureObject_ExecuteCode);
         // 
         // receiveTransaction
         // 
         this.receiveTransaction.Activities.Add(this.receiveMessage);
         this.receiveTransaction.Activities.Add(this.messageReceived);
         this.receiveTransaction.Name = "receiveTransaction";
         this.receiveTransaction.TransactionOptions.IsolationLevel = System.Transactions.IsolationLevel.Serializable;
         // 
         // sendTransaction
         // 
         this.sendTransaction.Activities.Add(this.ConfigureObjects);
         this.sendTransaction.Activities.Add(this.sendMessage);
         this.sendTransaction.Activities.Add(this.messageSent);
         this.sendTransaction.Name = "sendTransaction";
         this.sendTransaction.TransactionOptions.IsolationLevel = System.Transactions.IsolationLevel.Serializable;
         // 
         // SimpleSyncSendReceive
         // 
         this.Activities.Add(this.sendTransaction);
         this.Activities.Add(this.receiveTransaction);
         this.Name = "SimpleSyncSendReceive";
         this.CanModifyActivities = false;

		}

		#endregion

      private Winterdom.Workflow.Activities.Msmq.MsmqDirectReceiveActivity receiveMessage;
      private Winterdom.Workflow.Activities.Msmq.MsmqSendActivity sendMessage;
      private CodeActivity messageReceived;
      private TransactionScopeActivity receiveTransaction;
      private CodeActivity messageSent;
      private TransactionScopeActivity sendTransaction;
      private CodeActivity ConfigureObjects;














   }
}
