using RentBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentBook.Controllers
{
    public class GioHangController : Controller
    {
        //

        // GET: /GioHang/
        BookOnlineEntities db = new BookOnlineEntities();
      public List<ItemGioHang> LayGioHang()
        {
            // giỏ hàng đả tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                // nếu session giỏ hàng đã tồn tại
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;

            }
            return lstGioHang;
        }
        // thêm giỏ hàng thông thường (Load lại trang)
        public ActionResult ThemGioHang(int BookID, string strUrl)
        {
            // kiểm tra sản phẩm có tồn tại trong csdl ko
            Book book = db.Books.SingleOrDefault(n => n.BookID == BookID);
            if (book == null)
            {
                // trang đường dẩn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            // lấy giỏ hàng 
            List<ItemGioHang> lstGioHang = LayGioHang();
            // trường hợp đả tồn tại một sản phẩm trên giỏ hàng
            ItemGioHang sanphamcheck = lstGioHang.SingleOrDefault(n => n.BookID == BookID);
            if (sanphamcheck != null)
            {
                // kiểm tra số lượng sản phẩm tồn
                //if (book.SoLuongTon < sanphamcheck.SoLuong)
                //{
                //    return View("ThongBao");
                //}
                sanphamcheck.Quantity++;
                sanphamcheck.Total = sanphamcheck.Quantity * sanphamcheck.Price;
                return Redirect(strUrl);
            }
            ItemGioHang itemGH = new ItemGioHang(BookID);
            //if (sanpham.SoLuongTon < itemGH.SoLuong)
            //{
            //    return View("ThongBao");
            //}
            lstGioHang.Add(itemGH);
            db.SaveChanges();
            return Redirect(strUrl);
        }
        // xây dựng phương thức tính số lương
        public double TinhTongSoLuong()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.Quantity);
        }
        // phương thức tính tiền
        public decimal TongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.Total);
        }
        // xây dựng giỏ hàng partial
        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        // Xem giỏ hàng
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        [HttpGet]
        public ActionResult SuaGioHang(int BookID)
        {
            // kiểm tra sản phẩm có tồn tại ko
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // kiểm tra sản phẩm có tồn tại trong csdl ko
            Book book = db.Books.SingleOrDefault(n => n.BookID == BookID);
            if (book == null)
            {
                // trang đường dẩn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            // lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            // kiểm tra sản phẩm có tồn tại trong giỏ hàng chưa
            ItemGioHang sanphamcheck = lstGioHang.SingleOrDefault(n => n.BookID == BookID);
            if (sanphamcheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Lấy list giỏ hàng trên giao diện
            ViewBag.GioHang = lstGioHang;
            // nếu sản phẩm đả tồn tại
            return View(sanphamcheck);
        }
        // cập nhật giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            // kiểm tra số lượng tồn
            Book sanphamcheck = db.Books.SingleOrDefault(n => n.BookID == itemGH.BookID);
            //if (sanphamcheck.SoLuongTon < itemGH.SoLuong)
            //{
            //    return View("ThongBao");
            //}
            // cập nhật số lượng trong session giỏ hàng
            List<ItemGioHang> lstGH = LayGioHang();
            // lấy sản phẩm cập nhật từ  trong list<GioHang>
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.BookID == itemGH.BookID);
            // cập nhật lại số lượng và tổng tiền
            itemGHUpdate.Quantity = itemGH.Quantity;
            itemGHUpdate.Price = itemGHUpdate.Price * itemGHUpdate.Price;
            return RedirectToAction("XemGioHang");
        }
        public ActionResult XoaGioHang(int BookID)
        {
            // kiểm tra sản phẩm có tồn tại ko
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // kiểm tra sản phẩm có tồn tại trong csdl ko
            Book book = db.Books.SingleOrDefault(n => n.BookID == BookID);
            if (book == null)
            {
                // trang đường dẩn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            // lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            // kiểm tra sản phẩm có tồn tại trong giỏ hàng chưa
            ItemGioHang sanphamcheck = lstGioHang.SingleOrDefault(n => n.BookID == BookID);
            if (sanphamcheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // xóa sản phẩm 
            lstGioHang.Remove(sanphamcheck);
            return RedirectToAction("XemGioHang");
        }
        // xậy dựng action đặt hàng
        public ActionResult DatHang(Customer customer)
        {
           
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Customer cus = new Customer();


            if (Session["user"] == null)
            {
                // thêm khách hàng đối với khách hàng vãng lai(chưa có tài khoản)
                cus = customer;
                db.Customers.Add(cus);
                db.SaveChanges();
            }
           
                // thêm đơn hàng
                List<ItemGioHang> lstGH = LayGioHang();
            BorrowBook ddh = new BorrowBook();
            //ddh.CustomerID =  int.Parse(cus.CustomerID.ToString());
           
                ddh.CustomerID = int.Parse(Session["CusId"].ToString());
            
           

                ddh.BorrowDay = DateTime.Now;
            //ddh.TinhTrangGiaoHang = false;
            //ddh.DaThanhToan = false;
            //ddh.DaHuy = false;
            //ddh.DaXoa = false;
            //ddh.OderTotal = item.Total;
            ddh.OderTotal = TongTien();
                db.BorrowBooks.Add(ddh);
                db.SaveChanges();
                //}
                // thêm chi tiết đơn đặt hàng
                //List<ItemGioHang> lstGH = LayGioHang();
                foreach (var item in lstGH)
                {
              
                    BorrowBookDetail ctdh = new BorrowBookDetail();
                    ctdh.OrderID = ddh.OrderID;
                    ctdh.BookID = item.BookID;
                    //ctdh.TenSanPham = item.TenSanPham;
                    ctdh.Quantity = item.Quantity;
                    ctdh.Price = item.Price;
                    db.BorrowBookDetails.Add(ctdh);
                }
            
        
            Session["GioHang"] = null;
            db.SaveChanges();
            return View("ThongBao1");
        }
        // thêm giỏ hàng ajax
        public ActionResult ThemGioHangAjax(int BookID, string strUrl)
        {
            // kiểm tra sản phẩm có tồn tại trong csdl ko
            Book book = db.Books.SingleOrDefault(n => n.BookID == BookID);
            if (book == null)
            {
                // trang đường dẩn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            // lấy giỏ hàng 

            List<ItemGioHang> lstGioHang = LayGioHang();
            // trường hợp đả tồn tại một sản phẩm trên giỏ hàng
            ItemGioHang sanphamcheck = lstGioHang.SingleOrDefault(n => n.BookID == BookID);
            if (sanphamcheck != null)
            {
                // kiểm tra số lượng sản phẩm tồn
                //if (sanpham.SoLuongTon < sanphamcheck.SoLuong)
                //{
                //    return Content("<script>alert(\"Sản phẩm đả hết hàng !\")</script>");
                //}
                sanphamcheck.Quantity++;
                sanphamcheck.Total = sanphamcheck.Quantity * sanphamcheck.Price;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TongTien();
                return PartialView("GioHangPartial");
            }

            ItemGioHang itemGH = new ItemGioHang(BookID);
            //if (book.SoLuongTon < itemGH.SoLuong)
            //{
            //    return Content("<script>alert(\"Sản phẩm đả hết hàng !\")</script>");
            //}
            lstGioHang.Add(itemGH);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView("GioHangPartial");
        }
	}
}