using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class QuestionBlock
    {
        public QuestionBlock()
        {
            FinalQuestion = new HashSet<FinalQuestion>();
            QuestionQuestionBlock = new HashSet<QuestionQuestionBlock>();
        }

        [Column("ID")]
        public long Id { get; set; }
        public string Title { get; set; }

        [InverseProperty("QuestionBlockNavigation")]
        public virtual ICollection<FinalQuestion> FinalQuestion { get; set; }
        [InverseProperty("QuestionBlock")]
        public virtual ICollection<QuestionQuestionBlock> QuestionQuestionBlock { get; set; }
    }
}
