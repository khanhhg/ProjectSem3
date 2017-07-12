using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RentBook.Models
{
    public class BooksController : Controller
    {
        private BookOnlineEntities db = new BookOnlineEntities();

        // GET: Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher);
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.PublicsherID = new SelectList(db.Publishers, "PublisherID", "PublisherName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // image
       
       //public ActionResult Create(Book imageModel)
       // {
       //     string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
       //     string extension = Path.GetExtension(imageModel.ImageFile.FileName);
       //     imageModel.BookImage = "~/Image/" + fileName;
       //     fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
       //     imageModel.ImageFile.SaveAs(fileName);
         
       // }
        //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID,BookName,BookImage,Description,UpdateDate,Quantity,Price,PublicsherID,CategoryID,AuthorID")] Book book,
            HttpPostedFileBase BookImg)
        {
          if(BookImg != null)
            {
                var fileName = Path.GetFileName(BookImg.FileName);
                var directoryToSave =HttpContext.Server.MapPath(Url.Content("~/Images/Book"));

                var pathToSave = Path.Combine(directoryToSave, fileName);
                BookImg.SaveAs(pathToSave);
                book.BookImage = fileName;
            }
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
            ViewBag.PublicsherID = new SelectList(db.Publishers, "PublisherID", "PublisherName", book.PublicsherID);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
            ViewBag.PublicsherID = new SelectList(db.Publishers, "PublisherID", "PublisherName", book.PublicsherID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,BookName,BookImage,Description,UpdateDate,Quantity,Price,PublicsherID,CategoryID,AuthorID")] Book book,
            HttpPostedFileBase BookImg)
        {
            if (ModelState.IsValid)
            {
                if(BookImg != null)
                {
                    var fileName = Path.GetFileName(BookImg.FileName);
                    var directoryToSave = Server.MapPath(Url.Content("~/images"));
                    var pathToSave = Path.Combine(directoryToSave, fileName);
                    BookImg.SaveAs(pathToSave);
                    book.BookImage = fileName;
                }
                db.Entry(book).State = EntityState.Modified;
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
            ViewBag.PublicsherID = new SelectList(db.Publishers, "PublisherID", "PublisherName", book.PublicsherID);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
