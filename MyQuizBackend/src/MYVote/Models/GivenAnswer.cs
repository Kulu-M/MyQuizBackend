using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class GivenAnswer
    {
        public GivenAnswer()
        {
            AnswerOptionGivenAnswer = new HashSet<AnswerOptionGivenAnswer>();
            FinalQuestionGivenAnswer = new HashSet<FinalQuestionGivenAnswer>();
        }

        [Column("ID")]
        public long Id { get; set; }
        [Column("GroupID")]
        public long? GroupId { get; set; }
        [Column("QuestionID")]
        public long? QuestionId { get; set; }
        [Column("FinalQuestionID")]
        public long? FinalQuestionId { get; set; }
        public string DateNow { get; set; }

        [InverseProperty("GivenAnswer")]
        public virtual ICollection<AnswerOptionGivenAnswer> AnswerOptionGivenAnswer { get; set; }
        [InverseProperty("GivenAnswer")]
        public virtual ICollection<FinalQuestionGivenAnswer> FinalQuestionGivenAnswer { get; set; }
        [ForeignKey("FinalQuestionId")]
        [InverseProperty("GivenAnswer")]
        public virtual FinalQuestion FinalQuestion { get; set; }
        [ForeignKey("GroupId")]
        [InverseProperty("GivenAnswer")]
        public virtual Group Group { get; set; }
        [ForeignKey("QuestionId")]
        [InverseProperty("GivenAnswer")]
        public virtual Question Question { get; set; }
    }
}
