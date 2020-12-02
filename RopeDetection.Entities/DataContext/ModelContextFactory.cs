using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RopeDetection.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.Entities.DataContext
{
    public class ModelContextFactory : IDesignTimeDbContextFactory<ModelContext>
    {
        public ModelContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ModelContext>();
            optionsBuilder.UseSqlServer(BaseProjectSettings.ConnectionString);

            return new ModelContext(optionsBuilder.Options);
        }
    }
}
