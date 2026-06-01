namespace terchum.model;

public class MessageBoard(string room)
{
    private long? Id { get; set; }
    public string Name { get; set; } = room;
}