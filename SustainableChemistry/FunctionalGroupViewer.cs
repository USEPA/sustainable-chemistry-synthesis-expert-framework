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
    public partial class FunctionalGroupViewer : Form
    {
        ChemInfo.FunctionalGroupCollection m_FunctGroups;

        public FunctionalGroupViewer(ChemInfo.FunctionalGroupCollection groups)
        {
            InitializeComponent();
            m_FunctGroups = groups;
            this.comboBox1.Items.AddRange(m_FunctGroups.FunctionalGroups);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tableLayoutPanel1.Controls.Clear();
            ChemInfo.FunctionalGroup g = this.m_FunctGroups[this.comboBox1.SelectedItem.ToString()];
            int i = 0;
            foreach (ChemInfo.NamedReaction r in g.NamedReactions)
            {
                NamedReactionViewControl myControl = new NamedReactionViewControl();
                myControl.ReactionName = r.Name;
                this.tableLayoutPanel1.Controls.Add(myControl, 0 /* Column Index */, i++ /* Row index */);
                List<string> temp = new List<string>();
                if (!string.IsNullOrEmpty(r.ReactantA)) temp.Add(r.ReactantA);
                if (!string.IsNullOrEmpty(r.ReactantB)) temp.Add(r.ReactantB);
                if (!string.IsNullOrEmpty(r.ReactantC)) temp.Add(r.ReactantC);
                string[] reactants = temp.ToArray<string>(); ;
                if (reactants.Length == 2)
                {
                    ChemInfo.FunctionalGroup react0 = this.m_FunctGroups[reactants[0]];
                    ChemInfo.FunctionalGroup react1 = this.m_FunctGroups[reactants[1]];
                    myControl.Reactant1 = react0.Image;
                    myControl.Reactant2 = react1.Image;
                    myControl.Reactant1Name = react0.Name;
                    myControl.Reactant2Name = react1.Name;
                }
                myControl.Product = g.Image;
                myControl.FunctionalGroupName = g.Name;
                myControl.Catalyst = r.Catalyst;
            }
        }

        private void Clear()
        {
            //this.label1.Text = string.Empty;
            //this.label2.Text = string.Empty;
            //this.label3.Text = string.Empty;
            //this.label4.Text = string.Empty;
            //this.label5.Text = string.Empty;
            //this.label6.Text = string.Empty;
            //this.pictureBox1.Image = null;
            //this.pictureBox2.Image = null;
            //this.pictureBox3.Image = null;
        }
    }
}
