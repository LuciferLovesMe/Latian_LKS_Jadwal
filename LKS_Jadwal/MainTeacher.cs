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
    public partial class MainTeacher : Form
    {
        public MainTeacher()
        {
            InitializeComponent();
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

        private void panelclass_Click(object sender, EventArgs e)
        {
            TeachingSchedule schedule = new TeachingSchedule();
            this.Hide();
            schedule.ShowDialog();
        }

        private void panelEdit_Click(object sender, EventArgs e)
        {
            EditProfileStudent edit = new EditProfileStudent();
            this.Hide();
            edit.ShowDialog();
        }
    }
}
