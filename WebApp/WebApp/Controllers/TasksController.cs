using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Model;

namespace WebApp.Controllers
{
	public class TasksController : ApiController
	{
		private PomodoroContext db = new PomodoroContext();

		// GET: api/Tasks
		public IQueryable<Task> GetTasks()
		{
			return db.Tasks;
		}

		// GET: api/Tasks/5
		[ResponseType(typeof(Task))]
		public IHttpActionResult GetTask(Guid id)
		{
			Task task = db.Tasks.Find(id);

			task.Project = null; // unload all of this for serialization
			task.User = null;
			task.Pomodoroes = null;

			if (task == null)
				return NotFound();

			return Ok(task);
		}

		// PUT: api/Tasks/5
		[ResponseType(typeof(void))]
		public IHttpActionResult PostTask(Task task)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			db.Entry(task).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TaskExists(task.TaskId))
					return NotFound();

				throw;
			}

			return StatusCode(HttpStatusCode.NoContent);
		}

		// POST: api/Tasks
		[ResponseType(typeof(Task))]
		public IHttpActionResult PutTask(Task task)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			db.Tasks.Add(task);

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateException)
			{
				if (TaskExists(task.TaskId))
					return Conflict();

				throw;
			}

			return CreatedAtRoute("DefaultApi", new { id = task.TaskId }, task);
		}

		// DELETE: api/Tasks/5
		[ResponseType(typeof(Task))]
		public IHttpActionResult DeleteTask(Guid id)
		{
			Task task = db.Tasks.Find(id);
			if (task == null)
				return NotFound();

			db.Tasks.Remove(task);
			db.SaveChanges();

			return Ok(task);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool TaskExists(Guid id)
		{
			return db.Tasks.Count(e => e.TaskId == id) > 0;
		}
	}
}