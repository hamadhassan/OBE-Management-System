﻿using System;
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
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using Microsoft.Win32;
using iTextSharp.text.pdf.draw;

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
           
        }
       
        private void bindDataGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT S.id,S.FirstName,S.lastname,S.Contact,S.email,S.RegistrationNumber,L.Name AS Status FROM Student S JOIN Lookup  L ON L.LookupId=S.Status WHERE  L.Category='STUDENT_STATUS'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgStudent.ItemsSource = dt.DefaultView;
            //if (dgStudent.Columns.Count > 1)
            //{
            //    dgStudent.Columns[2].Visibility = Visibility.Hidden;
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
            string firstName = dataRowView[1].ToString();
            string lastName = dataRowView[2].ToString();
            string contact = dataRowView[3].ToString();
            string email=dataRowView[4].ToString();
            string RegistrationNumber = dataRowView[5].ToString();
            string status =dataRowView[6].ToString();
            AddStudent addStudent = new AddStudent(id,firstName,lastName,contact,email,RegistrationNumber,status);
            addStudent.ShowDialog();
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
                    bindDataGrid();
                   
                }
            }
            catch
            {
                MessageBox.Show("You are trying to access the wrong field","Information",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            bindDataGrid();
           
        }

       
      
        
    }
}
