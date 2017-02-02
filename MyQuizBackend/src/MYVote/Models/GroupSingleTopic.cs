using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    [Table("Group_SingleTopic")]
    public partial class GroupSingleTopic
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("SingleTopicID")]
        public long? SingleTopicId { get; set; }
        [Column("GroupID")]
        public long? GroupId { get; set; }
    }
}
