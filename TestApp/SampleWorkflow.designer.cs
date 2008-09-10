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
   public sealed partial class SampleWorkflow : SequentialWorkflow
   {
		#region Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
         this.SetMessageToSend = new System.Workflow.Activities.Code();
         this.SendToQueue = new Winterdom.Workflow.Activities.Msmq.MsmqSend();
         this.ReceiveFromQueue = new Winterdom.Workflow.Activities.Msmq.MsmqReceive();
         this.code1 = new System.Workflow.Activities.Code();
         // 
         // SetMessageToSend
         // 
         this.SetMessageToSend.ID = "SetMessageToSend";
         this.SetMessageToSend.ExecuteCode += new System.EventHandler(this.code1_ExecuteCode);
         // 
         // SendToQueue
         // 
         this.SendToQueue.ID = "SendToQueue";
         this.SendToQueue.Label = "zxzccz";
         this.SendToQueue.QueuePath = ".\\Private$\\TestQueue";
         this.SendToQueue.TransactionType = System.Messaging.MessageQueueTransactionType.Single;
         // 
         // ReceiveFromQueue
         // 
         this.ReceiveFromQueue.ID = "ReceiveFromQueue";
         this.ReceiveFromQueue.MessageType = typeof(TestApp.Customer);
         this.ReceiveFromQueue.QueuePath = ".\\Private$\\TestQueue";
         this.ReceiveFromQueue.TransactionType = System.Messaging.MessageQueueTransactionType.Single;
         // 
         // code1
         // 
         this.code1.ID = "code1";
         this.code1.ExecuteCode += new System.EventHandler(this.code1_ExecuteCode_1);
         // 
         // SampleWorkflow
         // 
         this.Activities.Add(this.SetMessageToSend);
         this.Activities.Add(this.SendToQueue);
         this.Activities.Add(this.ReceiveFromQueue);
         this.Activities.Add(this.code1);
         this.DynamicUpdateCondition = null;
         this.ID = "SampleWorkflow";

		}

		#endregion

      private Code SetMessageToSend;
      private Winterdom.Workflow.Activities.Msmq.MsmqReceive ReceiveFromQueue;
      private Code code1;
      private Winterdom.Workflow.Activities.Msmq.MsmqSend SendToQueue;




















   }
}
