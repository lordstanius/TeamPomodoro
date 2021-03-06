﻿using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Model;

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
        public IHttpActionResult Get(Guid id)
        {
            var user = db.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            db.Entry(user).Collection("Tasks").Load();
            foreach (var task in user.Tasks)
            {
                task.User = null;
                if (task.Pomodoroes != null)
                {
                    foreach (var pomodoro in task.Pomodoroes)
                    {
                        pomodoro.Task = null;
                    }
                }
            }

            return Ok(user);
        }

        // POST api/<controller>
        [ResponseType(typeof(void))]
        public IHttpActionResult Post(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserId))
                {
                    return NotFound();
                }

                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT api/<controller>/5
        [ResponseType(typeof(User))]
        public IHttpActionResult Put(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserId))
                {
                    return Conflict();
                }

                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
        }

        // DELETE api/<controller>/5
        [ResponseType(typeof(User))]
        public IHttpActionResult Delete(Guid id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool UserExists(Guid id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}