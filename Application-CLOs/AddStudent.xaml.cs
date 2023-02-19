using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for AddStudent.xaml
    /// </summary>
    public partial class AddStudent : Window
    {
        private int id=0;//The Student Id number
        string firstName;
        string lastName;
        string contact;
        string email;
        string RegistrationNumber;
        string status;

        // This constructor is used for standard ADD STUDENT
        public AddStudent()
        {
            InitializeComponent();
            loadStatusOfStudent();
        }
        // This construcotor is used for EDIT THE STUDENT
        public AddStudent(int id,string firstName,string lastName,string contact,string email,string registrationNumber,string status)
        {
            InitializeComponent();
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.contact = contact;
            this.email = email;
            this.RegistrationNumber = registrationNumber;
            this.status = status;
            loadStudentForEdit();
        }
        private void loadStudentForEdit()
        {
            txtbxFirstName.Text = firstName;
            txtbxLastName.Text = lastName;
            txtbxContact.Text = contact;
            txtbxEmail.Text = email;
            txtbxRegisrationNumber.Text = RegistrationNumber;
            cmbxStatus.Items.Add(status);
        }
        private void loadStatusOfStudent()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Name FROM Lookup WHERE Category='STUDENT_STATUS'", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Add data to combo box
                cmbxStatus.Items.Add(reader["Name"].ToString());
            }

        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
           this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            /*
             IF ID=0 => User come for insert
             IF ID!=0 => User come for update
             */
            if (id == 0)
            {
                string query = "SELECT LookupId FROM Lookup WHERE Name='" + cmbxStatus.Text + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                int LookupID = int.Parse(cmd.ExecuteScalar().ToString());
                SqlCommand cmd1 = new SqlCommand("INSERT into student values  (@FirstName, @LastName,@Contact,@Email,@RegistrationNumber,@status)", con);
                cmd1.Parameters.AddWithValue("@FirstName", txtbxFirstName.Text);
                cmd1.Parameters.AddWithValue("@LastName", txtbxLastName.Text);
                cmd1.Parameters.AddWithValue("@Contact", txtbxContact.Text);
                cmd1.Parameters.AddWithValue("@Email", txtbxEmail.Text);
                cmd1.Parameters.AddWithValue("@RegistrationNumber", txtbxRegisrationNumber.Text);
                /*
                 Now Retrive the lookup id number from rhe combox box name
                Then store the corresponding id into the database 
                 */
                cmd1.Parameters.AddWithValue("@status", LookupID);
                cmd1.ExecuteNonQuery();
            }
            else
            {
                string query = "SELECT LookupId FROM Lookup WHERE Name='" + cmbxStatus.Text + "'";
                SqlCommand cmd3 = new SqlCommand(query, con);
                int LookupID = int.Parse(cmd3.ExecuteScalar().ToString());
                string query2 = "UPDATE Student SET FirstName='"+txtbxFirstName.Text+ "',LastName='" + txtbxLastName.Text+ "',Contact='" + txtbxContact.Text + "',Email='" + txtbxEmail.Text + "',RegistrationNumber='" + txtbxRegisrationNumber.Text +"',Status='" +LookupID+ "'WHERE ID='" +id+"'";
                SqlCommand cmd4 = new SqlCommand(query2, con);
                cmd4.ExecuteNonQuery();
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbxStatus.SelectedIndex = 0;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            ViewStudent viewStudent=new ViewStudent();
            viewStudent.Show();
            this.Close();
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            txtbxFirstName.Clear();
            txtbxLastName.Clear();
            txtbxContact.Clear();
            txtbxRegisrationNumber.Clear();
            txtbxEmail.Clear();

        }
    }
}
