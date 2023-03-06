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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            LoadStatisticsData();
            LoadTopStudentMarks();
            LoadMostPunctionalStudents();
            LoadAssessments();
            LoadCLOs();
        }
        #region Navigation 
        private void btnStudent_Click(object sender, RoutedEventArgs e)
        {
            StudentDashboard studentDashboard = new StudentDashboard();
            this.Hide();
            studentDashboard.Show();
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
        }

        private void btnCLOs_Click(object sender, RoutedEventArgs e)
        {
            AddCLOs addCLOs = new AddCLOs();
            addCLOs.ShowDialog();
        }

        private void btnLevels_Click(object sender, RoutedEventArgs e)
        {
            AddRubricLevel addRubricLevel = new AddRubricLevel();
            addRubricLevel.ShowDialog();
        }

        private void btnRubric_Click(object sender, RoutedEventArgs e)
        {
            AddRubric addRubric = new AddRubric();
            addRubric.ShowDialog();
        }
        private void btnView_Student_Click_1(object sender, RoutedEventArgs e)
        {
            ViewStudent viewStudent = new ViewStudent();
            viewStudent.ShowDialog();
        }

        private void btnMark_Attendance_Click(object sender, RoutedEventArgs e)
        {
            StudentAttendance studentAttendance = new StudentAttendance(true);
            studentAttendance.ShowDialog();
        }

        private void btnView_Result_Click(object sender, RoutedEventArgs e)
        {
            ViewStudentResult viewStudent = new ViewStudentResult();
            viewStudent.ShowDialog();
        }

        private void btnAdd_Assessment_Click(object sender, RoutedEventArgs e)
        {
            AddAssessment addAssessment = new AddAssessment();
            addAssessment.ShowDialog();
        }

        private void btnAdd_Questions_Click(object sender, RoutedEventArgs e)
        {
            AddAssessmentComponent addAssessmentComponent = new AddAssessmentComponent();
            addAssessmentComponent.ShowDialog();
        }

        private void btnAdd_Rubric_Click(object sender, RoutedEventArgs e)
        {
            AddRubric addRubric = new AddRubric();
            addRubric.ShowDialog();
        }
        #endregion

        private string QueryData(string query)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            string output = cmd.ExecuteScalar().ToString();
            return output;
        }
        private void LoadStatisticsData()
        {
            string query = "SELECT COUNT(S.RegistrationNumber) FROM student S";
            lblTotalStudent.Content = QueryData(query);
            query = "SELECT COUNT(ID) FROM Assessment";
            lblTotalAssessment.Content = QueryData(query);
            query = "SELECT COUNT(AC.Id) FROM AssessmentComponent AC";
            lblTotalQuestion.Content = QueryData(query);
            query = "SELECT COUNT(R.Id) FROM Rubric R";
            lblTotalRubric.Content = QueryData(query);
            query = "SELECT COUNT(RL.Id) FROM RubricLevel RL\r\n";
            lblTotalLevels.Content = QueryData(query);
            query = "SELECT COUNT(C.Id) FROM Clo C";
            lblTotalCLOs.Content = QueryData(query);
            query = "SELECT COUNT(CA.Id) FROM ClassAttendance CA\r\n";
            lblTotalAttendance.Content = QueryData(query);
        }
        private void LoadTopStudentMarks()
        {
            string query = "DECLARE @MaxLevel as float=( SELECT MAx(RL.MeasurementLevel) FROM RubricLevel RL WHERE RL.RubricId IN (SELECT AC.RubricId FROM StudentResult SR JOIN AssessmentComponent AC ON SR.AssessmentComponentId=AC.Id))\r\nSELECT TOP 5 CONCAT(S.FirstName,' ',S.LastName) AS [Student Name],A.Title AS [Assessmmet Name],\r\n(RL.MeasurementLevel/@MaxLevel)*AC.TotalMarks AS[Obtained Marks]\r\nFROM StudentResult SR\r\nJOIN Student S\r\nON S.Id=SR.StudentId\r\nJOIN RubricLevel RL\r\nON RL.Id=SR.RubricMeasurementId\r\nJOIN Rubric R\r\nON R.Id=RL.RubricId\r\nJOIN AssessmentComponent AC\r\nON AC.ID=SR.AssessmentComponentId\r\nJOIN Assessment A\r\nON A.Id=AC.AssessmentId\r\nORDER BY [Obtained Marks] DESC";
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg1.ItemsSource = dt.DefaultView;
            
        }
        private void LoadMostPunctionalStudents()
        {
            string query = "SELECT  DISTINCT TOP 5 CONCAT(S.FirstName,' ',S.LastName)AS [Student Name],S.RegistrationNumber\r\nFROM Student S\r\nJOIN StudentAttendance SA\r\nON SA.StudentId=S.Id\r\nJOIN ClassAttendance CA\r\nON CA.Id=SA.AttendanceId\r\nJOIN Lookup L\r\nON L.LookupId=SA.AttendanceStatus\r\nWHERE L.Name='Present'\r\nORDER BY S.RegistrationNumber DESC \r\n";
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg2.ItemsSource = dt.DefaultView;

        }
        private void LoadAssessments()
        {
            string query = "SELECT TOP 5 A.Title AS Name, A.TotalMarks,A.TotalWeightage FROM Assessment A\r\n";
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg3.ItemsSource = dt.DefaultView;

        }
        private void LoadCLOs()
        {
            string query = "SELECT TOP 5 C.Name as [CLO Name],R.Details AS [Rubric Name] FROM Clo C JOIN Rubric R ON R.CloId=c.Id\r\n";
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dg4.ItemsSource = dt.DefaultView;

        }
    }
}
