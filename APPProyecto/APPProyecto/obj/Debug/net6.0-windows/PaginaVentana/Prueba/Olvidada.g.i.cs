// Updated by XamlIntelliSenseFileGenerator 30/12/2023 17:19:49
#pragma checksum "..\..\..\..\..\PaginaVentana\Prueba\Olvidada.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F051B5308FA08E296C6D03705EA7B541D10DE914"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using APPProyecto.PaginaVentana.Principal;
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


namespace APPProyecto.PaginaVentana.Principal
{


    /// <summary>
    /// Olvidada
    /// </summary>
    public partial class Olvidada : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector
    {

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.5.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/APPProyecto;component/paginaventana/prueba/olvidada.xaml", System.UriKind.Relative);

#line 1 "..\..\..\..\..\PaginaVentana\Prueba\Olvidada.xaml"
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
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.Token = ((System.Windows.Controls.TextBox)(target));
                    return;
                case 2:
                    this.Contra1 = ((System.Windows.Controls.PasswordBox)(target));
                    return;
                case 3:
                    this.Contra2 = ((System.Windows.Controls.PasswordBox)(target));
                    return;
            }
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.Button btnGenerar;
        internal System.Windows.Controls.Button btnCambiar;
        internal System.Windows.Controls.TextBox txtCorreo;
        internal System.Windows.Controls.Button btnToken;
        internal System.Windows.Controls.PasswordBox txtContra1;
        internal System.Windows.Controls.PasswordBox txtContra2;
        internal System.Windows.Controls.TextBox txtToken;
    }
}

