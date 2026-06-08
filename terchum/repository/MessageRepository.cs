using Microsoft.EntityFrameworkCore;
using terchum.model;

namespace terchum.service;

public class MessageRepository(AppDbContext dbContext)
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task SaveMessage(Message message)
    {
        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();
    }
    
}