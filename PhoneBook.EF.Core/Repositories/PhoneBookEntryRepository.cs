using Microsoft.EntityFrameworkCore;
using PhoneBook.EF.Core.Context;
using PhoneBook.EF.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.EF.Core.Repositories
{
    public class PhoneBookEntryRepository : EfCoreRepository<Entry, PhoneBookDBContext>
    {
        internal PhoneBookDBContext _context;
        public PhoneBookEntryRepository(PhoneBookDBContext context) : base(context)
        {
            this._context = context;
        }

        public virtual async Task<List<Entry>> GetByPhoneBookId(int id)
        {
            if(id > 0)
                return await _context.PhoneBookEntry.Where(x => x.PhoneBookId == id).ToListAsync();

            return new List<Entry>();
        }

    }
}
