namespace Cafe7.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class C007 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("PeopleInfo.Customer", "RegistrationNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("PeopleInfo.Customer", "RegistrationNumber", c => c.String(nullable: false));
        }
    }
}
