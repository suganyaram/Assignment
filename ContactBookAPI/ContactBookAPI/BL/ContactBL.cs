using ContactBookAPI.Models;
using ContactBookAPI.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ContactBookAPI.BL
{
    public class ContactBL : IContactBL
    {
        private readonly ICosmosDBService<dynamic> cosmosDBRepository;

        public ContactBL(ICosmosDBService<dynamic> cosmosDBRepository)
        {
            this.cosmosDBRepository = cosmosDBRepository;
        }

        public async Task<List<ContactResponse>> GetContactsAsync()
        {
            var query = $"SELECT c.id, c.firstname, c.lastname, c.phonenumber FROM c order by c._ts desc";
            var result = await this.cosmosDBRepository.GetItemsAsync(query);
            var json = JsonConvert.SerializeObject(result);
            return JsonConvert.DeserializeObject<IReadOnlyList<ContactResponse>>(json).ToList();
        }

        public async Task<List<ContactResponse>> SearchContactsAsync(ContactSearchRequest request)
        {
            var query = $"SELECT c.id, c.firstname, c.lastname, c.phonenumber FROM c where c.firstname like '%" + request.SearchCriteria + "%' order by c._ts desc OFFSET " + request.PageIndex + " LIMIT " + request.PageSize + " ";
            var result = await this.cosmosDBRepository.GetItemsAsync(query);
            var json = JsonConvert.SerializeObject(result);
            return JsonConvert.DeserializeObject<IReadOnlyList<ContactResponse>>(json).ToList();
        }

        public async Task<bool> CreateContactAsync(AddContactRequest contact)
        {
            try
            {
                var query = $"SELECT c.id FROM c where c.firstname ='" + contact.FirstName + "' and c.phonenumber ='" + contact.PhoneNumber + "' ";
                var result = await this.cosmosDBRepository.GetItemAsync(query);
                if (result != null)
                {
                    return false;
                }
                var payload = new
                {
                    firstname = contact.FirstName,
                    lastname = contact.LastName ?? string.Empty,
                    phonenumber = contact.PhoneNumber,
                };
                await cosmosDBRepository.CreateItemAsync(payload);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateContactAsync(Guid id, Contact contact)
        {
            try
            {
                var query = $"SELECT c.id FROM c where c.id ='" + id + "'";
                var result = await this.cosmosDBRepository.GetItemAsync(query);
                if (result != null)
                {
                    var response = new 
                    {
                        id = id,
                        firstname = contact.FirstName,
                        lastname = contact?.LastName ?? string.Empty,
                        phonenumber = contact.PhoneNumber,
                    };
                    await cosmosDBRepository.UpdateItemAsync(id.ToString(), response);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveContactAsync(Guid id)
        {
            try
            {
                var query = $"SELECT c.id FROM c where c.id ='" + id + "'";
                var result = await this.cosmosDBRepository.GetItemAsync(query);
                if (result != null)
                {
                    await this.cosmosDBRepository.DeleteItemAsync(id.ToString());
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
