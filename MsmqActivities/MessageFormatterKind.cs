
//
// MessageFormatterKind.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//

using System;

namespace Winterdom.Workflow.Activities.Msmq
{
   /// <summary>
   /// Kind of MSMQ Formatter to use
   /// </summary>
   public enum MessageFormatterKind
   {
      XmlFormatter,
      BinaryFormatter,
      ActiveXFormatter,
      Raw,
   } // enum MessageFormatterKind

} // namespace Winterdom.Workflow.Activities.Msmq

