namespace ORCA30523.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTicketTableAgain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ToEmail = c.String(),
                        FromEmail = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        CreateDate = c.String(),
                        DatePosted = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tickets");
        }
    }
}
