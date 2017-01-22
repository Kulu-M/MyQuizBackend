using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class FinalQuestion
    {
        [Column("ID")]
        public long Id { get; set; }
        public long? QuestionBlock { get; set; }
        [Column("GroupID")]
        public long? GroupId { get; set; }
        public string DateEnd { get; set; }
        [Column("SingleTopicID")]
        public long? SingleTopicId { get; set; }
    }
}
