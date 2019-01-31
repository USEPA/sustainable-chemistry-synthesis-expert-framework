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
    public partial class FunctionalGroupEditor : Form
    {
        System.Data.DataTable fGroups;
        System.Collections.IEnumerator fGroupEnum;
        ChemInfo.Molecule molecule;
        string m_ImagePath = string.Empty;


        public FunctionalGroupEditor(System.Data.DataTable groups, string imagePath)
        {
            InitializeComponent();
            m_ImagePath = imagePath;
            fGroups = groups;
            foreach(System.Data.DataRow row in fGroups.Rows)
            {
                this.comboBox1.Items.Add(row["Name".ToString()]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        void PopulateForm(System.Data.DataRow group)
        {
            if (group == null)
            {
                this.pictureBox1.Image = null;
                //this.textBox1.Text = string.Empty;
                this.textBox2.Text = string.Empty;
                this.moleculeViewer1 = null;
                molecule = null;
                return;
            }
            //this.pictureBox1.Image = group.Image;
            //this.textBox1.Text = group["Name"].ToString();
            this.textBox2.Text = group["Smart"].ToString();
            if (!String.IsNullOrEmpty(this.textBox2.Text))
            {
                molecule = new ChemInfo.Molecule(this.textBox2.Text);
                // molecule.FindRings();
                //molecule.FindAllPaths();
                this.moleculeViewer1.Molecule = molecule;
            }
        }

        private void moleculeViewer1_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            this.propertyGrid1.SelectedObject = null;
            if (args.SelectedObject != null) this.propertyGrid1.SelectedObject = ((GraphicObject)(args.SelectedObject)).Tag;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Data.DataRow row = null;
            foreach (System.Data.DataRow dr in fGroups.Rows)
            {
                if (string.Equals(dr["Name"].ToString(), this.comboBox1.Text))
                {
                    row = dr;
                    break;
                }
            }
            this.textBox2.Text = row["Smarts"].ToString();
            molecule = new ChemInfo.Molecule(this.textBox2.Text);
            this.moleculeViewer1.Molecule = molecule;
            string filename = m_ImagePath + row["Image"].ToString().Replace("/", "\\"); ;
            if (System.IO.File.Exists(filename))
            {
                this.pictureBox1.Image = System.Drawing.Image.FromFile(filename);
            }
        }
    }
}
