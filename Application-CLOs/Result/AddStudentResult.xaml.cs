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
        bool isForUpdateResult=false; //If the form open for update 
        int rubricId_;
        string assessmentTitle;
        string questionName;
        public AddStudentResult()
        {
            InitializeComponent();
        }
        public AddStudentResult(int studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
        }
        public AddStudentResult(int studentId,string assessmentTitle, string questionName, bool isForUpdateResult)
        {
            InitializeComponent();
            this.studentId = studentId;
            this.isForUpdateResult = isForUpdateResult;
            this.assessmentTitle = assessmentTitle;
            this.questionName = questionName;
            settingForUpdate();
        }
        private void settingForUpdate()
        {
            lblTitle.Content = "Update Student Result";
            btnSave.Content = "Update";
            cmbxAssessmentName.Items.Clear();
            cmbxAssessmentName.Items.Add(assessmentTitle);
            cmbxQuestion.Items.Clear();
            cmbxQuestion.Items.Add(questionName);
            cmbxAssessmentName.SelectedIndex= 0;
            cmbxQuestion.SelectedIndex= 0;
            cmbxAssessmentName.IsEnabled= false;
            cmbxQuestion.IsEnabled= false;

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
            try
            {
                //////Bind the Assessment Question from the selected assessment name
                cmbxQuestion.Items.Clear();
                AssessmentComponentDL.LoadDataIntoList();
                foreach (string a in AssessmentComponentDL.GetAssessmentComponentNameFromAssessmentId(assessmentid))
                {
                    cmbxQuestion.Items.Add(a);
                }
                cmbxQuestion.SelectedIndex = 0;
                if (cmbxQuestion.Items.Count == 0)
                {
                    lblMessage.Content = "Data is not available of " + cmbxAssessmentName.SelectedItem.ToString();
                    cmbxRubricLevel.Items.Clear();
                }
                else
                {
                    cmbxQuestion.SelectedIndex = 0;
                    lblMessage.Content = "";
                }
                
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
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (isForUpdateResult==false)
            {//INSERT DATA
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("INSERT into StudentResult values  (@StudentID, @AssessmentComponentID,@RubricMearumentID,GETDATE())", con);
                cmd1.Parameters.AddWithValue("@StudentID", studentId);
                cmd1.Parameters.AddWithValue("@AssessmentComponentID", AssessmentComponentDL.GetAssesmentComponetId(cmbxQuestion.SelectedItem.ToString(), rubricId_, int.Parse(txtbxTotalMarks.Text)));
                cmd1.Parameters.AddWithValue("@RubricMearumentID", RubricLevelDL.GetRubricLevelId(rubricId_, int.Parse(cmbxRubricLevel.SelectedItem.ToString())));
                cmd1.ExecuteNonQuery();
                lblMessage.Content = "Data Successfully Saved";
            }
            else if (isForUpdateResult)
            {//UPDATE DATA
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("UPDATE StudentResult SET RubricMeasurementId=@RubricMearumentID,EvaluationDatE=GETDATE() WHERE StudentID=@StudentID AND  AssessmentComponentId=@AssessmentComponentID", con);
                cmd1.Parameters.AddWithValue("@StudentID", studentId);
                cmd1.Parameters.AddWithValue("@AssessmentComponentID", AssessmentComponentDL.GetAssesmentComponetId(cmbxQuestion.SelectedItem.ToString(), rubricId_, int.Parse(txtbxTotalMarks.Text)));
                cmd1.Parameters.AddWithValue("@RubricMearumentID", RubricLevelDL.GetRubricLevelId(rubricId_, int.Parse(cmbxRubricLevel.SelectedItem.ToString())));
                cmd1.ExecuteNonQuery();
                lblMessage.Content = "Data Successfully Updated";
            }
           
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
            if (isForUpdateResult == false)
            {
                int assessmentId = AssessmentDL.GetAssessemntIdFromTitle(cmbxAssessmentName.SelectedItem.ToString());
                bindAssessmentQuestions(assessmentId);
            }
        }
        private void cmbxQuestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbxQuestion.Items.Count > 0 && cmbxAssessmentName.Items.Count > 0)
            {
                int rubricId = AssessmentComponentDL.GetRubricId(cmbxAssessmentName.SelectedItem.ToString(), cmbxQuestion.SelectedItem.ToString());
                bindMeasuremntLevel(rubricId);
                ////Mapped CLO 
                MappedCLOFromRubricID(rubricId);
                ////Maximum Rubric Level
                TotalMarks(rubricId);
                rubricId_ = rubricId;
            }
        }
        private void MappedCLOFromRubricID(int rubricId)
        {
            string query = "SELECT C.Name\r\nFROM Clo C\r\nJOIN Rubric R\r\nON C.Id=R.CloId\r\nWHERE R.Id=" + rubricId;
            txtbxMappedCLO.Text = queryData(query);
        }
        private void TotalMarks(int rubricId)
        {
            string query = "SELECT AC.TotalMarks FROM AssessmentComponent AC WHERE AC.RubricId= " + rubricId+ "AND AC.Name='"+cmbxQuestion.SelectedItem.ToString()+"'";
            txtbxTotalMarks.Text = queryData(query);
        }
        private void ObtainedMarks()
        {
            if (cmbxQuestion.Items.Count > 0 && cmbxAssessmentName.Items.Count > 0)
            {
                   
                string query = "DECLARE @StudentId AS int =" + studentId +
                    " DECLARE @TotalMarks as float =(SELECT AC.TotalMarks FROM StudentResult SR JOIN AssessmentComponent AC ON SR.AssessmentComponentId=AC.Id WHERE SR.StudentId=@StudentId)\r\n" +
                    "DECLARE @RubricId as float=(SELECT AC.RubricId FROM StudentResult SR JOIN AssessmentComponent AC ON SR.AssessmentComponentId=AC.Id WHERE SR.StudentId=@StudentId)\r\n" +
                    "DECLARE @MaxLevel as float=( SELECT MAx(RL.MeasurementLevel) FROM RubricLevel RL WHERE RL.RubricId=@RubricId) " +
                    "SELECT (" + int.Parse(cmbxRubricLevel.SelectedItem.ToString()) + "/@MaxLevel) *@TotalMarks\r\nFROM StudentResult SR\r\nJOIN Student S\r\nON S.Id=SR.StudentId\r\nWHERE SR.StudentId=@StudentId";
                txtbxObtainedMarks.Text = queryData(query);
            }
           
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bindAssessmentTitle();
        
        }
        private void cmbxRubricLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isForUpdateResult)
            {
                lblObtainedMarks.Visibility = Visibility.Visible;
                txtbxObtainedMarks.Visibility = Visibility.Visible;
                ////Obtained Marks
                //ObtainedMarks();
            }
            else
            {
                lblObtainedMarks.Visibility = Visibility.Hidden;
                txtbxObtainedMarks.Visibility = Visibility.Hidden;
            }
        }
    }
}
