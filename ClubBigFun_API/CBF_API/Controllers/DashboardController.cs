using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CBF_API.Helpers.enumeration;

namespace CBF_API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public DashboardController(CbfDbContext context)
        {
            _context = context;
        }

        //        [HttpGet("GetPoolCount")]
        //        public PoolCountResponses GetPoolCount(string LoginKey)
        //        {
        //            PoolCountResponses objResponse = new PoolCountResponses();
        //            TokenHelper tk = new TokenHelper(_context);
        //            if (!tk.ValidateToken(LoginKey))
        //            {

        //                objResponse.Status =  Messages.StatusError;
        //                objResponse.Message = "User session has expired.";
        //                return objResponse;
        //            }
        //            int poolCount = _context.Pool_Master.Count();
        //            PoolCount objCount = new PoolCount();
        //            objCount.Count = poolCount;
        //            objResponse.Message = "";
        //            objResponse.Status =  Messages.StatusError;
        //            objResponse.Maintaince = objCount;
        //            return objResponse;

        //}

        [HttpGet("GetDashboard")]
        public DashboardResponses GetDashboard(string LoginKey)
        {
            DashboardResponses objApiResponse = new DashboardResponses();
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
                Dashboard dashboard = new Dashboard();

                Token objToken = tk.GetTokenByKey(LoginKey);
                if (objToken != null)
                {
                    Administrators obj = _context.Administrators.Where(x => x.Member_Id == objToken.UserId).FirstOrDefault();
                    if (obj != null && obj.Admin_Type == "GroupAdmin")
                    {
                       


                    }
                    else
                    {
                        dashboard.PoolCount = _context.Pool_Master.Count();
                        dashboard.MemberCount = _context.Members.Count();
                        dashboard.AllAdminCount = _context.Administrators.Count();
                        dashboard.MailingCount = _context.MailingList.Count();
                        //  dashboard.UpcomingMatches = _context.NFLSchedule.Where(x => DateTime.ParseExact(x.StartTime, "MM/dd/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy").AddDays(+7), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToList();
                        //  dashboard.UpcomingMatches = _context.NFLSchedule.Where(x => x.GameDate == DateTime.Now.AddDays(7));
                        dashboard.SuperAdminCount = _context.Administrators.Where(x => x.Admin_Type == eRoleType.SuperAdmin).Count();
                        dashboard.GroupAdminCount = _context.Administrators.Where(x => x.Admin_Type == eRoleType.GroupAdmin).Count();
                        dashboard.AdminCount = _context.Administrators.Where(x => x.Admin_Type == eRoleType.Admin).Count();
                        dashboard.RececntMembers = _context.Members.Where(x => x.DTS >= DateTime.Now.AddMonths(-1)).Count();    //ToList();
                        dashboard.LatestLogins = _context.Administrators.OrderByDescending(x => x.Last_Login).Take(5).ToList();
                        try
                        {
                            //
                            dashboard.dashboard_Stats = _context.Query<Dashboard_Stats>().FromSql("SP_Dashboard_Stats").FirstOrDefault();
                            dashboard.poolEntryCounts = _context.Query<PoolEntryCount>().FromSql("Sp_Pool_Entry_Count").ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                objApiResponse.Message = "found";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Dashboard = dashboard;

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
    }
}