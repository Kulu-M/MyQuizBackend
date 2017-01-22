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
    }
}
