using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Model;

namespace WebApp.Controllers
{
    public class UserTeamsController : ApiController
    {
        private PomodoroContext db = new PomodoroContext();

        // GET: api/UserTeams
        public IQueryable<UserTeam> GetUserTeams()
        {
            return db.UserTeams;
        }

        // GET: api/UserTeams/5
        [ResponseType(typeof(UserTeam))]
        public IHttpActionResult GetUserTeam(Guid id)
        {
            UserTeam userTeam = db.UserTeams.Find(id);
            if (userTeam == null)
                return NotFound();

            return Ok(userTeam);
        }

        // PUT: api/UserTeams/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PostUserTeam(UserTeam userTeam)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Entry(userTeam).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTeamExists(userTeam.UserTeamId))
                    return NotFound();

                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UserTeams
        [ResponseType(typeof(UserTeam))]
        public IHttpActionResult PutUserTeam(UserTeam userTeam)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.UserTeams.Add(userTeam);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserTeamExists(userTeam.UserTeamId))
                    return Conflict();

                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = userTeam.UserTeamId }, userTeam);
        }

        // DELETE: api/UserTeams/5
        [ResponseType(typeof(UserTeam))]
        public IHttpActionResult DeleteUserTeam(Guid id)
        {
            UserTeam userTeam = db.UserTeams.Find(id);
            if (userTeam == null)
                return NotFound();

            db.UserTeams.Remove(userTeam);
            db.SaveChanges();

            return Ok(userTeam);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }

        private bool UserTeamExists(Guid id)
        {
            return db.UserTeams.Count(e => e.UserTeamId == id) > 0;
        }
    }
}