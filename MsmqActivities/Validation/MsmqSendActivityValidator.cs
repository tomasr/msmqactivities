
//
// MsmqSendActivityValidator.cs
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
   using Properties;

   /// <summary>
   /// Validator for the MsmqSendActivityValidator
   /// </summary>
   public class MsmqSendActivityValidator : ActivityValidator
   {
      public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
      {
         ValidationErrorCollection errors = base.Validate(manager, obj);

         MsmqSendActivity act = (MsmqSendActivity)obj;

         if ( !act.IsBindingSet(MsmqSendActivity.QueueProperty) )
         {
            if ( act.GetValue(MsmqSendActivity.QueueProperty) == null )
            {
               errors.Add(ValidationError.GetNotSetValidationError("Queue"));
            }
         }
         if ( !act.IsBindingSet(MsmqSendActivity.MessageToSendProperty) )
         {
            if ( act.GetValue(MsmqSendActivity.MessageToSendProperty) == null )
            {
               errors.Add(ValidationError.GetNotSetValidationError("MessageToSend"));
            }
         }

         // if the queue is transactional, one of our parents
         // must be a TransactionScope
         if ( act.IsTransactionalQueue )
         {
            bool isInTransaction = false;
            Activity parent = act.Parent;
            
            while ( parent != null )
            {
               if ( parent is TransactionScopeActivity )
               {
                  isInTransaction = true;
                  break;
               }
               parent = parent.Parent;
            }
            
            if ( !isInTransaction )
            {
               ValidationError error = 
                  new ValidationError(Resources.Send_MustBeInTransaction, 9001);
               errors.Add(error);
            }
         }

         return errors;
      }

   } // class MsmqReceiveActivityValidator

} // namespace Winterdom.Workflow.Activities.Msmq.Design
