using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
using System.Xml.Linq;
using System.Data;

namespace Application_CLOs
{
    /// <summary>
    /// Interaction logic for StudentResultDashboard.xaml
    /// </summary>
    public partial class StudentResultDashboard : Window
    {
        private int cloId;
        private int assessmentId;
        private int assessmentComponentId;

        public StudentResultDashboard()
        {
            InitializeComponent();
            bindDataGrid();
            loadCLOs();
            loadAssessment();
            loadQuestions();
        }
        private void bindDataGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT S.id,S.FirstName,S.lastname,S.RegistrationNumber FROM Student S ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgIndividual.ItemsSource = dt.DefaultView;
            //if (dgStudent.Columns.Count > 1)
            //{
            //    dgStudent.Columns[2].Visibility = Visibility.Hidden;
            //}
        }

        private void btnUpdateEvaluation_Click(object sender, RoutedEventArgs e)
        {
            ViewStudentResult viewStudentResult=new ViewStudentResult();
            viewStudentResult.ShowDialog();
        }

        private void btnMarkEvaluation_Click(object sender, RoutedEventArgs e)
        {
            ViewStudentForMarkeResult viewStudentForMarkeResult = new ViewStudentForMarkeResult();
            viewStudentForMarkeResult.ShowDialog();
        }

        private void btnResultCLOsWise_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save PDF File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                Reports.getInstance().GenerateCLOWiseResultReport(saveFileDialog,cloId);
            }
        }
        private string queryData(string query)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            string output = cmd.ExecuteScalar().ToString();
            return output;
        }
        private void loadCLOs()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT C.Name FROM CLO C", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Add data to combo box
                cmbxResultClosWise.Items.Add(reader["Name"].ToString());
            }
            reader.Close();
            cmbxResultClosWise.SelectedIndex = 0;

        }

        private void cmbxResultClosWise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string query = "SELECT C.Id FROM Clo C WHERE C.Name='"+cmbxResultClosWise.SelectedItem.ToString() + "'";
            cloId = int.Parse(queryData(query));
        }
        private void loadAssessment()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT A.Title FROM Assessment A", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Add data to combo box
                cmbxResultAssessmentWise.Items.Add(reader["Title"].ToString());
            }
            reader.Close();
            cmbxResultAssessmentWise.SelectedIndex = 0;

        }
        private void cmbxResultAssessmentWise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string query = "\r\nSELECT A.Id FROM Assessment A WHERE A.Title='"+ cmbxResultAssessmentWise.SelectedItem.ToString() + "'";
            assessmentId = int.Parse(queryData(query));
        }
        private void btnResultAssignemntWise_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save PDF File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                Reports.getInstance().GenerateAssessmentWiseResultReport(saveFileDialog, assessmentId);
            }

        }
        private void loadQuestions()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT AC.Name FROM AssessmentComponent AC \r\n", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Add data to combo box
                cmbxResultQuestionWise.Items.Add(reader["Name"].ToString());
            }
            reader.Close();
            cmbxResultQuestionWise.SelectedIndex = 0;

        }

        private void cmbxResultQuestionWise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string query = "SELECT AC.Id FROM AssessmentComponent AC WHERE AC.Name='" + cmbxResultQuestionWise.SelectedItem.ToString() + "'";
            assessmentComponentId = int.Parse(queryData(query));

        }

        private void btnResultQuestionWise_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save PDF File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                Reports.getInstance().GenerateAssessmentComponentWiseResultReport(saveFileDialog, assessmentComponentId);
            }
          
        }

        private void btnTop10Students_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save PDF File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                Reports.getInstance().GenerateTop10StudentReport(saveFileDialog);
            }
        }

        private void btnResult_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            int studentId = int.Parse(dataRowView[0].ToString());
            string firstName = dataRowView[1].ToString();
            string lastName = dataRowView[2].ToString();
            string registratioNumber = dataRowView[3].ToString();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save PDF File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                Reports.getInstance().GenerateIndividualStudentReport(saveFileDialog,studentId,firstName+" "+lastName,registratioNumber);
            }
            

        }

        private void btnAttendance_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            int studentId = int.Parse(dataRowView[0].ToString());
            string firstName = dataRowView[1].ToString();
            string lastName = dataRowView[2].ToString();
            string registratioNumber = dataRowView[3].ToString();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save PDF File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                Reports.getInstance().GenerateIndividualStudentAttendanceReport(saveFileDialog, studentId, firstName + " " + lastName, registratioNumber);
            }

        }

        private void btnStudentFailClos_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save PDF File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                Reports.getInstance().GenerateFailCLOWiseResultReport(saveFileDialog,33);
            }
        }
    }
}
