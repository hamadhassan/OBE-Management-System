using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Application_CLOs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Load the Xceed.Wpf.Toolkit assembly
            Assembly.Load("Xceed.Wpf.Toolkit");

            base.OnStartup(e);
        }
    }
}
