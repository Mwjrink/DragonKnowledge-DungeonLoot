﻿#pragma checksum "..\..\..\..\Views\UserControls\CharacterContainerPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BC79A937C32B47470BC6F35FDBF3D1DDE1672AD9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
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


namespace DKDG.Views.UserControls {
    
    
    /// <summary>
    /// CharacterContainerPage
    /// </summary>
    public partial class CharacterContainerPage : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 33 "..\..\..\..\Views\UserControls\CharacterContainerPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView CharacterList;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Views\UserControls\CharacterContainerPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem Save;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Views\UserControls\CharacterContainerPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem SaveAs;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\Views\UserControls\CharacterContainerPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem Delete;
        
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
            System.Uri resourceLocater = new System.Uri("/DKDG;component/views/usercontrols/charactercontainerpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\UserControls\CharacterContainerPage.xaml"
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
            this.CharacterList = ((System.Windows.Controls.ListView)(target));
            return;
            case 2:
            this.Save = ((System.Windows.Controls.MenuItem)(target));
            
            #line 37 "..\..\..\..\Views\UserControls\CharacterContainerPage.xaml"
            this.Save.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(this.MenuItem_MouseUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.SaveAs = ((System.Windows.Controls.MenuItem)(target));
            
            #line 38 "..\..\..\..\Views\UserControls\CharacterContainerPage.xaml"
            this.SaveAs.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(this.MenuItem_MouseUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Delete = ((System.Windows.Controls.MenuItem)(target));
            
            #line 39 "..\..\..\..\Views\UserControls\CharacterContainerPage.xaml"
            this.Delete.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(this.MenuItem_MouseUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

