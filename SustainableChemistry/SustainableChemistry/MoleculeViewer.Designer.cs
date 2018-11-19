namespace SustainableChemistry
{
    partial class MoleculeViewer
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
            // MoleculeViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(1100, 850);
            this.Name = "MoleculeViewer";
            this.Size = new System.Drawing.Size(300, 300);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MoleculeViewer_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MoleculeViewer_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MoleculeViewer_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoleculeViewer_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MoleculeViewer_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoleculeViewer_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
