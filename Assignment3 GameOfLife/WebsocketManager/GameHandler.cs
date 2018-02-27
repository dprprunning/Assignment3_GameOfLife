using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment3_GameOfLife.WebSocketManager;

namespace Assignment3_GameOfLife.WebsocketManager
{
    public class GameHandler : WebSocketHandler
    {
        public GameHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {

        }

    }
}
