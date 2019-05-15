using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryManagementSystem.Models
{
    public class ShelfModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Nullable<bool> isStatus { get; set; }
    }
}