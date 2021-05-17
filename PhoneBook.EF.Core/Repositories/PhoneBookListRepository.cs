using PhoneBook.EF.Core.Context;
using PhoneBook.EF.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.EF.Core.Repositories
{
    public class PhoneBookListRepository : EfCoreRepository<PhoneBookList, PhoneBookDBContext>
    {
        internal PhoneBookDBContext _context;
        public PhoneBookListRepository(PhoneBookDBContext context) : base(context)
        {
            this._context = context;
        }
        public async Task<List<PhoneBookList>> GetByName(string name)
        {
            return _context.PhoneBookList.Where(x => x.Name.Contains(name)).ToList();
        }
        // This can be used to add specific new methods for the phonebooklist in the future
    }
}
