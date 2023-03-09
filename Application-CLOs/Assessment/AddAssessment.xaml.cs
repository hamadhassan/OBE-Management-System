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
    /// Interaction logic for AddAssessment.xaml
    /// </summary>
    public partial class AddAssessment : Window
    {
        int id_ = 0;
        string titleForUpdate;
        int marksForUpdate;
        int weightageForUpdate;
        public AddAssessment()
        {
            InitializeComponent();
            timer();
            bindDataGrid();
        }
        private void bindDataGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT A.Id,A.Title,A.DateCreated,A.TotalMarks,A.TotalWeightage FROM Assessment A\r\n", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgAssessment.ItemsSource = dt.DefaultView;
            //dgAssessment.Columns[2].Visibility = Visibility.Hidden;
            

        }
        private void Window_Activated_1(object sender, EventArgs e)
        {
            bindDataGrid();
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            id_ = int.Parse(dataRowView[0].ToString());
            titleForUpdate = dataRowView[1].ToString();
            marksForUpdate = int.Parse(dataRowView[3].ToString());
            weightageForUpdate = int.Parse(dataRowView[4].ToString());
            txtbxTitle.Text = titleForUpdate;
            txtbxMarks.Text = marksForUpdate.ToString();
            txtbxWeightage.Text = weightageForUpdate.ToString();

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
                    SqlCommand cmd = new SqlCommand("DELETE FROM Assessment WHERE ID='" + id + "'", con);
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

            if (ValidateTitle(txtbxTitle.Text) == true && ValidateMarks(txtbxMarks.Text) == true && ValidateMarks(txtbxWeightage.Text) == true)
            {
                var con = Configuration.getInstance().getConnection();
                //// Now the input data will be stored into the database
                SqlCommand cmd = new SqlCommand("INSERT into Assessment values (@Title,GETDATE(),@TotalMarks,@TotalWeightage)", con);
                cmd.Parameters.AddWithValue("@Title", txtbxTitle.Text);
                cmd.Parameters.AddWithValue("@TotalMarks",int.Parse(txtbxMarks.Text));
                cmd.Parameters.AddWithValue("@TotalWeightage", int.Parse(txtbxWeightage.Text));
                cmd.ExecuteNonQuery();
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
                if (ValidateTitle(txtbxTitle.Text) == true && ValidateMarks(txtbxMarks.Text) == true && ValidateMarks(txtbxWeightage.Text)==true)
                {
                    if (txtbxTitle.Text != titleForUpdate || txtbxMarks.Text != marksForUpdate.ToString() || txtbxWeightage.Text!=weightageForUpdate.ToString())
                    {
                        var con = Configuration.getInstance().getConnection();
                        SqlCommand cmd = new SqlCommand("Update Assessment SET Title=@Title, TotalMarks=@TotalMarks ,TotalWeightage=@TotalWeightage WHERE ID=" + id_ + "", con);
                        cmd.Parameters.AddWithValue("@Title", txtbxTitle.Text);
                        cmd.Parameters.AddWithValue("@TotalMarks", int.Parse(txtbxMarks.Text));
                        cmd.Parameters.AddWithValue("@TotalWeightage", int.Parse(txtbxWeightage.Text));
                        cmd.ExecuteNonQuery();
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
            txtbxTitle.Clear();
            txtbxMarks.Clear();
            txtbxWeightage.Clear();
            id_ = -1;
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearAllFields();
        }
        public bool ValidateTitle(string name)
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
            if (ValidateTitle(txtbxTitle.Text) == false)
            {
                lblSignalTitle.Content = "Enter Title";
            }
            else
            {
                lblSignalTitle.Content = "";
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
        private void txtbxWeightage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateMarks(txtbxMarks.Text) == false)
            {
                lblSignalWeightage.Content = "Enter Weightage";
            }
            else
            {
                lblSignalWeightage.Content = "";
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

      
    }
}
