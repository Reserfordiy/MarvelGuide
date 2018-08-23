namespace MarvelGuide.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewModels : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Roules", newName: "Documents");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Documents", newName: "Roules");
        }
    }
}
