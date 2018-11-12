using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Warehouse.Entities;

namespace Warehouse.Data.Data
{
    public partial class IdentityDbContext : DbContext
    {
        public IdentityDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }

    }
}
