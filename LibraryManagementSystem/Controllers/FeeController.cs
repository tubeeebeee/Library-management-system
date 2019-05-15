using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.Models;
using PagedList;

namespace LibraryManagementSystem.Controllers
{
    public class FeeController : Controller
    {
        // GET: Fee
        public ActionResult Index()
        {
            DateTime d;
            FinalProjectEntities db = new FinalProjectEntities();
           
            List<Borrow> list = db.Borrows.Where(x => x.isStatus == true).ToList();
            var listFee = new List<FeeModel>();
            foreach(var item in list)
            {
                if(item.Until < DateTime.Now)
                {
                    FeeModel fee = new FeeModel();
                    fee.name = item.UserSystem.Name;
                    fee.fee = (item.Book.Cost / 100) * 15;
                    fee.note = item.Note;
                    listFee.Add(fee);
                }
                
            }
            ViewBag.feeList = listFee;
           
            return View();
        }
    }
}