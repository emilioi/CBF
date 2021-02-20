using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class Team_List
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamID { get; set; }
        [Required(ErrorMessage = " SportType is required")]
        public int SportType { get; set; }
        [Required(ErrorMessage = " TeamName is required")]
        [StringLength(50)]
        public String TeamName { get; set; }
        [Required(ErrorMessage = " TeamNameShort is required")]
        [StringLength(50)]
        public String TeamNameShort { get; set; }
        [Required(ErrorMessage = " TeamLogo is required")]
        [StringLength(50)]
        public String TeamLogo { get; set; }
        public bool Is_Deleted { get; set; }
        public int Last_Edited_By { get; set; }
        public DateTime DTS { get; set; }
    }
    public class TeamListApi
    {
        public int TeamID { get; set; }
        [Required(ErrorMessage = " SportType is required")]
        public string SportType { get; set; }
        [Required(ErrorMessage = " TeamName is required")]
        [StringLength(50)]
        public String TeamName { get; set; }
        [Required(ErrorMessage = " TeamNameShort is required")]
        [StringLength(50)]
        public String TeamNameShort { get; set; }
       

        //public IFormFile TeamLogo { get; set; }
    }
    public class TeamListResponses : APIResponses
    {
        public NFLTeam TeamList { get; set; }
    }
    public class TeamListResponsesList : APIResponses
    {
        public List<NFLTeam> TeamList { get; set; }
    }
    public class TeamUpload
    {

        public int TeamID { get; set; }
        public string TeamLogo { get; set; }
        public string Logo { get; set; }
    }
    public class TeamUploadResponses : APIResponses
    {
        public string FileName { get; set; }
    }
}
