using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class Rules
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Rule_Id { get; set; }
        public string Game_Type { get; set; }
        public string Rule_Title { get; set; }
        public string Rule_Content { get; set; }
    }

    public class RulesAPIResponses : APIResponses
    {
        public Rules Rule { get; set; }
    }

    public class RuleListAPIResponses : APIResponses
    {
        public List<Rules> Rules { get; set; }
    }

}
