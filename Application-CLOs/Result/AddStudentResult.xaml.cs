using System;
using System.Collections;
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
        int studentId;
        public AddStudentResult()
        {
            InitializeComponent();
        }
        public AddStudentResult(int studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
           // settingComboBox();
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
                lblMessage.Content = ex.Message;
            }
            return output;

        }
        private void bindAssessmentTitle()
        {

            //////Bind the Title from the assessmnt relation into Combox Box
            //var con = Configuration.getInstance().getConnection();
            //SqlCommand cmd1 = new SqlCommand("SELECT A.Title FROM Assessment A", con);
            //SqlDataReader reader1 = cmd1.ExecuteReader();
            //while (reader1.Read())
            //{
            //    // Add data to combo box
            //    cmbxAssessmentName.Items.Add(reader1["Title"].ToString());
            //}
            //reader1.Close();
            //cmbxAssessmentName.SelectedIndex = 0;
            cmbxAssessmentName.Items.Clear();
            AssessmentDL.LoadDataIntoList();
            foreach(string a in AssessmentDL.GetAssessmentTitle())
            {
                cmbxAssessmentName.Items.Add(a);
            }
            cmbxAssessmentName.SelectedIndex = 0;

        }
        private void bindAssessmentQuestions(int assessmentid)
        {
            //////Bind the Assessment Question from the selected assessment name
            //var con = Configuration.getInstance().getConnection();
            ///// bind Assessment question 
            //SqlCommand cmd3 = new SqlCommand("SELECT AC.Name FROM AssessmentComponent AC WHERE  AC.ID=" + assessmentid, con);
            //SqlDataReader reader2 = cmd3.ExecuteReader();
            //while (reader2.Read())
            //{
            //    // Add data to combo box
            //    cmbxQuestion.Items.Add(reader2["Name"].ToString());
            //}
            //reader2.Close();
            cmbxQuestion.Items.Clear();
            AssessmentComponentDL.LoadDataIntoList();
            foreach (string a in AssessmentComponentDL.GetAssessmentComponentNameFromAssessmentId(assessmentid))
            {
                cmbxQuestion.Items.Add(a);
            }
            cmbxQuestion.SelectedIndex = 0;
            if (cmbxQuestion.Items.Count== 0)
            {
                lblMessage.Content="Data is not available of "+ cmbxAssessmentName.SelectedItem.ToString();
                cmbxRubricLevel.Items.Clear();
            }
            else
            {
                cmbxQuestion.SelectedIndex = 0;
                lblMessage.Content = "";
            }
           

            try
            {
               
            }
            catch (Exception ex)
            {
                lblMessage.Content = ex.Message;
            }
        }
        private void bindMeasuremntLevel(int rubricId)
        {


            cmbxRubricLevel.Items.Clear();
            if (RubricLevelDL.isMeasuremntLevelExist(rubricId))
            {
                foreach (int level in RubricLevelDL.GetMeasurementLevelFromRubricId(rubricId))
                {
                    cmbxRubricLevel.Items.Add(level);
                }
                cmbxRubricLevel.SelectedIndex = 0;
            }
           
            //// Bind the measurment level from the rubric level relation into the combo box
            if (cmbxQuestion.Items.Count> 0)
            {
                //var con = Configuration.getInstance().getConnection();
                //SqlCommand cmd4 = new SqlCommand("\r\nSELECT RL.MeasurementLevel\r\nFROM RubricLevel RL\r\nJOIN AssessmentComponent AC\r\nON AC.RubricId=RL.RubricId\r\nWHERE AC.Id IN(SELECT AC1.Id FROM AssessmentComponent AC1 WHERE AC1.Name='" + cmbxQuestion.SelectedItem.ToString() + "')", con);
                //SqlDataReader reader3 = cmd4.ExecuteReader();
                //while (reader3.Read())
                //{
                //    // Add data to combo box
                //    cmbxRubricLevel.Items.Add(reader3["MeasurementLevel"].ToString());
                //}
                //reader3.Close();
              

            }
            ////   Display the signal
            if (cmbxRubricLevel.Items.Count == 0 && cmbxQuestion.Items.Count > 0)
            {
                lblMessage.Content = "Data is not available of " + cmbxQuestion.SelectedItem.ToString();
                cmbxRubricLevel.Items.Clear();
            }
            else
            {
                lblMessage.Content = "";
            }

        }
        private void Window_Activated(object sender, EventArgs e)
        {
            ////Retrive First Name and Last Name
            string query = "SELECT CONCAT(S.FirstName,' ',S.LastName)\r\nFROM Student S\r\nWHERE ID=" + studentId;
            txtbxStudentName.Text = queryData(query);
            ////Retrive Registration Number
            query = "SELECT S.RegistrationNumber \r\nFROM Student S\r\nWHERE ID=" + studentId;
            txtbxRegistrationNumber.Text = queryData(query);
            
            //bindAssessmentQuestions();
            //bindMeasuremntLevel();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnViewRubric_Click(object sender, RoutedEventArgs e)
        {
            AddRubric addRubric = new AddRubric();
            addRubric.ShowDialog();
        }

        private void btnAssessmnet_Click(object sender, RoutedEventArgs e)
        {
            AddAssessment addAssessment = new AddAssessment();
            addAssessment.ShowDialog();
           
        }

        private void cmbxAssessmentName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int assessmentId = AssessmentDL.GetAssessemntIdFromTitle(cmbxAssessmentName.SelectedItem.ToString());
            bindAssessmentQuestions(assessmentId);
        }
        private void cmbxQuestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbxQuestion.Items.Count > 0 && cmbxAssessmentName.Items.Count > 0)
            {
                //int assessmentId = AssessmentDL.GetAssessemntIdFromTitle(cmbxAssessmentName.SelectedItem.ToString());
                //int rubricId = AssessmentComponentDL.GetRubricIdFromAssesmentComponentName(cmbxQuestion.SelectedItem.ToString(), assessmentId);
                int rubricId = AssessmentComponentDL.GetRubricId(cmbxAssessmentName.SelectedItem.ToString(), cmbxQuestion.SelectedItem.ToString());
                bindMeasuremntLevel(rubricId);
                ////Mapped CLO 
                MappedCLOFromRubricID(rubricId);
                ////Maximum Rubric Level
                TotalMarks(rubricId);
                ////Obtained Marks
                ObtainedMarks();


            }
          
        }
        private void MappedCLOFromRubricID(int rubricId)
        {
            string query = "SELECT C.Name\r\nFROM Clo C\r\nJOIN Rubric R\r\nON C.Id=R.CloId\r\nWHERE R.Id=" + rubricId;
            txtbxMappedCLO.Text = queryData(query);
        }
        private void TotalMarks(int rubricId)
        {
            string query = "SELECT Max(RL.MeasurementLevel)\r\nFROM RubricLevel RL\r\nWHERE RL.RubricId=" + rubricId;
            txtbxTotalMarks.Text= queryData(query);
        }
        private void ObtainedMarks()
        {
            if(cmbxQuestion.Items.Count > 0 && cmbxAssessmentName.Items.Count > 0)
            {
                txtbxObtainedMarks.Text = cmbxRubricLevel.SelectedItem.ToString();

            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bindAssessmentTitle();
        }

        private void cmbxRubricLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ////Obtained Marks
            ObtainedMarks();
        }
    }
}
