using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class AuController : Controller
    {
        // GET: Au
        [Authorize]
        public ActionResult Index(int? page)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var pageNumber = page ?? 1;
            var pageSize = 5;
            var list = db.Authors.OrderBy(x => x.Name).Where(x => x.isStatus == true).Select(x => new AuthorModel
            {
                Id = x.Id,
                Name = x.Name,
                Note = x.Note,
                Addr = x.Addr
            }).ToPagedList(pageNumber, pageSize);
            ViewBag.AuList = list;
            ViewBag.numPage = pageNumber;
            ViewBag.sizePage = pageSize;
            return View(list);
        }
        [Authorize]
        public ActionResult addEdit(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            AuthorModel author = new AuthorModel();
            if (id > 0)
            {
                Author u = db.Authors.FirstOrDefault(x => x.Id == id);
                author.Id = u.Id;
                author.Name = u.Name;
                author.Addr = u.Addr;
                author.Note = u.Note;

            }
            return PartialView("_contentAddEditAu", author);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(AuthorModel model)
        {
            bool check;
            FinalProjectEntities db = new FinalProjectEntities();
            try
            {
                if (model.Id > 0)
                {
                    Author u = db.Authors.FirstOrDefault(x => x.Id == model.Id);
                    u.Name = model.Name;
                    u.Addr = model.Addr;
                    db.SaveChanges();
                    check = true;
                    return Json(check, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (!checkName(model.Name))
                    {
                        Author u = new Author();
                        u.Name = model.Name;

                        u.Addr = model.Addr;

                        u.isStatus = true;
                        db.Authors.Add(u);
                        db.SaveChanges();
                        check = true;
                        return Json(check, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        check = false;
                        return Json(check, JsonRequestBehavior.AllowGet);
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [Authorize]
        public JsonResult DeleteAu(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var author = db.Authors.FirstOrDefault(x => x.Id == id);
            if (author != null)
            {
                author.isStatus = false;
                db.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public bool checkName(string text)
        {
            var check = false;
            FinalProjectEntities db = new FinalProjectEntities();
            var author = db.Authors.Where(x => x.isStatus == true && x.Name == text).FirstOrDefault();
            if (author != null)
            {
                check = true;
            }
            return check;
        }
    }
}