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
    public partial class InfoSubject : Form
    {
        public InfoSubject()
        {
            InitializeComponent();

            getdata();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(Selected.cond == 1)
            {
                ViewScheduleStudent view = new ViewScheduleStudent();
                this.Hide();
                view.ShowDialog();
            }

            else if (Selected.cond == 2)
            {
                TeachingSchedule schedule = new TeachingSchedule();
                this.Hide();
                schedule.ShowDialog();
            }
        }

        void getdata()
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlCommand command = new SqlCommand("select * from subject where subjectid = " + Selected.id, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            lbltitle.Text = reader["name"].ToString();
            lbldesc.Text = reader["description"].ToString();
            lblassignment.Text = ": " + reader["asignment"].ToString() + "%";
            lblmid.Text = ": " + reader["mid_exam"].ToString() + "%";
            lblfinal.Text = ": " + reader["final_exam"].ToString() + "%";
            connection.Close();

        }
    }
}
