using ContactBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookAPI.BL
{
    public interface IContactGroupBL
    {
        Task<bool> CreateContactGroupAsync(ContactGroup groups);
        Task<List<ContactGroupResponse>> SearchContactGroupsAsync(ContactSearchRequest request);
        Task<List<ContactGroupResponse>> GetContactGroupsAsync();
        Task<bool> UpdateContactGroupAsync(Guid id, ContactGroup contactGroup);
        Task<bool> RemoveContactGroupAsync(Guid id);
    }
}
