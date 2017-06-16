using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataModel.Models
{
    public class DataModelContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public DataModelContext() : base("name=DataModelContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataModelContext, Migrations.Configuration>());
            //Database.SetInitializer<DataModelContext>(new DropCreateDatabaseIfModelChanges<DataModelContext>());
        }

        public System.Data.Entity.DbSet<DataModel.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<DataModel.Models.Metadata> ProductMetadatas { get; set; }
    }
}
