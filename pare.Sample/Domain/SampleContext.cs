using Microsoft.EntityFrameworkCore;
using pare.EntityFrameworkCore;
using pare.Sample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pare.Sample.Domain
{
    public partial class SampleContext : DbContext, IDbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options) : base(options) { }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Product> Products { get; set; }
    }
}
