using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Warehouse.Models
{
    [DataContract]
    public class RecaptchaResult
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}