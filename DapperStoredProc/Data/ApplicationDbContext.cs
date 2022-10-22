using DapperStoredProc.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperStoredProc.DTO;

namespace DapperStoredProc.Data
{
    public class ApplicationDbContext : DbContext
        
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options): base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Subjects> Subject { get; set; }
        
        //public DbSet<ImageUpload> img { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
        
        //public DbSet<ImageUpload> img { get; set; }
        public DbSet<DapperStoredProc.DTO.RegisterDto> RegisterDto { get; set; }
        
        //public DbSet<ImageUpload> img { get; set; }
        public DbSet<DapperStoredProc.DTO.LoginDto> LoginDto { get; set; }
    }
}
