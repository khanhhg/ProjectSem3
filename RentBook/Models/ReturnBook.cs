//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RentBook.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReturnBook
    {
        public int ReturnBookID { get; set; }
        public int OrderID { get; set; }
        public int BookID { get; set; }
        public System.DateTime ReturnDay { get; set; }
        public int Quantity { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual BorrowBook BorrowBook { get; set; }
    }
}
