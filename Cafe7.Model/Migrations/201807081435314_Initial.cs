namespace Cafe7.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "PeoplInfo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Family = c.String(nullable: false),
                        NationalId = c.String(nullable: false),
                        Mobile = c.String(nullable: false),
                        Birthday = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Address = c.String(),
                        Day = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Store.InvoiceItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Qty = c.Int(nullable: false),
                        PName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PeopleInfo.Customer", t => t.CustomerId)
                .ForeignKey("Store.Invoice", t => t.InvoiceId, cascadeDelete: true)
                .Index(t => t.InvoiceId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "Store.Invoice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.String(),
                        TableNumber = c.Int(nullable: false),
                        Total = c.String(),
                        InvoiceNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Store.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Qty = c.Int(nullable: false),
                        Type = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "PeopleInfo.Score",
                c => new
                    {
                        CustomerId = c.Int(nullable: false),
                        LastScore = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("PeopleInfo.Customer", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "PeopleInfo.Customer",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        RegistrationNumber = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PeoplInfo.People", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("PeopleInfo.Customer", "Id", "PeoplInfo.People");
            DropForeignKey("PeopleInfo.Score", "CustomerId", "PeopleInfo.Customer");
            DropForeignKey("Store.InvoiceItem", "InvoiceId", "Store.Invoice");
            DropForeignKey("Store.InvoiceItem", "CustomerId", "PeopleInfo.Customer");
            DropIndex("PeopleInfo.Customer", new[] { "Id" });
            DropIndex("PeopleInfo.Score", new[] { "CustomerId" });
            DropIndex("Store.InvoiceItem", new[] { "CustomerId" });
            DropIndex("Store.InvoiceItem", new[] { "InvoiceId" });
            DropTable("PeopleInfo.Customer");
            DropTable("PeopleInfo.Score");
            DropTable("Store.Product");
            DropTable("Store.Invoice");
            DropTable("Store.InvoiceItem");
            DropTable("PeoplInfo.People");
        }
    }
}
