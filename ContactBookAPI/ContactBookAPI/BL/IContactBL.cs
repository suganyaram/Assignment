using ContactBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookAPI.BL
{
    public interface IContactBL
    {
        Task<List<ContactResponse>> GetContactsAsync();
        Task<List<ContactResponse>> SearchContactsAsync(ContactSearchRequest request);
        Task<bool> CreateContactAsync(AddContactRequest contact);
        Task<bool> UpdateContactAsync(Guid id, Contact contact);
        Task<bool> RemoveContactAsync(Guid id);
    }
}
