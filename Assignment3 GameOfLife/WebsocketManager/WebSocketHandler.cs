using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment3_GameOfLife.WebSocketManager
{
    public abstract class WebSocketHandler
    {
        protected WebSocketConnectionManager WebSocketConnectionManager { get; set; }
        protected int playerCount;

        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
            try
            {
                WebSocketConnectionManager.AddSocket(socket);
                await UpdatePlayerCount();
            }
            catch (Exception)
            {
                return;
            }
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            try
            {
                await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));
                await UpdatePlayerCount();
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// updates the number of active sockets
        /// </summary>
        private async Task UpdatePlayerCount()
        {
            playerCount = 0;
            foreach (var pair in WebSocketConnectionManager.GetAll())
            {
                try
                {
                    if (pair.Value.State == WebSocketState.Open)
                        playerCount++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            string msg = "PLAYERCOUNT:" + playerCount;
            await SendMessageToAllAsync(msg);
        }

        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                  offset: 0,
                                                                  count: message.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageAsync(string socketId, string message)
        {
            await SendMessageAsync(WebSocketConnectionManager.GetSocketById(socketId), message);
        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in WebSocketConnectionManager.GetAll())
            {
                try
                {
                    if (pair.Value.State == WebSocketState.Open)
                        await SendMessageAsync(pair.Value, message);
                }
                catch (Exception ex)
                {
                    await WebSocketConnectionManager.RemoveSocket(pair.Key);
                    Console.WriteLine("ERROR IN SENDMESSAGETOALL: " + ex.Message);
                    continue;
                }
            }
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
