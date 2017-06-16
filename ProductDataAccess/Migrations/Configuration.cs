namespace ProductDataAccess.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProductDataAccess.Models.ProductDataAccessContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProductDataAccessContext context)
        {
            context.Products.AddOrUpdate(x => x.Id,
                new Product() { Id = 1, Name = "Update Service", CreateDate = DateTime.Now.AddDays(-1), isActive = true },
                new Product() { Id = 2, Name = "Product Registration", CreateDate = DateTime.Now.AddDays(-2), isActive = false },
                new Product() { Id = 3, Name = "Core Service", CreateDate = DateTime.Now.AddDays(-3), isActive = true },
                new Product() { Id = 4, Name = "Mobile Device Service", CreateDate = DateTime.Now.AddDays(-4), isActive = true },
                new Product() { Id = 5, Name = "Customer Service", CreateDate = DateTime.Now.AddDays(-5), isActive = false }
            );

            context.ProductMetadatas.AddOrUpdate(x => x.Id,
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
            );
        }
    }
}
