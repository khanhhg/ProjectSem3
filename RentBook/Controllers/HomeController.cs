using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentBook.Models;

namespace RentBook.Controllers
{
   
    public class HomeController : Controller
    {
        BookOnlineEntities db =new BookOnlineEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View(db.Books.ToList());
        }
        public ViewResult BookDetail(int BookID =0)
        {
            Book book = db.Books.SingleOrDefault(n => n.BookID == BookID);
            if(book == null)
            {
                // tra ve trang bao loi
                Response.StatusCode = 404;
                return null;
            }
            return View(book);
        }
      public ActionResult NewBookPartial()
        {
            return PartialView(db.Books.Take(4).ToList());
        }
        public ActionResult NewsPartial()
        {
            return PartialView(db.News.Take(4).ToList());
        }
     
    }
}