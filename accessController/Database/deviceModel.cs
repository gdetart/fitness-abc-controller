using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace accessController.Database
{
    public class Device
    {
        public int DeviceID { get; set; }
        public string IP { get; set; }
        public long SN { get; set; }
        public string name { get; set; }

    }
    public class Devicedb : DbContext
    {
        private static bool _created = false;
        public Devicedb()
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(@"Data Source=C:\sample.db");
        }

        public DbSet<Device> Devices { get; set; }


    }
}
