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
using System.Collections.ObjectModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Application_CLOs
{
    /// <summary>
    /// Interaction logic for StudentAttendance.xaml
    /// </summary>
    public partial class StudentAttendance : Window
    {
        /// <summary>
        /// IF isForAllPresent is true then show present column 
        /// Else Show absent column
        /// Means that All class is present and some student is absent and vice versa 
        /// 1 IS ID FOR PRESENT 
        /// 2 IS ID FOR ABSENT 
        /// </summary>
        private bool isForAllPresent=true;
        private bool isDateSelected = true;
        private int classAttendanceID = -1;
        public StudentAttendance()
        {
            InitializeComponent();
            timer();
            cmbxCategory.SelectedIndex = 0;
        }
        public StudentAttendance(bool isForAllPresent)
        {
            InitializeComponent();
            this.isForAllPresent = isForAllPresent;
            timer();
            cmbxCategory.SelectedIndex = 0;
        }
        private void bindDataGrid()
        {
            if (cmbxCategory.SelectedIndex == 0)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT S.Id,CONCAT(S.FirstName,' ',S.LastName) AS [Student Name],S.RegistrationNumber \r\nFROM Student S \r\nJOIN Lookup L\r\nON L.LookupId=S.Status\r\nWHERE L.Category='STUDENT_STATUS' AND L.Name='Active'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgAttendance.ItemsSource = dt.DefaultView;
                if (dgAttendance.Columns.Count > 1)
                {
                    dgAttendance.Columns[3].Visibility = Visibility.Hidden;
                }
                if (isForAllPresent)
                {
                    dgAttendance.Columns[2].Visibility = Visibility.Hidden;

                }
                else
                {//User come as majority is absent and some student is present
                    dgAttendance.Columns[1].Visibility = Visibility.Hidden;
                }
            }
            else
            {
               
            }
           
        }
        private void markInitialAttendance(string date,int attendanceStatus)
        {
            if (cmbxCategory.SelectedIndex == 0)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("INSERT INTO ClassAttendance(AttendanceDate) VALUES(@CurrentDate)\r\nDECLARE @AttendanceStatus  INT = @Status\r\nDECLARE @ClassAttendanceID  INT = (SELECT TOP 1 CA.Id FROM ClassAttendance CA ORDER BY CA.ID DESC)\r\nDECLARE @Counter  BIGINT = 0\r\nDECLARE @RowCount BIGINT = 0\r\nSET @Counter=0\r\nSELECT @RowCount =Count(S.Id) FROM Student S  JOIN Lookup L ON L.LookupId=S.Status  WHERE L.Category='STUDENT_STATUS' AND L.Name='Active'\r\nWHILE ( @Counter < @RowCount)\r\nBEGIN\r\n\tDECLARE @idStudent as INT =(SELECT S.Id FROM Student S  JOIN Lookup L ON L.LookupId=S.Status  WHERE L.Category='STUDENT_STATUS' AND L.Name='Active' \r\n\tORDER BY S.Id OFFSET @Counter ROWS   FETCH NEXT 1 ROWS ONLY)\r\n\tINSERT INTO StudentAttendance(AttendanceId,StudentId,AttendanceStatus) VALUES(@ClassAttendanceID,@idStudent,@AttendanceStatus)\r\n    SET @Counter  = @Counter  + 1\r\nEND", con);
                cmd1.Parameters.AddWithValue("@CurrentDate", date);
                cmd1.Parameters.AddWithValue("@Status", attendanceStatus);
                cmd1.ExecuteNonQuery();
            }
            else
            {

            }
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            var date = Convert.ToDateTime(dateTimePicker.SelectedDate).ToString("yyyy-MM-dd");
            if(cmbxCategory.SelectedIndex==1)
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

        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxCategory.SelectedIndex == 0)
            {
                if (dateTimePicker.SelectedDate != null)
                {
                    try
                    {
                        DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                        int studentId = int.Parse(dataRowView[0].ToString());
                        StudentAttendanceMore studentAttendanceMore = new StudentAttendanceMore(studentId, classAttendanceID);
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
            else
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
          
        }

        private void btnAbsent_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxCategory.SelectedIndex == 0)
            {
                if (dateTimePicker.SelectedDate != null)
                {
                    try
                    {
                        ////IF this button clicked then the mark present 
                        Button button = sender as Button;
                        if (button != null)
                        {
                            DataGridRow row = FindAncestor<DataGridRow>(button);
                            var cell = FindVisualParent<DataGridCell>(button);
                            if (row != null)
                            {
                                button.Visibility = Visibility.Hidden;
                                DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                                int studentId = int.Parse(dataRowView[0].ToString());

                                var con = Configuration.getInstance().getConnection();
                                SqlCommand cmd1 = new SqlCommand("UPDATE StudentAttendance SET AttendanceStatus=@Status WHERE  AttendanceId=@ClassAttendanceID AND StudentId=@StudentID ", con);
                                cmd1.Parameters.AddWithValue("@Status", 1);
                                cmd1.Parameters.AddWithValue("@ClassAttendanceID", classAttendanceID);
                                cmd1.Parameters.AddWithValue("@StudentID", studentId);
                                cmd1.ExecuteNonQuery();
                                lblMessage.Content = "Saved Successfully";
                                button.Visibility = Visibility.Hidden;
                                cell.IsEnabled = false;
                            }
                        }
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
            
        }

        private void btnPresent_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxCategory.SelectedIndex == 0)
            {
                if (dateTimePicker.SelectedDate != null)
                {
                    try
                    {
                        ////IF this button clicked then the mark absent 
                        Button button = sender as Button;
                        if (button != null)
                        {
                            DataGridRow row = FindAncestor<DataGridRow>(button);
                            var cell = FindVisualParent<DataGridCell>(button);
                            if (row != null)
                            {
                                DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                                int studentId = int.Parse(dataRowView[0].ToString());

                                var con = Configuration.getInstance().getConnection();
                                SqlCommand cmd1 = new SqlCommand("UPDATE StudentAttendance SET AttendanceStatus=@Status WHERE  AttendanceId=@ClassAttendanceID AND StudentId=@StudentID ", con);
                                cmd1.Parameters.AddWithValue("@Status", 2);
                                cmd1.Parameters.AddWithValue("@ClassAttendanceID", classAttendanceID);
                                cmd1.Parameters.AddWithValue("@StudentID", studentId);
                                cmd1.ExecuteNonQuery();
                                lblMessage.Content = "Saved Successfully";
                                button.Visibility = Visibility.Hidden;
                                cell.IsEnabled = false;
                            }
                        }
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
           
           
        }
        public T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent == null) return null;
            var parentT = parent as T;
            return parentT ?? FindVisualParent<T>(parent);
        }
        public T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            current = VisualTreeHelper.GetParent(current);

            while (current != null)
            {
                T result = current as T;
                if (result != null)
                {
                    return result;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }
        private void dateTimePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = Convert.ToDateTime(dateTimePicker.SelectedDate).ToString("yyyy-MM-dd");
            if (cmbxCategory.SelectedIndex == 0)
            {
                bindDataGrid();

                if (isDateSelected)
                {
                    if (isForAllPresent)
                    {
                        markInitialAttendance(date, 1);

                    }
                    else
                    {
                        markInitialAttendance(date, 2);
                    }
                    string query = "SELECT TOP 1 CA.Id FROM ClassAttendance CA ORDER BY CA.ID DESC\r\n";
                    classAttendanceID = int.Parse(queryData(query));
                    isDateSelected = false;
                }
                else
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd1 = new SqlCommand("UPDATE ClassAttendance SET AttendanceDate=@CurrentDate WHERE ID=@ID_", con);
                    cmd1.Parameters.AddWithValue("@CurrentDate", date);
                    cmd1.Parameters.AddWithValue("@ID_", classAttendanceID);
                    cmd1.ExecuteNonQuery();
                    lblMessage.Content = "Saved Successfully";
                }
            }
            else
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
                else
                {
                    lblMessage.Content = "Select the date";
                }
               
            }
           
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bindDataGrid();
        }
        private void timer()
        {
            DispatcherTimer dtClockTime = new DispatcherTimer();

            dtClockTime.Interval = new TimeSpan(0, 0, 3); //in Hour, Minutes, Second.
            dtClockTime.Tick += dtClockTime_Tick;
            dtClockTime.Start();
        }
        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            lblMessage.Content = "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
