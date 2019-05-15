using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.Models;
using PagedList;

namespace LibraryManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        [Authorize]
        public ActionResult Index(int? page)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var list = db.UserSystems.OrderBy(x => x.Name).Where(x => x.isStatus == true).Select(x => new StudentModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Addr = x.Addr,
                Fee = x.Fee,
                Note = x.Note
            }).ToPagedList(pageNumber, pageSize);
            ViewBag.studentList = list;
            ViewBag.numPage = pageNumber;
            ViewBag.sizePage = pageSize;
            return View(list);
        }
        [Authorize]
        public ActionResult addEdit(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            StudentModel student = new StudentModel();
            if (id > 0)
            {
                UserSystem u = db.UserSystems.FirstOrDefault(x => x.Id == id);
                student.Id = u.Id;
                student.Name = u.Name;
                student.Email = u.Email;
                student.Phone = u.Phone;
                student.Addr = u.Addr;
                student.Note = u.Note;

            }
            return PartialView("_contentAddEditStudent", student);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(StudentModel model)
        {
            
            FinalProjectEntities db = new FinalProjectEntities();
            try
            {
                if (model.Id > 0)
                {
                    UserSystem u = db.UserSystems.FirstOrDefault(x => x.Id == model.Id);
                    u.Name = model.Name;
                    u.Phone = model.Phone;
                    u.Email = model.Email;
                    u.Addr = model.Addr;
                    u.Note = model.Note;
                    db.SaveChanges();
                   
                }
                else
                {
                    
                    
                        UserSystem u = new UserSystem();
                        u.Name = model.Name;
                        u.Phone = model.Phone;
                        u.Email = model.Email;
                        u.Addr = model.Addr;
                        u.Note = model.Note;
                        u.Fee = 0;
                        u.isStatus = true;
                        db.UserSystems.Add(u);
                        db.SaveChanges();
                        
                    

                    }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [Authorize]
        public JsonResult DeleteStudent(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var student = db.UserSystems.FirstOrDefault(x => x.Id == id);
            if (student != null)
            {
                student.isStatus = false;
                db.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult getSearch(string text)
        {
            
            FinalProjectEntities db = new FinalProjectEntities();

            var list = db.UserSystems.Where(x => x.isStatus == true && x.Name.Contains(text)).Select(x => new StudentModel
            {
                Id = x.Id,
                Name = x.Name,
                Phone = x.Phone,
                Email = x.Email,
                Addr = x.Addr,
                Note = x.Note
            }).ToList();
            return PartialView("_contentStudentSearch", list);
        }
        [HttpPost]
        public ActionResult checkEmail(string text)
        {
            bool check = false;
            FinalProjectEntities db = new FinalProjectEntities();
            var e = db.UserSystems.Where(x => x.isStatus == true && x.Email == text).FirstOrDefault();
            if(e!=null)
            {
                check = true;
            }
            return Json(check,JsonRequestBehavior.AllowGet);
        }
        public bool checkPhone(string text)
        {
            bool check = false;
            FinalProjectEntities db = new FinalProjectEntities();
            var e = db.UserSystems.Where(x => x.isStatus == true && x.Phone == text).FirstOrDefault();
            if (e != null)
            {
                check = true;
            }
            return check;
        }
    }
}