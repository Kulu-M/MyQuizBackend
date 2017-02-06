using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace MyQuizBackend.Classes {
    public class ReceiveAnswerMiddleware {
        private readonly RequestDelegate _next;
        private readonly IVoteConnector _voteConnector;

        public ReceiveAnswerMiddleware(RequestDelegate next, IVoteConnector ivc) {
            _next = next;
            _voteConnector = ivc;
        }

        public async Task Invoke(HttpContext context) {
            if (context.Request.Path == new PathString("/api/givenanswer/") && context.Request.Method == "POST") {
                var bodyStr = "";
                var req = context.Request;
                req.EnableRewind();
                using (var reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true)) {
                    bodyStr = reader.ReadToEnd();
                }
                req.Body.Position = 0;

                var socketHandler = _voteConnector.GetSocketHandlers().First();
                await socketHandler.SendGivenAnswer(bodyStr);
            }
            await _next(context);
        }
    }

    public interface IVoteConnector {
        List<SocketHandler> GetSocketHandlers();
        void AddSocketHandler(SocketHandler sh);
        void RemoveSocketHandler(SocketHandler sh);
    }

    public class VoteConnector : IVoteConnector {
        private readonly List<SocketHandler> _socketHandlers = new List<SocketHandler>();

        public List<SocketHandler> GetSocketHandlers() { return _socketHandlers; }
        public void AddSocketHandler(SocketHandler sh) { _socketHandlers.Add(sh); }
        public void RemoveSocketHandler(SocketHandler sh) { _socketHandlers.Remove(sh); }
    }
}