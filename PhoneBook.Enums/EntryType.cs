using System;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Enums
{
    public enum EntryType
    {
        [Display(Name = "Cell no")]
        CellPhoneNumber = 1,
        [Display(Name = "Home No")]
        HomePhoneNumber = 2,
        [Display(Name = "Work no")]
        WorkPhoneNumber = 3
    }
}
