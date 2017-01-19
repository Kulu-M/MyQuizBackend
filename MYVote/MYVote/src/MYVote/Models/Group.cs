using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class Group
    {
        public Group()
        {
            DeviceGroup = new HashSet<DeviceGroup>();
            FinalQuestion = new HashSet<FinalQuestion>();
            GivenAnswer = new HashSet<GivenAnswer>();
        }

        [Column("ID")]
        public long Id { get; set; }
        public string Title { get; set; }
        public long? Topic { get; set; }
        [Column("EnterGroupPIN")]
        public long? EnterGroupPin { get; set; }

        [InverseProperty("Group")]
        public virtual ICollection<DeviceGroup> DeviceGroup { get; set; }
        [InverseProperty("Group")]
        public virtual ICollection<FinalQuestion> FinalQuestion { get; set; }
        [InverseProperty("Group")]
        public virtual ICollection<GivenAnswer> GivenAnswer { get; set; }
        [ForeignKey("Topic")]
        [InverseProperty("Group")]
        public virtual Topic TopicNavigation { get; set; }
    }
}
