using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MyQuizBackend.Classes {
    public class SocketHandler {
        public const int BufferSize = 4096;
        private readonly UTF8Encoding _encoder = new UTF8Encoding();
        private readonly IVoteConnector _voteConnector;

        private readonly WebSocket socket;
        private bool _finished;

        private int _surveyId;
        private SocketHandler(WebSocket socket, HttpContext context) {
            this.socket = socket;
            var path = context.Request.Path.ToString();
            path = path.Replace("/", "");
            int.TryParse(path, out _surveyId);
            _voteConnector = context.RequestServices.GetService<IVoteConnector>();
            _voteConnector.AddSocketHandler(_surveyId, this);
        }

        private static async Task Acceptor(HttpContext hc, Func<Task> n) {
            if (!hc.WebSockets.IsWebSocketRequest) {
                return;
            }

            var socket = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new SocketHandler(socket, hc);
            await h.EchoLoop();
        }

        private async Task EchoLoop() {
            while (socket.State == WebSocketState.Open) {
                // check every half second if socket is still open
                await Task.Delay(500);
            }
            _voteConnector.RemoveSocketHandler(_surveyId);
        }

        public async Task SendGivenAnswer(string json) {
            var buffer = _encoder.GetBytes(json);
            await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public static void Map(IApplicationBuilder app) {
            app.UseWebSockets();
            app.Use(Acceptor);
        }
    }
}