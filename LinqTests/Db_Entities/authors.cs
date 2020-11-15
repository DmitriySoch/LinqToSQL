﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LinqToSQL
{
    [Table(Name = "authors")]
    public class Authors
    {
        [Column(Name = "au_id", IsPrimaryKey = true, DbType="VarChar(11) NOT NULL", CanBeNull=false)]
        public string au_id { get; set; }

        [Column(Name = "au_lname", DbType="VarChar(40) NOT NULL", CanBeNull=false)]
        public string au_lname { get; set; }

        [Column(Name = "au_fname", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
        public string au_fname { get; set; }

        [Column(Name = "phone", DbType="VarChar(12) NOT NULL", CanBeNull=false)]
        public string phone { get; set; }

        [Column(Name = "address", DbType="VarChar(40)")]
        public string address { get; set; }

        [Column(Name = "city", DbType="VarChar(40)")]
        public string city { get; set; }

        [Column(Name = "state", DbType="Char(2)")]
        public string state { get; set; }

        [Column(Name = "zip", DbType="Char(5)")]
        public string zip { get; set; }

        [Column(Name = "contract", DbType="Bit NOT NULL", CanBeNull=false)]
        public bool contract { get; set; }

    }
}
