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
	partial class Workflow1
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
         this.msmqReceiveActivity1 = new Winterdom.Workflow.Activities.Msmq.MsmqReceiveActivity();
         this.terminateActivity1 = new System.Workflow.ComponentModel.TerminateActivity();
         activitybind1.Name = "Workflow1";
         activitybind1.Path = "Message";
         // 
         // msmqReceiveActivity1
         // 
         this.msmqReceiveActivity1.Name = "msmqReceiveActivity1";
         this.msmqReceiveActivity1.Queue = "\\\\.\\Private$\\QUeue1";
         this.msmqReceiveActivity1.SetBinding(Winterdom.Workflow.Activities.Msmq.MsmqBaseReceiveActivity.MessageReceivedProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
         // 
         // terminateActivity1
         // 
         this.terminateActivity1.Name = "terminateActivity1";
         // 
         // Workflow1
         // 
         this.Activities.Add(this.terminateActivity1);
         this.Activities.Add(this.msmqReceiveActivity1);
         this.Name = "Workflow1";
         this.CanModifyActivities = false;

		}

		#endregion

      private Winterdom.Workflow.Activities.Msmq.MsmqReceiveActivity msmqReceiveActivity1;
      private TerminateActivity terminateActivity1;

   }
}
