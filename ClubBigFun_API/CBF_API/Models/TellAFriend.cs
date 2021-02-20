using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class TellAFriend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [StringLength(256)]
        public string FriendName { get; set; }
        [StringLength(256)]
        public string FriendEmail { get; set; }
        [StringLength(256)]
        public string YourName { get; set; }
        [StringLength(256)]
        public string YourEmail { get; set; }
        [StringLength(256)]
        public string Referer { get; set; }
        public DateTime CreatedDT { get; set; }
        public bool Active { get; set; }
    }
    public class TellAFriendResponses : APIResponses
    {
        public TellAFriend TellFriend { get; set; }
    }
    public class TellAFriendListResponses : APIResponses
    {
        public List<TellAFriend> TellFriend { get; set; }
    }
}

