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
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }
        public void LoadForm(Window window)
        {
            panel.Children.Clear();
            object content = window.Content;
            window.Content = null;
            window.Close();
            this.panel.Children.Add(content as UIElement);
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            LoadForm(new Home());
        }

        private void btnStudent_Click(object sender, RoutedEventArgs e)
        {
            LoadForm(new StudentDashboard());
        }

        private void btnAttendance_Click(object sender, RoutedEventArgs e)
        {
            LoadForm(new StudentAttendance());
        }

        private void btnResult_Click(object sender, RoutedEventArgs e)
        {
            LoadForm(new StudentResultDashboard());
        }

        private void btnCLOs_Click(object sender, RoutedEventArgs e)
        {
            LoadForm(new AddCLOs());
        }

        private void btnLevels_Click(object sender, RoutedEventArgs e)
        {
            LoadForm(new AddRubricLevel());
        }

        private void btnRubric_Click(object sender, RoutedEventArgs e)
        {
            LoadForm(new AddRubric());
        }

        private void btnQuestions_Click(object sender, RoutedEventArgs e)
        {
            LoadForm(new AddAssessmentComponent());

        }

        private void btnAssessment_Click(object sender, RoutedEventArgs e)
        {
            LoadForm(new AddAssessment());
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadForm(new Home());
        }
    }
}
