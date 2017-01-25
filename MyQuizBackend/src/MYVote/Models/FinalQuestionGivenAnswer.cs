using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    [Table("FinalQuestion_GivenAnswer")]
    public partial class FinalQuestionGivenAnswer
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("FinalQuestionID")]
        public long FinalQuestionId { get; set; }
        [Column("GivenAnswerID")]
        public long GivenAnswerId { get; set; }
    }
}
