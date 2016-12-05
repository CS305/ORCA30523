namespace ORCA30523.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeforiegnkeys2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Comments", name: "PostID", newName: "ParentPost_PostID");
            RenameIndex(table: "dbo.Comments", name: "IX_PostID", newName: "IX_ParentPost_PostID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Comments", name: "IX_ParentPost_PostID", newName: "IX_PostID");
            RenameColumn(table: "dbo.Comments", name: "ParentPost_PostID", newName: "PostID");
        }
    }
}
