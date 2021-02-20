using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CBF_API.Helpers;
using CBF_API.Migrations;
using CBF_API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CBF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class NFLScheduleController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public NFLScheduleController(CbfDbContext context)
        {
            _context = context;
        }

        [HttpGet("id")]
        public async Task<IActionResult> Search([FromRoute]string id)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var objNFLSchedule = from m in _context.NFLSchedule
                                     select m;

                if (!String.IsNullOrEmpty(id))
                {
                    objNFLSchedule = objNFLSchedule.Where(s => s.Week.Contains(id));
                    // objNFLSchedule = objNFLSchedule.Where(p => p.WeekNumber.Contains(id));
                    // objNFLSchedule = objNFLSchedule.Where(q => q.GameDate.Contains(id));
                }

                return Ok(await objNFLSchedule.ToListAsync());
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpGet("All")]
        public NFLScheduleListResponses NFLScheduleList(string LoginKey)
        {
            NFLScheduleListResponses objApiResponse = new NFLScheduleListResponses();
            TokenHelper tk = new TokenHelper(_context);
            if (!tk.ValidateToken(LoginKey))
            {

                objApiResponse.Status = Messages.APIStatusError;
                objApiResponse.Message = Messages.InvalidToken;
                return objApiResponse;
            }
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
                List<NFLScheduleList> objTaskStatus = _context.Query<NFLScheduleList>().FromSql("exec SP_NFLSchedule").ToList();

                if (objTaskStatus != null)
                {
                    objApiResponse.Message = objTaskStatus.Count + " record found";
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.NFLScheduleList = objTaskStatus;
                }
                else
                {
                    objApiResponse.Message = 0 + " record found";
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.NFLScheduleList = null;

                }
            }

            catch (Exception ex)
            {
                objApiResponse.Message = ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
                objApiResponse.NFLScheduleList = null;
            }
            return objApiResponse;
        }


        [HttpGet("GetScheduleMenu")]
        public ScheduleMenuAPIResponse GetScheduleMenu(string LoginKey)
        {
            ScheduleMenuAPIResponse objApiResponse = new ScheduleMenuAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                string Sql = "select * from qrySurvScheduleCount order by Pool_Name";
                var scheduleMenus = _context.Query<ScheduleMenu>().FromSql(Sql).ToList();
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.scheduleMenus = scheduleMenus;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);

            }
            return objApiResponse;
        }

        //[HttpGet("GetNFLScheduleWeeksList")]
        //public ScheduleWeekListAPIResponse GetNFLScheduleWeeksList(string LoginKey, int Pool_ID, int PageNo = 1)
        //{
        //    ScheduleWeekListAPIResponse objApiResponse = new ScheduleWeekListAPIResponse();
        //    try
        //    {
        //        TokenHelper tk = new TokenHelper(_context);
        //        if (!tk.ValidateToken(LoginKey))
        //        {
        //            objApiResponse.Status = Messages.APIStatusError;
        //            objApiResponse.Message = Messages.InvalidToken;
        //            return objApiResponse;
        //        }

        //        //SP_qrySurvScheduleList_Pager
        //        List<SqlParameter> lstSql = new List<SqlParameter>();
        //        lstSql.Add(new SqlParameter("@PageNo", PageNo));
        //        lstSql.Add(new SqlParameter("@PoolId", Pool_ID));
        //        lstSql.Add(new SqlParameter("@PageSize", GNF.PageSize));
        //        var qrySurvScheduleList = _context.qrySurvScheduleList.FromSql("SP_qrySurvScheduleList_Pager @PageNo= @PageNo, @PoolId=@PoolId, @PageSize= @PageSize", lstSql.ToArray()).ToList();
        //        objApiResponse.Message = "success";
        //        objApiResponse.Status = Messages.APIStatusSuccess;
        //        objApiResponse.ScheduleWeekLists = qrySurvScheduleList;
        //        using (var connection = _context.Database.GetDbConnection())
        //        {
        //            connection.Open();

        //            using (var command = connection.CreateCommand())
        //            {
        //                string SqlCount = "select count(*) from qrySurvScheduleList where PoolID = " + Pool_ID + " ";
        //                command.CommandText = SqlCount;
        //                var result = command.ExecuteScalar().ToString();

        //                objApiResponse.TotalCount = Convert.ToInt32(result);
        //            }
        //            connection.Close();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        objApiResponse.Message = "Exception: " + ex.Message;
        //        objApiResponse.Status = Messages.APIStatusError;

        //    }
        //    return objApiResponse;
        //}

        [HttpGet("GetNFLScheduleWeeksListByWeek")]
        public ScheduleWeekListAPIResponse GetNFLScheduleWeeksListByWeek(string LoginKey, int Pool_ID, int Week = 1)
        {
            ScheduleWeekListAPIResponse objApiResponse = new ScheduleWeekListAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                //SP_qrySurvScheduleList_Pager
                List<SqlParameter> lstSql = new List<SqlParameter>();
                lstSql.Add(new SqlParameter("@Week", Week));
                lstSql.Add(new SqlParameter("@PoolId", Pool_ID));

                var qrySurvScheduleList = _context.Query<qrySurvScheduleList>().FromSql("SP_qrySurvScheduleList_ByWeek @Week= @Week, @PoolId=@PoolId", lstSql.ToArray()).ToList();
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.ScheduleWeekLists = qrySurvScheduleList;
                using (var connection = _context.Database.GetDbConnection())
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        string SqlCount = "select count(*) from qrySurvScheduleList where PoolID = " + Pool_ID + " ";
                        command.CommandText = SqlCount;
                        var result = command.ExecuteScalar().ToString();

                        objApiResponse.TotalCount = Convert.ToInt32(result);
                    }
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
        /// <summary>
        /// //
        /// </summary>
        /// <param name="objWeek"></param>
        /// <param name="LoginKey"></param>
        /// <returns></returns>
        [HttpPost("CreatePoolWeek")]
        public APIResponses CreatePoolWeek([FromBody] PoolScheduleRequest objPoolSchedule, string LoginKey)
        {

            APIResponses objApiResponse = new APIResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                string ValidationMsg = ValidationHelper.GetErrorListFromModelState(ModelState);
                if (!string.IsNullOrEmpty(ValidationMsg))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = ValidationMsg;
                    return objApiResponse;
                }

                if (objPoolSchedule.WinnerTeamID > 0 && !(objPoolSchedule.WinnerTeamID == objPoolSchedule.VisitingTeamID || objPoolSchedule.WinnerTeamID == objPoolSchedule.HomeTeamID))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Winner team selection is wrong.";
                    return objApiResponse;
                }


                if (objPoolSchedule.ScheduleID > 0)
                {
                    SurvScheduleList survSchedule = _context.SurvScheduleList.FirstOrDefault(x => x.ScheduleID == objPoolSchedule.ScheduleID);
                    if (survSchedule != null)
                    {
                        survSchedule.Winner = objPoolSchedule.WinnerTeamID;
                        _context.SurvScheduleList.Add(survSchedule);
                        _context.Entry(survSchedule).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();

                        if (survSchedule.Winner != null)
                        {
                            //Eliminate and Winner cron
                            CommonController objCommon = new CommonController(_context);
                            objCommon.CronElimnation();
                            objCommon.CronWinners();
                        }
                    }

                }
                else
                {
                    SurvScheduleList survSchedule = _context.SurvScheduleList.FirstOrDefault(x => x.PoolID == objPoolSchedule.Pool_ID && x.WeekNumber == objPoolSchedule.Week_Number && x.HomeTeam == objPoolSchedule.HomeTeamID && x.VisitingTeam == objPoolSchedule.VisitingTeamID);

                    if (survSchedule != null)
                    {
                        objApiResponse.Status = Messages.APIStatusError;
                        objApiResponse.Message = "This entry is already exist.";
                        return objApiResponse;
                    }
                    else
                    {
                        SurvScheduleList obj = new SurvScheduleList();
                        obj.VisitingTeam = objPoolSchedule.VisitingTeamID;
                        obj.WeekNumber = objPoolSchedule.Week_Number;
                        obj.Winner = objPoolSchedule.WinnerTeamID;
                        obj.PoolID = objPoolSchedule.Pool_ID;
                        obj.HomeTeam = objPoolSchedule.HomeTeamID;
                        obj.ScheduleID = 0;

                        _context.SurvScheduleList.Add(obj);
                        _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        _context.SaveChanges();
                    }
                }
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = " Saved successfully";
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("DeletePoolSchedule")]
        public APIResponses DeletePoolSchedule(string LoginKey, int Pool_ID, int ScheduleID)
        {
            APIResponses objApiResponse = new APIResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }


                try
                {
                    string sSQL = "delete from SurvScheduleList where PoolID =" + Pool_ID + "";

                    //DB execute here
                    _context.Database.ExecuteSqlCommand(sSQL);
                }
                catch (Exception ex)
                {
                    objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                }
                try
                {
                    string sSQL = " delete from SurvScheduleList where ScheduleID = " + Pool_ID + "";

                    //DB execute here
                    _context.Database.ExecuteSqlCommand(sSQL);
                }
                catch (Exception ex)
                {
                    objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                }
                //var weekNumbers = _context.survPoolWeekList.OrderBy(x => x.WeekNumber).Where(x => x.PoolID == Pool_ID).Select(x => x.WeekNumber).ToList();
                var SurvScheduleList = _context.SurvScheduleList.FirstOrDefault(x => x.PoolID == Pool_ID && x.ScheduleID == ScheduleID);
                if (SurvScheduleList != null)
                {
                    _context.SurvScheduleList.Remove(SurvScheduleList);
                    _context.SaveChanges();
                }
                //var weekNumbers = _context.survPoolWeekList.FirstOrDefault(x => x.PoolID == Pool_ID && x.WeekNumber == WeekNumber);
                //if (weekNumbers != null)
                //{
                //    _context.survPoolWeekList.Remove(weekNumbers);
                //    _context.SaveChanges();
                //}
                objApiResponse.Message = "Schedule has been deleted!";
                objApiResponse.Status = Messages.APIStatusSuccess;

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Schedule has not been deleted due to the folowing error(s): " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
            }
            return objApiResponse;
        }

        [HttpGet("GetPoolSchedule")]
        public SurvScheduleListAPIResponse GetPoolSchedule(string LoginKey, int Pool_ID, int ScheduleID)
        {
            SurvScheduleListAPIResponse objApiResponse = new SurvScheduleListAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                var survSchedules = _context.SurvScheduleList.Where(x => x.PoolID == Pool_ID && x.ScheduleID == ScheduleID).FirstOrDefault();
                if (survSchedules != null)
                {
                    objApiResponse.Message = "No schedule found!";
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.survScheduleList = survSchedules;
                }
                else
                {
                    objApiResponse.Message = "No schedule found!";
                    objApiResponse.Status = Messages.APIStatusError;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "schedule not found due to the folowing error(s): " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
            }
            return objApiResponse;
        }

        [HttpGet("SurvScheduleListByScheduleId")]
        public SurvScheduleListResponse SurvScheduleListByScheduleId(string LoginKey, int ScheduleID)
        {
            SurvScheduleListResponse objApiResponse = new SurvScheduleListResponse();
            TokenHelper tk = new TokenHelper(_context);
            if (!tk.ValidateToken(LoginKey))
            {

                objApiResponse.Status = Messages.APIStatusError;
                objApiResponse.Message = Messages.InvalidToken;
                return objApiResponse;
            }
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
                SurvScheduleList survScheduleList = _context.SurvScheduleList.Where(x => x.ScheduleID == ScheduleID).FirstOrDefault();

                if (survScheduleList != null)
                {
                    objApiResponse.Message = "";
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.survSchedule = survScheduleList;
                }
                else
                {
                    objApiResponse.Message = "Nothing record found";
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.survSchedule = null;
                }
            }

            catch (Exception ex)
            {
                objApiResponse.Message = ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
                objApiResponse.survSchedule = null;
            }
            return objApiResponse;
        }

        [HttpGet("NFLScheduleFilter")]
        public NFLScheduleFilter NFLScheduleFilter(string LoginKey, string SeasonCode, string week)
        {
            NFLScheduleFilter objApiResponse = new NFLScheduleFilter();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                if (!string.IsNullOrEmpty(SeasonCode) && !string.IsNullOrEmpty(week))
                {
                    objApiResponse.nFLSchedules = _context.NFLSchedule.Where(x => x.Week == week && x.SeasonCode == SeasonCode).ToList();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Success";
                }
                else
                if (!string.IsNullOrEmpty(SeasonCode))
                {
                    objApiResponse.nFLSchedules = _context.NFLSchedule.Where(x => x.SeasonCode == SeasonCode).ToList();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Success";
                }
                else
                if (!string.IsNullOrEmpty(week))
                {
                    objApiResponse.nFLSchedules = _context.NFLSchedule.Where(x => x.Week == week).ToList();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Success";
                }
                else
                {
                    objApiResponse.nFLSchedules = _context.NFLSchedule.ToList();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Success";
                }

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "schedule not found due to the folowing error(s): " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
            }
            return objApiResponse;
        }

        [HttpGet("MainScheduleWeekList")]
        public NFLSheduleByWeekResponse MainScheduleWeekList(string LoginKey)
        {
            NFLSheduleByWeekResponse objApiResponse = new NFLSheduleByWeekResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                string Sql = "select distinct cast (week as int) week from NFLSchedule where week is not null order by cast(week as int) asc";
                objApiResponse.Message = "Success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.WeekList = _context.Query<NFLScheduleByWeek>().FromSql(Sql).ToList();

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "week not found due to the folowing error(s): " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
            }
            return objApiResponse;
        }

        [HttpGet("MainScheduleSeasonList")]
        public NFLSheduleBySeasoncodeResponse MainScheduleSeasonList(string LoginKey)
        {
            NFLSheduleBySeasoncodeResponse objApiResponse = new NFLSheduleBySeasoncodeResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                string Sql = "select distinct seasonCode from NFLSChedule where Seasoncode is not null";
                objApiResponse.Message = "Success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.SeasoncodeList = _context.Query<NFLScheduleBySeasoncode>().FromSql(Sql).ToList();
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Season not found due to the folowing error(s): " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
            }
            return objApiResponse;
        }

        [HttpGet("GetNFLScheduleByID")]
        public NFLScheduleResponse GetNFLScheduleByID(string LoginKey, int id)
        {
            NFLScheduleResponse objApiResponse = new NFLScheduleResponse();
            TokenHelper tk = new TokenHelper(_context);
            if (!tk.ValidateToken(LoginKey))
            {
                objApiResponse.Status = Messages.APIStatusError;
                objApiResponse.Message = Messages.InvalidToken;
                return objApiResponse;
            }
            try
            {
                NFLSchedule nflSchedule = _context.NFLSchedule.Where(x => x.Id == id).FirstOrDefault();

                if (nflSchedule != null)
                {
                    objApiResponse.Message = "Success";
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.NFLSchedule = nflSchedule;
                }
                else
                {
                    objApiResponse.Message = "Nothing record found";
                    objApiResponse.Status = Messages.APIStatusError;
                }
            }

            catch (Exception ex)
            {
                objApiResponse.Message = ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
            }
            return objApiResponse;
        }

        [HttpPost("DeleteNFLSchedule")]
        public APIResponses DeleteNFLSchedule(string LoginKey, int id)
        {
            APIResponses objApiResponse = new APIResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                var nflSchedule = _context.NFLSchedule.FirstOrDefault(x => x.Id == id);
                if (nflSchedule != null)
                {
                    _context.NFLSchedule.Remove(nflSchedule);
                    _context.SaveChanges();
                }

                objApiResponse.Message = "Schedule has been deleted!";
                objApiResponse.Status = Messages.APIStatusSuccess;

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Schedule has not been deleted due to the folowing error(s): " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
            }
            return objApiResponse;
        }

        [HttpPost("CreateUpdateNFLSchedule")]
        public NFLScheduleResponse CreateUpdateNFLSchedule([FromBody] NFLSchedule objNflSchedule, string LoginKey)
        {

            NFLScheduleResponse objApiResponse = new NFLScheduleResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                string ValidationMsg = ValidationHelper.GetErrorListFromModelState(ModelState);
                if (!string.IsNullOrEmpty(ValidationMsg))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = ValidationMsg;
                    return objApiResponse;
                }


                if (objNflSchedule.Id > 0)
                {
                    NFLSchedule objSchedule = _context.NFLSchedule.FirstOrDefault(x => x.Id == objNflSchedule.Id);
                    if (objSchedule != null)
                    {
                        objSchedule.Attendance = objNflSchedule.Attendance;
                        objSchedule.Broadcasters = objNflSchedule.Broadcasters;
                        objSchedule.CutOffDate = objNflSchedule.CutOffDate;
                        objSchedule.DelayedOrPostponedReason = objNflSchedule.DelayedOrPostponedReason;
                        objSchedule.EndedTime = objNflSchedule.EndedTime;
                        objSchedule.GameDate = objNflSchedule.GameDate;
                        objSchedule.HomeTeamId = objNflSchedule.HomeTeamId;
                        objSchedule.HomeTeamShort = objNflSchedule.HomeTeamShort;
                        objSchedule.Officials = objNflSchedule.Officials;
                        objSchedule.OriginalStartTime = objNflSchedule.OriginalStartTime;
                        objSchedule.PlayedStatus = objNflSchedule.PlayedStatus;
                        objSchedule.StartTime = objNflSchedule.GameDate.ToString("MM-dd-yyyy hh:mm:ss tt");
                        objSchedule.VenueAllegiance = objNflSchedule.VenueAllegiance;
                        objSchedule.Venue_ID = objNflSchedule.Venue_ID;
                        objSchedule.VisitingTeamID = objNflSchedule.VisitingTeamID;
                        objSchedule.VisitingTeamShort = objNflSchedule.VisitingTeamShort;
                        objSchedule.SeasonCode = objNflSchedule.SeasonCode;
                        objSchedule.Week = objNflSchedule.Week;
                        _context.NFLSchedule.Add(objSchedule);
                        _context.Entry(objSchedule).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();
                        objApiResponse.Status = Messages.APIStatusSuccess;
                        objApiResponse.Message = " Update successfully";
                        objApiResponse.NFLSchedule = objSchedule;
                    }

                }
                else
                {
                    NFLSchedule objSchedule = new NFLSchedule();
                    int id = _context.NFLSchedule.Max(x => x.Id);
                    objSchedule.Id = id + 1;
                    objSchedule.Attendance = objNflSchedule.Attendance;
                    objSchedule.Broadcasters = objNflSchedule.Broadcasters;
                    objSchedule.CutOffDate = objNflSchedule.CutOffDate;
                    objSchedule.DelayedOrPostponedReason = objNflSchedule.DelayedOrPostponedReason;
                    objSchedule.EndedTime = objNflSchedule.EndedTime;
                    objSchedule.GameDate = objNflSchedule.GameDate;
                    objSchedule.HomeTeamId = objNflSchedule.HomeTeamId;
                    objSchedule.HomeTeamShort = objNflSchedule.HomeTeamShort;
                    objSchedule.Officials = objNflSchedule.Officials;
                    objSchedule.OriginalStartTime = objNflSchedule.OriginalStartTime;
                    objSchedule.PlayedStatus = objNflSchedule.PlayedStatus;
                    objSchedule.StartTime = objNflSchedule.GameDate.ToString("MM-dd-yyyy hh:mm:ss tt");
                    objSchedule.VenueAllegiance = objNflSchedule.VenueAllegiance;
                    objSchedule.Venue_ID = objNflSchedule.Venue_ID;
                    objSchedule.VisitingTeamID = objNflSchedule.VisitingTeamID;
                    objSchedule.VisitingTeamShort = objNflSchedule.VisitingTeamShort;
                    objSchedule.SeasonCode = objNflSchedule.SeasonCode;
                    objSchedule.Week = objNflSchedule.Week;

                    _context.NFLSchedule.Add(objSchedule);
                    _context.Entry(objSchedule).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = " Saved successfully";
                    objApiResponse.NFLSchedule = objSchedule;
                }

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
    }

}