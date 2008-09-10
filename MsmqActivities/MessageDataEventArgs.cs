
//
// MessageData.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//


using System;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using System.Text;

namespace Winterdom.Workflow.Activities.Msmq
{

   /// <summary>
   /// Presents a serializable copy
   /// of the data in an MSMQ Message 
   /// received
   /// </summary>
   /// <remarks>
   /// We need to copy the data from the original
   /// Message object because we need to ensure we 
   /// can be serialized while in the Workflow queues
   /// </remarks>
   [Serializable]
   internal class MessageDataEventArgs : EventArgs
   {
      // todo: add other message properties
      private string _label;
      private int _bodyType;
      private MemoryStream _messageStream;

      public string Label
      {
         get { return _label; }
      }

      public MemoryStream MessageStream
      {
         get { return _messageStream; }
      }

      public int BodyType
      {
         get { return _bodyType; }
      }

      public MessageDataEventArgs(Message msg)
      {
         _label = msg.Label;
         _bodyType = msg.BodyType;

         byte[] buffer = new byte[(int)msg.BodyStream.Length];
         int size = msg.BodyStream.Read(buffer, 0, buffer.Length);
         _messageStream = new MemoryStream(buffer, 0, size);
         _messageStream.Position = 0;
      }

   } // class MessageData

} // namespace Winterdom.Workflow.Activities.Msmq
