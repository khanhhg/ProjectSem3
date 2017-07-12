using RentBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentBook.Controllers
{
    public class ShopCartController : Controller
    {
        BookOnlineEntities db = new BookOnlineEntities();

        //###############################################################################################
        public List<ShopCart> GetShopCart()
        {
            List<ShopCart> listShopCart = Session["ShopCart"] as List<ShopCart>;
            if (listShopCart == null)
            {
                // neu gio hang chua ton tai ta tien hanh khoi tao list gio hang
                listShopCart = new List<ShopCart>();
                Session["ShopCart"] = listShopCart;
            }
            return listShopCart;
        }
        // them gio hang
        public ActionResult AddShopCart(int BookID, string strURL)
        {
            Book book = db.Books.SingleOrDefault(n => n.BookID == BookID);
            if (book == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // lay ra sesstin gio hang
            List<ShopCart> listShopCart = GetShopCart();
            // kiem tra sach nay da ton tai trong sesstion[] gio hang chua
            ShopCart SP = listShopCart.Find(n => n.BookID == BookID);
            if (SP == null)
            {
                SP = new ShopCart(BookID);
                // add san pham moi them
                listShopCart.Add(SP);
                return Redirect(strURL);
            }
            else
            {
                SP.Quantity++;
                return Redirect(strURL);
            }
        }
        // cap nhat gio hang
        public ActionResult UpdateShopCart(int BookID, FormCollection f)
        {  // neu get sai ma san pham se tra ve trang loi 404
            Book book = db.Books.SingleOrDefault(n => n.BookID == BookID);
            if (book == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // dung thi lay gio hang ra tu sesstion
            List<ShopCart> listShopCart = GetShopCart();
            // kiem tra sp co ton tai trong gio hang hay khong
            ShopCart SP = listShopCart.Find(n => n.BookID == BookID);
            // neu ma ton tai ta cho sua so luong lai
            if (SP != null)
            {
                SP.Quantity = int.Parse(f["txtSoLuong"].ToString());
            }
            return View("Shopcart");
        }
        // xoa gio hang
        public ActionResult DeleteShopCart(int BookID)
        {
            Book book = db.Books.SingleOrDefault(n => n.BookID == BookID);
            if (book == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // dung thi lay gio hang ra tu sesstion
            List<ShopCart> listShopCart = GetShopCart();
            // kiem tra sp co ton tai trong gio hang hay khong
            ShopCart SP = listShopCart.Find(n => n.BookID == BookID);
            // neu ma ton tai ta cho sua so luong lai
            if (SP != null)
            {
                listShopCart.RemoveAll(n => n.BookID == BookID);

            }
            if (listShopCart.Count == 0)
            {
                return RedirectToAction("Home", "Index");
            }
            return RedirectToAction("Shopcart");
        }
        // xau dung trang gio hang
        public ActionResult ShopCart()
        {
            if (Session["ShopCart"] == null)
            {
                return RedirectToAction("Home", "Index");
            }
            List<ShopCart> listShopCart = GetShopCart();
            return View(listShopCart);
        }
        // tinh tong so luong 
        private double TongSoLuong()
        {
            int Quantity = 0;
            List<ShopCart> listShopCart = Session["ShopCart"] as List<ShopCart>;
            if (listShopCart != null)
            {
                Quantity = listShopCart.Sum(n => n.Quantity);
            }
            return Quantity;
        }
        // tinh tong tien
        private decimal Total()
        {
            decimal Total = 0;
            List<ShopCart> listShopCart = Session["ShopCart"] as List<ShopCart>;
            if (listShopCart != null)
            {
                Total = listShopCart.Sum(n => n.Price);
            }
            return Total;
        }

    }
}