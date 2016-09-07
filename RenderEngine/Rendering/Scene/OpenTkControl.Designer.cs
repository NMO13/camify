namespace RenderEngine.Rendering.Scene
{
    partial class OpenTkControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // OpenTkControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "OpenTkControl";
            this.Load += new System.EventHandler(this.OpenTkControl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OpenTkControl_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OpenTkControl_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OpenTkControl_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OpenTkControl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OpenTkControl_MouseUp);
            this.Resize += new System.EventHandler(this.OpenTkControl_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
