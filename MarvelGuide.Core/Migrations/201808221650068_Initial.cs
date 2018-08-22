namespace MarvelGuide.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageSource = c.String(),
                        IsBorderRequired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Text = c.String(),
                        Picture_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.Picture_Id)
                .Index(t => t.Picture_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Roules", "Picture_Id", "dbo.Pictures");
            DropIndex("dbo.Roules", new[] { "Picture_Id" });
            DropTable("dbo.Roules");
            DropTable("dbo.Pictures");
        }
    }
}
