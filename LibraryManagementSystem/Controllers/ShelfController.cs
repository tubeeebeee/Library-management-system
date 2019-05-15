using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.Models;
using PagedList;

namespace LibraryManagementSystem.Controllers
{
    public class ShelfController : Controller
    {
        // GET: Shelf
        [Authorize]
        public ActionResult Index(int? page)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var pageNumber = page ?? 1;
            var pageSize = 5;
            var list = db.Shelves.OrderBy(x=>x.Name).Where(x => x.isStatus == true).Select(x => new ShelfModel
            {
                Id = x.Id,
                Name = x.Name,
                Note = x.Note
            }).ToPagedList(pageNumber, pageSize);
            ViewBag.ShelfList = list;
            ViewBag.numPage = pageNumber;
            ViewBag.sizePage = pageSize;
            return View(list);
        }
        [Authorize]
        public ActionResult addEdit(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            ShelfModel shelf = new ShelfModel();
            if (id > 0)
            {
                Shelf u = db.Shelves.FirstOrDefault(x => x.Id == id);
                shelf.Id = u.Id;
                shelf.Name = u.Name;
                
                shelf.Note = u.Note;

            }
            return PartialView("_contentAddEditShelf", shelf);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(Shelf model)
        {
            bool check;
            FinalProjectEntities db = new FinalProjectEntities();
            try
            {
                if (model.Id > 0)
                {
                    Shelf u = db.Shelves.FirstOrDefault(x => x.Id == model.Id);
                    u.Name = model.Name;
                    u.Note = model.Note;
                    db.SaveChanges();
                    check = true;
                    return Json(check, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (!checkName(model.Name))
                    {
                        Shelf u = new Shelf();
                        u.Name = model.Name;

                        u.Note = model.Note;

                        u.isStatus = true;
                        db.Shelves.Add(u);
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
        public JsonResult DeleteShelf(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var shelf = db.Shelves.FirstOrDefault(x => x.Id == id);
            if (shelf != null)
            {
                shelf.isStatus = false;
                db.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public bool checkName(string text)
        {
            var check = false;
            FinalProjectEntities db = new FinalProjectEntities();
            var shelf = db.Shelves.Where(x => x.isStatus==true && x.Name == text).FirstOrDefault();
            if(shelf!=null)
            {
                check = true;
            }
            return check;
        }
    }
}