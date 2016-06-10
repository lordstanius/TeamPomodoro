using System;
using System.Linq;
using Model;

namespace EntiryFrameworkDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var db = new PomodoroContext())
			{
				Console.Write("Enter a user name: ");
				var input = Console.ReadLine();
				User user = null;
				var users = from u in db.Users where u.UserName == input select u;

				foreach (var u in users)
				{
					user = u;
					break;
				}

				if (user == null)
				{
					user = new User
					{
						UserId = Guid.NewGuid(),
						UserName = input
					};

					db.Users.Add(user);
				}

				Console.WriteLine("Available projects:");
				var projects = from p in db.Projects
									select p;

				int i = 0;
				foreach (var p in projects)
					Console.WriteLine("{0}: {1}", ++i, p.Name);

				if (i == 0)
				{
					Console.WriteLine("None.");
					input = "A";
				}
				else
				{
					Console.Write("Type '1' to '{0}' to choose project, or 'A' to add new project: ", i);
					input = Console.ReadLine();
				}

				Project project = null;
				if (input.ToLower() == "a")
				{
					Console.Write("Enter project name: ");
					input = Console.ReadLine();
					project = new Project
					{
						ProjectId = Guid.NewGuid(),
						Name = input
					};
					db.Projects.Add(project);
				}
				else
				{
					i = 0;
					int index = int.Parse(input);
					foreach (var p in projects)
						if (++i == index)
						{
							project = p;
							break;
						}
				}

				Console.Write("Enter task name: ");
				input = Console.ReadLine();
				var task = new Task
				{
					TaskId = Guid.NewGuid(),
					Name = input,
					PomodoroCount = 3,
					Project = project,
					UserId = user.UserId
				};

				db.Tasks.Add(task);
				db.SaveChanges();

				// Display all users from the database 
				var query = from u in db.Users
								orderby u.UserName
								select u;

				Console.WriteLine("===================================");
				Console.WriteLine("All users in the database:");
				Console.WriteLine("-----------------------------------");
				foreach (var item in query)
				{
					Console.WriteLine("User '{0}' works on:", item.UserName);
					foreach (var t in item.Tasks)
						Console.WriteLine("  Task '{0}' in project '{1}'", t.Name, t.Project.Name);
				}

				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();
			}
		}
	}
}
