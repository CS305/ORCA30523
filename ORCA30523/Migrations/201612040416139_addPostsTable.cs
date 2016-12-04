namespace ORCA30523.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPostsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.String(nullable: false, maxLength: 128),
                        PostID = c.String(maxLength: 128),
                        Body = c.String(),
                        Username = c.String(),
                        DatePosted = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Posts", t => t.PostID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.PostID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(),
                        Body = c.String(),
                        ToEmail = c.String(),
                        FromEmail = c.String(),
                        DatePosted = c.DateTime(nullable: false),
                        LastDate = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PostID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "PostID", "dbo.Posts");
            DropIndex("dbo.Posts", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Comments", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Comments", new[] { "PostID" });
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
        }
    }
}
