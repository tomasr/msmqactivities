
//
// IMsmqSubscriptionPersistenceService.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Winterdom.Workflow.Activities.Msmq
{
   /// <summary>
   /// Interface representing the service used to
   /// persist subscription information
   /// for the MsmqListenerService
   /// </summary>
   public interface IMsmqSubscriptionPersistenceService
   {
      /// <summary>
      /// Persist a new subscription to the repository
      /// </summary>
      /// <param name="host">Name of Host this subscription belongs to</param>
      /// <param name="subscription">Subcription to persist</param>
      void Persist(string host, MsmqSubscription subscription);

      /// <summary>
      /// Load all subscriptions from the repository
      /// </summary>
      /// <param name="host">Name of host to load subscriptions for</param>
      /// <returns>A list of available subscriptions</returns>
      IEnumerable<MsmqSubscription> LoadAll(string host);

      /// <summary>
      /// Remove an existing suscription from the repository
      /// </summary>
      /// <param name="subscription">Subscription to remove</param>
      void Remove(MsmqSubscription subscription);

   } // interface IMsmqSubscriptionPersistenceService

} // namespace Winterdom.Workflow.Activities.Msmq
