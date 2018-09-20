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
    public partial class ReactionEditor : Form
    {

        ChemInfo.FunctionalGroupCollection m_FunctionalGroups;
        ChemInfo.FunctionalGroup m_FunctionalGroup;
        ChemInfo.Molecule molecule;

        public ReactionEditor(ChemInfo.FunctionalGroupCollection fGroups)
        {
            InitializeComponent();
            molecule = null;
            this.m_FunctionalGroups = fGroups;
            this.productComboBox.Items.AddRange(m_FunctionalGroups.FunctionalGroups);
            reactantAComboBox.Items.AddRange(ChemInfo.Reactants.ReactantList);
            reactantBComboBox.Items.AddRange(ChemInfo.Reactants.ReactantList);
            this.linkLabel1.Text = string.Empty;
        }

        private void ReactionEditor_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void ReactionNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChemInfo.NamedReaction r = m_FunctionalGroup.NamedReactions[ReactionNameComboBox.SelectedItem.ToString()];
            reactantAComboBox.SelectedItem = r.ReactantA.ToUpper();
            reactantBComboBox.SelectedItem = r.ReactantB.ToUpper();
            this.Solvent = (ChemInfo.SOLVENT)Enum.Parse(typeof(ChemInfo.SOLVENT), r.Solvent);
            this.AcidBase = r.GetAcidBase();
            this.HeatButton.Checked = false;
            if (r.Catalyst.ToLower().Contains("heat")) this.HeatButton.Checked = true;
            this.SmartsLabel.Text = "SMARTS: " + m_FunctionalGroup.Smart;
            this.linkLabel1.Text = r.URL;
        }

        private void productComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReactionNameComboBox.Items.Clear();
            m_FunctionalGroup = m_FunctionalGroups[productComboBox.SelectedItem.ToString()];
            foreach (ChemInfo.NamedReaction r in m_FunctionalGroup.NamedReactions)
            {
                this.ReactionNameComboBox.Items.Add(r.Name);
            }
            reactantAComboBox.Text = string.Empty;
            reactantBComboBox.Text = string.Empty;
            pictureBox1.Image = m_FunctionalGroup.Image;
            ReactionNameComboBox.SelectedIndex = 0;
        }

        public ChemInfo.FunctionalGroup SelectedFunctionalGroup
        {
            get
            {
                return this.m_FunctionalGroup;
            }
        }

        public ChemInfo.NamedReaction SelectedNamedReaction
        {
            get
            {
                if (this.m_FunctionalGroup == null) return null;
                return m_FunctionalGroup.NamedReactions[this.ReactionNameComboBox.Text];
            }
        }

        public bool Heat
        {
            get
            {
                return this.HeatButton.Checked;
            }
            set
            {
                this.HeatButton.Checked = value;
            }
        }

        public ChemInfo.SOLVENT Solvent
        {
            get
            {
                if (this.acetoneButton.Checked == true) return ChemInfo.SOLVENT.ACETONE;
                if (this.AcetonitrileButton.Checked == true) return ChemInfo.SOLVENT.ACETONITRILE;
                if (this.AmmoniaButton.Checked == true) return ChemInfo.SOLVENT.AQUEOUS_AMMONIA;
                if (this.BenzoicAcidButton.Checked == true) return ChemInfo.SOLVENT.BENZOIC_ACID_TOLUENE;
                if (this.DichloromethaneButton.Checked == true) return ChemInfo.SOLVENT.DCM;
                if (this.DimethylCarbonateButton.Checked == true) return ChemInfo.SOLVENT.DMC;
                if (this.DimethylformamideButton.Checked == true) return ChemInfo.SOLVENT.DMF;
                if (this.DimethylSulfoxideSolventButton.Checked == true) return ChemInfo.SOLVENT.DMSO;
                if (this.EthanolSolventButton.Checked == true) return ChemInfo.SOLVENT.ETHANOL;
                if (this.HaloKetoneSolventButton.Checked == true) return ChemInfo.SOLVENT.HALO_KETONE;
                if (this.MethanolSolventButton.Checked == true) return ChemInfo.SOLVENT.METHANOL;
                if (this.TriethylamineSolventButton.Checked == true) return ChemInfo.SOLVENT.METHANOL_TRIETHYLAMINE;
                if (this.NitreneSolventButton.Checked == true) return ChemInfo.SOLVENT.NITRENE;
                if (this.NitritesSolventButton.Checked == true) return ChemInfo.SOLVENT.NITRITES;
                if (this.TetrahydrofuranSolventButton.Checked == true) return ChemInfo.SOLVENT.THF;
                if (this.TolueneSolventButton.Checked == true) return ChemInfo.SOLVENT.TOLUENE;
                if (this.WaterSolventButton.Checked == true) return ChemInfo.SOLVENT.WATER;
                return ChemInfo.SOLVENT.NONE;
            }
            set
            {
                this.NoSolventButton.Checked = true;
                if (value == ChemInfo.SOLVENT.ACETONE) this.acetoneButton.Checked = true;
                if (value == ChemInfo.SOLVENT.ACETONITRILE) this.AcetonitrileButton.Checked = true;
                if (value == ChemInfo.SOLVENT.AQUEOUS_AMMONIA) this.AmmoniaButton.Checked = true;
                if (value == ChemInfo.SOLVENT.BENZOIC_ACID_TOLUENE) this.BenzoicAcidButton.Checked = true;
                if (value == ChemInfo.SOLVENT.DCM) this.DichloromethaneButton.Checked = true;
                if (value == ChemInfo.SOLVENT.DMC) this.DimethylCarbonateButton.Checked = true;
                if (value == ChemInfo.SOLVENT.DMF) this.DimethylformamideButton.Checked = true;
                if (value == ChemInfo.SOLVENT.DMSO) this.DimethylSulfoxideSolventButton.Checked = true;
                if (value == ChemInfo.SOLVENT.ETHANOL) this.EthanolSolventButton.Checked = true;
                if (value == ChemInfo.SOLVENT.HALO_KETONE) this.HaloKetoneSolventButton.Checked = true;
                if (value == ChemInfo.SOLVENT.METHANOL) this.MethanolSolventButton.Checked = true;
                if (value == ChemInfo.SOLVENT.METHANOL_TRIETHYLAMINE) this.TriethylamineSolventButton.Checked = true;
                if (value == ChemInfo.SOLVENT.NITRENE) this.NitreneSolventButton.Checked = true;
                if (value == ChemInfo.SOLVENT.NITRITES) this.NitritesSolventButton.Checked = true;
                if (value == ChemInfo.SOLVENT.THF) this.TetrahydrofuranSolventButton.Checked = true;
                if (value == ChemInfo.SOLVENT.TOLUENE) this.TolueneSolventButton.Checked = true;
                if (value == ChemInfo.SOLVENT.WATER) this.WaterSolventButton.Checked = true;
            }
        }
        public string Catalyst { get; set; }
        public ChemInfo.AcidBase AcidBase
        {
            get
            {
                if (AcidButton.Checked) return ChemInfo.AcidBase.ACID;
                if (BasicButton.Checked) return ChemInfo.AcidBase.BASE;
                if (AcidBaseButton.Checked) return ChemInfo.AcidBase.ACID_BASE;
                return ChemInfo.AcidBase.NONE;
            }
            set
            {
                this.NotAcidBaseButton.Checked = true;
                if (value == ChemInfo.AcidBase.ACID) AcidButton.Checked = true;
                if (value == ChemInfo.AcidBase.ACID_BASE) AcidBaseButton.Checked = true;
                if (value == ChemInfo.AcidBase.BASE) BasicButton.Checked = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(this.linkLabel1.Text);
        }


        //ChemInfo.AcidBase CheckAcidBase(string acidBase)
        //{
        //    if (acidBase.ToLower() == "acid") return ChemInfo.AcidBase.ACID;
        //    if (acidBase.ToLower() == "base") return ChemInfo.AcidBase.BASE;
        //    if (acidBase.ToLower() == "base/heat") return ChemInfo.AcidBase.BASE;
        //    if (acidBase.ToLower() == "base/acid") return ChemInfo.AcidBase.ACID_BASE;
        //    return ChemInfo.AcidBase.NONE;
        //}

        //void SetAcidBase(string acidBase)
        //{
        //    if (acidBase.ToLower() == "acid") this.AcidBase = ChemInfo.AcidBase.ACID;
        //    if (acidBase.ToLower() == "base") this.AcidBase = ChemInfo.AcidBase.BASE;
        //    if (acidBase.ToLower() == "base/heat") this.AcidBase = ChemInfo.AcidBase.BASE;
        //    if (acidBase.ToLower() == "base/acid") this.AcidBase = ChemInfo.AcidBase.ACID_BASE;
        //    if (acidBase.ToLower() == string.Empty) this.AcidBase = ChemInfo.AcidBase.NONE;
        //}

        //void SetSolvent(String solvent)
        //{
        //    if (solvent.ToLower() == "aceton") this.acetoneButton.Checked = true;
        //    if (solvent.ToLower() == "acetonitrile ") this.AcetonitrileButton.Checked = true;
        //    if (solvent.ToLower() == "aqueous ammonia/ the treated with lead nitrate") this.AmmoniaButton.Checked = true;
        //    if (solvent.ToLower() == "benzoic acid /toluene") this.BenzoicAcidButton.Checked = true;
        //    if (solvent.ToLower() == "dcm") this.DichloromethaneButton.Checked = true;
        //    if (solvent.ToLower() == "dmc") this.DimethylCarbonateButton.Checked = true;
        //    if (solvent.ToLower() == "dmf") this.DimethylformamideButton.Checked = true;
        //    if (solvent.ToLower() == "dmso") this.DimethylSulfoxideSolventButton.Checked = true;
        //    if (solvent.ToLower() == "ethanol") this.EthanolSolventButton.Checked = true;
        //    if (solvent.ToLower() == "halo ketones") this.HaloKetoneSolventButton.Checked = true;
        //    if (solvent.ToLower() == "methanol") this.MethanolSolventButton.Checked = true;
        //    if (solvent.ToLower() == "methanol/ triethylamine") this.TriethylamineSolventButton.Checked = true;
        //    if (solvent.ToLower() == "nitrene") this.NitreneSolventButton.Checked = true;
        //    if (solvent.ToLower() == "nitrites") this.NitritesSolventButton.Checked = true;
        //    if (solvent.ToLower() == "thf") this.TetrahydrofuranSolventButton.Checked = true;
        //    if (solvent.ToLower() == "toluene") this.TolueneSolventButton.Checked = true;
        //    if (solvent.ToLower() == "water") this.WaterSolventButton.Checked = true;
        //}
    }
}
