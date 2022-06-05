using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace Todo
{
    /// <summary>
    /// Interaction logic for updateTask.xaml
    /// </summary>
    public partial class updateTask : Window
    {
        int taskId = 0;
        public updateTask(int id)
        {
            InitializeComponent();
            taskId = id;
            loadData();
        }

        public void loadData()
        {
            OleDbConnection con = new OleDbConnection();
            string fileName = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"..\..\..\connection.txt");
            string ConnString = File.ReadAllText(fileName);
            //string SqlString ="Select * from todo where ID ='" + taskId + "'";
            string SqlString = "Select * from todo where ID=@ID";
            using (OleDbConnection conn = new OleDbConnection(ConnString))
            {
                using (OleDbCommand cmd = new OleDbCommand(SqlString, conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ID", taskId);
                    OleDbDataReader dt = cmd.ExecuteReader();
                    dt.Read();
                    titleText.Text = dt["Title"].ToString();
                    nameText.Text = dt["Name"].ToString();
                    descriptionText.Text = dt["Description"].ToString();
                    date.Text = dt["DueDate"].ToString();
                }
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if(sender.Equals(update))
            {
                if (titleText.Text == "" || nameText.Text == "")
                {
                    MessageBox.Show("Fill out Name and Title");
                    return;
                }
                if (!date.SelectedDate.HasValue)
                {
                    MessageBox.Show("Select Due Date");
                    return;
                }
                string fileName = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"..\..\..\connection.txt");
                string text = File.ReadAllText(fileName);
                string ConnString = text;
                string SqlString;
                if (check.IsChecked == true)
                    SqlString = "Update todo set Title=@Title, Name=@Name, Description=@Description, DueDate=@DueDate, IsDeleted=@IsDeleted where ID=@ID";
                else
                    SqlString = "Update todo set Title=@Title, Name=@Name, Description=@Description, DueDate=@DueDate where ID=@ID";
                using (OleDbConnection conn = new OleDbConnection(ConnString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(SqlString, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Title", titleText.Text);
                        cmd.Parameters.AddWithValue("@Name", nameText.Text);
                        cmd.Parameters.AddWithValue("@Description", descriptionText.Text);
                        cmd.Parameters.AddWithValue("@DueDate", date.SelectedDate);
                        if(check.IsChecked == true)
                            cmd.Parameters.AddWithValue("@IsDeleted", true);
                        cmd.Parameters.AddWithValue("@ID", taskId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                MessageBox.Show("Task Updated Successfully...");
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
            }
            else
            {
                this.Close();
            }
        }
    }
}
