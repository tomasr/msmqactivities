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
   public partial class SampleWorkflow2 : System.Workflow.Activities.SequentialWorkflowActivity
	{
      public Customer CustomerToSend;
      public Customer CustomerReceived;
      public string LabelToSend;
      public string LabelReceived;

      public const string QUEUE = @".\Private$\MsmqQueue";

      protected override void Initialize(IServiceProvider provider)
      {
         base.Initialize(provider);
         CustomerToSend = new Customer();
         CustomerToSend.Id = 1;
         CustomerToSend.Name = "Tomas Restrepo";

         LabelToSend = Guid.NewGuid().ToString();
      }


      private void MessageReceived_ExecuteCode(object sender, EventArgs e)
      {
         if ( CustomerReceived != null )
         {
            Console.WriteLine("Message Label: {0}", LabelReceived);
            Console.WriteLine("Message Received: {0}", CustomerReceived.Name);
         } else
         {
            Console.WriteLine("Message was not received in the specified time");
         }
      }

	}

}
