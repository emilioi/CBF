using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class PicksController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public PicksController(CbfDbContext context)
        {
            _context = context;
        }

        [HttpPost("SetDefaultPicks")]
        public APIResponses SetDefaultPicks([FromBody] PoolDefaultAPIRequest defaultAPIRequest, string LoginKey)
        {
            //  List<qrySurvScheduleList> lstSurvScheduleList = defaultAPIRequest.weeklyDefaults;
            List<WeeklyDefaults> weeklyDefaults = defaultAPIRequest.weeklyDefaults;
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
                if (!ModelState.IsValid)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.ModelStateInvalid;
                    return objApiResponse;
                }
                List<Pool_Defaults> PoolDefaultList = _context.Pool_Defaults.Where(x => x.Pool_Id == weeklyDefaults[0].PoolID && x.WeekNumber == weeklyDefaults[0].WeekNumber).ToList();
                foreach (WeeklyDefaults objDefaults in weeklyDefaults)
                {
                    if (objDefaults.ID > 0)
                    {
                        Pool_Defaults defaults = PoolDefaultList.FirstOrDefault(x => x.Id == objDefaults.ID);
                        if (defaults != null)
                        {
                            defaults.Rank = objDefaults.Rank;
                            _context.Add(defaults);
                            _context.Entry(defaults).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                        }
                    }
                    //else
                    //{
                    //    _context.Add(objDefaults);
                    //    _context.Entry(objDefaults).State = Microsoft.EntityFrameworkCore.EntityState.Added;

                    //}
                }

                _context.SaveChanges();


                #region Call Defaulted API
                //CommonController objCommon = new CommonController(_context);
                //objCommon.CronPickDefaulted();
                #endregion

                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = "Updated successfully";
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("GetWeeklyDefaultsSchedule")]//GetWeeklyDefaultsScheduleForPublic
        public PoolDefaultAPIResponse GetWeeklyDefaultsSchedule(string LoginKey, int Pool_ID, int Week = 1)
        {
            PoolDefaultAPIResponse objApiResponse = new PoolDefaultAPIResponse();
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

                var WeeklyDefaults = _context.Query<WeeklyDefaults>().FromSql("SP_WeeklyDefaults_ByPoolWeek @Week= @Week, @PoolId=@PoolId", lstSql.ToArray()).ToList();
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.weeklyDefaults = WeeklyDefaults;


            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);

            }
            return objApiResponse;
        }
        [HttpGet("PicksReport")]
        public PickReportResponses PicksReport(string LoginKey, int PoolId, int WeekNumber)
        {

            PickReportResponses objApiResponse = new PickReportResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                List<PickReport> PickReport = new List<PickReport>();
                Token objToken = tk.GetTokenByKey(LoginKey);
                if (objToken != null)
                {
                    Administrators obj = _context.Administrators.Where(x => x.Member_Id == objToken.UserId).FirstOrDefault();
                    if (obj != null && obj.Admin_Type == "GroupAdmin")
                    {
                        PickReport = _context.Query<PickReport>().FromSql("exec SP_Entry_Picks_Report_GroupAdmin @PoolId=" + PoolId + ",@WeekNumber=" + WeekNumber + ",@AdminId=" + obj.Member_Id).ToList();
                    }
                    else
                    {
                        PickReport = _context.Query<PickReport>().FromSql("exec SP_Entry_Picks_Report @PoolId=" + PoolId + ",@WeekNumber=" + WeekNumber).ToList();
                    }
                }
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.PickReport = PickReport;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("MakePick")]
        public APIResponses MakePick([FromBody] PickRequest defaultAPIRequest, string LoginKey)
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
                Token objToken = tk.GetTokenByKey(LoginKey);
                if (objToken != null)
                {
                    SurvEntries objSurvEntries = _context.SurvEntries.FirstOrDefault(x => x.EntryID == defaultAPIRequest.EntryID);
                    if (objSurvEntries != null)
                    {
                        int weekNumber = Convert.ToInt32(defaultAPIRequest.WeekNumber);
                        SurvPoolWeekList objPoolList = _context.survPoolWeekList.FirstOrDefault(x => x.WeekNumber == weekNumber && x.PoolID == objSurvEntries.PoolID);
                        if (objPoolList != null && objPoolList.Start) //objPoolList.CutOff < DateTime.Now ||
                        {
                            objApiResponse.Status = Messages.APIStatusError;
                            objApiResponse.Message = Messages.WeekClosed;
                            return objApiResponse;
                        }

                        if (objSurvEntries.Eliminated)
                        {
                            objApiResponse.Status = Messages.APIStatusError;
                            objApiResponse.Message = "Your Tikcet is Eliminated. You can not change or make a pick.";
                            return objApiResponse;
                        }
                    }
                    try
                    {
                        var objOldPick = _context.SurvEntryPicks.FromSql<SurvEntryPicks>("Exec SP_SurvEntryPicks_MemberIdEntryIDWinner @MemberID=" + objToken.UserId + ",@EntryID=" + defaultAPIRequest.EntryID + ",@Winner=" + defaultAPIRequest.YourWinner).FirstOrDefault();
                        // //  _context.SurvEntries.Where(x => x.MemberID == objToken.UserId) SurvEntryPicks.Where(x => x.Winner == defaultAPIRequest.YourWinner && x.EntryID != defaultAPIRequest.EntryID).FirstOrDefault();
                        if (objOldPick != null)
                        {
                            objApiResponse.Status = Messages.APIStatusError;
                            objApiResponse.Message = Messages.CanNotSelectATeamTwice;
                            return objApiResponse;
                        }
                    }
                    catch (Exception ex)
                    {
                        GNF.SaveException(ex, _context);
                        objApiResponse.Status = Messages.APIStatusError;
                        objApiResponse.Message = "An error occured. Our technical team is reviewing this issue.";
                        return objApiResponse;
                    }
                    bool IsModified = false;
                    SurvEntryPicks objPicks = _context.SurvEntryPicks.FirstOrDefault(x => x.EntryID == defaultAPIRequest.EntryID && x.WeekNumber == defaultAPIRequest.WeekNumber);
                    //SurvScheduleList survSchedule = _context.SurvScheduleList.FirstOrDefault(x => x.ScheduleID == defaultAPIRequest.ScheduleID);
                    if (objPicks != null)
                    {
                        IsModified = true;
                        _context.SurvEntryPicks.Remove(objPicks);
                        // _context.SaveChanges();
                    }
                    //if (objPicks == null)
                    //{
                    objPicks = new SurvEntryPicks();

                    objPicks.Created = DateTime.Now;
                    objPicks.Defaulted = false;
                    objPicks.Eliminated = false;
                    objPicks.EntryID = defaultAPIRequest.EntryID;
                    objPicks.ScheduleID = defaultAPIRequest.ScheduleID;
                    objPicks.Winner = defaultAPIRequest.YourWinner;
                    objPicks.WeekNumber = defaultAPIRequest.WeekNumber;
                    _context.SurvEntryPicks.Add(objPicks);
                    _context.Entry(objPicks).State = EntityState.Added;
                    // }
                    //else
                    //{
                    //    objPicks.Created = DateTime.Now;
                    //    objPicks.Defaulted = false;
                    //    objPicks.Eliminated = false;
                    //    objPicks.EntryID = defaultAPIRequest.EntryID;
                    //    objPicks.ScheduleID = defaultAPIRequest.ScheduleID;
                    //    objPicks.Winner = defaultAPIRequest.YourWinner;

                    //    _context.SurvEntryPicks.Add(objPicks);
                    //    _context.Entry(objPicks).State = EntityState.Modified;
                    //}
                    _context.SaveChanges();

                    //
                    var objPickNotification = _context.Query<PickbyEntryNotification>().FromSql("exec [SP_PickByEntryId] @EntryId=" + objPicks.EntryID + ",@weekNumber=" + objPicks.WeekNumber).FirstOrDefault();

                    try
                    {
                        NotificationHelper.sendNotificationPickTicket(objPickNotification, _context);
                    }
                    catch (Exception ex)
                    {
                        GNF.SaveException(ex, _context);
                    }
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = IsModified ? Messages.UpdatedSuccess : Messages.SavedSuccess;
                }
                else
                {

                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;

                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }


        [HttpPost("MakePickTwiceValidate")]
        public APIResponses MakePickTwiceValidate([FromBody] PickRequest defaultAPIRequest, string LoginKey)
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
                Token objToken = tk.GetTokenByKey(LoginKey);
                if (objToken != null)
                {
                    //SurvEntries objSurvEntries = _context.SurvEntries.FirstOrDefault(x => x.EntryID == defaultAPIRequest.EntryID);
                    //if (objSurvEntries != null)
                    //{
                    //    int weekNumber = Convert.ToInt32(defaultAPIRequest.WeekNumber);
                    //    SurvPoolWeekList objPoolList = _context.survPoolWeekList.FirstOrDefault(x => x.WeekNumber == weekNumber && x.PoolID == objSurvEntries.PoolID);
                    //    if (objPoolList != null && objPoolList.Start) //objPoolList.CutOff < DateTime.Now ||
                    //    {
                    //        objApiResponse.Status = Messages.APIStatusError;
                    //        objApiResponse.Message = Messages.WeekClosed;
                    //        return objApiResponse;
                    //    }

                    //    if (objSurvEntries.Eliminated)
                    //    {
                    //        objApiResponse.Status = Messages.APIStatusError;
                    //        objApiResponse.Message = "Your Tikcet is Eliminated. You can not change or make a pick.";
                    //        return objApiResponse;
                    //    }
                    //}
                    try
                    {
                        var objOldPick = _context.SurvEntryPicks.FromSql<SurvEntryPicks>("Exec SP_SurvEntryPicks_MemberIdEntryIDWinner @MemberID=" + objToken.UserId + ",@EntryID=" + defaultAPIRequest.EntryID + ",@Winner=" + defaultAPIRequest.YourWinner).FirstOrDefault();
                        // //  _context.SurvEntries.Where(x => x.MemberID == objToken.UserId) SurvEntryPicks.Where(x => x.Winner == defaultAPIRequest.YourWinner && x.EntryID != defaultAPIRequest.EntryID).FirstOrDefault();
                        if (objOldPick != null)
                        {
                            objApiResponse.Status = Messages.APIStatusError;
                            objApiResponse.Message = Messages.CanNotSelectATeamTwice;
                            return objApiResponse;
                        }
                    }
                    catch (Exception ex)
                    {
                        GNF.SaveException(ex, _context);
                        objApiResponse.Status = Messages.APIStatusError;
                        objApiResponse.Message = "An error occured. Our technical team is reviewing this issue.";
                        return objApiResponse;
                    }

                }
                else
                {

                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;

                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        //Pick center
        [HttpGet("GetPickCenterSchedule")]
        public PickCenterSchedule GetPickCenterSchedule(string LoginKey, int Pool_ID, int Week = 1)
        {
            PickCenterSchedule objApiResponse = new PickCenterSchedule();
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

                List<PickCenterSchedulesList> qrySurvScheduleList = _context.Query<PickCenterSchedulesList>().FromSql("SP_qrySurvScheduleList_ByWeek @Week= @Week, @PoolId=@PoolId", lstSql.ToArray()).ToList();

                List<SqlParameter> lstSql2 = new List<SqlParameter>();
                lstSql2.Add(new SqlParameter("@Week", Week));
                lstSql2.Add(new SqlParameter("@PoolId", Pool_ID));

                List<ScheduleGroupByDateTime> lstScheduleGroupByDateTime = _context.Query<ScheduleGroupByDateTime>().FromSql("SP_Pick_Center_GroupByDateTime @Week= @Week, @PoolId=@PoolId", lstSql2.ToArray()).ToList();

                //Get Dates
                List<SchedulesGroupDates> lstSchedulesGroupDates =
                    lstScheduleGroupByDateTime.GroupBy(item => item.Date).Select(group => new SchedulesGroupDates
                    {
                        ScheduleDate = Convert.ToDateTime(group.Key),
                        ScheduleDateString = Convert.ToDateTime(group.Key).ToLongDateString()
                    }).ToList();

                //Get Times
                foreach (SchedulesGroupDates objDate in lstSchedulesGroupDates)
                {

                    List<ScheduleGroupTime> lstScheduleGroupTime =
                 lstScheduleGroupByDateTime.Where(x => x.Date == objDate.ScheduleDate).GroupBy(item => item.Time).Select(group => new ScheduleGroupTime
                 {
                     ScheduleTime = (group.Key),
                     ScheduleTimeString = new DateTime(group.Key.Ticks).ToString("hh:mm tt")
                 }).ToList();

                    foreach (ScheduleGroupTime objTime in lstScheduleGroupTime)
                    {
                        List<int> schedulesIds =
                            lstScheduleGroupByDateTime.Where(x => x.Date == objDate.ScheduleDate && x.Time == objTime.ScheduleTime).Select(c => c.ScheduleId).ToList();

                        List<PickCenterSchedulesList> schedulesLists = qrySurvScheduleList.Where(x => schedulesIds.Contains(x.ScheduleID)).ToList();

                        objTime.ScheduleWeekLists = schedulesLists;
                    }

                    objDate.ScheduleGroupTime = lstScheduleGroupTime;
                }

                //foreach (ScheduleGroupByDateTime objSch in lstScheduleGroupByDateTime)
                //{
                //    SchedulesGroupDates objGroupDate = new SchedulesGroupDates();
                //    objGroupDate.ScheduleDate = Convert.ToDateTime(objSch.Date);
                //    objGroupDate.ScheduleDateString = Convert.ToDateTime(objSch.Date).ToLongDateString();
                //    //WORK here
                //    if (!lstSchedulesGroupDates.Contains(objGroupDate))
                //    {
                //        lstSchedulesGroupDates.Add(objGroupDate);
                //    }

                //}
                //
                //foreach (SchedulesGroupDates objDate in lstSchedulesGroupDates)
                //{
                //    List<PickCenterSchedulesList> lstPickCenterSchedulesList = new List<PickCenterSchedulesList>();
                //    List<ScheduleGroupTime> lstScheduleGroupTime = new List<ScheduleGroupTime>();
                //    foreach (ScheduleGroupByDateTime objSch in lstScheduleGroupByDateTime)
                //    {
                //        if (objDate.ScheduleDateString == Convert.ToDateTime(objSch.Date).ToLongDateString())
                //        {
                //            ScheduleGroupTime objGroupTime = new ScheduleGroupTime();
                //            objGroupTime.ScheduleTime = objSch.Time;
                //            objGroupTime.ScheduleTimeString =  objSch.Time.ToString();
                //            //if (objGroupTime.ScheduleWeekLists == null)
                //            //{
                //            //    objGroupTime.ScheduleWeekLists = new List<PickCenterSchedulesList>();
                //            //}
                //            //objGroupTime.ScheduleWeekLists.Add(qrySurvScheduleList.FirstOrDefault(x => x.ScheduleID == objSch.ScheduleId));

                //            if (!lstScheduleGroupTime.Contains(objGroupTime))
                //            {
                //                lstScheduleGroupTime.Add(objGroupTime);
                //            }
                //        }
                //    }




                //    objDate.ScheduleGroupTime = lstScheduleGroupTime;
                //}
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.SchedulesGroup = lstSchedulesGroupDates;


            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("PickReportWithLogo")]
        public PickReportWithLogoResponses PickReportWithLogo(string LoginKey, int EntryId)
        {

            PickReportWithLogoResponses objApiResponse = new PickReportWithLogoResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                List<PickReportWithLogo> PickReport = _context.Query<PickReportWithLogo>().FromSql("exec SP_Entry_Picks_Report_WithLogo @EntryId=" + EntryId).ToList();
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.PickReport = PickReport;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("PickAnalysisWithTeam")]
        public PickAnalysisWithTeamResponses PickAnalysisWithTeam(string LoginKey, int PoolId)
        {

            PickAnalysisWithTeamResponses objApiResponse = new PickAnalysisWithTeamResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                List<PickAnalysis> pickAnalysis = new List<PickAnalysis>();

             //   Pool_Master pool = _context.Pool_Master.Find(PoolId); 
                
                pickAnalysis = _context.Query<PickAnalysis>().FromSql("exec SP_Pick_Analysis_all_combined @poolId=" + PoolId).ToList();
                //if (pool != null && pool.Sport_Type == 1)
                //{PickAnalysisEliminatedByMember
                   

                    try
                    {
                        PickReportCount pickReportCounts = _context.Query<PickReportCount>().FromSql("exec SP_Pick_Analysis_Counts_combined @poolId=" + PoolId).FirstOrDefault();
                        objApiResponse.PickReportCount = pickReportCounts; 
                        
                        List<PickAnalysis> pickAnalysisFlag = _context.Query<PickAnalysis>().FromSql("exec SP_Pick_Analysis_ElimnatedFlags_combined @poolId=" + PoolId).ToList();
                    objApiResponse.PickAnalysisFlag = pickAnalysisFlag;
                    }
                    catch (Exception ex)
                    {
                        GNF.SaveException(ex, _context);
                    }
                //}
                //else if(pool != null && pool.Sport_Type == 2)
                //{
                //    pickAnalysis = _context.Query<PickAnalysis>().FromSql("exec SP_Pick_Analysis_All_NHL @poolId=" + PoolId).ToList();

                //    try
                //    {
                //        PickReportCount pickReportCounts = _context.Query<PickReportCount>().FromSql("exec SP_Pick_Analysis_Counts_NHL @poolId=" + PoolId).FirstOrDefault();
                //        objApiResponse.PickReportCount = pickReportCounts; 
                        
                //        List<PickAnalysis> pickAnalysisFlag = _context.Query<PickAnalysis>().FromSql("exec SP_Pick_Analysis_ElimnatedFlags_NHL @poolId=" + PoolId).ToList();
                //    objApiResponse.PickAnalysisFlag = pickAnalysisFlag;
                //    }
                //    catch (Exception ex)
                //    {
                //        GNF.SaveException(ex, _context);
                //    }


                //}
                
                
                
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.PickAnalysis = pickAnalysis;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("PickAnalysisWithTeamDynamic")]
        public async Task<PickAnalysisWithTeamDynamicResponses> PickAnalysisWithTeamDynamic(int poolId)
        {
            PickAnalysisWithTeamDynamicResponses objApiResponse = new PickAnalysisWithTeamDynamicResponses();
            try
            {
                if (poolId != null)
                {
                    DataTable dt = new DataTable();
                    using (SqlConnection sqlcon = new SqlConnection(GlobalConfig.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("exec SP_Pick_Analysis_All_dynamic @poolId=" + poolId, sqlcon))
                        {
                            cmd.CommandType = CommandType.Text;

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {


                                da.Fill(dt);
                            }
                        }
                    }
                    var dns = new List<dynamic>();

                    foreach (var item in dt.AsEnumerable())
                    {
                        // Expando objects are IDictionary<string, object>
                        IDictionary<string, object> dn = new ExpandoObject();

                        foreach (var column in dt.Columns.Cast<DataColumn>())
                        {
                            dn[column.ColumnName] = item[column];
                        }

                        dns.Add(dn);
                    }

                    //  var pickReport = await _context.Database.ExecuteSqlCommand("exec SP_Pick_Analysis_All_dynamic @poolId=" + poolId);
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Succesfull";
                    objApiResponse.PickReportAnalysis = dns;
                    return objApiResponse;
                }
            }
            catch(Exception ex)
            {

            }
            objApiResponse.Status = Messages.APIStatusError;
            objApiResponse.Message = "Something went wrong!";
            return objApiResponse;
        }

        //Total Alive picks by Member for open week
        [HttpGet("PickAnalysisAliveByMember")]
        public PickAnalysisAliveByMemberResponses PickAnalysisAliveByMember(string LoginKey, int PoolId, int Member_Id, bool AllTickets = false, string searchText = "")
        {
            PickAnalysisAliveByMemberResponses objApiResponse = new PickAnalysisAliveByMemberResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                if (!AllTickets)
                {
                    List<TotalAliveByMemberReport> pickAliveReport = _context.Query<TotalAliveByMemberReport>().FromSql("exec SP_Tikcet_Pick_Analysis_Alive_By_MemberId @poolId=" + PoolId + ",@Member_Id=" + Member_Id).ToList();
                    objApiResponse.PickAnalysisAlive = pickAliveReport;
                }
                else
                {
                    List<TotalAliveByMemberReport> pickAliveReport = _context.Query<TotalAliveByMemberReport>().FromSql("exec SP_Tikcet_Pick_Analysis_Alive_By_PoolId @poolId=" + PoolId).ToList();
                    objApiResponse.PickAnalysisAlive = pickAliveReport;
                }
                try
                {
                    List<PickAnalysis> pickAnalysisFlag = _context.Query<PickAnalysis>().FromSql("exec SP_Pick_Analysis_ElimnatedFlags @poolId=" + PoolId).ToList();
                    objApiResponse.PickAnalysisFlag = pickAnalysisFlag;

                }
                catch (Exception ex)
                {
                    GNF.SaveException(ex, _context);
                }

                try
                {
                    PickReportCount pickReportCounts = _context.Query<PickReportCount>().FromSql("exec SP_Pick_Analysis_Counts @poolId=" + PoolId).FirstOrDefault();
                    objApiResponse.PickReportCount = pickReportCounts;
                }
                catch (Exception ex)
                {
                    GNF.SaveException(ex, _context);
                }
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("PickAnalysisAliveByMemberLazyLoad")]
        public PickAnalysisAliveByMemberResponses PickAnalysisAliveByMemberLazyLoad(string LoginKey, int PoolId, int Member_Id, bool AllTickets = false, int PageNo = 1, string searchText = "")
        {
            int PageSize = 20;
            PickAnalysisAliveByMemberResponses objApiResponse = new PickAnalysisAliveByMemberResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                if (!string.IsNullOrEmpty(searchText))
                {

                    if (!AllTickets)
                    {
                        List<TotalAliveByMemberReport> pickAliveReport = _context.Query<TotalAliveByMemberReport>().FromSql("exec SP_Tikcet_Pick_Analysis_Alive_By_MemberId_Search @poolId=" + PoolId + ",@Member_Id=" + Member_Id + ",@searchText='" + searchText + "'").ToList();
                        objApiResponse.PickAnalysisAlive = pickAliveReport;
                    }
                    else
                    {
                        List<TotalAliveByMemberReport> pickAliveReport = _context.Query<TotalAliveByMemberReport>().FromSql("exec SP_Tikcet_Pick_Analysis_Alive_By_PoolId_Pager_Search @poolId=" + PoolId + ",@PageSize=" + PageSize + ",@PageNo=" + PageNo + ",@searchText='" + searchText + "'").ToList();
                        objApiResponse.PickAnalysisAlive = pickAliveReport;
                    }
                }
                else
                {
                    if (!AllTickets)
                    {
                        List<TotalAliveByMemberReport> pickAliveReport = _context.Query<TotalAliveByMemberReport>().FromSql("exec SP_Tikcet_Pick_Analysis_Alive_By_MemberId @poolId=" + PoolId + ",@Member_Id=" + Member_Id).ToList();
                        objApiResponse.PickAnalysisAlive = pickAliveReport;
                    }
                    else
                    {
                        List<TotalAliveByMemberReport> pickAliveReport = _context.Query<TotalAliveByMemberReport>().FromSql("exec SP_Tikcet_Pick_Analysis_Alive_By_PoolId_Pager @poolId=" + PoolId + ",@PageSize=" + PageSize + ",@PageNo=" + PageNo).ToList();
                        objApiResponse.PickAnalysisAlive = pickAliveReport;
                    }
                }
                try
                {
                    List<PickAnalysis> pickAnalysisFlag = _context.Query<PickAnalysis>().FromSql("exec SP_Pick_Analysis_ElimnatedFlags @poolId=" + PoolId).ToList();
                    objApiResponse.PickAnalysisFlag = pickAnalysisFlag;

                }
                catch (Exception ex)
                {
                    GNF.SaveException(ex, _context);
                }

                try
                {
                    PickReportCount pickReportCounts = _context.Query<PickReportCount>().FromSql("exec SP_Pick_Analysis_Counts @poolId=" + PoolId).FirstOrDefault();
                    objApiResponse.PickReportCount = pickReportCounts;
                }
                catch (Exception ex)
                {
                    GNF.SaveException(ex, _context);
                }
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        //Total Eliminated picks by Member for open week
        [HttpGet("PickAnalysisEliminatedByMember")]
        public PickAnalysisEliminatedByMemberResponses PickAnalysisEliminatedByMember(string LoginKey, int PoolId, int Member_Id, string searchText = "")
        {
            PickAnalysisEliminatedByMemberResponses objApiResponse = new PickAnalysisEliminatedByMemberResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                List<TotalEliminatedByMemberReport> pickEliminatedReport = new List<TotalEliminatedByMemberReport>();
                if (!string.IsNullOrEmpty(searchText))
                {
                    pickEliminatedReport = _context.Query<TotalEliminatedByMemberReport>().FromSql("exec SP_Tikcet_Pick_Analysis_Eliminated_By_MemberId_Search_Combined @poolId=" + PoolId + ",@Member_Id=" + Member_Id + ",@searchText='" + searchText + "'").ToList();

                }
                else
                {
                    pickEliminatedReport = _context.Query<TotalEliminatedByMemberReport>().FromSql("exec SP_Tikcet_Pick_Analysis_Eliminated_By_MemberId_Combined @poolId=" + PoolId + ",@Member_Id=" + Member_Id).ToList();

                }
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.PickAnalysisEliminated = pickEliminatedReport;
                try
                {
                    PickReportCount pickReportCounts = _context.Query<PickReportCount>().FromSql("exec SP_Pick_Analysis_Counts @poolId=" + PoolId).FirstOrDefault();
                    objApiResponse.PickReportCount = pickReportCounts;
                }
                catch (Exception ex)
                {
                    GNF.SaveException(ex, _context);
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }


        [HttpGet("GetWeeklyDefaultsScheduleForPublic")]//GetWeeklyDefaultsScheduleForPublic
        public PoolDefaultAPIResponse GetWeeklyDefaultsScheduleForPublic(string LoginKey, int Pool_ID)
        {
            int CurrentWeek = 1;
            try
            {
                CurrentWeek = _context.Query<CustomInt>().FromSql("Exec SP_GetCurrentWeekNumber").FirstOrDefault().WeekNumber;
            }
            catch (Exception ex)
            {

            }
            PoolDefaultAPIResponse objApiResponse = new PoolDefaultAPIResponse();
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
                lstSql.Add(new SqlParameter("@Week", CurrentWeek));
                lstSql.Add(new SqlParameter("@PoolId", Pool_ID));

                var WeeklyDefaults = _context.Query<WeeklyDefaults>().FromSql("SP_WeeklyDefaults_ByPoolWeekOnlyClosed @Week= @Week, @PoolId=@PoolId", lstSql.ToArray()).ToList();
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.weeklyDefaults = WeeklyDefaults;


            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);

            }
            return objApiResponse;
        }


        [HttpGet("GetCurrentWeek")]//GetWeeklyDefaultsScheduleForPublic
        public CurrentWeek GetCurrentWeek()
        {
            int CurrentWeek = 1;
            CurrentWeek objApiResponse = new CurrentWeek();

            try
            {
                CurrentWeek = _context.Query<CustomInt>().FromSql("Exec SP_GetCurrentWeekNumber").FirstOrDefault().WeekNumber;
                objApiResponse.WeekNumber = CurrentWeek;
            }
            catch (Exception ex)
            {

            }

            return objApiResponse;
        }
    }
}