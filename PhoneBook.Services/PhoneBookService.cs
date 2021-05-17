using PhoneBook.DAL;
using PhoneBook.DTO;
using PhoneBook.EF.Core.Models;
using PhoneBook.EF.Core.Repositories;
using PhoneBook.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneBook.Services
{
    public class PhoneBookService
    {
        private readonly PhoneBookListRepository _phoneBookListRepository;
        private readonly PhoneBookEntryRepository _phoneBookEntryRepository;

        public PhoneBookService(PhoneBookListRepository phoneBookListRepository, PhoneBookEntryRepository phoneBookEntryRepository)
        {
            _phoneBookListRepository = phoneBookListRepository;
            _phoneBookEntryRepository = phoneBookEntryRepository;
        }

        /// <summary>
        /// This method is to Create a entry into the Phone Book including name and numbers
        /// </summary>
        /// <param name="phoneBookEntryDto"></param>
        /// <returns>System result</returns>
        public async Task<SystemResult> CreatePhoneBookEntry(PhoneBookEntryDto phoneBookEntryDto)
        {
            try
            {
                if(phoneBookEntryDto != null)
                {
                    //Store the contact into the db
                    var resultStoreEntry = await StorePhoneBookListEntry(phoneBookEntryDto.Name);

                    if (phoneBookEntryDto.Entries != null && phoneBookEntryDto.Entries.Count > 0 && resultStoreEntry.Data > 0)
                    {
                        //Store the different numbers linked to the contact in the db
                        var resultEntries = await StorePhoneBookEntries(phoneBookEntryDto.Entries, resultStoreEntry.Data);

                        //return success
                        if(resultEntries.IsSuccess)
                            return await Task.FromResult(SystemResult.Success());
                    }
                }
                //Return positive result
                return await Task.FromResult(SystemResult.Success());
            }
            catch(Exception ex)
            {
                //Send error result
                return await Task.FromResult(new SystemResult(ex.Message));
            }
        }

        /// <summary>
        /// This method is to store the different phone type entries with the phone number into the database
        /// </summary>
        /// <param name="entries">List of phone type and the number </param>
        /// <param name="phoneBookId">Id of the Name stored in the Phone book list table</param>
        /// <returns>System Result</returns>
        private async Task<SystemResult> StorePhoneBookEntries(List<EntryDto> entries, int phoneBookId)
        {
            try
            {
                if (entries != null && entries.Count > 0)
                {
                    //Loop each entry to store
                    foreach (var entry in entries)
                    {
                        //Create ef model to store
                        var entryEf = new Entry();
                        entryEf.Name = entry.Name.ToString();
                        entryEf.PhoneNumber = entry.PhoneNumber;
                        entryEf.isActive = true;
                        entryEf.DateAdded = DateTime.Now;
                        entryEf.PhoneBookId = phoneBookId;

                        //Store the ef record
                        var entryResult = await _phoneBookEntryRepository.Add(entryEf);
                    }
                }
                //Return positive result
                return await Task.FromResult(SystemResult.Success());
            }
            catch(Exception ex)
            {
                //Send error result
                return await Task.FromResult(new SystemResult(ex.Message));
            }
        }

        /// <summary>
        /// This method is to store the Contacts name into the Phone Book list table
        /// </summary>
        /// <param name="name">Contact name being stored</param>
        /// <returns>System Result</returns>
        private async Task<SystemResult<int>> StorePhoneBookListEntry(string name)
        {
            try
            {
                //Create ef model to store
                var phoneBookListEf = new PhoneBookList();

                phoneBookListEf.Name = name;
                phoneBookListEf.DateAdded = DateTime.Now;
                phoneBookListEf.isActive = true;

                //Store the ef record
                var listResult = await _phoneBookListRepository.Add(phoneBookListEf);
                
                //Return positive result
                if (listResult != null && listResult.Id > 0)
                    return await Task.FromResult(SystemResult<int>.Success(listResult.Id, "Success", ""));


                return await Task.FromResult(SystemResult<int>.Success(0, "Failed", ""));

            }
            catch(Exception ex)
            {
                return await Task.FromResult(new SystemResult<int>(ex.Message));
            }
        }

        /// <summary>
        /// This method is to return a list of all phone book entries
        /// </summary>
        /// <returns>System result containging all phone book name and numbers</returns>
        public async Task<SystemResult<List<PhoneContactResponseDto>>> GetListOfPhoneBookEntries(string name = "")
        {
            try
            {
                var phoneContactResponseDtoList = new List<PhoneContactResponseDto>();

                //Get the whole phone book list
                var phoneBookList = await GetPhoneBookList(name);

                if (phoneBookList.IsSuccess && phoneBookList.Data.Count > 0)
                {
                    //Navigate throught the list and retrieve the numbers for each contact
                    foreach (var x in phoneBookList.Data)
                    {
                        var phoneContactResponseDto = new PhoneContactResponseDto();
                        phoneContactResponseDto.Name = x.Name;

                        //Get the tel numbers for the contact by id
                        var entriesResult = await GetListEntries(x.Id);

                        if (entriesResult.IsSuccess && entriesResult.Data.Count > 0)
                        {
                            foreach (var y in entriesResult.Data)
                            {
                                //Build up a contact for the response list
                                EntryType entryType;
                                Enum.TryParse(y.Name, out entryType);
                                var entry = new EntryDto
                                {
                                    Name = entryType,
                                    PhoneNumber = y.PhoneNumber
                                };

                                //Check the contact type and populate the number of said type
                                switch (entryType)
                                {
                                    case EntryType.CellPhoneNumber:
                                        phoneContactResponseDto.CellPhoneNumber = y.PhoneNumber;
                                        break;
                                    case EntryType.HomePhoneNumber:
                                        phoneContactResponseDto.HomePhoneNumber = y.PhoneNumber;
                                        break;
                                    default:
                                        phoneContactResponseDto.WorkPhoneNumber = y.PhoneNumber;
                                        break;
                                }
                            }
                        }
                        //Add the built up contact to the response list of contacts
                        phoneContactResponseDtoList.Add(phoneContactResponseDto);
                    }
                }

                //Return positive result
                return await Task.FromResult(SystemResult<List<PhoneContactResponseDto>>.Success(phoneContactResponseDtoList, "Success", ""));
            }
            catch(Exception ex)
            {
                //Send error result
                return await Task.FromResult(new SystemResult<List<PhoneContactResponseDto>>(ex.Message));
            }
        }

        /// <summary>
        /// This method is to fetch the phone list from the database
        /// </summary>
        /// <returns>System result with all names for the phone book</returns>
        private async Task<SystemResult<List<PhoneBookList>>> GetPhoneBookList(string name = "")
        {
            try
            {
                List<PhoneBookList> list = new List<PhoneBookList>();
                if (string.IsNullOrEmpty(name))
                {
                    list = await _phoneBookListRepository.GetAll();
                }
                else
                {
                    list = await _phoneBookListRepository.GetByName(name);
                }
                //Return positive result
                return await Task.FromResult(SystemResult<List<PhoneBookList>>.Success(list, "Success", ""));
            }
            catch(Exception ex)
            {
                //Send error result
                return await Task.FromResult(new SystemResult<List<PhoneBookList>>(ex.Message));
            }
        }

        /// <summary>
        /// This method is to return all the numbers of the phone book entry by id
        /// </summary>
        /// <param name="phoneBookId"></param>
        /// <returns>System result with the numbers of a particular contact</returns>
        private async Task<SystemResult<List<Entry>>> GetListEntries(int phoneBookId)
        {
            try
            {
                var entries = await _phoneBookEntryRepository.GetByPhoneBookId(phoneBookId);
                //Return positive result
                return await Task.FromResult(SystemResult<List<Entry>>.Success(entries, "Success", ""));
            }
            catch (Exception ex)
            {
                //Send error result
                return await Task.FromResult(new SystemResult<List<Entry>>(ex.Message));
            }
        }
    }
}
