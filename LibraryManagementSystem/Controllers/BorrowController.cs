using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.Models;
using PagedList;

namespace LibraryManagementSystem.Controllers
{
    public class BorrowController : Controller
    {
        // GET: Borrow
        [Authorize]
        public ActionResult Index(int? page)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var list = db.Borrows.OrderBy(x=>x.UserSystem.Name).Where(x => x.isStatus == true).Select(x => new BorrowModel
            {
                Id_bor = x.Id_bor,
                userName = x.UserSystem.Name,
                bookName = x.Book.Title,
                Since = x.Since,
                Until =x.Until,
                Note = x.Note
            }).ToPagedList(pageNumber, pageSize);
            ViewBag.borrowList = list;
            ViewBag.numPage = pageNumber;
            ViewBag.sizePage = pageSize;
            return View(list);
        }
        [Authorize]
        public ActionResult addEdit(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            BorrowModel model = new BorrowModel();
            if (id > 0)
            {
                var bor = db.Borrows.Where(x => x.Id_bor == id).FirstOrDefault();
                model.Id_bor = bor.Id_bor;
                model.Id_Book = bor.Id_Book;
                model.Id_User = bor.Id_User;
                model.Isbn = bor.Book.Isbn;
                model.Note = bor.Note;
            }
            return PartialView("_borrowAddEditContent", model);
        }
        [Authorize]
        public JsonResult checkUser(int id)
        {
            int check = 0;
            if (id == 0)
            {
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string user = "";
                string email = "";
                string note = "";
                FinalProjectEntities db = new FinalProjectEntities();
                var u = db.UserSystems.Where(x => x.Id == id && x.isStatus==true).FirstOrDefault();
                if (u != null)
                {
                    user = u.Name;
                    email = u.Email;
                    note = u.Note;
                }
                var res = new
                {
                    resUser = user,
                    resEmail = email,
                    resNote = note
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize]
        public JsonResult checkBook(string isbn)
        {
            int check = 0;
            if (isbn == "")
            {
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string title = "";
                string author = "";
                string desc = "";
                FinalProjectEntities db = new FinalProjectEntities();
                var u = db.Books.Where(x => x.Isbn == isbn && x.isStatus == true).FirstOrDefault();
                if (u != null)
                {
                    title = u.Title;
                    desc = u.Descriptions;
                    author = u.Author.Name;
                }
                var res = new
                {
                    resTitle = title,
                    resDesc = desc,
                    resAuthor = author
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(BorrowModel model)
        {
            int check = 0;
            FinalProjectEntities db = new FinalProjectEntities();
            var book = db.Books.Where(x => x.Isbn == model.Isbn && x.isStatus == true).FirstOrDefault();
            var checkCountBorrow = db.Borrows.Where(x => x.isStatus == true && x.Id_User == model.Id_User).Count(x => x.Id_User==model.Id_User);
            if (checkCountBorrow > 2)
            {
                check = 1;
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var final = db.Borrows.Where(x => x.Id_User == model.Id_User && x.Id_Book == book.Id && x.isStatus == true).FirstOrDefault();
                if (final == null)
                {
                    var idBook = db.Books.Where(x => x.Isbn == model.Isbn).FirstOrDefault();
                    Borrow b = new Borrow();
                    b.Id_User = model.Id_User;
                    b.Id_Book = idBook.Id;
                    b.Isbn = model.Isbn;
                    b.isStatus = true;
                    b.Since = DateTime.Now;
                    b.Until = DateTime.Now.Date.AddDays(10);
                    db.Borrows.Add(b);
                    db.SaveChanges();
                    var sendTo = db.UserSystems.Where(x => x.Id == model.Id_User).FirstOrDefault();
                    //email
                    bool result = false;
                    result = SendEmail(sendTo.Email,
                        "Check in Book",
                        "<p>Email send to you because you have borrow it<br/>Thank you!</p>");

                    //result = SendEmail(sendTo.Email,
                    //   "Check in Book",
                    //   "<p>Email send to you because you have borrow it<br/><table><thead><tr><th>Student Name</th><th>Book title</th><th>Since</th><th>Until</th></tr></thead><tbody><tr><td>" + b.Id_User + "</td><td>" + b.Isbn + "</td><td>" + b.Since + "</td><td>" + b.Until + "</td></tr></tbody></table>Thank you!</p>");
                    return Json(check, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    check = 2;
                    return Json(check, JsonRequestBehavior.AllowGet);
                }
            }
        }
        
        public bool SendEmail(string toEmail, string subject, string emailBody)
        {
            try
            {
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                MailMessage mailMessage = new MailMessage(senderEmail,toEmail,subject,emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;

                client.Send(mailMessage);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        [Authorize]
        public JsonResult returnBook(int id)
        {
            FinalProjectEntities db = new FinalProjectEntities();
            var bookReturn = db.Borrows.FirstOrDefault(x => x.Id_bor == id);
            if (bookReturn != null)
            {
                bookReturn.isStatus = false;
                db.SaveChanges();
                var sendTo = db.UserSystems.Where(x => x.Id == bookReturn.Id_User).FirstOrDefault();
                bool result = false;
                result = SendEmail(sendTo.Email, "Check out Book", "<p>Email send to you because you have return it<br/>Thank you!</p>");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

    }
}