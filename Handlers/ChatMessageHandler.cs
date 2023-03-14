using System.Net.WebSockets;
using System.Text;
using ChatApplicationWebsockets.Exceptions;
using WebSocketLibrary;

namespace ChatApplicationWebsockets.Handlers
{
    public class ChatMessageHandler : WebSocketHandler
    {
        public ChatMessageHandler(ConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override async Task<bool> OnConnected(WebSocket socket, string userName)
        {
            bool result = default;
            try
            {
                result = await base.OnConnected(socket, userName);
                if (!result)
                {
                    throw new UserAlreadyExistException(userName);
                }
                var socketUserName = WebSocketConnectionManager.GetUserName(socket);
                await SendMessageToAllAsync($"{socketUserName} joined the chat!");
            }
            catch (UserAlreadyExistException ex)
            {
                return false;
            }

            return result;
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            if(socket.State != WebSocketState.Open)
                return;
            var socketUserName = WebSocketConnectionManager.GetUserName(socket);
            await base.OnDisconnected(socket);
            await SendMessageToAllAsync($"{socketUserName} left the chat!");
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketUserName = WebSocketConnectionManager.GetUserName(socket);
            var message = $"{socketUserName}:{Encoding.UTF8.GetString(buffer, 0, result.Count)}";

            await SendMessageToAllAsync(message);
        }
    }
}
