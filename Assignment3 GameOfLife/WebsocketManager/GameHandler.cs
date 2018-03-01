using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Assignment3_GameOfLife.WebSocketManager;
using Assignment3_GameOfLife.GameLogic;

namespace Assignment3_GameOfLife
{
    public class GameHandler : WebSocketHandler
    {
        public GameHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
            Thread yourmom = new Thread(yourmomsloop);
            yourmom.Start();
        }

        public void yourmomsloop()
        {
            while (true)
            {
                Thread.Sleep(100);
                Task.Run(async () =>
                {
                    await SendMessageToAllAsync("Your mom");
                });
            }
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {

            var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
            //GameLogic.
        }

    }
}
