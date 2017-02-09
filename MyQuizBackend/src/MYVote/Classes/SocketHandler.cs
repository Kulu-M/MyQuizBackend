using System;
using System.Net.WebSockets;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MYVote.Models;
using System.Collections.Generic;

namespace MyQuizBackend.Classes
{
    public class SocketHandler {
        public const int BufferSize = 1024;
        private readonly UTF8Encoding _encoder = new UTF8Encoding();
        private readonly IVoteConnector _voteConnector;
        private readonly Dictionary<int,WebSocket> sockets = new Dictionary<int,WebSocket> ();
        public bool _finished;
        private int _surveyId;
        public Queue<string> MessageQueue {get;set;}

        private SocketHandler(WebSocket socket, HttpContext context) {
            MessageQueue = new Queue<string>();
            var path = context.Request.Path.ToString();
            path = path.Replace("/", "");
            int.TryParse(path, out _surveyId);
            
            sockets.Add(_surveyId,socket);
            _voteConnector = context.RequestServices.GetService<IVoteConnector>();
            if(_voteConnector.GetSocketHandlers().ContainsKey(_surveyId))
                _voteConnector.RemoveSocketHandler(_surveyId);     
            _voteConnector.AddSocketHandler(_surveyId, this);
            // get timestamp for survey and set time to die respectively

            using (var db = new EF_DB_Context() ){
                var timestamp = (from g in db.GivenAnswer where g.SurveyId == _surveyId && !string.IsNullOrWhiteSpace(g.TimeStamp) select g.TimeStamp).First();
                var end = int.Parse(timestamp);
                var now = Time.ConvertToUnixTimestamp(DateTime.Now);
                var timeToDieInMilliseconds = (int)(end - now)*1000 + 60000;
                new Timer(_ => { 
                    _finished = true; 
                    MessageQueue.Clear();
                    _voteConnector.RemoveSocketHandler(_surveyId);
                    }, null, timeToDieInMilliseconds, Timeout.Infinite);                
            }          
            // WaitForClose();  
        }

        // private async void WaitForClose() {
        //     var stringBuffer = new ArraySegment<byte>(new byte[BufferSize]);       
        //     if(socket.State == WebSocketState.Open)   {
      
        //     var result = await socket.ReceiveAsync(stringBuffer, CancellationToken.None);
        //     if (result.MessageType == WebSocketMessageType.Close) {
        //         await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        //     }
        //     }     

        // }
        private static async Task Acceptor(HttpContext hc, Func<Task> n) {
            if (!hc.WebSockets.IsWebSocketRequest) return;
            var socket = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new SocketHandler(socket, hc);
            await h.EchoLoop();
        }

        private async Task EchoLoop() {
            while (sockets[_surveyId].State == WebSocketState.Open) {
                if(_finished) {  
                    break;   
                }
                await Task.Delay(100);
                while(MessageQueue.Count > 0) {
                    var toSend = MessageQueue.Dequeue();
                    await SendGivenAnswer(toSend);
                }  
            }
            // Remove SocketHandler from VoteConnector after closing       
        }

        public async Task SendGivenAnswer(string json) {
            var buffer = _encoder.GetBytes(json);
            var segment = new ArraySegment<byte>(buffer);
            var stringL = json.Length;
            var segL = segment.Count;
            if(stringL != segL) {
                var x=1;
            }

            await sockets[_surveyId].SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public static void Map(IApplicationBuilder app) {
            app.UseWebSockets();
            app.Use(Acceptor);
        }
    }

    
}