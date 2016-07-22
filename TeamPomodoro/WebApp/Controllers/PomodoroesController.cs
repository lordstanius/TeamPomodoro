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
	public class PomodoroesController : ApiController
	{
		private PomodoroContext db = new PomodoroContext();

		// GET: api/Pomodoroes
		public IQueryable<Pomodoro> GetPomodoros()
		{
			return db.Pomodoros;
		}

		// GET: api/Pomodoroes/5
		[ResponseType(typeof(Pomodoro))]
		public IHttpActionResult GetPomodoro(Guid id)
		{
			Pomodoro pomodoro = db.Pomodoros.Find(id);

			if (pomodoro == null)
				return NotFound();

			return Ok(pomodoro);
		}

		// PUT: api/Pomodoroes/5
		[ResponseType(typeof(void))]
		public IHttpActionResult PostPomodoro(Pomodoro pomodoro)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			db.Entry(pomodoro).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PomodoroExists(pomodoro.PomodoroId))
					return NotFound();

				throw;
			}

			return StatusCode(HttpStatusCode.NoContent);
		}

		// POST: api/Pomodoroes
		[ResponseType(typeof(Pomodoro))]
		public IHttpActionResult PutPomodoro(Pomodoro pomodoro)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			db.Pomodoros.Add(pomodoro);

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateException)
			{
				if (PomodoroExists(pomodoro.PomodoroId))
					return Conflict();

				throw;
			}

			return CreatedAtRoute("DefaultApi", new { id = pomodoro.PomodoroId }, pomodoro);
		}

		// DELETE: api/Pomodoroes/5
		[ResponseType(typeof(Pomodoro))]
		public IHttpActionResult DeletePomodoro(Guid id)
		{
			Pomodoro pomodoro = db.Pomodoros.Find(id);
			if (pomodoro == null)
				return NotFound();

			db.Pomodoros.Remove(pomodoro);
			db.SaveChanges();

			return Ok(pomodoro);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				db.Dispose();

			base.Dispose(disposing);
		}

		private bool PomodoroExists(Guid id)
		{
			return db.Pomodoros.Count(e => e.PomodoroId == id) > 0;
		}
	}
}