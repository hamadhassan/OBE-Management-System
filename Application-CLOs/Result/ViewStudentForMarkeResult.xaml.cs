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
    /// Interaction logic for ViewStudentForMarkeResult.xaml
    /// </summary>
    public partial class ViewStudentForMarkeResult : Window
    {
        public ViewStudentForMarkeResult()
        {
            InitializeComponent();
        }
        private void bindDataGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("SELECT S.id,S.FirstName,S.lastname,S.Contact,S.email,S.RegistrationNumber,L.Name AS Status FROM Student S JOIN Lookup  L ON L.LookupId=S.Status WHERE  L.Category='STUDENT_STATUS'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgStudent.ItemsSource = dt.DefaultView;

        }
      
        private void Window_Activated(object sender, EventArgs e)
        {
            bindDataGrid();
        }

        private void btnAssessment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgStudent.SelectedIndex >= 0 && dgStudent.SelectedValue != null)
                {
                    DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                    int studentId = int.Parse(dataRowView[0].ToString());
                    AddStudentResult addStudentResult = new AddStudentResult(studentId);
                    addStudentResult.ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show("You are trying to access the wrong field", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
          
        }

      
    }
}
