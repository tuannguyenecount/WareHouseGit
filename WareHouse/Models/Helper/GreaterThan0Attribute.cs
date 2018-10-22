using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WareHouse.Models
{
    public class GreaterThan0 : ValidationAttribute
    {
       
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            return (decimal)value > 0;
        }
       
    }
}