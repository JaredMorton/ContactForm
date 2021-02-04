using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactForm.Data.Models
{
    public class Address
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Description("Postal Code")]
        public string Zip { get; set; }
    }
}