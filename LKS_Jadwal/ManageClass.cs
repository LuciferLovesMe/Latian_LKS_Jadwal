using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Jadwal
{
    public partial class ManageClass : Form
    {
        int id1, id2;
        public ManageClass()
        {
            InitializeComponent();

            lbldate.Text = DateTime.Now.ToString("dddd, dd MM yyyy / HH:mm:ss");
            lblname.Text = Session.name;
            loadcombo();
            loadwithout();
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

        }

        private void panelmanageclass_Click(object sender, EventArgs e)
        {
            ManageClass manage = new ManageClass();
            this.Hide();
            manage.ShowDialog();
        }

        void loadcombo()
        {
            string com = "select * from class";
            comboBox1.DataSource = Command.data(com);
            comboBox1.ValueMember = "classid";
            comboBox1.DisplayMember = "name";
        }

        void loadwithout()
        {
            string com = "select studentid, name from student where classid is null";
            dataGridView1.DataSource = Command.data(com);
        }

        void loadwith()
        {
            string com = "select studentid, name from student where classid = " + comboBox1.SelectedValue;
            dataGridView2.DataSource = Command.data(com);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadwith();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string com = "select studentid, name from student where classid is null and name like '%" + textBox1.Text + "%'";
            dataGridView1.DataSource = Command.data(com);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string com = "select studentid, name from student where name like '%" + textBox2.Text + "%' and classid = " + comboBox1.SelectedValue;
            dataGridView2.DataSource = Command.data(com);
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CurrentRow.Selected = true;
            id2 = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                string com = "update student set classid = " + comboBox1.SelectedValue + " where studentid = " + id1;
                try
                {
                    Command.exec(com);
                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    id1 = 0;
                    loadwithout();
                    dataGridView2.DataSource = null;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select an item from left datagrid", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(dataGridView2.CurrentRow != null)
            {
                string com = "update student set classid = null where studentid = " + id2;
                try
                {
                    Command.exec(com);
                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    id1 = 0;
                    loadwithout();
                    dataGridView2.DataSource = null;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select an item from right datagrid", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id1 = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
        }
    }
}
