using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NinjaDomain.Classes;

namespace NinjaDomain.DataModel
{
    public class DisconnectedRepository
    {
        public List<Ninja> GetNinjasWithClan()
        {
            using (var context = new NinjaContext())
            {
                //return context.Ninjas.Include(n => n.Clan).ToList(); // typical linq euery
                return context.Ninjas.AsNoTracking().Include(n => n.Clan).ToList();
            }
        }

        public Ninja GetNinjaWithEquipment(int id)
        {
            using (var context = new NinjaContext())
            {
                return context.Ninjas.AsNoTracking()
                    .Include(n => n.EquipmentOwned)
                    .FirstOrDefault(n => n.Id == id);
            }
        }

        public Ninja GetNinjaWithEquipmentAndClan(int id)
        {
            using (var context = new NinjaContext())
            {
                return context.Ninjas.AsNoTracking()
                    .Include(n => n.EquipmentOwned)
                    .Include(n => n.Clan)
                    .FirstOrDefault(n => n.Id == id);
            }
        }

        public IEnumerable GetClanList()
        {
            using (var context = new NinjaContext())
            {
                return context.Clans.AsNoTracking()
                    .OrderBy(n => n.ClanName)
                    .Select(c => new {c.Id, c.ClanName})
                    .ToList();
            }
        }

        public Ninja GetNinjaById(int id)
        {
            using (var context = new NinjaContext())
            {
                return context.Ninjas.Find(id);
                //return context.Ninjas.AsNoTracking().SingleOrDefault(n => n.Id == id);
            }
        }


        public NinjaEquipment GetEquipmentById(int id)
        {
            using (var context = new NinjaContext())
            {
                return context.Equipment.Find(id);
                //return context.Ninjas.AsNoTracking().SingleOrDefault(n => n.Id == id);
            }
        }
        public void SaveUpdateNinja(Ninja ninja)
        {
            using (var context = new NinjaContext())
            {
                context.Entry(ninja).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void SaveNewNinja(Ninja ninja)
        {
            using (var context = new NinjaContext())
            {
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }
        }

        public void DeleteNinja(int ninjaId)
        {
            using (var context = new NinjaContext())
            {
                var ninja = context.Ninjas.Find(ninjaId);
                context.Entry(ninja).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void SaveNewEquipment(NinjaEquipment equipment, int ninjaId)
        {
            using (var context = new NinjaContext())
            {
                var equipmentWithNinjaFromDatabase =
                    context.Equipment.Include(n => n.Ninja).FirstOrDefault(e => e.Id == equipment.Id);
                context.Entry(equipmentWithNinjaFromDatabase).CurrentValues.SetValues(equipment);
                context.SaveChanges();
            }
        }

        public void SaveUpdatedEquipment(NinjaEquipment equipment, int ninjaId)
        {
            using (var context = new NinjaContext())
            {
                var equipmentWithNinjaFromDatabase =
                    context.Equipment.Include(n => n.Ninja).FirstOrDefault(e => e.Id == ninjaId);
                context.Entry(equipmentWithNinjaFromDatabase).CurrentValues.SetValues(equipment);
                context.SaveChanges();
            }
        }

    }
}

