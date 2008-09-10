
//
// MsmqActivityToolboxItem.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;

namespace Winterdom.Workflow.Activities.Msmq.Design
{
   using Properties;

   /// <summary>
   /// Sample ToolboxItem that renames the activity
   /// for the toolbox
   /// </summary>
   [Serializable]
   public class MsmqActivityToolboxItem : ActivityToolboxItem
   {
      public MsmqActivityToolboxItem()
      {
      }

      public MsmqActivityToolboxItem(Type type)
         : base(type)
      {
         const string SUFFIX = "Activity";
         if ( DisplayName.EndsWith(SUFFIX) )
         {
            DisplayName = DisplayName.Substring(0, DisplayName.Length - SUFFIX.Length);
         }
      }

      protected MsmqActivityToolboxItem(SerializationInfo info, StreamingContext context)
      {
         Deserialize(info, context);
      }

   } // class MsmqActivityToolboxItem


} // namespace Winterdom.Workflow.Activities.Msmq.Design
