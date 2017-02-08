using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYVote.Models
{
    public partial class Question
    {
        public List<AnswerOption> AnswerOptions = new List<AnswerOption>();

        public void fillValues()
        {
            using (var db = new EF_DB_Context())
            {
                var answerOptionsFinder = from a in db.QuestionAnswerOption
                                          where a.QuestionId == Id
                                          select a.AnswerOptionId;

                AnswerOptions = db.AnswerOption.Where(a => answerOptionsFinder.Any(a2 => a2 == a.Id)).ToList();
            }
        }
    }
}
