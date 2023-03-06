using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for AddStudent.xaml
    /// </summary>
    public partial class AddStudent : Window
    {
        private int id = 0;//The Student Id number
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
            cmbxStatus.SelectedIndex = 0;
            timer();

        }
        // This construcotor is used for EDIT THE STUDENT
        public AddStudent(int id, string firstName, string lastName, string contact, string email, string registrationNumber, string status)
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
            loadStatusOfStudent();
            lblAddStudent.Content = "Update Student Record";
            cmbxStatus.SelectedIndex = 0;
            timer();

        }
        private void loadStudentForEdit()
        {
            txtbxFirstName.Text = firstName;
            txtbxLastName.Text = lastName;
            txtbxContact.Text = contact;
            txtbxEmail.Text = email;
            txtbxRegisrationNumber.Text = RegistrationNumber;
            //cmbxStatus.Items.Add(status);
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
            if (ValidateFirstName(txtbxFirstName.Text) && ValidateLastName(txtbxLastName.Text)
            && ValidateContactNumber(txtbxContact.Text) && ValidateRegistrationNumber(txtbxRegisrationNumber.Text) && ValidateEmail(txtbxEmail.Text))
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
                    lblMessage.Content = "Data Successfully Saved";
                    clearAllFields();
                }
                else
                {
                    //// If the previous content is not same update it else dsiplay signal
                    if (matchPreviousContent() == false)
                    {
                        string query = "SELECT LookupId FROM Lookup WHERE Name='" + cmbxStatus.Text + "'";
                        SqlCommand cmd3 = new SqlCommand(query, con);
                        int LookupID = int.Parse(cmd3.ExecuteScalar().ToString());
                        string query2 = "UPDATE Student SET FirstName='" + txtbxFirstName.Text + "',LastName='" + txtbxLastName.Text + "',Contact='" + txtbxContact.Text + "',Email='" + txtbxEmail.Text + "',RegistrationNumber='" + txtbxRegisrationNumber.Text + "',Status='" + LookupID + "'WHERE ID='" + id + "'";
                        SqlCommand cmd4 = new SqlCommand(query2, con);
                        cmd4.ExecuteNonQuery();
                        lblMessage.Content = "Data Successfully Updated";
                        clearAllFields();
                    }
                    else
                    {
                        lblMessage.Content = "There is no chnage in feilds";
                    }
                   
                }
            }
            else
            {
                lblMessage.Content = "Required fields are missing ";
            }


        }
        private bool matchPreviousContent()
        {
            if(txtbxFirstName.Text == firstName && txtbxLastName.Text == lastName&& txtbxEmail.Text==email && 
                txtbxContact.Text == contact && txtbxRegisrationNumber.Text == RegistrationNumber && cmbxStatus.Text==status)
            {
                return true;
            }
            return false;
        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            cmbxStatus.SelectedIndex = 0;
            timer();
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

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            ViewStudent viewStudent = new ViewStudent();
            viewStudent.Show();
            this.Close();
        }
        private void clearAllFields()
        {
            txtbxFirstName.Clear();
            txtbxLastName.Clear();
            txtbxContact.Clear();
            txtbxRegisrationNumber.Clear();
            txtbxEmail.Clear();
            cmbxStatus.SelectedIndex = 0;
        }
        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            clearAllFields();

        }
        public bool ValidateFirstName(string firstName)
        {
            string pattern = "^[a-zA-Z ]{2,20}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(firstName);
        }
        public bool ValidateLastName(string lastName)
        {
            string pattern = "^[a-zA-Z ]{2,20}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(lastName);
        }
    


        public bool ValidateContactNumber(string contactNumber)
        {
            string pattern = @"^\d{11}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(contactNumber);
        }
        public bool ValidateRegistrationNumber(string registrationNumber)
        {
            string pattern = @"^\d{4}-[A-Z]{2,3}-\d{1,3}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(registrationNumber);

        }
        public bool ValidateEmail(string email)
        {
            string pattern = @"[a-z0-9]+@[a-z]+\.[a-z]{2,3}";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
        private void txtbxFirstName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxFirstName.Text == "Enter First Name")
            {
                txtbxFirstName.Text = "";
                txtbxFirstName.Foreground = Brushes.Black;
            }
        }

        private void txtbxFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxFirstName.Text == "")
            {
                txtbxFirstName.Text = "Enter First Name";
                txtbxFirstName.Foreground = Brushes.Silver;
            }
        }

        private void txtbxLastName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxLastName.Text == "Enter Last Name")
            {
                txtbxLastName.Text = "";
                txtbxLastName.Foreground = Brushes.Black;
            }
        }

        private void txtbxLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxLastName.Text == "")
            {
                txtbxLastName.Text = "Enter Last Name";
                txtbxLastName.Foreground = Brushes.Silver;
            }
        }

        private void txtbxRegisrationNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxRegisrationNumber.Text == "Enter Registration Number")
            {
                txtbxRegisrationNumber.Text = "";
                txtbxRegisrationNumber.Foreground = Brushes.Black;
            }
        }

        private void txtbxRegisrationNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxRegisrationNumber.Text == "")
            {
                txtbxRegisrationNumber.Text = "Enter Registration Number";
                txtbxRegisrationNumber.Foreground = Brushes.Silver;
            }

        }

        private void txtbxContact_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxContact.Text == "Enter Contact Number")
            {
                txtbxContact.Text = "";
                txtbxContact.Foreground = Brushes.Black;
            }
        }

        private void txtbxContact_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxContact.Text == "")
            {
                txtbxContact.Text = "Enter Contact Number";
                txtbxContact.Foreground = Brushes.Silver;
            }
        }

        private void txtbxEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxEmail.Text == "Enter Email Address")
            {
                txtbxEmail.Text = "";
                txtbxEmail.Foreground = Brushes.Black;
            }
        }

        private void txtbxEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxEmail.Text == "")
            {
                txtbxEmail.Text = "Enter Email Address";
                txtbxEmail.Foreground = Brushes.Silver;
            }
        }

        private void txtbxFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateFirstName(txtbxFirstName.Text)==false)
            {
                lblSignalFirstName.Content = "Enter first name";
            }
            else
            {
                lblSignalFirstName.Content = "";
            }
        }

        private void txtbxLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateLastName(txtbxLastName.Text) == false)
            {
                lblSignalLastName.Content = "Enter last name";
            }
            else
            {
                lblSignalLastName.Content = "";
            }
        }

        private void txtbxRegisrationNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateRegistrationNumber(txtbxRegisrationNumber.Text) == false)
            {
                lblSignalRegistrationNumber.Content = "Enter registration number";
            }
            else
            {
                lblSignalRegistrationNumber.Content = "";
            }
        }

        private void txtbxContact_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateContactNumber(txtbxContact.Text) == false)
            {
                lblSignalContact.Content = "Enter contact number";
            }
            else
            {
                lblSignalContact.Content = "";
            }
        }

        private void txtbxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidateEmail(txtbxEmail.Text) == false)
            {
                lblSignalEmail.Content = "Enter email address";
            }
            else
            {
                lblSignalEmail.Content = "";
            }
        }
    }
}
