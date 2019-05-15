using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryManagementSystem.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="eror!")]
        public Nullable<int> Id_pub { get; set; }
        public Nullable<int> Id_loc { get; set; }
        public Nullable<int> Id_shelf { get; set; }
        public Nullable<int> Id_lan { get; set; }
        public Nullable<int> Id_cate { get; set; }
        public Nullable<int> Id_author { get; set; }
        public string authorName { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public Nullable<int> Pages { get; set; }
        public string keyword { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string images { get; set; }
        public string Isbn { get; set; }
        public string Note { get; set; }
        public Nullable<bool> isStatus { get; set; }
        public string pubName { get; set; }
        public string locName { get; set; }
        public string shelfName { get; set; }
        public string lanName { get; set; }
        public string cateName { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}