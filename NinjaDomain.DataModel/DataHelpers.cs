using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NinjaDomain.Classes;
using NinjaDomain.Classes.Enums;

namespace NinjaDomain.DataModel
{
    public class DataHelpers
    {
        public static void NewDbWithSeed()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<NinjaContext>());


            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                if (context.Ninjas.Any())
                {
                    return;
                }
                var vtClan = context.Clans.Add(new Clan {ClanName = "Vermont Clan"});
                var turtleClan = context.Clans.Add(new Clan { ClanName = "Turtle Clan" });
                var amClan = context.Clans.Add(new Clan { ClanName = "amClan Clan" });

                context.SaveChanges();
                
                var shinobiNinja = context.Ninjas.Add(new Ninja
                {
                    Name = "Harris",
                    ServedIdOniwaban = false,
                    DateOfBirth = new DateTime(2010, 1, 28),
                    ClanId = 1
                });

                var johnNinja = new Ninja
                {
                    Name = "John",
                    ServedIdOniwaban = false,
                    DateOfBirth = new DateTime(2011, 2, 12),
                    ClanId = 1
                };
                var mathewNinja1 = new Ninja
                {
                    Name = "Mathew",
                    ServedIdOniwaban = false,
                    DateOfBirth = new DateTime(2001, 1, 2),
                    ClanId = 1
                };
                
                context.Ninjas.AddRange(new List<Ninja> { shinobiNinja, johnNinja, mathewNinja1 });
                context.SaveChanges();

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
    }
}
