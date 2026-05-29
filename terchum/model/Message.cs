namespace terchum.model;

public class Message
{
    private long? Id { get; set; }
    private String Content { get; set; }
    private DateTime Date { get; set; } = DateTime.Now;
}