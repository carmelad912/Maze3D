﻿#pragma checksum "..\..\..\View\Generate.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9D079D18E053C9C95F2F668DB2B7D050"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ATPProject.View;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace ATPProject.View {
    
    
    /// <summary>
    /// Generate
    /// </summary>
    public partial class Generate : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\View\Generate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label mazename;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\View\Generate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label rows;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\View\Generate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label columns;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\View\Generate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label floors;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\View\Generate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button generate;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\View\Generate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Tname;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\View\Generate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Trows;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\View\Generate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Tcolumns;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\View\Generate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Tfloors;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\View\Generate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cancel;
        
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
            System.Uri resourceLocater = new System.Uri("/ATPProject;component/view/generate.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\Generate.xaml"
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
            this.mazename = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.rows = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.columns = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.floors = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.generate = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\View\Generate.xaml"
            this.generate.Click += new System.Windows.RoutedEventHandler(this.generate_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Tname = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.Trows = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.Tcolumns = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.Tfloors = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.cancel = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\View\Generate.xaml"
            this.cancel.Click += new System.Windows.RoutedEventHandler(this.cancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

