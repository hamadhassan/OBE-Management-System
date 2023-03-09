using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for StudentDashboard.xaml
    /// </summary>
    public partial class StudentDashboard : Window
    {
        public StudentDashboard()
        {
            InitializeComponent();
            loadData();
            loadTopperData();
            loadClosFailStudents();
            loadResultAssessmentWise();
            loadClassAttendance();
        }
        private void loadTopperData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT TOP 5 CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],S.RegistrationNumber,A.Title AS Assignment,A.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [Obtained Marks]\r\nFROM Student S\r\nJOIN StudentResult SR\r\nON SR.StudentId=S.Id\r\nJOIN AssessmentComponent AC\r\nON AC.Id=SR.AssessmentComponentId\r\nJOIN RubricLevel RL\r\nON RL.Id=SR.RubricMeasurementId\r\nJOIN  Assessment A\r\nON A.Id=AC.AssessmentId\r\nJOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML\r\nON ML.RubricId=AC.RubricId\r\nGROUP BY S.RegistrationNumber,A.Title,A.TotalMarks\r\nORDER BY [Obtained Marks] DESC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg1.ItemsSource = dt.DefaultView;
           
        }
        private void loadClosFailStudents()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT TOP 5 CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],S.RegistrationNumber,A.Title AS Assignment,A.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [ObtainedMarks],\r\nC.Name AS [CLOName]\r\nFROM Student S\r\nJOIN StudentResult SR\r\nON SR.StudentId=S.Id\r\nJOIN AssessmentComponent AC\r\nON AC.Id=SR.AssessmentComponentId\r\nJOIN RubricLevel RL\r\nON RL.Id=SR.RubricMeasurementId\r\nJOIN  Assessment A\r\nON A.Id=AC.AssessmentId\r\nJOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML\r\nON ML.RubricId=AC.RubricId\r\nJOIN Rubric R\r\nON R.Id=AC.RubricId\r\nJOIN Clo C\r\nON C.Id=R.CloId\r\nGROUP BY S.RegistrationNumber,A.Title,A.TotalMarks,C.Name\r\nHAVING  ((SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks))/A.TotalMarks)*100<=33\r\nORDER BY C.Name ASC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg2.ItemsSource = dt.DefaultView;

        }
        private void loadResultAssessmentWise()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("\r\nSELECT TOP 5 CONCAT(Max(S.FirstName),' ',Max(S.LastName))AS [StudentName],S.RegistrationNumber,A.Title AS Assignment,A.TotalMarks,SUM((CAST (RL.MeasurementLevel AS float)/ML.MaxLevel)*AC.TotalMarks) AS [ObtainedMarks]\r\nFROM Student S\r\nJOIN StudentResult SR\r\nON SR.StudentId=S.Id\r\nJOIN AssessmentComponent AC\r\nON AC.Id=SR.AssessmentComponentId\r\nJOIN RubricLevel RL\r\nON RL.Id=SR.RubricMeasurementId\r\nJOIN  Assessment A\r\nON A.Id=AC.AssessmentId\r\nJOIN (SELECT RL1.RubricId,MAX(RL1.MeasurementLevel) AS MaxLevel FROM RubricLevel RL1 GROUP BY RubricId) AS ML\r\nON ML.RubricId=AC.RubricId\r\nJOIN Rubric R\r\nON R.Id=AC.RubricId\r\nJOIN Clo C\r\nON C.Id=R.CloId\r\nWHERE A.Id=2\r\nGROUP BY S.RegistrationNumber,A.Title,A.TotalMarks\r\nORDER BY  A.Title ASC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg3.ItemsSource = dt.DefaultView;

        }
        private void loadClassAttendance()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT TOP 5 CONCAT(S.FirstName,' ',S.LastName)AS Name,S.RegistrationNumber,L.Name\r\nFROM Student S\r\nJOIN StudentAttendance ST\r\nON S.Id=ST.StudentId\r\nJOIN ClassAttendance C\r\nON C.Id=ST.AttendanceId\r\nJOIN Lookup L\r\nON L.LookupId=ST.AttendanceStatus\r\nWHERE L.Category='ATTENDANCE_STATUS' \r\nORDER BY C.AttendanceDate\r\n", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg4.ItemsSource = dt.DefaultView;

        }

        private void loadData()
        {
            string query = "SELECT COUNT(S.RegistrationNumber) FROM student S";
            lblCountTotalStudent.Content = queryData(query);
            query = "SELECT COUNT(L.Name) FROM Student S JOIN Lookup  L ON L.LookupId=S.Status \r\nWHERE  L.Category='STUDENT_STATUS' AND L.Name='Active'";
            lblCountActive.Content = queryData(query);
            query = "SELECT COUNT(L.Name) FROM Student S JOIN Lookup  L ON L.LookupId=S.Status \r\nWHERE  L.Category='STUDENT_STATUS' AND L.Name='InActive'";
            lblCountInactive.Content = queryData(query);
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            Home home=new Home();
            home.Show();
            this.Hide();
        }

        private void btnStudent_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddStudent addStudent=new AddStudent();
            addStudent.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ViewStudent viewStudent=new ViewStudent();
            viewStudent.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ViewStudent viewStudent=new ViewStudent();
            viewStudent.ShowDialog();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            ViewStudent viewStudent = new ViewStudent();
            viewStudent.ShowDialog();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ViewStudent viewStudent = new ViewStudent();
            viewStudent.ShowDialog();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
        private string queryData(string query)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            string output = cmd.ExecuteScalar().ToString();
            return output;
        }
        private void btnSetRules_Click(object sender, RoutedEventArgs e)
        {
            Lookup lookup = new Lookup();
            lookup.ShowDialog();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            
        }
    }
}
