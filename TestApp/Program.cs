

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using TestApp.Properties;

using Winterdom.Workflow.Activities.Msmq;
namespace TestApp
{
   class Program
   {
      static void Main(string[] args)
      {
         AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
         try
         {
            WorkflowRuntime workflowRuntime = new WorkflowRuntime("ConsoleApplication");
            workflowRuntime.StartRuntime();

            AutoResetEvent waitHandle = new AutoResetEvent(false);
            workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
            workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
            {
               Console.WriteLine(e.Exception.Message);
               waitHandle.Set();
            };

            WorkflowInstance instance = workflowRuntime.CreateWorkflow(typeof(TestApp.Workflow1));
            instance.Start();
            
/*
            WorkflowInstance instance = workflowRuntime.CreateWorkflow(typeof(TestApp.DehydrationTest));
            instance.Start();
            //Thread.Sleep(10000);
            //Console.WriteLine("Unloading...");
            //instance.Unload();

            //WorkflowInstance instance = workflowRuntime.GetWorkflow(new Guid("F98F2DF6-7C05-4A20-9AEA-DBD2243881B4"));
            //instance.Load();
*/ 
            waitHandle.WaitOne();
            return;
         } catch ( Exception ex )
         {
            Console.WriteLine("Exception Occurred:");
            Console.WriteLine(ex);
         }
      }

      static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
      {
         Console.WriteLine("Exception Occurred:");
         Console.WriteLine(e.ExceptionObject);
      }
   }
}
