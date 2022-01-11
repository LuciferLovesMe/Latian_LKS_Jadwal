using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Jadwal
{
    public partial class MainLogin : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        public MainLogin()
        {
            InitializeComponent();
        }
        
        bool val()
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox2.TextLength < 8)
            {
                MessageBox.Show("Password minimal has 8 characters", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else if (!checkBox1.Checked)
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (val())
            {
                SqlCommand command = new SqlCommand("select * from [dbo].[user] where username = '" + textBox1.Text + "' and password = '" + textBox2.Text + "'", connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        string role = reader["role"].ToString();
                        if (role.ToLower() == "admin")
                        {
                            Session.id = Convert.ToInt32(reader["userid"]);
                            Session.name = Convert.ToString(reader["username"]);
                            MainAdmin admin = new MainAdmin();
                            this.Hide();
                            admin.ShowDialog();
                        }
                        else if (role.ToLower() == "student")
                        {
                            int id = Convert.ToInt32(reader["userid"]);
                            connection.Close();
                            SqlCommand com = new SqlCommand("select * from student where userid = " + id, connection);

                            connection.Open();
                            SqlDataReader sql = com.ExecuteReader();
                            sql.Read();
                            Session.id = Convert.ToInt32(sql["studentId"]);
                            Session.name = sql["name"].ToString();
                            Session.classid = Convert.ToInt32(sql["classid"]);
                            Session.userid = id;
                            connection.Close();

                            MainStudent main = new MainStudent();
                            this.Hide();
                            main.ShowDialog();
                        }
                        else if (role.ToLower() == "teacher")
                        {
                            int id = Convert.ToInt32(reader["userid"]);
                            connection.Close();
                            SqlCommand com = new SqlCommand("select * from teacher where userid = " + id, connection);

                            connection.Open();
                            SqlDataReader sql = com.ExecuteReader();
                            sql.Read();
                            Session.id = Convert.ToInt32(sql["teacherId"]);
                            Session.name = sql["name"].ToString();
                            Session.classid = 0;
                            Session.userid = id;
                            connection.Close();

                            MainTeacher main = new MainTeacher();
                            this.Hide();
                            main.ShowDialog();
                        }
                    }
                    else
                    {
                        connection.Close();
                        MessageBox.Show("Can't find user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
