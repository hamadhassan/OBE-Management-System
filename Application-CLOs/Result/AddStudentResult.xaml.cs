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
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            string output = cmd.ExecuteScalar().ToString();
            return output;
        }
        private void bindAssessmentName()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT A.Title\r\nFROM Assessment A", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Add data to combo box
                cmbxAssessmentName.Items.Add(reader["Title"].ToString());
            }
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            string query = "SELECT CONCAT(S.FirstName,' ',S.LastName)\r\nFROM Student S\r\nWHERE ID=" + studentId;
            txtbxStudentName.Text=queryData(query);
            query = "SELECT S.RegistrationNumber \r\nFROM Student S\r\nWHERE ID=" + studentId;
            txtbxRegistrationNumber.Text = queryData(query);
            bindAssessmentName();
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

       
    }
}
