using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Models
{
    public class Pool_Master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pool_ID { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = " Pool Name is required")]
        public string Pool_Name { get; set; }
        [Required(ErrorMessage = " Sport Type is required")]
        public int Sport_Type { get; set; }
        [StringLength(250)]
        //[Required(ErrorMessage = " Rules URL is required")]
        public string Rules_URL { get; set; }
        [Required(ErrorMessage = " Price is required")]
        public decimal Price { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        // [Required(ErrorMessage = " Is Active is required")]
        public bool Is_Active { get; set; }
        [Required(ErrorMessage = " Cut Off is required")]
        public DateTime Cut_Off { get; set; }
        public bool Is_Started { get; set; }
        public bool Is_Deleted { get; set; }
        public int Last_Edited_By { get; set; }
        public DateTime DTS { get; set; }
        public bool Ruler_Season { get; set; }
        public bool Private { get; set; }
        public bool ThrusdayGames { get; set; }
        public bool SaturdayGames { get; set; }
        public bool SundayGames { get; set; }
        public string PassCode { get; set; }
        [NotMapped]
        public string CutOffDateString
        {
            get
            {
                return Cut_Off.ToString("dddd, dd MMMM yyyy");
            }
        }
        public string Image_Url { get; set; }

        public string SportTypeName
        {
            get
            {
                if (Sport_Type == 1)
                {
                    return "NFL";
                }
                else if (Sport_Type == 2)
                {
                    return "NHL";
                }
                else
                {
                    return "";
                }
            }
        }

    }
    public class Pool_MasterResponses : APIResponses
    {
        public Pool_Master Pool_Master { get; set; }
    }
    public class PoolMaintaince
    {

        public int Pool_ID { get; set; }
        [Required(ErrorMessage = " Pool Name is required")]
        public string Pool_Name { get; set; }
        [Required(ErrorMessage = " Sport Type is required")]
        public int Sport_Type { get; set; }

        [Required(ErrorMessage = " Rules URL is required")]
        [StringLength(250)]
        public string Rules_URL { get; set; }
        [Required(ErrorMessage = " Price is required")]
        public decimal Price { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [Required(ErrorMessage = " Cut Off is required")]
        public DateTime Cut_Off { get; set; }
        public bool Is_Started { get; set; }
        public bool Ruler_Season { get; set; }
        public bool Private { get; set; }
        public bool ThrusdayGames { get; set; }
        public bool SaturdayGames { get; set; }
        public string PassCode { get; set; }
        public string Image_Url { get; set; }
    }
    public class PoolMaintainceResponses : APIResponses
    {
        public PoolMaintaince Maintaince { get; set; }
    }
    public class PoolMaintainceMappedResponses : APIResponses
    {
        public List<SurvPoolWeekListMapped> PoolMapped { get; set; }
    }
    public class PoolMaintainceListResponses : APIResponses
    {
        public List<Pool_Master> Pool_Master { get; set; }
    }
    //public class Pool_MasterResponses : APIResponses
    //{
    //    public Pool_Master Maintaince { get; set; }
    //}
    [NotMapped]
    public class Club
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pool_ID { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = " Pool Name is required")]
        public string Pool_Name { get; set; }
        [Required(ErrorMessage = " Sport Type is required")]
        public int Sport_Type { get; set; }
        [StringLength(250)]
        //[Required(ErrorMessage = " Rules URL is required")]
        public string Rules_URL { get; set; }
        [Required(ErrorMessage = " Price is required")]
        public decimal Price { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        // [Required(ErrorMessage = " Is Active is required")]
        public bool Is_Active { get; set; }
        [Required(ErrorMessage = " Cut Off is required")]
        public DateTime Cut_Off { get; set; }
        public bool Is_Started { get; set; }
        public bool Is_Deleted { get; set; }
        public int Last_Edited_By { get; set; }
        public DateTime DTS { get; set; }
        public bool Ruler_Season { get; set; }
        public bool Private { get; set; }
        public string PassCode { get; set; }
        [NotMapped]
        public string SportTypeName { get; set; }
        [NotMapped]
        public List<PoolWeekList> PoolWeekLists { get; set; }
        [NotMapped]
        public string CutOffDateString { get; set; }
        public string Image_Url { get; set; }
        public int? TicketCount { get; set; }
    }
    [NotMapped]
    public class PicksCount
    {
        [Key]
        public int EntryID { get; set; }
        public int PoolID { get; set; }
        public int NumOfPicks { get; set; }
    }
    public class EntryWithoutPicks {
        public string EntryName { get; set; }
    }

    [NotMapped]
    public class PoolWeekList
    {
        [Key]
        public int WeekNumber { get; set; }
        public int EntriesCount { get; set; }
        public int MembersCount { get; set; }
        public int PicksMadeCount { get; set; }
        public int NoPicksCount { get; set; }
        public int EliminatedCount { get; set; }
        public int AliveCount { get; set; }
        public List<MostPickedList> MostPickedTeams { get; set; }
    }
    [NotMapped]
    public class MostPickedTeam : NFLTeam
    {
        public int PickCount { get; set; }
    }
    [NotMapped]
    public class PoolListResponse : APIResponses

    {
        public List<Club> clubs { get; set; }
    }
    [NotMapped]
    public class MostPickedList
    {
        //[Key]
        //[Column(Order = 0)]
        public int Pool_Id { get; set; }
        //[Key]
        //[Column(Order = 1)]
        public int Team_Id { get; set; }
        public int SportType { get; set; }
        public string SportTypeName { get; set; }
        public string Team_Name { get; set; }
        public string Abbreviation { get; set; }
        public string LogoImageSrc { get; set; }
        public int PickCount { get; set; }

    }
    public class Pool_Master_Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pool_ID { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = " Pool Name is required")]
        public string Pool_Name { get; set; }
        [Required(ErrorMessage = " Sport Type is required")]
        public int Sport_Type { get; set; }
        [StringLength(250)]
        //[Required(ErrorMessage = " Rules URL is required")]
        public string Rules_URL { get; set; }
        [Required(ErrorMessage = " Price is required")]
        public decimal Price { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        // [Required(ErrorMessage = " Is Active is required")]
        public bool Is_Active { get; set; }
        [Required(ErrorMessage = " Cut Off is required")]
        public string Cut_Off { get; set; }
        public bool Is_Started { get; set; }
        public bool Is_Deleted { get; set; }
        public int Last_Edited_By { get; set; }
        public DateTime DTS { get; set; }
        public bool Ruler_Season { get; set; }
        public bool Private { get; set; }
        public bool ThrusdayGames { get; set; }
        public bool SaturdayGames { get; set; }
        public bool SundayGames { get; set; }
        public string PassCode { get; set; }


    }
}
