using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());
            InsertNinjaWithEquipment();
        }

        private static void InsertNinja()
        {
            var ninja = new Ninja
            {
                Name = "SampsonSan",
                ServedInOniwaban = false,
                DataOfBirth = new DateTime(2008, 1, 28),
                ClanId = 1
            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }
        }

        private static void InsertMultipleNinjas()
        {
            var ninja1 = new Ninja
            {
                Name = "Adam",
                ServedInOniwaban = true,
                DataOfBirth = new DateTime(1999, 5, 15),
                ClanId = 1
            };
            var ninja2 = new Ninja
            {
                Name = "Sean",
                ServedInOniwaban = false,
                DataOfBirth = new DateTime(2017, 12, 1),
                ClanId = 1
            };
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.AddRange(new List<Ninja> { ninja1, ninja2 });
                context.SaveChanges();
            }

        }

        private static void SimpleNinjaQueries()
        {
            using (var context = new NinjaContext())
            {
                var ninjas = context.Ninjas.Where(n => n.DataOfBirth >= new DateTime(2000, 1, 1)).OrderBy(n => n.Name);
                //var query = context.Ninjas;
                //var someNinjas = query.ToList();
                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }
            }
        }

        private static void QueryAndUpdateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);
                context.SaveChanges();
            }
        }

        private static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Attach(ninja);
                context.Entry(ninja).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private static void RetrieveDataWithFind()
        {
            var keyval = 4;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find#1: " + ninja.Name);

                var someNinja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find#2: " + someNinja.Name);
                ninja = null;
            }
        }

        private static void DeleteNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                context.Ninjas.Remove(ninja);
                context.SaveChanges();
            }
        }

        private static void DeleteNinjaWithKeyValue()
        {
            var keyval = 1;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);
                context.Ninjas.Remove(ninja);
                context.SaveChanges();
            }
        }

        private static void InsertNinjaWithEquipment()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = new Ninja
                {
                    Name = "Keith",
                    ServedInOniwaban = false,
                    DataOfBirth = new DateTime(2005, 11, 11),
                    ClanId = 1
                };
                var katana = new NinjaEquipment
                {
                    Name = "Katana",
                    Type = NinjaDomain.Classes.Enums.EquipmentType.Weapon
                };

                context.Ninjas.Add(ninja);
                ninja.EquipmentOwned.Add(katana);
                context.SaveChanges();
            }
        }

        private static void SimpleNinjaGraphQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                //var ninja = context.Ninjas
                //    .Include(n => n.EquipmentOwned).FirstOrDefault(n => n.Name.StartsWith("Keith"));
                var ninja = context.Ninjas
                    .FirstOrDefault(n => n.Name.StartsWith("Keith"));
                Console.WriteLine("Ninja retrieved: " + ninja.Name);
                context.Entry(ninja).Collection(n => n.EquipmentOwned).Load();
            }
        }
    }
}
