namespace Cafe7.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Web;

    public partial class Publish001 : DbMigration
    {
        public override void Up()
        {
            SqlFile("queries/publish001.sql");
        }
        
        public override void Down()
        {
        }
    }
}
