using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PhoneBook.DTO;
using PhoneBook.EF.Core.Context;
using PhoneBook.EF.Core.Models;
using PhoneBook.EF.Core.Repositories;
using PhoneBook.Enums;
using PhoneBook.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PhoneBook.Tests
{
    public class PhoneBookServiceTests
    {
        private PhoneBookService _phoneBookService;


        [SetUp]
        public void Setup()
        {
            try
            {
                var context = new Mock<PhoneBookDBContext>();
                var phoneBookSet = new Mock<DbSet<PhoneBookList>>();
                var entrySet = new Mock<DbSet<Entry>>();
                context.Setup(c => c.Set<PhoneBookList>()).Returns(phoneBookSet.Object);
                context.Setup(c => c.Set<Entry>()).Returns(entrySet.Object);

                var mockPhoneBookListRepositor = new Mock<PhoneBookListRepository>(context.Object);
                var mockPhoneBookEntryRepository = new Mock<PhoneBookEntryRepository>(context.Object);

                var entry = new Entry()
                {
                    Id = 1,
                    Name = EntryType.CellPhoneNumber.ToString(),
                    PhoneNumber = "0821234567",
                    PhoneBookId = 1,
                    DateAdded = DateTime.Now,
                    isActive = true
                };

                Task<List<Entry>> listOfEntries = Task.FromResult(new List<Entry>());
                listOfEntries.Result.Add(entry);

                Task<List<PhoneBookList>> listOfPhoneBookEntries = Task.FromResult(new List<PhoneBookList>());
                var phoneBookList = new PhoneBookList()
                {
                    Id = 1,
                    Name = "John Doe",
                    DateAdded = DateTime.Now,
                    isActive = true
                };
                listOfPhoneBookEntries.Result.Add(phoneBookList);

                mockPhoneBookListRepositor.Setup(x => x.GetAll()).Returns(listOfPhoneBookEntries);
                mockPhoneBookListRepositor.Setup(x => x.GetByName("John")).Returns(listOfPhoneBookEntries);
                mockPhoneBookEntryRepository.Setup(x => x.GetByPhoneBookId(1)).Returns(listOfEntries);

                _phoneBookService = new PhoneBookService(mockPhoneBookListRepositor.Object, mockPhoneBookEntryRepository.Object);
            }
            catch(Exception ex)
            {
                var test = "";
            }
        }

        /// <summary>
        /// This method setups up a phone book entry dto. Acts as a user enter details to capture name and numbers
        /// </summary>
        /// <returns>PhoneBookEntryDto</returns>
        public async Task<PhoneBookEntryDto> SetupEntryDto(string name, string cellPhoneNumber, string homePhoneNumber, string workPhoneNumber)
        {
            try
            {
                var phoneBookEntry = new PhoneBookEntryDto();
                phoneBookEntry.Name = name;
                List<EntryDto> entries = new List<EntryDto>();

                if (!string.IsNullOrEmpty(cellPhoneNumber))
                {
                    var entryCell = await _phoneBookService.CreateEntryDto(EntryType.CellPhoneNumber, cellPhoneNumber);
                    if (entryCell.IsSuccess)
                        entries.Add(entryCell.Data);
                }
                if (!string.IsNullOrEmpty(homePhoneNumber))
                {
                    var entryHome = await _phoneBookService.CreateEntryDto(EntryType.HomePhoneNumber, homePhoneNumber);
                    if (entryHome.IsSuccess)
                        entries.Add(entryHome.Data);
                }
                if (!string.IsNullOrEmpty(workPhoneNumber))
                {
                    var entryWork = await _phoneBookService.CreateEntryDto(EntryType.WorkPhoneNumber, workPhoneNumber);
                    if (entryWork.IsSuccess)
                        entries.Add(entryWork.Data);
                }

                phoneBookEntry.Entries = entries;

                return phoneBookEntry;
            }
            catch(Exception ex)
            {
                return new PhoneBookEntryDto();
            }
        }

        /// <summary>
        /// Test a phone book entry to pass
        /// </summary>
        /// <returns>Pass expected</returns>
        [Test]
        public async Task TestCreatePhoneBookEntryPass()
        {
            //Arrange
            var phoneBookEntry = await SetupEntryDto("Kenny", "0821234567", "0219051234", "0218005456");

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

        /// <summary>
        /// Test a null value being sent to add a phone book entry
        /// </summary>
        /// <returns>Pass expected but a message returned to say no user added</returns>
        [Test]
        public async Task TestCreatePhoneBookEntryNoValues()
        {
            //Act
            var result = await _phoneBookService.CreatePhoneBookEntry(null);

            //Assert
            if (result.IsSuccess)
            {
                Assert.AreEqual("No contact was added", result.Message);
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Test contact being returned by name
        /// </summary>
        /// <returns>Expect atleast 1 returned based on data setup</returns>
        [Test]
        public async Task TestRetrieveContactByName()
        {
            var result = await _phoneBookService.GetListOfPhoneBookEntries("John");

            //Assert
            if (result.IsSuccess)
            {
                Assert.IsTrue(result.Data.Count > 0);
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Test all contacts being returned.
        /// </summary>
        /// <returns>Atleast 1 or more in the return set</returns>
        [Test]
        public async Task TestRetrieveAllContacts()
        {
            var result = await _phoneBookService.GetListOfPhoneBookEntries();

            //Assert
            if (result.IsSuccess)
            {
                Assert.IsTrue(result.Data.Count > 0);
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Test search by name that does not exist in test set
        /// </summary>
        /// <returns>empty dto with message of no users found</returns>
        [Test]
        public async Task TestRetrieveContactDoesNotExist()
        {
            var result = await _phoneBookService.GetListOfPhoneBookEntries("Pablo");

            //Assert
            if (result.IsSuccess)
            {
                Assert.IsTrue(result.Data.Count == 0);
                Assert.AreEqual("Contact not found", result.Message);
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}