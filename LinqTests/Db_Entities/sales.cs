﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;


namespace LinqToSQL
{
    [Table(Name = "sales")]
    public class Sale
    {
        [Column(Name = "stor_id", IsPrimaryKey = true, DbType="Char(4) NOT NULL", CanBeNull=false)]
        public string stor_id { get; set; }

        [Column(Name = "ord_num", IsPrimaryKey = true, DbType="VarChar(20) NOT NULL", CanBeNull=false)]
        public string ord_num { get; set; }

        [Column(Name = "ord_date", DbType="DateTime NOT NULL", CanBeNull=false)]
        public DateTime ord_date { get; set; }

        [Column(Name = "qty", DbType="SmallInt NOT NULL", CanBeNull=false)]
        public short qty { get; set; }

        [Column(Name = "payterms", DbType="VarChar(12) NOT NULL", CanBeNull=false)]
        public string payterms { get; set; }

        [Column(Name = "title_id", IsPrimaryKey = true, DbType="VarChar(6) NOT NULL", CanBeNull=false)]
        public string title_id { get; set; }

    }
}
