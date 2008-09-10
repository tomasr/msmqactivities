using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
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
      private Customer _customer;
		public SampleWorkflow()
		{
			InitializeComponent();

         _customer = new Customer();
         _customer.Id = 1;
         _customer.Name = "Tomas Restrepo";
		}

      private void code1_ExecuteCode(object sender, EventArgs e)
      {
         Console.WriteLine("hola!");
         SendToQueue.MessageToSend = _customer;
      }

      private void code1_ExecuteCode_1(object sender, EventArgs e)
      {
         Console.WriteLine("Message Received: " + ReceiveFromQueue.Label);
         Customer cust = (Customer)ReceiveFromQueue.MessageReceived;
         Console.WriteLine(cust.Name);
      }
	}

}
