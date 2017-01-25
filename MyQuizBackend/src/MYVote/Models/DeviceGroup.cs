using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    [Table("Device_Group")]
    public partial class DeviceGroup
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("DeviceID")]
        public long DeviceId { get; set; }
        [Column("GroupID")]
        public long GroupId { get; set; }
    }
}
