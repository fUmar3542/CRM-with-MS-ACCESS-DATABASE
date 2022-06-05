using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace Todo
{
    /// <summary>
    /// Interaction logic for Task_Assignment.xaml
    /// </summary>
    public partial class Task_Assignment : Window
    {
        public Task_Assignment()
        {
            InitializeComponent();
        }

        private void done_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(done))
            {
                if (titleText.Text == "" || nameText.Text == "")
                {
                    MessageBox.Show("Fill out Name and Title");
                    return;
                }
                if(!date.SelectedDate.HasValue)
                {
                    MessageBox.Show("Select Due Date");
                    return;
                }
                string fileName = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"..\..\..\connection.txt");
                string text = File.ReadAllText(fileName);
                string ConnString = text;
                string SqlString = "Insert Into todo (Title, Name, Description, AssignDate, DueDate) Values (?,?,?,?,?)";
                using (OleDbConnection conn = new OleDbConnection(ConnString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(SqlString, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("Title", titleText.Text);
                        cmd.Parameters.AddWithValue("Name", nameText.Text);
                        cmd.Parameters.AddWithValue("Description", descriptionText.Text);
                        cmd.Parameters.AddWithValue("AssignDate", DateTime.Now.Date);
                        cmd.Parameters.AddWithValue("DueDate", date.SelectedDate);
                        string f = date.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                MessageBox.Show("Task Assigned Successfully...");
                string t = "Dashboard";
                var ex = Application.Current.Windows.
                      Cast<Window>().SingleOrDefault(x => x.Title.Equals(t));
                if (ex != null)
                {
                    ex.Close();
                    Dashboard newWindow = new Dashboard(); /* Give Your window Instance */
                    newWindow.Title = t;
                    newWindow.Show();
                }
                this.Close();
                Task_Assignment newWindow1 = new Task_Assignment(); /* Give Your window Instance */
                newWindow1.Title = newWindow1.ToString();
                newWindow1.Show();
            }
            else
            {
                this.Close();
            }
        }
    }
}
