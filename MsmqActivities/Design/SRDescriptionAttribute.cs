
//
// SRDescription.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//

using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Winterdom.Workflow.Activities.Msmq.Design
{
   using Properties;

   internal class SRDescriptionAttribute : DescriptionAttribute
   {
      public SRDescriptionAttribute(string resource)
         : base(Resources.ResourceManager.GetString(resource))
      {
      }
   } // class SRDescriptionAttribute

} // namespace Winterdom.Workflow.Activities.Msmq.Design
