
//
// MsmqSubscription.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//


using System;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using System.Text;

using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;

namespace Winterdom.Workflow.Activities.Msmq
{

   /// <summary>
   /// Represents a subscription from
   /// an activity instance to an actual 
   /// MSMQ Queue
   /// </summary>
   [Serializable]
   public class MsmqSubscription
   {
      private Guid _id = Guid.NewGuid();
      private IComparable _wfQueueName;
      private string _msmqQueue;
      private Guid _workflowInstance;

      public Guid ID
      {
         get { return _id; }
         set { _id = value; }
      }

      public IComparable WfQueueName
      {
         get { return _wfQueueName; }
         set { _wfQueueName = value; }
      }

      public string MsmqQueue
      {
         get { return _msmqQueue; }
         set { _msmqQueue = value; }
      }

      public Guid WorkflowInstance
      {
         get { return _workflowInstance; }
         set { _workflowInstance = value; }
      }

   } // class MsmqSubscription

} // namespace Winterdom.Workflow.Activities.Msmq
