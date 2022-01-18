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
    public partial class MasterStudent : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        int id, cond;
        public MasterStudent()
        {
            InitializeComponent();

            lbldate.Text = DateTime.Now.ToString("dddd, dd MM yyyy / HH:mm:ss");
            lblname.Text = Session.name;
            loadgrid();
            loadcombo();
            dis();
        }

        private void panelStudent_Click(object sender, EventArgs e)
        {
            MasterStudent master = new MasterStudent();
            this.Hide();
            master.ShowDialog();
        }

        private void panelTeacher_Click(object sender, EventArgs e)
        {
            MasterTeacher master = new MasterTeacher();
            this.Hide();
            master.ShowDialog();
        }

        private void panelClass_Click(object sender, EventArgs e)
        {
            MasterClass master = new MasterClass();
            this.Hide();
            master.ShowDialog();
        }

        private void panelSchedule_Click(object sender, EventArgs e)
        {
            ManageSchedule master = new ManageSchedule();
            this.Hide();
            master.ShowDialog();
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
        private void panelmanageclass_Click(object sender, EventArgs e)
        {
            ManageClass manage = new ManageClass();
            this.Hide();
            manage.ShowDialog();
        }

        void dis()
        {
            btninsert.Enabled = true;
            btnup.Enabled = true;
            btndel.Enabled = true;
            comboBox1.Enabled = false;
            txtaddress.Enabled = false;
            txtname.Enabled = false;
            txtphone.Enabled = false;
            rbfemale.Enabled = false;
            rbmale.Enabled = false;
            dateTimePicker1.Enabled = false;
            btnsave.Enabled = false;
            btncancel.Enabled = false;
            btnupl.Enabled = false;
        }

        void enable()
        {
            txtaddress.Enabled = true;
            txtname.Enabled = true;
            txtphone.Enabled = true;
            rbfemale.Enabled = true;
            rbmale.Enabled = true;
            dateTimePicker1.Enabled = true;
            btnsave.Enabled = true;
            btncancel.Enabled = true;
            btnupl.Enabled = true;
            comboBox1.Enabled = true;
            btndel.Enabled = false;
            btninsert.Enabled = false;
            btnup.Enabled = false;
        }

        bool val()
        {
            if(comboBox1.Text.Length < 1 || txtaddress.TextLength < 1 || txtname.TextLength < 1 || txtphone.TextLength < 1 || dateTimePicker1.Value == null || !rbfemale.Checked && !rbmale.Checked || pictureBox1.Image == null)
            {
                MessageBox.Show("Field must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtphone.TextLength < 11)
            {
                MessageBox.Show("Phone at least has 11 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        string getuser()
        {
            SqlCommand command = new SqlCommand("select COUNT(*) as num from [dbo].[user] where role = 'student'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                int last = Convert.ToInt32(reader["num"]) + 1;
                connection.Close();
                return "STU" + last.ToString();
            }

            connection.Close();
            return "STU1";
        }

        void clear()
        {
            txtaddress.Text = "";
            txtname.Text = "";
            txtphone.Text = "";
            rbfemale.Checked = false;
            rbmale.Checked = true;
            comboBox1.Text = "";
            pictureBox1.Image = null;
        }

        void loadcombo()
        {
            string com = "select * from class";
            comboBox1.DataSource = Command.data(com);
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "classid";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string com = "select student.*,class.name, [dbo].[user].* from student join class on student.classid = class.classid join [dbo].[user] on student.userid = [dbo].[user].userid where student.name like '%" + textBox1.Text +"%'"; 
            dataGridView1.DataSource = Command.data(com);
        }

        private void btninsert_Click(object sender, EventArgs e)
        {
            enable();
            cond = 1;
        }

        private void btnup_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                enable();
                cond = 2;
            }
        }

        private void btndel_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete ?", "confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    string com = "delete from student where studentid = " + id;
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadgrid();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            dis();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if(cond == 1)
            {
                if (val())
                {
                    string com = "insert into [dbo].[user] values('" + getuser() + "', '123123123', 'student')";
                    try
                    {
                        Command.exec(com);

                        SqlCommand command1 = new SqlCommand("select top(1) * from [dbo].[user] where role = 'student' order by userid desc", connection);
                        connection.Open();
                        SqlDataReader reader = command1.ExecuteReader();
                        reader.Read();
                        int userid = Convert.ToInt32(reader["userid"]);
                        connection.Close();

                        string gender = "Unknown";
                        if (rbfemale.Checked)
                        {
                            gender = "Female";

                        }
                        else if (rbmale.Checked)
                        {
                            gender = "Male";
                        }

                        ImageConverter converter = new ImageConverter();
                        byte[] img = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                        SqlCommand sql = new SqlCommand("insert into student values('" + txtname.Text + "', '" + txtaddress.Text + "', '" + gender + "', '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', '+62" + txtphone.Text + "', " + userid + ", " + comboBox1.SelectedValue + ", @img)", connection);
                        sql.Parameters.AddWithValue("@img", img);
                    
                        try
                        {
                            connection.Open();
                            sql.ExecuteNonQuery();
                            connection.Close();

                            MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadgrid();
                            dis();
                            clear();
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);

                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }

                }
            }

            else if (cond == 2)
            {
                if (val())
                {
                    string gender = "none";
                    if (rbfemale.Checked)
                    {
                        gender = "Female";

                    }
                    else if (rbmale.Checked)
                    {
                        gender = "Male";
                    }

                    ImageConverter converter = new ImageConverter();
                    byte[] img = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                    SqlCommand sql = new SqlCommand("update student set name = '" + txtname.Text + "', address = '" + txtaddress.Text + "', gender = '" + gender + "', dateofbirth = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', nohp = '" + txtphone.Text + "', classid = " + comboBox1.SelectedValue + ", photo = @img where studentid = " + id, connection);
                    sql.Parameters.AddWithValue("@img", img);
                    try
                    {
                        connection.Open();
                        sql.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadgrid();
                        dis();
                        clear();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        private void rbmale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbmale.Checked)
            {
                rbfemale.Checked = false;
            }
        }

        private void rbfemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbfemale.Checked)
            {
                rbmale.Checked = false;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            txtname.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtaddress.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[3].Value.ToString() == "Male")
            {
                rbmale.Checked = true;
            }
            else if(dataGridView1.SelectedRows[0].Cells[3].Value.ToString() == "Female")
            {
                rbfemale.Checked = true;
            }
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[4].Value);
            txtphone.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
            comboBox1.SelectedValue = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();

            if(dataGridView1.SelectedRows[0].Cells[8].Value == System.DBNull.Value)
            {
                pictureBox1.Image = null;
            }
            byte[] b = (byte[])dataGridView1.SelectedRows[0].Cells[8].Value;
            MemoryStream stream = new MemoryStream(b);
            pictureBox1.Image = Image.FromStream(stream);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void txtphone_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsNumber(e.KeyChar) || e.KeyChar == 8);
        }

        private void btnupl_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image| *.png; *.jpg; *jpeg";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(ofd.FileName);
                Bitmap bmp = (Bitmap)img;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void txtname_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        void loadgrid()
        {
            string com = "select student.*,class.name, [dbo].[user].* from student join class on student.classid = class.classid join [dbo].[user] on student.userid = [dbo].[user].userid";
            dataGridView1.DataSource = Command.data(com);
            dataGridView1.Columns[8].Visible = false;
        }


    }
}
