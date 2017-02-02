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
        [Column("DeviceID")]
        public long DeviceId { get; set; }
        [Column("GroupID")]
        public long GroupId { get; set; }
        [Column("SingleTopicID")]
        public long SingleTopicId { get; set; }
        [Column("QuestionBlockID")]
        public long QuestionBlockId { get; set; }
        [Column("QuestionID")]
        public long QuestionId { get; set; }
        [Column("AnswerOptionID")]
        public long AnswerOptionId { get; set; }
        public string TimeStamp { get; set; }
    }
}
