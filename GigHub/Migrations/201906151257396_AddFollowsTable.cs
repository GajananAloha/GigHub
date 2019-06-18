namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFollowsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Follows",
                c => new
                    {
                        ArtistId = c.Int(nullable: false),
                        FolloweeId = c.Int(nullable: false),
                        Artist_Id = c.String(maxLength: 128),
                        Followee_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ArtistId, t.FolloweeId })
                .ForeignKey("dbo.AspNetUsers", t => t.Artist_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Followee_Id)
                .Index(t => t.Artist_Id)
                .Index(t => t.Followee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follows", "Followee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "Artist_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "Followee_Id" });
            DropIndex("dbo.Follows", new[] { "Artist_Id" });
            DropTable("dbo.Follows");
        }
    }
}
