using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryManagementSystem.Models
{
    public class FeeModel
    {
        public string name { get; set; }
        public string number { get; set; }
        public decimal? fee { get; set; }
        public string note { get; set; }
    }
}