using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class FinalQuestion
    {
        public FinalQuestion()
        {
            GivenAnswer = new HashSet<GivenAnswer>();
        }

        [Column("ID")]
        public long Id { get; set; }
        public long? QuestionBlock { get; set; }
        [Column("GroupID")]
        public long? GroupId { get; set; }
        public string DateEnd { get; set; }
        [Column("SingleTopicID")]
        public long? SingleTopicId { get; set; }

        [InverseProperty("FinalQuestion")]
        public virtual FinalQuestionGivenAnswer FinalQuestionGivenAnswer { get; set; }
        [InverseProperty("FinalQuestion")]
        public virtual ICollection<GivenAnswer> GivenAnswer { get; set; }
        [ForeignKey("GroupId")]
        [InverseProperty("FinalQuestion")]
        public virtual Group Group { get; set; }
        [ForeignKey("QuestionBlock")]
        [InverseProperty("FinalQuestion")]
        public virtual QuestionBlock QuestionBlockNavigation { get; set; }
        [ForeignKey("SingleTopicId")]
        [InverseProperty("FinalQuestion")]
        public virtual SingleTopic SingleTopic { get; set; }
    }
}
