namespace ORCA30523.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeForiegnKeys : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "PostID");
            DropColumn("dbo.AspNetUsers", "CommentID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "CommentID", c => c.String());
            AddColumn("dbo.AspNetUsers", "PostID", c => c.String());
        }
    }
}
