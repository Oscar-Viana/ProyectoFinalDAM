﻿#pragma checksum "..\..\..\..\..\MisControles\Administracion\UCPeticiones.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7B8ADEA22661439AE5AFAA3867E5738FD4D07C23"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using APPProyecto.MisControles.Administracion;
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


namespace APPProyecto.MisControles.Administracion {
    
    
    /// <summary>
    /// UCPeticiones
    /// </summary>
    public partial class UCPeticiones : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\..\MisControles\Administracion\UCPeticiones.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel stackPadre;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\..\..\MisControles\Administracion\UCPeticiones.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel stackPrimer;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\..\MisControles\Administracion\UCPeticiones.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblCorreo;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\..\MisControles\Administracion\UCPeticiones.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblNombre;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\..\MisControles\Administracion\UCPeticiones.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel stackSegun;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\..\MisControles\Administracion\UCPeticiones.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblAsunto;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\..\MisControles\Administracion\UCPeticiones.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPeticion;
        
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
            System.Uri resourceLocater = new System.Uri("/APPProyecto;component/miscontroles/administracion/ucpeticiones.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\MisControles\Administracion\UCPeticiones.xaml"
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
            this.stackPadre = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.stackPrimer = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.lblCorreo = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.lblNombre = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            
            #line 15 "..\..\..\..\..\MisControles\Administracion\UCPeticiones.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.stackSegun = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.lblAsunto = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.txtPeticion = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

