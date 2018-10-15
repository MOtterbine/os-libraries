using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace OS.Web.Validation
{
    public class EnsureTrueAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (value.GetType() != typeof(bool)) throw new InvalidOperationException("can only be used on boolean properties.");

            return (bool)value == true;
        }


    }

}