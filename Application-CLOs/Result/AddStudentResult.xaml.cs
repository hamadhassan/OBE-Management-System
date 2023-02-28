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
    /// Interaction logic for AddStudentResult.xaml
    /// </summary>
    public partial class AddStudentResult : Window
    {
        int assessmentId;
        int studentId;
        public AddStudentResult()
        {
            InitializeComponent();
        }
        public AddStudentResult(int studentId)
        {
            InitializeComponent();
            this.studentId=studentId;
            settingComboBox();
        }
        private void settingComboBox()
        {
            cmbxAssessmentName.SelectedIndex = 0;
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
                lblMessage.Content= ex.Message;
            }
            return output;

        }
        
        private void bindAssessmentQuestions()
        {
           
            try
            {
                
                //////Bind the Title from the assessmnt relation into Combox Box
                var con = Configuration.getInstance().getConnection();

                AssessmentDL.LoadDataIntoList();
                foreach (string title in AssessmentDL.GetAssessmentTitle())
                {
                    cmbxAssessmentName.Items.Add(title);
                }
                cmbxAssessmentName.SelectedIndex = 0;
                ////Bind the Assessment Question from the selected assessment name
                /// This command retrive the Assessment ID corresponding to the name of the assessment
                string query = "SELECT A.Id FROM Assessment A WHERE A.Title='" + cmbxAssessmentName.Text + "'";
                string assessmentId = queryData(query);
                /// bind Assessment question 
                SqlCommand cmd3 = new SqlCommand("\r\nSELECT AC.Name\r\nFROM Assessment A\r\nJOIN AssessmentComponent AC\r\nON AC.AssessmentId=A.Id\r\nWHERE A.Id=" + assessmentId, con);
                SqlDataReader reader2 = cmd3.ExecuteReader();
                while (reader2.Read())
                {
                    // Add data to combo box
                    cmbxQuestion.Items.Add(reader2["Name"].ToString());
                }
                cmbxQuestion.SelectedIndex = 0;
                reader2.Close();
                //// Bind the measurment level from the rubric level relation into the combo box
                ///Firstly get the rubric id from the asssessment component relation name 
                query = "SELECT AC.RubricID FROM AssessmentComponent AC WHERE AC.Name='" + cmbxQuestion.Text + "'";
                string rubricID = queryData(query);
                /// bind measurment level 
                SqlCommand cmd4 = new SqlCommand("SELECT RL.MeasurementLevel FROM RubricLevel RL WHERE RL.RubricId=" + rubricID, con);
                SqlDataReader reader3 = cmd4.ExecuteReader();
                while (reader3.Read())
                {
                    // Add data to combo box
                    cmbxRubricLevel.Items.Add(reader3["MeasurementLevel"].ToString());
                }
                cmbxRubricLevel.SelectedIndex = 0;
                reader3.Close();

            }
            catch (Exception ex)
            {
                lblMessage.Content = ex.Message;
            }
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            ////Retrive First Name and Last Name
            string query = "SELECT CONCAT(S.FirstName,' ',S.LastName)\r\nFROM Student S\r\nWHERE ID=" + studentId;
            txtbxStudentName.Text=queryData(query);
            ////Retrive Registration Number
            query = "SELECT S.RegistrationNumber \r\nFROM Student S\r\nWHERE ID=" + studentId;
            txtbxRegistrationNumber.Text = queryData(query);
            bindAssessmentQuestions();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnViewRubric_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAssessmnet_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbxAssessmentName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          //  bindAssessmentQuestions();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // bindAssessmentQuestions();
        }
    }
}
