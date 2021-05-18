using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.DAL;
using PhoneBook.DTO;
using PhoneBook.EF.Core.Context;
using PhoneBook.EF.Core.Repositories;
using PhoneBook.Enums;
using PhoneBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBookApi.Controllers
{
    [Route("api/PhoneBook")]
    [ApiController]
    public class PhoneBookController : ControllerBase
    {
        private readonly PhoneBookListRepository _phoneBookListRepositor;
        private readonly PhoneBookEntryRepository _phoneBookEntryRepository;
        private readonly PhoneBookService _phoneBookService;
        public PhoneBookController(PhoneBookDBContext context)
        {
            _phoneBookListRepositor = new PhoneBookListRepository(context);
            _phoneBookEntryRepository = new PhoneBookEntryRepository(context);
            _phoneBookService = new PhoneBookService(_phoneBookListRepositor, _phoneBookEntryRepository);
        }

        /// <summary>
        /// Add an entry to the phone book
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cellphoneNumber"></param>
        /// <param name="homePhoneNumber"></param>
        /// <param name="workPhoneNumber"></param>
        /// <returns>Ok result if successful</returns>
        [HttpPost] 
        [Route("AddEntry")]
        public async Task<IActionResult> AddEntry(string name, string cellphoneNumber, string homePhoneNumber, string workPhoneNumber)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var phoneBookEntryDto = new PhoneBookEntryDto();
                    var entrieslist = new List<EntryDto>();
                    phoneBookEntryDto.Name = name;

                    if(!string.IsNullOrEmpty(cellphoneNumber))
                    {
                        var cellNumberEntry = await _phoneBookService.CreateEntryDto(EntryType.CellPhoneNumber, cellphoneNumber);
                        if(cellNumberEntry.IsSuccess)
                            entrieslist.Add(cellNumberEntry.Data);
                    }
                    if (!string.IsNullOrEmpty(homePhoneNumber))
                    {
                        var homeNumberEntry = await _phoneBookService.CreateEntryDto(EntryType.HomePhoneNumber, homePhoneNumber);
                        if(homeNumberEntry.IsSuccess)
                            entrieslist.Add(homeNumberEntry.Data);
                    }
                    if (!string.IsNullOrEmpty(workPhoneNumber))
                    {
                        var workNumberEntry = await _phoneBookService.CreateEntryDto(EntryType.WorkPhoneNumber, workPhoneNumber);
                        if(workNumberEntry.IsSuccess)
                            entrieslist.Add(workNumberEntry.Data);
                    }

                    if(entrieslist != null && entrieslist.Count > 0)
                        phoneBookEntryDto.Entries = entrieslist;

                    var result = await _phoneBookService.CreatePhoneBookEntry(phoneBookEntryDto);

                    if(result.IsSuccess)
                        return Ok();
                }
                return BadRequest("An error occured trying to save an entry");
            }
            catch(Exception ex)
            {
                return BadRequest("An error occured trying to save an entry");
            }
        }

        /// <summary>
        /// Get List returns a list of concacts by the name passed or if empty it returns all contacts
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List of PhoneContactResponseDto</returns>
        [HttpGet]
        [Route("GetList")]
        public async Task<IActionResult> GetList(string name = "")
        {
            try
            {
                //Call service to retrieve all the phone book entries or search by name
                var result = await _phoneBookService.GetListOfPhoneBookEntries(name);
                if (result.IsSuccess)
                    return Ok(result.Data);

                return BadRequest("An error occured trying to save an entry");
            }
            catch(Exception ex)
            {
                return BadRequest("An error occured trying to save an entry");
            }
        }

        /// <summary>
        /// Creates a entry dto to build the list for the phone book
        /// </summary>
        /// <param name="entryType"></param>
        /// <param name="phoneNumber"></param>
        /// <returns>EntryDto</returns>
        private EntryDto CreateEntryDto(EntryType entryType, string phoneNumber)
        {
            var entry = new EntryDto();
            entry.PhoneNumber = phoneNumber;
            switch (entryType)
            {
                case EntryType.CellPhoneNumber:
                    entry.Name = EntryType.CellPhoneNumber;
                    break;
                case EntryType.HomePhoneNumber:
                    entry.Name = EntryType.HomePhoneNumber;
                    break;
                default:
                    entry.Name = EntryType.WorkPhoneNumber;
                    break;
            }

            return entry;
        }
    }
}
