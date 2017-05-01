using System;
using System.Data.Entity;
using System.Linq;
using NinjaDomain.Classes;
using NinjaDomain.Classes.Interfaces;

namespace NinjaDomain.DataModel
{
    public class NinjaContext : DbContext
    {
        public DbSet<Ninja> Ninjas { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<NinjaEquipment> Equipment { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //isDirty to be used only by the client application not in EF queries
            modelBuilder.Types()
                .Configure(c=>c.Ignore("IsDirty"));
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            //overiding the saveChanges from the DbContext
            // to save DateModified and DateCreated dates
            foreach (var history in this.ChangeTracker.Entries()
                .Where(e => e.Entity is IModificationHistory &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified))
                .Select(e => e.Entity as IModificationHistory)
            )
            {
                if (history != null)
                {
                    history.DateModified = DateTime.Now;
                    if (history.DateCreated == DateTime.MinValue)
                    {
                        history.DateCreated = DateTime.Now;
                    }
                }
            }

            int result = base.SaveChanges();

            //if my application is relying on the isDirty i am setting it back to false.
            foreach (var history in this.ChangeTracker.Entries()
                .Where(e => e.Entity is IModificationHistory)
                .Select(e => e.Entity as IModificationHistory)
            )
            {
                if (history != null) history.IsDirty = false;
            }
            return result;
        }

    }
}
