using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    [Table("Topic_SingleTopic")]
    public partial class TopicSingleTopic
    {
        [Column("TopicID")]
        [Key]
        public long TopicId { get; set; }
        [Column("SingleTopicID")]
        public long SingleTopicId { get; set; }

        [InverseProperty("IdNavigation")]
        public virtual Topic Topic { get; set; }
        [ForeignKey("SingleTopicId")]
        [InverseProperty("TopicSingleTopic")]
        public virtual SingleTopic SingleTopic { get; set; }
        [ForeignKey("TopicId")]
        [InverseProperty("TopicSingleTopic")]
        public virtual Topic TopicNavigation { get; set; }
    }
}
