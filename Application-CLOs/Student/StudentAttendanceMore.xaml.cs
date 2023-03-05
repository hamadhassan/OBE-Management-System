using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Application_CLOs
{
    /// <summary>
    /// Interaction logic for StudentAttendanceMore.xaml
    /// </summary>
    public partial class StudentAttendanceMore : Window
    {
        private int studentId;
        private int attendanceId;
        public StudentAttendanceMore()
        {
            InitializeComponent();
        }
        public StudentAttendanceMore(int studentId, int attendanceId)
        {
            InitializeComponent();
            this.studentId = studentId;
            this.attendanceId = attendanceId;
        }


        private void Window_Activated(object sender, EventArgs e)
        {
            
            bindStudentInfo();
        }
        private void bindStatus()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT L.Name FROM Lookup L WHERE L.Category='ATTENDANCE_STATUS'\r\n", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Add data to combo box
                cmbxStatus.Items.Add(reader["Name"].ToString());
            }
            reader.Close();
            cmbxStatus.SelectedIndex = 0;
        }
        private void bindStudentInfo()
        {
            string query = "SELECT CONCAT(S.FirstName,' ',S.LastName) FROM Student S WHERE S.Id="+studentId;
            txtbxName.Text = queryData(query);
            query = "SELECT S.RegistrationNumber FROM Student S WHERE S.Id=" + studentId;
            txtbxRegistratioNumber.Text = queryData(query);
        }
        private string queryData(string query)
        {
            string output = null;
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(query, con);
                output = cmd.ExecuteScalar().ToString();
            }

            catch (Exception ex)
            {
                lblMessage.Content = ex.Message;
            }
            return output;

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("UPDATE StudentAttendance SET AttendanceStatus=@Status WHERE  AttendanceId=@ClassAttendanceID AND StudentId=@StudentID ", con);
            cmd1.Parameters.AddWithValue("@Status", cmbxStatus.SelectedIndex+1);
            cmd1.Parameters.AddWithValue("@ClassAttendanceID", attendanceId);
            cmd1.Parameters.AddWithValue("@StudentID", studentId);
            cmd1.ExecuteNonQuery();
            lblMessage.Content = "Saved Successfully";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bindStatus();
        }
    }
}
