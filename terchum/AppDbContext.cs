using Microsoft.EntityFrameworkCore;
using terchum.model;

namespace terchum;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Usuarios { get; set; }
    
}