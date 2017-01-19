using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class Question
    {
        public Question()
        {
            GivenAnswer = new HashSet<GivenAnswer>();
            QuestionAnswerOption = new HashSet<QuestionAnswerOption>();
        }

        [Column("ID")]
        public long Id { get; set; }
        public string Text { get; set; }

        [InverseProperty("Question")]
        public virtual ICollection<GivenAnswer> GivenAnswer { get; set; }
        [InverseProperty("Question")]
        public virtual ICollection<QuestionAnswerOption> QuestionAnswerOption { get; set; }
        [InverseProperty("QuestionNavigation")]
        public virtual QuestionQuestionBlock QuestionQuestionBlock { get; set; }
        [ForeignKey("Id")]
        [InverseProperty("Question")]
        public virtual QuestionQuestionBlock IdNavigation { get; set; }
    }
}
