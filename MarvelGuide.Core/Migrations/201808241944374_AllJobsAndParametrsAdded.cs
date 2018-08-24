namespace MarvelGuide.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllJobsAndParametrsAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Creator", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "AdminEditor", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "AdminAgent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "Agent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "ManagersRole", c => c.String());
            AddColumn("dbo.Users", "EditorsRubric", c => c.String());
            AddColumn("dbo.Users", "EditorsFrequency", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "AgentsNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "AgentsFirstWords", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "AgentsLastWords", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "Admin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Admin", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "AgentsLastWords");
            DropColumn("dbo.Users", "AgentsFirstWords");
            DropColumn("dbo.Users", "AgentsNumber");
            DropColumn("dbo.Users", "EditorsFrequency");
            DropColumn("dbo.Users", "EditorsRubric");
            DropColumn("dbo.Users", "ManagersRole");
            DropColumn("dbo.Users", "Agent");
            DropColumn("dbo.Users", "AdminAgent");
            DropColumn("dbo.Users", "AdminEditor");
            DropColumn("dbo.Users", "Creator");
        }
    }
}
