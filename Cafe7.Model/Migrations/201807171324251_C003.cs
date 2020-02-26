namespace Cafe7.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class C003 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PointModels", newName: "Point");
            MoveTable(name: "dbo.Point", newSchema: "PeopleInfo");
        }
        
        public override void Down()
        {
            MoveTable(name: "PeopleInfo.Point", newSchema: "dbo");
            RenameTable(name: "dbo.Point", newName: "PointModels");
        }
    }
}
