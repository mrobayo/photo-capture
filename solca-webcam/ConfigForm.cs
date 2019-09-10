using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using Oracle.ManagedDataAccess.Client;

using IniParser;
using IniParser.Model;
using System.IO;

namespace solca_webcam
{
    partial class ConfigForm : Form
    {

        /**
         * web cam
         */
        WebcamApp webcam;

        /**
         * Mensaje de error de la conexion
         */
        // public string errorMessage;

        public ConfigForm()
        {
            InitializeComponent();
            this.Text = String.Format("Opciones... {0}", AssemblyTitle);            
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void ValidarButton_Click(object sender, EventArgs e)
        {
            string db = DbText.Text;
            string user = UserText.Text;
            string pass = PassText.Text;

            Cursor.Current = Cursors.WaitCursor;
            Boolean ok = webcam.ValidarConexion(user, pass, db);
            Cursor.Current = Cursors.Default;
            
            if (ok)
            {
                MessageBox.Show("Conexion exitosa a " + db, "Conexion exitosa!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(webcam.ErrorMessage, "Fallo conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
         * Guardar ini
         */
        public void GuardarOpciones()
        {
            // Guardar configuracion
            this.webcam.GuardarOpciones(UserText.Text, PassText.Text, DbText.Text);
        }

        /**
         * Lee archivo ini 
         */
        public void InicilizaOpciones(WebcamApp webcam) //string appDataPath, string configFileName)
        {
            this.webcam = webcam; //new WebcamApp(appDataPath, configFileName);
            this.webcam.InicilizaOpciones();

            UserText.Text = this.webcam.User;
            PassText.Text = this.webcam.Pass;
            DbText.Text = this.webcam.Db;
        }

    }
}
