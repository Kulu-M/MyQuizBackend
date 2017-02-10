using System;
using System.Net.WebSockets;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace MyQuizBackend.Classes
{
    public struct SocketInit{
        public int SurveyId {get;set;}
        public int TimeStamp {get;set;}
    }
    public class SocketHandler {
        private readonly UTF8Encoding _encoder = new UTF8Encoding();
        private readonly IVoteConnector _voteConnector;
        private readonly WebSocket socket;
        public CancellationTokenSource tokenSource;
        private SocketInit InitObject;
        public Queue<string> MessageQueue {get;set;}
        
        private SocketHandler(WebSocket socket, HttpContext context) {
            this.socket = socket;            
            tokenSource = new CancellationTokenSource();
            MessageQueue = new Queue<string>();
            _voteConnector = context.RequestServices.GetService<IVoteConnector>();            

            var pathString = context.Request.Path.ToString();
            var splitPath = pathString.Split('/');

            InitObject = new SocketInit {
                SurveyId = int.Parse(splitPath[splitPath.Count() - 2]),
                TimeStamp = int.Parse(splitPath[splitPath.Count() - 1])
            };

            if(_voteConnector.GetSocketHandlers().ContainsKey(InitObject.SurveyId)) throw new Exception($"SurveyId {InitObject.SurveyId} already in voteconnector");
            _voteConnector.AddSocketHandler(InitObject.SurveyId, this);

            var endTime = InitObject.TimeStamp;
            var nowTime = Time.ConvertToUnixTimestamp(DateTime.Now);
            var timeToDieInMilliseconds = (int)(endTime - nowTime) * 1000 + 5000;
            Console.WriteLine($"Time to Die {timeToDieInMilliseconds / 1000} seconds");
            if(timeToDieInMilliseconds < 0) throw new Exception($"Time is too low: {timeToDieInMilliseconds} => SurveyId: {InitObject.SurveyId}");
            
            new Timer(_ => { 
                Console.WriteLine("TimeOut");
                tokenSource.Cancel();
            }, null, timeToDieInMilliseconds, Timeout.Infinite);                          
        }

        private async Task EchoLoop() {
            while (true) {                
                if(tokenSource.Token.IsCancellationRequested) break;
                if(MessageQueue.Count > 0) {
                    var messageToSend = MessageQueue.Dequeue();
                    await SendGivenAnswer(messageToSend); 
                }
            }
            await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            _voteConnector.RemoveSocketHandler(InitObject.SurveyId);
        }

        public async Task SendGivenAnswer(string json) {
            var segment = new ArraySegment<byte>(_encoder.GetBytes(json));
            if(json.Length > segment.Count) throw new Exception($"String length and buffer size do not match {json.Length} != {segment.Count}");
            await socket.SendAsync(segment, WebSocketMessageType.Text, true, tokenSource.Token);
        }

        public static void Map(IApplicationBuilder app) {
            app.UseWebSockets();
            app.Use(Acceptor);
        }

        private static async Task Acceptor(HttpContext hc, Func<Task> n) {
            if (!hc.WebSockets.IsWebSocketRequest) return; 
            try {
                var socket = await hc.WebSockets.AcceptWebSocketAsync();
                var h = new SocketHandler(socket, hc);
                await h.EchoLoop();            
            } catch (OperationCanceledException) {
                // Do nothing
            }
        }
    }    
}