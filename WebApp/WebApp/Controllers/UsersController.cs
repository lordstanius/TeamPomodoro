using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Model;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace WebApp.Controllers
{
	public class UsersController : ApiController
	{
		private PomodoroContext db = new PomodoroContext();

		// GET api/<controller>
		public IQueryable<User> Get()
		{
			return db.Users;
		}

		[ResponseType(typeof(User))]
		public IHttpActionResult Get(string userName)
		{
			User user = db.Users.FirstOrDefault(u => u.UserName == userName);
			if (user != null)
				return Ok(user);

			return NotFound();
		}

		// POST api/<controller>
		[ResponseType(typeof(void))]
		public IHttpActionResult Post(User user)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			db.Entry(user).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserExists(user.UserId))
					return NotFound();

				throw;
			}

			return StatusCode(HttpStatusCode.NoContent);
		}

		// PUT api/<controller>/5
		[ResponseType(typeof(void))]
		public IHttpActionResult Put(string userName)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var user = new User
			{
				UserName = userName,
				UserId = Guid.NewGuid(),
				PomodoroDurationInMin = 30,
			};

			db.Users.Add(user);

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateException)
			{
				if (UserExists(user.UserId))
					return Conflict();

				throw;
			}

			return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
		}

		// DELETE api/<controller>/5
		[ResponseType(typeof(User))]
		public IHttpActionResult Delete(string userName)
		{
			User user = db.Users.FirstOrDefault(u => u.UserName == userName);
			if (user == null)
				return NotFound();

			db.Users.Remove(user);
			db.SaveChanges();

			return Ok(user);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				db.Dispose();

			base.Dispose(disposing);
		}

		private bool UserExists(Guid id)
		{
			return db.Users.Count(e => e.UserId == id) > 0;
		}
	}
}