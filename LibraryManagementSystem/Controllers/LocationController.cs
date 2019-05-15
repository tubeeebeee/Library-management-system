using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class LocationController : Controller
    {
        // GET: Location
        [Authorize]
        public ActionResult Index(int? page)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var pageNumber = page ?? 1;
            var pageSize = 5;
            var list = db.Locations.OrderBy(x => x.Name).Where(x => x.isStatus == true).Select(x => new LocationModel
            {
                Id = x.Id,
                Name = x.Name,
                Note = x.Note
            }).ToPagedList(pageNumber, pageSize);
            ViewBag.LocationList = list;
            ViewBag.numPage = pageNumber;
            ViewBag.sizePage = pageSize;
            return View(list);
        }
        [Authorize]
        public ActionResult addEdit(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            LocationModel loc = new LocationModel();
            if (id > 0)
            {
                Location u = db.Locations.FirstOrDefault(x => x.Id == id);
                loc.Id = u.Id;
                loc.Name = u.Name;

                loc.Note = u.Note;

            }
            return PartialView("_contentAddEditLocation", loc);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(LocationModel model)
        {
            bool check;
            FinalProjectEntities db = new FinalProjectEntities();
            try
            {

                if (model.Id > 0)
                {
                    Location u = db.Locations.FirstOrDefault(x => x.Id == model.Id);
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
                        Location u = new Location();
                        u.Name = model.Name;

                        u.Note = model.Note;

                        u.isStatus = true;
                        db.Locations.Add(u);
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
        public JsonResult DeleteLocation(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var loc = db.Locations.FirstOrDefault(x => x.Id == id);
            if (loc != null)
            {
                loc.isStatus = false;
                db.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public bool checkName(string text)
        {
            var check = false;
            FinalProjectEntities db = new FinalProjectEntities();
            var loc = db.Locations.Where(x => x.isStatus == true && x.Name == text).FirstOrDefault();
            if (loc != null)
            {
                check = true;
            }
            return check;
        }
    }
}