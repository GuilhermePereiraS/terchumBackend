using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace terchum.ws;

public static class WebSocketController
{
    public static void Configure(WebApplication app)
    {
        app.Map("/ws/{room}", async (HttpContext context, RoomManager roomManager, string room) =>
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = 400;
                return;
            }

            var webSocket = await context.WebSockets.AcceptWebSocketAsync();

            roomManager.AddConnection(room, webSocket);

            await HandleConnection(webSocket, roomManager, room);
        });
    }


private static async Task HandleConnection(WebSocket webSocket, RoomManager roomManager, string room)
    {
        var buffer = new byte[1024];

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
            
            await Broadcast(roomManager, room, message);
        }
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
    2. Carregar mensagens do db
*/