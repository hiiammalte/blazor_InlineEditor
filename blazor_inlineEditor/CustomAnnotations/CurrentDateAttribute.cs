using System;
using System.ComponentModel.DataAnnotations;

namespace blazor_inlineEditor.CustomAnnotations
{
    public class LessThanCurrentDateAttribute : ValidationAttribute
    {
        public LessThanCurrentDateAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            var dt = (DateTime)value;
            if (dt < DateTime.Now)
            {
                return true;
            }
            return false;
        }
    }
}
