using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CBF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public EntriesController(CbfDbContext context)
        {
            _context = context;
        }

        #region API for Admin Module

        [HttpGet("GetEntryMenu")]
        public EntryMenuAPIResponse GetEntryMenu(string LoginKey)
        {
            EntryMenuAPIResponse objApiResponse = new EntryMenuAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                List<EntryMenu> EntryMenus = new List<EntryMenu>();//
                Token objToken = tk.GetTokenByKey(LoginKey);
                if (objToken != null)
                {
                    Administrators obj = _context.Administrators.Where(x => x.Member_Id == objToken.UserId).FirstOrDefault();
                    if (obj != null && obj.Admin_Type == "GroupAdmin")
                    {
                        EntryMenus = _context.Query<EntryMenu>().FromSql("exec SP_GetSurvEntryCountByGroupAdmin @GroupAdminID=" + objToken.UserId).ToList();
                    }
                    else
                    {
                        EntryMenus = _context.Query<EntryMenu>().FromSql("exec SP_GetSurvEntryCount").ToList();
                    }
                }
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.EntryMenus = EntryMenus;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);

            }
            return objApiResponse;
        }
        [HttpGet("GetEntriesWeeksList")]
        public EntryListAPIResponse GetEntriesWeeksList(string LoginKey, int Pool_ID)
        {
            EntryListAPIResponse objApiResponse = new EntryListAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                List<qrySurvEntries> survEntryWeekLists = _context.Query<qrySurvEntries>().FromSql("exec SP_GetqrySurvEntries @Pool_ID=" + Pool_ID).ToList();
                //string Sql = "select PoolID, Pool_Name, EntryID, EntryName, Eliminated, Login_ID, MemberID, FullName, Price, Active, Defaults from qrySurvEntries  where PoolID = " + Pool_ID + " order by CASE WHEN ISNUMERIC(EntryName) = 1 THEN ABS(EntryName) END ASC, CASE WHEN ISNUMERIC(EntryName) = 0 THEN EntryName END ASC";
                // var survEntryWeekLists = _context.qrySurvEntries.FromSql(Sql).ToList();
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.EntryWeekLists = survEntryWeekLists;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
        [HttpGet("SearchEntry")]
        public EntryListAPIResponse SearchEntry(string LoginKey, int Pool_ID, string EntryName)
        {
            EntryListAPIResponse objApiResponse = new EntryListAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                if (string.IsNullOrEmpty(EntryName) || string.IsNullOrWhiteSpace(EntryName))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Entry name is empty or invalid.";
                    return objApiResponse;
                }
                List<qrySurvEntries> survEntryWeekLists = _context.Query<qrySurvEntries>().FromSql("exec SP_GetqrySurvEntries @Pool_ID=" + Pool_ID).ToList();

                if (survEntryWeekLists != null && survEntryWeekLists.Count > 0)
                {
                    survEntryWeekLists = survEntryWeekLists.Where(x => x.EntryName.ToLower().Contains(EntryName.ToLower())).ToList();
                }
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.EntryWeekLists = survEntryWeekLists;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
        [HttpGet("GetEntryById")]
        public EntryAPIResponse GetEntryById(int EntryID, string LoginKey)
        {

            EntryAPIResponse objApiResponse = new EntryAPIResponse();
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


                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.SurvEntries = _context.SurvEntries.Find(EntryID);
                try
                {
                    objApiResponse.Pool_Master = _context.Pool_Master.Find(objApiResponse.SurvEntries.PoolID);
                }
                catch(Exception ex)
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
        [HttpGet("GetEntryPickById")]
        public EntryPickAPIResponse GetEntryPickById(int EntryID, string LoginKey)
        {

            EntryPickAPIResponse objApiResponse = new EntryPickAPIResponse();
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
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.SurvEntryPicks = _context.SurvEntryPicks.FirstOrDefault(x => x.EntryID == EntryID);

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
        [HttpGet("GetEntryPickListById")]
        public EntryPickListAPIResponse GetEntryPickListById(string LoginKey, int EntryID)
        {

            EntryPickListAPIResponse objApiResponse = new EntryPickListAPIResponse();
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
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.SurvEntryPicks = _context.SurvEntryPicks.Where(x => x.EntryID == EntryID).ToList();

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
        [HttpDelete("DeletePoolEntriesById")]
        public EntryWeekAPIResponse DeletePoolEntriesById(int EntryID, string LoginKey)
        {
            EntryWeekAPIResponse objApiResponse = new EntryWeekAPIResponse();
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
                //var objEntries = _context.qrySurvEntries.Find(EntryID);
                //if (objEntries == null)
                //{
                //    objApiResponse.Status =  Messages.StatusError;
                //    objApiResponse.Message = Messages.InvalidToken;
                //    return objApiResponse;
                //}
                //string Sql = "Delete from qrySurvEntries where EntryID = " + EntryID + "";
                //  _context.Database.ExecuteSqlCommand(Sql);
                var SurvEntryPicks = _context.SurvEntryPicks.FirstOrDefault(x => x.EntryID == EntryID);
                if (SurvEntryPicks != null)
                {
                    _context.SurvEntryPicks.Remove(SurvEntryPicks);
                    _context.SaveChanges();
                }
                var SurvEntries = _context.SurvEntries.FirstOrDefault(x => x.EntryID == EntryID);
                if (SurvEntries != null)
                {
                    _context.SurvEntries.Remove(SurvEntries);
                    _context.SaveChanges();
                }
                try
                {
                    string sSQL = "delete from SurvEntries where EntryID=" + EntryID + "";

                    //DB execute here
                    _context.Database.ExecuteSqlCommand(sSQL);
                }
                catch (Exception ex)
                {
                    objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                }
                objApiResponse.Message = "Deleted successfully";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.EntryWeekLists = null;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;

        }
        [HttpGet("EntriesListWithoutPickByPoolandWeek")]
        public SurvEntries_WithoutPicksResponses EntriesListWithoutPickByPoolandWeek(string LoginKey, int PoolId, int WeekNumber)
        {

            SurvEntries_WithoutPicksResponses objApiResponse = new SurvEntries_WithoutPicksResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                var EntryList = _context.Query<qrySurvEntriesNew>().FromSql("exec SP_SurvEntries_WithoutPicks @PoolId=" + PoolId + ",@WeekNumber=" + WeekNumber).Distinct().ToList();
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.SurvEntries_WithoutPicks = EntryList;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
        [HttpPost("TransferTickets")]
        public APIResponses TransferTickets(string LoginKey, int EntryId, int NewOwnerId)
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
                    var entry = _context.SurvEntries.FirstOrDefault(x => x.EntryID == EntryId);
                    if (entry != null)
                    {
                        var oldMember = _context.Members.FirstOrDefault(x => x.Member_Id == entry.MemberID);
                        var newMember = _context.Members.FirstOrDefault(x => x.Member_Id == NewOwnerId);

                        entry.MemberID = NewOwnerId;
                        _context.SurvEntries.Add(entry);
                        _context.Entry(entry).State = EntityState.Modified;
                        _context.SaveChanges();

                        try
                        {
                            NotificationHelper.sendNotoficationToNewOwnerMoveTicket(oldMember, newMember, entry, _context);
                            NotificationHelper.sendNotoficationToOldOwnerMoveTicket(oldMember, newMember, entry, _context);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Transfered Successfully";

                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("GetTicketByPoolId")]
        public TicketByPoolIdResponses GetTicketByPoolId(string LoginKey, int PoolId)
        {
            TicketByPoolIdResponses objApiResponse = new TicketByPoolIdResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                List<TicketByPoolId> TicketByPoolId = _context.Query<TicketByPoolId>().FromSql("exec SP_GetTIcketByPoolId @PoolId=" + PoolId).ToList();
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.TicketByPoolId = TicketByPoolId;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
        #endregion

        #region API for Public Module
        [HttpPost("JoinClubAddTickets")]
        public TicketsAPIResponse JoinClubAddTickets(string LoginKey, int PoolId, int NoOfTickets)
        {
            TicketsAPIResponse objApiResponse = new TicketsAPIResponse();
            try
            {
                Pool_Master objPoolMaster = _context.Pool_Master.FirstOrDefault(x => x.Pool_ID == PoolId);

                // if (objPoolMaster.Cut_Off < DateTime.Now)
                //{
                //    objApiResponse.Status = Messages.APIStatusError;
                //    objApiResponse.Message = Messages.ClubClosed;
                //    return objApiResponse;
                //}
                if (NoOfTickets > 50)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.maxticketallowed;
                    return objApiResponse;
                }
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

                    Members objMembers = _context.Members.FirstOrDefault(x => x.Member_Id == objToken.UserId);

                    List<SurvEntries> survEntries = new List<SurvEntries>();
                    int MaxEntry = 0;
                    if (_context.SurvEntries.Count() > 0)
                    {
                        MaxEntry = _context.SurvEntries.Max(x => x.EntryID);
                    }

                    for (int i = 1; i <= NoOfTickets; i++)
                    {
                        MaxEntry++;
                        SurvEntries objEntry = new SurvEntries();
                        objEntry.PoolID = PoolId;
                        objEntry.MemberID = objToken.UserId;
                        objEntry.EntryID = MaxEntry;
                        objEntry.EntryName = MaxEntry.ToString("00000");
                        survEntries.Add(objEntry);
                    }

                    foreach (SurvEntries entry in survEntries)
                    {
                        SurvEntries objTicket = new SurvEntries();
                        objTicket.EntryID = 0;
                        objTicket.EntryName = entry.EntryName;
                        objTicket.Active = true;
                        objTicket.CreatedDT = DateTime.Now;
                        objTicket.Defaults = entry.Defaults;
                        objTicket.Eliminated = entry.Eliminated;
                        objTicket.LastUpdated = DateTime.Now;
                        objTicket.MemberID = entry.MemberID;
                        objTicket.PoolID = entry.PoolID;

                        _context.SurvEntries.Add(objTicket);
                        _context.Entry(objTicket).State = EntityState.Added;

                    }
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.SurvEntries = _context.SurvEntries.Where(x => x.MemberID == objToken.UserId && x.PoolID == PoolId).ToList();
                    //objResponse.Status = Messages.APIStatusSuccess;
                    //objResponse.Message = "";
                    //objResponse.SurvEntries = survEntries;

                    NotificationHelper.SendNotificationClubJoinConfirmation(objMembers, objPoolMaster, survEntries, _context);

                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("GetTicketsByMemberId")]
        public TicketsDetailAPIResponse GetTicketsByMemberId(string LoginKey)
        {
            TicketsDetailAPIResponse objApiResponse = new TicketsDetailAPIResponse();
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


                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "";
                    objApiResponse.SurvEntries = _context.Query<SurvEntryPicksDetails>().FromSql("exec SP_TicketList_ByMemberId @member_Id=" + objToken.UserId).ToList();

                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        //[HttpPost("JoinClubSaveTickets")]
        //public TicketsAPIResponse JoinClubSaveTickets([FromBody] List<SurvEntries> tickets, string LoginKey, int PoolId)
        //{
        //    TicketsAPIResponse objResponse = new TicketsAPIResponse();
        //    try
        //    {
        //        TokenHelper tk = new TokenHelper(_context);
        //        if (!tk.ValidateToken(LoginKey))
        //        {
        //            objResponse.Status = Messages.APIStatusError;
        //            objResponse.Message = Messages.InvalidToken;
        //            return objResponse;
        //        }
        //        Token objToken = tk.GetTokenByKey(LoginKey);
        //        if (objToken != null)
        //        {
        //            List<SurvEntries> survEntries = new List<SurvEntries>();

        //            foreach (SurvEntries entry in tickets)
        //            {
        //                SurvEntries objTicket = new SurvEntries();
        //                objTicket.EntryID = 0;
        //                objTicket.EntryName = entry.EntryName;
        //                objTicket.Active = entry.Active;
        //                objTicket.CreatedDT = DateTime.Now;
        //                objTicket.Defaults = entry.Defaults;
        //                objTicket.Eliminated = entry.Eliminated;
        //                objTicket.LastUpdated = DateTime.Now;
        //                objTicket.MemberID = entry.MemberID;
        //                objTicket.PoolID = entry.PoolID;


        //                _context.SurvEntries.Add(objTicket);
        //                _context.Entry(objTicket).State = EntityState.Added;

        //            }
        //            _context.SaveChanges();
        //            objResponse.Status = Messages.APIStatusSuccess;
        //            objResponse.Message = Messages.SavedSuccess;
        //            objResponse.SurvEntries = _context.SurvEntries.Where(x => x.MemberID == objToken.UserId && x.PoolID == PoolId).ToList();
        //            try
        //            {
        //                NotificationHelper.SendNotificationTicketPurchase(objToken, survEntries, _context);
        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objResponse.Message = "Exception: " + ex.Message;
        //        objResponse.Status = Messages.APIStatusError;
        //    }
        //    return objResponse;
        //}
        //[HttpGet("GetEntryByPoolAndWeek")]
        //public EntryAPIResponse GetEntryByPoolAndWeek(int PoolId, int WeekNumber, string LoginKey)
        //{

        //    EntryAPIResponse objApiResponse = new EntryAPIResponse();
        //    try
        //    {
        //        TokenHelper tk = new TokenHelper(_context);
        //        if (!tk.ValidateToken(LoginKey))
        //        {
        //            objApiResponse.Status = Messages.APIStatusError;
        //            objApiResponse.Message = Messages.InvalidToken;
        //            return objApiResponse;
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            objApiResponse.Status = Messages.APIStatusError;
        //            objApiResponse.Message = Messages.ModelStateInvalid;
        //            return objApiResponse;
        //        }

        //        SurvEntries SurvEntries = _context.SurvEntries.FirstOrDefault(x => x.PoolID == PoolId && x.we);
        //        objApiResponse.Message = "success";
        //        objApiResponse.Status = Messages.APIStatusSuccess;
        //      //  objApiResponse.SurvEntries = _context.SurvEntries.Find(PoolId);

        //    }
        //    catch (Exception ex)
        //    {
        //        objApiResponse.Message = "Exception: " + ex.Message;
        //        objApiResponse.Status = Messages.APIStatusError;
        //        // objResponse.Members = null;
        //    }
        //    return objApiResponse;
        //}
        #endregion

        [HttpPost("EntryWithoutPicks")]
        public ListStringAPIResponse EntryWithoutPicks(string LoginKey)
        {
            ListStringAPIResponse objApiResponse = new ListStringAPIResponse();
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
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "";
                    objApiResponse.ListString = _context.Query<EntryWithoutPicks>().FromSql("exec SP_EntryWithoutPicks @memberId=" + objToken.UserId).ToList();

                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
                GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("UpdateEntries")]
        public EntryAPIResponse UpdateEntries([FromBody]SurvEntries objEntry, string LoginKey)
        {
            EntryAPIResponse objApiResponse = new EntryAPIResponse();
            try
            {
                string ValidationMsg = ValidationHelper.GetErrorListFromModelState(ModelState);
                if (!string.IsNullOrEmpty(ValidationMsg))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = ValidationMsg;
                    return objApiResponse;
                }

                if (objEntry != null && objEntry.EntryID > 0)
                {
                    SurvEntries obj = _context.SurvEntries.Where(x => x.EntryID == objEntry.EntryID).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.Eliminated = objEntry.Eliminated;
                        obj.Defaults = objEntry.Defaults;
                        obj.EntryName = objEntry.EntryName;

                        _context.SurvEntries.Add(obj);
                        _context.Entry(obj).State = EntityState.Modified;
                    }
                }
                else
                {
                    _context.SurvEntries.Add(objEntry);
                    _context.Entry(objEntry).State = EntityState.Added;
                }
                objApiResponse.Message = Messages.SavedSuccess;
                objApiResponse.Status = Messages.APIStatusSuccess;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
                GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("JoinClubAddTicketsFromAdmin")]
        public TicketsAPIResponse JoinClubAddTicketsFromAdmin(string LoginKey, string MemberEmail, int PoolId, int NoOfTickets)
        {
            TicketsAPIResponse objApiResponse = new TicketsAPIResponse();
            try
            {
                Pool_Master objPoolMaster = _context.Pool_Master.FirstOrDefault(x => x.Pool_ID == PoolId);
                if (string.IsNullOrEmpty(MemberEmail) || string.IsNullOrWhiteSpace(MemberEmail))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Member email is empty or invalid.";
                    return objApiResponse;
                }

                if (NoOfTickets > 50)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.maxticketallowed;
                    return objApiResponse;
                }
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }


                Members objMembers = _context.Members.FirstOrDefault(x => x.Email_Address.Trim().ToLower() == MemberEmail.Trim().ToLower());
                if (objMembers != null)
                {
                    List<SurvEntries> survEntries = new List<SurvEntries>();
                    int MaxEntry = 0;
                    if (_context.SurvEntries.Count() > 0)
                    {
                        MaxEntry = _context.SurvEntries.Max(x => x.EntryID);
                    }

                    for (int i = 1; i <= NoOfTickets; i++)
                    {
                        MaxEntry++;
                        SurvEntries objEntry = new SurvEntries();
                        objEntry.PoolID = PoolId;
                        objEntry.MemberID = objMembers.Member_Id;
                        objEntry.EntryID = MaxEntry;
                        objEntry.EntryName = MaxEntry.ToString("00000");
                        survEntries.Add(objEntry);
                    }

                    foreach (SurvEntries entry in survEntries)
                    {
                        SurvEntries objTicket = new SurvEntries();
                        objTicket.EntryID = 0;
                        objTicket.EntryName = entry.EntryName;
                        objTicket.Active = true;
                        objTicket.CreatedDT = DateTime.Now;
                        objTicket.Defaults = entry.Defaults;
                        objTicket.Eliminated = entry.Eliminated;
                        objTicket.LastUpdated = DateTime.Now;
                        objTicket.MemberID = entry.MemberID;
                        objTicket.PoolID = entry.PoolID;

                        _context.SurvEntries.Add(objTicket);
                        _context.Entry(objTicket).State = EntityState.Added;

                    }
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.SurvEntries = _context.SurvEntries.Where(x => x.MemberID == objMembers.Member_Id && x.PoolID == PoolId).ToList();


                    NotificationHelper.SendNotificationClubJoinConfirmation(objMembers, objPoolMaster, survEntries, _context);
                }
                else
                {
                    throw new Exception("Member not found >> Email >> " + MemberEmail);
                }

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("GetTicketsByMemberIdPoolId")]
        public TicketsDetailAPIResponse GetTicketsByMemberIdPoolId(string LoginKey, int PoolId)
        {
            TicketsDetailAPIResponse objApiResponse = new TicketsDetailAPIResponse();
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


                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "";
                    objApiResponse.SurvEntries = _context.Query<SurvEntryPicksDetails>().FromSql("exec SP_TicketList_ByMemberIdPoolId @member_Id=" + objToken.UserId + ",  @PoolId=" + PoolId).ToList();

                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("DefaultedReport")]
        public Defaulted_ReportAPIResponse DefaultedReport(string LoginKey)
        {
            Defaulted_ReportAPIResponse objApiResponse = new Defaulted_ReportAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = "";
                objApiResponse.Defaulted_Report = _context.Query<Defaulted_Report>().FromSql("exec SP_Defaulted_Report").ToList();

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("DefaultedTicketByMemberId")]
        public Defaulted_ReportAPIResponse DefaultedTicketByMemberId(string LoginKey, int UserId)
        {
              Defaulted_ReportAPIResponse objApiResponse = new Defaulted_ReportAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = "";
                objApiResponse.Defaulted_Report = _context.Query<Defaulted_Report>().FromSql("exec SP_Defaulted_Report_ById @MemberID=" + UserId).ToList();
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("GetEntriesWeeksListByReferral")]
        public EntryReferralListAPIResponse GetEntriesWeeksListByReferral(string LoginKey, string Referral)
        {
            EntryReferralListAPIResponse objApiResponse = new EntryReferralListAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                List<qrySurvEntries> survEntryWeekLists = _context.Query<qrySurvEntries>().FromSql("exec SP_GetqrySurvEntriesByReferral @referral='" + Referral + "'").ToList();

                List<MemberReferralDetail> members = survEntryWeekLists.GroupBy(group => new { group.Login_ID, group.MemberID, group.FullName }).Select(y => new MemberReferralDetail
                {
                    Count = y.Count(),
                    FullName = y.Key.FullName,
                    Login_ID = y.Key.Login_ID,
                    MemberID = y.Key.MemberID,
                    EntryWeekLists = survEntryWeekLists.Where(x => x.MemberID == y.Key.MemberID).ToList()

                }).ToList();

                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.members = members;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }


        [HttpPost("TransferTicketsMultiple")]
        public APIResponses TransferTicketsMultiple([FromBody]List<int> entries, string LoginKey, int OldMemberID, int NewOwnerId)
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

                    List<string> strEntryIds = new List<string>();
                    foreach (int EntryId in entries)
                    {
                        var entry = _context.SurvEntries.FirstOrDefault(x => x.EntryID == EntryId);
                        if (entry != null)
                        {

                            strEntryIds.Add(entry.EntryName);

                            entry.MemberID = NewOwnerId;
                            _context.SurvEntries.Add(entry);
                            _context.Entry(entry).State = EntityState.Modified;



                        }
                    }
                    _context.SaveChanges();
                    try
                    {
                        string strEntries = string.Join(",", strEntryIds);
                        //Send Transfer email
                        var oldMember = _context.Members.FirstOrDefault(x => x.Member_Id == OldMemberID);
                        var newMember = _context.Members.FirstOrDefault(x => x.Member_Id == NewOwnerId);
                        NotificationHelper.sendNotoficationToNewOwnerMoveTicket(oldMember, newMember, strEntries, _context);
                        NotificationHelper.sendNotoficationToOldOwnerMoveTicket(oldMember, newMember, strEntries, _context);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Transfered Successfully";

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