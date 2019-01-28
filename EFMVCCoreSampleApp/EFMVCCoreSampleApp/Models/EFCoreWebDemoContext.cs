using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFMVCCoreSampleApp.Models
{
    public class EFCoreWebDemoContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string ConnectionString = @"Server=(localdb)\ProjectsV13;Database=EFCoreWebDemo;Trusted_Connection=True;";
            optionsBuilder.UseSqlServer(ConnectionString);

        }
    }
}
