using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    public class ReferenceAddedEventArgs : EventArgs
    {
        private Reference[] reference;

        public ReferenceAddedEventArgs(Reference[] reference)
        {
            this.reference = reference;
        }

        public Reference[] AddedReferences { get; }
    }

    [Serializable]
    public class References : List<Reference>
    {
        public event EventHandler ReferenceAdded;

        protected virtual void OnReferenceAdded(EventArgs e)
        {
            ReferenceAdded?.Invoke(this, e);
        }

        public References()
        {
        }

        public References(System.Data.DataTable table)
        {
            foreach (System.Data.DataRow row in table.Rows)
            {
                this.Add(new Reference(row["FunctionalGroup"].ToString(), row["ReactionName"].ToString(), row["RISData"].ToString()));
            }
            //NamedReactions = new NamedReactionCollection();
        }

        public References(String functionalGroup, string Reaction, String Data, System.Drawing.Image image)
        {
            string temp = Data;
            int len = temp.Length;
            int index = temp.IndexOf("ER  -");
            while (index > 0)
            {
                this.Add(new Reference(functionalGroup, Reaction, temp.Substring(0, index + 6)));
                temp = temp.Remove(0, index + 6);
                int len1 = temp.Length;
                index = temp.IndexOf("ER  -");
            }
        }

        public Reference[] GetReferences(string functionalGroup)
        {
            var results = from reference in this where reference.FunctionalGroup == functionalGroup select reference;

            List<Reference> retVal = new List<Reference>();
            foreach (Reference r in results)
            {
                retVal.Add(r);
            }

            return retVal.ToArray<Reference>();
        }

        public Reference this[string text]
        {
            get
            {
                foreach (Reference r in this)
                {
                    if (r.ToString() == text)
                    {
                        return r;
                    }
                }
                return null;
            }
        }


        public List<string[]> ReferenceList
        {
            get
            {
                List<string[]> retVal = new List<string[]>();
                foreach (Reference r in this)
                {
                    retVal.Add(new string[] { r.FunctionalGroup, r.ToString() });
                }
                return retVal;
            }
        }
    }
}
