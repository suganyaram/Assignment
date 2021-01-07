using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactBookAPI.BL;
using ContactBookAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactBookAPI.Controllers
{
    [Route("api/contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactBL contactBL;

        public ContactController(IContactBL contactBL)
        {
            this.contactBL = contactBL;
        }

        // GET: api/<ContactController>
        [HttpGet("GetContacts")]
        public async Task<List<ContactResponse>> Get()
        {
            return await contactBL.GetContactsAsync();
        }

        [HttpPost("SearchContact")]
        public async Task<List<ContactResponse>> SearchContacts([FromBody] ContactSearchRequest request)
        {
            return await contactBL.SearchContactsAsync(request);
        }

        [HttpPost("CreateContact")]
        public async Task<IActionResult> Create(AddContactRequest request)
        {
            var response = await contactBL.CreateContactAsync(request);
            if (!response)
            {
                return BadRequest("Dupliate Contacts");
            }
            return Ok(response);
        }

        [HttpPut("UpdateContact")]
        public async Task<IActionResult> Put(Guid id, Contact request)
        {
            var response = await contactBL.UpdateContactAsync(id, request);
            if (!response)
            {
                return BadRequest("id not exist");
            }
            return Ok(response);
        }

        [HttpDelete("RemoveContact")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await contactBL.RemoveContactAsync(id);
            if (!response)
            {
                return BadRequest("id not exist");
            }
            return Ok(response);
        }
    }
}
