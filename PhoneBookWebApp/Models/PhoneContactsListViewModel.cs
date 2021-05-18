using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBookWebApp.Models
{
    public class PhoneContactsListViewModel
    {
        public PhoneContactsListViewModel()
        {
            PhoneContacts = new List<PhoneContactViewModel>();
        }
        public List<PhoneContactViewModel> PhoneContacts { get; set; }

        public string AddContactClientMessage { get; set; }

        public string SearchContactClientMessage { get; set; }
    }

    public class PhoneContactViewModel
    {
        public string Name { get; set; }

        public string CellPhoneNumber { get; set; }

        public string HomePhoneNumber { get; set; }

        public string WorkPhoneNumber { get; set; }
    }
}
