using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class MailingList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MailingList_ID { get; set; }
        [StringLength(150)]
        [Display(Description = "Required: No, Max Length: 128 (string), For example: xyz@example.com, Regular Expression Validation: ^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$")]

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress]
        public string Email { get; set; }
        public string Referer { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Active { get; set; }
    }
    public class Mailing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MailingList_ID { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        public string Referer { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class MailingListResponses : APIResponses
    {
        public MailingList MailingLists { get; set; }
    }
    public class EmailPreferenceAPIResponse : APIResponses
    {
        public bool EmailPreference { get; set; }
    }
}
