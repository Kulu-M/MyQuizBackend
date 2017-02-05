using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYVote.Models
{
    public partial class Question
    {
        public List<AnswerOption> AnswerOptions = new List<AnswerOption>();
    }
}
