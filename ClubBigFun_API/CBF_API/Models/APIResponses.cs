using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class APIResponses
    {

        public string Status { get; set; }
        public string Message { get; set; }
        public string ResponseReason { get; set; }

    }
    public class ListStringAPIResponse : APIResponses
    {
        public List<EntryWithoutPicks> ListString { get; set; }
    }
    public class APIRequest
    {
        public string LoginKey { get; set; }
        public int User_Id { get; set; }
    }

    public class FilterSortingRequest
    {
        public bool IsFilter { get; set; }
        public bool IsSorting { get; set; }
        public bool IsAscending { get; set; }
        public string ShortByName { get; set; }
        public string FilterByName { get; set; }
        public string FilterByValue { get; set; }
    }

    public class LoginClientRequest
    {
        public string OSVersion { get; set; }
        public string IPAddress { get; set; }
        public string BrowserVersion { get; set; }
        public string URL { get; set; }
        public string UserType { get; set; }
        public string AppDomainName { get; set; }
    }
}
