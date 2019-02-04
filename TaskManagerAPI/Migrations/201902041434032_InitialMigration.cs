namespace TaskManagerAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Task",
                c => new
                    {
                        Task_ID = c.Int(nullable: false, identity: true),
                        Parent_ID = c.Int(),
                        Task = c.String(nullable: false),
                        Start_Date = c.DateTime(nullable: false),
                        End_Date = c.DateTime(nullable: false),
                        Priority = c.Int(nullable: false),
                        IsTaskEnded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Task_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Task");
        }
    }
}
