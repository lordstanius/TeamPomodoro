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
	public class ProjectsController : ApiController
	{
		private PomodoroContext db = new PomodoroContext();

		// GET: api/Projects
		public IQueryable<Project> GetProjects()
		{
			return db.Projects;
		}

		// GET: api/Projects/5
		[ResponseType(typeof(Project))]
		public IHttpActionResult GetProject(Guid id)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
				return NotFound();

			return Ok(project);
		}

		// PUT: api/Projects/5
		[ResponseType(typeof(void))]
		public IHttpActionResult PostProject(Project project)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			db.Entry(project).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProjectExists(project.ProjectId))
					return NotFound();

				throw;
			}

			return StatusCode(HttpStatusCode.NoContent);
		}

		// POST: api/Projects
		[ResponseType(typeof(Project))]
		public IHttpActionResult PutProject(Project project)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			db.Projects.Add(project);

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateException)
			{
				if (ProjectExists(project.ProjectId))
					return Conflict();

				throw;
			}

			return CreatedAtRoute("DefaultApi", new { id = project.ProjectId }, project);
		}

		// DELETE: api/Projects/5
		[ResponseType(typeof(Project))]
		public IHttpActionResult DeleteProject(Guid id)
		{
			Project project = db.Projects.Find(id);
			if (project == null)
				return NotFound();

			db.Projects.Remove(project);
			db.SaveChanges();

			return Ok(project);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				db.Dispose();

			base.Dispose(disposing);
		}

		private bool ProjectExists(Guid id)
		{
			return db.Projects.Count(e => e.ProjectId == id) > 0;
		}
	}
}