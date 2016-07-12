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
				//unitOfWork.UsersAsync.AddAsync(new User { UserName = "Zika", UserId = Guid.NewGuid() });
				//unitOfWork.Users.Add(new User { UserName = "Laza", UserId = Guid.NewGuid() });
				//unitOfWork.Users.Add(new User { UserName = "Pera", UserId = Guid.NewGuid() });

				foreach (var user in unitOfWork.Users.GetAll())
					Console.WriteLine("User name: {0}, id = {1}", user.UserName, user.UserId);
			}
		}
	}
}
