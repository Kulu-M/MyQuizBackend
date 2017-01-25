using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    [Table("Topic_SingleTopic")]
    public partial class TopicSingleTopic
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("TopicID")]
        public long TopicId { get; set; }
        [Column("SingleTopicID")]
        public long SingleTopicId { get; set; }
    }
}
