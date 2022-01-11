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
    public partial class MasterTeacher : Form
    {
        int id, cond;
        SqlConnection connection = new SqlConnection(Utils.conn);
        public MasterTeacher()
        {
            InitializeComponent();
            loadgrid();
            dis();

            lbldate.Text = DateTime.Now.ToString("dddd, dd MM yyyy / HH:mm:ss");
            lblname.Text = Session.name;
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

        private void panelmanageclass_Click(object sender, EventArgs e)
        {
            ManageClass manage = new ManageClass();
            this.Hide();
            manage.ShowDialog();
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

        void dis()
        {
            btninsert.Enabled = true;
            btnup.Enabled = true;
            btndel.Enabled = true;
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
            btndel.Enabled = false;
            btninsert.Enabled = false;
            btnup.Enabled = false;
        }

        bool val()
        {
            if (pictureBox1.Image == null || txtaddress.TextLength < 1 || txtname.TextLength < 1 || txtphone.TextLength < 1 || dateTimePicker1.Value == null || !rbfemale.Checked && !rbmale.Checked)
            {
                MessageBox.Show("Field must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        string getuser()
        {
            SqlCommand command = new SqlCommand("select COUNT(*) as num from [dbo].[user] where role = 'teacher'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                int last = Convert.ToInt32(reader["num"]) + 1;
                connection.Close();
                return "TEA" + last.ToString();
            }

            connection.Close();
            return "TEA1";
        }

        void clear()
        {
            txtaddress.Text = "";
            txtname.Text = "";
            txtphone.Text = "";
            rbfemale.Checked = false;
            rbmale.Checked = true;
            pictureBox1.Image = null;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string com = "select teacher.*, [dbo].[user].* from teacher join [dbo].[user] on teacher.userid = [dbo].[user].userid where teacher.name like '%" + textBox1.Text + "%'";
            dataGridView1.DataSource = Command.data(com);
        }

        private void btninsert_Click(object sender, EventArgs e)
        {
            enable();
            cond = 1;
        }

        private void btnup_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
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
            if (dataGridView1.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete ?", "confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string com = "delete from teacher where teacherid = " + id;
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
            if (cond == 1)
            {
                if (val())
                {

                    string com = "insert into [dbo].[user] values('" + getuser() + "', '123123123', 'teacher')";
                    try
                    {
                        Command.exec(com);

                        SqlCommand command1 = new SqlCommand("select top(1) * from [dbo].[user] where role = 'teacher' order by userid desc", connection);
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
                        SqlCommand sql = new SqlCommand("insert into teacher(name, address, gender, dateofbirth, noHp, userid, photo) values('" + txtname.Text + "', '" + txtaddress.Text + "', '" + gender + "', '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', '+62" + txtphone.Text + "', " + userid + ", @img)", connection);
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
                    string com = "update teacher set name = '" + txtname.Text + "', address = '" + txtaddress.Text + "', gender = '" + gender + "', dateofbirth = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', nohp = '" + txtphone.Text + "' where teacherid = " + id;
                    ImageConverter converter = new ImageConverter();
                    byte[] img = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                    SqlCommand command = new SqlCommand("update teacher set name = '" + txtname.Text + "', address = '" + txtaddress.Text + "', gender = '" + gender + "', dateofbirth = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', nohp = '" + txtphone.Text + "', photo = @img where teacherid = " + id, connection);
                    command.Parameters.AddWithValue("@img", img);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
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
            txtaddress.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "Male")
            {
                rbmale.Checked = true;
            }
            else if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "Female")
            {
                rbfemale.Checked = true;
            }
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[5].Value);
            txtphone.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();

            byte[] b = (byte[])dataGridView1.SelectedRows[0].Cells[7].Value;
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
            ofd.Filter = "Image|*.png; *.jpg; *.jpegg";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(ofd.FileName);
                    Bitmap bmp = (Bitmap)img;
                    pictureBox1.Image = bmp;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void loadgrid()
        {
            string com = "select teacher.*, [dbo].[user].* from teacher join [dbo].[user] on teacher.userid = [dbo].[user].userid";
            dataGridView1.DataSource = Command.data(com);
            dataGridView1.Columns[7].Visible = false;
        }
    }
}
