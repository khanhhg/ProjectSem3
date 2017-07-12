using RentBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models
{

    public class ItemGioHang
    {

        public int BookID { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string BookImage { get; set; }
        public ItemGioHang(int BookID)
        {
            using (BookOnlineEntities db = new BookOnlineEntities())
            {
                this.BookID = BookID;
                Book book = db.Books.Single(n => n.BookID == BookID);
                this.BookName = book.BookName;
                this.BookImage = book.BookImage;
                this.Price = book.Price;
                this.Quantity = 1;
                this.Total = Price * Quantity;
            }
        }
        //public ItemGioHang(int BookID, int SL)
        //{
        //    using (BookOnlineEntities db = new BookOnlineEntities())
        //    {
        //        this.IdSanPham = IdSanPham;
        //        SanPham sanpham = db.SanPhams.Single(n => n.IdSanPham == IdSanPham);
        //        this.TenSanPham = sanpham.TenSanPham;
        //        this.HinhAnh = sanpham.HinhAnh;
        //        this.DonGia = sanpham.DonGia.Value;
        //        this.SoLuong = SL;
        //        this.TongTien = DonGia * SoLuong;
        //    }
        //}
        public ItemGioHang()
        {

        }
    }
}