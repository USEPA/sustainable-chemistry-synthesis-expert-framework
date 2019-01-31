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
    public partial class smilesInput : Form
    {
        string m_Smile;

        public smilesInput()
        {
            InitializeComponent();
            m_Smile = string.Empty;
        }

        public string SMILES
        {
            get
            {
                return m_Smile;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBox1.Text))
            {
                m_Smile = this.textBox1.Text.Trim();
                this.Close();
            }
        }        
    }
}
