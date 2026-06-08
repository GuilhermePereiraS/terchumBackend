using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using terchum.service;

namespace terchum.ws;


public static class WebSocketController
{

    private class Connection
    {
        public string ConnectionId { get; set; }
        public WebSocket WebSocket { get; set; }
    }
    
    public static void Configure(WebApplication app)
    {
        app.Map("/ws/{room}", async (HttpContext context, RoomManager roomManager, MessageBoardService messageBoardService, string room) =>
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = 400;
                return;
            }

            var webSocket = await context.WebSockets.AcceptWebSocketAsync();

            roomManager.AddConnection(room, webSocket);

            await HandleConnection(webSocket, roomManager, messageBoardService, room, GetClientId(context));
        });
    }

    private static string GetClientId(HttpContext context)
    {
        var clientId = context.Request.Cookies["clientId"];

        if (clientId == null)
        {
            clientId = Guid.NewGuid().ToString();
                
            context.Response.Cookies.Append(
                "clientId", 
                clientId, 
                new CookieOptions{HttpOnly = true, Expires = DateTimeOffset.UtcNow.AddYears(1)}
            );
        } return clientId;
    }

    private static async Task HandleConnection(WebSocket webSocket, RoomManager roomManager, MessageBoardService messageBoardService, string room, string clientId)
    {
        var buffer = new byte[1024];

        await messageBoardService.SaveRoomInDbIfNotExists(room);
        
        loadMessages(webSocket); 

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Fechado",
                    CancellationToken.None);
                
                break;
            }
            
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

            saveMessage(message, clientId);
            
            await Broadcast(roomManager, room, message);
        }
    }

    private static void saveMessage(string message, string clientId)
    {
        throw new NotImplementedException();
    }

    private static void loadMessages(WebSocket webSocket)
    {
      
    }

    private static void saveRoomInDbIfNotExists(string room)
    {
        throw new NotImplementedException();
    }

    private static async Task Broadcast(RoomManager roomManager, string roomName, string message)
    {
        ConcurrentBag<WebSocket> sockets = roomManager.GetRoom(roomName);

        if (sockets == null) return;
        
        var bytes = Encoding.UTF8.GetBytes(message);
        foreach (var socket in sockets)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

}

/* TODO*
    1. Salvamento e criação dinamica de salas
    2. Salvar mensagens do usuario especifico
    2. Carregar mensagens do db
*/