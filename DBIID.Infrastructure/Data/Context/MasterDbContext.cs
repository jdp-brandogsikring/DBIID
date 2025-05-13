using DBIID.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DBIID.Infrastructure.Data.Context
{
    public class MasterDbContext : DbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options)
        : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<OtpTransaction> OtpTransactions { get; set; }
        public DbSet<Domain.Entities.Application> Applications { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<LinkUserCompany> LinkUserCompanies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LinkUserCompany>()
                  .HasKey(m => new { m.UserId, m.CompanyId});
        }
    }
}
