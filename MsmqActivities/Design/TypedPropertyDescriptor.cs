//
// TypedPropertyDescriptor.cs
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
   /// PropertyDescriptor derived class used to synthetize the 
   /// MessageReceived property. 
   /// </summary>
   internal class TypedPropertyDescriptor : PropertyDescriptor
   {
      private MsmqBaseReceiveActivity _activity;

      /// <summary>
      /// Type of the component that defines the property
      /// </summary>
      public override Type ComponentType
      {
         get { return _activity.GetType(); }
      }
      /// <summary>
      /// Is the property read-only? no.
      /// </summary>
      public override bool IsReadOnly
      {
         get { return false; }
      }

      /// <summary>
      /// Description of the Property
      /// </summary>
      public override string Description
      {
         get { return Resources.Recv_MessageReceived; }
      }

      /// <summary>
      /// Type of the property. We return the type selected
      /// in Activity.MessageType, or else, typeof(ActivityBind) to allow
      /// binding.
      /// </summary>
      public override Type PropertyType
      {
         get { return _activity.MessageType != null ? _activity.MessageType : typeof(ActivityBind); }
      }
      /// <summary>
      /// Allow an ActivityBindConverter to be returned here
      /// so that we support binding of the property
      /// </summary>
      public override TypeConverter Converter
      {
         get { return new ActivityBindTypeConverter(); }
      }

      /// <summary>
      /// Create a new instance
      /// </summary>
      /// <param name="activity">Instance of the activity</param>
      public TypedPropertyDescriptor(MsmqBaseReceiveActivity activity)
         : base("MessageReceived", new Attribute[] { DesignOnlyAttribute.Yes })
      {
         _activity = activity;
      }

      /// <summary>
      /// Can we reset the value? No.
      /// </summary>
      /// <param name="component">Component</param>
      /// <returns>Always false.</returns>
      public override bool CanResetValue(object component)
      {
         return false;
      }

      /// <summary>
      /// Get the current value of the property
      /// </summary>
      /// <param name="component">Component on which to return the property</param>
      /// <returns>The value of the property</returns>
      /// <remarks>
      /// If the property is bound, we return the associated binding (ActivityBind object).
      /// Else, we return the actual value.
      /// </remarks>
      public override object GetValue(object component)
      {
         if ( _activity.IsBindingSet(MsmqBaseReceiveActivity.MessageReceivedProperty) )
         {
            return _activity.GetBinding(MsmqBaseReceiveActivity.MessageReceivedProperty);
         }
         return _activity.MessageReceived;
      }


      /// <summary>
      /// Reset value (do nothing)
      /// </summary>
      /// <param name="component"></param>
      public override void ResetValue(object component)
      {
      }

      /// <summary>
      /// Set the value of the property
      /// </summary>
      /// <param name="component">Component that defines the property</param>
      /// <param name="value">New value</param>
      /// <remarks>
      /// If the new value is an ActivityBind, we configure the binding to the
      /// external activity property. Else we change the value of the property
      /// directly.
      /// </remarks>
      public override void SetValue(object component, object value)
      {
         ActivityBind abValue = value as ActivityBind;
         if ( abValue != null )
         {
            _activity.SetBinding(MsmqBaseReceiveActivity.MessageReceivedProperty, abValue);
         } else
         {
            _activity.MessageReceived = value;
         }
         OnValueChanged(this, EventArgs.Empty);
      }

      /// <summary>
      /// Should we serialize the value? No.
      /// </summary>
      /// <param name="component"></param>
      /// <returns>Always false</returns>
      public override bool ShouldSerializeValue(object component)
      {
         return false;
      }

   } // class TypedPropertyDescriptor

} // namespace Winterdom.Workflow.Activities.Msmq.Design
