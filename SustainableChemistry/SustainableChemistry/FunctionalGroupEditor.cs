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
        ChemInfo.FunctionalGroupCollection fGroupColl;
        IEnumerator<ChemInfo.FunctionalGroup> fGroupEnum;
        ChemInfo.Molecule molecule;


        public FunctionalGroupEditor(ChemInfo.FunctionalGroupCollection groups)
        {
            InitializeComponent();
            fGroupColl = groups;
            fGroupEnum = groups.GetEnumerator();
            fGroupEnum.MoveNext();
            //while (fGroupEnum.Current.Name != "TRIAZOLE") fGroupEnum.MoveNext();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fGroupEnum.MoveNext()) this.PopulateForm(fGroupEnum.Current);
            else
            {
                fGroupEnum.Reset();
                this.PopulateForm(null);
            }
        }

        void PopulateForm(ChemInfo.FunctionalGroup group)
        {
            if (group == null)
            {
                this.pictureBox1.Image = null;
                this.textBox1.Text = string.Empty;
                this.textBox2.Text = string.Empty;
                this.moleculeViewer1 = null;
                molecule = null;
                return;
            }
            this.pictureBox1.Image = group.Image;
            this.textBox1.Text = group.Name;
            this.textBox2.Text = group.Smart;
            if (!String.IsNullOrEmpty(group.Smart))
            {
                molecule = new ChemInfo.Molecule(group.Smart);
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
    }
}
