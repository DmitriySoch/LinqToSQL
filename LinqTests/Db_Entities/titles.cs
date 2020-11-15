﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LinqToSQL
{
    [Table(Name = "titles")]
    public class Titles
    {
        public Titles()
        {
            pubdate = DateTime.Now;
            type = "UNDECIDED";
        }
        [Column(Name = "title_id", IsPrimaryKey = true, DbType="VarChar(6) NOT NULL", CanBeNull=false)]
        public string title_id { get; set; }

        [Column(Name = "title", DbType="VarChar(80) NOT NULL", CanBeNull=false)]
        public string title { get; set; }

        [Column(Name = "type", DbType="Char(12) NOT NULL", CanBeNull=false)]
        public string type { get; set; }

        [Column(Name = "pub_id", DbType="Char(4)")]
        public string pub_id { get; set; }

        [Column(Name = "price", DbType="Money")]
        public decimal? price { get; set; }

        [Column(Name = "advance", DbType="Money")]
        public decimal? advance { get; set; }

        [Column(Name = "royalty", DbType="Int")]
        public int? royalty { get; set; }

        [Column(Name = "ytd_sales", DbType="Int")]
        public int? ytd_sales { get; set; }

        [Column(Name = "notes", DbType="VarChar(200)")]
        public string notes { get; set; }

        [Column(Name = "pubdate", DbType="DateTime NOT NULL", CanBeNull=false)]
        public DateTime pubdate { get; set; }

    }
}
