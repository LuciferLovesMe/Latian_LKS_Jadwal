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
    public partial class MainAdmin : Form
    {
        public MainAdmin()
        {
            InitializeComponent();

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

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to exit ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
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
    }
}
