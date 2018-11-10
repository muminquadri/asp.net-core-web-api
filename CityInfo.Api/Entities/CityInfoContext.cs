using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.Entities
{
    public class CityInfoContext:DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options)
               :base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<City> Cities { get; set; }

        public DbSet<PointOfInterest> PointsOfInterest { get; set; }
        
        //Connect the DbContext with the database
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("connectionstring");

        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
