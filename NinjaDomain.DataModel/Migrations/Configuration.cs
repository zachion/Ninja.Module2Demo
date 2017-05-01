namespace NinjaDomain.DataModel.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NinjaDomain.DataModel.NinjaContext>
    {
        // set project to ninjaDomain.DataModel
        // 1) run "enable-migrations" to pupulate this file
        // 2) add-migration 'Initial' to create migration file 
        // 3) update-database -script to create a sctipt file
        // 4) update-database -verbose to create db but view output
        // 5) add-migration 'addedBirthdayToNinja' create migrations for new changes
        // 6) update-database -verbose to uppdate db but view output
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NinjaDomain.DataModel.NinjaContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
