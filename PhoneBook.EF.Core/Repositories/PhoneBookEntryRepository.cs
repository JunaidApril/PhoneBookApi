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

        public async Task<List<Entry>> GetByPhoneBookId(int id)
        {
            return _context.PhoneBookEntry.Where(x => x.PhoneBookId == id).ToList();
        }

    }
}
