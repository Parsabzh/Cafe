using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Model.Customer;
using Cafe7.Model.Invoice;
using Cafe7.Model.Migrations;
using Cafe7.Model.People;
using Cafe7.Model.Product;

namespace Cafe7.Model
{
   public class DbCafe7Managment:DbContext
    {
        public DbSet<PeopleModel> Peoples { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<InvoiceModel> Invoices { get; set; }
        public DbSet<InvoiceItemModel> InvoiceItems { get; set; }
        public DbSet<ScoreModel> Scores { get; set; }
        public DbSet<PointModel> Points { get; set; }


        public DbCafe7Managment():base("Server=.;Database=DbCafeManagmentTest2;Integrated Security=true")
        {
            if(Database.Exists())
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbCafe7Managment, Configuration>());
            else
                Database.SetInitializer(new CreateDatabaseIfNotExists<DbCafe7Managment>());

        }
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PointModel>().Property(o => o.Point).HasPrecision(12, 10);
            base.OnModelCreating(modelBuilder);
        }
    }
}
