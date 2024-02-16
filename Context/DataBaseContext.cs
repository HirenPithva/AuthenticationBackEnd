using Microsoft.EntityFrameworkCore;
using tokenAuth.Model;

namespace tokenAuth.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> option):base(option) 
        {

        }

        public DbSet<users> users { get; set; }
    }
}
