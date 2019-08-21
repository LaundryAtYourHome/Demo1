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
using LaundryAtYourDoor.Models;

namespace LaundryAtYourDoor.Controllers
{
    public class LaundriesController : ApiController
    {
        private registrationDBEntities1 db = new registrationDBEntities1();

        // GET: api/Laundries
        public IQueryable<Laundry> GetLaundries()
        {
            return db.Laundries;
        }

        // GET: api/Laundries/5
        [ResponseType(typeof(Laundry))]
        public IHttpActionResult GetLaundry(string id)
        {
            Laundry laundry = db.Laundries.Find(id);
            if (laundry == null)
            {
                return NotFound();
            }

            return Ok(laundry);
        }

        // PUT: api/Laundries/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLaundry(string id, Laundry laundry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != laundry.LaundryOwner)
            {
                return BadRequest();
            }

            db.Entry(laundry).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaundryExists(id))
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

        // POST: api/Laundries
        [ResponseType(typeof(Laundry))]
        public IHttpActionResult PostLaundry(Laundry laundry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Laundries.Add(laundry);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LaundryExists(laundry.LaundryOwner))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = laundry.LaundryOwner }, laundry);
        }

        // DELETE: api/Laundries/5
        [ResponseType(typeof(Laundry))]
        public IHttpActionResult DeleteLaundry(string id)
        {
            Laundry laundry = db.Laundries.Find(id);
            if (laundry == null)
            {
                return NotFound();
            }

            db.Laundries.Remove(laundry);
            db.SaveChanges();

            return Ok(laundry);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LaundryExists(string id)
        {
            return db.Laundries.Count(e => e.LaundryOwner == id) > 0;
        }
    }
}