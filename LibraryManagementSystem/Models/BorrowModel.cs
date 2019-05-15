using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryManagementSystem.Models
{
    public class BorrowModel
    {
        public int Id_bor { get; set; }
        public Nullable<int> Id_User { get; set; }
        public string userName { get; set; }
        public Nullable<int> Id_Book { get; set; }
        public string bookName { get; set; }
        public string Isbn { get; set; }
        public Nullable<System.DateTime> Since { get; set; }
        public Nullable<System.DateTime> Until { get; set; }
        public string Note { get; set; }
        public Nullable<bool> isStatus { get; set; }
    }
}