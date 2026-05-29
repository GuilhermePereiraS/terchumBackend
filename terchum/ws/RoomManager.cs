using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace terchum.ws;

public class RoomManager
{
    private readonly ConcurrentDictionary<string, ConcurrentBag<WebSocket>> _rooms = new();

    public void AddConnection(string roomName, WebSocket webSocket)
    {
        ConcurrentBag<WebSocket> room = _rooms.GetOrAdd(roomName, _=> new ConcurrentBag<WebSocket>());
        
        room.Add(webSocket);
    }
}