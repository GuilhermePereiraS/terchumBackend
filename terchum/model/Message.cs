namespace terchum.model;

public class Message
{
    public long? Id { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public long UserId { get; set; }
    public User User { get; set; } = null!;
}

/* todo essa estrutura não vai funcionar direito, como não tem login não vai precisar salvar o usuario no banco, so o cookie,
    ou alterar o usuario pra não ser uma entidade "gerenciada" 
 */   