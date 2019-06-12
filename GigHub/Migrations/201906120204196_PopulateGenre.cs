namespace GigHub.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PopulateGenre : DbMigration
    {
        public override void Up()
        {
            Sql("Insert INTO Genres (id, Name) Values(1, 'Jazz')");
            Sql("Insert INTO Genres (id, Name) Values(2,'Blues')");
            Sql("Insert INTO Genres (id, Name) Values(3,'Rock')");
            Sql("Insert INTO Genres (id, Name) Values(4, 'Country')");
        }
        
        public override void Down()
        {
            Sql("Delete from Genres where Id in (1,2,3,4)");
        }
    }
}
