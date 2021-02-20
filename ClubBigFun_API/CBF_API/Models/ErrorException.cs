using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBF_API.Models
{
    public class ErrorException
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Log_id { get; set; }
        public DateTime date { get; set; }
        public string Thread { get; set; }
        public string Context { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public int User_Id { get; set; }
        public string Url { get; set; }

    }

    public class ErrorExceptionListResponse : APIResponses
    {
       public List<ErrorExceptionGroupBy> ErrorList { get; set; }
    }

    public class ErrorExceptionGroupBy
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public int ErrorCount { get; set; }
        

    }
 
}
