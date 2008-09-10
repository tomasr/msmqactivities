//
// MsmqReceiveActivityDesigner.cs
//
// Author:
//    Tomas Restrepo (tomas@winterdom.com)
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
   /// Activity designer for the MsmqReceive activity.
   /// </summary>
   /// <remarks>
   /// Through this designer we acomplish several goals:
   /// <list type="bullet">
   ///   <item><description>We synthetize the MessageReceived property in the designer 
   ///   so that it only allows you to bind it to external properties that match
   ///   the type selected in the MessageType property</description></item>
   ///   <item><description>We reset the MessageReceived property when
   ///   the MessageType property is changed.</description></item>
   /// </list>
   /// </remarks>
   [ActivityDesignerTheme(typeof(MsmqReceiveActivityDesignerTheme))]
   public class MsmqReceiveActivityDesigner : MsmqActivityDesigner
   {
      protected override void OnActivityChanged(ActivityChangedEventArgs e)
      {
         base.OnActivityChanged(e);
         if ( e.Member.Name == "MessageType" )
         {
            ((MsmqBaseReceiveActivity)e.Activity).SetValue(MsmqBaseReceiveActivity.MessageReceivedProperty, null);
         }
      }

      /// <summary>
      /// Prefilter the set of properties to show in the property grid for
      /// the activity
      /// </summary>
      /// <param name="properties">Property collection</param>
      /// <remarks>
      /// In here, we create a new pseudo-property (MessageReceived)
      /// that the designer allows you to bind to an external activity property.
      /// 
      /// <para>To do this, we create a new PropertyDescriptor for the property
      /// that is related to the type selected in the activity MessageType property.</para>
      /// </remarks>
      protected override void PreFilterProperties(IDictionary properties)
      {
         base.PreFilterProperties(properties);

         MsmqBaseReceiveActivity act = (MsmqBaseReceiveActivity)Activity;
         Type type = act.MessageType;
         if ( type != null )
         {
            PropertyDescriptor desc = CreatePropertyDescriptor(act);
            properties["MessageReceived"] = desc;
         }
      }

      private PropertyDescriptor CreatePropertyDescriptor(MsmqBaseReceiveActivity act)
      {
         return new TypedPropertyDescriptor(act);
      }

   } // class MsmqReceiveActivityDesigner

} // namespace Winterdom.Workflow.Activities.Msmq.Design
