using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MYVote.Models
{
    public partial class QuestionBlock
    {
        public List<Question> Questions = new List<Question>();

        public void fillWithValues()
        {
            using (var db = new EF_DB_Context())
            {
                var questionsFinder =
                    from q in db.QuestionQuestionBlock
                    where q.QuestionBlockId == Id
                    select q.QuestionId;

                var questionList = db.Question.Where(q => questionsFinder.Any(q2 => q2 == q.Id));

                foreach (var question in questionList)
                {
                    question.fillValues();
                }
                Questions = questionList.ToList();
            }
        }
    }
}
