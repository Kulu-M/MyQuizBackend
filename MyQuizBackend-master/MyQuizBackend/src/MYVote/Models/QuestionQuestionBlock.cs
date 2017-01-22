using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    [Table("Question_QuestionBlock")]
    public partial class QuestionQuestionBlock
    {
        [Column("QuestionID")]
        [Key]
        public long QuestionId { get; set; }
        [Column("QuestionBlockID")]
        public long QuestionBlockId { get; set; }
    }
}
