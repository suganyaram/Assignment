using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ContactBookAPI.BL;
using ContactBookAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookAPI.Controllers
{
    [Route("api/ContactGroup")]
    [ApiController]
    public class ContactGroupController : ControllerBase
    {
        private readonly IContactGroupBL contactGroupBL;
        public ContactGroupController(IContactGroupBL contactGroupBL)
        {
            this.contactGroupBL = contactGroupBL;
        }

        [HttpGet("GetContactGroups")]
        public async Task<IActionResult> Get()
        {
            var response = await contactGroupBL.GetContactGroupsAsync();
            if (response.Count > 0)
            {
                return Ok(response);
            }
            return NoContent();
        }

        [HttpPost("SearchContact")]
        public async Task<IActionResult> SearchContact([FromBody] ContactSearchRequest request)
        {
            var response = await contactGroupBL.SearchContactGroupsAsync(request);
            if (response.Count > 0)
            {
                return Ok(response);
            }
            return NoContent();
        }

        [HttpPost("CreateContactGroup")]
        public async Task<IActionResult> Create(ContactGroup request)
        {
            var response = await contactGroupBL.CreateContactGroupAsync(request);
            if (!response)
            {
                return BadRequest("Duplicate Contacts Exist");
            }
            return Ok(response);
        }

        [HttpPut("UpdateContactGroup")]
        public async Task<IActionResult> Put(Guid id, ContactGroup request)
        {
            var response = await contactGroupBL.UpdateContactGroupAsync(id, request);
            if (!response)
            {
                return BadRequest("id not exist");
            }
            return Ok(response);
        }

        [HttpDelete("RemoveContactGroup")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await contactGroupBL.RemoveContactGroupAsync(id);
            if (!response)
            {
                return BadRequest("id not exist");
            }
            return Ok(response);
        }
    }
}
