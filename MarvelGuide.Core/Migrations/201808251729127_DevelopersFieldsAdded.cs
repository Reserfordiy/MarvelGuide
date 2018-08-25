namespace MarvelGuide.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DevelopersFieldsAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Male", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "SuperDeveloper", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "DeveloperManager", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "DeveloperAgent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "DeveloperEditor", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "DeveloperModerator", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "DeveloperModerator");
            DropColumn("dbo.Users", "DeveloperEditor");
            DropColumn("dbo.Users", "DeveloperAgent");
            DropColumn("dbo.Users", "DeveloperManager");
            DropColumn("dbo.Users", "SuperDeveloper");
            DropColumn("dbo.Users", "Male");
        }
    }
}
