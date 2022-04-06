using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaPPP.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public byte[] ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryName { get; internal set; }
    }
}