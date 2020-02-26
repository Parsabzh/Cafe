namespace Cafe7.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class C006 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("PeopleInfo.Point", "Point", c => c.Decimal(nullable: false, precision: 12, scale: 10));
        }
        
        public override void Down()
        {
            AlterColumn("PeopleInfo.Point", "Point", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
