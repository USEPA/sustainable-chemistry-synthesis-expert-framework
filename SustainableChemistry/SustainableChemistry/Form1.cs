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
        System.Data.DataTable NamedReactions;
        System.Data.DataTable FunctionalGroups;
        System.Data.DataTable References;
        static Encoding enc8 = Encoding.UTF8;
        string documentPath;
        ChemInfo.FunctionalGroupCollection fGroups;
        ChemInfo.NamedReactionCollection reactions;
        ChemInfo.References m_References;
        List<ChemInfo.Reference> currentReferences;
        System.Data.SQLite.SQLiteConnection m_dbConnection;
        System.Data.SQLite.SQLiteDataAdapter db_Adapter;
        System.Data.SQLite.SQLiteCommand db_Command;        
        string dataPath;
        string imagePath;

        public Form1()
        {
            InitializeComponent();
            dataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) + "\\SustainableChemistry\\Data\\";
            if (!System.IO.Directory.Exists(dataPath)) dataPath = "..\\..\\..\\..\\Data\\";

            imagePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) + "\\SustainableChemistry\\Images\\";
            if (!System.IO.Directory.Exists("Images")) imagePath = "..\\..\\..\\..\\Images\\";
            
            m_dbConnection = new System.Data.SQLite.SQLiteConnection("Data Source=" + dataPath + "SustainableChemistry.sqlite;Version=3;");

            FunctionalGroups = GetDataTable("FunctionalGroups");
            fGroups = new ChemInfo.FunctionalGroupCollection(FunctionalGroups);
            NamedReactions = GetDataTable("NamedReactions");
            reactions = new ChemInfo.NamedReactionCollection(NamedReactions);
            References = GetDataTable("ReferenceList");
            m_References = new ChemInfo.References(References);
            molecule = new ChemInfo.Molecule();
            this.trackBar1.Value = (int)(this.moleculeViewer1.Zoom * 100);
            documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\USEPA\\SustainableChemistry";

            currentReferences = new List<ChemInfo.Reference>();
            
            //string[] dir = System.IO.Directory.GetDirectories(dataPath + "PHOSPHONATE ESTER");
            //List<ChemInfo.Reference> refs = new List<ChemInfo.Reference>();
            //foreach (string d in dir)
            //{
            //    string react = d.Remove(0, dataPath.Length + "PHOSPHONATE ESTER".Length + 1);
            //    string[] files = System.IO.Directory.GetFiles(d, "*.ris");
            //    foreach (string f in files)
            //    {
            //        System.IO.StreamReader r = new System.IO.StreamReader(f);
            //        string refData = r.ReadToEnd();
            //        refs.Add(new ChemInfo.Reference("PHOSPHONATE ESTER", react, refData));
            //        DataRow row = References.NewRow();
            //        row["FunctionalGroup"] = "PHOSPHONATE ESTER";
            //        row["ReactionName"] = react;
            //        row["RISData"] = refData;
            //        References.Rows.Add(row);
            //    }
            //    //files = System.IO.Directory.GetFiles(d, "*.jpg");
            //    //System.Drawing.Image image = System.Drawing.Image.FromFile(files[0]);
            //    //image.Save(imagePath + "Reactions\\" + "PHOSPHONATE ESTER_" + react + ".jpg");
            //}
            //this.OpenFunctionGroupExcelResource();
        }

        private void OpenFunctionGroupExcelResource()
        {
            FunctionalGroups.Clear();
            NamedReactions.Clear();
            // Reads functional Groups from Excel file.
            string fileName = dataPath + "Full Functional Group List 20180731.xlsx";
            //FunctionalGroups.Columns.Add("Name", typeof(System.String));
            //FunctionalGroups.Columns.Add("Smarts", typeof(System.String));
            //FunctionalGroups.Columns.Add("Image", typeof(System.String));
            using (DocumentFormat.OpenXml.Packaging.SpreadsheetDocument document = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(fileName, false))
            {
                DocumentFormat.OpenXml.Packaging.WorkbookPart wbPart = document.WorkbookPart;
                DocumentFormat.OpenXml.Spreadsheet.SheetData sheetData = GetWorkSheetFromSheet(wbPart, GetSheetFromName(wbPart, "Full Functional Group List")).Elements<DocumentFormat.OpenXml.Spreadsheet.SheetData>().First();

                string text = string.Empty;
                bool first = true;
                foreach (DocumentFormat.OpenXml.Spreadsheet.Row r in sheetData.Elements<DocumentFormat.OpenXml.Spreadsheet.Row>())
                {
                    if (!first)
                    {
                        foreach (DocumentFormat.OpenXml.Spreadsheet.Cell c in r.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
                        {
                            text = text + this.GetExcelCellValue(c, wbPart) + '\t';
                        }
                        System.Data.DataRow row = FunctionalGroups.NewRow();
                        ChemInfo.FunctionalGroup temp = fGroups.Add(text, row);
                        string filename = imagePath + "FunctionalGroups\\" + temp.Name.ToLower() + ".jpg";
                        FunctionalGroups.Rows.Add(row);
                    }
                    text = string.Empty;
                    first = false;
                }

                sheetData = GetWorkSheetFromSheet(wbPart, GetSheetFromName(wbPart, "Reaction List")).Elements<DocumentFormat.OpenXml.Spreadsheet.SheetData>().First();
                text = string.Empty;
                first = true;
                //NamedReactions.Columns.Add("Name", typeof(System.String));
                //NamedReactions.Columns.Add("FunctionalGroup", typeof(System.String));
                //NamedReactions.Columns.Add("Image", typeof(System.String));
                //NamedReactions.Columns.Add("URL", typeof(System.String));
                //NamedReactions.Columns.Add("ReactantA", typeof(System.String));
                //NamedReactions.Columns.Add("ReactantB", typeof(System.String));
                //NamedReactions.Columns.Add("ReactantC", typeof(System.String));
                //NamedReactions.Columns.Add("Product", typeof(System.String));
                //NamedReactions.Columns.Add("Heat", typeof(System.String));
                //NamedReactions.Columns.Add("AcidBase", typeof(System.String));
                //NamedReactions.Columns.Add("Catalyst", typeof(System.String));
                //NamedReactions.Columns.Add("Solvent", typeof(System.String));
                //NamedReactions.Columns.Add("ByProducts", typeof(System.String));

                foreach (DocumentFormat.OpenXml.Spreadsheet.Row r in sheetData.Elements<DocumentFormat.OpenXml.Spreadsheet.Row>())
                {
                    if (!first)
                    {
                        foreach (DocumentFormat.OpenXml.Spreadsheet.Cell c in r.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
                        {
                            text = text + this.GetExcelCellValue(c, wbPart) + '\t';
                        }
                        System.Data.DataRow row = NamedReactions.NewRow();

                        reactions.Add(new ChemInfo.NamedReaction(text, row));
                        NamedReactions.Rows.Add(row);

                    }
                    text = string.Empty;
                    first = false;
                }
                document.Close();
                this.SaveDataTable(FunctionalGroups);
                this.SaveDataTable(NamedReactions);
            }


            void ImageFileReconciliation()
            {
                // This next line creates a list of strings that don't have images. Can be commented out!
                List<string> missingImages = new List<string>();

                // Creates the collection of functional groups.
                foreach (ChemInfo.FunctionalGroup temp in fGroups)
                {
                    string filename = imagePath + "FunctionalGroups\\" + temp.Name.ToLower() + ".jpg";
                    if (System.IO.File.Exists(filename)) temp.Image = System.Drawing.Image.FromFile(filename);

                    //this line adds the missing image to the list of missing images. Can be commented out.
                    else missingImages.Add(temp.Name);
                }
                // Writes the missing images to a file.

                // Write the string array to a new file named "WriteLines.txt".
                using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(documentPath + @"\MissingImages.txt"))
                {
                    foreach (string line in missingImages)
                        outputFile.WriteLine(line);
                }

                string[] imageFiles = System.IO.Directory.GetFiles(imagePath + "FunctionalGroups\\");
                string[] groupNames = fGroups.FunctionalGroups;
                List<string> extraImages = new List<string>();
                foreach (string name in imageFiles)
                {
                    string temp = name.Replace(imagePath + "FunctionalGroups\\", string.Empty);
                    temp = temp.Replace(".jpg", string.Empty);
                    bool add = true;
                    foreach (string gName in groupNames)
                    {
                        if (temp.ToUpper() == gName.ToUpper()) add = false;
                    }
                    if (add) extraImages.Add(temp);
                }

                // Write the string array to a new file named "WriteLines.txt".
                using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(imagePath + "FunctionalGroups\\" + @"\ExtraImages.txt"))
                {
                    foreach (string line in extraImages)
                        outputFile.WriteLine(line);
                }
            }
        }


        public System.Data.DataTable GetDataTable(string tablename)
        {
            System.Data.DataTable DT = new System.Data.DataTable();
            m_dbConnection.Open();
            db_Command = m_dbConnection.CreateCommand();
            db_Command.CommandText = string.Format("SELECT * FROM {0}", tablename);
            db_Adapter = new System.Data.SQLite.SQLiteDataAdapter(db_Command);
            db_Adapter.AcceptChangesDuringFill = false;
            db_Adapter.Fill(DT);
            m_dbConnection.Close();
            DT.TableName = tablename;
            return DT;
        }

        public void SaveDataTable(System.Data.DataTable DT)
        {
            try
            {
                Execute(string.Format("DELETE FROM {0}", DT.TableName));
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

        public int Execute(string sql_statement)
        {
            m_dbConnection.Open();
            db_Command = m_dbConnection.CreateCommand();
            db_Command.CommandText = sql_statement;
            int row_updated;
            try
            {
                row_updated = db_Command.ExecuteNonQuery();
            }
            catch
            {
                m_dbConnection.Close();
                return 0;
            }
            m_dbConnection.Close();
            return row_updated;
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
            this.listBox1.Items.Clear();
            this.moleculeViewer1.Molecule = molecule;
            this.propertyGrid1.SelectedObject = this.molecule;
            foreach (ChemInfo.FunctionalGroup f in this.fGroups)
            {
                if ((f.Name != "ESTER-SULFIDE") || (f.Name != "KETENIMINE")) this.molecule.FindFunctionalGroup(f);
            }

            this.textBox1.Text = Newtonsoft.Json.JsonConvert.SerializeObject(this.molecule, Newtonsoft.Json.Formatting.Indented);
            foreach (ChemInfo.FunctionalGroup group in this.molecule.FunctionalGroups) this.functionalGroupComboBox.Items.Add(group.Name);
        }

        private void enterSMILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            smilesInput smiles = new smilesInput();
            smiles.ShowDialog();
            if (string.IsNullOrEmpty(smiles.SMILES)) return;
            this.molecule = new ChemInfo.Molecule(smiles.SMILES);
            this.listBox1.Items.Clear();
            if (molecule == null) return;
            this.moleculeViewer1.Molecule = this.molecule;
            this.propertyGrid1.SelectedObject = this.molecule;
            foreach (ChemInfo.FunctionalGroup f in this.fGroups)
            {
                if ((f.Name != "ESTER-SULFIDE") || (f.Name != "KETENIMINE")) this.molecule.FindFunctionalGroup(f);
            }

            this.textBox1.Text = Newtonsoft.Json.JsonConvert.SerializeObject(this.molecule, Newtonsoft.Json.Formatting.Indented);
            foreach (ChemInfo.FunctionalGroup group in this.molecule.FunctionalGroups) this.functionalGroupComboBox.Items.Add(group.Name);
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
            foreach (ChemInfo.FunctionalGroup f in this.fGroups)
            {
                this.molecule.FindFunctionalGroup(f);
            }
            this.textBox1.Text = Newtonsoft.Json.JsonConvert.SerializeObject(molecule, Newtonsoft.Json.Formatting.Indented);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.SaveDataTable(FunctionalGroups);
            //this.SaveDataTable(NamedReactions);
            //this.SaveDataTable(References);
        }

        private void showReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> references = new List<string>();
            if (!molecule.FunctionalGroupsFound) this.findSMARTSToolStripMenuItem_Click(sender, e);
            ChemInfo.FunctionalGroup[] funcs = molecule.FunctionalGroups;
            foreach (ChemInfo.FunctionalGroup f in funcs)
            {
                var refs = m_References.GetReferences(f.Name);
                foreach (ChemInfo.Reference r in refs) references.Add(r.ToString());
            }
            ReferenceList form = new ReferenceList
            {
                References = m_References
            };
            form.ShowDialog();
        }

        private void editReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void importRISFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("THIS NEEDS FIXED", "THIS NEEDS FIXED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            AddNewReference form = new AddNewReference(fGroups);
            if (form.ShowDialog() == DialogResult.OK)
            {
                m_References.Add(new ChemInfo.Reference(form.FunctionalGroup, form.ReactionName, form.Data));
                System.Data.DataRow row = References.NewRow();
                row["FunctionalGroup"] = form.FunctionalGroup;
                row["ReactionName"] = form.ReactionName;
                row["RISData"] = form.Data;
                References.Rows.Add(row);
            }
        }

        private void exportJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ChemInfo.Reference> references = new List<ChemInfo.Reference>();
            if (!molecule.FunctionalGroupsFound) this.findSMARTSToolStripMenuItem_Click(sender, e);
            ChemInfo.FunctionalGroup[] funcs = molecule.FunctionalGroups;
            foreach (ChemInfo.FunctionalGroup f in funcs)
            {
                var refs = m_References.GetReferences(f.Name);
                foreach (ChemInfo.Reference r in refs) references.Add(r);
            }
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(documentPath + "\\output.json");
            writer.Write(serializer.Serialize(references));
            writer.Close();
            //  System.Diagnostics.Process.Start(documentPath + "\\output.json");
        }

        private void functionalGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FunctionalGroupViewer viewer = new FunctionalGroupViewer(this.fGroups);
            viewer.ShowDialog();
        }

        private void reactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReactionEditor editor = new ReactionEditor(fGroups);
            System.Windows.Forms.DialogResult result = editor.ShowDialog();
            ChemInfo.NamedReaction rxn = editor.SelectedNamedReaction;
            if (rxn != null)
            {
                rxn.Solvent = editor.Solvent.ToString();
                rxn.SetAcidBase(editor.AcidBase);
                rxn.Catalyst = editor.Catalyst;
            }
        }

        private void functionalGroupsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FunctionalGroupEditor editor = new FunctionalGroupEditor(fGroups);
            editor.ShowDialog();
        }

        private void moleculeViewer1_Load(object sender, EventArgs e)
        {
        }

        private void testChemicalListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DSSToxChemicals> chemicals = new List<DSSToxChemicals>();
            // Open the document for editing.
            string fileName = documentPath + "\\DSSTox_ToxCastRelease_20151019.xlsx";
            using (DocumentFormat.OpenXml.Packaging.SpreadsheetDocument document = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(fileName, false))
            {
                DocumentFormat.OpenXml.Packaging.WorkbookPart wbPart = document.WorkbookPart;
                DocumentFormat.OpenXml.Packaging.WorksheetPart wsPart = wbPart.WorksheetParts.First();
                DocumentFormat.OpenXml.Spreadsheet.SheetData sheetData = wsPart.Worksheet.Elements<DocumentFormat.OpenXml.Spreadsheet.SheetData>().First();

                string text = string.Empty;
                bool first = true;
                foreach (DocumentFormat.OpenXml.Spreadsheet.Row r in sheetData.Elements<DocumentFormat.OpenXml.Spreadsheet.Row>())
                {
                    if (!first)
                    {
                        foreach (DocumentFormat.OpenXml.Spreadsheet.Cell c in r.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
                        {
                            text = text + this.GetExcelCellValue(c, wbPart) + '\t';
                        }
                        chemicals.Add(new DSSToxChemicals(text));
                    }
                    first = false;
                    text = string.Empty;
                }
                document.Close();
            }
            foreach (DSSToxChemicals chem in chemicals)
            {
                if (!string.IsNullOrEmpty(chem.Structure_SMILES))
                {
                    ChemInfo.Molecule mol = new ChemInfo.Molecule(chem.Structure_SMILES);
                    if (mol != null)
                    {
                        foreach (ChemInfo.FunctionalGroup f in this.fGroups)
                        {
                            if ((f.Name != "ESTER-SULFIDE") || (f.Name != "KETENIMINE")) mol.FindFunctionalGroup(f);
                        }
                    }
                    chem.AddFunctionalGroups(mol.FunctionalGroups);
                }
            }
            fileName = documentPath + "\\chemicals.json";
            System.IO.File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(chemicals, Newtonsoft.Json.Formatting.Indented));
        }

        private string GetExcelCellValue(DocumentFormat.OpenXml.Spreadsheet.Cell cell, DocumentFormat.OpenXml.Packaging.WorkbookPart wbpart)
        {
            string retVal = string.Empty;

            if (cell.DataType != null)
            {
                if (cell.DataType == DocumentFormat.OpenXml.Spreadsheet.CellValues.SharedString)
                {
                    int id = -1;
                    if (Int32.TryParse(cell.InnerText, out id))
                    {
                        DocumentFormat.OpenXml.Spreadsheet.SharedStringItem item = GetSharedStringItemById(wbpart, id);

                        if (item.Text != null)
                        {
                            retVal = item.Text.Text;
                        }
                        else if (item.InnerText != null)
                        {
                            retVal = item.InnerText;
                        }
                        else if (item.InnerXml != null)
                        {
                            retVal = item.InnerXml;
                        }
                    }
                }
                else retVal = cell.InnerText;
            }
            return retVal;
        }

        public static DocumentFormat.OpenXml.Spreadsheet.SharedStringItem GetSharedStringItemById(DocumentFormat.OpenXml.Packaging.WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<DocumentFormat.OpenXml.Spreadsheet.SharedStringItem>().ElementAt(id);
        }

        public static DocumentFormat.OpenXml.Spreadsheet.Sheet GetSheetFromName(DocumentFormat.OpenXml.Packaging.WorkbookPart workbookPart, string sheetName)
        {
            return workbookPart.Workbook.Sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>()
                .FirstOrDefault(s => s.Name.HasValue && s.Name.Value == sheetName);
        }

        public static DocumentFormat.OpenXml.Spreadsheet.Worksheet GetWorkSheetFromSheet(DocumentFormat.OpenXml.Packaging.WorkbookPart workbookPart, DocumentFormat.OpenXml.Spreadsheet.Sheet sheet)
        {
            var worksheetPart = (DocumentFormat.OpenXml.Packaging.WorksheetPart)workbookPart.GetPartById(sheet.Id);
            return worksheetPart.Worksheet;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabPage4.Tag != null) return;
            if (((System.Windows.Forms.TabControl)sender).SelectedIndex == 3)
            {
                string fileName = documentPath + "\\chemicals.json";
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
            else this.findSMARTSToolStripMenuItem_Click(sender, e);
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
            this.listBox1.Items.Clear();
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
            var results = from myRow in this.NamedReactions.AsEnumerable()
                          where myRow.Field<string>("FunctionalGroup") == this.functionalGroupComboBox.SelectedItem.ToString()
                          select myRow;
            foreach (DataRow row in results)
            {
                this.namedReactionComboBox.Items.Add(row["Name"].ToString());
            }
        }
        // O=P(OC)(OC)C

        private void namedReactionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            currentReferences.Clear();
            var results = from myRow in this.References.AsEnumerable()
                          where myRow.Field<string>("FunctionalGroup") == this.functionalGroupComboBox.SelectedItem.ToString()
                          && myRow.Field<string>("ReactionName") == this.namedReactionComboBox.SelectedItem.ToString()
                          select myRow;
            foreach (DataRow row in results)
            {
                ChemInfo.Reference reference = new ChemInfo.Reference(row["FunctionalGroup"].ToString(), row["ReactionName"].ToString(), row["RISData"].ToString());
                this.listBox1.Items.Add(reference.ToString());
                currentReferences.Add(reference);
            }
            string filename = imagePath + "Reactions\\" + this.functionalGroupComboBox.SelectedItem.ToString() + "_" + this.namedReactionComboBox.SelectedItem.ToString() + ".jpg";
            if (System.IO.File.Exists(filename))
            {
                this.pictureBox1.Image = System.Drawing.Image.FromFile(filename);
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            String testStr = this.listBox1.SelectedItem.ToString();
            ChemInfo.Reference reference = null;
            foreach (ChemInfo.Reference r in this.currentReferences)
            {
                if (r.ToString() == testStr) reference = r;
            }
            if (reference != null) {
                if (!string.IsNullOrEmpty(reference.URL))
                    System.Diagnostics.Process.Start(reference.URL);
                else if (!string.IsNullOrEmpty(reference.doi))
                    System.Diagnostics.Process.Start("https://doi.org/" + reference.doi);
            }
        }
    }
}
