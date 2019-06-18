namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyCloumnNameInFollowTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Follows", name: "FolloweeId", newName: "FollowerId");
            RenameIndex(table: "dbo.Follows", name: "IX_FolloweeId", newName: "IX_FollowerId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Follows", name: "IX_FollowerId", newName: "IX_FolloweeId");
            RenameColumn(table: "dbo.Follows", name: "FollowerId", newName: "FolloweeId");
        }
    }
}
