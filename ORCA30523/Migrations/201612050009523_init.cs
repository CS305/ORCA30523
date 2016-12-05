namespace ORCA30523.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PostID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PostID");
        }
    }
}
