﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYVote.Models
{
    public partial class SingleTopic
    {
        [Column("ID")]
        public long Id { get; set; }
        public string Name { get; set; }
        public string DateTime { get; set; }
    }
}
