//
// MsmqActivityDesignerTheme.cs
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
   /// <summary>
   /// DesignerTheme class for the MSMQ Receive Activity,
   /// which selects the colors to use with them
   /// when painting them in the designer
   /// </summary>
   public class MsmqReceiveActivityDesignerTheme : ActivityDesignerTheme
   {
      public MsmqReceiveActivityDesignerTheme(WorkflowTheme theme)
         : base(theme)
      {
      }
      public override void Initialize()
      {
         base.Initialize();

         BorderColor = Color.DarkGreen;
         BorderStyle = DashStyle.Solid;
         BackColorStart = Color.Honeydew;
         BackColorEnd = Color.DarkSeaGreen;
         BackgroundStyle = LinearGradientMode.Vertical;
      }

   } // class MsmqReceiveActivityDesignerTheme


   /// <summary>
   /// DesignerTheme class for the MSMQ Send Activity,
   /// which selects the colors to use with them
   /// when painting them in the designer
   /// </summary>
   public class MsmqSendActivityDesignerTheme : ActivityDesignerTheme
   {
      public MsmqSendActivityDesignerTheme(WorkflowTheme theme)
         : base(theme)
      {
      }
      public override void Initialize()
      {
         base.Initialize();

         BorderColor = Color.FromArgb(142, 145, 64);
         BorderStyle = DashStyle.Solid;
         BackColorStart = Color.Ivory;
         BackColorEnd = Color.FromArgb(200, 197, 128);
         BackgroundStyle = LinearGradientMode.Vertical;
      }

   } // class MsmqSendActivityDesignerTheme

} // namespace Winterdom.Workflow.Activities.Msmq.Design
