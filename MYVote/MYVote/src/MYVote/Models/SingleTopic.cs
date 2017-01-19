using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class SingleTopic
    {
        public SingleTopic()
        {
            FinalQuestion = new HashSet<FinalQuestion>();
            TopicSingleTopic = new HashSet<TopicSingleTopic>();
        }

        [Column("ID")]
        public long Id { get; set; }
        public string String { get; set; }

        [InverseProperty("SingleTopic")]
        public virtual ICollection<FinalQuestion> FinalQuestion { get; set; }
        [InverseProperty("SingleTopic")]
        public virtual ICollection<TopicSingleTopic> TopicSingleTopic { get; set; }
    }
}
