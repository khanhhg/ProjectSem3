using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentBook.Models;

namespace RentBook.Controllers
{
    public class LoginController : Controller
    {
        BookOnlineEntities db = new BookOnlineEntities();
        public ActionResult Index()
        {
            return View();
        }
        // GET: Login
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            string Email = collection["txtEmail"].ToString();
            string Password = collection["txtPass"].ToString();
            Customer cus = db.Customers.SingleOrDefault(n => n.CusEmail == Email && n.PassWord == Password);
            if (cus != null)
            {
                Session["user"] = cus;
                return View("home");



            }
            return Content("Tài khoản hoặc mật khẩu không đúng!");

        }
      
    }
}