using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    [Table("Question_AnswerOption")]
    public partial class QuestionAnswerOption
    {
        [Column("QuestionID")]
        public long QuestionId { get; set; }
        [Column("AnswerOptionID")]
        [Key]
        public long AnswerOptionId { get; set; }

        [InverseProperty("Id1")]
        public virtual AnswerOption AnswerOption { get; set; }
        [ForeignKey("AnswerOptionId")]
        [InverseProperty("QuestionAnswerOption")]
        public virtual AnswerOption AnswerOptionNavigation { get; set; }
        [ForeignKey("QuestionId")]
        [InverseProperty("QuestionAnswerOption")]
        public virtual Question Question { get; set; }
    }
}
