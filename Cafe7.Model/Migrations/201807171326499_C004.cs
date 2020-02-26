namespace Cafe7.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class C004 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("PeopleInfo.Point", "Point", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("PeopleInfo.Point", "Point", c => c.Int(nullable: false));
        }
    }
}
