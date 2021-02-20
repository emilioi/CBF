using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class Email_Templates
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(55),Required]
        public string EmailID { get; set; }
        [StringLength(250)]
        public string Purpose { get; set; }
        [StringLength(225)]
        public string FromAddress { get; set; }
        [StringLength(225)]
        public string Subject { get; set; }
        public string Body { get; set; }
        [StringLength(250)]
        public string  Cc { get; set; }
        [StringLength(250)]
        public string Bcc { get; set; }
        public short Importance { get; set; }
        public short BodyFormat { get; set; }
        public short MailFormat { get; set; }
    }
    public class EmailAPIResponses : APIResponses
    {
        public Email_Templates Email_Templates { get; set; }
    }
    public class EmailListAPIResponses : APIResponses
    {
        public List<Email_Templates> Email_Templates { get; set; }
    }
   

    
}
