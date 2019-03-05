using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SustainableChemistryWeb.Models
{
    public class UserIdStatus
    {
        // user ID from AspNetUser table.
        public string OwnerID { get; set; }

        public Status Status { get; set; }
    }

    public enum Status
    {
        Submitted,
        Approved,
        Rejected
    }
}
