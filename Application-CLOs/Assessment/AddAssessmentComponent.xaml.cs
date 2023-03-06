using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Application_CLOs
{
    /// <summary>
    /// Interaction logic for AddAssessmentComponent.xaml
    /// </summary>
    public partial class AddAssessmentComponent : Window
    {
        int id_ = 0;
        string NameForUpdate;
        int marksForUpdate;
        public AddAssessmentComponent()
        {
            InitializeComponent();
            timer();
            cmbxRubric.SelectedIndex = 0;
            cmbxAssessment.SelectedIndex = 0;
            bindDataGrid();
            bindRubricName();
            bindAssessmentTitle();
        }
        private void bindDataGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT AC.ID ,AC.Name AS [Question Detail],AC.TotalMarks AS [Question Marks],A.Title AS [Assessment Title],\r\nA.TotalMarks AS [Assessment Total Marks], A.TotalWeightage AS [Assessment Weightage], R.Details AS [Rubric Name],C.Name AS[Mapped CLO]\r\nFROM AssessmentComponent AC\r\nJOIN Assessment A\r\nON AC.AssessmentId=A.Id\r\nJOIN Rubric R\r\nON AC.RubricId=R.Id\r\nJOIN CLO C\r\nON R.CloId=C.Id", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgAssessment.ItemsSource = dt.DefaultView;

        }
        private void bindRubricName()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("SELECT R.Details FROM Rubric R", con);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                // Add data to combo box
                cmbxRubric.Items.Add(reader2["Details"].ToString());
            }

            reader2.Close();
        }
        private void bindAssessmentTitle()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT A.Title FROM Assessment A", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Add data to combo box
                cmbxAssessment.Items.Add(reader["Title"].ToString());
            }
            reader.Close();
        }
        private void Window_Activated_1(object sender, EventArgs e)
        {
            bindDataGrid();
            bindRubricName();
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgAssessment.SelectedIndex >= 0 && dgAssessment.SelectedValue != null)
                {
                    DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                    id_ = int.Parse(dataRowView[0].ToString());
                    NameForUpdate = dataRowView[1].ToString();
                    marksForUpdate = int.Parse(dataRowView[2].ToString());
                    txtbxName.Text = NameForUpdate;
                    txtbxMarks.Text = marksForUpdate.ToString();
                }
                bindDataGrid();
            }
            catch
            {
                MessageBox.Show("You are trying to access the wrong field", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
           
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgAssessment.SelectedIndex >= 0 && dgAssessment.SelectedValue != null)
                {
                    DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                    int id = int.Parse(dataRowView[0].ToString());
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("DELETE FROM AssessmentComponent WHERE ID='" + id + "'", con);
                    cmd.ExecuteNonQuery();
                }
                bindDataGrid();
            }
            catch
            {
                MessageBox.Show("You are trying to access the wrong field", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (ValidateName(txtbxName.Text) == true && ValidateMarks(txtbxMarks.Text) == true)
            {
                /// This command retrive the Rubric ID corresponding to the detail of rubric
                var con = Configuration.getInstance().getConnection();
                string query = "SELECT ID FROM Rubric WHERE Details='" + cmbxRubric.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                int rubricID = int.Parse(cmd.ExecuteScalar().ToString());
                /// This command retrive the Assessmnet ID corresponding to the title of Assessment
                string query1 = "SELECT ID FROM Assessment WHERE Title='" + cmbxAssessment.Text + "'";
                SqlCommand cmd1 = new SqlCommand(query1, con);
                int assessmnetID = int.Parse(cmd1.ExecuteScalar().ToString());
                /// Now the input data will be stored into the database
                SqlCommand cmd3 = new SqlCommand("INSERT into AssessmentComponent values (@Name,@RubricID,@TotalMarks,GETDATE(),GETDATE(),@AssessmentID)", con);
                cmd3.Parameters.AddWithValue("@Name", txtbxName.Text);
                cmd3.Parameters.AddWithValue("@RubricID", rubricID);
                cmd3.Parameters.AddWithValue("@TotalMarks", int.Parse(txtbxMarks.Text));
                cmd3.Parameters.AddWithValue("@AssessmentID", assessmnetID);
                cmd3.ExecuteNonQuery();
                lblMessage.Content = "Data Successfully Saved";
                bindDataGrid();
                ClearAllFields();

            }
            else
            {
                lblMessage.Content = "You data is not saved";
            }


        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (id_ >= 0)
            {
                if (ValidateName(txtbxName.Text) == true && ValidateMarks(txtbxMarks.Text) == true)
                {
                    if (txtbxName.Text != NameForUpdate || txtbxMarks.Text != marksForUpdate.ToString())
                    { /// This command retrive the Rubric ID corresponding to the detail of rubric
                        var con = Configuration.getInstance().getConnection();
                        string query = "SELECT ID FROM Rubric WHERE Details='" + cmbxRubric.Text + "'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        int rubricID = int.Parse(cmd.ExecuteScalar().ToString());
                        /// This command retrive the Assessmnet ID corresponding to the title of Assessment
                        string query1 = "SELECT ID FROM Assessment WHERE Title='" + cmbxAssessment.Text + "'";
                        SqlCommand cmd1 = new SqlCommand(query1, con);
                        int assessmnetID = int.Parse(cmd1.ExecuteScalar().ToString());
                        // Now the input data will be updated into the database
                        SqlCommand cmd3 = new SqlCommand("Update AssessmentComponent SET Name=@Name, RubricID=@RubricID,TotalMarks=@TotalMarks ,DateUpdated=GETDATE(),AssessmentID=@AssessmentID WHERE ID=" + id_ + "", con);
                        cmd3.Parameters.AddWithValue("@Name", txtbxName.Text);
                        cmd3.Parameters.AddWithValue("@RubricID", rubricID);
                        cmd3.Parameters.AddWithValue("@TotalMarks", int.Parse(txtbxMarks.Text));
                        cmd3.Parameters.AddWithValue("@AssessmentID", assessmnetID);
                        cmd3.ExecuteNonQuery();
                        lblMessage.Content = "Data Successfully Updated";
                        bindDataGrid();
                        ClearAllFields();
                    }
                    else
                    {
                        lblMessage.Content = "You had not made any change";
                    }

                }
                else
                {
                    lblMessage.Content = "Fill all required fields";
                }
            }
            else
            {
                lblMessage.Content = "You had not select item to update";
            }

        }

        private void timer()
        {
            DispatcherTimer dtClockTime = new DispatcherTimer();

            dtClockTime.Interval = new TimeSpan(0, 0, 3); //in Hour, Minutes, Second.
            dtClockTime.Tick += dtClockTime_Tick;
            dtClockTime.Start();
        }
        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            lblMessage.Content = "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClearAllFields()
        {
            txtbxName.Clear();
            txtbxMarks.Clear();
            cmbxRubric.SelectedIndex = 0;
            cmbxAssessment.SelectedIndex = 0;
            id_ = -1;
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearAllFields();
        }
        public bool ValidateName(string name)
        {
            string pattern = "^[a-zA-Z0-9 ]{2,50}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(name);
        }
        public bool ValidateMarks(string marks)
        {
            string pattern = "^[0-9]{1,4}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(marks);
        }

        private void txtbxDetail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateName(txtbxName.Text) == false)
            {
                lblSignalName.Content = "Enter Name";
            }
            else
            {
                lblSignalName.Content = "";
            }
        }
        private void txtbxMarks_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateMarks(txtbxMarks.Text) == false)
            {
                lblSignalMarks.Content = "Enter Marks";
            }
            else
            {
                lblSignalMarks.Content = "";
            }
        }

        private void txtbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtbxSearch_GotFocus(object sender, RoutedEventArgs e)
        {

            if (txtbxSearch.Text == "Search Here")
            {
                txtbxSearch.Text = "";
                txtbxSearch.Foreground = Brushes.Black;
            }
        }
        private void txtbxSearch_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (txtbxSearch.Text == "")
            {
                txtbxSearch.Text = "Search Here";
                txtbxSearch.Foreground = Brushes.Silver;
            }
        }

        private void btnEditRubric1_Click(object sender, RoutedEventArgs e)
        {
            AddRubric addRubric = new AddRubric();
            addRubric.ShowDialog();
        }

        private void btnEditAssessment_Click(object sender, RoutedEventArgs e)
        {
            AddAssessment addAssessment = new AddAssessment();
            addAssessment.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bindAssessmentTitle();
        }
    }
}
