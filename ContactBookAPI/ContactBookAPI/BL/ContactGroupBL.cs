using ContactBookAPI.Models;
using ContactBookAPI.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ContactBookAPI.BL
{
    public class ContactGroupBL : IContactGroupBL
    {
        private readonly IContactGroupDBService<dynamic> contactGroupDBRepository;

        public ContactGroupBL(IContactGroupDBService<dynamic> contactGroupDBRepository)
        {
            this.contactGroupDBRepository = contactGroupDBRepository;
        }

        public async Task<List<ContactGroupResponse>> GetContactGroupsAsync()
        {
            var query = $"SELECT c.id, c.FirstName, c.LastName, c.PhoneNumber, c.IsContactGroup, c.AssociatedId  FROM c order by c._ts desc";
            var result = await this.contactGroupDBRepository.GetItemsAsync(query);
            var json = JsonConvert.SerializeObject(result);
            return JsonConvert.DeserializeObject<IReadOnlyList<ContactGroupResponse>>(json).ToList();
        }

        public async Task<List<ContactGroupResponse>> SearchContactGroupsAsync(ContactSearchRequest request)
        {
            var query = $"SELECT c.id, c.FirstName, c.LastName, c.PhoneNumber, c.IsContactGroup, c.AssociatedId FROM c where c.FirstName like '%" + request.SearchCriteria + "%' or c.LastName like '%" + request.SearchCriteria + "%' order by c._ts desc OFFSET " + request.PageIndex + " LIMIT " + request.PageSize + " ";
            var result = await this.contactGroupDBRepository.GetItemsAsync(query);
            var json = JsonConvert.SerializeObject(result);
            return JsonConvert.DeserializeObject<IReadOnlyList<ContactGroupResponse>>(json).ToList();
        }

        public async Task<bool> CreateContactGroupAsync(ContactGroup group)
        {
            try
            {
                var query = $"SELECT c.id FROM c where c.PhoneNumber = " + group.PhoneNumber + " and c.FirstName = '" + group.FirstName + "' ";
                var result = await this.contactGroupDBRepository.GetItemAsync(query);
                if (result != null)
                {
                    return false;
                }

                await contactGroupDBRepository.CreateItemAsync(group);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateContactGroupAsync(Guid id, ContactGroup contactGroup)
        {
            try
            {
                var query = $"SELECT c.id FROM c where c.id = '" + id + "'";
                var result = await this.contactGroupDBRepository.GetItemAsync(query);
                if (result != null)
                {
                    ContactGroupResponse response = new ContactGroupResponse
                    {
                        id = id,
                        FirstName = contactGroup.FirstName,
                        LastName = contactGroup.LastName,
                        PhoneNumber = contactGroup.PhoneNumber,
                        AssociatedId = contactGroup.AssociatedId,
                        IsContactGroup = contactGroup.IsContactGroup,
                    };
                    await contactGroupDBRepository.UpdateItemAsync(id.ToString(), response);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveContactGroupAsync(Guid id)
        {
            try
            {
                var query = $"SELECT c.id FROM c where c.id = '" + id + "'";
                var result = await this.contactGroupDBRepository.GetItemAsync(query);
                if (result != null)
                {
                    await this.contactGroupDBRepository.DeleteItemAsync(id.ToString());
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
