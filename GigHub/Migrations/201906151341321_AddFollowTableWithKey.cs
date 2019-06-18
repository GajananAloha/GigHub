namespace GigHub.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddFollowTableWithKey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Follows",
                c => new
                    {
                        ArtistId = c.String(nullable: false, maxLength: 128),
                        FolloweeId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ArtistId, t.FolloweeId })
                .ForeignKey("dbo.AspNetUsers", t => t.ArtistId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.FolloweeId, cascadeDelete: false)
                .Index(t => t.ArtistId)
                .Index(t => t.FolloweeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follows", "FolloweeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "ArtistId", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "FolloweeId" });
            DropIndex("dbo.Follows", new[] { "ArtistId" });
            DropTable("dbo.Follows");
        }
    }
}
