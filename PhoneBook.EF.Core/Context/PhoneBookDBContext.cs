using Microsoft.EntityFrameworkCore;
using PhoneBook.EF.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.EF.Core.Context
{
    public class PhoneBookDBContext : DbContext
    {
        public PhoneBookDBContext()
        {
        }

        public PhoneBookDBContext(DbContextOptions<PhoneBookDBContext> options)
            : base(options)
        {

        }

        public DbSet<PhoneBookList> PhoneBookList { get; set; }

        public DbSet<Entry> PhoneBookEntry { get; set; }

    }
}
