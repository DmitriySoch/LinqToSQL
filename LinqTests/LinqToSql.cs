using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace LinqToSQL
{
    public class LinqToSql:DataContext
    {
        public static StringBuilder file = new StringBuilder();
        public static StringBuilder log = new StringBuilder();

        public LinqToSql(SqlConnection dbConnection):base(dbConnection)
        {
            this.Log = new StringWriter(log);
        }

        public void DeleteData(object title_id)
        {
            ClearLog();
            //Попытка удаления строки со значением title_id = DC1234 из таблицы titles
            var obj = this.GetTable<Titles>().First(x => x.title_id.CompareTo((string)title_id)==0);
            file.AppendLine("Результат работы");
            file.AppendLine($"До удаления : {obj.title_id} | {obj.title} ");
            this.GetTable<Titles>().DeleteOnSubmit(obj);
            //Принятие изменений
            this.SubmitChanges();
            //Проверка наличия удаленой строки
            var res = this.GetTable<Titles>().FirstOrDefault(x => x.title_id.CompareTo((string)title_id) == 0);
            file.AppendLine($"После удаления : {res?.title_id} | {res?.title} ");
        }

        private void ClearLog()
        {
            log = new StringBuilder();
            this.Log = new StringWriter(log);
            file = new StringBuilder();
        }

        public void UpdateData(object stor_id, object qty)
        {
            ClearLog();            
            var data = this.GetTable<Sale>().First(x => x.title_id.CompareTo((string)stor_id) == 0);
            file.AppendLine("До изменения");
            file.AppendLine($"stor_id {data.stor_id} | qty  {data.qty} ");
            //Изменение значения первой строки в таблице Sale
            data.qty = short.Parse((string)qty);
            //Проверка изменений
            this.SubmitChanges(); //TODO
            file.AppendLine("После изменения");
            file.AppendLine($"stor_id {data.stor_id} | qty  {data.qty} ");
        }

        public void FilterData(object start_with)
        {
            ClearLog();
            //Выбираем только тех авторов, номер телефона которых начинается на 415
            var filtredData =
                from u in this.GetTable<Authors>()
                where u.phone.StartsWith((string)start_with)
                select u;
            Print(filtredData);
        }

        private void Print<T>(IQueryable<T> query)
        {
            //Медод вывода результатов работы других методов
            file.AppendLine("Результат работы");
            var parameters = typeof(T).GetProperties();
            foreach (var item in query)
            {
                file.AppendLine(string.Join(" | ",
                    parameters.Select(x => typeof(T).GetProperty(x.Name)?.GetValue(item))));
            }
        }

        public void OrderData()
        {
            ClearLog();
            //Сортируем значения в таблице titles сначала по убыванию стоимости книги, а потом по названию
            var orderedData = from u in this.GetTable<Titles>()
                              orderby u.price descending, u.title
                              select new { Title = u.title, Title_Id = u.title_id, Price = u.price };
            Print(orderedData);
        }

        public void GroupData()
        {
            ClearLog();
            //Группируем строки в таблице по значению типа
            var groupedData = this.GetTable<Titles>().GroupBy(x => x.type);
            foreach (var item in groupedData)
            {
                file.AppendLine(item.Key);
                foreach (var i in item)
                {
                    //Выводим каждую группу
                    file.AppendLine($"{i.title_id} | {i.title} | {i.type}");
                }
            }
        }

        public void PaggingData(object start, object step)
        {
            ClearLog();
            //Выводим 13 строк из таблицы titleauthors начиная с 5 строки
            var pageData = this.GetTable<Titleauthor>().Skip(int.Parse((string)start)).Take(int.Parse((string)step));
            Print(pageData);
        }

        public void InsertData(object title_id, object title)
        {
            ClearLog();
            //Вставляем новую книгу с id DC1234 в таблицу titles
            var newItem = new Titles() { title_id = (string)title_id, title = (string)title};
            this.GetTable<Titles>().InsertOnSubmit(newItem);
            //Принимаем изменения
            this.SubmitChanges();
            //Проверяем наличие добавленной книги
            var result = this.GetTable<Titles>().First(x => x.title_id.CompareTo(title_id)==0);
            file.AppendLine(result.title_id);
        }

        public void Execute()
        {
            ClearLog();
            this.ExecuteQuery<Titles>("select * from titles where type = 'business'");
            this.ExecuteCommand("drop table if exists test");
            this.ExecuteCommand("create table test (price int)");
            var result = 0.0;
            GetMaxPrice("213-46-8915", ref result);
            file.AppendLine(result.ToString());
            
        }

        [Function(Name = "getMaxPrice")]
        [return: Parameter(DbType = "Int")]
        private int GetMaxPrice([Parameter(Name = "author_id", DbType = "NVarChar(100)")] string author_id,
                                 [Parameter(Name = "maxPrice", DbType = "Money")] ref double maxPrice)
{
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), author_id, maxPrice);
            maxPrice = (double)result.GetParameterValue(1);
            return (int)result.ReturnValue;
        }
    }
}