using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Template.Models;

namespace Template
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EditField>()
                .HasIndex(u => u.UniqeKey)
                .IsUnique();
        }

        public DbSet<EditField> EditFields { get; set; }
    }
}
