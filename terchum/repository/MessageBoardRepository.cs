using Microsoft.EntityFrameworkCore;
using terchum.model;

namespace terchum.service;

public class MessageBoardRepository(AppDbContext dbContext)
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task SaveMessageBoard(MessageBoard messageBoard)
    {
        await _dbContext.MessageBoard.AddAsync(messageBoard);
        await _dbContext.SaveChangesAsync();
    }

    public Task<bool> ExistMessageBoard(string room)
    {
        return _dbContext.MessageBoard.AnyAsync(board => board.Name == room);
    }
}