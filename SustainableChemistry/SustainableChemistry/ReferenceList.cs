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
    public partial class ReferenceList : Form
    {
        public ReferenceList()
        {
            InitializeComponent();
        }

        public ChemInfo.References References
        {
            set
            {
                this.dataGridView1.DataSource = value;
            }
        }
    }
}
