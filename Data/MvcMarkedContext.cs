using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcMarked.Models;

namespace MvcMarked.Data
{
    public class MvcMarkedContext : DbContext
    {
        public MvcMarkedContext(DbContextOptions<MvcMarkedContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}
