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
using System.Collections.ObjectModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Application_CLOs
{
    /// <summary>
    /// Interaction logic for StudentAttendance.xaml
    /// </summary>
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public partial class StudentAttendance : Window
    {
        public StudentAttendance()
        {
            InitializeComponent();
        }
        private void bindDataGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT S.Id,CONCAT(S.FirstName,' ',S.LastName) AS [Student Name],S.RegistrationNumber \r\nFROM Student S \r\nJOIN Lookup L\r\nON L.LookupId=S.Status\r\nWHERE L.Category='STUDENT_STATUS' AND L.Name='Active'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgAttendance.ItemsSource = dt.DefaultView;
            //if (dgAttendance.Columns.Count > 1)
            //{
            //    dgAttendance.Columns[0].Visibility = Visibility.Hidden;
            //}
          
        }
        public ObservableCollection<Person> People { get; set; }
        public List<int> AgeList { get; set; }

        private void bindAttendanceName()
        {

            //var con = Configuration.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("SELECT Name FROM Lookup WHERE Category='ATTENDANCE_STATUS'\r\n", con);
            //SqlDataReader reader = cmd.ExecuteReader();
         

            // Find the DataGridTemplateColumn
            DataGridTemplateColumn column = dgAttendance.Columns[3] as DataGridTemplateColumn;

            // Find the ComboBox control within the column's DataTemplate
            ComboBox comboBoxMarkAttendance = column.CellTemplate.FindName("Attendance", column.GetCellContent(dgAttendance.Items[2])) as ComboBox;

            // Do something with the ComboBox control
            if (comboBoxMarkAttendance != null)
            {
                comboBoxMarkAttendance.Items.Add("abc");

                // Access the properties or subscribe to events of the ComboBox control here
            }


            //}

            // Get the DataGridRow that contains the ComboBox
            //DataGridRow row = (DataGridRow)myDataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);

            //// Find the ComboBox within the row
            //ComboBox comboBox = FindVisualChild<ComboBox>(row);

            //// Helper method to find a visual child element of a certain type
            //private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
            //{
            //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            //    {
            //        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
            //        if (child != null && child is T)
            //        {
            //            return (T)child;
            //        }
            //        else
            //        {
            //            T childOfChild = FindVisualChild<T>(child);
            //            if (childOfChild != null)
            //            {
            //                return childOfChild;
            //            }
            //        }
            //    }
            //    return null;
        }
        private void Window_Activated(object sender, EventArgs e)
        {
             bindDataGrid();
             bindAttendanceName();
        }

        private void cmbxMarkAttendance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
