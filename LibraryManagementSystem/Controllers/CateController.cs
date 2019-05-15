using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class CateController : Controller
    {
        // GET: Cate
        [Authorize]
        public ActionResult Index(int? page)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var pageNumber = page ?? 1;
            var pageSize = 5;
            var list = db.Categories.OrderBy(x => x.Name).Where(x => x.isStatus == true).Select(x => new CateModel
            {
                Id = x.Id,
                Name = x.Name,
                Note = x.Note
            }).ToPagedList(pageNumber, pageSize);
            ViewBag.CateList = list;
            ViewBag.numPage = pageNumber;
            ViewBag.sizePage = pageSize;
            return View(list);
        }
        [Authorize]
        public ActionResult addEdit(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            CateModel cate = new CateModel();
            if (id > 0)
            {
                Category u = db.Categories.FirstOrDefault(x => x.Id == id);
                cate.Id = u.Id;
                cate.Name = u.Name;

                cate.Note = u.Note;

            }
            return PartialView("_contentAddEditCate", cate);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(CateModel model)
        {
            bool check;
            FinalProjectEntities db = new FinalProjectEntities();
            try
            {
                if (model.Id > 0)
                {
                    Category u = db.Categories.FirstOrDefault(x => x.Id == model.Id);
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
                        Category u = new Category();
                        u.Name = model.Name;

                        u.Note = model.Note;

                        u.isStatus = true;
                        db.Categories.Add(u);
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
        public JsonResult DeleteCate(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var cate = db.Categories.FirstOrDefault(x => x.Id == id);
            if (cate != null)
            {
                cate.isStatus = false;
                db.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public bool checkName(string text)
        {
            var check = false;
            FinalProjectEntities db = new FinalProjectEntities();
            var cate = db.Categories.Where(x => x.isStatus == true && x.Name == text).FirstOrDefault();
            if (cate != null)
            {
                check = true;
            }
            return check;
        }
    }
}