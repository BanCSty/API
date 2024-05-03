
namespace API.DAL
{
    public class DbInitializer
    {
        public static void Initialize(ApiDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
