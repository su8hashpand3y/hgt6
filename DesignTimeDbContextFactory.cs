using HGT6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HGT6
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HGTDbContext>
    {
        public HGTDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<HGTDbContext>();

            var connectionString = configuration.GetConnectionString("HGTDB");

            builder.UseSqlServer(connectionString);

            return new HGTDbContext(builder.Options);
        }
    }
}
