using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookAPI.Models
{
    public class ContactGroup : IValidatableObject
    {
        [Required(ErrorMessage = "Required FirstName ")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string PhoneNumber { get; set; }
        public int? AssociatedId { get; set; }
        public bool IsContactGroup { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AssociatedId != null && AssociatedId != 0 && IsContactGroup == false)
            {
                yield return new ValidationResult("IsContactGroup should be true");
            }
        }
    }

    public class ContactGroupResponse
    {
        public Guid id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int? AssociatedId { get; set; }
        public bool IsContactGroup { get; set; }
    }
}
