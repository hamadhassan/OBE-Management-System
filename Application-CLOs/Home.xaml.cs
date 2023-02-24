﻿using System;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }
        private void btnStudent_Click(object sender, RoutedEventArgs e)
        {
            StudentDashboard studentDashboard = new StudentDashboard();
            this.Hide();
            studentDashboard.Show();
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
        }

        private void btnCLOs_Click(object sender, RoutedEventArgs e)
        {
            AddCLOs addCLOs = new AddCLOs();
            addCLOs.ShowDialog();
        }

        private void btnLevels_Click(object sender, RoutedEventArgs e)
        {
            AddRubricLevel addRubricLevel = new AddRubricLevel();
            addRubricLevel.ShowDialog();
        }

        private void btnRubric_Click(object sender, RoutedEventArgs e)
        {
            AddRubric addRubric = new AddRubric();
            addRubric.ShowDialog();
        }
    }
}
