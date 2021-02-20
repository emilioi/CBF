using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CBF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class PoolMasterController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public PoolMasterController(CbfDbContext context)
        {
            _context = context;
        }

        #region API for Admin Module
        [HttpGet]
        public PoolMaintainceListResponses GetPool_Master(string LoginKey)
        {
            PoolMaintainceListResponses objApiResponse = new PoolMaintainceListResponses();
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
                var PoolMaster = _context.Pool_Master.ToList();
                objApiResponse.Message = "found";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Pool_Master = PoolMaster;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("GetPool_Master")]
        public PoolMaintainceResponses GetPool_Master(int id, string LoginKey)
        {

            PoolMaintainceResponses objApiResponse = new PoolMaintainceResponses();
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

                var objPool_Master = _context.Pool_Master.Find(id);
                if (objPool_Master == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Nothing found.";
                    return objApiResponse;
                }
                PoolMaintaince obj = new PoolMaintaince();
                obj.Pool_Name = objPool_Master.Pool_Name;
                obj.Pool_ID = id;
                obj.Price = objPool_Master.Price;
                obj.Is_Started = objPool_Master.Is_Started;
                obj.Rules_URL = objPool_Master.Rules_URL;
                obj.Sport_Type = objPool_Master.Sport_Type;
                obj.Description = objPool_Master.Description;
                obj.Cut_Off = objPool_Master.Cut_Off;
                obj.Private = objPool_Master.Private;
                obj.ThrusdayGames = objPool_Master.ThrusdayGames;
                obj.SaturdayGames = objPool_Master.SaturdayGames;
                obj.Ruler_Season = objPool_Master.Ruler_Season;
                obj.PassCode = objPool_Master.PassCode;
                obj.Image_Url = objPool_Master.Image_Url;

                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = " found.";
                objApiResponse.Maintaince = obj;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("PostPool_Master")]
        public Pool_MasterResponses PostPool_Master([FromBody] Pool_Master_Request objPoolMaster, string LoginKey, bool build_schedule = false, bool thursdayGames = false, bool saturdayGames = false, bool sundayGames = false)
        {
            //build_schedule NFL/NHL Regular schedule checkbox

            Pool_MasterResponses objApiResponse = new Pool_MasterResponses();
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
                //CutOff Date Fix
                DateTime Cut_Off = GNF.GetFormattedDateDDMMYYYY(objPoolMaster.Cut_Off);
                if (Cut_Off <= DateTime.MinValue)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Invalid Cut-Off date, Please use dd/MM/yyyy";
                    return objApiResponse;
                }
                if (objPoolMaster != null && (objPoolMaster.Sport_Type == null || objPoolMaster.Sport_Type <= 0))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Please select valid Sport Type.";
                    return objApiResponse;
                }

                //Pool_Master obj = new Pool_Master();

                var obj = _context.Pool_Master.FirstOrDefault(x => x.Pool_ID == objPoolMaster.Pool_ID);
                if (obj != null)
                {
                    obj.Pool_Name = objPoolMaster.Pool_Name;
                    obj.Pool_ID = objPoolMaster.Pool_ID;
                    obj.Price = objPoolMaster.Price;
                    obj.Ruler_Season = objPoolMaster.Ruler_Season;
                    obj.Is_Started = objPoolMaster.Is_Started;
                    obj.Rules_URL = objPoolMaster.Rules_URL;
                    obj.Sport_Type = objPoolMaster.Sport_Type;
                    obj.Description = objPoolMaster.Description;
                    obj.Cut_Off = Cut_Off;
                    obj.Is_Active = true;
                    obj.DTS = DateTime.Now;
                    obj.Private = objPoolMaster.Private;
                    obj.ThrusdayGames = objPoolMaster.ThrusdayGames;
                    obj.SaturdayGames = objPoolMaster.SaturdayGames;
                    obj.SundayGames = objPoolMaster.SundayGames;
                    obj.PassCode = objPoolMaster.PassCode;
                    _context.Pool_Master.Attach(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    if (obj.Is_Started == true)
                    {
                        objApiResponse.Status = Messages.APIStatusSuccess;
                        objApiResponse.Message = "Pool " + objPoolMaster.Pool_Name + " officially closed.";
                    }
                    //else if(obj.Is_Started == false)
                    // {
                    //     objApiResponse.Status = Messages.APIStatusSuccess;
                    //     objApiResponse.Message = "Pool " + objPoolMaster.Pool_Name + " officially opened.";
                    // }
                    else
                    {
                        objApiResponse.Status = Messages.APIStatusSuccess;
                        objApiResponse.Message = Messages.UpdatedSuccess;
                    }

                    objApiResponse.Pool_Master = obj;
                }
                else if (objPoolMaster != null && objPoolMaster.Pool_ID == 0)
                {
                    obj = new Pool_Master();
                    obj.Pool_Name = objPoolMaster.Pool_Name;
                    obj.Pool_ID = objPoolMaster.Pool_ID;
                    obj.Price = objPoolMaster.Price;
                    obj.Ruler_Season = objPoolMaster.Ruler_Season;
                    obj.Is_Started = objPoolMaster.Is_Started;
                    obj.Rules_URL = objPoolMaster.Rules_URL;
                    obj.Sport_Type = objPoolMaster.Sport_Type;
                    obj.Description = objPoolMaster.Description;
                    obj.Cut_Off = Cut_Off;
                    obj.Is_Active = true;
                    obj.DTS = DateTime.Now;
                    obj.Private = objPoolMaster.Private;
                    obj.ThrusdayGames = objPoolMaster.ThrusdayGames;
                    obj.SaturdayGames = objPoolMaster.SaturdayGames;
                    obj.PassCode = objPoolMaster.PassCode;
                    obj.SundayGames = objPoolMaster.SundayGames;
                    _context.Pool_Master.Attach(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.Pool_Master = obj;
                }
                else
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.ModelStateInvalid;
                }

                if (build_schedule && obj.Pool_ID > 0 && objPoolMaster.Pool_ID == 0)
                {
                    try
                    {
                        if (obj.Sport_Type == 1) //NFL 
                        {
                            string sSQL = "exec prInsertNFLScheduleToPool  @poolId=" + obj.Pool_ID + ",@thursdayGames=" + thursdayGames + ",@saturdayGames=" + saturdayGames;
                            int res = _context.Database.ExecuteSqlCommand(sSQL);

                        }
                        else if (obj.Sport_Type == 2) //NHL
                        {
                            string sSQL = "exec prInsertNHLScheduleToPool  @poolId=" + obj.Pool_ID + ",@sundayGames=" + sundayGames;// + ",@saturdayGames=" + saturdayGames;
                            int res = _context.Database.ExecuteSqlCommand(sSQL);
                            //Do nothing for now
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("CopyPoolMaster")]
        public Pool_MasterResponses CopyPoolMaster(int OldPoolID, string LoginKey)
        {
            //build_schedule NFL/NHL Regular schedule checkbox
            int NewPoolID = 0;

            Pool_MasterResponses objApiResponse = new Pool_MasterResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }



                Pool_Master obj = _context.Pool_Master.FirstOrDefault(x => x.Pool_ID == OldPoolID);
                if (obj != null)
                {
                    Pool_Master objNewPool = new Pool_Master();
                    objNewPool.Pool_ID = 0;
                    objNewPool.Cut_Off = obj.Cut_Off;
                    objNewPool.Description = obj.Description;
                    objNewPool.DTS = obj.DTS;
                    objNewPool.Is_Active = obj.Is_Active;
                    objNewPool.Is_Deleted = obj.Is_Deleted;
                    objNewPool.Is_Started = obj.Is_Started;
                    objNewPool.Last_Edited_By = obj.Last_Edited_By;
                    objNewPool.Pool_Name = obj.Pool_Name;
                    objNewPool.Price = obj.Price;
                    objNewPool.Ruler_Season = obj.Ruler_Season;
                    objNewPool.Rules_URL = obj.Rules_URL;
                    objNewPool.Sport_Type = obj.Sport_Type;
                    objNewPool.Private = obj.Private;
                    objNewPool.ThrusdayGames = obj.ThrusdayGames;
                    objNewPool.SaturdayGames = obj.SaturdayGames;
                    objNewPool.SundayGames = obj.SundayGames;
                    objNewPool.PassCode = obj.PassCode;

                    _context.Pool_Master.Add(objNewPool);
                    _context.Entry(objNewPool).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Copied successfully.";
                    objApiResponse.Pool_Master = objNewPool;

                    //New pool id
                    NewPoolID = objNewPool.Pool_ID;

                    //Insert into SurvPoolWeekList
                    string sSQL = (" insert into SurvPoolWeekList(PoolID,WeekNumber,CutOff,Start,Updated) select "
                + (NewPoolID + (" as PoolID ,WeekNumber,CutOff,Start,getDate() as Updated from SurvPoolWeekList where PoolID=" + OldPoolID)));

                    _context.Database.ExecuteSqlCommand(sSQL);

                    //Looping by SurvScheduleList

                    int ScheduleId = 0;
                    sSQL = (" select ScheduleID,"
               + (NewPoolID + (" as PoolID,WeekNumber,Description,HomeTeam,VisitingTeam,Winner from SurvScheduleList where PoolID=" + OldPoolID)));

                    var survScheduleList = _context.Query<NFLScheduleList>().FromSql(sSQL).ToList();
                    foreach (NFLScheduleList objSchedule in survScheduleList)
                    {
                        //Implement the code
                        // sSQL = ("insert into SurvScheduleList(ScheduleID,PoolID,WeekNumber,Description,HomeTeam,VisitingTeam,Winner)" + (" values("
                        //+ (ScheduleId + (","
                        //+ (objSchedule("PoolID") + (","
                        //+ (objSchedule("WeekNumber") + (",\'"
                        //+ (objSchedule("Description") + ("\',"
                        //+ (objSchedule("HomeTeam") + (","
                        //+ (objSchedule("VisitingTeam") + (","
                        //+ (objSchedule("Winner") + ")")))))))))))))));
                    }


                }

                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = "Pool Has been copied successfully!";
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Pool Was Not Copied Due to the following error(s): " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
                // objResponse.Members = null;
            }
            return objApiResponse;
        }

        [HttpDelete("{id}")]
        public PoolMaintainceResponses DeletePool_Master([FromRoute] int id, string LoginKey)
        {
            PoolMaintainceResponses objApiResponse = new PoolMaintainceResponses();
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

                //Pool Master delete logic
                try
                {
                    string sSQL = "delete from SurvEntryPicks where EntryID in (select EntryID from  SurvEntries where PoolID=" + id + ")";

                    //DB execute here
                    _context.Database.ExecuteSqlCommand(sSQL);
                }
                catch (Exception ex)
                {
                    objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                }
                try
                {
                    string sSQL = ("delete from SurvEntries where PoolID=" + id);
                    //Db execute here
                    _context.Database.ExecuteSqlCommand(sSQL);

                }
                catch (Exception ex)
                {
                    objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                }
                try
                {
                    string sSQL = ("delete from SurvScheduleList where PoolID=" + id);
                    //Db execute here
                    _context.Database.ExecuteSqlCommand(sSQL);
                }
                catch (Exception ex)
                {
                    objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                }
                try
                {
                    string sSQL = ("delete from SurvPoolWeekList where PoolID=" + id);
                    //db execute here
                    _context.Database.ExecuteSqlCommand(sSQL);
                }
                catch (Exception ex)
                {
                    objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                }
                try
                {
                    string sSQL = ("delete from Pool_Master where Pool_ID=" + id);
                    //db execute
                    _context.Database.ExecuteSqlCommand(sSQL);
                }
                catch (Exception ex)
                {
                    objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                }
                objApiResponse.Message = Messages.DeleteSuccess;
                objApiResponse.Status = Messages.APIStatusSuccess;
                return objApiResponse;
            }
            catch (Exception ex)
            {

            }
            return objApiResponse;
        }
        [HttpGet("GetWeekList")]
        public SurvPoolWeekListResponse GetWeekList(string LoginKey)
        {
            SurvPoolWeekListResponse objApiResponse = new SurvPoolWeekListResponse();
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
                    objApiResponse.Status = "0";
                    objApiResponse.Message = "Invalid object, Please verify the data.";
                    return objApiResponse;
                }
                var WeekList = _context.survPoolWeekList.ToList();
                objApiResponse.Message = "found";
                objApiResponse.Status = "1";
                objApiResponse.SurvPoolWeekList = WeekList;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);

            }
            return objApiResponse;
        }
        #region PUblic Join A Pool
        //[HttpPost("JoinAPool")]
        //public EntryMenuResponse JoinAPool([FromBody] EntryMenu objPool, string LoginKey, int Pool_Id, string Pool_Name)
        //{
        //    EntryMenuResponse objApiResponse = new EntryMenuResponse();
        //    try
        //    {
        //        TokenHelper tk = new TokenHelper(_context);
        //        if (!tk.ValidateToken(LoginKey))
        //        {
        //            objApiResponse.Status = "0";
        //            objApiResponse.Message = "Invalid Token";
        //            return objApiResponse;
        //        }

        //        EntryMenu obj = _context.Query<EntryMenu>().FirstOrDefault(x => x.Pool_ID == Pool_Id && x.Pool_Name == Pool_Name);
        //        if (obj != null)
        //        {
        //            // obj.Pool_Name = objPool.Pool_Name;
        //            obj.TheCount = objPool.TheCount;
        //            _context.EntryMenus.Add(obj);
        //            _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
        //            _context.SaveChanges();

        //            objApiResponse.Status = "1";
        //            objApiResponse.Message = "Copied successfully.";
        //            objApiResponse.EntryMenus = obj;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objApiResponse.Message = "Pool Was Not Copied Due to the following error(s): " + ex.Message;
        //        objApiResponse.Status = "0";

        //    }
        //    return objApiResponse;
        //}
        #endregion
        private bool Pool_MasterExists(int id)
        {
            return _context.Pool_Master.Any(e => e.Pool_ID == id);
        }


        #endregion


        #region API for Public Module
        [HttpGet("PoolListForMemberClubHouse")]
        public PoolListResponse PoolListForMemberClubHouse(string LoginKey, bool AllClubs = false)
        {

            int CurrentWeek = 1;
            
            PoolListResponse objApiResponse = new PoolListResponse();
            TokenHelper tk = new TokenHelper(_context);
            if (!tk.ValidateToken(LoginKey))
            {
                objApiResponse.Message = Messages.InvalidToken;
                objApiResponse.Status = Messages.APIStatusError;
                return objApiResponse;
            }

            Token objToken = tk.GetTokenByKey(LoginKey);
            if (objToken != null && !AllClubs)
            {
                try
                {
                    List<Club> clubs = _context.Query<Club>().FromSql("exec SP_ClubHouseBy_MemberId @MemberID=" + objToken.UserId).ToList();

                    foreach (Club objClub in clubs)
                    {
                        objClub.CutOffDateString = objClub.Cut_Off.AddMinutes(-1).ToString("dddd, dd MMMM yyyy hh:mm tt");
                        List<PoolWeekList> poolWeekLists = new List<PoolWeekList>();
                        PoolWeekList objWeek = new PoolWeekList();
                        PickCustomCount objPickCustomCount = _context.Query<PickCustomCount>().FromSql("Exec SP_All_Picks_Counts @poolId=" + objClub.Pool_ID).FirstOrDefault();
                        if (objPickCustomCount != null)
                        {
                            objWeek.EntriesCount = objPickCustomCount.AllEntriesCount;
                            objWeek.MembersCount = objPickCustomCount.TotalMemberCount;
                            objWeek.PicksMadeCount = objPickCustomCount.PickMadeCount;
                            objWeek.NoPicksCount = objPickCustomCount.NoPickCount;
                            objWeek.EliminatedCount = objPickCustomCount.EliminatedCount;
                            objWeek.AliveCount = objPickCustomCount.TotalAliveCount;
                        }


                        // objWeek.EntriesCount = //_context.SurvEntries.Count(x => x.PoolID == objClub.Pool_ID);
                        //                         (from p in _context.SurvEntries
                        //                          join m in _context.Members on p.MemberID equals m.Member_Id
                        //                          where p.PoolID == objClub.Pool_ID
                        //                          select new { p.EntryID }).Count();

                        //objWeek.MembersCount = (from p in _context.SurvEntries
                        //                        join m in _context.Members on p.MemberID equals m.Member_Id
                        //                        where p.PoolID == objClub.Pool_ID
                        //                        select new { p.MemberID }).Distinct().Count();

                        // objWeek.PicksMadeCount = (from p in _context.SurvEntryPicks
                        //                           join e in _context.SurvEntries on p.EntryID equals e.EntryID
                        //                           join m in _context.Members on e.MemberID equals m.Member_Id
                        //                           where e.PoolID == objClub.Pool_ID && p.WeekNumber == CurrentWeek && !e.Eliminated
                        //                           select new { p.EntryID }).Count();
                        // objWeek.NoPicksCount = _context.Query<CustomCount>().FromSql("Exec SP_EntryWithoutPicksCountByPoolId @poolId=" + objClub.Pool_ID).FirstOrDefault().Count;//  
                        try
                        {
                            CurrentWeek = _context.Query<CustomInt>().FromSql("Exec SP_GetCurrentWeekNumberByPoolId @PoolId=" + objClub.Pool_ID).FirstOrDefault().WeekNumber;
                        }
                        catch (Exception ex)
                        {

                        }
                        objWeek.WeekNumber = CurrentWeek;
                        try
                        {
                            // string SQL = ("SELECT  SurvEntries.EntryID, SurvEntries.PoolID, ISNULL(qrySurvEntryPicksCountSubQuery.NumOfPicks, 0) AS NumOfPicks FROM SurvEntries LEFT OUTER JOIN qrySurvEntryPicksCountSubQuery ON SurvEntries.PoolID = qrySurvEntryPicksCountSubQuery.PoolID AND SurvEntries.EntryID = qrySurvEntryPicksCountSubQuery.EntryID where SurvEntries.PoolID= "+objClub.Pool_ID);
                            //var poolWeekList = _context.PicksCount.Where(x => x.PoolID == objClub.Pool_ID).ToList();
                            //foreach (PicksCount objWeek in poolWeekList)
                            //{
                            //    //PoolWeekList objPoolWeek = new PoolWeekList();
                            //    //objPoolWeek.EntriesCount = 0;
                            //    //objPoolWeek.WeekNumber = objWeek.WeekNumber;
                            //    //objPoolWeek.NoPicksCount = _context. ;
                            //    //objPoolWeek.PicksMadeCount = _context. ;
                            //    //objPoolWeek.EntriesCount = _context.SurvEntryPicks.Where(x => x.pool_Id = objClub.Pool_ID).count();

                            //    poolWeekList.Add(objWeek);

                            //}
                            //objClub.PoolWeekLists = poolWeekList;
                        }
                        catch (Exception ex)
                        {
                            objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                        }
                        //  objClub.PoolWeekLists = poolWeekLists;
                        List<MostPickedList> mostPickedLists = new List<MostPickedList>();

                        //string sSQL = ("SELECT top 3 0 as 'PickCount', Pool_Master.Pool_ID, Sports_Type.SportType, Sports_Type.SportTypeName, NFLTeam.Team_Id, NFLTeam.Team_Name, NFLTeam.Abbreviation, NFLTeam.LogoImageSrc FROM Sports_Type INNER JOIN Pool_Master ON Sports_Type.SportType = Pool_Master.Sport_Type INNER JOIN NFLTeam  ON Sports_Type.SportType = NFLTeam.SportType AND Sports_Type.SportType = NFLTeam.SportType  where Pool_Master.Pool_ID= " + objClub.Pool_ID);

                        //var MostPickedLists = _context.Query<MostPickedList>().FromSql("exec SP_Pool_Master_3_MOST_PICKED_TEAMS @Pool_Id=" + objClub.Pool_ID + ", @WeekNumber=" + 1).ToList();
                        //foreach (MostPickedList objMostPicked in MostPickedLists)
                        //{
                        //    mostPickedLists.Add(objMostPicked);
                        //}
                        objWeek.MostPickedTeams = mostPickedLists;
                        poolWeekLists.Add(objWeek);
                        objClub.PoolWeekLists = poolWeekLists;
                    }
                    objApiResponse.clubs = clubs;
                    objApiResponse.Message = "";
                    objApiResponse.Status = Messages.APIStatusSuccess;
                }
                catch (Exception ex)
                {
                    objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                }
            }
            else if (objToken != null && AllClubs)
            {
                try
                {
                    CommonController commonController = new CommonController(_context);
                    ClubSettingResponse objSetting = commonController.ClubSettings();
                    if (objSetting != null && objSetting.settings != null)
                    {
                        if (objSetting.settings[0].Lookup_Name == "RegistrationOpen" && objSetting.settings[0].Lookup_Value == "true")
                        {
                            List<Club> clubs = _context.Query<Club>().FromSql("exec SP_ClubHouseAllBy_MemberId @MemberID=" + objToken.UserId).ToList();

                            foreach (Club objClub in clubs)
                            {
                                objClub.CutOffDateString = objClub.Cut_Off.AddMinutes(-1).ToString("dddd, dd MMMM yyyy hh:mm tt");
                                List<PoolWeekList> poolWeekLists = new List<PoolWeekList>();
                                PoolWeekList objWeek = new PoolWeekList();
                                PickCustomCount objPickCustomCount = _context.Query<PickCustomCount>().FromSql("Exec SP_All_Picks_Counts @poolId=" + objClub.Pool_ID).FirstOrDefault();
                                if (objPickCustomCount != null)
                                {
                                    objWeek.EntriesCount = objPickCustomCount.AllEntriesCount;
                                    objWeek.MembersCount = objPickCustomCount.TotalMemberCount;
                                    objWeek.PicksMadeCount = objPickCustomCount.PickMadeCount;
                                    objWeek.NoPicksCount = objPickCustomCount.NoPickCount;
                                    objWeek.EliminatedCount = objPickCustomCount.EliminatedCount;
                                    objWeek.AliveCount = objPickCustomCount.TotalAliveCount;
                                }
                                //objWeek.EntriesCount = //_context.SurvEntries.Count(x => x.PoolID == objClub.Pool_ID);
                                //                        (from p in _context.SurvEntries
                                //                         join m in _context.Members on p.MemberID equals m.Member_Id
                                //                         where p.PoolID == objClub.Pool_ID
                                //                         select new { p.EntryID }).Count();

                                //objWeek.MembersCount = (from p in _context.SurvEntries
                                //                        join m in _context.Members on p.MemberID equals m.Member_Id
                                //                        where p.PoolID == objClub.Pool_ID
                                //                        select new { p.MemberID }).Distinct().Count();

                                //objWeek.PicksMadeCount = (from p in _context.SurvEntryPicks
                                //                          join e in _context.SurvEntries on p.EntryID equals e.EntryID
                                //                          join m in _context.Members on e.MemberID equals m.Member_Id
                                //                          where e.PoolID == objClub.Pool_ID && p.WeekNumber == CurrentWeek && !e.Eliminated
                                //                          select new { p.EntryID }).Count();
                                //objWeek.NoPicksCount = _context.Query<CustomCount>().FromSql("Exec SP_EntryWithoutPicksCountByPoolId @poolId=" + objClub.Pool_ID).FirstOrDefault().Count;
                                try
                                {
                                    CurrentWeek = _context.Query<CustomInt>().FromSql("Exec SP_GetCurrentWeekNumberByPoolId @PoolId=" + objClub.Pool_ID).FirstOrDefault().WeekNumber;
                                }
                                catch (Exception ex)
                                {

                                }
                                objWeek.WeekNumber = CurrentWeek;
                                List<MostPickedList> mostPickedLists = new List<MostPickedList>();

                                //var MostPickedLfists = _context.Query<MostPickedList>().FromSql("exec SP_Pool_Master_3_MOST_PICKED_TEAMS @Pool_Id=" + objClub.Pool_ID + ", @WeekNumber=" + 1).ToList();
                                //foreach (MostPickedList objMostPicked in MostPickedLists)
                                //{
                                //    mostPickedLists.Add(objMostPicked);
                                //}
                                objWeek.MostPickedTeams = mostPickedLists;
                                poolWeekLists.Add(objWeek);
                                objClub.PoolWeekLists = poolWeekLists;
                            }
                            objApiResponse.clubs = clubs;
                            objApiResponse.Message = "";
                            objApiResponse.Status = Messages.APIStatusSuccess;
                        }
                        else
                        {
                            objApiResponse.clubs = new List<Club>();
                            objApiResponse.Message = "";
                            objApiResponse.Status = Messages.APIStatusSuccess;
                        }
                    }
                    else
                    {
                        objApiResponse.clubs = new List<Club>();
                        objApiResponse.Message = "";
                        objApiResponse.Status = Messages.APIStatusSuccess;
                    }

                }
                catch (Exception ex)
                {
                    objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                }
            }
            else
            {
                objApiResponse.Message = Messages.InvalidToken;
                objApiResponse.Status = Messages.APIStatusError;
            }
            return objApiResponse;

        }

        #endregion

        [HttpPost("PoolFileUploadBase64")]
        public TeamUploadResponses PoolFileUploadBase64([FromBody] ImageBase64 image, string LoginKey, int Pool_Id)
        {
            TeamUploadResponses objApiResponse = new TeamUploadResponses();
            try
            {
                Guid obj = Guid.NewGuid();
                string ImageUniqueId = obj.ToString();
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {

                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;

                    return objApiResponse;
                }
                Pool_Master objPool = _context.Pool_Master.Find(Pool_Id);
                if (objPool != null && image != null && !string.IsNullOrEmpty(image.fileExtention) && !string.IsNullOrEmpty(image.base64image))
                {
                    string fileName = Pool_Id + "-" + ImageUniqueId + "." + image.fileExtention;

                    //Backend
                    string folderPath = GlobalConfig.PoolProfileUploadUrlBackend;// @"../cbfAdmin/assets/profile/members/";
                    DirectoryInfo di = new DirectoryInfo(folderPath);
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                    string base64Extracted = image.base64image.Replace("data:image/png;base64,", String.Empty);
                    base64Extracted = base64Extracted.Replace("data:image/jpg;base64,", String.Empty);
                    base64Extracted = base64Extracted.Replace("data:image/jpeg;base64,", String.Empty);
                    base64Extracted = base64Extracted.Replace("data:image/bmp;base64,", String.Empty);
                    base64Extracted = base64Extracted.Replace("data:image/gif;base64,", String.Empty);
                    var bytes = Convert.FromBase64String(base64Extracted);


                    string file = Path.Combine(folderPath, fileName);

                    if (bytes.Length > 0)
                    {
                        using (var stream = new FileStream(file, FileMode.Create))
                        {
                            stream.Write(bytes, 0, bytes.Length);
                            stream.Flush();
                        }
                    }
                    //Frontend
                    string folderPathpub = GlobalConfig.PoolProfileUploadUrlPublic;// @"../cbfpublic/assets/profile/members/";

                    DirectoryInfo dipub = new DirectoryInfo(folderPathpub);
                    if (!dipub.Exists)
                    {
                        dipub.Create();
                    }
                    string base64Extractedpub = image.base64image.Replace("data:image/png;base64,", String.Empty);
                    base64Extractedpub = base64Extractedpub.Replace("data:image/jpg;base64,", String.Empty);
                    base64Extractedpub = base64Extractedpub.Replace("data:image/jpeg;base64,", String.Empty);
                    base64Extractedpub = base64Extractedpub.Replace("data:image/bmp;base64,", String.Empty);
                    base64Extractedpub = base64Extractedpub.Replace("data:image/gif;base64,", String.Empty);
                    var bytespub = Convert.FromBase64String(base64Extractedpub);


                    string filepub = Path.Combine(folderPathpub, fileName);

                    if (bytespub.Length > 0)
                    {
                        using (var stream = new FileStream(filepub, FileMode.Create))
                        {
                            stream.Write(bytespub, 0, bytespub.Length);
                            stream.Flush();
                        }
                    }


                    objPool.Image_Url = fileName;

                    _context.Pool_Master.Attach(objPool);
                    _context.SaveChanges();

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Image uploaded successfully.";
                    objApiResponse.FileName = fileName;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Failed: Please use only JPG, PNG, GIF images.";
                objApiResponse.Status = Messages.APIStatusError;
                GNF.SaveException(ex, _context);
            }
            return objApiResponse; //null just to make error free
        }

    }
}