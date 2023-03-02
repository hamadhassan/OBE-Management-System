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
    /// Interaction logic for ViewStudentResult.xaml
    /// </summary>
    public partial class ViewStudentResult : Window
    {
        int assessmentComponentId;
        int studentId;
        public ViewStudentResult()
        {
            InitializeComponent();
        }
        private void bindStuentResult()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("DECLARE @MaxLevel as float=( SELECT MAx(RL.MeasurementLevel) FROM RubricLevel RL WHERE RL.RubricId IN (SELECT AC.RubricId FROM StudentResult SR JOIN AssessmentComponent AC ON SR.AssessmentComponentId=AC.Id))\r\nSELECT  CONCAT(S.FirstName,' ',S.LastName) AS [Full Name],@MaxLevel AS [Maximum Level] ,RL.MeasurementLevel,AC.TotalMarks, (RL.MeasurementLevel/@MaxLevel)*AC.TotalMarks AS[Obtained Marks],\r\nAC.Name AS [Question Name],A.Title AS [Assessmmet Name],SR.AssessmentComponentId,SR.StudentId\r\nFROM StudentResult SR\r\nJOIN Student S\r\nON S.Id=SR.StudentId\r\nJOIN RubricLevel RL\r\nON RL.Id=SR.RubricMeasurementId\r\nJOIN Rubric R\r\nON R.Id=RL.RubricId\r\nJOIN AssessmentComponent AC\r\nON AC.ID=SR.AssessmentComponentId\r\nJOIN Assessment A\r\nON A.Id=AC.AssessmentId", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgStudent.ItemsSource = dt.DefaultView;
            if (dgStudent.Columns.Count > 1)
            {
                dgStudent.Columns[9].Visibility = Visibility.Hidden;
                dgStudent.Columns[10].Visibility = Visibility.Hidden;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                if (dgStudent.SelectedIndex >= 0 && dgStudent.SelectedValue != null)
                {
                    DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                    assessmentComponentId = int.Parse(dataRowView[7].ToString());
                    studentId = int.Parse(dataRowView[8].ToString());
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("DELETE FROM StudentResult WHERE StudentId= " + studentId + " AND AssessmentComponentId=" + assessmentComponentId, con);
                    cmd.ExecuteNonQuery();
                    bindStuentResult();
                }
            }
            catch
            {
                MessageBox.Show("You are trying to access the wrong field", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            string questionName = dataRowView[5].ToString();
            string assessmentTitle = dataRowView[6].ToString();
            studentId = int.Parse(dataRowView[8].ToString());
            AddStudentResult addStudentResult=new AddStudentResult(studentId,assessmentTitle,questionName,true);
            addStudentResult.ShowDialog();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            bindStuentResult();

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
