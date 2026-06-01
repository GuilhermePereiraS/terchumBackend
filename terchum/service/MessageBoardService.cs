using terchum.model;

namespace terchum.service;

public class MessageBoardService(MessageBoardRepository repository)
{
    private readonly MessageBoardRepository _repository = repository;

    public async Task SaveRoomInDbIfNotExists(string room)
    {
        var existsInDb = await _repository.ExistMessageBoard(room);
        if (!existsInDb)
        {
            MessageBoard messageBoard = new MessageBoard(room);
            await _repository.SaveMessageBoard(messageBoard);
        }
    }
}