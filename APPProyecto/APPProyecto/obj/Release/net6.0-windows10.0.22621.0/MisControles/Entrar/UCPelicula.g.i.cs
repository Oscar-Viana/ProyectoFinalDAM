﻿#pragma checksum "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9B47F41C75E759ADBB662E91021115400AF5833D"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using APPProyecto.MisControles.Entrar;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace APPProyecto.MisControles.Entrar {
    
    
    /// <summary>
    /// UCPelicula
    /// </summary>
    public partial class UCPelicula : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 42 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border bordeComprobar;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Poster;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border pendienteFalse;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border pendienteTrue;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button favoritoFalse;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button favoritoTrue;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/APPProyecto;V1.0.0.0;component/miscontroles/entrar/ucpelicula.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.bordeComprobar = ((System.Windows.Controls.Border)(target));
            
            #line 42 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
            this.bordeComprobar.MouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Poster_MouseRightButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Poster = ((System.Windows.Controls.Image)(target));
            
            #line 43 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
            this.Poster.MouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Poster_MouseRightButtonDown);
            
            #line default
            #line hidden
            
            #line 43 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
            this.Poster.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Poster_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.pendienteFalse = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            
            #line 48 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.pendienteTrue = ((System.Windows.Controls.Border)(target));
            return;
            case 6:
            
            #line 52 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 7:
            this.favoritoFalse = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
            this.favoritoFalse.Click += new System.Windows.RoutedEventHandler(this.favoritoFalse_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.favoritoTrue = ((System.Windows.Controls.Button)(target));
            
            #line 68 "..\..\..\..\..\MisControles\Entrar\UCPelicula.xaml"
            this.favoritoTrue.Click += new System.Windows.RoutedEventHandler(this.favoritoTrue_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

