using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.DTO
{
    public class PhoneContactResponseDto
    {
        public string Name { get; set; }

        public string CellPhoneNumber { get; set; }

        public string HomePhoneNumber { get; set; }

        public string WorkPhoneNumber { get; set; }
    }
}
