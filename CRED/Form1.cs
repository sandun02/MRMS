using System;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace CRED
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=sandunDB;Integrated Security=True");

        private void Form1_Load(object sender, EventArgs e)
        {
            GetStudentsRecord();
        }

        private void GetStudentsRecord()
        {
            

            SqlCommand cmd = new SqlCommand("Select * from tbStudent", con);
            DataTable dt = new DataTable();
            
            con.Open();


            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            StudentsRecordDataGridView.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                string gender;
                if (RMale.Checked == true)
                {
                    gender = "Male";
                }
                else if(RFemale.Checked == true)
                {
                    gender = "Female";
                }
                else
                {
                    gender = "Other";
                }

                SqlCommand cmd = new SqlCommand("INSERT INTO tbStudent VALUES(@Name,@Age,@Gender,@Address,@Mobile,@Degree_Program)",con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", txtStudentName.Text );
                cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                cmd.Parameters.AddWithValue("@Degree_Program", cmbPrograme.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("New student is successfully saved in the database!!","Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                GetStudentsRecord();
                ResetFormControls();

            }
        }

        private bool IsValid()
        {
            if(txtStudentName.Text == string.Empty)
            {
                MessageBox.Show("Student Name is requied","Faild",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ResetFormControls();

        }

        private void ResetFormControls()
        {
            StudentID = 0;
            txtStudentName.Clear();
            txtAge.Clear();
            txtAddress.Clear();
            txtMobile.Clear();
            cmbPrograme.Items.Clear();

            txtStudentName.Focus();
        }

        public int StudentID;

        private void StudentsRecordDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StudentID = Convert.ToInt32(StudentsRecordDataGridView.SelectedRows[0].Cells[0].Value); 
            txtStudentName.Text = StudentsRecordDataGridView.SelectedRows[0].Cells[1].Value.ToString();
            txtAge.Text = StudentsRecordDataGridView.SelectedRows[0].Cells[2].Value.ToString();

            if("Male" == StudentsRecordDataGridView.SelectedRows[0].Cells[3].Value.ToString())
            {
                RMale.Checked=true;
            }
            else
            {
                RFemale.Checked=true;
            }

            txtAddress.Text = StudentsRecordDataGridView.SelectedRows[0].Cells[4].Value.ToString();
            txtMobile.Text = StudentsRecordDataGridView.SelectedRows[0].Cells[5].Value.ToString();
            cmbPrograme.Text = StudentsRecordDataGridView.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(StudentID > 0)
            {
                string gender;
                if (RMale.Checked == true)
                {
                    gender = "Male";
                }
                else if (RFemale.Checked == true)
                {
                    gender = "Female";
                }
                else
                {
                    gender = "Other";
                }
                SqlCommand cmd = new SqlCommand("UPDATE tbStudent SET Name = @Name,Age = @Age,Gender = @Gender,Address = @Address,Mobile = @Mobile,Degree_Program = @Degree_Program WHERE StudentID = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", txtStudentName.Text);
                cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                cmd.Parameters.AddWithValue("@Degree_Program", cmbPrograme.Text);

                cmd.Parameters.AddWithValue("@ID", this.StudentID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Student Information is Updated successfully!!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetStudentsRecord();
                ResetFormControls();

            }
            else
            {
                MessageBox.Show("Please Select an Student to Update his information", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(StudentID > 0)
            {
                string gender;
                if (RMale.Checked == true)
                {
                    gender = "Male";
                }
                else if (RFemale.Checked == true)
                {
                    gender = "Female";
                }
                else
                {
                    gender = "Other";
                }
                SqlCommand cmd = new SqlCommand("DELETE FROM tbStudent WHERE StudentID = @ID", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@ID", this.StudentID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Student Information is Deleted successfully!!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetStudentsRecord();
                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Please Select an Student to Delete his information", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
