namespace solca_webcam
{
    partial class WebCamForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebCamForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.iniciarCamMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detenerCamMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.OpcionesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.urlIntranetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.salirMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGuardar = new System.Windows.Forms.TabPage();
            this.NumHcText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.NombreText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabWebCam = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.dispositivosBox = new System.Windows.Forms.ComboBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.GuardarButton = new System.Windows.Forms.Button();
            this.SubirButton = new System.Windows.Forms.Button();
            this.fotoPaciente = new System.Windows.Forms.PictureBox();
            this.CapturarButton = new System.Windows.Forms.Button();
            this.imageBox = new Emgu.CV.UI.ImageBox();
            this.onlineStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabGuardar.SuspendLayout();
            this.tabWebCam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fotoPaciente)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onlineStatus,
            this.toolStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 532);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(806, 25);
            this.statusStrip.TabIndex = 7;
            // 
            // toolStatusLabel
            // 
            this.toolStatusLabel.Name = "toolStatusLabel";
            this.toolStatusLabel.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.toolStatusLabel.Size = new System.Drawing.Size(20, 20);
            this.toolStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(806, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iniciarCamMenuItem,
            this.detenerCamMenuItem,
            this.toolStripSeparator1,
            this.OpcionesMenuItem,
            this.urlIntranetMenuItem,
            this.toolStripSeparator2,
            this.salirMenuItem});
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(88, 22);
            this.toolStripButton1.Text = "Camara Web";
            // 
            // iniciarCamMenuItem
            // 
            this.iniciarCamMenuItem.Name = "iniciarCamMenuItem";
            this.iniciarCamMenuItem.Size = new System.Drawing.Size(157, 22);
            this.iniciarCamMenuItem.Text = "Iniciar camera";
            this.iniciarCamMenuItem.Click += new System.EventHandler(this.iniciarCamMenuItem_Click);
            // 
            // detenerCamMenuItem
            // 
            this.detenerCamMenuItem.Name = "detenerCamMenuItem";
            this.detenerCamMenuItem.Size = new System.Drawing.Size(157, 22);
            this.detenerCamMenuItem.Text = "Detener camera";
            this.detenerCamMenuItem.Click += new System.EventHandler(this.detenerCamMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(154, 6);
            // 
            // OpcionesMenuItem
            // 
            this.OpcionesMenuItem.Name = "OpcionesMenuItem";
            this.OpcionesMenuItem.Size = new System.Drawing.Size(157, 22);
            this.OpcionesMenuItem.Text = "Opciones...";
            this.OpcionesMenuItem.Click += new System.EventHandler(this.OpcionesMenuItem_Click);
            // 
            // urlIntranetMenuItem
            // 
            this.urlIntranetMenuItem.Name = "urlIntranetMenuItem";
            this.urlIntranetMenuItem.Size = new System.Drawing.Size(157, 22);
            this.urlIntranetMenuItem.Text = "Url Intranet...";
            this.urlIntranetMenuItem.Visible = false;
            this.urlIntranetMenuItem.Click += new System.EventHandler(this.urlIntranetMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(154, 6);
            // 
            // salirMenuItem
            // 
            this.salirMenuItem.Name = "salirMenuItem";
            this.salirMenuItem.Size = new System.Drawing.Size(157, 22);
            this.salirMenuItem.Text = "Salir";
            this.salirMenuItem.Click += new System.EventHandler(this.salirMenuItem_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGuardar);
            this.tabControl.Controls.Add(this.tabWebCam);
            this.tabControl.Location = new System.Drawing.Point(0, 28);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(806, 508);
            this.tabControl.TabIndex = 10;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // tabGuardar
            // 
            this.tabGuardar.Controls.Add(this.searchButton);
            this.tabGuardar.Controls.Add(this.NumHcText);
            this.tabGuardar.Controls.Add(this.label4);
            this.tabGuardar.Controls.Add(this.label3);
            this.tabGuardar.Controls.Add(this.GuardarButton);
            this.tabGuardar.Controls.Add(this.NombreText);
            this.tabGuardar.Controls.Add(this.label2);
            this.tabGuardar.Controls.Add(this.SubirButton);
            this.tabGuardar.Controls.Add(this.fotoPaciente);
            this.tabGuardar.Location = new System.Drawing.Point(4, 22);
            this.tabGuardar.Name = "tabGuardar";
            this.tabGuardar.Padding = new System.Windows.Forms.Padding(3);
            this.tabGuardar.Size = new System.Drawing.Size(798, 482);
            this.tabGuardar.TabIndex = 1;
            this.tabGuardar.Text = "Paciente";
            this.tabGuardar.UseVisualStyleBackColor = true;
            // 
            // NumHcText
            // 
            this.NumHcText.Location = new System.Drawing.Point(38, 37);
            this.NumHcText.MaxLength = 9;
            this.NumHcText.Name = "NumHcText";
            this.NumHcText.Size = new System.Drawing.Size(89, 20);
            this.NumHcText.TabIndex = 9;
            this.NumHcText.TextChanged += new System.EventHandler(this.NumHcText_TextChanged);
            this.NumHcText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumHcText_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Número Paciente:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(429, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Subir la foto a la Base de Datos?";
            // 
            // NombreText
            // 
            this.NombreText.Location = new System.Drawing.Point(133, 37);
            this.NombreText.Name = "NombreText";
            this.NombreText.ReadOnly = true;
            this.NombreText.Size = new System.Drawing.Size(290, 20);
            this.NombreText.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(133, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Nombres: ";
            // 
            // tabWebCam
            // 
            this.tabWebCam.Controls.Add(this.label1);
            this.tabWebCam.Controls.Add(this.dispositivosBox);
            this.tabWebCam.Controls.Add(this.CapturarButton);
            this.tabWebCam.Controls.Add(this.imageBox);
            this.tabWebCam.Location = new System.Drawing.Point(4, 22);
            this.tabWebCam.Name = "tabWebCam";
            this.tabWebCam.Padding = new System.Windows.Forms.Padding(3);
            this.tabWebCam.Size = new System.Drawing.Size(798, 482);
            this.tabWebCam.TabIndex = 0;
            this.tabWebCam.Text = "Tomar Foto";
            this.tabWebCam.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(650, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Cámara web:";
            // 
            // dispositivosBox
            // 
            this.dispositivosBox.FormattingEnabled = true;
            this.dispositivosBox.Location = new System.Drawing.Point(653, 103);
            this.dispositivosBox.Name = "dispositivosBox";
            this.dispositivosBox.Size = new System.Drawing.Size(137, 21);
            this.dispositivosBox.TabIndex = 8;
            // 
            // searchButton
            // 
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Image = global::solca_webcam.Properties.Resources.seach_icon;
            this.searchButton.Location = new System.Drawing.Point(429, 32);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(28, 28);
            this.searchButton.TabIndex = 10;
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // GuardarButton
            // 
            this.GuardarButton.Enabled = false;
            this.GuardarButton.Image = global::solca_webcam.Properties.Resources.save_icon;
            this.GuardarButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.GuardarButton.Location = new System.Drawing.Point(709, 81);
            this.GuardarButton.Name = "GuardarButton";
            this.GuardarButton.Size = new System.Drawing.Size(78, 55);
            this.GuardarButton.TabIndex = 6;
            this.GuardarButton.Tag = "Guardar Foto";
            this.GuardarButton.Text = "Guardar foto";
            this.GuardarButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.GuardarButton.UseVisualStyleBackColor = true;
            this.GuardarButton.Click += new System.EventHandler(this.GuardarButton_Click);
            // 
            // SubirButton
            // 
            this.SubirButton.Enabled = false;
            this.SubirButton.Image = global::solca_webcam.Properties.Resources.download_icon;
            this.SubirButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SubirButton.Location = new System.Drawing.Point(597, 81);
            this.SubirButton.Name = "SubirButton";
            this.SubirButton.Size = new System.Drawing.Size(106, 55);
            this.SubirButton.TabIndex = 3;
            this.SubirButton.Tag = "Subir foto a la BD";
            this.SubirButton.Text = "Subir Foto a BD";
            this.SubirButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.SubirButton.UseVisualStyleBackColor = true;
            this.SubirButton.Click += new System.EventHandler(this.SubirButton_Click);
            // 
            // fotoPaciente
            // 
            this.fotoPaciente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fotoPaciente.Location = new System.Drawing.Point(38, 63);
            this.fotoPaciente.Name = "fotoPaciente";
            this.fotoPaciente.Size = new System.Drawing.Size(385, 401);
            this.fotoPaciente.TabIndex = 0;
            this.fotoPaciente.TabStop = false;
            // 
            // CapturarButton
            // 
            this.CapturarButton.Enabled = false;
            this.CapturarButton.Image = global::solca_webcam.Properties.Resources.camera_icon;
            this.CapturarButton.Location = new System.Drawing.Point(653, 18);
            this.CapturarButton.Name = "CapturarButton";
            this.CapturarButton.Size = new System.Drawing.Size(137, 55);
            this.CapturarButton.TabIndex = 7;
            this.CapturarButton.UseVisualStyleBackColor = true;
            this.CapturarButton.Click += new System.EventHandler(this.CapturarButton_Click);
            // 
            // imageBox
            // 
            this.imageBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.imageBox.Location = new System.Drawing.Point(3, 3);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(640, 480);
            this.imageBox.TabIndex = 6;
            this.imageBox.TabStop = false;
            // 
            // onlineStatus
            // 
            this.onlineStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.onlineStatus.Image = global::solca_webcam.Properties.Resources.nocnn_icon;
            this.onlineStatus.Name = "onlineStatus";
            this.onlineStatus.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.onlineStatus.Size = new System.Drawing.Size(123, 20);
            this.onlineStatus.Text = "<Usuario/BD>";
            this.onlineStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // WebCamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 557);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WebCamForm";
            this.Text = "Webcam - Solca";
            this.Load += new System.EventHandler(this.WebCamForm_Load);
            this.Shown += new System.EventHandler(this.WebCamForm_Shown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabGuardar.ResumeLayout(false);
            this.tabGuardar.PerformLayout();
            this.tabWebCam.ResumeLayout(false);
            this.tabWebCam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fotoPaciente)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem iniciarCamMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detenerCamMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem salirMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStatusLabel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabWebCam;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox dispositivosBox;
        private System.Windows.Forms.Button CapturarButton;
        private Emgu.CV.UI.ImageBox imageBox;
        private System.Windows.Forms.TabPage tabGuardar;
        private System.Windows.Forms.Button SubirButton;
        private System.Windows.Forms.PictureBox fotoPaciente;
        private System.Windows.Forms.TextBox NombreText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button GuardarButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem urlIntranetMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem OpcionesMenuItem;
        private System.Windows.Forms.TextBox NumHcText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripStatusLabel onlineStatus;
        private System.Windows.Forms.Button searchButton;
    }
}

