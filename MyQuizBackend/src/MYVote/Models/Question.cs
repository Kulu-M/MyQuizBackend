using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class Question
    {
        [Column("ID")]
        public long Id { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }
}
