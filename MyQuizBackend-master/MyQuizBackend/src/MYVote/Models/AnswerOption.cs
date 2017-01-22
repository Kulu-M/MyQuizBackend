using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class AnswerOption
    {
        [Column("ID")]
        public long Id { get; set; }
        public string Text { get; set; }
        [Column("True/False")]
        public string TrueFalse { get; set; }
    }
}
