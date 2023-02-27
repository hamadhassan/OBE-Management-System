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
                MessageBox.Show(output);
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
                ////bindAssessmentName into Combox Box
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("SELECT A.Title FROM Assessment A", con);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    // Add data to combo box
                    cmbxAssessmentName.Items.Add(reader1["Title"].ToString());
                }
                cmbxAssessmentName.SelectedIndex = 0;
                //// This command retrive the Assessment ID corresponding to the name of the assessment
                //// Then bind the correspoinding Assessment Question 
                //string query = "SELECT A.Id FROM Assessment A WHERE A.Title='" + cmbxAssessmentName.Text + "'";
                string query = "SELECT A.Id FROM Assessment A WHERE A.Title='Quiz123'";
                string assessmentId = queryData(query);
                MessageBox.Show("ID is ", assessmentId);
                /// bind Assessment question 
                SqlCommand cmd3 = new SqlCommand("\r\nSELECT AC.Name\r\nFROM Assessment A\r\nJOIN AssessmentComponent AC\r\nON AC.AssessmentId=A.Id\r\nWHERE A.Id=" + assessmentId, con);
                SqlDataReader reader2 = cmd3.ExecuteReader();
                while (reader2.Read())
                {
                    // Add data to combo box
                    cmbxQuestion.Items.Add(reader2["Name"].ToString());
                }
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
