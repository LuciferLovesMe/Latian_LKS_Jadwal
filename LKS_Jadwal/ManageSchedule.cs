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
    public partial class ManageSchedule : Form
    {
        int id, cond;
        public ManageSchedule()
        {
            InitializeComponent();

            lbldate.Text = DateTime.Now.ToString("dddd, dd MM yyyy / HH:mm:ss");
            lblname.Text = Session.name;

            dtstart.Format = DateTimePickerFormat.Custom;
            dtstart.CustomFormat = "HH:mm";
            dtstart.ShowUpDown = true;

            dtfinish.Format = DateTimePickerFormat.Custom;
            dtfinish.CustomFormat = "HH:mm";
            dtfinish.ShowUpDown = true;

            loadclass();
            loadsubject();
            loadteacher();
            loadgrid();
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
            btndel.Enabled = true;
            btnup.Enabled = true;
            btnsave.Enabled = false;
            btncancel.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            dtfinish.Enabled = false;
            dtstart.Enabled = false;
        }

        void enable()
        {
            btninsert.Enabled = false;
            btndel.Enabled = false;
            btnup.Enabled = false;
            btnsave.Enabled = true;
            btncancel.Enabled = true;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            dtfinish.Enabled = true;
            dtstart.Enabled = true;
        }

        void loadsubject()
        {
            string com = "select * from subject";
            comboBox1.DataSource = Command.data(com);
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "subjectid";
        }

        void loadteacher()
        {
            string com = "select * from teacher";
            comboBox2.DataSource = Command.data(com);
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "teacherid";
        }

        void loadclass()
        {
            string com = "select * from class";
            comboBox3.DataSource = Command.data(com);
            comboBox3.DisplayMember = "name";
            comboBox3.ValueMember = "classid";
        }

        bool val()
        {
            if(comboBox1.Text.Length < 1 || comboBox2.Text.Length < 1 || comboBox3.Text.Length < 1 || comboBox4.Text.Length < 1 || dtfinish.Value == null || dtstart == null)
            {
                MessageBox.Show("All Fields Must be Filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (dtstart.Value > dtfinish.Value)
            {
                MessageBox.Show("Start value must be less than finish value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        void loadgrid()
        {
            string com = "select headerschedule.*, teacher.name, class.name, subject.name from headerschedule join teacher on headerschedule.teacherid = teacher.teacherid join subject on headerschedule.subjectid = subject.subjectid join class on headerschedule.classid = class.classid";
            dataGridView1.DataSource = Command.data(com);
        }

        private void btnup_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                cond = 2;
                enable();
            }
            else
            {
                MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string com = "insert into headerschedule values(" + comboBox1.SelectedValue + ", " + comboBox2.SelectedValue + ", " + comboBox3.SelectedValue + ", '" + comboBox4.Text + "', '" + dtstart.Value.ToString("HH:mm") + "-" + dtfinish.Value.ToString("HH:mm") + "')";
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dis();
                        loadgrid();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex);
                        throw;
                    }
                }
            }
            else if (cond == 2)
            {
                if (val())
                {
                    string comm = "update headerschedule set subjectid = " + comboBox1.SelectedValue + ", teacherid = " + comboBox2.SelectedValue + ", classid = " + comboBox3.SelectedValue + ", day = '" + comboBox4.Text + "', time = '" + dtstart.Value.ToString("HH:mm") + "-" + dtfinish.Value.ToString("HH:mm") + "' where scheduleid =" + id;
                    try
                    {
                        Command.exec(comm);
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dis();
                        loadgrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex);
                        throw;
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string com = "select headerschedule.*, teacher.name, class.name, subject.name from headerschedule join teacher on headerschedule.teacherid = teacher.teacherid join subject on headerschedule.subjectid = subject.subjectid join class on headerschedule.classid = class.classid where subject.name like '%" + textBox1.Text + "%'";
            dataGridView1.DataSource = Command.data(com);
        }

        private void btndel_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(dialogResult == DialogResult.Yes)
                {
                    string com = "delete from headerschedule where scheduleid = " + id;
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dis();
                        loadgrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex);
                        throw;
                    }
                }

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
            comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            comboBox2.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[2].Value);
            comboBox3.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            comboBox3.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[3].Value);
            comboBox4.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();

            string time = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            string[] s = time.Split('-');
            dtfinish.Value = DateTime.ParseExact(s[1], "HH:mm", null);
            dtstart.Value = DateTime.ParseExact(s[0], "HH:mm", null);
        }

        private void btninsert_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }
    }
}
