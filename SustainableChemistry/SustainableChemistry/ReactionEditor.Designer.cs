namespace SustainableChemistry
{
    partial class ReactionEditor
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
            this.productComboBox = new System.Windows.Forms.ComboBox();
            this.reactantBComboBox = new System.Windows.Forms.ComboBox();
            this.reactantAComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SolventBox = new System.Windows.Forms.GroupBox();
            this.NoSolventButton = new System.Windows.Forms.RadioButton();
            this.BenzeneSolventButton = new System.Windows.Forms.RadioButton();
            this.WaterSolventButton = new System.Windows.Forms.RadioButton();
            this.TolueneSolventButton = new System.Windows.Forms.RadioButton();
            this.TriethylamineSolventButton = new System.Windows.Forms.RadioButton();
            this.TetrahydrofuranSolventButton = new System.Windows.Forms.RadioButton();
            this.NitritesSolventButton = new System.Windows.Forms.RadioButton();
            this.NitreneSolventButton = new System.Windows.Forms.RadioButton();
            this.MethanolSolventButton = new System.Windows.Forms.RadioButton();
            this.HaloKetoneSolventButton = new System.Windows.Forms.RadioButton();
            this.EthanolSolventButton = new System.Windows.Forms.RadioButton();
            this.DimethylSulfoxideSolventButton = new System.Windows.Forms.RadioButton();
            this.DimethylformamideButton = new System.Windows.Forms.RadioButton();
            this.DimethylCarbonateButton = new System.Windows.Forms.RadioButton();
            this.DichloromethaneButton = new System.Windows.Forms.RadioButton();
            this.BenzoicAcidButton = new System.Windows.Forms.RadioButton();
            this.AmmoniaButton = new System.Windows.Forms.RadioButton();
            this.AcetonitrileButton = new System.Windows.Forms.RadioButton();
            this.acetoneButton = new System.Windows.Forms.RadioButton();
            this.AcidBaseBox = new System.Windows.Forms.GroupBox();
            this.NotAcidBaseButton = new System.Windows.Forms.RadioButton();
            this.AcidBaseButton = new System.Windows.Forms.RadioButton();
            this.BasicButton = new System.Windows.Forms.RadioButton();
            this.AcidButton = new System.Windows.Forms.RadioButton();
            this.HeatButton = new System.Windows.Forms.RadioButton();
            this.DecompositionButton = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.ReactionNameComboBox = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.SmartsLabel = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SolventBox.SuspendLayout();
            this.AcidBaseBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // productComboBox
            // 
            this.productComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.productComboBox.FormattingEnabled = true;
            this.productComboBox.Location = new System.Drawing.Point(25, 29);
            this.productComboBox.Name = "productComboBox";
            this.productComboBox.Size = new System.Drawing.Size(258, 21);
            this.productComboBox.TabIndex = 0;
            this.productComboBox.SelectedIndexChanged += new System.EventHandler(this.productComboBox_SelectedIndexChanged);
            // 
            // reactantBComboBox
            // 
            this.reactantBComboBox.FormattingEnabled = true;
            this.reactantBComboBox.Location = new System.Drawing.Point(303, 152);
            this.reactantBComboBox.Name = "reactantBComboBox";
            this.reactantBComboBox.Size = new System.Drawing.Size(197, 21);
            this.reactantBComboBox.TabIndex = 1;
            // 
            // reactantAComboBox
            // 
            this.reactantAComboBox.FormattingEnabled = true;
            this.reactantAComboBox.Location = new System.Drawing.Point(36, 152);
            this.reactantAComboBox.Name = "reactantAComboBox";
            this.reactantAComboBox.Size = new System.Drawing.Size(226, 21);
            this.reactantAComboBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Product Functional Group";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(300, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Reactant B";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Reactant A";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(379, 374);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Solvent";
            // 
            // SolventBox
            // 
            this.SolventBox.Controls.Add(this.NoSolventButton);
            this.SolventBox.Controls.Add(this.BenzeneSolventButton);
            this.SolventBox.Controls.Add(this.WaterSolventButton);
            this.SolventBox.Controls.Add(this.TolueneSolventButton);
            this.SolventBox.Controls.Add(this.TriethylamineSolventButton);
            this.SolventBox.Controls.Add(this.TetrahydrofuranSolventButton);
            this.SolventBox.Controls.Add(this.NitritesSolventButton);
            this.SolventBox.Controls.Add(this.NitreneSolventButton);
            this.SolventBox.Controls.Add(this.MethanolSolventButton);
            this.SolventBox.Controls.Add(this.HaloKetoneSolventButton);
            this.SolventBox.Controls.Add(this.EthanolSolventButton);
            this.SolventBox.Controls.Add(this.DimethylSulfoxideSolventButton);
            this.SolventBox.Controls.Add(this.DimethylformamideButton);
            this.SolventBox.Controls.Add(this.DimethylCarbonateButton);
            this.SolventBox.Controls.Add(this.DichloromethaneButton);
            this.SolventBox.Controls.Add(this.BenzoicAcidButton);
            this.SolventBox.Controls.Add(this.AmmoniaButton);
            this.SolventBox.Controls.Add(this.AcetonitrileButton);
            this.SolventBox.Controls.Add(this.acetoneButton);
            this.SolventBox.Location = new System.Drawing.Point(26, 202);
            this.SolventBox.Name = "SolventBox";
            this.SolventBox.Size = new System.Drawing.Size(314, 277);
            this.SolventBox.TabIndex = 9;
            this.SolventBox.TabStop = false;
            this.SolventBox.Text = "Solvent";
            // 
            // NoSolventButton
            // 
            this.NoSolventButton.AutoSize = true;
            this.NoSolventButton.Location = new System.Drawing.Point(73, 254);
            this.NoSolventButton.Name = "NoSolventButton";
            this.NoSolventButton.Size = new System.Drawing.Size(78, 17);
            this.NoSolventButton.TabIndex = 18;
            this.NoSolventButton.Text = "No Solvent";
            this.NoSolventButton.UseVisualStyleBackColor = true;
            // 
            // BenzeneSolventButton
            // 
            this.BenzeneSolventButton.AutoSize = true;
            this.BenzeneSolventButton.Location = new System.Drawing.Point(10, 88);
            this.BenzeneSolventButton.Name = "BenzeneSolventButton";
            this.BenzeneSolventButton.Size = new System.Drawing.Size(67, 17);
            this.BenzeneSolventButton.TabIndex = 17;
            this.BenzeneSolventButton.Text = "Benzene";
            this.BenzeneSolventButton.UseVisualStyleBackColor = true;
            // 
            // WaterSolventButton
            // 
            this.WaterSolventButton.AutoSize = true;
            this.WaterSolventButton.Location = new System.Drawing.Point(177, 168);
            this.WaterSolventButton.Name = "WaterSolventButton";
            this.WaterSolventButton.Size = new System.Drawing.Size(54, 17);
            this.WaterSolventButton.TabIndex = 16;
            this.WaterSolventButton.Text = "Water";
            this.WaterSolventButton.UseVisualStyleBackColor = true;
            // 
            // TolueneSolventButton
            // 
            this.TolueneSolventButton.AutoSize = true;
            this.TolueneSolventButton.Location = new System.Drawing.Point(177, 148);
            this.TolueneSolventButton.Name = "TolueneSolventButton";
            this.TolueneSolventButton.Size = new System.Drawing.Size(64, 17);
            this.TolueneSolventButton.TabIndex = 15;
            this.TolueneSolventButton.Text = "Toluene";
            this.TolueneSolventButton.UseVisualStyleBackColor = true;
            // 
            // TriethylamineSolventButton
            // 
            this.TriethylamineSolventButton.AutoSize = true;
            this.TriethylamineSolventButton.Location = new System.Drawing.Point(177, 128);
            this.TriethylamineSolventButton.Name = "TriethylamineSolventButton";
            this.TriethylamineSolventButton.Size = new System.Drawing.Size(87, 17);
            this.TriethylamineSolventButton.TabIndex = 14;
            this.TriethylamineSolventButton.Text = "Triethylamine";
            this.TriethylamineSolventButton.UseVisualStyleBackColor = true;
            // 
            // TetrahydrofuranSolventButton
            // 
            this.TetrahydrofuranSolventButton.AutoSize = true;
            this.TetrahydrofuranSolventButton.Location = new System.Drawing.Point(177, 108);
            this.TetrahydrofuranSolventButton.Name = "TetrahydrofuranSolventButton";
            this.TetrahydrofuranSolventButton.Size = new System.Drawing.Size(130, 17);
            this.TetrahydrofuranSolventButton.TabIndex = 13;
            this.TetrahydrofuranSolventButton.Text = "Tetrahydrofuran (THF)";
            this.TetrahydrofuranSolventButton.UseVisualStyleBackColor = true;
            // 
            // NitritesSolventButton
            // 
            this.NitritesSolventButton.AutoSize = true;
            this.NitritesSolventButton.Location = new System.Drawing.Point(177, 88);
            this.NitritesSolventButton.Name = "NitritesSolventButton";
            this.NitritesSolventButton.Size = new System.Drawing.Size(57, 17);
            this.NitritesSolventButton.TabIndex = 12;
            this.NitritesSolventButton.Text = "Nitrites";
            this.NitritesSolventButton.UseVisualStyleBackColor = true;
            // 
            // NitreneSolventButton
            // 
            this.NitreneSolventButton.AutoSize = true;
            this.NitreneSolventButton.Location = new System.Drawing.Point(177, 68);
            this.NitreneSolventButton.Name = "NitreneSolventButton";
            this.NitreneSolventButton.Size = new System.Drawing.Size(59, 17);
            this.NitreneSolventButton.TabIndex = 11;
            this.NitreneSolventButton.Text = "Nitrene";
            this.NitreneSolventButton.UseVisualStyleBackColor = true;
            // 
            // MethanolSolventButton
            // 
            this.MethanolSolventButton.AutoSize = true;
            this.MethanolSolventButton.Location = new System.Drawing.Point(177, 48);
            this.MethanolSolventButton.Name = "MethanolSolventButton";
            this.MethanolSolventButton.Size = new System.Drawing.Size(69, 17);
            this.MethanolSolventButton.TabIndex = 10;
            this.MethanolSolventButton.Text = "Methanol";
            this.MethanolSolventButton.UseVisualStyleBackColor = true;
            // 
            // HaloKetoneSolventButton
            // 
            this.HaloKetoneSolventButton.AutoSize = true;
            this.HaloKetoneSolventButton.Location = new System.Drawing.Point(177, 28);
            this.HaloKetoneSolventButton.Name = "HaloKetoneSolventButton";
            this.HaloKetoneSolventButton.Size = new System.Drawing.Size(84, 17);
            this.HaloKetoneSolventButton.TabIndex = 9;
            this.HaloKetoneSolventButton.Text = "Halo Ketone";
            this.HaloKetoneSolventButton.UseVisualStyleBackColor = true;
            // 
            // EthanolSolventButton
            // 
            this.EthanolSolventButton.AutoSize = true;
            this.EthanolSolventButton.Location = new System.Drawing.Point(10, 208);
            this.EthanolSolventButton.Name = "EthanolSolventButton";
            this.EthanolSolventButton.Size = new System.Drawing.Size(61, 17);
            this.EthanolSolventButton.TabIndex = 8;
            this.EthanolSolventButton.Text = "Ethanol";
            this.EthanolSolventButton.UseVisualStyleBackColor = true;
            // 
            // DimethylSulfoxideSolventButton
            // 
            this.DimethylSulfoxideSolventButton.AutoSize = true;
            this.DimethylSulfoxideSolventButton.Location = new System.Drawing.Point(10, 188);
            this.DimethylSulfoxideSolventButton.Name = "DimethylSulfoxideSolventButton";
            this.DimethylSulfoxideSolventButton.Size = new System.Drawing.Size(152, 17);
            this.DimethylSulfoxideSolventButton.TabIndex = 7;
            this.DimethylSulfoxideSolventButton.Text = "Dimethyl Sulfoxide (DMSO)";
            this.DimethylSulfoxideSolventButton.UseVisualStyleBackColor = true;
            // 
            // DimethylformamideButton
            // 
            this.DimethylformamideButton.AutoSize = true;
            this.DimethylformamideButton.Location = new System.Drawing.Point(10, 168);
            this.DimethylformamideButton.Name = "DimethylformamideButton";
            this.DimethylformamideButton.Size = new System.Drawing.Size(145, 17);
            this.DimethylformamideButton.TabIndex = 6;
            this.DimethylformamideButton.Text = "Dimethylformamide (DMF)";
            this.DimethylformamideButton.UseVisualStyleBackColor = true;
            // 
            // DimethylCarbonateButton
            // 
            this.DimethylCarbonateButton.AutoSize = true;
            this.DimethylCarbonateButton.Location = new System.Drawing.Point(10, 148);
            this.DimethylCarbonateButton.Name = "DimethylCarbonateButton";
            this.DimethylCarbonateButton.Size = new System.Drawing.Size(150, 17);
            this.DimethylCarbonateButton.TabIndex = 5;
            this.DimethylCarbonateButton.Text = "Dimethyl Carbonate (DMC)";
            this.DimethylCarbonateButton.UseVisualStyleBackColor = true;
            // 
            // DichloromethaneButton
            // 
            this.DichloromethaneButton.AutoSize = true;
            this.DichloromethaneButton.Location = new System.Drawing.Point(10, 128);
            this.DichloromethaneButton.Name = "DichloromethaneButton";
            this.DichloromethaneButton.Size = new System.Drawing.Size(138, 17);
            this.DichloromethaneButton.TabIndex = 4;
            this.DichloromethaneButton.Text = "Dichloromethane (DMC)";
            this.DichloromethaneButton.UseVisualStyleBackColor = true;
            // 
            // BenzoicAcidButton
            // 
            this.BenzoicAcidButton.AutoSize = true;
            this.BenzoicAcidButton.Location = new System.Drawing.Point(10, 108);
            this.BenzoicAcidButton.Name = "BenzoicAcidButton";
            this.BenzoicAcidButton.Size = new System.Drawing.Size(87, 17);
            this.BenzoicAcidButton.TabIndex = 3;
            this.BenzoicAcidButton.Text = "Benzoic Acid";
            this.BenzoicAcidButton.UseVisualStyleBackColor = true;
            // 
            // AmmoniaButton
            // 
            this.AmmoniaButton.AutoSize = true;
            this.AmmoniaButton.Location = new System.Drawing.Point(10, 68);
            this.AmmoniaButton.Name = "AmmoniaButton";
            this.AmmoniaButton.Size = new System.Drawing.Size(68, 17);
            this.AmmoniaButton.TabIndex = 2;
            this.AmmoniaButton.Text = "Ammonia";
            this.AmmoniaButton.UseVisualStyleBackColor = true;
            // 
            // AcetonitrileButton
            // 
            this.AcetonitrileButton.AutoSize = true;
            this.AcetonitrileButton.Location = new System.Drawing.Point(10, 48);
            this.AcetonitrileButton.Name = "AcetonitrileButton";
            this.AcetonitrileButton.Size = new System.Drawing.Size(77, 17);
            this.AcetonitrileButton.TabIndex = 1;
            this.AcetonitrileButton.Text = "Acetonitrile";
            this.AcetonitrileButton.UseVisualStyleBackColor = true;
            // 
            // acetoneButton
            // 
            this.acetoneButton.AutoSize = true;
            this.acetoneButton.Location = new System.Drawing.Point(10, 28);
            this.acetoneButton.Name = "acetoneButton";
            this.acetoneButton.Size = new System.Drawing.Size(65, 17);
            this.acetoneButton.TabIndex = 0;
            this.acetoneButton.Text = "Acetone";
            this.toolTip1.SetToolTip(this.acetoneButton, "For more information about Acetone, Click here.");
            this.acetoneButton.UseVisualStyleBackColor = true;
            // 
            // AcidBaseBox
            // 
            this.AcidBaseBox.Controls.Add(this.NotAcidBaseButton);
            this.AcidBaseBox.Controls.Add(this.AcidBaseButton);
            this.AcidBaseBox.Controls.Add(this.BasicButton);
            this.AcidBaseBox.Controls.Add(this.AcidButton);
            this.AcidBaseBox.Location = new System.Drawing.Point(365, 202);
            this.AcidBaseBox.Name = "AcidBaseBox";
            this.AcidBaseBox.Size = new System.Drawing.Size(135, 113);
            this.AcidBaseBox.TabIndex = 10;
            this.AcidBaseBox.TabStop = false;
            this.AcidBaseBox.Text = "Acid/Base Conditions";
            // 
            // NotAcidBaseButton
            // 
            this.NotAcidBaseButton.AutoSize = true;
            this.NotAcidBaseButton.Location = new System.Drawing.Point(17, 89);
            this.NotAcidBaseButton.Name = "NotAcidBaseButton";
            this.NotAcidBaseButton.Size = new System.Drawing.Size(45, 17);
            this.NotAcidBaseButton.TabIndex = 3;
            this.NotAcidBaseButton.TabStop = true;
            this.NotAcidBaseButton.Text = "N/A";
            this.NotAcidBaseButton.UseVisualStyleBackColor = true;
            // 
            // AcidBaseButton
            // 
            this.AcidBaseButton.AutoSize = true;
            this.AcidBaseButton.Location = new System.Drawing.Point(17, 66);
            this.AcidBaseButton.Name = "AcidBaseButton";
            this.AcidBaseButton.Size = new System.Drawing.Size(75, 17);
            this.AcidBaseButton.TabIndex = 2;
            this.AcidBaseButton.TabStop = true;
            this.AcidBaseButton.Text = "Acid/Base";
            this.AcidBaseButton.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.AcidBaseButton.UseMnemonic = false;
            this.AcidBaseButton.UseVisualStyleBackColor = true;
            // 
            // BasicButton
            // 
            this.BasicButton.AutoSize = true;
            this.BasicButton.Location = new System.Drawing.Point(17, 43);
            this.BasicButton.Name = "BasicButton";
            this.BasicButton.Size = new System.Drawing.Size(51, 17);
            this.BasicButton.TabIndex = 1;
            this.BasicButton.TabStop = true;
            this.BasicButton.Text = "Basic";
            this.BasicButton.UseVisualStyleBackColor = true;
            // 
            // AcidButton
            // 
            this.AcidButton.AutoSize = true;
            this.AcidButton.Location = new System.Drawing.Point(17, 20);
            this.AcidButton.Name = "AcidButton";
            this.AcidButton.Size = new System.Drawing.Size(46, 17);
            this.AcidButton.TabIndex = 0;
            this.AcidButton.TabStop = true;
            this.AcidButton.Text = "Acid";
            this.AcidButton.UseVisualStyleBackColor = true;
            // 
            // HeatButton
            // 
            this.HeatButton.AutoSize = true;
            this.HeatButton.Location = new System.Drawing.Point(545, 298);
            this.HeatButton.Name = "HeatButton";
            this.HeatButton.Size = new System.Drawing.Size(48, 17);
            this.HeatButton.TabIndex = 11;
            this.HeatButton.TabStop = true;
            this.HeatButton.Text = "Heat";
            this.HeatButton.UseVisualStyleBackColor = true;
            // 
            // DecompositionButton
            // 
            this.DecompositionButton.AutoSize = true;
            this.DecompositionButton.Location = new System.Drawing.Point(365, 330);
            this.DecompositionButton.Name = "DecompositionButton";
            this.DecompositionButton.Size = new System.Drawing.Size(95, 17);
            this.DecompositionButton.TabIndex = 12;
            this.DecompositionButton.TabStop = true;
            this.DecompositionButton.Text = "Decomposition";
            this.DecompositionButton.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(365, 353);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(85, 17);
            this.radioButton3.TabIndex = 13;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "radioButton3";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Reaction Name:";
            // 
            // ReactionNameComboBox
            // 
            this.ReactionNameComboBox.FormattingEnabled = true;
            this.ReactionNameComboBox.Location = new System.Drawing.Point(121, 69);
            this.ReactionNameComboBox.Name = "ReactionNameComboBox";
            this.ReactionNameComboBox.Size = new System.Drawing.Size(354, 21);
            this.ReactionNameComboBox.TabIndex = 15;
            this.ReactionNameComboBox.SelectedIndexChanged += new System.EventHandler(this.ReactionNameComboBox_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(545, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(300, 300);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // SmartsLabel
            // 
            this.SmartsLabel.AutoSize = true;
            this.SmartsLabel.Location = new System.Drawing.Point(545, 341);
            this.SmartsLabel.Name = "SmartsLabel";
            this.SmartsLabel.Size = new System.Drawing.Size(58, 13);
            this.SmartsLabel.TabIndex = 17;
            this.SmartsLabel.Text = "SMARTS: ";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(36, 108);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(55, 13);
            this.linkLabel1.TabIndex = 18;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "linkLabel1";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // ReactionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 497);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.SmartsLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ReactionNameComboBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.DecompositionButton);
            this.Controls.Add(this.HeatButton);
            this.Controls.Add(this.AcidBaseBox);
            this.Controls.Add(this.SolventBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reactantAComboBox);
            this.Controls.Add(this.reactantBComboBox);
            this.Controls.Add(this.productComboBox);
            this.Name = "ReactionEditor";
            this.Text = "ReactionEditor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ReactionEditor_FormClosed);
            this.SolventBox.ResumeLayout(false);
            this.SolventBox.PerformLayout();
            this.AcidBaseBox.ResumeLayout(false);
            this.AcidBaseBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox productComboBox;
        private System.Windows.Forms.ComboBox reactantBComboBox;
        private System.Windows.Forms.ComboBox reactantAComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox SolventBox;
        private System.Windows.Forms.RadioButton BenzeneSolventButton;
        private System.Windows.Forms.RadioButton WaterSolventButton;
        private System.Windows.Forms.RadioButton TolueneSolventButton;
        private System.Windows.Forms.RadioButton TriethylamineSolventButton;
        private System.Windows.Forms.RadioButton TetrahydrofuranSolventButton;
        private System.Windows.Forms.RadioButton NitritesSolventButton;
        private System.Windows.Forms.RadioButton NitreneSolventButton;
        private System.Windows.Forms.RadioButton MethanolSolventButton;
        private System.Windows.Forms.RadioButton HaloKetoneSolventButton;
        private System.Windows.Forms.RadioButton EthanolSolventButton;
        private System.Windows.Forms.RadioButton DimethylSulfoxideSolventButton;
        private System.Windows.Forms.RadioButton DimethylformamideButton;
        private System.Windows.Forms.RadioButton DimethylCarbonateButton;
        private System.Windows.Forms.RadioButton DichloromethaneButton;
        private System.Windows.Forms.RadioButton BenzoicAcidButton;
        private System.Windows.Forms.RadioButton AmmoniaButton;
        private System.Windows.Forms.RadioButton AcetonitrileButton;
        private System.Windows.Forms.RadioButton acetoneButton;
        private System.Windows.Forms.GroupBox AcidBaseBox;
        private System.Windows.Forms.RadioButton AcidBaseButton;
        private System.Windows.Forms.RadioButton BasicButton;
        private System.Windows.Forms.RadioButton AcidButton;
        private System.Windows.Forms.RadioButton NotAcidBaseButton;
        private System.Windows.Forms.RadioButton HeatButton;
        private System.Windows.Forms.RadioButton DecompositionButton;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ReactionNameComboBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton NoSolventButton;
        private System.Windows.Forms.Label SmartsLabel;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}