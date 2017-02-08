using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYVote.Models
{
    public partial class GivenAnswer
    {
        public Device Device;
        public Group Group;
        public SingleTopic SingleTopic;
        public AnswerOption AnswerOption;
        public Question Question;
        public QuestionBlock QuestionBlock;

        public void fillValues()
        {
            using (var db = new EF_DB_Context())
            {
                Device = db.Device.FirstOrDefault(d => d.Id == DeviceId);
                Group = db.Group.FirstOrDefault(g => g.Id == GroupId);
                SingleTopic = db.SingleTopic.FirstOrDefault(st => st.Id == SingleTopicId);
                AnswerOption = db.AnswerOption.FirstOrDefault(ao => ao.Id == AnswerOptionId);
                Question = db.Question.FirstOrDefault(q => q.Id == QuestionId);
                QuestionBlock = db.QuestionBlock.FirstOrDefault(qb => qb.Id == QuestionBlockId);
                QuestionBlock?.fillWithValues();
            }
        }

        public void fillIds()
        {
            if (Device != null)
            {
                DeviceId = Device.Id;
            }
            if (Group != null)
            {
                GroupId = Group.Id;
            }
            if (SingleTopic != null)
            {
                SingleTopicId = SingleTopic.Id;
            }
            if (AnswerOption != null)
            {
                AnswerOptionId = AnswerOption.Id;
            }
            if (Question != null)
            {
                QuestionId = Question.Id;
            }
            if (QuestionBlock != null)
            {
                QuestionBlockId = QuestionBlock.Id;
            }
        }
    }

    
}
