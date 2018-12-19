using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    [Serializable]
    [Newtonsoft.Json.JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
    public class FunctionalGroup
    {
        //NamedReactionCollection m_Reactions;

        public FunctionalGroup(string str, System.Data.DataRow row)
        {   
            string[] parts = str.Split('\t');
            Name = parts[0].Trim();
            row["Name"] = Name;
            Smart = parts[1].Trim();
            row["Smarts"] = Smart;
            //m_Reactions = new NamedReactionCollection();
            string imagePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) + "\\SustainableChemistry\\Images\\FunctionalGroups\\";
            if (!System.IO.Directory.Exists(imagePath)) imagePath = "..\\..\\..\\..\\Images\\FunctionalGroups\\";
            string fileName = imagePath + Name + ".jpg";
            if (System.IO.File.Exists(fileName))
            {
                Image = System.Drawing.Image.FromFile(fileName);
            }
        }

        public FunctionalGroup(string name, string smart)
        {
            Name = name;
            Smart = smart;
            string imagePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) + "\\SustainableChemistry\\Images\\FunctionalGroups\\";
            if (!System.IO.Directory.Exists(imagePath)) imagePath = "..\\..\\..\\..\\Images\\FunctionalGroups\\";
            string fileName = imagePath + Name + ".jpg";
            if (System.IO.File.Exists(fileName))
            {
                Image = System.Drawing.Image.FromFile(fileName);
            }
        }

        //public FunctionalGroup(string func, string directory)
        //{
        //    Name = func;
        //    m_refList = new List<Reference>();
        //    string[] imageFile = System.IO.Directory.GetFiles(directory, "*.jpg");
        //    if (imageFile.Length == 1)
        //        m_FunctGroupImage = System.Drawing.Image.FromFile(imageFile[0]);
        //    string[] references = System.IO.Directory.GetFiles(directory, "*.ris");
        //    foreach (string file in references)
        //        m_refList.Add(new Reference(this.Name, "", System.IO.File.ReadAllText(file)));
        //    m_Reactions = new NamedReactionCollection();
        //    m_AtomIndices = new List<int[]>();
        //}

        //public void AddNamedReaction(NamedReaction reaction)
        //{
        //    m_Reactions.Add(reaction);
        //}

        [Newtonsoft.Json.JsonProperty]
        public string Name { get; set; }

        //[Newtonsoft.Json.JsonProperty]
        //public int[][] AtomIndices {
        //    get
        //    {
        //        return this.m_AtomIndices.ToArray<int[]>();
        //    }
        //}
        public System.Drawing.Image Image { get; set; }
        public string Smart { get; set; }
        public string ImageFile { get; set; }

        //[Newtonsoft.Json.JsonProperty]
        //public NamedReactionCollection NamedReactions
        //{
        //    get
        //    {
        //        return this.m_Reactions;
        //    }
        //}
    }
}
