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
	public sealed partial class SampleWorkflow1: SequentialWorkflowActivity
	{
      public Customer _customerToSend;
      public Customer _customerReceived;
      public string _labelToSend;
      public string _labelReceived;

      public const string QUEUE = @".\Private$\MsmqQueue";

		public SampleWorkflow1()
		{
			InitializeComponent();
		}
   
      private void ConfigureObject_ExecuteCode(object sender, EventArgs e)
      {
         _customerToSend = new Customer();
         _customerToSend.Id = 1;
         _customerToSend.Name = "Tomas Restrepo";

         _labelToSend = Guid.NewGuid().ToString();
      }

      private void MessageSent_ExecuteCode(object sender, EventArgs e)
      {
         Console.WriteLine("Message was sent");
      }

      private void MessageReceived_ExecuteCode(object sender, EventArgs e)
      {
         if ( _customerReceived != null )
         {
            Console.WriteLine("Message Label: {0}", _labelReceived);
            Console.WriteLine("Message Received: {0}", _customerReceived.Name);
         } else
         {
            Console.WriteLine("Message was not received in the specified time");
         }
      }

      private void DelayDone_ExecuteCode(object sender, EventArgs e)
      {
         Console.WriteLine("Timeout waiting for message");
      }

   }

}
