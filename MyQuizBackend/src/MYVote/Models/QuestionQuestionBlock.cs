using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    [Table("Question_QuestionBlock")]
    public partial class QuestionQuestionBlock
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("QuestionBlockID")]
        public long? QuestionBlockId { get; set; }
        [Column("QuestionID")]
        public long? QuestionId { get; set; }
    }
}
