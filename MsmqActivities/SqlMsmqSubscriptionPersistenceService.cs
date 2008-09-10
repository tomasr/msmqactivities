
//
// SqlMsmqSubscriptionPersistenceService.cs
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
   /// Implements the subscription service on top
   /// of SqlServer
   /// </summary>
   public class SqlMsmqSubscriptionPersistenceService
      : IMsmqSubscriptionPersistenceService
   {
      private string _connectionString;
      BinaryFormatter _formatter = new BinaryFormatter();


      /// <summary>
      /// Creates a new instance
      /// </summary>
      /// <param name="connString">Connection String to the store db</param>
      public SqlMsmqSubscriptionPersistenceService(string connString)
      {
         if ( String.IsNullOrEmpty(connString) )
            throw new ArgumentNullException("connString");

         _connectionString = connString;
      }

      /// <summary>
      /// Creates a new instance based on a set of parameters
      /// </summary>
      /// <param name="parameters">Parameters</param>
      public SqlMsmqSubscriptionPersistenceService(NameValueCollection parameters)
      {
         if ( parameters == null )
            throw new ArgumentNullException("parameters");

         _connectionString = parameters["ConnectionString"];
         if ( String.IsNullOrEmpty(_connectionString) )
            throw new InvalidOperationException("no connection string specified");
      }

      #region IMsmqSubscriptionPersistenceService Implementation
      // 
      // IMsmqSubscriptionPersistenceService Implementation
      //

      public void Persist(string host, MsmqSubscription subscription)
      {
         byte[] wfQueueName = Serialize(subscription.WfQueueName);

         SqlConnection connection = new SqlConnection(_connectionString);
         using ( connection )
         {
            SqlCommand cmd = new SqlCommand("InsertMsmqSubscription");
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@subscriptionId", subscription.ID);
            cmd.Parameters.AddWithValue("@host", host);
            cmd.Parameters.AddWithValue("@workflowInstance", 
               subscription.WorkflowInstance);
            cmd.Parameters.AddWithValue("@msmqQueue", subscription.MsmqQueue);
            cmd.Parameters.AddWithValue("@wfQueueName", wfQueueName);

            connection.Open();
            cmd.ExecuteNonQuery();
         }
      }

      public IEnumerable<MsmqSubscription> LoadAll(string host)
      {
         SqlConnection connection = new SqlConnection(_connectionString);
         using ( connection )
         {
            SqlCommand cmd = new SqlCommand("GetAllMsmqSubscriptions");
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@host", host);

            connection.Open();
            using ( SqlDataReader reader = cmd.ExecuteReader() )
            {
               //List<MsmqSubscription> list = new List<MsmqSubscription>();
               while ( reader.Read() )
               {
                  MsmqSubscription subs = new MsmqSubscription();
                  subs.ID = reader.GetGuid(0);
                  subs.WorkflowInstance = reader.GetGuid(2);
                  subs.MsmqQueue = reader.GetString(3);
                  subs.WfQueueName = Deserialize(reader.GetSqlBytes(4));

                  //list.Add(subs);
                  yield return subs;
               }
               //return list;
            }
         }
      }

      public void Remove(MsmqSubscription subscription)
      {
         SqlConnection connection = new SqlConnection(_connectionString);
         using ( connection )
         {
            SqlCommand cmd = new SqlCommand("RemoveMsmqSubscription");
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@subscriptionId", subscription.ID);

            connection.Open();
            cmd.ExecuteNonQuery();
         }
      }

      #endregion // IMsmqSubscriptionPersistenceService Implementation


      #region Private Methods
      //
      // Private Methods
      //

      private byte[] Serialize(object obj)
      {
         using ( MemoryStream stream = new MemoryStream() )
         {
            _formatter.Serialize(stream, obj);
            return stream.ToArray();
         }
      }

      private IComparable Deserialize(SqlBytes bytes)
      {
         using ( MemoryStream stream = new MemoryStream(bytes.Buffer) )
         {
            return (IComparable)_formatter.Deserialize(stream);
         }
      }
      #endregion // Private Methods

   } // class SqlMsmqSubscriptionPersistenceService

} // namespace Winterdom.Workflow.Activities.Msmq
