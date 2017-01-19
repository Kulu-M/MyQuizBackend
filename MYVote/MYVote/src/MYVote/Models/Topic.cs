using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class Topic
    {
        public Topic()
        {
            Group = new HashSet<Group>();
        }

        [Column("ID")]
        public long Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("TopicNavigation")]
        public virtual ICollection<Group> Group { get; set; }
        [InverseProperty("TopicNavigation")]
        public virtual TopicSingleTopic TopicSingleTopic { get; set; }
        [ForeignKey("Id")]
        [InverseProperty("Topic")]
        public virtual TopicSingleTopic IdNavigation { get; set; }
    }
}
