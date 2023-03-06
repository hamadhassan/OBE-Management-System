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
    /// Interaction logic for AddRubricLevel.xaml
    /// </summary>
    public partial class AddRubricLevel : Window
    {
        int id_ = 0;
        string detailsForUpdate;
        int marksForUpdate;
        public AddRubricLevel()
        {
            InitializeComponent();
            timer();
            bindDataGrid();
            bindRrubricName();
            cmbxRubric.SelectedIndex = 0;
        }
        private void bindDataGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT RL.Id,RL.Details As [Rubric Detail],RL.MeasurementLevel AS [Marks],R.Details AS [Rubric Name],C.Name AS[CLO Name]\r\nFROM RubricLevel RL\r\nJOIN Rubric R\r\nON RL.RubricId=R.Id\r\nJOIN Clo C\r\nON C.Id=R.CloId", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgRubric.ItemsSource = dt.DefaultView;

        }
        private void bindRrubricName()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT R.Details FROM Rubric R", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Add data to combo box
                cmbxRubric.Items.Add(reader["Details"].ToString());
            }
            reader.Close();
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            bindDataGrid();
            bindRrubricName();
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            id_ = int.Parse(dataRowView[0].ToString());
            detailsForUpdate = dataRowView[1].ToString();
            marksForUpdate = int.Parse(dataRowView[2].ToString());
            txtbxDetail.Text = detailsForUpdate;
            txtbxMarks.Text = marksForUpdate.ToString();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgRubric.SelectedIndex >= 0 && dgRubric.SelectedValue != null)
                {
                    DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                    int id = int.Parse(dataRowView[0].ToString());
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("DELETE FROM RubricLevel WHERE ID='" + id + "'", con);
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

            if (ValidateDetail(txtbxDetail.Text) == true && ValidateMarks(txtbxMarks.Text)==true)
            {
                /// This command retrive the Rubric ID corresponding to the detail of rubric
                var con = Configuration.getInstance().getConnection();
                string query = "SELECT ID FROM Rubric WHERE Details='" + cmbxRubric.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                int rubricID = int.Parse(cmd.ExecuteScalar().ToString());
                //// Now the input data will be stored into the database
                SqlCommand cmd3 = new SqlCommand("INSERT into RubricLevel values  (" + rubricID + ",@Detail, " + int.Parse(txtbxMarks.Text) + ")", con);
                cmd3.Parameters.AddWithValue("@Detail", txtbxDetail.Text);
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
                if (ValidateDetail(txtbxDetail.Text) == true && ValidateMarks(txtbxMarks.Text)==true) 
                { 
                    if(txtbxDetail.Text!=detailsForUpdate || txtbxMarks.Text != marksForUpdate.ToString())
                    {
                        //// This command retrive the Rubric ID corresponding to the detail of rubric
                        var con = Configuration.getInstance().getConnection();
                        string query = "SELECT ID FROM Rubric WHERE Details='" + cmbxRubric.Text + "'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        int rubricID = int.Parse(cmd.ExecuteScalar().ToString());
                        SqlCommand cmd1 = new SqlCommand("Update RubricLevel SET Details=@Details, MeasurementLevel=@Level ,RubricID=" + rubricID + " WHERE ID=" + id_ + "", con);
                        cmd1.Parameters.AddWithValue("@Details", txtbxDetail.Text);
                        cmd1.Parameters.AddWithValue("@Level", int.Parse(txtbxMarks.Text));
                        cmd1.ExecuteNonQuery();
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
            txtbxDetail.Clear();
            txtbxMarks.Clear();
            cmbxRubric.SelectedIndex = 0;
            id_ = -1;
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearAllFields();
        }
        public bool ValidateDetail(string name)
        {
            string pattern = "^[a-zA-Z0-9 ]{2,10000}$";
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
            if (ValidateDetail(txtbxDetail.Text) == false)
            {
                lblSignalDetail.Content = "Enter Details";
            }
            else
            {
                lblSignalDetail.Content = "";
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
        private void btnEditCLO_Click(object sender, RoutedEventArgs e)
        {
            AddCLOs addCLOs = new AddCLOs();
            addCLOs.ShowDialog();
        }

        private void btnEditRubric1_Click(object sender, RoutedEventArgs e)
        {
            AddRubric addRubric = new AddRubric();
            addRubric.ShowDialog();
        }

        private void cmbxRubric_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
