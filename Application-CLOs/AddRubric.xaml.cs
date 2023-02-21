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
    /// Interaction logic for AddRubric.xaml
    /// </summary>
    public partial class AddRubric : Window
    {
        int id_ = 0;
        public AddRubric()
        {
            InitializeComponent();
            timer();
            cmbxCLO.SelectedIndex= 0;
        }
        private void bindDataGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT R.ID,R.Details AS [Rubric Detail],C.Name AS [CLO Name]\r\nFROM Rubric R \r\nJOIN  Clo C\r\nON R.CloId=C.Id", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgRubric.ItemsSource = dt.DefaultView;

        }
        private void bindCLOName()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT C.Name\r\nFROM CLO C", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Add data to combo box
                cmbxCLO.Items.Add(reader["Name"].ToString());
            }
        }
        private void Window_Activated_1(object sender, EventArgs e)
        {
            bindDataGrid();
            bindCLOName();
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            id_ = int.Parse(dataRowView[0].ToString());
            string name = dataRowView[1].ToString();
            txtbxDetail.Text = name;
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
                    SqlCommand cmd = new SqlCommand("DELETE FROM RUBRIC WHERE ID='" + id + "'", con);
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
           
            if (ValidateName(txtbxDetail.Text) == true)
            {
                /// This command retrive the CLO ID corresponding to the name of the clo
                var con = Configuration.getInstance().getConnection();
                string query = "SELECT ID FROM CLO WHERE Name='" + cmbxCLO.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                int cloID = int.Parse(cmd.ExecuteScalar().ToString());
                /// This command retrive the rubric id
                /// IF THE Rrubric id is exist display message the data already exist
                /// ELSE store thr data into the database with the increase in the id count
                string query1 = "SELECT R.Id FROM Rubric R WHERE R.Details='"+cmbxCLO.Text+"' AND R.CloId="+ cloID+ " \r\n";
                SqlCommand cmd1 = new SqlCommand(query1, con);
                object result = cmd1.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    string query2 = "SELECT TOP 1 R.ID FROM Rubric R ORDER BY R.Id DESC \r\n";
                    SqlCommand cmd2 = new SqlCommand(query2, con);
                    int rubricID = int.Parse(cmd2.ExecuteScalar().ToString());
                    rubricID += 1;
                    //// Now the input data will be stored into the database
                    SqlCommand cmd3 = new SqlCommand("INSERT into RUBRIC values  (" + rubricID + ",@Detail, " + cloID + ")", con);
                    cmd3.Parameters.AddWithValue("@Detail", txtbxDetail.Text);
                    cmd3.ExecuteNonQuery();
                    lblMessage.Content = "Data Successfully Saved";
                    bindDataGrid();
                    ClearAllFields();
                }
                else
                {
                    lblMessage.Content = "The rubric is already exist";
                }
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
                if (ValidateName(txtbxDetail.Text) == true)
                {
                    /// This command retrive the CLO ID corresponding to the name of the clo
                    var con = Configuration.getInstance().getConnection();
                    string query = "SELECT ID FROM CLO WHERE Name='" + cmbxCLO.Text + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    int cloID = int.Parse(cmd.ExecuteScalar().ToString());
                    SqlCommand cmd1 = new SqlCommand("Update RUBRIC SET Details=@Details,CLOID="+ cloID + " WHERE ID='" + id_ + "'", con);
                    cmd1.Parameters.AddWithValue("@Details", txtbxDetail.Text);
                    cmd1.ExecuteNonQuery();
                    lblSignalDetail.Content = "Data Successfully Updated";
                    txtbxDetail.Clear();
                    bindDataGrid();
                    ClearAllFields();
                }
            }
            else
            {
                lblSignalDetail.Content = "You had not select item to update";
            }

        }
        public bool ValidateName(string name)
        {
            string pattern = "^[a-zA-Z0-9 ]{2,49}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(name);
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
            lblSignalDetail.Content = "";
            lblMessage.Content = "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClearAllFields()
        {
            txtbxDetail.Clear();
            cmbxCLO.SelectedIndex = 0;
            id_ = -1;
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearAllFields();
        }

        private void txtbxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateName(txtbxDetail.Text) == false)
            {
                lblSignalDetail.Content = "Enter CLO Name";
            }
            else
            {
                lblSignalDetail.Content = "";
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

        private void txtbxSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxSearch.Text == "")
            {
                txtbxSearch.Text = "Search Here";
                txtbxSearch.Foreground = Brushes.Silver;
            }
        }

        private void btnEditCLO_Click(object sender, RoutedEventArgs e)
        {
            AddCLOs addCLOs=new AddCLOs();
            addCLOs.ShowDialog();
        }
    }
}
