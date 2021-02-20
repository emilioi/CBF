using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class Email_Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Notification { get; set; }
        [StringLength(100)]
        public string To_Email { get; set; }
        [StringLength(500)]
        public string Subject { get; set; }
        public int Member_Id { get; set; }
        public string Email_Content { get; set; }
        [StringLength(100)]
        public string From_Email { get; set; }
        public bool Is_Sent { get; set; }
        [StringLength(500)]
        public string Failed_Error { get; set; }
        public DateTime? Sent_On { get; set; }
        public DateTime DTS { get; set; }
         
    }
    public class Email_NotificationResponses: APIResponses
    {
        public Email_Notification Email_Notification { get; set; }
    }
    public class EmailSend
    {
        public int Template_Id { get; set; }
        [StringLength(100)]
        public string To_Email { get; set; }
        [StringLength(500)]
        public string Subject { get; set; }
        [StringLength(100)]
        public string From_Email { get; set; }
        public string Email_Content { get; set; }
    }
    public class EmailSendResponses : APIResponses
    {
        public  EmailSend Email { get; set; }
    }
    }
