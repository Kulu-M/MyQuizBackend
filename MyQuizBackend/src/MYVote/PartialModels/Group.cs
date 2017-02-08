using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYVote.Models
{
    public partial class Group
    {
        public List<SingleTopic> SingleTopics = new List<SingleTopic>();
        public int DeviceCount;
    }
}
