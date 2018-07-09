using HGT6.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGT6.Migrations
{
    public static class DatabaseSeeder
    { 
        public static IWebHost SeedData(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<HGTDbContext>();

                if (!context.Capthas.Any())
                {
                    var capthas = new List<Captha>
                                  {
                                    new Captha { CapthaText= "2+2", CapthaAnswer ="4"},
                                    new Captha { CapthaText= "2*3", CapthaAnswer ="6"},
                                    new Captha { CapthaText= "yes", CapthaAnswer ="yes"},
                                    new Captha { CapthaText= "12-2", CapthaAnswer ="10"},
                                    new Captha { CapthaText= "5*1", CapthaAnswer ="5"},
                                  };
                    context.AddRange(capthas);
                    context.SaveChanges();
                }
            }
            return host;
        }
    }
}
