using PhoneBook.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoneBook.EF.Core.Models
{
    [Table("Entry")]
    public class Entry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public int PhoneBookId { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public bool isActive { get; set; }

        #region Foreign keys

        [ForeignKey(nameof(PhoneBookId))]
        public virtual PhoneBookList PhoneBookList { get; set; }

        #endregion
    }
}
