namespace TaskManagerClassWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MyTasks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DateTimeStart = c.DateTime(nullable: false),
                        Event = c.Int(nullable: false),
                        Periodcity = c.Int(nullable: false),
                        Parameter1 = c.String(),
                        Parameter2 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MyTasks");
        }
    }
}
