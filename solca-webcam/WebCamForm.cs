using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Microsoft.Net.Http;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Drawing.Imaging;

using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace solca_webcam
{
    public partial class WebCamForm : Form
    {
        private VideoCapture _capture;
        private CascadeClassifier _cascade;
        private SaveFileDialog dialog;

        private int _cameraIndex = 0; 
        private Boolean _camaraEnProgreso;
        private Boolean _saveImageToFile;

        private String appDataPath;
        private WebcamApp webcam;

        /**
         * Path of last image file
         */ 
        private String lastImagePath;

        static readonly String ConfigFile = "solca.webcam.ini";
        
        /**
         * Inicializa la app
         */
        public WebCamForm()
        {
            InitializeComponent();

            // Lista webcams
            BuscarDispositivos();

            //
            InicializaAppFolder();

            // 
            InicializaControlador();
        }

        /**
         * Inicializa el controlador
         */
        private void InicializaControlador()
        {
            // Habilita el controlador
            this.webcam = new WebcamApp(this.appDataPath, ConfigFile);
            this.onlineStatus.Image = global::solca_webcam.Properties.Resources.offline_icon;
            this.searchButton.Enabled = false;

        }


        /**
         * Inicializa carpeta de trabajo
         */
        private void InicializaAppFolder()
        {
            appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                @"SOLCA\WebCam\");

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
        }
        
        /**
         * Busca dispositivos
         */
        public void BuscarDispositivos()
        {
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                for (int i = 0; i < videoDevices.Count; i++) {
                    dispositivosBox.Items.Add(videoDevices[i].Name.ToString());
                }

                dispositivosBox.Text = dispositivosBox.Items[0].ToString();
            }
        }

        /**
         * Inicializa Video
         */
        public void InitializeVideo()
        {
            // Desactiva Zoom & Pan
            imageBox.FunctionalMode = ImageBox.FunctionalModeOption.Minimum;

            String cascadeFile = Path.Combine(Environment.CurrentDirectory, "haarcascade_frontalface_alt2.xml");            
            /*
            if (Debugger.IsAttached)
            {
                cascadeFile = @"C:\dev\dev_net\solca-webcam\resourcefiles\haarcascade_frontalface_alt2.xml";
            }*/

            if (! File.Exists(cascadeFile) ) {
                // Notificar y salir
                MessageBox.Show("Archivo de configuración: " + cascadeFile + " no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            if (dispositivosBox.Items.Count == 0)
            {
                // Notificar y salir
                MessageBox.Show("No se encontró ninguna Webcam.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            // Activar configuración                
            _cascade = new CascadeClassifier(cascadeFile);

            // Activa proceso
            ActivarDesactivarCamera(false);
        }

        /**
         * Procesa la imagen (detecta el rostro)
         * 
         * (Esta funcion hace la magia)
         */
        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                using (var imageFrame = _capture.QueryFrame().ToImage<Bgr, Byte>())
                {
                    if (imageFrame != null)
                    {
                        var grayframe = imageFrame.Convert<Gray, byte>();
                        var faces = _cascade.DetectMultiScale(grayframe, 1.1, 10, Size.Empty); //the actual face detection happens here
                        foreach (var face in faces)
                        {
                            Rectangle r2 = new Rectangle(face.X - 30, face.Y - 50, face.Width + 60, face.Height + 130);
                            imageFrame.Draw(r2, new Bgr(Color.BurlyWood), 3); // the detected face(s) is highlighted here using a box that is drawn around it/them

                            // Guardar imagen
                            if (_saveImageToFile)
                            {
                                _saveImageToFile = false;
                                ActivarDesactivarCamera(true);

                                string imagePath = Path.Combine(appDataPath, "original.jpg");
                                imageFrame.Save(imagePath);
                                CropImage(imagePath, r2);                                
                            }

                        }
                    }

                    imageBox.Image = imageFrame;
                }   
            }
        }
        
        /**
         * Salir de la aplicacion
         */
        private void salirMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /**
         * Iniciar Camara 
         */
        private void iniciarCamMenuItem_Click(object sender, EventArgs e)
        {
            ActivarDesactivarCamera(true);
        }

        /**
         * Detener camara
         */
        private void detenerCamMenuItem_Click(object sender, EventArgs e)
        {
            ActivarDesactivarCamera(true);
        }

        /**
         * Realiza el proceso de la camera
         */ 
        private void ActivarDesactivarCamera(bool changeTab)
        {
            int camIndex = 0;
            _saveImageToFile = false;

            Boolean activar = !_camaraEnProgreso; // Activa proceso
            
            if (activar && _cameraIndex != dispositivosBox.SelectedIndex)
            {
                if (_capture != null)
                {
                    _capture.Stop();
                    _capture.Dispose();

                    _capture = null;
                    _cameraIndex = dispositivosBox.SelectedIndex;

                    camIndex = _cameraIndex;
                }                    
            }
            
            if (_capture == null)
            {
                try
                {
                    // Inicializar video
                    _capture = new VideoCapture(camIndex);
                }
                catch (Exception excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }

            if (_capture != null)
            {
                if (activar)
                {
                    Application.Idle += ProcessFrame;
                    toolStatusLabel.Text = "Camara " + camIndex + " activada";
                    if (changeTab) tabControl.SelectedIndex = 1; // Cambiar tab
                }
                else
                {
                    Application.Idle -= ProcessFrame;
                    toolStatusLabel.Text = "Camara " + camIndex + " finalizada";
                }

                CapturarButton.Enabled = activar;
                detenerCamMenuItem.Enabled = activar;

                iniciarCamMenuItem.Enabled = !activar;
                dispositivosBox.Enabled = !activar;

                _camaraEnProgreso = activar;
            }
        }
        
        /**
         * Guardar imagen
         */
        private void CropImage(String sourceImage, Rectangle cropRect)
        {
            Bitmap src = Image.FromFile(sourceImage) as Bitmap;
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }

            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            // EncoderParameter myEncoderParameter = ;
            myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, 50L);

            String cropImagePath = getCropImagePath();
            target.Save(cropImagePath, jpgEncoder, myEncoderParameters);
                        
            //target.Save(cropImagePath);

            fotoPaciente.Image = target;
            fotoPaciente.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;

            tabControl.SelectedIndex = 0; // Cambiar tab            
            toolStatusLabel.Text = "Imagen guardada como " + cropImagePath;

            SubirButton.Enabled = true;
            GuardarButton.Enabled = true;
        }

        private String getCropImagePath()
        {
            this.lastImagePath = Path.Combine(appDataPath, this.webcam.NumeroPaciente + ".jpg");
            return this.lastImagePath;
        }

        private void CapturarButton_Click(object sender, EventArgs e)
        {
            CapturarButton.Enabled = false;
            _saveImageToFile = true;
        }

        /**
         * Upload foto 
         */
        private async Task<System.IO.Stream> UploadFoto(
            string actionUrl,
            string paramString,
            string filePath)
        {
            using (var client = new HttpClient())
            using (var fileStream = File.OpenRead(filePath))
            using (var formData = new MultipartFormDataContent())
            {
                HttpContent bytes = new ByteArrayContent(
                    new StreamContent(fileStream).ReadAsByteArrayAsync().Result);

                bytes.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
                bytes.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "foto.jpg",
                    Name ="foto",
                };

                HttpContent stringContent = new StringContent(paramString);

                formData.Add(stringContent, "param1", "param1");
                   
                //
                //formData.Add(fileStreamContent, "file1", "file1");
                //formData.Add(bytesContent, "file2", "file2");

                var response = await client.PostAsync(actionUrl, formData);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadAsStreamAsync();
            }
        }
        
        private void WebCamForm_Shown(object sender, EventArgs e)
        {
            // InitializeVideo(); // Inicializa cideo
            if (this.webcam.getConnectionString() != null)
            {
                // Validate DB async
                Task<bool> ok = TestDbConexion();
            }
        }


        /**
         * Test conexion is valid
         */
        private async Task<bool> TestDbConexion()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool ok = await Task.Run(() => ValidarConexion(webcam));
            Cursor.Current = Cursors.Default;
            
            showDbStatus(ok);            
            return ok;
        }

        private static bool ValidarConexion(WebcamApp webcam)
        {
            return webcam.ValidarConexion();
        }

        private void WebCamForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen(); 
        }

        private void SubirButton_Click(object sender, EventArgs e)
        {
            bool ok = false;
            Cursor.Current = Cursors.WaitCursor;
            SubirButton.Enabled = false;

            try
            {
                ok = this.webcam.SubirImageBaseDatos(getCropImagePath());

            } finally
            {
                Cursor.Current = Cursors.Default;
            }
            
            if (ok)
            {
                ActivarDesactivarCamera(false);
                MessageBox.Show("Foto Paciente HC="+ this.webcam.NumeroPaciente +" guardada exitosamente.");
            }
            else
            {
                SubirButton.Enabled = true;
                MessageBox.Show(this.webcam.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
         * Guardar en archivo
         */
        private void GuardarButton_Click(object sender, EventArgs e)
        {
            if (dialog == null)
            {
                dialog = new System.Windows.Forms.SaveFileDialog();
            }

            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Title = "Guardar foto como...";
            dialog.Filter = "Image files (*.jpg)|*.jpg";
            dialog.FilterIndex = 1;
            dialog.CheckPathExists = true;
            dialog.FileName = this.webcam.NumeroPaciente + "_foto.jpg";
           
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Guardar imagen
                String saveFile = dialog.FileName;
                File.Copy(this.lastImagePath, saveFile);
            }
        }

        private System.Windows.Forms.ToolTip ToolTip1;

        private void PrepareTooltips()
        {
            ToolTip1 = new System.Windows.Forms.ToolTip();
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button && ctrl.Tag is string)
                {
                    ctrl.MouseHover += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        var btn = (Control)o;
                        ToolTip1.SetToolTip(btn, btn.Tag.ToString());
                    });
                }
            }
        }

        /**
         * Get Temp filename
         */
        public static string GetTempFileName(string extension)
        {
            int attempt = 0;
            while (true)
            {
                string fileName = Path.GetRandomFileName();
                fileName = Path.ChangeExtension(fileName, extension);
                fileName = Path.Combine(Path.GetTempPath(), fileName);

                try
                {
                    using (new FileStream(fileName, FileMode.CreateNew)) { }
                    return fileName;
                }
                catch (IOException ex)
                {
                    if (++attempt == 10)
                        throw new IOException("No unique temporary file name is available.", ex);
                }
            }
        }

        /**
         * 
         */
        private void urlIntranetMenuItem_Click(object sender, EventArgs e)
        {
            String urlIntranet = PromptDialog("Ingrese el URL Intranet: ", "URL Intranet", GetUrlIntranet());
            if (urlIntranet != null && urlIntranet.Length > 0)
            {
                SetUrlIntranet(urlIntranet);
            }            
        }

        /**
         * Prompt a Dialog
         */ 
        public string PromptDialog(string label, string caption, string text, int formWidth = 340, int formHeight = 200)
        {
            var prompt = new Form
            {
                Width = formWidth,
                Height = formHeight - 40,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false
            };

            var textLabel = new Label
            {
                Left = 14,
                Padding = new Padding(0, 3, 0, 0),
                Text = label,
                Dock = DockStyle.Top
            };

            var textBox = new TextBox
            {
                Left = 10,
                Top = textLabel.Height + 14,
                Text = text,
                Dock = DockStyle.None,
                Width = prompt.Width - 36,
                Anchor = AnchorStyles.Left | AnchorStyles.Right                
            };

            var confirmationButton = new Button
            {
                Text = @"OK",
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Bottom,
            };

            confirmationButton.Click += (sender, e) =>
            {
                prompt.Close();
            };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmationButton);
            prompt.Controls.Add(textLabel);

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }

        /**
         * Guarda el url intranet
         */ 
        private void SetUrlIntranet(String urlIntranet)
        {
            String configFile = Path.Combine(appDataPath, ConfigFile);

            System.IO.File.WriteAllText(configFile, "intranet.url=" + urlIntranet);
            toolStatusLabel.Text = "Url Intranet: " + urlIntranet + " guardado en " + configFile;
        }

        /**
         * Get url intranet
         */
        private String GetUrlIntranet()
        {
            String configFile = Path.Combine(appDataPath, ConfigFile);
            if (File.Exists(configFile))
            {
                String rawText = System.IO.File.ReadAllText(configFile);                
                if (rawText != null && rawText.IndexOf('=') != -1)
                {
                    String urlIntranet = rawText.Split('=')[1];
                    toolStatusLabel.Text = "Url Intranet: " + urlIntranet;
                    return urlIntranet;
                }                
            }
            return null;
        }

        /**
         * 
         */
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void OpcionesMenuItem_Click(object sender, EventArgs e)
        {
            //
            ConfigForm form = new ConfigForm();
            form.InicilizaOpciones(this.webcam); // this.appDataPath, ConfigFile);

            DialogResult res = form.ShowDialog(this);
            form.GuardarOpciones();
            form.Dispose();

            showDbStatus(this.webcam.ConnectionAvailable);
        }

        /**
         * Habilita el boton search
         */
        private void showDbStatus(Boolean ok)
        {
            if (ok)
            {
                // 
                this.onlineStatus.Image = global::solca_webcam.Properties.Resources.online_icon;
                this.onlineStatus.Text = this.webcam.User + "/" + this.webcam.Db;                
            }
            else
            {
                // 
                this.onlineStatus.Text = webcam.ErrorMessage;
                this.onlineStatus.Image = global::solca_webcam.Properties.Resources.nocnn_icon;
            }
            this.searchButton.Enabled = ok;
        }

        private void NumHcText_KeyDown(object sender, KeyEventArgs e)
        {
            toolStatusLabel.Text = "";

            if (e.KeyCode == Keys.Enter)
            {
                buscarPacienteFoto(NumHcText.Text);
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            buscarPacienteFoto(NumHcText.Text);
        }

        /**
         * Busca una paciente
         */
        private void buscarPacienteFoto(String numeroPacienteText)
        {   
            // Quitar espacios
            numeroPacienteText = numeroPacienteText == null ? "" : numeroPacienteText.Trim();

            if (String.IsNullOrEmpty(numeroPacienteText)) {
                toolStatusLabel.Text = "HC debe ser un numero!";
                NumHcText.Focus();
                return;
            }

            try
            {
                Int32 hc = Int32.Parse(numeroPacienteText);
                (string nombre, byte[] imageData) foto = webcam.ConsultarFoto(hc);

                fotoPaciente.Image = null;
                toolStatusLabel.Text = foto.nombre;
                NombreText.Text = foto.nombre;

                if (webcam.ErrorMessage != null)
                {
                    toolStatusLabel.Text = webcam.ErrorMessage;
                    NumHcText.Focus();
                }
                else 
                {
                    GuardarButton.Enabled = (foto.imageData != null);
                    searchButton.Enabled = false;

                    if (foto.imageData != null)
                    {
                        // Guardar ultima imagen
                        this.lastImagePath = Path.Combine(appDataPath, hc + ".jpg");
                        using (FileStream fs = new FileStream(this.lastImagePath, FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(foto.imageData, 0, (int)foto.imageData.Length);
                            fs.Close();
                        }

                        // Presentar en pantalla
                        using (MemoryStream memStream = new MemoryStream(foto.imageData))
                        {
                            Bitmap image = Image.FromStream(memStream) as Bitmap;
                            fotoPaciente.Image = image;
                            fotoPaciente.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;                            
                        }
                    }                                         
                }
                
            }
            catch (Exception e)
            {
                toolStatusLabel.Text = "HC debe ser un numero! - " + e.Message;
            }                        
        }

        private void NumHcText_TextChanged(object sender, EventArgs e)
        {
            ResetNumeroPaciente();
        }

        private void ResetNumeroPaciente()
        {
            string numeroPaciente = NumHcText.Text;
            numeroPaciente = numeroPaciente == null ? "" : numeroPaciente.Trim();

            Int32 hc = 0;
            Int32.TryParse(numeroPaciente, out hc);

            if (hc != webcam.NumeroPaciente)
            {
                fotoPaciente.Image = null;
                toolStatusLabel.Text = null;
                NombreText.Text = null;

                this.webcam.NumeroPaciente = 0;
                GuardarButton.Enabled = false;
                searchButton.Enabled = true;
            }
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl.SelectedIndex == 1)
            {
                if (this._cascade == null)
                {
                    InitializeVideo(); // Inicializa video
                }
                else if (!this._camaraEnProgreso)
                {
                    ActivarDesactivarCamera(false);
                }
            }
        }

        // END/CLASS
    }

}
