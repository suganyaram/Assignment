using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookAPI.Models
{
    public class ContactSearchRequest
    {
        public string SearchCriteria { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 100;
    }
}
