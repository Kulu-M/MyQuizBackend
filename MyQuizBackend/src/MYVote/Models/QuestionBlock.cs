using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class QuestionBlock
    {
        [DefaultValue(0)]
        [Column("ID")]
        public long Id { get; set; }
        public string Title { get; set; }
    }
}
