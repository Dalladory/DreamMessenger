using Base.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data
{
    internal class AppDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer(@"Server=194.44.93.225;Database=DreamMessenger;User Id=test;Password=Qwerty-1;MultipleActiveResultSets =True;");
              
                string connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ConnectionString.txt");
                optionsBuilder.UseSqlServer(File.ReadAllText(connectionString));
                   
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ErrorsLog> ErrorsLogs  { get; set; }

    }
}
