using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class Sports_Type
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SportType { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = " SportTypeName is required")]
        public string SportTypeName { get; set; }
    }
    public class SportsTypeResponses : APIResponses
    {
        public Sports_Type SportsType { get; set; }

    }
    public class SportsListTypeResponses : APIResponses
    {
        public List<Sports_Type> SportsType { get; set; }
    }
}
