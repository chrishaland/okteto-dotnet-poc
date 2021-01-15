using Microsoft.EntityFrameworkCore;

namespace okteto_dotnet_poc
{
    public class Database : DbContext
    {
        public Database(DbContextOptions options) : base(options)
        {
            
        }
    }
}