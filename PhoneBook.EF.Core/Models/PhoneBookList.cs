using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoneBook.EF.Core.Models
{
    [Table("PhoneBookList")]
    public class PhoneBookList
    {
        public PhoneBookList()
        {
            Entries = new List<Entry>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public bool isActive { get; set; }

        #region inverse properties

        [InverseProperty("PhoneBookList")]
        public virtual ICollection<Entry> Entries { get; set; }

        #endregion

    }
}
