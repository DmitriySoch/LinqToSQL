﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LinqToSQL
{
    [Table(Name = "titleauthor")]
    public class Titleauthor
    {
        [Column(Name = "au_id", IsPrimaryKey = true, DbType="VarChar(11) NOT NULL", CanBeNull=false)]
        public string au_id { get; set; }

        [Column(Name = "title_id", IsPrimaryKey = true, DbType="VarChar(6) NOT NULL", CanBeNull=false)]
        public string title_id { get; set; }

        [Column(Name = "au_ord", DbType="TinyInt")]
        public byte? au_ord { get; set; }

        [Column(Name = "royaltyper", DbType="Int")]
        public int? royaltyper { get; set; }

    }
}
