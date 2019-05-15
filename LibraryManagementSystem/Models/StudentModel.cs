using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryManagementSystem.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Phone { get; set; }
        
        public string Email { get; set; }
        public string Addr { get; set; }
        public Nullable<decimal> Fee { get; set; }
        public string Note { get; set; }
        public Nullable<bool> isStatus { get; set; }
    }
}