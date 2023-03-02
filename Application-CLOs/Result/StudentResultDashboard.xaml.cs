using System;
using System.Collections.Generic;
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
    /// Interaction logic for StudentResultDashboard.xaml
    /// </summary>
    public partial class StudentResultDashboard : Window
    {
        public StudentResultDashboard()
        {
            InitializeComponent();
        }

        private void btnMarkResult_Click(object sender, RoutedEventArgs e)
        {
            AddStudentResult addStudentResult=new AddStudentResult();
            addStudentResult.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewStudentResult viewStudentResult = new ViewStudentResult();
            viewStudentResult.ShowDialog();
        }
    }
}
