using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;
using SustainableChemistryWeb.ViewModels;
using Microsoft.AspNetCore.Hosting;

// See answer here for getting webroot...
// https://stackoverflow.com/questions/43709657/how-to-get-root-directory-of-project-in-asp-net-core-directory-getcurrentdirect

namespace SustainableChemistryWeb.Controllers
{
    public class FunctionalGroupsController : Controller
    {
        private readonly SustainableChemistryContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FunctionalGroupsController(SustainableChemistryContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // O=P(OC)(OC)C
        // GET: FunctionalGroups
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Index(int? Id, int? funcGroupId, int? namedReactionId, string nameSearchString, string smilesSearchString)
        {
            var viewModel = new SustainableChemistryWeb.ViewModels.FunctionalGroupIndexData();
            viewModel.FunctionalGroups = await _context.AppFunctionalgroup
                  .Include(i => i.AppNamedreaction)
                    .ThenInclude(i => i.AppReference)
                  .AsNoTracking()
                  .OrderBy(i => i.Name)
                  .ToListAsync();

            List<FunctionalGroup> fgFound = new List<FunctionalGroup>();
            ChemInfo.Molecule molecule = null;

            if (!String.IsNullOrEmpty(nameSearchString))
            {
                ViewData["SearchString"] = nameSearchString.Trim();
                fgFound.AddRange(viewModel.FunctionalGroups.Where(s => s.Name.Contains(nameSearchString, StringComparison.OrdinalIgnoreCase)));
            }

            else if (!String.IsNullOrEmpty(smilesSearchString))
            {
                string CASNo = string.Empty;
                TestCASNo(ref CASNo, ref smilesSearchString);
                molecule = new ChemInfo.Molecule(smilesSearchString.Trim());
                if (molecule.Atoms.Length != 0)
                {
                    string url = "https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/smiles/" + smilesSearchString + "/synonyms/TXT";
                    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
                    string[] output = reader.ReadToEnd().Split('\n');
                    string pattern = "^CAS-(?<1>\\d{2,7}-\\d{2}-\\d)";
                    foreach (string line in output)
                    {
                        System.Text.RegularExpressions.Match m1 = System.Text.RegularExpressions.Regex.Match(line, pattern,
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled,
                            TimeSpan.FromSeconds(1));
                        if (m1.Groups.Count > 1)
                        {
                            if (this.validateCasNumber(m1.Groups[1].Value))
                            {
                                CASNo = m1.Groups[1].Value;
                            }
                        }
                    }
                    ViewData["CASNO"] = CASNo;
                    ViewData["SmilesString"] = smilesSearchString;
                    url = "https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/smiles/" + smilesSearchString + "/property/IUPACName/TXT";
                    request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                    response = request.GetResponse();
                    reader = new System.IO.StreamReader(response.GetResponseStream());                
                    ViewData["IUPACName"] = reader.ReadToEnd();
                    foreach (var fg in viewModel.FunctionalGroups)
                    {
                        string smarts = fg.Smarts;
                        if (!string.IsNullOrEmpty(fg.Smarts))
                            if (molecule.FindFunctionalGroup(fg))
                            {
                                fgFound.Add(fg);
                            }
                    }
                    if (molecule.Aromatic) fgFound.Add(_context.AppFunctionalgroup
                        .FirstOrDefault(m => m.Id == 35));
                    if (molecule.Heterocyclic) fgFound.Add(_context.AppFunctionalgroup
                        .FirstOrDefault(m => m.Id == 118));
                    if (molecule.HeterocyclicAromatic) fgFound.Add(_context.AppFunctionalgroup
                        .FirstOrDefault(m => m.Id == 224));
                }
            }

            else if (funcGroupId != null)
            {
                ViewData["FunctionalGroupID"] = funcGroupId.Value;
                FunctionalGroup group = viewModel.FunctionalGroups.Where(
                    i => i.Id == funcGroupId.Value).Single();
                fgFound.Add(group);
            }

            if (funcGroupId != null)
            {
                ViewData["FunctionalGroupID"] = funcGroupId.Value;
                FunctionalGroup group = viewModel.FunctionalGroups.Where(
                    i => i.Id == funcGroupId.Value).Single();
                viewModel.NamedReactions = group.AppNamedreaction;
                ViewData["FunctionalGroupName"] = group.Name;
            }

            if (namedReactionId != null)
            {
                ViewData["NamedReactionID"] = namedReactionId.Value;
                NamedReaction rxn = viewModel.NamedReactions.Where(
                    i => i.Id == namedReactionId.Value).Single();
                var referenceViewModels = new List<SustainableChemistryWeb.ViewModels.ReferenceViewModel>();
                ViewData["NamedReactionName"] = rxn.Name;
                foreach (var referecnce in rxn.AppReference)
                {
                    referenceViewModels.Add(new SustainableChemistryWeb.ViewModels.ReferenceViewModel
                    {
                        Id = referecnce.Id,
                        FunctionalGroupId = referecnce.FunctionalGroupId,
                        FunctionalGroup = viewModel.FunctionalGroups.Where(
                            i => i.Id == referecnce.FunctionalGroupId).Single(),
                        ReactionId = referecnce.ReactionId,
                        Reaction = referecnce.Reaction,
                        Risdata = referecnce.Risdata
                    });
                }
                viewModel.References = referenceViewModels;
            }
            if (!string.IsNullOrEmpty(nameSearchString) || !string.IsNullOrEmpty(smilesSearchString) || funcGroupId != null) viewModel.FunctionalGroups = fgFound.OrderBy(i => i.Name);
            return View(viewModel);
        }

        // GET: FunctionalGroups/Details/5
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appFunctionalgroup = await _context.AppFunctionalgroup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appFunctionalgroup == null)
            {
                return NotFound();
            }

            return View(appFunctionalgroup);
        }

        // GET: FunctionalGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FunctionalGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Smarts,ImageFile,URL")] SustainableChemistryWeb.ViewModels.FunctionalGroupViewModel functionalGroupView)
        {
            if (ModelState.IsValid)
            {
                string name = System.IO.Path.GetFileName(functionalGroupView.ImageFile.FileName);
                FunctionalGroup appFunctionalGroup = new FunctionalGroup()
                {
                    Name = functionalGroupView.Name,
                    Smarts = functionalGroupView.Smarts,
                    Image = "Images/FunctionalGroups/" + name,
                    URL = functionalGroupView.URL
                };
                if (functionalGroupView.ImageFile.Length > 0)
                {
                    using (var stream = new System.IO.FileStream(_hostingEnvironment.WebRootPath + "/Images/FunctionalGroups/" + name, System.IO.FileMode.Create))
                    {
                        await functionalGroupView.ImageFile.CopyToAsync(stream);
                        stream.Close();
                    }
                }
                _context.Add(appFunctionalGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(functionalGroupView);
        }

        // GET: FunctionalGroups/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appFunctionalgroup = await _context.AppFunctionalgroup.FindAsync(id);
            if (appFunctionalgroup == null)
            {
                return NotFound();
            }
            SustainableChemistryWeb.ViewModels.FunctionalGroupViewModel functionalGroupView = new SustainableChemistryWeb.ViewModels.FunctionalGroupViewModel()
            {
                Name = appFunctionalgroup.Name,
                Smarts = appFunctionalgroup.Smarts,
                Image = appFunctionalgroup.Image,
                URL = appFunctionalgroup.URL
            };

            return View(functionalGroupView);
        }

        // POST: FunctionalGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Smarts,Image,ImageFile,URL")] SustainableChemistryWeb.ViewModels.FunctionalGroupViewModel functionalGroupView)
        {
            if (ModelState.IsValid)
                if (id != functionalGroupView.Id)
                {
                    return NotFound();
                }

            if (id != functionalGroupView.Id)
            {
                return NotFound();
            }

            var functionalGroupToUpdate = await _context.AppFunctionalgroup
                .SingleOrDefaultAsync(m => m.Id == id);

            if (await TryUpdateModelAsync<FunctionalGroup>(
                functionalGroupToUpdate,
                "",
                r => r.Name, r => r.Smarts, r => r.URL))
            {
                try
                {
                    var fileName = _hostingEnvironment.WebRootPath + "/" + functionalGroupToUpdate.Image;
                    if (functionalGroupView.ImageFile != null)
                    {
                        if (System.IO.File.Exists(fileName))
                        {
                            System.IO.File.Delete(fileName);
                        }
                        functionalGroupToUpdate.Image = "Images/FunctionalGroups/" + Guid.NewGuid().ToString() + System.IO.Path.GetFileName(functionalGroupView.ImageFile.FileName);
                        using (var stream = new System.IO.FileStream(_hostingEnvironment.WebRootPath + "/" + functionalGroupToUpdate.Image, System.IO.FileMode.Create))
                        {
                            await functionalGroupView.ImageFile.CopyToAsync(stream);
                            stream.Close();
                        }
                    }
                    //else
                    //{
                    //    functionalGroupToUpdate.Image = "Images/FunctionalGroups/" + Guid.NewGuid().ToString() + ".jpg";
                    //    System.IO.StreamReader image = new System.IO.StreamReader(_hostingEnvironment.WebRootPath + "/Images/Reactions/th.jpg");
                    //    using (var stream = new System.IO.FileStream(_hostingEnvironment.WebRootPath + "/" + functionalGroupToUpdate.Image, System.IO.FileMode.Create))
                    //    {
                    //        await image.BaseStream.CopyToAsync(stream);
                    //        stream.Close();
                    //    }
                    //}
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppFunctionalgroupExists(functionalGroupToUpdate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(functionalGroupView);
        }

        // GET: FunctionalGroups/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appFunctionalgroup = await _context.AppFunctionalgroup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appFunctionalgroup == null)
            {
                return NotFound();
            }

            return View(appFunctionalgroup);
        }

        // POST: FunctionalGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var appFunctionalgroup = await _context.AppFunctionalgroup.FindAsync(id);
            var fileName = _hostingEnvironment.WebRootPath + "/" + appFunctionalgroup.Image;
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
            var reactions = _context.AppNamedreaction
                .Include(a => a.Catalyst)
                .Include(a => a.FunctionalGroup)
                .Include(a => a.Solvent)
                    .Include(i => i.AppNamedreactionReactants)
                        .ThenInclude(i => i.Reactant)
                    .Include(i => i.AppNamedreactionByProducts)
                        .ThenInclude(i => i.Reactant)
                .Include(a => a.AppNamedreactionByProducts)
                .Where(i => i.FunctionalGroupId == id).ToList();
            foreach (NamedReaction rxn in reactions)
            {
                fileName = _hostingEnvironment.WebRootPath + "/" + rxn.Image;
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
                var reactionByProducts = new HashSet<long>
                    (rxn.AppNamedreactionByProducts.Select(c => c.Reactant.Id));
                foreach (var reactant in _context.AppReactant)
                {
                    if (reactionByProducts.Contains(reactant.Id))
                    {
                        NamedReactionByProducts reactantToRemove = rxn.AppNamedreactionByProducts.SingleOrDefault(i => i.ReactantId == reactant.Id);
                        _context.Remove(reactantToRemove);
                    }
                }

                var reactionReactants = new HashSet<long>
                    (rxn.AppNamedreactionReactants.Select(c => c.Reactant.Id));
                foreach (var reactant in _context.AppReactant)
                {
                    if (reactionReactants.Contains(reactant.Id))
                    {
                        NamedReactionReactants reactantToRemove = rxn.AppNamedreactionReactants.SingleOrDefault(i => i.ReactantId == reactant.Id);
                        _context.Remove(reactantToRemove);
                    }
                }
                rxn.FunctionalGroup = null;
                rxn.Catalyst = null;
                rxn.Solvent = null;
                _context.AppNamedreaction.Remove(rxn);
            }
            var references = _context.AppReference.Where(i => i.FunctionalGroupId == id).ToList();
            foreach (Reference r in references) _context.AppReference.Remove(r);

            _context.AppFunctionalgroup.Remove(appFunctionalgroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppFunctionalgroupExists(long id)
        {
            return _context.AppFunctionalgroup.Any(e => e.Id == id);
        }

        private bool TestCASNo(ref string CASNo, ref string smilesSearchString)
        {
            string pattern = "(?<1>\\d+-\\d{2}-\\d)";
            System.Text.RegularExpressions.Match m1 = System.Text.RegularExpressions.Regex.Match(smilesSearchString, pattern,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled,
                TimeSpan.FromSeconds(1));
            if (m1.Groups.Count > 1)
            {
                if (this.validateCasNumber(m1.Groups[0].Value))
                {
                    CASNo = smilesSearchString;
                    string url = "https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/xref/RN/" + CASNo + "/property/CanonicalSMILES/JSON";
                    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
                    //string output = reader.ReadToEnd();
                    System.Runtime.Serialization.Json.DataContractJsonSerializer jSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Areas.Online.Rootobject));
                    Areas.Online.Rootobject chemicalName = (Areas.Online.Rootobject)jSerializer.ReadObject(response.GetResponseStream());
                    smilesSearchString = chemicalName.PropertyTable.Properties[0].CanonicalSMILES;
                    return true;
                }
            }
            return false;
        }

        bool validateCasNumber(string casNo)
        {
            // split the CAS Number into parts separated by a dash...
            string[] parts = casNo.Split('-');
            // check that there were three pieces of the cas number...if not, the cas number is improperly formatted and return false
            if (parts.Length != 3) return false;

            // Check to see if a numbers were submitted...
            int part1 = 0;
            if (!int.TryParse(parts[0], out part1))
            {
                // if not, return false
                return false;
            }
            // Check to see if a numbers were submitted...
            int part2 = 0;
            if (!int.TryParse(parts[1], out part2))
            {
                // if not, return false
                return false;
            }
            // Check to see if a numbers were submitted...
            int part3 = 0;
            if (!int.TryParse(parts[2], out part3))
            {
                // if not, return false
                return false;
            }

            //initialize the checksum
            int checksum = 0;

            // handle part 2...
            checksum = (part2 % 10);
            checksum = checksum + (part2 / 10) * 2;

            // now handle part 1, it can have between 2 and 7 digits...
            int n = 3;
            while (part1 > 10)
            {
                checksum = checksum + (part1 % 10) * n++;
                part1 = part1 / 10;
            }
            checksum = checksum + part1 * n;

            // number is valid if the last digit equals the remainder from dividing 
            // the checksum by 10.
            if (checksum % 10 == part3)
            {
                // throw exception if the checksum does not work...
                return true;
            }
            return false;

        }

        //private void findCompound(ref string compoundName, ref string CasNo)
        //{
        //    string url = "https://chemspell.nlm.nih.gov/spell/restspell/restSpell/getQuery4JSON?query=" + compoundName;
        //    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
        //    System.Net.WebResponse response = request.GetResponse();
        //    string responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
        //    List<SynonymChemical> synonyms = null;
        //    if (responseString.StartsWith("Synonym List:"))
        //    {
        //        synonyms = this.ExtractSynonyms(responseString.Remove(0, 13));
        //        CasNo = synonyms[0].CAS;
        //        bool casSame = true;
        //        this.listBox1.BeginUpdate();
        //        foreach (SynonymChemical chemical in synonyms)
        //        {
        //            this.listBox1.Items.Add(chemical.Name);
        //            if (CasNo != chemical.CAS)
        //            {
        //                casSame = false;
        //            }
        //        }
        //        if (!casSame) System.Windows.Forms.MessageBox.Show("Not all synomymns have the same CAS CAS Number.");
        //        this.listBox1.EndUpdate();
        //        return;
        //    }
        //    synonyms = this.ExtractSynonyms(responseString.Remove(0, 26));
        //    Form3 selector = new Form3();
        //    selector.chemicals = synonyms.ToArray(); ;
        //    selector.ShowDialog();
        //    compoundName = selector.SelectedChemical;
        //    CasNo = string.Empty;
        //    this.findCompound(ref compoundName, ref CasNo);

        //    // They broke this with their new web service.
        //    //gov.nih.nlm.chemspell.SpellAidService service = new gov.nih.nlm.chemspell.SpellAidService();
        //    //string response = service.getSugList(compoundName, "All databases");
        //    //response = response.Replace("&", "&amp;");
        //    //var XMLReader = new System.Xml.XmlTextReader(new System.IO.StringReader(response));
        //    //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Synonym));
        //    //if (serializer.CanDeserialize(XMLReader))
        //    //{
        //    //    // Synonyms means more than one name for the same chemical/CAS Number.
        //    //    Synonym synonym = (Synonym)serializer.Deserialize(XMLReader);
        //    //    CasNo = synonym.Chemical[0].CAS;
        //    //    this.listBox1.BeginUpdate();
        //    //    foreach (SynonymChemical chemical in synonym.Chemical)
        //    //    {
        //    //        this.listBox1.Items.Add(chemical.Name);
        //    //        if (CasNo != chemical.CAS)
        //    //        {
        //    //            System.Windows.Forms.MessageBox.Show(compoundName + " has a synonym with a different CAS Number.");
        //    //            return;
        //    //        }
        //    //    }
        //    //    this.listBox1.EndUpdate();
        //    //    return;
        //    //}
        //    //serializer = new System.Xml.Serialization.XmlSerializer(typeof(SpellAid));
        //    //if (serializer.CanDeserialize(XMLReader))
        //    //{
        //    //    SpellAid aid = (SpellAid)serializer.Deserialize(XMLReader);
        //    //    Form3 selector = new Form3();
        //    //    selector.chemicals = aid;
        //    //    selector.ShowDialog();
        //    //    compoundName = selector.SelectedChemical;
        //    //    this.findCompound(ref compoundName, ref CasNo);
        //    //    return;
        //    //}
        //}

    }
}
