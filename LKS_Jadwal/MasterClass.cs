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
    public partial class MasterClass : Form
    {
        int id, cond;
        public MasterClass()
        {
            InitializeComponent();

            lbldate.Text = DateTime.Now.ToString("dddd, dd MM yyyy / HH:mm:ss");
            lblname.Text = Session.name;

            dis();
            loadgrid();
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
            btnsave.Enabled = false;
            btncancel.Enabled = false;
            textBox2.Enabled = false;
        }

        void enable()
        {
            btninsert.Enabled = false;
            btnup.Enabled = false;
            btndel.Enabled = false;
            btnsave.Enabled = true;
            btncancel.Enabled = true;
            textBox2.Enabled = true;
        }

        bool val()
        {
            if(textBox2.TextLength < 1)
            {
                MessageBox.Show("Field must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlCommand command = new SqlCommand("select * from class where name = '"+ textBox2.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                MessageBox.Show("Class Name Already in Use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();
                return false;
            }

            connection.Close();
            return true;
        }

        void clear()
        {
            textBox2.Text = "";
        }

        void loadgrid()
        {
            string com = "select * from class";
            dataGridView1.DataSource = Command.data(com);
        }

        private void btninsert_Click(object sender, EventArgs e)
        {
            enable();
            cond = 1;
           
        }

        private void btnup_Click(object sender, EventArgs e)
        {
            enable();
            cond = 2;
            
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if(cond == 1)
            {
                if (val())
                {
                    string com = "insert into class values('" + textBox2.Text + "')";
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        dis();
                        loadgrid();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else if (cond == 2)
            {
                if (dataGridView1.SelectedRows != null)
                {
                    if (val())
                    {
                        string com = "update class set name = '" + textBox2.Text + "' where classid ="+id;
                        try
                        {
                            Command.exec(com);
                            MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clear();
                            dis();
                            loadgrid();
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            dis();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            dis();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
        }

        private void btndel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows != null)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    string com = "delete from class where classid = " + id;
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        dis();
                        loadgrid();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string com = "select * from class where name like '%" + textBox1.Text + "%'";
            dataGridView1.DataSource = Command.data(com);
        }
    }
}
