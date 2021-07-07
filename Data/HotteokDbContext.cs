using HotteokChatBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotteokChatBot.Data
{
    public class HotteokDbContext : DbContext
    {
        public HotteokDbContext()
        {

        }

        public HotteokDbContext(DbContextOptions<HotteokDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));

            optionsBuilder.UseMySql("Server=localhost;Port=3306;Database=Hotteok;Uid=root;Pwd=1234;", serverVersion);
        }

        public DbSet<User> User { get; set; }
        public DbSet<Blacklist> Blacklist { get; set; }
        public DbSet<Codetable> Codetable { get; set; }
        public DbSet<Commands> Commands { get; set; }
        public DbSet<CommandsDetail> CommandsDetail { get; set; }

    }
}
