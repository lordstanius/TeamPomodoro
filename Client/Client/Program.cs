using System;
using DataAccess.Persistance;
using Model;

namespace Client
{
	class Program
	{
		const string Uri = "http://localhost:7718/api";

		static void Main(string[] args)
		{
			using (var unitOfWork = new UnitOfWork(Uri))
			{
				User u = unitOfWork.Users.Get(new Guid("34cf6108-73ed-4e1d-a3ea-1c1da65ea3b0"));
				Task t = unitOfWork.Tasks.Get(new Guid("317c4e5f-f2d7-4303-a747-3a52d883ca63"));

				Console.WriteLine(u.UserId + ": " + u.UserName);
				Console.WriteLine(t.TaskId + ": " + t.Name);

				foreach (var user in unitOfWork.Users.GetAll())
					Console.WriteLine(user.UserId + ": " + user.UserName);

				foreach (var task in unitOfWork.Tasks.GetAll())
					Console.WriteLine(task.UserId + ": " + task.Name);
			}
		}
	}
}
