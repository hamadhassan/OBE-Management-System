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
using System.Collections;

namespace Application_CLOs
{
    /// <summary>
    /// Interaction logic for ViewStudent.xaml
    /// </summary>
    public partial class ViewStudent : Window
    {
        public ViewStudent()
        {
            InitializeComponent();
            bindDataGrid();
        }
       
        private void bindDataGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM student S", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgStudent.ItemsSource = dt.DefaultView;
            //if (dgStudent.Columns.Count > 1)
            //{
            //    dgStudent.Columns[2].Visibility = Visibility.Collapsed;
            //}
        }
        public void UpdateDataGrid()
        {
            DataView dataView = (DataView)dgStudent.ItemsSource;
            DataTable dataTable = dataView.Table;
            bindDataGrid();
            dgStudent.ItemsSource = null;
            dgStudent.ItemsSource = dataTable.DefaultView;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            int id = int.Parse(dataRowView[0].ToString());           
            MessageBox.Show("You Clicked : " + id.ToString() + "\r\nDescription : ");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgStudent.SelectedIndex >= 0 && dgStudent.SelectedValue != null)
                {
                    DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                    int id = int.Parse(dataRowView[0].ToString());
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("delete from student where id='" + id.ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                }
                bindDataGrid();
            }
            catch
            {
                MessageBox.Show("You are trying to access the wrong field","Information",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            





        }
     }
}
