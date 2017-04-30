using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NinjaDomain.Classes;
using NinjaDomain.Classes.Enums;
using NinjaDomain.DataModel;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //disable database initialization for the current context
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());
            //InsertNinja();
            //InsertMultipleNinjas();
            //InsertNinjaWithEquipment();
            //SimpleNinjaQuery();
            SimpleNinjaGraphQuery();
            //QueryAndUpdateNinja();
            //QueryAndUpdateNinjaDisconnected();
            //RetrieveDataWithFind();
            //RetrieveDataWithSql();
            //DeleteNinja();
            Console.ReadKey();
        }

        public static void InsertNinja()
        {
            var ninja = new Ninja
            {
                Name = "Harris",
                ServedIdOniwaban = false,
                DateOfBirth = new DateTime(2010, 1, 28),
                ClanId = 1
            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }
        }
        public static void InsertMultipleNinjas()
        {
            var ninja = new Ninja
            {
                Name = "John",
                ServedIdOniwaban = false,
                DateOfBirth = new DateTime(2011, 2, 12),
                ClanId = 1
            };
            var ninja1 = new Ninja
            {
                Name = "Mathew",
                ServedIdOniwaban = false,
                DateOfBirth = new DateTime(2001, 1, 2),
                ClanId = 1
            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.AddRange(new List<Ninja>{ ninja, ninja1 });
                context.SaveChanges();
            }
        }
        public static void InsertNinjaWithEquipment()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = new Ninja
                {
                    Name = "Kapamarou",
                    ServedIdOniwaban = false,
                    DateOfBirth = new DateTime(1977, 7, 4),
                    ClanId = 1
                };
                var muscles = new NinjaEquipment
                {
                    Name = "Muscles",
                    Type = EquipmentType.Tool
                };
                var spunk = new NinjaEquipment
                {
                    Name = "spunk",
                    Type = EquipmentType.Weapon
                };
                context.Ninjas.Add(ninja);
                ninja.EquipmentOwned.Add(muscles);

                ninja.EquipmentOwned.Add(spunk);
                context.SaveChanges();
            }    
        }
        public static void SimpleNinjaQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                //var ninjas = context.Ninjas.ToList();
                var theName = "Sampson";
                //var ninjas = context.Ninjas.Where(n => n.Name == theName);
                //var ninjas = context.Ninjas.Where(n => n.DateOfBirth >= new DateTime(1984,1,1));
                var ninjas = context.Ninjas.Where(n => n.DateOfBirth >= new DateTime(1980, 2, 2))
                    .OrderBy(n => n.Name)
                    .Skip(1)
                    .Take(2);
                    
                //var query = context.Ninjas;
                //var someninjas = query.ToList();
                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }
            }
        }

        public static void SimpleNinjaGraphQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                //simple like sql
                //var ninja = context.Ninjas
                //    .FirstOrDefault(n => n.Name.StartsWith("Har"));
                
                //join sql
                var ninja = context.Ninjas.Include(n => n.EquipmentOwned)
                    .FirstOrDefault(n => n.Name.StartsWith("kap"));
                if (ninja != null) Console.WriteLine(ninja.Name);
                context.Entry(ninja).Collection(n=>n.EquipmentOwned).Load();
            }
        }
        public static void QueryAndUpdateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                if (ninja != null) ninja.ServedIdOniwaban = !ninja.ServedIdOniwaban;
                context.SaveChanges();
            }

        }
        public static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }
            if (ninja != null) ninja.ServedIdOniwaban = !ninja.ServedIdOniwaban;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Attach(ninja);
                context.Entry(ninja).State= EntityState.Modified;
                context.SaveChanges();
            }
        }
        public static void RetrieveDataWithFind()
        {
            var keyval = 4;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.Write;
                var ninja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find #1" + ninja.Name);
                // seccond ninja is stored in the memory so seccond sql run
                var someNinja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find #2" + ninja.Name);
                ninja = null;
            }
        }
        public static void RetrieveDataWithSql()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas.SqlQuery("exec GetOldNinjas");
                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }
            }
            

        }
        public static void DeleteNinja()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
                //context.Ninjas.Remove(ninja);//1
                //context.SaveChanges();//1
            }
            using (var context = new NinjaContext())
            {
                var keyval = 4;
                context.Database.Log = Console.WriteLine;
                //context.Ninjas.Attach(ninja);//2
                //context.Ninjas.Remove(ninja);//2
                //context.Entry(ninja).State = EntityState.Deleted;//3
                //var delNinja = context.Ninjas.Find(keyval);//4
                //context.Ninjas.Remove(delNinja);//4
                context.Database.ExecuteSqlCommand("exec DeleteNinjaById {0}", keyval);
                context.SaveChanges();
            }
        }
    }
}
