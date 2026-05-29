using System.Net.WebSockets;

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
            }
    }

}