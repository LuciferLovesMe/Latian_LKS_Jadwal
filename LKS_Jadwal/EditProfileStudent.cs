using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Jadwal
{
    public partial class EditProfileStudent : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        string role;
        int id;
        public EditProfileStudent()
        {
            InitializeComponent();

            lbldate.Text = DateTime.Now.ToString("dddd, dd MM yyyy / HH:mm:ss");
            lblname.Text = Session.name;

            loadprofile();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to exit ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to logout ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin master = new MainLogin();
                this.Hide();
                master.ShowDialog();
            }
        }

        private void panelEdit_Click(object sender, EventArgs e)
        {
            EditProfileStudent ed = new EditProfileStudent();
            this.Hide();
            ed.ShowDialog();
        }

        private void panelclass_Click(object sender, EventArgs e)
        {
            ViewScheduleStudent view = new ViewScheduleStudent();
            this.Hide();
            view.ShowDialog();
        }

        void loadprofile()
        {
            SqlCommand sqlCommand = new SqlCommand("select * from [dbo].[user] where userid = " + Session.userid, connection);
            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            reader.Read();
            id = Convert.ToInt32(reader["userid"]);
            role = reader["role"].ToString();
            connection.Close();

            if (role == "student")
            {
                SqlCommand sql = new SqlCommand("select * from student where userid = " + Session.userid, connection);
                connection.Open();
                SqlDataReader dataReader = sql.ExecuteReader();
                dataReader.Read();

                txtname.Text = dataReader["name"].ToString();
                txtaddress.Text = dataReader["address"].ToString();
                txtphone.Text = dataReader["nohp"].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dataReader["dateofbirth"]);

                if(dataReader["photo"] == System.DBNull.Value)
                {
                    pictureBox1.Image = null;
                }
                else
                {
                    byte[] b = (byte[])dataReader["photo"];
                    MemoryStream stream = new MemoryStream(b);
                    pictureBox1.Image = Image.FromStream(stream);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    connection.Close();

                }
            }
            else if (role == "teacher")
            {
                SqlCommand sql = new SqlCommand("select * from teacher where userid = " + Session.userid, connection);
                connection.Open();
                SqlDataReader dataReader = sql.ExecuteReader();
                dataReader.Read();

                txtname.Text = dataReader["name"].ToString();
                txtaddress.Text = dataReader["address"].ToString();
                txtphone.Text = dataReader["nohp"].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dataReader["dateofbirth"]);

                if (dataReader["photo"] == System.DBNull.Value)
                {
                    pictureBox1.Image = null;
                }
                else
                {
                    byte[] b = (byte[])dataReader["photo"];
                    MemoryStream stream = new MemoryStream(b);
                    pictureBox1.Image = Image.FromStream(stream);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    connection.Close();

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtaddress.TextLength < 1 || txtname.TextLength < 1 || txtphone.TextLength < 1 || dateTimePicker1.Value == null || pictureBox1.Image == null)
            {
                MessageBox.Show("All Field Must be Filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string com = "update " + role + " set name = '" + txtname.Text + "', dateofbirth = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', address = '" + txtaddress.Text + "', nohp = '" + txtphone.Text + "' where userid =" + id;

                try
                {
                    Command.exec(com);
                    MessageBox.Show("Success", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadprofile();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
