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

    public partial class Form1 : Form
    {

        ChemInfo.Molecule molecule;
        System.Data.DataTable app_namedreaction;
        System.Data.DataTable app_functionalgroup;
        System.Data.DataTable app_reference;
        System.Data.DataTable app_catalyst;
        System.Data.DataTable app_namedreaction_ByProd;
        System.Data.DataTable app_namedreaction_Reactants;
        System.Data.DataTable app_reactant;
        System.Data.DataTable app_solvent;
        System.Data.DataTable app_compound;
        static Encoding enc8 = Encoding.UTF8;
        string documentPath;
        List<Reference> currentReferences;
        System.Data.SQLite.SQLiteConnection m_dbConnection;
        System.Data.SQLite.SQLiteDataAdapter db_Adapter;
        System.Data.SQLite.SQLiteCommand db_Command;
        string dataPath;
        string imagePath;

        public Form1()
        {
            InitializeComponent();
            dataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) + "\\SustainableChemistry\\Data";
            if (!System.IO.Directory.Exists(dataPath)) dataPath = "..\\..\\..\\..\\SustainableChemistryData\\SustainableChemistryData";
            imagePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) + "\\SustainableChemistry\\Data";
            if (!System.IO.Directory.Exists(imagePath)) imagePath = "..\\..\\..\\..\\SustainableChemistryData\\SustainableChemistryData\\static\\media\\";
            m_dbConnection = new System.Data.SQLite.SQLiteConnection("Data Source=" + dataPath + "\\SustainableChemistry.sqlite3;Version=3;");

            app_catalyst = GetDataTable("app_catalyst");
            app_compound = GetDataTable("app_compound");
            app_functionalgroup = GetDataTable("app_functionalgroup");
            app_namedreaction = GetDataTable("app_namedreaction");
            app_reference = GetDataTable("app_reference");
            app_namedreaction_ByProd = GetDataTable("app_namedreaction_ByProducts");
            app_namedreaction_ByProd.Rows.Clear();
            app_namedreaction_Reactants = GetDataTable("app_namedreaction_Reactants");
            app_namedreaction_Reactants.Rows.Clear();
            app_reactant = GetDataTable("app_reactant");
            app_solvent = GetDataTable("app_solvent");
            //app_compound = GetDataTable("app_compound");
            molecule = new ChemInfo.Molecule();
            this.trackBar1.Value = (int)(this.moleculeViewer1.Zoom * 100);
            documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\USEPA\\SustainableChemistry";
            currentReferences = new List<Reference>();
        }

        private void FillDataTable(System.Data.DataTable table)
        {
            string text = System.IO.File.ReadAllText(table.TableName + ".csv");
            text = text.Replace("\r", string.Empty);
            string[] lines = text.Split('\n');
            string[] fieldNames = lines[0].Split('\t');
            System.Type[] types = new Type[fieldNames.Length];
            for (int i = 1; i < fieldNames.Length; i++)
            {
                types[i] = table.Columns[fieldNames[i]].DataType;
            }
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split('\t');
                System.Data.DataRow row = table.NewRow();
                if (fields.Length > 1)
                {
                    for (int j = 0; j < fieldNames.Length; j++)
                    {
                        if (types[j] == typeof(Int64))
                        {
                            row[fieldNames[j]] = Convert.ToInt64(fields[j]);
                        }
                        else row[fieldNames[j]] = fields[j];
                    }
                    table.Rows.Add(row);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveDataTable(app_functionalgroup);
            this.SaveDataTable(app_namedreaction);
            this.SaveDataTable(app_catalyst);
            this.SaveDataTable(app_solvent);
            this.SaveDataTable(app_reference);
            this.SaveDataTable(app_reactant);
            this.SaveDataTable(app_namedreaction_ByProd);
            this.SaveDataTable(app_namedreaction_Reactants);
            this.SaveDataTable(app_compound);
        }

        //    void ImageFileReconciliation()
        //    {
        //        // This next line creates a list of strings that don't have images. Can be commented out!
        //        List<string> missingImages = new List<string>();

        //        // Creates the collection of functional groups.
        //        foreach (ChemInfo.FunctionalGroup temp in fGroups)
        //        {
        //            string filename = imagePath + "FunctionalGroups\\" + temp.Name.ToLower() + ".jpg";
        //            if (System.IO.File.Exists(filename)) temp.Image = System.Drawing.Image.FromFile(filename);

        //            //this line adds the missing image to the list of missing images. Can be commented out.
        //            else missingImages.Add(temp.Name);
        //        }
        //        // Writes the missing images to a file.

        //        // Write the string array to a new file named "WriteLines.txt".
        //        using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(documentPath + @"\MissingImages.txt"))
        //        {
        //            foreach (string line in missingImages)
        //                outputFile.WriteLine(line);
        //        }

        //        string[] imageFiles = System.IO.Directory.GetFiles(imagePath + "FunctionalGroups\\");
        //        string[] groupNames = fGroups.FunctionalGroups;
        //        List<string> extraImages = new List<string>();
        //        foreach (string name in imageFiles)
        //        {
        //            string temp = name.Replace(imagePath + "FunctionalGroups\\", string.Empty);
        //            temp = temp.Replace(".jpg", string.Empty);
        //            bool add = true;
        //            foreach (string gName in groupNames)
        //            {
        //                if (temp.ToUpper() == gName.ToUpper()) add = false;
        //            }
        //            if (add) extraImages.Add(temp);
        //        }

        //        // Write the string array to a new file named "WriteLines.txt".
        //        using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(imagePath + "FunctionalGroups\\" + @"\ExtraImages.txt"))
        //        {
        //            foreach (string line in extraImages)
        //                outputFile.WriteLine(line);
        //        }
        //    }
        //}

        // Code from https://stackoverflow.com/questions/20419630/saving-datatable-to-sqlite-database-by-adapter-update
        public System.Data.DataTable GetDataTable(string tablename)
        {
            System.Data.DataTable DT = new System.Data.DataTable();
            m_dbConnection.Open();
            db_Command = m_dbConnection.CreateCommand();
            db_Command.CommandText = string.Format("SELECT * FROM {0}", tablename);
            db_Adapter = new System.Data.SQLite.SQLiteDataAdapter(db_Command);
            db_Adapter.Fill(DT);
            m_dbConnection.Close();
            DT.TableName = tablename;
            return DT;
        }

        public void SaveDataTable(System.Data.DataTable DT)
        {
            try
            {
                m_dbConnection.Open();
                db_Command = m_dbConnection.CreateCommand();
                db_Command.CommandText = string.Format("SELECT * FROM {0}", DT.TableName);
                db_Adapter = new System.Data.SQLite.SQLiteDataAdapter(db_Command);
                System.Data.SQLite.SQLiteCommandBuilder builder = new System.Data.SQLite.SQLiteCommandBuilder(db_Adapter);
                db_Adapter.Update(DT);
                m_dbConnection.Close();
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show(Ex.Message);
            }
        }

        private void importFormTESTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestResults test = null;
            try
            {
                test = new TestResults();
            }
            catch (Exception)
            {
                return;
            }
            test.ShowDialog();
            string filepath = test.FilePath;
            if (string.IsNullOrEmpty(filepath)) return;
            molecule = ChemInfo.MoleFileReader.ReadMoleFile(test.FilePath);
            this.dataGridView1.Rows.Clear();
            this.moleculeViewer1.Molecule = molecule;
            this.propertyGrid1.SelectedObject = this.molecule;
            foreach (System.Data.DataRow dr in app_functionalgroup.Rows)
            {
                string smarts = dr["Smarts"].ToString();
                if (!string.IsNullOrEmpty(smarts)) this.molecule.FindFunctionalGroup(dr);
            }

            this.textBox1.Text = Newtonsoft.Json.JsonConvert.SerializeObject(this.molecule, Newtonsoft.Json.Formatting.Indented);
            foreach (string group in this.molecule.FunctionalGroups) this.functionalGroupComboBox.Items.Add(group);
        }

        private void enterSMILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            smilesInput smiles = new smilesInput();
            smiles.ShowDialog();
            if (string.IsNullOrEmpty(smiles.SMILES)) return;
            this.molecule = new ChemInfo.Molecule(smiles.SMILES);
            this.dataGridView1.Rows.Clear();
            if (molecule == null) return;
            this.moleculeViewer1.Molecule = this.molecule;
            this.propertyGrid1.SelectedObject = this.molecule;
            foreach (System.Data.DataRow dr in app_functionalgroup.Rows)
            {
                string smarts = dr["Smarts"].ToString();
                if (!string.IsNullOrEmpty(smarts)) this.molecule.FindFunctionalGroup(dr);
            }

            this.textBox1.Text = Newtonsoft.Json.JsonConvert.SerializeObject(this.molecule, Newtonsoft.Json.Formatting.Indented);
            foreach (string group in this.molecule.FunctionalGroups) this.functionalGroupComboBox.Items.Add(group);
        }

        private void moleculeViewer1_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            this.propertyGrid1.SelectedObject = null;
            if (args.SelectedObject != null) this.propertyGrid1.SelectedObject = ((GraphicObject)(args.SelectedObject)).Tag;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            this.moleculeViewer1.Zoom = (double)(this.trackBar1.Value) / 100.0;
        }

        private void findSMARTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.findSmarts();
            foreach (System.Data.DataRow dr in app_functionalgroup.Rows)
            {
                string smarts = dr["Smarts"].ToString();
                if (!string.IsNullOrEmpty(smarts)) this.molecule.FindFunctionalGroup(dr);
            }
            this.textBox1.Text = Newtonsoft.Json.JsonConvert.SerializeObject(molecule, Newtonsoft.Json.Formatting.Indented);
        }

        private void showReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> references = new List<string>();
            if (!molecule.FunctionalGroupsFound) this.findSMARTSToolStripMenuItem_Click(sender, e);
            string[] funcs = molecule.FunctionalGroups;
            //foreach (ChemInfo.FunctionalGroup f in funcs)
            //{
            //    var refs = m_References.GetReferences(f.Name);
            //    foreach (ChemInfo.Reference r in refs) references.Add(r.ToString());
            //}
            //ReferenceList form = new ReferenceList
            //{
            //    References = m_References
            //};
            //form.ShowDialog();
        }

        private void editReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void importRISFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewReference form = new AddNewReference(app_functionalgroup, app_namedreaction, app_reference);
            if (form.ShowDialog() == DialogResult.OK)
            {
                System.Data.DataRow row = app_reference.NewRow();
                row["Functional_Group_id"] = form.FunctionalGroupId;
                row["Reaction_id"] = form.ReactionNameId;
                row["RISData"] = form.Data;
                row["Name"] = form.ReactionName;
                app_reference.Rows.Add(row);
            }
        }

        private void exportJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //List<Reference> references = new List<Reference>();
            //if (!molecule.FunctionalGroupsFound) this.findSMARTSToolStripMenuItem_Click(sender, e);
            //string[] funcs = molecule.FunctionalGroups;
            //foreach (string f in funcs)
            //{
            //    var fGroup = from myRow in this.app_functionalgroup.AsEnumerable()
            //                 where myRow.Field<string>("Name") == f
            //                 select myRow;
            //    List<Int64> groups = new List<Int64>();
            //    foreach (DataRow dr in fGroup)
            //    {
            //        groups.Add(Convert.ToInt64(dr["id"]));
            //    }
            //    //int value = groups[0];
            //    var results = from myRow in this.app_namedreaction.AsEnumerable()
            //                  where myRow.Field<Int64>("Functional_Group_id") == groups[0]
            //                  select myRow;
            //    foreach (DataRow row in results)
            //    {
            //        this.namedReactionComboBox.Items.Add(row["Name"].ToString());
            //    }
            //}
            Results results = new Results(molecule.Smiles, app_functionalgroup, app_namedreaction, app_reactant, app_namedreaction_Reactants, app_catalyst, app_solvent, app_namedreaction_ByProd, app_reference);
            this.textBox1.Text = Newtonsoft.Json.JsonConvert.SerializeObject(molecule, Newtonsoft.Json.Formatting.Indented);

            //  System.Diagnostics.Process.Start(documentPath + "\\output.json");
        }

        private string[] GetFunctionalGroups(string smiles)
        {
            List<string> retVal = new List<string>();
            ChemInfo.Molecule m = new ChemInfo.Molecule(smiles);
            foreach (System.Data.DataRow dr in app_functionalgroup.Rows)
            {
                string smarts = dr["Smarts"].ToString();
                if (this.molecule.FindFunctionalGroup(dr)) retVal.Add(dr["Name"].ToString());
            }
            return retVal.ToArray<string>();
        }

        private Int64 GetFunctionalGroupId(string fGroup)
        {
            var f = from myRow in this.app_functionalgroup.AsEnumerable()
                    where myRow.Field<string>("Name") == fGroup
                    select myRow;
            List<Int64> groups = new List<Int64>();
            foreach (DataRow dr in f)
            {
                groups.Add(Convert.ToInt64(dr["id"]));
            }
            return groups[0];
        }

        private string GetFunctionalGroupName(Int64 fGroup)
        {
            var f = from myRow in this.app_functionalgroup.AsEnumerable()
                    where myRow.Field<Int64>("id") == fGroup
                    select myRow;
            List<string> groups = new List<string>();
            foreach (DataRow dr in f)
            {
                groups.Add(dr["Name"].ToString());
            }
            return groups[0];
        }

        private string[] GetNamedReactions(string fGroup)
        {
            List<string> retVal = new List<string>();
            var results = from myRow in this.app_namedreaction.AsEnumerable()
                          where myRow.Field<Int64>("Functional_Group_id") == this.GetFunctionalGroupId(fGroup)
                          select myRow;
            foreach (DataRow row in results)
            {
                retVal.Add(row["Name"].ToString());
            }
            return retVal.ToArray<string>();
        }

        private Int64[] GetNamedReactionsIds(string fGroup)
        {
            List<Int64> retVal = new List<Int64>();
            var results = from myRow in this.app_namedreaction.AsEnumerable()
                          where myRow.Field<Int64>("Functional_Group_id") == this.GetFunctionalGroupId(fGroup)
                          select myRow;
            foreach (DataRow row in results)
            {
                retVal.Add((Int64)row["id"]);
            }
            return retVal.ToArray<Int64>();
        }

        private Int64[] GetNamedReactionsIds(Int64 fGroup)
        {
            List<Int64> retVal = new List<Int64>();
            var results = from myRow in this.app_namedreaction.AsEnumerable()
                          where myRow.Field<Int64>("Functional_Group_id") == fGroup
                          select myRow;
            foreach (DataRow row in results)
            {
                retVal.Add((Int64)row["id"]);
            }
            return retVal.ToArray<Int64>();
        }

        private string[] GetNamedReactions(Int64 fGroup)
        {
            List<string> retVal = new List<string>();
            var results = from myRow in this.app_namedreaction.AsEnumerable()
                          where myRow.Field<Int64>("Functional_Group_id") == fGroup
                          select myRow;
            foreach (DataRow row in results)
            {
                retVal.Add(row["Name"].ToString());
            }
            return retVal.ToArray<string>();
        }

        private string GetNamedReaction(Int64 fGroup, string Name)
        {
            string retVal = String.Empty;
            var results = from myRow in this.app_namedreaction.AsEnumerable()
                          where myRow.Field<Int64>("Functional_Group_id") == fGroup
                          && myRow.Field<string>("Name") == Name
                          select myRow;
            foreach (DataRow row in results)
            {
                retVal = row["Name"].ToString();
            }
            return retVal;
        }
        private Int64 GetNamedReactionId(Int64 fGroup, string Name)
        {
            Int64 retVal = -1;
            var results = from myRow in this.app_namedreaction.AsEnumerable()
                          where myRow.Field<Int64>("Functional_Group_id") == fGroup
                          && myRow.Field<string>("Name") == Name
                          select myRow;
            foreach (DataRow row in results)
            {
                retVal = Convert.ToInt64(row["id"]);
            }
            return retVal;
        }

        private void functionalGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FunctionalGroupViewer viewer = new FunctionalGroupViewer(this.app_functionalgroup);
            viewer.ShowDialog();
        }

        private void reactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ReactionEditor editor = new ReactionEditor(fGroups);
            //System.Windows.Forms.DialogResult result = editor.ShowDialog();
            //ChemInfo.NamedReaction rxn = editor.SelectedNamedReaction;
            //if (rxn != null)
            //{
            //    rxn.Solvent = editor.Solvent.ToString();
            //    rxn.SetAcidBase(editor.AcidBase);
            //    rxn.Catalyst = editor.Catalyst;
            //}
        }

        private void functionalGroupsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FunctionalGroupEditor editor = new FunctionalGroupEditor(app_functionalgroup, imagePath);
            editor.ShowDialog();
        }

        private void testChemicalListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //List<DSSToxChemicals> chemicals = new List<DSSToxChemicals>();
            //// Open the document for editing.
            //string fileName = documentPath + "\\DSSTox_ToxCastRelease_20151019.xlsx";
            //using (DocumentFormat.OpenXml.Packaging.SpreadsheetDocument document = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(fileName, false))
            //{
            //    DocumentFormat.OpenXml.Packaging.WorkbookPart wbPart = document.WorkbookPart;
            //    DocumentFormat.OpenXml.Packaging.WorksheetPart wsPart = wbPart.WorksheetParts.First();
            //    DocumentFormat.OpenXml.Spreadsheet.SheetData sheetData = wsPart.Worksheet.Elements<DocumentFormat.OpenXml.Spreadsheet.SheetData>().First();

            //    string text = string.Empty;
            //    bool first = true;
            //    foreach (DocumentFormat.OpenXml.Spreadsheet.Row r in sheetData.Elements<DocumentFormat.OpenXml.Spreadsheet.Row>())
            //    {
            //        if (!first)
            //        {
            //            foreach (DocumentFormat.OpenXml.Spreadsheet.Cell c in r.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
            //            {
            //                text = text + this.GetExcelCellValue(c, wbPart) + '\t';
            //            }
            //            chemicals.Add(new DSSToxChemicals(text));
            //        }
            //        first = false;
            //        text = string.Empty;
            //    }
            //    document.Close();
            //}
            //foreach (DSSToxChemicals chem in chemicals)
            //{
            //    if (!string.IsNullOrEmpty(chem.Structure_SMILES))
            //    {
            //        ChemInfo.Molecule mol = new ChemInfo.Molecule(chem.Structure_SMILES);
            //        if (mol != null)
            //        {
            //            foreach (ChemInfo.FunctionalGroup f in this.fGroups)
            //            {
            //                if ((f.Name != "ESTER-SULFIDE") || (f.Name != "KETENIMINE")) mol.FindFunctionalGroup(f);
            //            }
            //        }
            //        chem.AddFunctionalGroups(mol.FunctionalGroups);
            //    }
            //}
            //fileName = documentPath + "\\chemicals.json";
            //System.IO.File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(chemicals, Newtonsoft.Json.Formatting.Indented));
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabPage4.Tag != null) return;
            if (((System.Windows.Forms.TabControl)sender).SelectedIndex == 3)
            {
                string fileName = dataPath + "\\chemicals.json";
                //Reads in functional groups from JSON file. This should be used after Excel file is completed.
                var json = new System.Web.Script.Serialization.JavaScriptSerializer
                {
                    MaxJsonLength = 20000000
                };
                List<DSSToxChemicals> chemicals = null;
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName))
                {
                    chemicals = (List<DSSToxChemicals>)json.Deserialize(sr.ReadToEnd(), typeof(List<DSSToxChemicals>));
                }

                List<DSSToxChemicals>.Enumerator enumerator = chemicals.GetEnumerator();
                enumerator.MoveNext();
                pictureBox2.Image = PUGGetCompoundImage(enumerator.Current.Structure_SMILES, enumerator.Current.Substance_CASRN);
                checkedListBox1.Items.Add("Other");
                if (enumerator.Current.FunctionalGroups != null) checkedListBox1.Items.AddRange(enumerator.Current.FunctionalGroups);
                tabPage4.Tag = enumerator;
            }
            else
            {
                Results results = new Results(molecule.Smiles, app_functionalgroup, app_namedreaction, app_reactant, app_namedreaction_Reactants, app_catalyst, app_solvent, app_namedreaction_ByProd, app_reference);
                this.textBox1.Text = Newtonsoft.Json.JsonConvert.SerializeObject(results, Newtonsoft.Json.Formatting.Indented);
            }
        }

        public static Image PUGGetCompoundImage(string smiles, string casNo)
        {
            string imageReference = "http://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/name/" + casNo + "/PNG";
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(imageReference);
            try
            {
                System.Net.WebResponse response = request.GetResponse();
                return Image.FromStream(response.GetResponseStream());
            }
            catch (System.Exception p_Ex)
            {
                return null;// Properties.Resources.Image1;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            List<DSSToxChemicals>.Enumerator enumerator = (List<DSSToxChemicals>.Enumerator)tabPage4.Tag;
            enumerator.MoveNext();
            this.dataGridView1.Rows.Clear();
            if (string.IsNullOrEmpty(enumerator.Current.Structure_SMILES)) return;
            molecule = new ChemInfo.Molecule(enumerator.Current.Structure_SMILES);
            if (molecule == null) return;
            // molecule.FindAllPaths();
            this.moleculeViewer1.Molecule = molecule;
            this.propertyGrid1.SelectedObject = molecule;
            this.textBox1.Text = Newtonsoft.Json.JsonConvert.SerializeObject(this.molecule, Newtonsoft.Json.Formatting.Indented);
            pictureBox2.Image = PUGGetCompoundImage(enumerator.Current.Structure_SMILES, enumerator.Current.Substance_CASRN);
            checkedListBox1.Items.Add("Other");
            if (molecule.Aromatic) checkedListBox1.Items.Add("AROMATIC");
            if (molecule.Heterocyclic) checkedListBox1.Items.Add("HETEROCYCLIC");
            if (molecule.HeterocyclicAromatic) checkedListBox1.Items.Add("HETEROCYCLICAROMATIC");
            checkedListBox1.Items.AddRange(enumerator.Current.FunctionalGroups);
            tabPage4.Tag = enumerator;
        }

        private void functionalGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.namedReactionComboBox.Items.Clear();
            this.namedReactionComboBox.Items.AddRange(this.GetNamedReactions(this.functionalGroupComboBox.SelectedItem.ToString()));
        }
        // O=P(OC)(OC)C

        private void namedReactionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
            currentReferences.Clear();
            Int64 fGroup = this.GetFunctionalGroupId(this.functionalGroupComboBox.SelectedItem.ToString());
            Int64 rxn = this.GetNamedReactionId(fGroup, this.namedReactionComboBox.SelectedItem.ToString());
            var results = from myRow in this.app_reference.AsEnumerable()
                          where myRow.Field<Int64>("Functional_Group_id") == fGroup
                          && myRow.Field<Int64>("Reaction_id") == rxn
                          select myRow;
            foreach (DataRow row in results)
            {
                Reference reference = new Reference((Int64)row["Functional_Group_id"], (Int64)row["Reaction_id"], row["RISData"].ToString());
                this.dataGridView1.Rows.Add(reference.ToString());
                currentReferences.Add(reference);
            }
            results = from myRow in this.app_namedreaction.AsEnumerable()
                          where myRow.Field<Int64>("Functional_Group_id") == fGroup
                          && myRow.Field<Int64>("id") == rxn
                          select myRow;
            string imageName = string.Empty;
            foreach(System.Data.DataRow dr in results)
            {
                imageName = dr["Image"].ToString();
            }
            string filename = System.IO.Path.GetFullPath(imagePath + imageName.Replace("/", "\\"));
            if (System.IO.File.Exists(filename))
            {
                this.pictureBox1.Image = System.Drawing.Image.FromFile(filename);
            }
            this.dataGridView1.CurrentCell = null;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            String testStr = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            Reference reference = null;
            foreach (Reference r in this.currentReferences)
            {
                if (r.ToString() == testStr) reference = r;
            }
            if (reference != null)
            {
                if (!string.IsNullOrEmpty(reference.URL))
                    System.Diagnostics.Process.Start(reference.URL);
                else if (!string.IsNullOrEmpty(reference.doi))
                    System.Diagnostics.Process.Start("https://doi.org/" + reference.doi);
            }
            this.dataGridView1.CurrentCell = null;
        }
    }
}
