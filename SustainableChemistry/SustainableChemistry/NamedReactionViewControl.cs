using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SustainableChemistry
{
    public partial class NamedReactionViewControl : UserControl
    {
        public NamedReactionViewControl()
        {
            InitializeComponent();
            this.label1.Text = string.Empty;
            this.label2.Text = string.Empty;
            this.label3.Text = string.Empty;
            this.label4.Text = "+";
            this.label5.Text = "=>";
            this.label6.Text = string.Empty;
            this.label8.Text = string.Empty;
            //this.label5.Text = string.Empty;
            this.Text = string.Empty;
        }

        public string ReactionName
        {
            get
            {
                return this.label6.Text;
            }
            set
            {
                this.label6.Text = value;
            }
        }

        public Image Reactant1
        {
            set
            {
                this.pictureBox1.Image = value;
            }
        }

        public string Reactant1Name
        {
            set
            {
                this.label1.Text = value;
            }
        }

        public Image Reactant2
        {
            set
            {
                this.pictureBox2.Image = value;
            }
        }

        public string Reactant2Name
        {
            set
            {
                this.label2.Text = value;
            }
        }

        public Image Product
        {
            set
            {
                this.pictureBox3.Image = value;
            }
        }

        public string FunctionalGroupName
        {
            set
            {
                this.label3.Text = value;
            }
        }

        public string Catalyst
        {
            set
            {
                this.label8.Text = value;
            }
        }
    }
}
