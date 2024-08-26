using UserApi.Data;

namespace AgentsRest.Data
{
    public class DbContextFactory
    {
        public static ApplicationDbContext CreateBdContext(IServiceProvider serviceProvider)
        {
            var scope  = serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
    }
}
