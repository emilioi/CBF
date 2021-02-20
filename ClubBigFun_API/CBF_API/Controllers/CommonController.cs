using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static CBF_API.Helpers.enumeration;

namespace CBF_API.Controllers
{
    //   [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public CommonController(CbfDbContext context)
        {
            _context = context;
        }
        //[HttpGet("GetUserType")]
        //public IEnumerable<SelectListItem> GetUserType()
        //{
        //    LookupsWeatherListResponse objResponse = new LookupsWeatherListResponse();
        //    var list = new Lookups();
        //    IEnumerable<SelectListItem> ObjList = (from T in _context.Lookups
        //                                           where T.Lookup_Type == "UserType"
        //                                           select new SelectListItem
        //                                           {

        //                                               Text = T.Lookup_Name,
        //                                               Value = T.Lookup_Value.ToString()
        //                                           });
        //    return ObjList.ToList();
        //}
        [HttpGet("GetClosePoolType")]
        public IEnumerable<SelectListItem> GetClosePoolType()
        {
            LookupsWeatherListResponse objResponse = new LookupsWeatherListResponse();
            var list = new Lookups();
            IEnumerable<SelectListItem> ObjList = (from T in _context.Lookups
                                                   where T.Lookup_Type == "ClosePool"
                                                   select new SelectListItem
                                                   {

                                                       Text = T.Lookup_Name,
                                                       Value = T.Lookup_Value.ToString()
                                                   });
            return ObjList.ToList();
        }
        [HttpGet("GetRegularSeason")]
        public IEnumerable<SelectListItem> GetRegularSeason()
        {
            LookupsWeatherListResponse objResponse = new LookupsWeatherListResponse();
            var list = new Lookups();
            IEnumerable<SelectListItem> ObjList = (from T in _context.Lookups
                                                   where T.Lookup_Type == "Season"
                                                   select new SelectListItem
                                                   {

                                                       Text = T.Lookup_Name,
                                                       Value = T.Lookup_Value.ToString()
                                                   });
            return ObjList.ToList();
        }
        [HttpGet("GetCountriesList")]
        public CountriesResponses GetCountriesList(string LoginKey)
        {
            CountriesResponses objApiResponse = new CountriesResponses();
            string ValidationMsg = ValidationHelper.GetErrorListFromModelState(ModelState);
            if (!string.IsNullOrEmpty(ValidationMsg))
            {
                objApiResponse.Status = Messages.APIStatusError;
                objApiResponse.Message = ValidationMsg;
                return objApiResponse;
            }
            if (!ModelState.IsValid)
            {
                objApiResponse.Status = Messages.APIStatusError;
                objApiResponse.Message = Messages.ModelStateInvalid;
                return objApiResponse;
            }
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {

                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                var Countries = _context.Countries.ToList();
                objApiResponse.Message = "found";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Countries = Countries;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);

            }
            return objApiResponse;
        }
        [HttpGet("GetStateListByCountries")]
        public StatesResponses GetStateListByCountries(string LoginKey, int Country_ID)
        {
            StatesResponses objApiResponse = new StatesResponses();
            if (!ModelState.IsValid)
            {
                objApiResponse.Status = Messages.APIStatusError;
                objApiResponse.Message = Messages.ModelStateInvalid;
                return objApiResponse;
            }
            string ValidationMsg = ValidationHelper.GetErrorListFromModelState(ModelState);
            if (!string.IsNullOrEmpty(ValidationMsg))
            {
                objApiResponse.Status = Messages.APIStatusError;
                objApiResponse.Message = ValidationMsg;
                return objApiResponse;
            }
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {

                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                var States = _context.States.Where(x => x.Country_ID == Country_ID).ToList();
                objApiResponse.Message = "found";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.States = States;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
        [HttpGet("Cron")]
        public Email_NotificationResponses Cron()
        {
            Email_NotificationResponses objApiResponse = new Email_NotificationResponses();
            try
            {

                List<Email_Notification> lstNotifications = _context.Email_Notification.Where(x => x.Is_Sent == false).Take(10).ToList();
                foreach (Email_Notification objNotify in lstNotifications)
                {
                    Email_Notification email_Notification = EmailHelper.SendMail(objNotify.To_Email, objNotify.Subject, objNotify.Email_Content, true, objNotify);

                    _context.Email_Notification.Add(email_Notification);
                    _context.Entry(email_Notification).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                _context.SaveChanges();
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = Messages.UpdatedSuccess;
                //objApiResponse.Email_Notification = objNotify;
                // CronElimnation();
                // CronWinners();
                // CronPickDefaulted();
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
                GNF.SaveException(ex, _context);

            }
            return objApiResponse;
        }

        private class MemberIdEmail
        {
            public int? MemberId { get; set; }
            public string Email_Address { get; set; }
        }
        [HttpGet("CronElimnation")]
        public Email_NotificationResponses CronElimnation()
        {
            Email_NotificationResponses objApiResponse = new Email_NotificationResponses();
            try
            {
                Email_Templates objTemplate = _context.Email_Templates.Where(x => x.EmailID == eEmailTemplates.GeneralNotification).FirstOrDefault();

                List<NFLEliminationProcess> lstElimiation = _context.Query<NFLEliminationProcess>().FromSql("exec [SP_PickWithWrongWinner]").ToList();


                List<MemberIdEmail> lstMemberDetails = lstElimiation
                                                        .GroupBy(x => new { x.Email_Address, x.MemberID })
                                                        .Select(y => new MemberIdEmail { Email_Address = y.Key.Email_Address, MemberId = y.Key.MemberID }).ToList();

                foreach (MemberIdEmail objMember in lstMemberDetails)
                {
                    Email_Templates objEmailTemp = objTemplate;
                    List<NFLEliminationProcess> lstFilteredEliminatedList = lstElimiation.Where(x => x.MemberID == objMember.MemberId).ToList();
                    if (lstFilteredEliminatedList != null)
                    {
                        string subject = objEmailTemp.Subject;
                        string body = objEmailTemp.Body;

                        string ElimatedText = "Dear Member, <br/><br/>Following tickets are eliminated: <br/><br/><table width='100%' border='1' style='text-align:center'>";
                        ElimatedText += "<tr style='background:#eee'><th>Entry Name</th><th>Week Number</th><th>Your Pick</th><th>Result</th><tr>";
                        foreach (NFLEliminationProcess objElimation in lstFilteredEliminatedList)
                        {

                            if (objElimation.ActualWinner != objElimation.YourWinner)
                            {
                                SurvEntryPicks objPicks = _context.SurvEntryPicks.Where(x => x.EntryID == objElimation.EntryId && x.WeekNumber == objElimation.WeekNumber && x.ScheduleID == objElimation.ScheduleId).FirstOrDefault();
                                if (objPicks != null && !objPicks.Eliminated)
                                {
                                    objPicks.Eliminated = true;
                                    objPicks.EliminatedNotificationSent = true;
                                    //Format email
                                    ElimatedText += "<tr><td>" + objElimation.EntryName + "</td><td> " + objElimation.WeekNumber + "</td><td>" + objElimation.YourWinner + "</td><td>" + objElimation.ActualWinner + "</td><tr>";


                                    SurvEntries objEntry = _context.SurvEntries.FirstOrDefault(x => x.EntryID == objPicks.EntryID);
                                    if (objEntry != null)
                                    {
                                        objEntry.Eliminated = true;
                                        objEntry.LastUpdated = DateTime.Now;
                                        _context.SurvEntries.Add(objEntry);
                                        _context.Entry(objEntry).State = EntityState.Modified;
                                    }

                                    _context.SurvEntryPicks.Add(objPicks);
                                    _context.Entry(objPicks).State = EntityState.Modified;
                                }
                            }
                        }
                        ElimatedText += "</table> <br/>";
                        subject = subject.Replace("#SUBJECT#", "Your tickets are eliminated.");
                        body = body.Replace("#BODY#", ElimatedText);
                        NotificationHelper.SaveNotifications(objMember.Email_Address, subject, body, eNotificationType.GeneralNotification, _context, false, objEmailTemp);
                    }


                }


                _context.SaveChanges();
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = Messages.UpdatedSuccess;
                //objApiResponse.Email_Notification = objNotify;

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);

            }
            return objApiResponse;
        }
        [HttpGet("CronWinners")]
        public Email_NotificationResponses CronWinners()
        {
            Email_NotificationResponses objApiResponse = new Email_NotificationResponses();
            try
            {
                Email_Templates objTemplate = _context.Email_Templates.Where(x => x.EmailID == eEmailTemplates.GeneralNotification).FirstOrDefault();

                List<NFLEliminationProcess> lstElimiation = _context.Query<NFLEliminationProcess>().FromSql("exec [SP_PickWithCorrect_Winner]").ToList();


                List<MemberIdEmail> lstMemberDetails = lstElimiation
                                                        .GroupBy(x => new { x.Email_Address, x.MemberID })
                                                        .Select(y => new MemberIdEmail { Email_Address = y.Key.Email_Address, MemberId = y.Key.MemberID }).ToList();

                foreach (MemberIdEmail objMember in lstMemberDetails)
                {
                    Email_Templates objEmailTemp = objTemplate;
                    List<NFLEliminationProcess> lstFilteredEliminatedList = lstElimiation.Where(x => x.MemberID == objMember.MemberId).ToList();
                    if (lstFilteredEliminatedList != null)
                    {
                        string subject = objEmailTemp.Subject;
                        string body = objEmailTemp.Body;

                        string ElimatedText = "Dear Member, <br/><br/>Congratulations! You won the match for following tickets: <br/><br/><table width='100%' border='1' style='text-align:center'>";
                        ElimatedText += "<tr style='background:#eee'><th>Entry Name</th><th>Week Number</th><th>Your Pick</th><th>Result</th><tr>";
                        foreach (NFLEliminationProcess objElimation in lstFilteredEliminatedList)
                        {

                            if (objElimation.ActualWinner == objElimation.YourWinner)
                            {
                                SurvEntryPicks objPicks = _context.SurvEntryPicks.Where(x => x.EntryID == objElimation.EntryId && x.WeekNumber == objElimation.WeekNumber && x.ScheduleID == objElimation.ScheduleId).FirstOrDefault();
                                if (objPicks != null && !objPicks.WinnerNotificationSent)
                                {
                                    //objPicks.Eliminated = true;

                                    //Format email
                                    ElimatedText += "<tr><td>" + objElimation.EntryName + "</td><td> " + objElimation.WeekNumber + "</td><td>" + objElimation.YourWinner + "</td><td>" + objElimation.ActualWinner + "</td><tr>";

                                    objPicks.WinnerNotificationSent = true;
                                    //SurvEntries objEntry = _context.SurvEntries.FirstOrDefault(x => x.EntryID == objPicks.EntryID);
                                    //if (objEntry != null)
                                    //{
                                    //   // objEntry.Eliminated = true;
                                    //    objEntry.LastUpdated = DateTime.Now;
                                    //    _context.SurvEntries.Add(objEntry);
                                    //    _context.Entry(objEntry).State = EntityState.Modified;
                                    //}

                                    _context.SurvEntryPicks.Add(objPicks);
                                    _context.Entry(objPicks).State = EntityState.Modified;
                                }
                            }
                        }
                        ElimatedText += "</table> <br/>";
                        subject = subject.Replace("#SUBJECT#", "Congratulations! You won the match for following tickets.");
                        body = body.Replace("#BODY#", ElimatedText);
                        NotificationHelper.SaveNotifications(objMember.Email_Address, subject, body, eNotificationType.GeneralNotification, _context, false, objEmailTemp);
                    }


                }


                _context.SaveChanges();
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = Messages.UpdatedSuccess;
                //objApiResponse.Email_Notification = objNotify;

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);

            }
            return objApiResponse;
        }
        [HttpGet("CronPickDefaulted")]
        public Email_NotificationResponses CronPickDefaulted()
        {
            Email_NotificationResponses objApiResponse = new Email_NotificationResponses();
            try
            {
                Email_Templates objTemplate = _context.Email_Templates.Where(x => x.EmailID == eEmailTemplates.GeneralNotification).FirstOrDefault();

                List<SurvEntries_WithoutPicks_All> lstDefaulted = _context.Query<SurvEntries_WithoutPicks_All>().FromSql("exec [SP_SurvEntries_WithoutPicks_All]").ToList();

                List<MemberIdEmail> lstMemberDetails = lstDefaulted
                                                      .GroupBy(x => new { x.Email_Address, x.Member_Id })
                                                      .Select(y => new MemberIdEmail { Email_Address = y.Key.Email_Address, MemberId = y.Key.Member_Id }).ToList();

                foreach (MemberIdEmail objMember in lstMemberDetails)
                {
                    Email_Templates objEmailTemp = objTemplate;

                    List<SurvEntries_WithoutPicks_All> lstFilteredEliminatedList = lstDefaulted.Where(x => x.Member_Id == objMember.MemberId).ToList();
                    if (lstFilteredEliminatedList != null)
                    {
                        string subject = objEmailTemp.Subject;
                        string body = objEmailTemp.Body;
                        string ElimatedText = "Dear Member, <br/><br/>Following tickets are defaulted: <br/><br/><table width='100%' border='1' style='text-align:center'>";
                        ElimatedText += "<tr style='background:#eee'><th>Entry Name</th><th>Week Number</th><th>Default Pick</th><tr>";
                        foreach (SurvEntries_WithoutPicks_All objDefaulted in lstFilteredEliminatedList)
                        {
                            List<Pool_Defaults> lstPoolDefaults = _context.Pool_Defaults.Where(x => x.Pool_Id == objDefaulted.PoolID && x.WeekNumber == objDefaulted.WeekNumber).OrderBy(x => x.Rank).ToList();
                            if (lstPoolDefaults != null)
                            {

                                List<SurvEntryPicks> lstSurvPicks = _context.SurvEntryPicks.Where(x => x.EntryID == objDefaulted.EntryID).ToList();
                                lstSurvPicks = lstSurvPicks == null ? new List<SurvEntryPicks>() : lstSurvPicks;
                                List<int> lstWinnerSelection = lstSurvPicks.Select(x => x.Winner).ToList();
                                SurvEntryPicks objPicks = _context.SurvEntryPicks.Where(x => x.EntryID == objDefaulted.EntryID && x.WeekNumber == objDefaulted.WeekNumber && x.ScheduleID == objDefaulted.ScheduleID).FirstOrDefault();
                                if (objPicks != null)
                                {
                                    //Do nothing
                                }
                                else
                                {

                                    Pool_Defaults objRightPick = new Pool_Defaults();
                                    foreach (Pool_Defaults poolDef in lstPoolDefaults)
                                    {
                                        if (!lstWinnerSelection.Contains(poolDef.Team_Id))
                                        {
                                            objRightPick = poolDef;
                                            break;
                                        }
                                    }
                                    if (objRightPick == null)
                                    {
                                        throw new Exception("Pool Default setup is not done.");
                                    }
                                    NFLTeam objTeam = _context.NFLTeam.Where(x => x.Team_Id == objRightPick.Team_Id).FirstOrDefault();
                                    if (objTeam == null)
                                    {
                                        throw new Exception("Team not found");
                                    }
                                    SurvEntries objEntry = _context.SurvEntries.FirstOrDefault(x => x.EntryID == objDefaulted.EntryID);
                                    if (objEntry != null)
                                    {

                                        objEntry.Defaults = objEntry.Defaults == null ? 0 : objEntry.Defaults + 1;
                                        _context.SurvEntries.Add(objEntry);
                                        _context.Entry(objEntry).State = EntityState.Modified;


                                        objPicks = new SurvEntryPicks();

                                        objPicks.Created = DateTime.Now;
                                        objPicks.Defaulted = true;
                                        objPicks.EntryID = Convert.ToInt32(objDefaulted.EntryID);

                                        objPicks.ScheduleID = Convert.ToInt32(objRightPick.Schedule_Id);
                                        objPicks.Winner = objRightPick.Team_Id;


                                        objPicks.WeekNumber = Convert.ToInt32(objDefaulted.WeekNumber);


                                        objPicks.DefaultedNotificationSent = true;
                                        _context.SurvEntryPicks.Add(objPicks);
                                        _context.Entry(objPicks).State = EntityState.Added;

                                        ElimatedText += "<tr><td>" + objDefaulted.EntryName + "</td><td> " + objDefaulted.WeekNumber + "</td><td>" + objTeam.Team_Name + "</td><tr>";
                                        ////Format email
                                        //string subject = objEmailTemp.Subject;
                                        //string body = objEmailTemp.Body;

                                        //string ElimatedText = "Dear Member, <br/><br/>Following tickets are eliminated: <br/>";
                                        //ElimatedText += "<b>";
                                        //ElimatedText += "Entry Name: " + objEntry.EntryName + " <br />";
                                        //ElimatedText += "Week Number: " + objDefaulted.WeekNumber + " <br />";
                                        //ElimatedText += "</b>";
                                        //subject = subject.Replace("#SUBJECT#", "Your ticket is eliminated.");
                                        //body = body.Replace("#BODY#", ElimatedText);
                                        //NotificationHelper.SaveNotifications(objDefaulted.Email_Address, objEmailTemp.Subject, body, eNotificationType.GeneralNotification, _context, false, objEmailTemp);

                                    }
                                }
                            }

                        }
                        ElimatedText += "</table> <br/>";
                        subject = subject.Replace("#SUBJECT#", "Your tickets are defaulted because of no pick made on time.");
                        body = body.Replace("#BODY#", ElimatedText);
                        NotificationHelper.SaveNotifications(objMember.Email_Address, subject, body, eNotificationType.GeneralNotification, _context, false, objEmailTemp);
                    }
                }
                _context.SaveChanges();
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = Messages.UpdatedSuccess;
                //objApiResponse.Email_Notification = objNotify;

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
                GNF.SaveException(ex, _context);

            }
            return objApiResponse;
        }
        [HttpPost("NotifyDeveloper")]
        public APIResponses NotifyDeveloper(string Error)
        {
            APIResponses objApiResponse = new APIResponses();

            try
            {
                var Countries = _context.Countries.ToList();
                objApiResponse.Message = "Support team is looking on it.";
                objApiResponse.Status = Messages.APIStatusSuccess;
                EmailHelper.SendMail("askdev@nowgray.com", "CBF Application Error", Error, true);
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }


        [HttpPost("UpdateScore")]
        public APIResponses UpdateScore(int access = 0)
        {
            APIResponses objResponse = new APIResponses();

            objResponse.Message = "API is okay";
            objResponse.Status = Messages.APIStatusSuccess;
            try
            {
                NFL_API_Helper objNFL = new NFL_API_Helper(_context);
                objNFL.Get_NFLScore_API();

                List<ScoreDetail> scoreDetails = _context.Query<ScoreDetail>().FromSql("SP_NFLScore_UpdateNewScores").ToList();
                if (scoreDetails != null && scoreDetails.Count > 0)
                {
                    string html = "";
                    html += "<table width='100%' style='font-size:12px' border='1' cellpadding='10'><tr>";
                    html += "<th>NFLScheduleID </th><th>Description</th><th>HomeTeam </th><th>HomeScoreTotal</th><th>VisitingTeam</th><th>AwayScoreTotal</th>";
                    html += "</tr>";
                    foreach (ScoreDetail score in scoreDetails)
                    {
                        html += "<tr>";
                        html += "<th>" + score.NFLScheduleID + "  </th><th>" + score.Description + "</th><th>" + score.HomeTeam + "   </th><th>     " + score.HomeScoreTotal + "</th><th> " + score.VisitingTeam + "    </th><th>  " + score.AwayScoreTotal + "   </th>";
                        html += "</tr>";
                    }

                    html += "</table>";


                    string msg = "<h5>Following schedules score updated</h5> ";

                    EmailHelper.SendMail("nowgray@gmail.com", "Hurray!!!!! Score updated", msg + html, true);
                    EmailHelper.SendMail(GlobalConfig.ScoreAdmin, "Hurray!!!!! Score updated", msg + html, true);
                }
                else
                {
                    EmailHelper.SendMail("nowgray@gmail.com", "Hurray!!!!! Score cron ran with >> NO DATA >> " + DateTime.Now.ToString(), "Score cron ran with >> NO DATA >>" + DateTime.Now.ToString(), true);
                }

                if (access == 5)
                {
                    CronElimnation();
                    CronWinners();
                }
            }
            catch (Exception ex)
            {
                objResponse.Message = "API is not good. " + ex.Message;
                objResponse.Status = Messages.APIStatusError;
                GNF.SaveException(ex, _context);
            }
            return objResponse;
        }

        [HttpPost("UpdateNHLScore")]
        public APIResponses UpdateNHLScore(int access = 0)
        {
            APIResponses objResponse = new APIResponses();

            objResponse.Message = "API is okay";
            objResponse.Status = Messages.APIStatusSuccess;
            try
            {
                NHL_API_Helper objNFL = new NHL_API_Helper(_context);
                objNFL.Get_NHLScore_API();
                
                List<ScoreDetail> scoreDetails = _context.Query<ScoreDetail>().FromSql("SP_NHLScore_UpdateNewScores").ToList();
                if (scoreDetails != null && scoreDetails.Count > 0)
                {
                    string html = "";
                    html += "<table width='100%' style='font-size:12px' border='1' cellpadding='10'><tr>";
                    html += "<th>NHLScheduleID </th><th>Description</th><th>HomeTeam </th><th>HomeScoreTotal</th><th>VisitingTeam</th><th>AwayScoreTotal</th>";
                    html += "</tr>";
                    foreach (ScoreDetail score in scoreDetails)
                    {
                        html += "<tr>";
                        html += "<th>" + score.NFLScheduleID + "  </th><th>" + score.Description + "</th><th>" + score.HomeTeam + "   </th><th>     " + score.HomeScoreTotal + "</th><th> " + score.VisitingTeam + "    </th><th>  " + score.AwayScoreTotal + "   </th>";
                        html += "</tr>";
                    }

                    html += "</table>";


                    string msg = "<h5>Following schedules score updated</h5> ";

                    EmailHelper.SendMail("nowgray@gmail.com", "Hurray!!!!! Score updated", msg + html, true);
                    EmailHelper.SendMail(GlobalConfig.ScoreAdmin, "Hurray!!!!! Score updated", msg + html, true);
                }
                else
                {
                    EmailHelper.SendMail("nowgray@gmail.com", "Hurray!!!!! Score cron ran with >> NO DATA >> " + DateTime.Now.ToString(), "Score cron ran with >> NO DATA >>" + DateTime.Now.ToString(), true);
                }

                if (access == 5)
                {
                    CronElimnation();
                    CronWinners();
                }
            }
            catch (Exception ex)
            {
                objResponse.Message = "API is not good. " + ex.Message;
                objResponse.Status = Messages.APIStatusError;
                GNF.SaveException(ex, _context);
            }
            return objResponse;
        }


        [HttpPost("UpdateNFLSchedule")]
        public APIResponses UpdateNFLSchedule()
        {
            APIResponses objResponse = new APIResponses();

            objResponse.Message = "API is okay";
            objResponse.Status = Messages.APIStatusSuccess;
            try
            {
                NFL_API_Helper objNFL = new NFL_API_Helper(_context);
                //objNFL.RequestSeasonalAPI2();
                objNFL.Get_NFLSchedule_API();
                //  objNFL.RequestBoxScoreAPI();
            }
            catch (Exception ex)
            {
                objResponse.Message = "API is not good. " + ex.Message;
                objResponse.Status = Messages.APIStatusError;
            }
            return objResponse;
        }

        [HttpPost("UpdateNFLTeam")]
        public APIResponses UpdateNFLTeam()
        {
            APIResponses objResponse = new APIResponses();

            objResponse.Message = "API is okay";
            objResponse.Status = Messages.APIStatusSuccess;
            try
            {
                NFL_API_Helper objNFL = new NFL_API_Helper(_context);
                //objNFL.RequestSeasonalAPI2();
                objNFL.Get_NFLTeam_API();
                //  objNFL.RequestBoxScoreAPI();
            }
            catch (Exception ex)
            {
                objResponse.Message = "API is not good. " + ex.Message;
                objResponse.Status = Messages.APIStatusError;
            }
            return objResponse;
        }


        [HttpPost("UpdateNFLVenue")]
        public APIResponses UpdateNFLVenue()
        {
            APIResponses objResponse = new APIResponses();

            objResponse.Message = "API is okay";
            objResponse.Status = Messages.APIStatusSuccess;
            try
            {
                NFL_API_Helper objNFL = new NFL_API_Helper(_context);
                //objNFL.RequestSeasonalAPI2();
                objNFL.Get_NFLVenue_API();
                //  objNFL.RequestBoxScoreAPI();
            }
            catch (Exception ex)
            {
                objResponse.Message = "API is not good. " + ex.Message;
                objResponse.Status = Messages.APIStatusError;
            }
            return objResponse;
        }

        [HttpGet("ErrorExceptionsList")]
        public ErrorExceptionListResponse ErrorExceptionsList()
        {
            ErrorExceptionListResponse objResponse = new ErrorExceptionListResponse();

            objResponse.Message = "API is okay";
            objResponse.Status = Messages.APIStatusSuccess;
            try
            {
                List<ErrorExceptionGroupBy> lstError = _context.ErrorExceptions.GroupBy(group => new { group.Message, group.date.Date }).Select(x => new ErrorExceptionGroupBy
                {
                    ErrorCount = x.Count(),
                    Message = x.Key.Message,
                    Date = x.Key.Date
                }).OrderByDescending(y => y.Date).ToList();

                objResponse.Message = "Success";
                objResponse.Status = Messages.APIStatusSuccess;
                objResponse.ErrorList = lstError;
            }
            catch (Exception ex)
            {
                objResponse.Message = "API is not good. " + ex.Message;
                objResponse.Status = Messages.APIStatusError;
            }
            return objResponse;
        }

        [HttpGet("ClubSettings")]
        public ClubSettingResponse ClubSettings(string type = "RegistrationOpen")
        {
            ClubSettingResponse objResponse = new ClubSettingResponse();
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    List<Lookups> ObjList = _context.Lookups.Where(x => x.Lookup_Type == "CBFSettings" && x.Lookup_Name == type).ToList();
                    objResponse.Message = "Success";
                    objResponse.settings = ObjList;
                }
                else
                {
                    List<Lookups> ObjList = _context.Lookups.Where(x => x.Lookup_Type == "CBFSettings").ToList();
                    objResponse.Message = "Success";
                    objResponse.settings = ObjList;
                }

                objResponse.Status = Messages.APIStatusSuccess;
                return objResponse;
            }
            catch (Exception ex)
            {
                objResponse.Message = "Error";
                objResponse.settings = null;
                objResponse.Status = Messages.APIStatusError;
            }
            return objResponse;
        }


        [HttpPost("ChangeRegistrationSetting")]
        public ClubSettingResponse ChangeRegistrationSetting(string LoginKey, bool RegistrationOpen)
        {
            ClubSettingResponse objResponse = new ClubSettingResponse();
            TokenHelper tk = new TokenHelper(_context);
            if (!tk.ValidateToken(LoginKey))
            {

                objResponse.Status = Messages.APIStatusError;
                objResponse.Message = Messages.InvalidToken;
                return objResponse;
            }
            Token objToken = tk.GetTokenByKey(LoginKey);
            if (objToken != null)
            {
                Administrators objAdmin = _context.Administrators.Find(objToken.UserId);
                if (objAdmin != null)
                {
                    try
                    {
                        Lookups objLookup = _context.Lookups.Where(x => x.Lookup_Type == "CBFSettings" && x.Lookup_Name == "RegistrationOpen").FirstOrDefault();
                        if (objLookup != null)
                        {
                            objLookup.Lookup_Value = RegistrationOpen ? "true" : "false";
                            _context.Lookups.Add(objLookup);
                            _context.Entry(objLookup).State = EntityState.Modified;
                            _context.SaveChanges();

                            //

                            var list = new Lookups();
                            List<Lookups> ObjList = _context.Lookups.Where(x => x.Lookup_Type == "CBFSettings").ToList();
                            objResponse.Message = "Success";
                            objResponse.settings = ObjList;
                            objResponse.Status = Messages.APIStatusSuccess;
                            return objResponse;
                        }
                        else
                        {
                            objResponse.Status = Messages.APIStatusError;
                            objResponse.Message = "There is some problem, Please contact Technical Team.";
                            return objResponse;
                        }

                    }
                    catch (Exception ex)
                    {
                        objResponse.Message = "Error";
                        objResponse.settings = null;
                        objResponse.Status = Messages.APIStatusError;
                        return objResponse;
                    }
                }
            }
            objResponse.Status = Messages.APIStatusError;
            objResponse.Message = Messages.InvalidToken;
            return objResponse;
        }

        //maintanence setting for admin
        [HttpGet("MaintenanceSettings")]
        public ClubSettingResponse MaintenanceSettings()
        {
            ClubSettingResponse objResponse = new ClubSettingResponse();
            try
            {
                //if (!string.IsNullOrEmpty(type))
                //{
                //    List<Lookups> ObjList = _context.Lookups.Where(x => x.Lookup_Type == "CBFSettings" && x.Lookup_Name == type).ToList();
                //    objResponse.Message = "Success";
                //    objResponse.settings = ObjList;
                //}
                //else
                //{
                List<Lookups> ObjList = _context.Lookups.Where(x => x.Lookup_Type == "CBFSettings").ToList();
                objResponse.Message = "Success";
                objResponse.settings = ObjList;
                //}

                objResponse.Status = Messages.APIStatusSuccess;
                return objResponse;
            }
            catch (Exception ex)
            {
                objResponse.Message = "Error";
                objResponse.settings = null;
                objResponse.Status = Messages.APIStatusError;
            }
            return objResponse;
        }
        //maintanence text for admin
        [HttpPost("ChangeMaintenanceSettingAndText")]
        public ClubSettingResponse ChangeMaintenanceSettingAndText([FromBody] MaintenanceRequest objMaintenance, string LoginKey)//, bool MaintenanceOn, string MaintenanceText
        {
            ClubSettingResponse objResponse = new ClubSettingResponse();
            TokenHelper tk = new TokenHelper(_context);
            if (!tk.ValidateToken(LoginKey))
            {

                objResponse.Status = Messages.APIStatusError;
                objResponse.Message = Messages.InvalidToken;
                return objResponse;
            }
            Token objToken = tk.GetTokenByKey(LoginKey);
            if (objToken != null)
            {
                Administrators objAdmin = _context.Administrators.Find(objToken.UserId);
                if (objAdmin != null)
                {
                    try
                    {
                        Lookups objLookup1 = _context.Lookups.Where(x => x.Lookup_Type == "CBFSettings" && x.Lookup_Name == "MaintenanceOn").FirstOrDefault();
                        Lookups objLookup2 = _context.Lookups.Where(x => x.Lookup_Type == "CBFSettings" && x.Lookup_Name == "MaintenanceText").FirstOrDefault();

                        if (objLookup1 != null)
                        {
                            string str = objMaintenance.MaintenanceOn.ToString().ToLower();
                            objLookup1.Lookup_Value = str;
                            _context.Lookups.Add(objLookup1);
                            _context.Entry(objLookup1).State = EntityState.Modified;
                            try
                            {
                                if (objMaintenance.MaintenanceOn)
                                {
                                    _context.RemoveRange(_context.UserSession.Where(x => x.UserID != objToken.UserId).ToList());
                                }
                            }
                            catch(Exception ex)
                            {
                                GNF.SaveException(ex, _context);
                            }
                            _context.SaveChanges();
                        }
                        else
                        {
                            objResponse.Status = Messages.APIStatusError;
                            objResponse.Message = "There is some problem, Please contact Technical Team.";
                            return objResponse;
                        }
                        if (objLookup2 != null)
                        {
                            objLookup2.Lookup_Value = objMaintenance.MaintenanceText;
                            _context.Lookups.Add(objLookup2);
                            _context.Entry(objLookup2).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                        else
                        {
                            objResponse.Status = Messages.APIStatusError;
                            objResponse.Message = "There is some problem, Please contact Technical Team.";
                            return objResponse;
                        }

                        var list = new Lookups();
                        List<Lookups> ObjList = _context.Lookups.Where(x => x.Lookup_Type == "CBFSettings").ToList();
                        objResponse.Message = "Success";
                        objResponse.settings = ObjList;
                        objResponse.Status = Messages.APIStatusSuccess;
                        return objResponse;
                    }
                    catch (Exception ex)
                    {
                        objResponse.Message = "Error";
                        objResponse.settings = null;
                        objResponse.Status = Messages.APIStatusError;
                        return objResponse;
                    }
                }
            }
            objResponse.Status = Messages.APIStatusError;
            objResponse.Message = Messages.InvalidToken;
            return objResponse;
        }
   
        #region NHL APIs

        [HttpPost("UpdateNHLSchedule")]
        public APIResponses UpdateNHLSchedule()
        {
            APIResponses objResponse = new APIResponses();

            objResponse.Message = "API is okay";
            objResponse.Status = Messages.APIStatusSuccess;
            try
            {
                NHL_API_Helper objNHL = new NHL_API_Helper(_context);
                //objNHL.RequestSeasonalAPI2();
                objNHL.Get_NHLSchedule_API();
                //  objNHL.RequestBoxScoreAPI();
            }
            catch (Exception ex)
            {
                objResponse.Message = "API is not good. " + ex.Message;
                objResponse.Status = Messages.APIStatusError;
            }
            return objResponse;
        }

        [HttpPost("UpdateNHLTeam")]
        public APIResponses UpdateNHLTeam()
        {
            APIResponses objResponse = new APIResponses();

            objResponse.Message = "API is okay";
            objResponse.Status = Messages.APIStatusSuccess;
            try
            {
                NHL_API_Helper objNHL = new NHL_API_Helper(_context);
                //objNHL.RequestSeasonalAPI2();
                objNHL.Get_NHLTeam_API();
                //  objNHL.RequestBoxScoreAPI();
            }
            catch (Exception ex)
            {
                objResponse.Message = "API is not good. " + ex.Message;
                objResponse.Status = Messages.APIStatusError;
            }
            return objResponse;
        }


        [HttpPost("UpdateNHLVenue")]
        public APIResponses UpdateNHLVenue()
        {
            APIResponses objResponse = new APIResponses();

            objResponse.Message = "API is okay";
            objResponse.Status = Messages.APIStatusSuccess;
            try
            {
                NHL_API_Helper objNHL = new NHL_API_Helper(_context);
                //objNHL.RequestSeasonalAPI2();
                objNHL.Get_NHLVenue_API();
                //  objNHL.RequestBoxScoreAPI();
            }
            catch (Exception ex)
            {
                objResponse.Message = "API is not good. " + ex.Message;
                objResponse.Status = Messages.APIStatusError;
            }
            return objResponse;
        }

        #endregion
    }
}
