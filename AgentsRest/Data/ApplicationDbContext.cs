using Microsoft.EntityFrameworkCore;
using UserDogs.Models;

namespace UserApi.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ,IConfiguration configuration) : DbContext(options)
    {

        public DbSet<UserModel> Users { get; set; }
        public DbSet<DogModel> Dogs { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasMany(x => x.Dogs)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
