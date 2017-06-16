using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductDataAccess.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool isActive { get; set; }
    }
}