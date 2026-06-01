using Microsoft.EntityFrameworkCore;
using terchum.model;

namespace terchum;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Usuarios { get; set; }
    public DbSet<Message> Messages { get; set; }  
    public DbSet<MessageBoard> MessageBoard { get; set; }
    
}