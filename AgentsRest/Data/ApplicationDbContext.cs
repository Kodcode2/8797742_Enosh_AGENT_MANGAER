using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;

namespace UserApi.Data
    {
        public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : DbContext(options)
        {

            public DbSet<AgentModel> Agents { get; set; }
            public DbSet<TargetModel> Targets { get; set; }
            public DbSet<MissionModel> Missions { get; set; }





        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgentModel>()
                .Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<TargetModel>()
                .Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<MissionModel>()
                .Property(m => m.MissionStatus)
                .HasConversion<string>()
                .IsRequired();




            modelBuilder.Entity<MissionModel>()
                .HasOne(x => x.Agent)
                .WithMany(a => a.Missions)
                .HasForeignKey(x => x.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MissionModel>()
                .HasOne(x => x.Target)
                .WithMany()
                .HasForeignKey(x => x.TargetId)
                .OnDelete(DeleteBehavior.Restrict);

            

            modelBuilder.Entity<KillModel>()
                .HasOne(x => x.Agent)
                .WithMany()
                .HasForeignKey(x => x.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<KillModel>()
                .HasOne(x => x.Target)
                .WithMany()
                .HasForeignKey(x => x.TargetId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

                
        }
    }
    }



