
//
// NullMsmqSubscriptionPersistenceService.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//

using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Winterdom.Workflow.Activities.Msmq
{
   /// <summary>
   /// Implements the subscription service without
   /// actually persisting anything. Useful for
   /// cases where you don't want/need subscription
   /// persistence.
   /// </summary>
   public class NullMsmqSubscriptionPersistenceService
      : IMsmqSubscriptionPersistenceService
   {

      #region IMsmqSubscriptionPersistenceService Implementation
      // 
      // IMsmqSubscriptionPersistenceService Implementation
      //

      public void Persist(string host, MsmqSubscription subscription)
      {
         // do nothing
      }

      public IEnumerable<MsmqSubscription> LoadAll(string host)
      {
         // return an empty list
         List<MsmqSubscription> list = new List<MsmqSubscription>();
         return list;
      }

      public void Remove(MsmqSubscription subscription)
      {
         // do nothing
      }

      #endregion // IMsmqSubscriptionPersistenceService Implementation

   } // class NullMsmqSubscriptionPersistenceService

} // namespace Winterdom.Workflow.Activities.Msmq
