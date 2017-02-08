using System.Collections.Generic;

namespace MyQuizBackend.Classes
{
    public interface IVoteConnector {
        Dictionary<int,SocketHandler> GetSocketHandlers();
        void AddSocketHandler(int id, SocketHandler sh);
        void RemoveSocketHandler(int id);
    }

    public class VoteConnector : IVoteConnector {
        private readonly Dictionary<int,SocketHandler> _socketHandlers;

        public VoteConnector() {
            _socketHandlers = new Dictionary<int, SocketHandler>();
        }

        public Dictionary<int,SocketHandler> GetSocketHandlers() { return _socketHandlers; }
        public void AddSocketHandler(int id, SocketHandler sh) { _socketHandlers.Add(id,sh); }
        public void RemoveSocketHandler(int id) { _socketHandlers.Remove(id); }
    }
}