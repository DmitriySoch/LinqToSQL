using LinqToSQL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LinqTests
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public LinqToSql linqToSql;
        public LinqToDataset linqToDataset;
        public List<Control> temp_controls;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            var dbConnection = new SqlConnection(@"Server=(local);Database=Books;Integrated Security=true;");
            linqToSql = new LinqToSql(dbConnection);
            linqToDataset = new LinqToDataset(dbConnection);
            temp_controls = new List<Control>();
        }

        private void ComboBox_Initialized(object sender, RoutedEventArgs e)
        {
            var list = linqToSql.GetType().GetMethods().Where(x=>x.ReturnType == typeof(void)&&x.DeclaringType==linqToSql.GetType()).Select(x => x.Name);
            cmBox1.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedMethod = cmBox1.SelectedItem?.ToString();
            if (selectedMethod != null)
            {
                var metd = linqToSql.GetType().GetMethod(selectedMethod);
                var parameters = temp_controls.Where(x => x.GetType() == typeof(TextBox)).Select(x=>((TextBox)x).Text).ToArray();
                metd.Invoke(linqToSql, parameters);
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

        private void cmBox1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selected_item = (string) e.AddedItems[0];
            var methodParams = linqToSql.GetType().GetMethod(selected_item).GetParameters();
            var position = 127;
            foreach(var item in temp_controls)
            {
                Table.Children.Remove(item);
            }
            temp_controls = new List<Control>();
            foreach(var parameter in methodParams)
            {
                var text = new Label() { Content = parameter.Name, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(593, position, 0, 0), VerticalAlignment = VerticalAlignment.Top };
                Table.Children.Add(text);
                temp_controls.Add(text);
                position += 30;
                var textBox = new TextBox() { HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 23, Margin = new Thickness(593, position, 0, 0), TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Top, Width = 185 };
                Table.Children.Add(textBox);
                temp_controls.Add(textBox);
                position += 30;
            }
        }
    }
}
