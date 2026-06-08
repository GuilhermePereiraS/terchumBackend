using terchum.model;

namespace terchum.service;

public class MessageService(MessageRepository repository)
{
    private readonly MessageRepository _repository = repository;

    public async Task SaveMessage(Message Message)
    {
        await _repository.SaveMessage(Message);
    }
}