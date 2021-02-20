using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class SurvEntries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryID { get; set; }
        public int MemberID { get; set; }
        public int PoolID { get; set; }
        [StringLength(50)]
        public string EntryName { get; set; }
        //[StringLength(50)]
        //public string Prefix { get; set; }
        public bool Eliminated { get; set; }
        public bool Active { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedDT { get; set; }

        public int  Defaults{ get; set; }
        
    }
}
