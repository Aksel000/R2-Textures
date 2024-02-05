using System;
using System.Windows.Forms;
using System.Drawing;
using OpenGL;
using System.IO;
using System.Runtime.InteropServices;

namespace MyFormProject {
    /// <summary>
    /// Example form to contain the example implementation of BaseGLControl
    /// </summary>
    class MainForm :System.Windows.Forms.Form {
        TestGL glControl = new TestGL(true);		//Example implementation
        private Timer updateTimer = new Timer();	//Refresh  timer

        static Form _this = null;
        /// <summary>
        /// Singleton for accessing our application
        /// </summary>
        public static Form App {
            get {
                if (_this == null)
                    _this = new MainForm();
                return _this;
            }
        }

        public MainForm() {
            InitializeComponent();
            glControl.Location = new Point(0, 0);	//Position control at 0
            glControl.Dock = DockStyle.Fill;		//Dock to fill form
            //glControl.Size = new Size(this.Width - 20, this.Height - 20);
            //glControl.BorderStyle = BorderStyle.Fixed3D;
            glControl.Visible = true;


            this.Load += new EventHandler(MainForm_Load);	//Add load handler to create timer
            this.Closing += new System.ComponentModel.CancelEventHandler(MainForm_Closing);
            this.Controls.Add(glControl);
        }
        void InitializeComponent() {
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(492, 373);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NeHe C# Framework";
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (updateTimer != null)		//Close refresh timer
				{
                    updateTimer.Stop();
                    updateTimer.Dispose();
                    updateTimer = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// When the form loads create a refresh timer
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e) {
            //glControl.Size = new Size(this.Width - 20, this.Height - 20);

            updateTimer.Interval = 10;
            updateTimer.Tick += new EventHandler(updateTimer_Tick);
            updateTimer.Start();
        }

        /// <summary>
        /// When the timer fires, refresh control
        /// </summary>
        private void updateTimer_Tick(object sender, EventArgs e) {
            glControl.Invalidate();
        }

        /// <summary>
        /// When the form closes, close the refresh timer
        /// </summary>
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (updateTimer != null) {
                updateTimer.Stop();
                updateTimer.Dispose();
                updateTimer = null;
            }
        }

        [STAThread]
        public static void Main(string[] args) {
            MainForm form = (MainForm)MainForm.App;
            /*DialogResult res = MessageBox.Show(null,"Would You Like To Run In Fullscreen Mode?",
				"Start Fullscreen?",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
			
			if(res == DialogResult.Yes)
			{
				form.FormBorderStyle = FormBorderStyle.None;
				form.Location = new Point(0,0);
				form.Size = Screen.PrimaryScreen.Bounds.Size;
			}*/
            Application.Run(form);
        }


    }
}
