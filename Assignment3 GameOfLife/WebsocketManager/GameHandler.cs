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
            
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {

            var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);



            //   //this is all test code so feel free to change or delete this
            //   //if message is true or false, send it out to all browsers
            //   //so that all instances of those browsers can update to current state
            //    if (message == "true" || message == "false")
            //    {
            //        await SendMessageToAllAsync(message);
            //    }
            //    //else send following messages out to all browsers
            //    //this could be changed to: 
            //    //else grab message from client side, send to server, 
            //    //then grab server side info and send back out 
            //    else
            //    {
            //        await SendMessageToAllAsync(message);
            //        await SendMessageToAllAsync("00,01,02");
            //        Thread.Sleep(100);
            //        await SendMessageToAllAsync("10,11,12,13,03");
            //        Thread.Sleep(100);
            //        await SendMessageToAllAsync("30,31,32,33,34");
            //        Thread.Sleep(100);
            //        await SendMessageToAllAsync("50,51,52,53,54,55,46,36,26,16,06");
            //    }
            ////GameLogic.
        }

    }
}
