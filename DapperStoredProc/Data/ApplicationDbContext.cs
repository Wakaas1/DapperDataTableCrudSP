using DapperStoredProc.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Data
{
    public class ApplicationDbContext : DbContext
        
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options): base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Subjects> Subject { get; set; }

    }
}
