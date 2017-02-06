using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MyQuizBackend.Classes
{
    public static class GlobalSocketContainer
    {
        public static SocketHandler GlobalSocketHandler;
    }

    public class SocketHandler
    {
        public const int BufferSize = 4096;
        
        WebSocket socket;

        public static string outgoingString;

        SocketHandler(WebSocket socket)
        {
            this.socket = socket;
            GlobalSocketContainer.GlobalSocketHandler = this;
        }

        static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            //Every Incoming Websocket Request lands here
            if (!hc.WebSockets.IsWebSocketRequest) return;
            var socket = await hc.WebSockets.AcceptWebSocketAsync();
            new SocketHandler(socket);
            await GlobalSocketContainer.GlobalSocketHandler.EchoLoop();
        }

        public async Task EchoLoop()
        {
            var buffer = new byte[BufferSize];

            //Code not safe
            while (this.socket.State == WebSocketState.Open)
            {
                //if (!string.IsNullOrWhiteSpace(outgoingString))
                //{
                //    var outgoing = new ArraySegment<byte>(buffer, 0, outgoingString.Length);
                //    await this.socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
                //    outgoingString = "";
                //}
            }
        }

        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(SocketHandler.Acceptor);
        }

        public async void SendViaSocket(string toSend)
        {
            outgoingString = toSend;
            await GlobalSocketContainer.GlobalSocketHandler.EchoLoop();
        }
    }
}
