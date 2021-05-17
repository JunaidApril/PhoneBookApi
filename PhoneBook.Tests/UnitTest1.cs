using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PhoneBook.DTO;
using PhoneBook.EF.Core.Context;
using PhoneBook.EF.Core.Models;
using PhoneBook.EF.Core.Repositories;
using PhoneBook.Enums;
using PhoneBook.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneBook.Tests
{
    public class Tests
    {
        private PhoneBookService _phoneBookService;


        [SetUp]
        public void Setup()
        {
            var context = new Mock<PhoneBookDBContext>();
            var phoneBookSet = new Mock<DbSet<PhoneBookList>>();
            var entrySet = new Mock<DbSet<Entry>>();
            context.Setup(c => c.Set<PhoneBookList>()).Returns(phoneBookSet.Object);
            context.Setup(c => c.Set<Entry>()).Returns(entrySet.Object);                

            PhoneBookListRepository phoneBookListRepositor = new PhoneBookListRepository(context.Object);
            PhoneBookEntryRepository phoneBookEntryRepository = new PhoneBookEntryRepository(context.Object);
            _phoneBookService = new PhoneBookService(phoneBookListRepositor, phoneBookEntryRepository);
        }

        [Test]
        public async Task TestAddContact()
        {
            var mock = new Mock<PhoneBookListRepository>();
            //mock.Setup(x => x.GetAll()).ReturnsAsync()
            //Arrange
            var phoneBookEntry = new PhoneBookEntryDto();
            phoneBookEntry.Name = "Namies";
            List<EntryDto> entries = new List<EntryDto>();
            var entryCell = new EntryDto
            {
                Name = EntryType.CellPhoneNumber,
                PhoneNumber = "0821234567"
            };
            entries.Add(entryCell);
            var entryHome = new EntryDto
            {
                Name = EntryType.HomePhoneNumber,
                PhoneNumber = "0219051234"
            };
            entries.Add(entryHome);
            var entryWork = new EntryDto
            {
                Name = EntryType.WorkPhoneNumber,
                PhoneNumber = "0218005456"
            };
            entries.Add(entryWork);
            phoneBookEntry.Entries = entries;

            //Act
            var result = await _phoneBookService.CreatePhoneBookEntry(phoneBookEntry);

            //Assert
            if (result.IsSuccess)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}