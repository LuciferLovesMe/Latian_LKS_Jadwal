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
    public partial class ViewScheduleStudent : Form
    {
        public ViewScheduleStudent()
        {
            InitializeComponent();
            loadgrid();

            lbldate.Text = DateTime.Now.ToString("dddd, dd MM yyyy / HH:mm:ss");
            lblname.Text = Session.name;
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

        void loadgrid()
        {
            string com = "select headerschedule.*, teacher.name, subject.name, subject.subjectid from headerschedule join teacher on headerschedule.teacherid = teacher.teacherid join subject on headerschedule.subjectid = subject.subjectid where headerschedule.classid = " + Session.classid;
            dataGridView1.DataSource = Command.data(com);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string com = "select headerschedule.*, teacher.name, subject.subjectid from headerschedule join teacher on headerschedule.teacherid = teacher.teacherid join subject on headerschedule.subjectid = subject.subjectid where subject.name like '%"+textBox1.Text+"%' and headerschedule.classid = " + Session.classid;
            dataGridView1.DataSource = Command.data(com);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            Selected.id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            Selected.cond = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                InfoSubject info = new InfoSubject();
                //this.Hide();
                info.ShowDialog();

            }
            else
            {
                MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
