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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionalGroupEditor));
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.moleculeViewer1 = new SustainableChemistry.MoleculeViewer();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 135);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(386, 348);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(549, 27);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 3;
            this.button1.Text = "Next";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 78);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "SMART";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(105, 73);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(379, 22);
            this.textBox2.TabIndex = 5;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid1.Location = new System.Drawing.Point(1211, 135);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(4);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(451, 576);
            this.propertyGrid1.TabIndex = 7;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(105, 29);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(379, 24);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // moleculeViewer1
            // 
            this.moleculeViewer1.AutoScroll = true;
            this.moleculeViewer1.AutoScrollMinSize = new System.Drawing.Size(2280, 1761);
            this.moleculeViewer1.AutoSize = true;
            this.moleculeViewer1.BackColor = System.Drawing.SystemColors.Window;
            this.moleculeViewer1.GridLineColor = System.Drawing.Color.LightBlue;
            this.moleculeViewer1.GridLineDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.moleculeViewer1.GridLineWidth = 1;
            this.moleculeViewer1.GridSize = 50D;
            this.moleculeViewer1.Location = new System.Drawing.Point(408, 135);
            this.moleculeViewer1.Margin = new System.Windows.Forms.Padding(5);
            this.moleculeViewer1.MarginColor = System.Drawing.Color.Green;
            this.moleculeViewer1.MarginLineDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.moleculeViewer1.MarginLineWidth = 1;
            this.moleculeViewer1.MaximumSize = new System.Drawing.Size(1467, 1046);
            this.moleculeViewer1.MinimumGridSize = 1D;
            this.moleculeViewer1.Name = "moleculeViewer1";
            this.moleculeViewer1.NonPrintingAreaColor = System.Drawing.Color.Gray;
            this.moleculeViewer1.PrintGrid = false;
            this.moleculeViewer1.PrintMargins = false;
            this.moleculeViewer1.SelectedObject = null;
            this.moleculeViewer1.SelectedObjects = new SustainableChemistry.GraphicObject[0];
            this.moleculeViewer1.ShowGrid = false;
            this.moleculeViewer1.ShowMargins = false;
            this.moleculeViewer1.Size = new System.Drawing.Size(763, 576);
            this.moleculeViewer1.SurfaceBounds = new System.Drawing.Rectangle(0, 0, 1100, 850);
            this.moleculeViewer1.SurfaceMargins = new System.Drawing.Rectangle(100, 100, 900, 650);
            this.moleculeViewer1.TabIndex = 6;
            this.moleculeViewer1.Zoom = 1.7272727272727273D;
            this.moleculeViewer1.SelectionChanged += new SustainableChemistry.MoleculeViewer.SelectionChangedHandler(this.moleculeViewer1_SelectionChanged);
            // 
            // FunctionalGroupEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1679, 726);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.moleculeViewer1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FunctionalGroupEditor";
            this.Text = "FunctionalGroupEditor";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private MoleculeViewer moleculeViewer1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}