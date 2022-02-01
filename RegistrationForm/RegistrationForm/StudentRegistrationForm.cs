using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RegistrationForm
{
    public partial class StudentRegistrationForm : Form
    {
        public StudentRegistrationForm()
        {
            InitializeComponent();
            Load();
        }

        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-APKAFFC; Initial Catalog=StudentInfo; Integrated Security=SSPI");
        SqlCommand cmd;
        SqlDataReader read;
        string id;
        bool Mode = true;
        string sql;

        //if mode is true add record else update record

        public void Load()
        {
            try
            {
                sql = "select * from Stud_Info";
                cmd = new SqlCommand(sql, conn);
                conn.Open();

                read = cmd.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (read.Read())
                {
                    dataGridView1.Rows.Add(read[0], read[1], read[2], read[3]);
                }

                conn.Close();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getID(string id)
        {
            sql = ("select * from Stud_Info where id='" + id + "'");
            cmd = new SqlCommand(sql, conn);
            conn.Open();
            read = cmd.ExecuteReader();

            while (read.Read())
            {
                txtStudName.Text = read[1].ToString();
                txtCourse.Text = read[2].ToString();
                txtCourseFee.Text = read[3].ToString();
            }

            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = txtStudName.Text;
            string course = txtCourse.Text;
            string fee = txtCourseFee.Text;

            if (Mode == true)
            {
                sql = "insert into Stud_Info(stud_name,course_name,fee) values(@stud_name,@course_name,@fee)";
                conn.Open();
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@stud_name", name);
                cmd.Parameters.AddWithValue("@course_name", course);
                cmd.Parameters.AddWithValue("@fee", fee);

                MessageBox.Show("New record inserted");

                cmd.ExecuteNonQuery();

                txtStudName.Clear();
                txtCourse.Clear();
                txtCourseFee.Clear();

                txtStudName.Focus();
            }

            else
            {
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "update Stud_Info set stud_name=@stud_name,course_name=@course_name,fee=@fee where id=@ID";
                conn.Open();
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@stud_name", name);
                cmd.Parameters.AddWithValue("@course_name", course);
                cmd.Parameters.AddWithValue("@fee", fee);
                cmd.Parameters.AddWithValue("@ID", id);

                MessageBox.Show("Record updated");

                cmd.ExecuteNonQuery();

                txtStudName.Clear();
                txtCourse.Clear();
                txtCourseFee.Clear();

                txtStudName.Focus();

                button1.Text = "Save";
                Mode = true;
            }

            conn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==dataGridView1.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                getID(id);
                button1.Text = "Edit";
            }

            else if(e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "delete from Stud_Info where id=@ID";
                conn.Open();
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Deleted");
                conn.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Load();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtStudName.Clear();
            txtCourse.Clear();
            txtCourseFee.Clear();

            txtStudName.Focus();

            button1.Text = "Save";

            Mode = true;
        }
    }
}
