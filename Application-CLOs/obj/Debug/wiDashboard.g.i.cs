﻿#pragma checksum "..\..\wiDashboard.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "2DD58B9C6E6D16A9D511FC6814739B97596F38B132F8736F26FCC8AB8A6A85A8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Application_CLOs;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Application_CLOs {
    
    
    /// <summary>
    /// windDashboard
    /// </summary>
    public partial class windDashboard : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\wiDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Application_CLOs.windDashboard Dashoard;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\wiDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAssessment;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\wiDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnQuestions;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\wiDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRubric;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\wiDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnLevels;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\wiDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDashboard;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\wiDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnStudent;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\wiDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAttendance;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\wiDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnResult;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\wiDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCLOs;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Application-CLOs;component/widashboard.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\wiDashboard.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Dashoard = ((Application_CLOs.windDashboard)(target));
            return;
            case 2:
            this.btnAssessment = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.btnQuestions = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.btnRubric = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.btnLevels = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.btnDashboard = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\wiDashboard.xaml"
            this.btnDashboard.Click += new System.Windows.RoutedEventHandler(this.btnDashboard_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnStudent = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\wiDashboard.xaml"
            this.btnStudent.Click += new System.Windows.RoutedEventHandler(this.btnStudent_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnAttendance = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.btnResult = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.btnCLOs = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

