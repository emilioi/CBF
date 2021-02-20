using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CBF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public AlertsController(CbfDbContext context)
        {
            _context = context;
        }

        // GET: api/Alerts
        [HttpGet]
        public Member_AlertsListResponses GetMember_Alerts(string LoginKey)
        {
            Member_AlertsListResponses objApiResponse = new Member_AlertsListResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                //List<Member_Alerts> obj = _context.Member_Alerts.Where(x => x.ReminderStart.Date >= DateTime.Now.Date && x.ReminderEnd.Date <= DateTime.Now.Date && !x.IsExpired).ToList();
                List<Member_Alerts> obj = _context.Member_Alerts.Where(x =>x.IsExpired).ToList();
                objApiResponse.Message = "found";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Member_Alerts = obj;

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        // GET: api/Alerts
        [HttpGet("GetAllAlerts")]
        public Member_AlertsListAllResponses GetMember_AlertsList(string LoginKey)
        {
            Member_AlertsListAllResponses objApiResponse = new Member_AlertsListAllResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                List<Member_Alerts> obj = _context.Member_Alerts.Where(x => x.Alert_Id > 0).ToList();
                objApiResponse.Message = "found";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Member_Alerts = obj;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("GetById")]
        public Member_AlertsByIdResponses GetById(string LoginKey, int Alert_Id)
        {
            Member_AlertsByIdResponses objApiResponse = new Member_AlertsByIdResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                Member_Alerts obj = _context.Member_Alerts.Where(x => x.Alert_Id == Alert_Id).FirstOrDefault();
                objApiResponse.Message = "found";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Member_Alerts = obj;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        // POST: api/Alerts
        [HttpPost]
        public Member_AlertsResponses PostMember_Alerts([FromBody] Member_Alerts member_Alerts, string LoginKey)
        {
            Member_AlertsResponses objApiResponse = new Member_AlertsResponses();
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
                Member_Alerts objAlerts = _context.Member_Alerts.FirstOrDefault(x => x.Alert_Id == member_Alerts.Alert_Id);
                if (objAlerts == null)
                {
                    objAlerts = new Member_Alerts();
                    //objAlerts.Alert_Id = member_Alerts.Alert_Id;
                    objAlerts.Alert_Name = member_Alerts.Alert_Name;
                    objAlerts.Alert_Description = member_Alerts.Alert_Description;
                    objAlerts.Is_AfterLogin = member_Alerts.Is_AfterLogin;
                    objAlerts.ReminderStart = DateTime.Now;
                    objAlerts.ReminderEnd = DateTime.Now;
                    objAlerts.One_TimeReminder = member_Alerts.One_TimeReminder;
                    objAlerts.IsExpired = member_Alerts.IsExpired;
                    objAlerts.AlertColor = member_Alerts.AlertColor;
                    _context.Member_Alerts.Add(objAlerts);
                    _context.Entry(objAlerts).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.Member_Alerts = objAlerts;
                }
                else
                {
                    objAlerts.Alert_Id = member_Alerts.Alert_Id;
                    objAlerts.Alert_Name = member_Alerts.Alert_Name;
                    objAlerts.Alert_Description = member_Alerts.Alert_Description;
                    objAlerts.Is_AfterLogin = member_Alerts.Is_AfterLogin;
                    objAlerts.ReminderStart = DateTime.Now;
                    objAlerts.ReminderEnd = DateTime.Now;
                    objAlerts.One_TimeReminder = member_Alerts.One_TimeReminder;
                    objAlerts.AlertColor = member_Alerts.AlertColor;
                    objAlerts.IsExpired = member_Alerts.IsExpired;
                    _context.Member_Alerts.Add(objAlerts);
                    _context.Entry(objAlerts).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.Member_Alerts = objAlerts;
                }

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpDelete("DeleteAlert")]
        public Member_AlertsByIdResponses DeleteAlert(int id, string LoginKey)
        {
            Member_AlertsByIdResponses objApiResponse = new Member_AlertsByIdResponses();
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

                var objAlert = _context.Member_Alerts.Find(id);
                if (objAlert == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Not Found";
                    return objApiResponse;
                }

                _context.Member_Alerts.Remove(objAlert);
                _context.SaveChanges();
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = Messages.DeleteSuccess; ;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }


        //private bool Member_AlertsExists(int id)
        //{
        //    return _context.Member_Alerts.Any(e => e.Alert_Id == id);
        //}
    }
}