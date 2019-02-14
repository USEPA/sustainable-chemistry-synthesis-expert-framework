using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChemInfo
{
    public static class OnlineData
    {

        static public string GetCasNmber(string compoundName)
        {
            try
            {
                //gov.nih.nlm.chemspell.SpellAidService service = new gov.nih.nlm.chemspell.SpellAidService();
                string casNo = string.Empty;
                string url = "https://chemspell.nlm.nih.gov/spell/restspell/restSpell/getQuery4JSON?query=" + compoundName;
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                System.Net.WebResponse response = request.GetResponse();
                using (System.IO.Stream stream = response.GetResponseStream())
                {
                    System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8);
                    String responseString = reader.ReadToEnd();
                    string regex = @"(\d+\-\d\d\-\d)";
                    System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(regex);
                    System.Text.RegularExpressions.MatchCollection matches = rgx.Matches(responseString);
                    if (matches.Count > 0)
                    {
                        casNo = matches[0].Groups[0].Value;
                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            if (!string.Equals(match.Groups[0].Value, casNo)) casNo = string.Empty;
                        }
                    }
                }
                return casNo;
            }
            catch (System.Exception p_Ex)
            {
                System.Windows.Forms.MessageBox.Show("Unable to get Chemical Data from Internet. Please check Network Connection.");
                return string.Empty;
            }
        }
    }
}
