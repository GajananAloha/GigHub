namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revertFollow : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Follows", "Artist_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follows", "Followee_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Follows", new[] { "Artist_Id" });
            DropIndex("dbo.Follows", new[] { "Followee_Id" });
            DropTable("dbo.Follows");
        }
        
        public override void Down()
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
                .PrimaryKey(t => new { t.ArtistId, t.FolloweeId });
            
            CreateIndex("dbo.Follows", "Followee_Id");
            CreateIndex("dbo.Follows", "Artist_Id");
            AddForeignKey("dbo.Follows", "Followee_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Follows", "Artist_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
