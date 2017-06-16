using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductDataAccess.Models
{
    public class Metadata
    {
        public int Id { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}