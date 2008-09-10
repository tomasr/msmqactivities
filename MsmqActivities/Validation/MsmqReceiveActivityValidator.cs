
//
// MsmqReceiveActivityValidator.cs
//
// Author:
//    Tomas Restrepo (tomasr@mvps.org)
//

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Messaging;
using System.IO;
using System.Text;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;

namespace Winterdom.Workflow.Activities.Msmq.Validation
{
   
   /// <summary>
   /// Validator class for the MsmqReceiveActivity
   /// </summary>
   public class MsmqReceiveActivityValidator : ActivityValidator
   {
      /// <summary>
      /// Validate the receive activity
      /// </summary>
      /// <param name="manager">Validation Manager to use</param>
      /// <param name="obj">Activity instance to validate</param>
      /// <returns>Collection of validation errors</returns>
      public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
      {
         ValidationErrorCollection errors = base.Validate(manager, obj);
         MsmqBaseReceiveActivity act = (MsmqBaseReceiveActivity)obj;

         if ( !act.IsBindingSet(MsmqBaseReceiveActivity.QueueProperty) )
         {
            if ( act.GetValue(MsmqBaseReceiveActivity.QueueProperty) == null )
            {
               errors.Add(ValidationError.GetNotSetValidationError("Queue"));
            }
         }

         if ( act.MessageType == null )
         {
            errors.Add(ValidationError.GetNotSetValidationError("MessageType"));
         }

         if ( !act.IsBindingSet(MsmqBaseReceiveActivity.MessageReceivedProperty) )
         {
            errors.Add(ValidationError.GetNotSetValidationError("MessageReceived"));
         }
         return errors;
      }

   } // class MsmqReceiveActivityValidator

} // namespace Winterdom.Workflow.Activities.Msmq.Design
