using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace LinqToSQL
{
    public class LinqToDataset
    {
        private string selectAllTables = @"select * from authors
select * from sales
select * from titles
select * from titleauthor";     //Запрос получения всех таблиц
        private SqlDataAdapter adapter;
        private DataSet ds;
        public static StringBuilder file = new StringBuilder();
        public LinqToDataset(SqlConnection dbConnection)
        {
            adapter = new SqlDataAdapter(selectAllTables, dbConnection);    //Создаем аддаптер
            ds = new DataSet();
            adapter.Fill(ds);   //Заполняем датасет
        }

        public void ExampleOfSelect()
        {
            file = new StringBuilder();
            DataTable authors = ds.Tables[0];
            IEnumerable<DataRow> query =
                from author in authors.AsEnumerable()   //Получаем таблицу authors
                select author;
            file.AppendLine("Тест linq to dataSet");
            foreach (DataRow p in query)
            {
                //Выводим всех авторов содержащихся в таблице
                file.AppendLine($"{p.Field<string>("au_id")} | {p.Field<string>("au_lname")} | {p.Field<string>("au_fname")}");
            }

        }
    }
}