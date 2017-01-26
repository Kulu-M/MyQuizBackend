using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class Device
    {
        [Column("ID")]
        public long Id { get; set; }
        public string PushUpToken { get; set; }
        public string IsAdmin { get; set; }
    }
}
