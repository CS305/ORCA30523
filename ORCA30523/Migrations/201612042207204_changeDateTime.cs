namespace ORCA30523.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "DatePosted", c => c.String());
            AlterColumn("dbo.Posts", "LastDate", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "LastDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Posts", "DatePosted", c => c.DateTime(nullable: false));
        }
    }
}
