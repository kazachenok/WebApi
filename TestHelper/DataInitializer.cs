using DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsHelper
{
    public class DataInitializer
    {
        public static List<Product> GetAllProducts()
        {
            var products = new List<Product>
            {
                new Product() { Id = 1, Name = "Update Service", CreateDate = DateTime.Now.AddDays(-1), isActive = true },
                new Product() { Id = 2, Name = "Product Registration", CreateDate = DateTime.Now.AddDays(-2), isActive = false },
                new Product() { Id = 3, Name = "Core Service", CreateDate = DateTime.Now.AddDays(-3), isActive = true },
                new Product() { Id = 4, Name = "Mobile Device Service", CreateDate = DateTime.Now.AddDays(-4), isActive = true },
                new Product() { Id = 5, Name = "Customer Service", CreateDate = DateTime.Now.AddDays(-5), isActive = false }
            };

            return products;
        }

        public static List<Metadata> GetAllMetadata()
        {
            var metadata = new List<Metadata>
            {
                new Metadata() { Id = 1, Key = "", Value = "", ProductId = 1 },
                new Metadata() { Id = 2, Key = "", Value = "", ProductId = 1 },
                new Metadata() { Id = 3, Key = "", Value = "", ProductId = 2 },
                new Metadata() { Id = 4, Key = "", Value = "", ProductId = 2 },
                new Metadata() { Id = 5, Key = "", Value = "", ProductId = 2 },
                new Metadata() { Id = 6, Key = "", Value = "", ProductId = 2 },
                new Metadata() { Id = 7, Key = "", Value = "", ProductId = 2 },
                new Metadata() { Id = 8, Key = "", Value = "", ProductId = 2 },
                new Metadata() { Id = 9, Key = "", Value = "", ProductId = 3 },
                new Metadata() { Id = 10, Key = "", Value = "", ProductId = 3 },
                new Metadata() { Id = 11, Key = "", Value = "", ProductId = 3 },
                new Metadata() { Id = 12, Key = "", Value = "", ProductId = 3 },
                new Metadata() { Id = 13, Key = "", Value = "", ProductId = 3 },
                new Metadata() { Id = 14, Key = "", Value = "", ProductId = 3 },
                new Metadata() { Id = 15, Key = "", Value = "", ProductId = 3 },
                new Metadata() { Id = 16, Key = "", Value = "", ProductId = 3 },
                new Metadata() { Id = 17, Key = "", Value = "", ProductId = 3 }
            };
            return metadata;
        }
    }
}
