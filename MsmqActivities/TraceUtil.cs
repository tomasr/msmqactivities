
//
// TraceUtil.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//


using System;
using System.Diagnostics;
using System.IO;
using System.Text;


namespace Winterdom.Workflow.Activities.Msmq
{

   /// <summary>
   /// Helper trace class
   /// </summary>
   internal static class TraceUtil
   {
      const string PREFIX = "MsmqActivities: ";
      static TraceSwitch _traceSwitch = new TraceSwitch("MsmqActivitiesSwitch", "");

      public static void WriteInfo(string format, params object[] args)
      {
         if ( _traceSwitch.TraceInfo )
         {
            Trace.WriteLine(string.Format(PREFIX + format, args));
         }
      }

      public static void WriteException(Exception ex)
      {
         if ( _traceSwitch.TraceError )
         {
            Trace.WriteLine(string.Format(PREFIX + "Exception: {0}", ex));
         }
      }
   } // class TraceUtil

} // namespace Winterdom.Workflow.Activities.Msmq
