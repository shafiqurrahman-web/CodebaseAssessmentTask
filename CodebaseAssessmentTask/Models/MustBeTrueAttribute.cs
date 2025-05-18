using System.ComponentModel.DataAnnotations;

namespace CodebaseAssessmentTask.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple =false)]
    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
           return value is bool boolValue && boolValue;

            //if (value is bool b)
            //    return b;
            //return false;


        }        
    }
} 