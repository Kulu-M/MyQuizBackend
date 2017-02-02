using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class Group
    {
        [Column("ID")]
        public long Id { get; set; }
        public string Title { get; set; }
        public string EnterGroupPin { get; set; }
    }
}
