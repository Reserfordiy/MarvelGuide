namespace MarvelGuide.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        Admin = c.Boolean(nullable: false),
                        SuperAdmin = c.Boolean(nullable: false),
                        Moderator = c.Boolean(nullable: false),
                        Editor = c.Boolean(nullable: false),
                        Manager = c.Boolean(nullable: false),
                        Avatar_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.Avatar_Id)
                .Index(t => t.Avatar_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Avatar_Id", "dbo.Pictures");
            DropIndex("dbo.Users", new[] { "Avatar_Id" });
            DropTable("dbo.Users");
        }
    }
}
