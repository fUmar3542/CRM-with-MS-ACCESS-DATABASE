using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Globalization;
using System.IO;

namespace Todo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
            setConnection();
            loadData();
        }

        private void setConnection()
        {
            string s = "";
            try
            {
                //string f = System.Reflection.Assembly.GetExecutingAssembly().Location;
                //string temp = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xls").ToList().FirstOrDefault();
                //string temp2 = Directory.GetFiles(temp, "*.xls").ToList().FirstOrDefault();
                //string fileName = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"..\connection.txt");
                //string dbName = System.IO.Path.GetFullPath(f + @"db todo tasks.accdb");
                //s = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source =" + dbName + ";Persist Security Info=False;";
                //File.WriteAllText(fileName, s);

                string fileName = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"..\..\..\connection.txt");
                string dbName = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"/Data/db todo tasks.accdb");
                s = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source =" + dbName + ";Persist Security Info=False;";
                File.WriteAllText(fileName, s);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
        }
        List<task> taskList = new List<task>(){};
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string title = "TaskAssignment";  /*Your Window Instance Name*/
            var existingWindow = Application.Current.Windows.
            Cast<Window>().SingleOrDefault(x => x.Title.Equals(title));
            if (existingWindow == null)
            {
                Task_Assignment newWindow = new Task_Assignment(); /* Give Your window Instance */
                newWindow.Title = title;
                newWindow.Show();
            }
        }

        private void loadData()
        {
            try
            {
                OleDbConnection con = new OleDbConnection();
                string fileName = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"..\..\..\connection.txt");
                string text = File.ReadAllText(fileName);
                con.ConnectionString = text;
                con.Open();
                OleDbCommand command = new OleDbCommand();
                command.CommandText = "Select * from todo";
                command.Connection = con;
                OleDbDataReader dt = command.ExecuteReader();
                string d = "";
                int res = 0;
                DateTime c = DateTime.Now.AddDays(2).Date, due;
                while (dt.Read())
                {
                    due = Convert.ToDateTime(dt["DueDate"]);
                    res = DateTime.Compare(c, due);
                    if (dt[6].Equals(false))
                    {
                        if (res >= 0)
                            taskList.Add(new task() { Id = Convert.ToInt32(dt[0].ToString()), Title = dt[1].ToString(), Name = dt[2].ToString(), Assign = Convert.ToDateTime(dt["AssignDate"]).ToString("dd/MM/yyyy"), Due = due.ToString("dd/MM/yyyy") });
                    }
                }
                dt.Close();
                table.ItemsSource = taskList;
                con.Close();
            }
            catch (Exception x)
            {
                MessageBox.Show("Database error " + x.ToString());
            }
        }

        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = ItemsControl.ContainerFromElement((DataGrid)sender,
                                                e.OriginalSource as DependencyObject) as DataGridRow;
            if (row == null)
                return;
            else
            {
                int a = table.SelectedIndex;               
                updateTask newWindow = new updateTask(taskList[a].Id); /* Give Your window Instance */
                newWindow.Title = newWindow.ToString();
                newWindow.Show();
            }
        }
    }

    public class task
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string assign;
        public string Assign
        {
            get { return assign; }
            set { assign = value; }
        }

        private string due;
        public string Due
        {
            get { return due; }
            set { due = value; }
        }

    }
}
