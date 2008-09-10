
//
// MsmqActivityDesigner.cs
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
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;

namespace Winterdom.Workflow.Activities.Msmq.Design
{
   using Properties;

   /// <summary>
   /// Base designer class for the MSMQ
   /// activities
   /// </summary>
   public class MsmqActivityDesigner : ActivityDesigner
   {
      protected override void Initialize(Activity activity)
      {
         base.Initialize(activity);

         Bitmap image = Resources.MessageQueuing; 
         image.MakeTransparent();
         this.Image = image;

      }

   } // class MsmqActivityDesigner


} // namespace Winterdom.Workflow.Activities.Msmq.Design
