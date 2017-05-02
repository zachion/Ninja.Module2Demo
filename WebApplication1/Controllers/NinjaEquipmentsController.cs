using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NinjaDomain.Classes;
using NinjaDomain.DataModel;

namespace NinjaMVC.Controllers
{
    public class NinjaEquipmentsController : Controller
    {
        //private NinjaContext db = new NinjaContext();
        private DisconnectedRepository _repo = new DisconnectedRepository();

        // GET: NinjaEquipments
        public ActionResult Index(int ninjaId, string ninjaName)
        {
            //return View(db.Equipment.ToList());
            ViewBag.NinjaId = ninjaId;
            ViewBag.NinjaName = ninjaName;
            return View();
        }

        // GET: NinjaEquipments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //NinjaEquipment ninjaEquipment = db.Equipment.Find(id);
            var ninjaEquipment = _repo.GetEquipmentById(id.Value);
            
            if (ninjaEquipment == null)
            {
                return HttpNotFound();
            }
            return View(ninjaEquipment);
        }

        // GET: NinjaEquipments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NinjaEquipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Type,DateModified,DateCreated")] NinjaEquipment ninjaEquipment,int ninjaId)
        {
            //if (ModelState.IsValid)
            if (ninjaEquipment.Name != null)
            {
                _repo.SaveNewEquipment(ninjaEquipment, ninjaId);
                
                //db.Equipment.Add(ninjaEquipment);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ninjaEquipment);
        }

        // GET: NinjaEquipments/Edit/5
        public ActionResult Edit(int? id, int ninjaId, string name)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.NinjaId = ninjaId;
            ViewBag.NinjaName = name;
            var ninjaEquipment = _repo.GetEquipmentById(id.Value);
            
            
            //NinjaEquipment ninjaEquipment = db.Equipment.Find(id);
            if (ninjaEquipment == null)
            {
                return HttpNotFound();
            }
            return View(ninjaEquipment);
        }

        // POST: NinjaEquipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Type,DateModified,DateCreated")] NinjaEquipment ninjaEquipment)
        {
            int ninjaId;
            if (!int.TryParse(Request.Form["NinjaId"], out ninjaId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _repo.SaveUpdatedEquipment(ninjaEquipment,ninjaId);
            return RedirectToAction("Edit", "Ninjas", new {id = ninjaId});
            //if (ModelState.IsValid)
            //{
            //    db.Entry(ninjaEquipment).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(ninjaEquipment);
        }

        // GET: NinjaEquipments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //NinjaEquipment ninjaEquipment = db.Equipment.Find(id);
            var ninjaEquipment = _repo.GetEquipmentById(id.Value);
            
            if (ninjaEquipment == null)
            {
                return HttpNotFound();
            }
            return View(ninjaEquipment);
        }

        // POST: NinjaEquipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //NinjaEquipment ninjaEquipment = db.Equipment.Find(id);
            //db.Equipment.Remove(ninjaEquipment);
            //db.SaveChanges();
            var ninjaEquipment = _repo.GetEquipmentById(id);
            
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
