namespace SustainableChemistry
{
    partial class FunctionalGroupEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.moleculeViewer1 = new SustainableChemistry.MoleculeViewer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(76, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(301, 20);
            this.textBox1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(34, 140);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(327, 282);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(412, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Next";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "SMART";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(79, 59);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(285, 20);
            this.textBox2.TabIndex = 5;
            // 
            // moleculeViewer1
            // 
            this.moleculeViewer1.AutoScroll = true;
            this.moleculeViewer1.AutoScrollMinSize = new System.Drawing.Size(1056, 816);
            this.moleculeViewer1.AutoSize = true;
            this.moleculeViewer1.BackColor = System.Drawing.SystemColors.Window;
            this.moleculeViewer1.GridLineColor = System.Drawing.Color.LightBlue;
            this.moleculeViewer1.GridLineDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.moleculeViewer1.GridLineWidth = 1;
            this.moleculeViewer1.GridSize = 50D;
            this.moleculeViewer1.Location = new System.Drawing.Point(367, 110);
            this.moleculeViewer1.MarginColor = System.Drawing.Color.Green;
            this.moleculeViewer1.MarginLineDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.moleculeViewer1.MarginLineWidth = 1;
            this.moleculeViewer1.MaximumSize = new System.Drawing.Size(1100, 850);
            this.moleculeViewer1.MinimumGridSize = 1D;
            this.moleculeViewer1.Name = "moleculeViewer1";
            this.moleculeViewer1.NonPrintingAreaColor = System.Drawing.Color.Gray;
            this.moleculeViewer1.PrintGrid = false;
            this.moleculeViewer1.PrintMargins = false;
            this.moleculeViewer1.SelectedObject = null;
            this.moleculeViewer1.SelectedObjects = new SustainableChemistry.GraphicObject[0];
            this.moleculeViewer1.ShowGrid = false;
            this.moleculeViewer1.ShowMargins = false;
            this.moleculeViewer1.Size = new System.Drawing.Size(572, 468);
            this.moleculeViewer1.SurfaceBounds = new System.Drawing.Rectangle(0, 0, 1100, 850);
            this.moleculeViewer1.SurfaceMargins = new System.Drawing.Rectangle(100, 100, 900, 650);
            this.moleculeViewer1.TabIndex = 6;
            this.moleculeViewer1.Zoom = 1D;
            this.moleculeViewer1.SelectionChanged += new SustainableChemistry.MoleculeViewer.SelectionChangedHandler(this.moleculeViewer1_SelectionChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid1.Location = new System.Drawing.Point(946, 110);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(301, 468);
            this.propertyGrid1.TabIndex = 7;
            // 
            // FunctionalGroupEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1259, 590);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.moleculeViewer1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "FunctionalGroupEditor";
            this.Text = "FunctionalGroupEditor";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private MoleculeViewer moleculeViewer1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}