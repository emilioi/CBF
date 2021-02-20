using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBF_API.Models
{
    public class Lookups
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Lookup_Id { get; set; }
        [StringLength(80)]
        public string Lookup_Type { get; set; }
        [StringLength(80)]
        public string Lookup_Name { get; set; }
        [StringLength(80)]
        public string Lookup_Value { get; set; }
        public bool Is_Deleted { get; set; }
    }
    public class LookupsWeatherListResponse : APIResponses
    {
        public List<Lookups> WeatherList { get; set; }
    }
    public class ClubSettingResponse : APIResponses
    {
        public List<Lookups> settings { get; set; }
    }

    public class MaintenanceRequest
    {
        public bool MaintenanceOn { get; set; }
        public string MaintenanceText { get; set; }
    }

    public class States
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        public int Country_ID { get; set; }
    }
    public class StatesResponses : APIResponses
    {
        public List<States> States { get; set; }
    }
    public class Countries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(3)]
        public string SortName { get; set; }
        public int PhoneCode { get; set; }
    }
    public class CountriesResponses : APIResponses
    {
        public List<Countries> Countries { get; set; }
    }
}
