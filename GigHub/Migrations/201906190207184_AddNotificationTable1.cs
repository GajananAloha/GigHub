namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notifications", "DateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "DateTime", c => c.Int(nullable: false));
        }
    }
}
