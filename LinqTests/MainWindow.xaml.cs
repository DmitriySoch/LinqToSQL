using LinqToSQL;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace LinqTests
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public LinqToSql linqToSql;
        public LinqToDataset linqToDataset;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            var dbConnection = new SqlConnection(@"Server=(local);Integrated Security=true");
            linqToSql = new LinqToSql(dbConnection);
            linqToDataset = new LinqToDataset(dbConnection);
        }

        private void ComboBox_Initialized(object sender, RoutedEventArgs e)
        {
            var list = linqToSql.GetType().GetMethods().Where(x=>x.ReturnType == typeof(void)).Select(x => x.Name);
            cmBox1.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedMethod = cmBox1.SelectedItem?.ToString();
            if (selectedMethod != null)
            {
                var metd = linqToSql.GetType().GetMethod(selectedMethod);
                metd.Invoke(linqToSql, null);
                fileInfo.Text = LinqToSql.file.ToString();
                LogInfo.Text = LinqToSql.log.ToString();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cmBox2.SelectedItem != null)
            {
                linqToDataset.ExampleOfSelect();
                fileInfo1.Text = LinqToDataset.file.ToString();
            }
        }
    }
}
