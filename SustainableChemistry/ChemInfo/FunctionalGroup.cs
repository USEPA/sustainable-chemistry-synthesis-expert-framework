﻿using System;
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
        NamedReactionCollection m_Reactions;
        List<Reference> m_refList;
        [NonSerialized]
        List<int[]> m_AtomIndices;

        public FunctionalGroup(string str, System.Data.DataRow row)
        {   
            string[] parts = str.Split('\t');
            Name = parts[0].Trim();
            row["Name"] = Name;
            Smart = parts[1].Trim();
            row["Smarts"] = Smart;
            m_Reactions = new NamedReactionCollection();
            string fileName = "..\\..\\..\\..\\Images\\FunctionalGroups\\" + Name + ".jpg";
            if (System.IO.File.Exists(fileName))
            {
                Image = System.Drawing.Image.FromFile(fileName);
                row["Image"] = fileName;
            }
            m_AtomIndices = new List<int[]>();
        }

        public FunctionalGroup(string name, string smart, string imageFile)
        {
            Name = name;
            Smart = smart;
            if (System.IO.File.Exists(imageFile))
            {
                Image = System.Drawing.Image.FromFile(imageFile);
            }
            ImageFile = imageFile;
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

        public void AddNamedReaction(NamedReaction reaction)
        {
            m_Reactions.Add(reaction);
        }

        public void AddAtoms(int[] atomIndices)
        {
            this.m_AtomIndices.Add(atomIndices);
        }

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

        [Newtonsoft.Json.JsonProperty]
        public NamedReactionCollection NamedReactions
        {
            get
            {
                return this.m_Reactions;
            }
        }
    }
}