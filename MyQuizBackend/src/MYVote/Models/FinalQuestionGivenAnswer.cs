using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    [Table("FinalQuestion_GivenAnswer")]
    public partial class FinalQuestionGivenAnswer
    {
        [Column("FinalQuestionID")]
        [Key]
        public long FinalQuestionId { get; set; }
        [Column("GivenAnswerID")]
        public long GivenAnswerId { get; set; }

        [ForeignKey("FinalQuestionId")]
        [InverseProperty("FinalQuestionGivenAnswer")]
        public virtual FinalQuestion FinalQuestion { get; set; }
        [ForeignKey("GivenAnswerId")]
        [InverseProperty("FinalQuestionGivenAnswer")]
        public virtual GivenAnswer GivenAnswer { get; set; }
    }
}
