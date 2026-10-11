using Microsoft.EntityFrameworkCore; 

namespace OnlineStore.API.Data
{
    public class StoreDbContext : DbContext 
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
    }
}
