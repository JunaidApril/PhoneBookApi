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
    public class PhoneBookListRepository : EfCoreRepository<PhoneBookList, PhoneBookDBContext>
    {
        internal PhoneBookDBContext _context;
        public PhoneBookListRepository(PhoneBookDBContext context) : base(context)
        {
            this._context = context;
        }
        public virtual async Task<List<PhoneBookList>> GetByName(string name)
        {
            if(name != null)
                return await _context.PhoneBookList.Where(x => x.Name.Contains(name)).ToListAsync();

            return new List<PhoneBookList>();
        }
    }
}
