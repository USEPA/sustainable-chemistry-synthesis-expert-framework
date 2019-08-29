using System;
using System.Collections.Generic;
using System.Linq;

namespace SustainableChemistryWeb.ViewModels
{
    public class ReferenceViewModel
    {
        List<string> m_authors;

        public ReferenceViewModel()
        {
            m_authors = new List<string>();
        }

        public long Id { get; set; }
        string _Risdata;
        public Microsoft.AspNetCore.Http.IFormFile RISFile { get; set; }
        public string Risdata
        {
            get
            {
                return _Risdata;
            }
            set
            {
                m_authors = new List<string>();
                string[] lines = value.Replace("\r", string.Empty).Split('\n');
                _Risdata = value.TrimStart(' ', '\r', '\n');
                foreach (string line in lines)
                {

                    if (line.StartsWith("TY")) Type = line.Substring(6);
                    if (String.IsNullOrEmpty(Title) && line.StartsWith("TI")) Title = line.Substring(6);
                    if (String.IsNullOrEmpty(Title) && line.StartsWith("T1")) Title = line.Substring(6);
                    if (line.StartsWith("AU")) m_authors.Add(line.Substring(6));
                    if (line.StartsWith("AB")) Abstract = line.Substring(6);
                    if (line.StartsWith("AD")) AuthorAddress = line.Substring(6);
                    if (line.StartsWith("JO")) Journal = line.Substring(6);
                    if (line.StartsWith("VL")) Volume = line.Substring(6);
                    if (line.StartsWith("IS")) Issue = line.Substring(6);
                    if (line.StartsWith("SP")) StartPage = line.Substring(6);
                    if (line.StartsWith("EP")) EndPage = line.Substring(6);
                    if (line.StartsWith("DA")) Date = line.Substring(6);
                    if (line.StartsWith("UR")) URL = line.Substring(6);
                    if (line.StartsWith("DO")) doi = line.Substring(6);
                    if (line.StartsWith("PY")) PY = line.Substring(6);
                }
                if (string.IsNullOrEmpty(URL))
                {
                    if (!string.IsNullOrEmpty(doi))
                    {
                        URL = "https://doi.org/" + doi;
                        if (doi.StartsWith("http"))
                        {
                            URL = doi;
                        }
                    }
                }
            }
        }

        public long? FunctionalGroupId { get; set; }
        public long? ReactionId { get; set; }

        public virtual SustainableChemistryWeb.Models.FunctionalGroup FunctionalGroup { get; set; }
        public virtual SustainableChemistryWeb.Models.NamedReaction Reaction { get; set; }
        public string Type { get; private set; }
        public string Title { get; private set; }
        public string[] Authors
        {
            get
            {
                return m_authors.ToArray<string>();
            }
        }
        public string Abstract { get; private set; }
        public string AuthorAddress { get; private set; }
        public string Journal { get; private set; }
        public string Volume { get; private set; }
        public string Issue { get; private set; }
        public string StartPage { get; private set; }
        public string EndPage { get; private set; }
        public string Date { get; private set; }
        public string URL { get; private set; }
        public string doi { get; private set; }
        string _PY;
        public string PY
        {
            get
            {
                return _PY;
            }
            private set
            {
                _PY = value;
                while (_PY.Contains("/"))
                {
                    _PY = _PY.Replace("/", string.Empty);
                }
            }
        }
        public string Text
        {
            get
            {
                return this.ToString();
            }
        }

        override public string ToString()
        {
            string retVal = String.Empty;
            foreach (string author in this.Authors)
            {
                retVal = retVal + author + ", ";
            }
            if (this.Authors.Length < 1) retVal = ", ";
            retVal = retVal.Remove(retVal.Length - 2, 2) + " (" + PY + "). \"" + Title + ".\" " + Journal + " " + Volume + ": " + StartPage + "-" + EndPage + ".";
            return retVal;
        }
    }
}
