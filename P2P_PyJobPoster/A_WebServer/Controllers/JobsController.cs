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
using A_WebServer.Models;

namespace A_WebServer.Controllers
{
    public class JobsController : ApiController
    {
        private clientDBEntities db = new clientDBEntities();

        // GET: api/Jobs
        public List<Job> GetJobs()
        {
            return db.Jobs.ToList();
        }

        // GET: api/Jobs/5
        [ResponseType(typeof(Job))]
        public IHttpActionResult GetJob(int id)
        {
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        // PUT: api/Jobs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJob(int id, Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != job.Id)
            {
                return BadRequest();
            }

            db.Entry(job).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Jobs
        [ResponseType(typeof(Job))]
        public IHttpActionResult PostJob(Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Jobs.Add(job);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (JobExists(job.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = job.Id }, job);
        }

        // DELETE: api/Jobs/5
        [ResponseType(typeof(Job))]
        public IHttpActionResult DeleteJob(int id)
        {
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return NotFound();
            }

            db.Jobs.Remove(job);
            db.SaveChanges();

            return Ok(job);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JobExists(int id)
        {
            return db.Jobs.Count(e => e.Id == id) > 0;
        }
    }
}