namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pomodoroes",
                c => new
                    {
                        PomodoroId = c.Guid(nullable: false),
                        TaskId = c.Guid(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        DurationInMin = c.Int(nullable: false),
                        IsSuccessfull = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PomodoroId)
                .ForeignKey("dbo.Tasks", t => t.TaskId, cascadeDelete: false)
                .Index(t => t.TaskId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Guid(nullable: false),
                        Name = c.String(),
                        PomodoroCount = c.Int(nullable: false),
                        UserId = c.Guid(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ProjectId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                        ShowWarningAfterPomodoroExpires = c.Boolean(nullable: false),
                        PomodoroDurationInMin = c.Int(nullable: false),
                        TeamId = c.Guid(nullable: true),
                        CurrentUserTeamId = c.Guid(nullable: true),
                    })
                .PrimaryKey(t => t.UserId)
                //.ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TeamId);
            
            CreateTable(
                "dbo.UserTeams",
                c => new
                    {
                        UserTeamId = c.Guid(nullable: false),
                        TeamId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        StopTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserTeamId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.TeamId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTeams", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserTeams", "TeamId", "dbo.Teams");
            //DropForeignKey("dbo.Users", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Tasks", "UserId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Pomodoroes", "TaskId", "dbo.Tasks");
            DropIndex("dbo.UserTeams", new[] { "UserId" });
            DropIndex("dbo.UserTeams", new[] { "TeamId" });
            DropIndex("dbo.Users", new[] { "TeamId" });
            DropIndex("dbo.Tasks", new[] { "ProjectId" });
            DropIndex("dbo.Tasks", new[] { "UserId" });
            DropIndex("dbo.Pomodoroes", new[] { "TaskId" });
            DropTable("dbo.UserTeams");
            DropTable("dbo.Teams");
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
            DropTable("dbo.Tasks");
            DropTable("dbo.Pomodoroes");
        }
    }
}
