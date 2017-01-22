using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    [Table("AnswerOption_GivenAnswer")]
    public partial class AnswerOptionGivenAnswer
    {
        [Column("AnswerOptionID")]
        [Key]
        public long AnswerOptionId { get; set; }
        [Column("GivenAnswerID")]
        public long GivenAnswerId { get; set; }
    }
}
