using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.Models;
using PagedList;

namespace LibraryManagementSystem.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        [Authorize]
        public ActionResult Index(int? page)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var list = db.Books.Where(x => x.isStatus == true).OrderBy(x => x.Title).Select(x => new BookModel
            {
                Id = x.Id,
                Title = x.Title,
                Descriptions = x.Descriptions,
                Pages = x.Pages,
                authorName = x.Author.Name
            }).ToPagedList(pageNumber, pageSize);
            ViewBag.bookList = list;
            ViewBag.numPage = pageNumber;
            ViewBag.sizePage = pageSize;
            return View(list);
        }
        [Authorize]
        public ActionResult addEdit(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            List<Publisher> publisher = db.Publishers.Where(x => x.isStatus == true).ToList();
            ViewBag.publisher = new SelectList(publisher, "Id", "Name");
            List<Location> location = db.Locations.Where(x => x.isStatus == true).ToList();
            ViewBag.location = new SelectList(location, "Id", "Name");
            List<Shelf> shelf = db.Shelves.Where(x => x.isStatus == true).ToList();
            ViewBag.shelf = new SelectList(shelf, "Id", "Name");
            List<Language> lan = db.Languages.Where(x => x.isStatus == true).ToList();
            ViewBag.lan = new SelectList(lan, "Id", "Name");
            List<Category> cate = db.Categories.Where(x => x.isStatus == true).ToList();
            ViewBag.cate = new SelectList(cate, "Id", "Name");
            List<Author> author = db.Authors.Where(x => x.isStatus == true).ToList();
            ViewBag.author = new SelectList(author, "Id", "Name");

            Book d = db.Books.FirstOrDefault(x => x.Id == id);
            BookModel book = new BookModel();
            if (id > 0)
            {
                book.Id = d.Id;
                book.Id_pub = d.Id_pub;
                book.Id_loc = d.Id_loc;
                book.Id_shelf = d.Id_shelf;
                book.Id_lan = d.Id_lan;
                book.Id_cate = d.Id_cate;
                book.Id_author = d.Id_author;
                book.Title = d.Title;
                book.Descriptions = d.Descriptions;
                book.Pages = d.Pages;
                book.keyword = d.keyword;
                book.Cost = d.Cost;
                book.images = d.images;
                book.Isbn = d.Isbn;
                book.Note = d.Note;
            }
            return PartialView("_ContentAddEditBook", book);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(BookModel dep)
        {
           
            FinalProjectEntities db = new FinalProjectEntities();

            //update
            if (dep.Id > 0)
            {

                Book d = db.Books.FirstOrDefault(x => x.Id == dep.Id);
                if (ModelState.IsValid)
                {
                    var model = db.Books.Find(dep.Id);
                    string oldFilePath = model.images;
                    if (dep.ImageFile != null && dep.ImageFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(dep.ImageFile.FileName);
                        string extention = Path.GetExtension(dep.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssff") + extention;
                        d.images = "/Image/" + fileName;
                        db.SaveChanges();
                        fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                        dep.ImageFile.SaveAs(fileName);
                        string fullPath = Request.MapPath(oldFilePath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                }

                d.Id_pub = dep.Id_pub;
                d.Id_loc = dep.Id_loc;
                d.Id_shelf = dep.Id_shelf;
                d.Id_lan = dep.Id_lan;
                d.Id_cate = dep.Id_cate;
                d.Id_author = dep.Id_author;
                d.Title = dep.Title;
                d.Descriptions = dep.Descriptions;
                d.Pages = dep.Pages;
                d.keyword = dep.keyword;
                d.Cost = dep.Cost;
                d.Isbn = dep.Isbn;
                d.Note = dep.Note;
                db.SaveChanges();
                
            }
            else //add
            {
                
                    Book d = new Book();
                    string fileName = Path.GetFileNameWithoutExtension(dep.ImageFile.FileName);
                    string extention = Path.GetExtension(dep.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssff") + extention;
                    d.images = "/Image/" + fileName;

                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                    dep.ImageFile.SaveAs(fileName);

                    d.Id_pub = dep.Id_pub;
                    d.Id_loc = dep.Id_loc;
                    d.Id_shelf = dep.Id_shelf;
                    d.Id_lan = dep.Id_lan;
                    d.Id_cate = dep.Id_cate;
                    d.Id_author = dep.Id_author;
                    d.Title = dep.Title;
                    d.Descriptions = dep.Descriptions;
                    d.Pages = dep.Pages;
                    d.keyword = dep.keyword;
                    d.Cost = dep.Cost;
                    d.Isbn = dep.Isbn;
                    d.Note = dep.Note;
                    d.isStatus = true;
                    db.Books.Add(d);
                    db.SaveChanges();
                   
                    
                
            }
            return RedirectToAction("Index");

        }
        [Authorize]
        public JsonResult DeleteBook(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            Book d = db.Books.FirstOrDefault(x => x.Id == id);
            if (d != null)
            {
                string fullPath = Request.MapPath(d.images);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                d.isStatus = false;
                db.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult getSearch(string text)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var list = db.Books.
                Where(x => x.isStatus == true
                && (x.Title.Contains(text)
                || x.Location.Name.Contains(text)
                || x.Shelf.Name.Contains(text)
                || x.Author.Name.Contains(text)
                || x.Publisher.Name.Contains(text))).Select(x => new BookModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Descriptions = x.Descriptions,
                    Pages = x.Pages,
                    authorName = x.Author.Name
                }).ToList();
            return PartialView("_contentSearchBook", list);
        }
        [Authorize]
        public JsonResult checkTitle(string text)
        {
            bool check = true;
            FinalProjectEntities db = new FinalProjectEntities();
            var c = db.Books.Where(x => x.Title == text && x.isStatus==true).FirstOrDefault();
            if (c != null && text != null)
            {
                check = true;
            }
            else
            {
                check = false;
            }
            return Json(check,JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult checkIsbn(string text)
        {
            bool check = true;
            FinalProjectEntities db = new FinalProjectEntities();
            var c = db.Books.Where(x => x.Isbn == text && x.isStatus == true).FirstOrDefault();
            if (c != null && text != null)
            {
                check = true;
            }
            else
            {
                check = false;
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }
    }
}