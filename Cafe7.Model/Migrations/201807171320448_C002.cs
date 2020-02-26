namespace Cafe7.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class C002 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PointModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Point = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PointModels");
        }
    }
}
