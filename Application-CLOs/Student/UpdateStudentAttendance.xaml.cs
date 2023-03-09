using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Interaction logic for UpdateStudentAttendance.xaml
    /// </summary>
    public partial class UpdateStudentAttendance : Window
    {
        public UpdateStudentAttendance()
        {
            InitializeComponent();
            WindowActivitedLoad();
        }
        private void WindowActivitedLoad()
        {
            var date = Convert.ToDateTime(dateTimePicker.SelectedDate).ToString("yyyy-MM-dd");
          
            if (dateTimePicker.SelectedDate != null)
            {
                if (date != null)
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("SELECT CONCAT(S.FirstName,' ',S.LastName)AS [Student Name],S.RegistrationNumber,L.Name AS Status,S.Id,SA.AttendanceId\r\nFROM Student S\r\nJOIN StudentAttendance SA\r\nON S.Id=SA.StudentId\r\nJOIN ClassAttendance CA\r\nON CA.Id=SA.AttendanceId\r\nJOIN Lookup L\r\nON L.LookupId=SA.AttendanceStatus\r\nWHERE CA.AttendanceDate=@Date\r\nORDER BY RegistrationNumber", con);
                    cmd.Parameters.AddWithValue("@Date", date);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgAttendance.ItemsSource = dt.DefaultView;
                    if (dgAttendance.Columns.Count > 1)
                    {
                        dgAttendance.Columns[1].Visibility = Visibility.Hidden;
                        dgAttendance.Columns[2].Visibility = Visibility.Hidden;
                        dgAttendance.Columns[6].Visibility = Visibility.Hidden;
                        dgAttendance.Columns[7].Visibility = Visibility.Hidden;
                    }
                }
            }

        }
        private void bindData()
        {
            var date = Convert.ToDateTime(dateTimePicker.SelectedDate).ToString("yyyy-MM-dd");
            if (date != null)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT CONCAT(S.FirstName,' ',S.LastName)AS [Student Name],S.RegistrationNumber,L.Name AS Status,S.Id,SA.AttendanceId\r\nFROM Student S\r\nJOIN StudentAttendance SA\r\nON S.Id=SA.StudentId\r\nJOIN ClassAttendance CA\r\nON CA.Id=SA.AttendanceId\r\nJOIN Lookup L\r\nON L.LookupId=SA.AttendanceStatus\r\nWHERE CA.AttendanceDate=@Date\r\nORDER BY RegistrationNumber", con);
                cmd.Parameters.AddWithValue("@Date", date);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgAttendance.ItemsSource = dt.DefaultView;
                if (dgAttendance.Columns.Count > 1)
                {
                    dgAttendance.Columns[4].Visibility = Visibility.Hidden;
                    dgAttendance.Columns[5].Visibility = Visibility.Hidden;
                }
            }
            else
            {
                lblMessage.Content = "Select the date";
            }
        }

        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            if (dateTimePicker.SelectedDate != null)
            {
                try
                {
                    DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                    int studentId = int.Parse(dataRowView[3].ToString());
                    int classAttendanceId = int.Parse(dataRowView[4].ToString());
                    lblMessage.Content = studentId.ToString();
                    StudentAttendanceMore studentAttendanceMore = new StudentAttendanceMore(studentId, classAttendanceId);
                    studentAttendanceMore.ShowDialog();
                }
                catch
                {
                    lblMessage.Content = "You are trying to access the wrong field";
                }
            }
            else
            {
                lblMessage.Content = "Select the date";
            }

        }

        private void dateTimePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            bindData();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
        }
    }
}
