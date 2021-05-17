using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.DTO
{
    public class PhoneBookEntryDto
    {
        public PhoneBookEntryDto()
        {
            Entries = new List<EntryDto>();
        }
        public string Name { get; set; }

        public List<EntryDto> Entries { get; set; }

    }
}
