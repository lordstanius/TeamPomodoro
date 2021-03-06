namespace Model.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pomodoroes",
                c => new
                {
                    PomodoroId = c.Guid(nullable: false),
                    TaskId = c.Guid(nullable: false),
                    StartTime = c.DateTime(),
                    DurationInMin = c.Int(nullable: false),
                    IsSuccessfull = c.Boolean(),
                })
                .PrimaryKey(t => t.PomodoroId)
                .ForeignKey("dbo.Tasks", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId);

            CreateTable(
                "dbo.Tasks",
                c => new
                {
                    TaskId = c.Guid(nullable: false),
                    Name = c.String(maxLength: 50),
                    PomodoroCount = c.Int(nullable: false),
                    UserId = c.Guid(nullable: false),
                    ProjectId = c.Guid(nullable: false),
                    TeamId = c.Guid(),
                })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.UserId)
                .Index(t => t.ProjectId)
                .Index(t => t.TeamId);

            CreateTable(
                "dbo.Projects",
                c => new
                {
                    ProjectId = c.Guid(nullable: false),
                    Name = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.ProjectId);

            CreateTable(
                "dbo.Teams",
                c => new
                {
                    TeamId = c.Guid(nullable: false),
                    Name = c.String(maxLength: 20),
                })
                .PrimaryKey(t => t.TeamId);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    UserId = c.Guid(nullable: false),
                    UserName = c.String(maxLength: 20),
                    Password = c.String(maxLength: 64),
                    ShowWarningAfterPomodoroExpires = c.Boolean(nullable: false),
                    PomodoroDurationInMin = c.Int(nullable: false),
                    TeamId = c.Guid(),
                    CurrentUserTeamId = c.Guid(),
                })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.TeamId);

            CreateTable(
                "dbo.UserTeams",
                c => new
                {
                    UserTeamId = c.Guid(nullable: false),
                    TeamId = c.Guid(),
                    UserId = c.Guid(nullable: false),
                    StartTime = c.DateTime(),
                    StopTime = c.DateTime(),
                })
                .PrimaryKey(t => t.UserTeamId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.UserTeams", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserTeams", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Users", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Tasks", "UserId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Pomodoroes", "TaskId", "dbo.Tasks");
            DropIndex("dbo.UserTeams", new[] { "UserId" });
            DropIndex("dbo.UserTeams", new[] { "TeamId" });
            DropIndex("dbo.Users", new[] { "TeamId" });
            DropIndex("dbo.Tasks", new[] { "TeamId" });
            DropIndex("dbo.Tasks", new[] { "ProjectId" });
            DropIndex("dbo.Tasks", new[] { "UserId" });
            DropIndex("dbo.Pomodoroes", new[] { "TaskId" });
            DropTable("dbo.UserTeams");
            DropTable("dbo.Users");
            DropTable("dbo.Teams");
            DropTable("dbo.Projects");
            DropTable("dbo.Tasks");
            DropTable("dbo.Pomodoroes");
        }
    }
}
