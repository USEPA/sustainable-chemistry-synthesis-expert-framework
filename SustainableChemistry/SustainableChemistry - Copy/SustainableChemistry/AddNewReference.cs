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
        System.Data.DataTable m_fGroups;
        System.Data.DataTable m_Reactions;
        System.Data.DataTable m_References;

        public AddNewReference(System.Data.DataTable fGroups, System.Data.DataTable rxns, System.Data.DataTable References)
        {
            InitializeComponent();
            m_fGroups = fGroups;
            m_Reactions = rxns;
            m_References = References;
            functionalGroupComboBox.Items.Add(string.Empty);
            foreach (System.Data.DataRow row in fGroups.Rows)
            {
                functionalGroupComboBox.Items.Add(row["Name"].ToString());
            }
            label4.Text = string.Empty;
            label5.Text = string.Empty;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.Text = string.Empty;
            this.namedReactionComboBox.Items.Clear();
            this.FunctionalGroup = this.functionalGroupComboBox.SelectedItem.ToString();
            var fGroup = from myRow in this.m_fGroups.AsEnumerable()
                         where myRow.Field<string>("Name") == this.FunctionalGroup
                         select myRow;
            List<Int64> groups = new List<Int64>();
            foreach (DataRow dr in fGroup)
            {
                groups.Add(Convert.ToInt64(dr["id"]));
            }
            //int value = groups[0];
            var results = from myRow in this.m_Reactions.AsEnumerable()
                          where myRow.Field<Int64>("Functional_Group_id") == groups[0]
                          select myRow;
            foreach (DataRow row in results)
            {
                this.namedReactionComboBox.Items.Add(row["Name"].ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("THIS NEEDS FIXED", "THIS NEEDS FIXED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            System.IO.Stream myStream = null;
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "\\Downloads";
            openFileDialog.Filter = "RIS Files (*.ris)|*.ris|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            var fGroup = from myRow in this.m_fGroups.AsEnumerable()
                         where myRow.Field<string>("Name") == this.functionalGroupComboBox.SelectedItem.ToString()
                         select myRow;
            List<Int64> groups = new List<Int64>();
            foreach (DataRow dr in fGroup)
            {
                groups.Add(Convert.ToInt64(dr["id"]));
            }
            var results = from myRow in this.m_fGroups.AsEnumerable()
                         where myRow.Field<string>("Name") == this.namedReactionComboBox.SelectedItem.ToString()
                          && myRow.Field<Int64>("Functional_Group_id") == groups[0]
                          select myRow;
            List<Int64> rxn = new List<Int64>();
            foreach (DataRow dr in results)
            {
                rxn.Add(Convert.ToInt64(dr["id"]));
            }
            FunctionalGroupId = groups[0];
            ReactionNameId = rxn[0];
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
                            Reference = new Reference(groups[0], rxn[0], Data);
                            textBox1.Text = Reference.ToString();
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
        internal Int64 ReactionNameId { get; private set; }
        public string FunctionalGroup { get; set; }
        internal Int64 FunctionalGroupId { get; private set; }
        internal Reference Reference { get; private set; }

        private void AddReferenceButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Data))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            if (string.IsNullOrEmpty(Data)) label4.Text = "Please select a Functional Group";
            if (String.IsNullOrEmpty(functionalGroupComboBox.Text)) label5.Text = "Please select a reference file.";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ReactionName = namedReactionComboBox.Text;
        }
    }
}
