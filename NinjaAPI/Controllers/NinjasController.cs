using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using NinjaDomain.Classes;
using NinjaDomain.DataModel;

namespace NinjaAPI.Controllers
{
    public class NinjasController : ApiController
    {
        //private NinjaContext db = new NinjaContext();
        private readonly DisconnectedRepository _repo = new DisconnectedRepository();

        public IEnumerable<ViewListNinja> Get(string query="", int page = 0, int pageSize = 20)
        {
            var ninjas = _repo.GetQuryableNinjasWithClan(query, page, pageSize);
            return ninjas.Select(n => new ViewListNinja
            {
                ClanName = n.Clan.ClanName,
                DateOfBirth = n.DateOfBirth,
                Id = n.Id,
                Name = n.Name,
                ServedInOniwaban = n.ServedInOniwaban
            });
        }
        //GET: /api/Ninjas/1
        public Ninja Get(int id)
        {
            //return _repo.GetNinjaById(id);
            //return _repo.GetNinjaWithEquipment(id);
            return _repo.GetNinjaWithEquipmentAndClan(id);
        }

        public void Post([FromBody] object ninja)
        {
            var asNinja = JsonConvert.DeserializeObject<Ninja>(ninja.ToString());
            _repo.SaveUpdateNinja(asNinja);
        }
        public void Put(int id, [FromBody] Ninja ninja)
        {
            _repo.SaveNewNinja(ninja);
        }

        public void Delete(int id)
        {
            _repo.DeleteNinja(id);
        }
        // GET: api/Ninjas/5
        //[ResponseType(typeof(Ninja))]
        //public async Task<IHttpActionResult> GetNinja(int id)
        //{
        //    Ninja ninja = await db.Ninjas.FindAsync(id);
        //    if (ninja == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(ninja);
        //}

        //// PUT: api/Ninjas/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutNinja(int id, Ninja ninja)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != ninja.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(ninja).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!NinjaExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Ninjas
        //[ResponseType(typeof(Ninja))]
        //public async Task<IHttpActionResult> PostNinja(Ninja ninja)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Ninjas.Add(ninja);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = ninja.Id }, ninja);
        //}

        //// DELETE: api/Ninjas/5
        //[ResponseType(typeof(Ninja))]
        //public async Task<IHttpActionResult> DeleteNinja(int id)
        //{
        //    Ninja ninja = await db.Ninjas.FindAsync(id);
        //    if (ninja == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Ninjas.Remove(ninja);
        //    await db.SaveChangesAsync();

        //    return Ok(ninja);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool NinjaExists(int id)
        //{
        //    return db.Ninjas.Count(e => e.Id == id) > 0;
        //}
    }
}