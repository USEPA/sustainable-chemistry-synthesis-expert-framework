using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SustainableChemistry
{
    public partial class AddNewReference : Form
    {
        public AddNewReference()
        {
            InitializeComponent();
            comboBox1.Items.Add(string.Empty);
            comboBox1.Items.AddRange(ChemInfo.Functionalities.AvailablePhosphateFunctionalGroups);
            label4.Text = string.Empty;
            label5.Text = string.Empty;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.Text = string.Empty;
            FunctionalGroup = comboBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("THIS NEEDS FIXED", "THIS NEEDS FIXED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            System.IO.Stream myStream = null;
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "\\Downloads";
            openFileDialog.Filter = "RIS Files (*.ris)|*.ris|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            System.IO.StreamReader reader = new System.IO.StreamReader(myStream);
                            Data = reader.ReadToEnd();
                            // ChemInfo.Reference reference = new ChemInfo.Reference(string.Empty, string.Empty, Data);
                            //textBox1.Text = reference.ToString();
                            label5.Text = string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        public string Data { get; set; }
        public string ReactionName { get; set; }
        public string FunctionalGroup { get; set; }

        private void AddReferenceButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Data) && ChemInfo.Functionalities.AvailablePhosphateFunctionalGroups.Contains(comboBox1.Text))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            if (string.IsNullOrEmpty(Data)) label4.Text = "Please select a Functional Group";
            if (!ChemInfo.Functionalities.AvailablePhosphateFunctionalGroups.Contains(comboBox1.Text)) label5.Text = "Please select a reference file.";

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ReactionName = comboBox1.Text;
        }
    }
}
