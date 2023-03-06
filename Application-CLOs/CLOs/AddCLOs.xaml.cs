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
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace Application_CLOs
{
    /// <summary>
    /// Interaction logic for AddCLOs.xaml
    /// </summary>
    public partial class AddCLOs : Window
    {
        int id_=0;
        public AddCLOs()
        {
            InitializeComponent();
            timer();
            bindDataGrid();
        }
        private void bindDataGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT *\r\nFROM Clo", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgCLOs.ItemsSource = dt.DefaultView;
        }
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            id_ = int.Parse(dataRowView[0].ToString());
            string name= dataRowView[1].ToString();
            txtbxName.Text = name;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCLOs.SelectedIndex >= 0 && dgCLOs.SelectedValue != null)
                {
                    DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                    int id = int.Parse(dataRowView[0].ToString());
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Clo WHERE ID='" + id+ "'", con);
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
            if (ValidateName(txtbxName.Text)== true)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd1 = new SqlCommand("INSERT into CLO values  (@Name,GETDATE(),GETDATE())", con);
                cmd1.Parameters.AddWithValue("@Name", txtbxName.Text);
                cmd1.ExecuteNonQuery();
                lblSignalName.Content = "Data Successfully Saved";
                txtbxName.Clear();
                bindDataGrid();
            }
           
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (id_ >= 0)
            {
                if(ValidateName(txtbxName.Text) == true)
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd1 = new SqlCommand("Update CLO SET Name=@Name,DateUpdated=GETDATE() WHERE ID='" + id_ + "'", con);
                    cmd1.Parameters.AddWithValue("@Name", txtbxName.Text);
                    cmd1.ExecuteNonQuery();
                    lblSignalName.Content = "Data Successfully Updated";
                    txtbxName.Clear();
                    bindDataGrid();
                }
            }
            else
            {
                lblSignalName.Content = "You had not select item to update";
            }
          
        }
        public bool ValidateName(string name)
        {
            string pattern = "^[a-zA-Z0-9]{2,49}$";
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
            lblSignalName.Content = "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtbxName.Clear();
            id_ = -1;
        }

        private void txtbxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateName(txtbxName.Text) == false)
            {
                lblSignalName.Content = "Enter CLO Name";
            }
            else
            {
                lblSignalName.Content = "";
            }
        }

        private void txtbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            bindDataGrid();
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
    }
}
