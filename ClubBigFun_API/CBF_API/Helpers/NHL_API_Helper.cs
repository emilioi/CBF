using CBF_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CBF_API.Helpers
{
    public class NHL_API_Helper
    {
        private readonly CbfDbContext _context;

        public NHL_API_Helper(CbfDbContext context)
        {
            _context = context;
        }

        public bool Get_NHLSchedule_API()
        {
            //Remove data from Table
            //_context.NHLSchedule.RemoveRange(_context.NHLSchedule);
            //_context.SaveChanges();



            List<NHLSchedule> schedules = new List<NHLSchedule>();

            string MYSPORTSFEEDS = "MYSPORTSFEEDS";  
             //string url = "https://api.mysportsfeeds.com/v2.1/pull/nhl/current/games.json";
           string url = "https://api.mysportsfeeds.com/v2.1/pull/nhl/2019-2020-regular/games.json";
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

                                NHLSchedule schedule = new NHLSchedule();

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

                                schedules.Add(schedule);
                                _context.NHLSchedule.Add(schedule);
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

        public bool Get_NHLScore_API()
        {
            //Remove data from Table
            //_context.NHLScore.RemoveRange(_context.NHLScore);
            //_context.SaveChanges();

            return true;
            List<NHLSchedule> schedules = new List<NHLSchedule>();

            string MYSPORTSFEEDS = "MYSPORTSFEEDS";
            string url = "https://api.mysportsfeeds.com/v2.1/pull/NHL/2019-2020-regular/games.json";
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
                            NHLScore objScore = _context.NHLScore.FirstOrDefault(x => x.Schedule_Id == ScheduleID);
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


                                _context.NHLScore.Add(objScore);
                                _context.Entry(objScore).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            }
                            else
                            {
                                objScore = new NHLScore();
                                objScore.AwayScoreTotal = score.awayScoreTotal;

                                objScore.CurrentDown = score.currentDown;
                                objScore.CurrentIntermission = score.currentIntermission;
                                objScore.CurrentQuarter = score.currentQuarter;
                                objScore.CurrentQuarterSecondsRemaining = score.currentQuarterSecondsRemaining;
                                objScore.CurrentYardsRemaining = score.currentYardsRemaining;
                                objScore.HomeScoreTotal = score.homeScoreTotal;
                                objScore.LineOfScrimmage = score.lineOfScrimmage;
                                //objScore.Quarters = score.quarters == null ? score.quarters.ToString() : "";

                                objScore.TeamInPossession = score.teamInPossession;

                                objScore.Schedule_Id = obj.id;


                                _context.NHLScore.Add(objScore);
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
        public bool Get_NHLTeam_API()
        {
            //Remove data from Table
            //_context.NHLTeam.RemoveRange(_context.NHLTeam);
            //_context.SaveChanges();


            List<NHLSchedule> schedules = new List<NHLSchedule>();

            string MYSPORTSFEEDS = "MYSPORTSFEEDS";
            string url = "https://api.mysportsfeeds.com/v2.1/pull/NHL/2019-2020-regular/games.json";
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
                        int SportType = _context.Sports_Type.FirstOrDefault(x => x.SportTypeName == "NHL").SportType;
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
        public bool Get_NHLVenue_API()
        {
            //Remove data from Table
            _context.NHLVenue.RemoveRange(_context.NHLVenue);
            _context.SaveChanges();


            List<NHLSchedule> schedules = new List<NHLSchedule>();

            string MYSPORTSFEEDS = "MYSPORTSFEEDS";
            string url = "https://api.mysportsfeeds.com/v2.1/pull/NHL/2019-2020-regular/games.json";
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
                        List<NHLVenue> venues = new List<NHLVenue>();

                        dynamic venueReferences = list.references.venueReferences;
                        foreach (var obj in venueReferences)
                        {
                            try
                            {

                                NHLVenue venue = new NHLVenue();
                                venue.Venue_ID = obj.id;
                                venue.City = obj.city;
                                venue.Venue_Name = obj.name;
                                venue.Country = obj.country;
                                venue.GeoCoordinates = obj.geoCoordinates != null ? obj.geoCoordinates.ToString() : "";


                                _context.NHLVenue.Add(venue);
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
        private DateTime? GetCutOffDate(List<NHLSchedule> schedules, string week, string startTime, DateTime gameDate)
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
                    NHLSchedule schedule = schedules.OrderBy(x => x.GameDate).FirstOrDefault(x => x.Week == week);
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


    }

}
