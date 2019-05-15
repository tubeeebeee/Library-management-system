//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Borrow
    {
        public int Id_bor { get; set; }
        public Nullable<int> Id_User { get; set; }
        public Nullable<int> Id_Book { get; set; }
        public string Isbn { get; set; }
        public Nullable<System.DateTime> Since { get; set; }
        public Nullable<System.DateTime> Until { get; set; }
        public string Note { get; set; }
        public Nullable<bool> isStatus { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual UserSystem UserSystem { get; set; }
    }
}