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
        private int _remainingTime;

        private int _surveyId;
        private SocketHandler(WebSocket socket, HttpContext context) {
            this.socket = socket;
            var path = context.Request.Path.ToString();
            var test = context.Request.Path.ToUriComponent();
            path = path.Replace("/", "");
            int.TryParse(path, out _surveyId);
            _voteConnector = context.RequestServices.GetService<IVoteConnector>();
            _voteConnector.AddSocketHandler(_surveyId, this);
            _remainingTime = 30;
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
                //await SendGivenAnswer(_remainingTime.ToString());
                if (_finished) {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    _voteConnector.RemoveSocketHandler(_surveyId);
                }
                await Task.Delay(1000);
                _remainingTime -= 1;
                if (_remainingTime <= 0) {
                    _finished = true;
                }
            }
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