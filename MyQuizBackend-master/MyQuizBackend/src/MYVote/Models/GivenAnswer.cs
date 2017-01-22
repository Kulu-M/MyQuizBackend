using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class GivenAnswer
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("GroupID")]
        public long? GroupId { get; set; }
        [Column("QuestionID")]
        public long? QuestionId { get; set; }
        [Column("FinalQuestionID")]
        public long? FinalQuestionId { get; set; }
        public string DateNow { get; set; }
    }
}
