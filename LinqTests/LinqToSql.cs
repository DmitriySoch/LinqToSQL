using System;
using System.Data.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace LinqToSQL
{
    public class LinqToSql
    {
        private DataContext dbContext;
        public static StringBuilder file = new StringBuilder();
        public static StringBuilder log = new StringBuilder();
        private static DataContext Connect(SqlConnection dbConnection)
        {
            return new DataContext(dbConnection) { Log = new StringWriter(log) };
        }

        public LinqToSql(SqlConnection dbConnection)
        {
            dbContext = Connect(dbConnection);
        }

        public void DeleteData()
        {
            ClearLog();
            //Попытка удаления строки со значением title_id = DC1234 из таблицы titles
            var obj = dbContext.GetTable<Titles>().First(x => x.title_id == "DC1234");
            file.AppendLine("Результат работы");
            file.AppendLine($"До удаления : {obj.title_id} | {obj.title} ");
            dbContext.GetTable<Titles>().DeleteOnSubmit(obj);
            //Принятие изменений
            dbContext.SubmitChanges();
            //Проверка наличия удаленой строки
            var res = dbContext.GetTable<Titles>().FirstOrDefault(x => x.title_id == "DC1234");
            file.AppendLine($"После удаления : {res.title_id} | {res.title} ");

        }

        private void ClearLog()
        {
            log = new StringBuilder();
            dbContext.Log = new StringWriter(log);
            file = new StringBuilder();
        }

        public void UpdateData()
        {
            ClearLog();            
            var data = dbContext.GetTable<Sale>().Take(1);
            file.AppendLine("До изменения");
            Print(data);
            //Изменение значения первой строки в таблице Sale
            data.First().qty += 11;
            file.AppendLine("После изменения");
            //Проверка изменений
            dbContext.SubmitChanges();
            Print(data);
        }

        public void FilterData()
        {
            ClearLog();
            //Выбираем только тех авторов, номер телефона которых начинается на 415
            var filtredData =
                from u in dbContext.GetTable<Authors>()
                where u.phone.StartsWith("415")
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
            var orderedData = from u in dbContext.GetTable<Titles>()
                              orderby u.price descending, u.title
                              select new { Title = u.title, Title_Id = u.title_id, Price = u.price };
            Print(orderedData);
        }

        public void GroupData()
        {
            ClearLog();
            //Группируем строки в таблице по значению типа
            var groupedData = dbContext.GetTable<Titles>().GroupBy(x => x.type);
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

        public void PaggingData()
        {
            ClearLog();
            //Выводим 13 строк из таблицы titleauthors начиная с 5 строки
            var pageData = dbContext.GetTable<Titleauthor>().Skip(5).Take(13);
            Print(pageData);
        }

        public void InsertData()
        {
            ClearLog();
            //Вставляем новую книгу с id DC1234 в таблицу titles
            var newItem = new Titles() { title_id = "DC1234", title = "Torment with SQL" };
            dbContext.GetTable<Titles>().InsertOnSubmit(newItem);
            //Принимаем изменения
            dbContext.SubmitChanges();
            //Проверяем наличие добавленной книги
            var result = dbContext.GetTable<Titles>().First(x => x.title_id == "DC1234");
            file.AppendLine(result.title_id);
        }
    }
}