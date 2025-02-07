using Microsoft.EntityFrameworkCore;
using ByteMe.API.Data.Entities;
using System.Collections.Generic;

namespace ByteMe.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<Game> Games { get; set; }
    }
}
