using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentBook.Models;

namespace RentBook.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        BookOnlineEntities db = new BookOnlineEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult XemChiTiet()
        {
            return View(db.Customers.OrderBy(n => n.CustomerID));
        }
        [HttpPost]
        public ActionResult LoginAdmin(FormCollection collection)
        {
            string Email = collection["txtUser"].ToString();
            string Pass = collection["txtPass"].ToString();
            Customer customer = db.Customers.SingleOrDefault(n => n.CusEmail == Email && n.PassWord == Pass);
            if (customer != null)
            {
               
                Session["user"] = customer;
                Session["CusId"] = customer.CustomerID;
                return RedirectToAction("Index","Home");
                //return View("Home");

            }
            return Content("Tài khoản hoặc mật khẩu không đúng!");
        }
	}
}