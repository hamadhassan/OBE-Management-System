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
    /// Interaction logic for StudentDashboard.xaml
    /// </summary>
    public partial class StudentDashboard : Window
    {
        public StudentDashboard()
        {
            InitializeComponent();
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            Home home=new Home();
            home.Show();
            this.Hide();
        }

        private void btnStudent_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddStudent addStudent=new AddStudent();
            addStudent.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ViewStudent viewStudent=new ViewStudent();
            viewStudent.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ViewStudent viewStudent=new ViewStudent();
            viewStudent.ShowDialog();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            ViewStudent viewStudent = new ViewStudent();
            viewStudent.ShowDialog();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ViewStudent viewStudent = new ViewStudent();
            viewStudent.ShowDialog();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string query = "SELECT COUNT(S.RegistrationNumber) FROM student S";
            SqlCommand cmd = new SqlCommand(query, con);
            string firstName = cmd.ExecuteScalar().ToString();
            lblCountTotalStudent.Content = firstName;
        }

        private void btnSetRules_Click(object sender, RoutedEventArgs e)
        {
            Lookup lookup = new Lookup();
            lookup.ShowDialog();
        }
    }
}
