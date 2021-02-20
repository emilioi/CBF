using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CBF_API.Models;
using Newtonsoft.Json;

namespace CBF_API.Helpers
{
    public class NFL_API_Helper
    {
        private readonly CbfDbContext _context;

        public NFL_API_Helper(CbfDbContext context)
        {
            _context = context;
        }

        public bool Get_NFLSchedule_API()
        {
            //Remove data from Table
            //_context.NFLSchedule.RemoveRange(_context.NFLSchedule);
            //_context.SaveChanges();



            List<NFLSchedule> schedules = new List<NFLSchedule>();

            string MYSPORTSFEEDS = "MYSPORTSFEEDS";
            string url = ApiConfig.Url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

            request.Headers.Add("Authorization", "Basic " + GNF.Base64Encode(GlobalConfig.APIKey
          + ":" + MYSPORTSFEEDS));

            request.Method = "GET";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader objSR = new StreamReader(stream, encode, true);
                    string strResponse = objSR.ReadToEnd();
                    dynamic list = JsonConvert.DeserializeObject<dynamic>(strResponse);
                    try
                    {


                        dynamic games = list.games;
                        foreach (dynamic game in games)
                        {
                            var obj = game.schedule;
                            try
                            {

                                NFLSchedule schedule = new NFLSchedule();

                                schedule.Id = obj.id;

                                schedule.Week = obj.week;
                                var dtTemp = obj.startTime;
                                schedule.StartTime = obj.startTime;
                                try
                                {
                                    var timeUtc = DateTime.ParseExact(schedule.StartTime, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                                    var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                                    schedule.GameDate = today;
                                    schedule.CutOffDate = GetCutOffDate(schedules, schedule.Week, schedule.StartTime, schedule.GameDate);
                                    schedule.StartTime = today.ToString();

                                }
                                catch (Exception ex) { }


                                schedule.HomeTeamId = obj.homeTeam.id;
                                schedule.HomeTeamShort = obj.homeTeam.abbreviation;

                                schedule.VisitingTeamID = obj.awayTeam.id;
                                schedule.VisitingTeamShort = obj.awayTeam.abbreviation;

                                schedule.Venue_ID = obj.venue.id;

                                schedule.EndedTime = obj.endedTime;
                                schedule.Weather = obj.weather;
                                schedule.PlayedStatus = obj.playedStatus;
                                schedule.ScheduleStatus = obj.scheduleStatus;
                                schedule.VenueAllegiance = obj.venueAllegiance;


                                //  schedule.officials = obj.offici
                                schedule.SeasonCode = ApiConfig.SeasonCode;
                                schedules.Add(schedule);
                                _context.NFLSchedule.Add(schedule);
                                _context.Entry(schedule).State = Microsoft.EntityFrameworkCore.EntityState.Added;


                                //  var sch = JsonConvert.DeserializeObject(obj);
                            }
                            catch (Exception ex)
                            { throw ex; }


                        }
                        try
                        {
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        { throw ex; }

                    }
                    catch (Exception ex)
                    {
                        throw ex;

                    }
                    return true;
                    //process the response
                }
            }

        }
        public bool Get_NFLScore_API()
        {
            //Remove data from Table
            //_context.NFLScore.RemoveRange(_context.NFLScore);
            //_context.SaveChanges();


            List<NFLSchedule> schedules = new List<NFLSchedule>();

            string MYSPORTSFEEDS = "MYSPORTSFEEDS";
            string url = "https://api.mysportsfeeds.com/v2.1/pull/nfl/2019-regular/games.json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

            request.Headers.Add("Authorization", "Basic " + GNF.Base64Encode(GlobalConfig.APIKey
          + ":" + MYSPORTSFEEDS));

            request.Method = "GET";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader objSR = new StreamReader(stream, encode, true);
                    string strResponse = objSR.ReadToEnd();
                    dynamic list = JsonConvert.DeserializeObject<dynamic>(strResponse);
                    try
                    {


                        dynamic games = list.games;
                        foreach (dynamic game in games)
                        {
                            var obj = game.schedule;

                            dynamic score = game.score;
                            int ScheduleID = Convert.ToInt32(obj.id);
                            NFLScore objScore = _context.NFLScore.FirstOrDefault(x => x.Schedule_Id == ScheduleID);
                            if (objScore != null)
                            {
                                objScore.AwayScoreTotal = score.awayScoreTotal;

                                //objScore.CurrentDown = score.currentDown;
                                //objScore.CurrentIntermission = score.currentIntermission;
                                //objScore.CurrentQuarter = score.currentQuarter;
                                //objScore.CurrentQuarterSecondsRemaining = score.currentQuarterSecondsRemaining;
                                //objScore.CurrentYardsRemaining = score.currentYardsRemaining;
                                objScore.HomeScoreTotal = score.homeScoreTotal;
                                //objScore.LineOfScrimmage = score.lineOfScrimmage;
                                //objScore.Quarters = score.quarters == null ? score.quarters.ToString() : "";

                                //objScore.TeamInPossession = score.teamInPossession;

                                objScore.Schedule_Id = obj.id;


                                _context.NFLScore.Add(objScore);
                                _context.Entry(objScore).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            }
                            else
                            {
                                objScore = new NFLScore();
                                objScore.AwayScoreTotal = score.awayScoreTotal;

                                objScore.CurrentDown = score.currentDown;
                                objScore.CurrentIntermission = score.currentIntermission;
                                objScore.CurrentQuarter = score.currentQuarter;
                                objScore.CurrentQuarterSecondsRemaining = score.currentQuarterSecondsRemaining;
                                objScore.CurrentYardsRemaining = score.currentYardsRemaining;
                                objScore.HomeScoreTotal = score.homeScoreTotal;
                                objScore.LineOfScrimmage = score.lineOfScrimmage;
                                objScore.Quarters = score.quarters == null ? score.quarters.ToString() : "";

                                objScore.TeamInPossession = score.teamInPossession;

                                objScore.Schedule_Id = obj.id;


                                _context.NFLScore.Add(objScore);
                                _context.Entry(objScore).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            }


                        }
                        try
                        {
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        { throw ex; }

                    }
                    catch (Exception ex)
                    {
                        throw ex;

                    }
                    return true;
                    //process the response
                }
            }

        }
        public bool Get_NFLTeam_API()
        {
            //Remove data from Table
            _context.NFLTeam.RemoveRange(_context.NFLTeam);
            _context.SaveChanges();


            List<NFLSchedule> schedules = new List<NFLSchedule>();

            string MYSPORTSFEEDS = "MYSPORTSFEEDS";
            string url = "https://api.mysportsfeeds.com/v2.1/pull/nfl/2019-regular/games.json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

            request.Headers.Add("Authorization", "Basic " + GNF.Base64Encode(GlobalConfig.APIKey
          + ":" + MYSPORTSFEEDS));

            request.Method = "GET";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader objSR = new StreamReader(stream, encode, true);
                    string strResponse = objSR.ReadToEnd();
                    dynamic list = JsonConvert.DeserializeObject<dynamic>(strResponse);
                    try
                    {



                        //Team 
                        List<NFLTeam> teams = new List<NFLTeam>();
                        int SportType = _context.Sports_Type.FirstOrDefault(x => x.SportTypeName == "NFL").SportType;
                        dynamic teamReferences = list.references.teamReferences;
                        foreach (var obj in teamReferences)
                        {
                            try
                            {

                                NFLTeam team = new NFLTeam();
                                team.Team_Id = obj.id;
                                team.City = obj.city;
                                team.Team_Name = obj.name;
                                team.Abbreviation = obj.abbreviation;
                                team.Venue_ID = obj.homeVenue.id;
                                team.LogoImageSrc = obj.city + " " + obj.name + ".gif";
                                team.SportType = SportType.ToString();
                                _context.NFLTeam.Add(team);
                                _context.Entry(team).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        try
                        {
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        { throw ex; }

                    }
                    catch (Exception ex)
                    {
                        throw ex;

                    }
                    return true;
                    //process the response
                }
            }

        }
        public bool Get_NFLVenue_API()
        {
            //Remove data from Table
            _context.NFLVenue.RemoveRange(_context.NFLVenue);
            _context.SaveChanges();


            List<NFLSchedule> schedules = new List<NFLSchedule>();

            string MYSPORTSFEEDS = "MYSPORTSFEEDS";
            string url = "https://api.mysportsfeeds.com/v2.1/pull/nfl/2019-regular/games.json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

            request.Headers.Add("Authorization", "Basic " + GNF.Base64Encode(GlobalConfig.APIKey
          + ":" + MYSPORTSFEEDS));

            request.Method = "GET";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader objSR = new StreamReader(stream, encode, true);
                    string strResponse = objSR.ReadToEnd();
                    dynamic list = JsonConvert.DeserializeObject<dynamic>(strResponse);
                    try
                    {



                        //Team 
                        //Venue 
                        List<NFLVenue> venues = new List<NFLVenue>();

                        dynamic venueReferences = list.references.venueReferences;
                        foreach (var obj in venueReferences)
                        {
                            try
                            {

                                NFLVenue venue = new NFLVenue();
                                venue.Venue_ID = obj.id;
                                venue.City = obj.city;
                                venue.Venue_Name = obj.name;
                                venue.Country = obj.country;
                                venue.GeoCoordinates = obj.geoCoordinates != null ? obj.geoCoordinates.ToString() : "";


                                _context.NFLVenue.Add(venue);
                                _context.Entry(venue).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            }
                            catch (Exception ex)
                            { }
                        }
                        try
                        {
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        { throw ex; }

                    }
                    catch (Exception ex)
                    {
                        throw ex;

                    }
                    return true;
                    //process the response
                }
            }

        }
        public bool BoxScoreAPI(string GameStr)
        {
            //List<NFLSchedule> schedules = new List<NFLSchedule>();

            string MYSPORTSFEEDS = "MYSPORTSFEEDS";
            string url = "https://api.mysportsfeeds.com/v2.1/pull/nfl/2019-regular/games/"+GameStr+"/boxscore.json";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

            request.Headers.Add("Authorization", "Basic " + GNF.Base64Encode(GlobalConfig.APIKey
          + ":" + MYSPORTSFEEDS));

            request.Method = "GET";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader objSR = new StreamReader(stream, encode, true);
                    string strResponse = objSR.ReadToEnd();
                    dynamic list = JsonConvert.DeserializeObject<dynamic>(strResponse);
                    try
                    {

                        //foreach (dynamic game in games)
                        //{
                        dynamic game = list.game;

                        int ScheduleID = Convert.ToInt32(game.id);
                        dynamic score = list.scoring;
                        NFLScore objScore = _context.NFLScore.FirstOrDefault(x => x.Schedule_Id == ScheduleID);
                        if (objScore != null)
                        {
                            objScore.AwayScoreTotal = score.awayScoreTotal;

                            objScore.CurrentDown = score.currentDown;
                            objScore.CurrentIntermission = score.currentIntermission;
                            objScore.CurrentQuarter = score.currentQuarter;
                            objScore.CurrentQuarterSecondsRemaining = score.currentQuarterSecondsRemaining;
                            objScore.CurrentYardsRemaining = score.currentYardsRemaining;
                            objScore.HomeScoreTotal = score.homeScoreTotal;
                            objScore.LineOfScrimmage = score.lineOfScrimmage;
                            objScore.Quarters = score.quarters.ToString();

                            objScore.TeamInPossession = score.teamInPossession;

                            objScore.Schedule_Id = game.id;


                            _context.NFLScore.Add(objScore);
                            _context.Entry(objScore).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        }
                        else
                        {
                              objScore = new NFLScore();
                            objScore.AwayScoreTotal = score.awayScoreTotal;

                            objScore.CurrentDown = score.currentDown;
                            objScore.CurrentIntermission = score.currentIntermission;
                            objScore.CurrentQuarter = score.currentQuarter;
                            objScore.CurrentQuarterSecondsRemaining = score.currentQuarterSecondsRemaining;
                            objScore.CurrentYardsRemaining = score.currentYardsRemaining;
                            objScore.HomeScoreTotal = score.homeScoreTotal;
                            objScore.LineOfScrimmage = score.lineOfScrimmage;
                            objScore.Quarters = score.quarters.ToString();

                            objScore.TeamInPossession = score.teamInPossession;

                            objScore.Schedule_Id = game.id;


                            _context.NFLScore.Add(objScore);
                            _context.Entry(objScore).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        }

                        //}
                        try
                        {
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        { throw ex; }


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return true;
                    //process the response
                }
            }
        }


        #region OLD CODE HELPER - DO NOT REMOVE
        public bool RequestSeasonalAPI()
        {

            //_context.NFLSchedule.RemoveRange(_context.NFLSchedule);
            //_context.NFLTeam.RemoveRange(_context.NFLTeam);
            //_context.NFLScore.RemoveRange(_context.NFLScore);
            //_context.NFLVenue.RemoveRange(_context.NFLVenue);
            //_context.SaveChanges();
            List<NFLSchedule> schedules = new List<NFLSchedule>();

            string MYSPORTSFEEDS = "MYSPORTSFEEDS";
            // string url = "https://api.mysportsfeeds.com/v2.1/pull/nfl/2019-regular/games.json";
            //string url = "https://api.mysportsfeeds.com/v2.1/pull/nfl/2018-regular/games.json";
            // string url = "https://api.mysportsfeeds.com/v2.1/pull/nfl/2018-regular/team_gamelogs.json";
            string url = "https://api.mysportsfeeds.com/v2.1/pull/nfl/2019-regular/games/20190905-GB-CHI/boxscore.json";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

            request.Headers.Add("Authorization", "Basic " + GNF.Base64Encode(GlobalConfig.APIKey
          + ":" + MYSPORTSFEEDS));

            request.Method = "GET";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader objSR = new StreamReader(stream, encode, true);
                    string strResponse = objSR.ReadToEnd();
                    dynamic list = JsonConvert.DeserializeObject<dynamic>(strResponse);
                    try
                    {
                        // ProcessAPIData(list);

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return true;
                    //process the response
                }
            }
        }
        public bool RequestBoxScoreAPI()
        {

            // _context.NFLSchedule.RemoveRange(_context.NFLSchedule);
            // _context.NFLTeam.RemoveRange(_context.NFLTeam);
            // _context.NFLVenue.RemoveRange(_context.NFLVenue);
            // _context.SaveChanges();
            List<NFLSchedule> schedules = new List<NFLSchedule>();

            string MYSPORTSFEEDS = "MYSPORTSFEEDS";
            //string url = "https://api.mysportsfeeds.com/v2.1/pull/nfl/2019-regular/games.json";
            //string url = "https://api.mysportsfeeds.com/v2.1/pull/nfl/2018-regular/games.json";
            string url = "https://api.mysportsfeeds.com/v2.1/pull/nfl/2018-regular/team_gamelogs.json";
            // string url = "https://api.mysportsfeeds.com/v2.1/pull/nfl/2017-2018-regular/games/20180907-ATL-PHI/boxscore.json";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

            request.Headers.Add("Authorization", "Basic " + GNF.Base64Encode(GlobalConfig.APIKey
          + ":" + MYSPORTSFEEDS));

            request.Method = "GET";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader objSR = new StreamReader(stream, encode, true);
                    string strResponse = objSR.ReadToEnd();
                    dynamic list = JsonConvert.DeserializeObject<dynamic>(strResponse);
                    try
                    {
                        //ProcessAPIData(list);

                    }
                    catch (Exception ex)
                    {

                    }
                    return true;
                    //process the response
                }
            }
        }
        public void ProcessAPIData(dynamic response)
        {
            List<NFLSchedule> schedules = new List<NFLSchedule>();

            dynamic games = response.games;
            foreach (dynamic game in games)
            {
                var obj = game.schedule;
                try
                {

                    NFLSchedule schedule = new NFLSchedule();

                    schedule.Id = obj.id;
                    // schedule.attendance = obj.attendance;
                    // schedule.broadcasters = obj.broadcasters;
                    // schedule.delayedOrPostponedReason = obj.delayedOrPostponedReason;
                    schedule.Week = obj.week;
                    var dtTemp = obj.startTime;
                    schedule.StartTime = obj.startTime;
                    try
                    {
                        var timeUtc = DateTime.ParseExact(schedule.StartTime, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                        var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                        schedule.GameDate = today;
                        schedule.CutOffDate = GetCutOffDate(schedules, schedule.Week, schedule.StartTime, schedule.GameDate);
                        schedule.StartTime = today.ToString();

                    }
                    catch (Exception ex) { }


                    schedule.HomeTeamId = obj.homeTeam.id;
                    schedule.HomeTeamShort = obj.homeTeam.abbreviation;

                    schedule.VisitingTeamID = obj.awayTeam.id;
                    schedule.VisitingTeamShort = obj.awayTeam.abbreviation;

                    schedule.Venue_ID = obj.venue.id;

                    schedule.EndedTime = obj.endedTime;
                    schedule.Weather = obj.weather;
                    schedule.PlayedStatus = obj.playedStatus;
                    schedule.ScheduleStatus = obj.scheduleStatus;
                    schedule.VenueAllegiance = obj.venueAllegiance;


                    //  schedule.officials = obj.offici

                    schedules.Add(schedule);
                    _context.NFLSchedule.Add(schedule);
                    _context.Entry(schedule).State = Microsoft.EntityFrameworkCore.EntityState.Added;


                    //  var sch = JsonConvert.DeserializeObject(obj);
                }
                catch (Exception ex)
                { throw ex; }
                List<NFLScore> lstScore = new List<NFLScore>();
                dynamic score = game.score;
                NFLScore objScore = new NFLScore();
                objScore.AwayScoreTotal = score.awayScoreTotal;

                objScore.CurrentDown = score.currentDown;
                objScore.CurrentIntermission = score.currentIntermission;
                objScore.CurrentQuarter = score.currentQuarter;
                objScore.CurrentQuarterSecondsRemaining = score.currentQuarterSecondsRemaining;
                objScore.CurrentYardsRemaining = score.currentYardsRemaining;
                objScore.HomeScoreTotal = score.homeScoreTotal;
                objScore.LineOfScrimmage = score.lineOfScrimmage;
                objScore.Quarters = score.quarters.ToString();

                objScore.TeamInPossession = score.teamInPossession;

                objScore.Schedule_Id = obj.id;

                lstScore.Add(objScore);
                _context.NFLScore.Add(objScore);
                _context.Entry(objScore).State = Microsoft.EntityFrameworkCore.EntityState.Added;

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            { throw ex; }
            //return;
            //Team 
            List<NFLTeam> teams = new List<NFLTeam>();
            int SportType = _context.Sports_Type.FirstOrDefault(x => x.SportTypeName == "NFL").SportType;
            dynamic teamReferences = response.references.teamReferences;
            foreach (var obj in teamReferences)
            {
                try
                {

                    NFLTeam team = new NFLTeam();
                    team.Team_Id = obj.id;
                    team.City = obj.city;
                    team.Team_Name = obj.name;
                    team.Abbreviation = obj.abbreviation;
                    team.Venue_ID = obj.homeVenue.id;
                    team.LogoImageSrc = obj.city + " " + obj.name + ".gif";
                    team.SportType = SportType.ToString();
                    _context.NFLTeam.Add(team);
                    _context.Entry(team).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                catch (Exception ex)
                {

                }
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            { throw ex; }
            //Venue 
            List<NFLVenue> venues = new List<NFLVenue>();

            dynamic venueReferences = response.references.venueReferences;
            foreach (var obj in venueReferences)
            {
                try
                {

                    NFLVenue venue = new NFLVenue();
                    venue.Venue_ID = obj.id;
                    venue.City = obj.city;
                    venue.Venue_Name = obj.name;
                    venue.Country = obj.country;
                    venue.GeoCoordinates = obj.geoCoordinates != null ? obj.geoCoordinates.ToString() : "";


                    _context.NFLVenue.Add(venue);
                    _context.Entry(venue).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                catch (Exception ex)
                { }
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            { throw ex; }


        }

        private DateTime? GetCutOffDate(List<NFLSchedule> schedules, string week, string startTime, DateTime gameDate)
        {
            try
            {
                
                if (schedules == null || schedules.Count == 0)
                {
                    var timeUtc = DateTime.ParseExact(startTime, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                    
                    return today.StartOfWeek(DayOfWeek.Saturday);
                   // return today.AddDays(-1).StartOfWeek(DayOfWeek.Saturday); 
                }
                else
                {
                    NFLSchedule schedule = schedules.OrderBy(x => x.GameDate).FirstOrDefault(x => x.Week == week);
                    if (schedule != null)
                    {
                        //  var today = schedule.GameDate.ToString("MM/dd/yyyy 23:59:59");
                        // var timeUtc = DateTime.ParseExact(schedule.GameDate.ToString(), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        //var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                        //var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                        //  var fixDate = schedule.GameDate.ToString("MM/dd/yyyy 23:59:59");
                        return schedule.GameDate.StartOfWeek(DayOfWeek.Saturday);
                        //return schedule.GameDate.AddDays(-1).StartOfWeek(DayOfWeek.Saturday); 
                    }
                    else
                    {
                        return gameDate.StartOfWeek(DayOfWeek.Saturday);
                       // return gameDate.AddDays(-1).StartOfWeek(DayOfWeek.Saturday); 
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool RequestSeasonalAPI2()
        {
            string MYSPORTSFEEDS = "!@nT0rn0";
            string url = "https://api.mysportsfeeds.com/v1.2/pull/nfl/2019-regular/full_game_schedule.json";



            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Headers.Add("Authorization", "Basic " + GNF.Base64Encode("emilio_i "
            + ":" + MYSPORTSFEEDS));

            request.Method = "GET";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    return true;
                    //process the response
                }
            }
        }
        #endregion
    }
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
