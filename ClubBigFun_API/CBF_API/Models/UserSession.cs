using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class UserSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 TokenID { get; set; }
        public int UserID { get; set; }
        public string SessionGUID { get; set; }
        public string UserHostIPAddress { get; set; }
        public string RequestBrowsertypeVersion { get; set; }
        public string BrowserUniqueID { get; set; }
        public string AppDomainName { get; set; }
        public string Key { get; set; }
        public DateTime? IssuedOn { get; set; }
        public bool Expired { get; set; }
        public DateTime? ExpiredOn { get; set; }
    }
    public class Token
    {

        public int UserId { get; set; }
        public Int64 TokenId { get; set; }
        public string UserHostIPAdress { get; set; }
        public string RequestBrowsertypeVersion { get; set; }
    }


    public class UserSessionResponse : APIResponses
    {
        public List<UserSession> UserSession { get; set; }
    }
}
