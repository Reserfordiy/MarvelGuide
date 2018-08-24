namespace MarvelGuide.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeBugsFixed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "AgentsFirstWords", c => c.String());
            AlterColumn("dbo.Users", "AgentsLastWords", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "AgentsLastWords", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "AgentsFirstWords", c => c.Int(nullable: false));
        }
    }
}
