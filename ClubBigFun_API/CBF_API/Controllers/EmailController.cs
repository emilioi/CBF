using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CBF_API.Models;
using Microsoft.AspNetCore.Cors;
using CBF_API.Helpers;

namespace CBF_API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public EmailController(CbfDbContext context)
        {
            _context = context;
        }

        // GET: api/Email_Templates
        [HttpGet]
        public IEnumerable<Email_Templates> GetEmail_Templates()
        {

            return _context.Email_Templates;
        }

        // GET: api/Email_Templates/5
        [HttpGet("GetEmail_Templates")]
        public EmailAPIResponses GetEmail_Templates(string id, string LoginKey)
        {
            EmailAPIResponses objApiResponse = new EmailAPIResponses();
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

                var email_Templates = _context.Email_Templates.Find(id);

                if (email_Templates == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "email not found.";
                    return objApiResponse;
                }


                objApiResponse.Status = Messages.APIStatusError;
                objApiResponse.Message = "email found.";
                objApiResponse.Email_Templates = email_Templates;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }

            return objApiResponse;
        }

        [HttpPost("PostEmailTemplates")]
        public EmailAPIResponses PostEmailTemplates([FromBody] Email_Templates objEmail, string LoginKey)
        {
            EmailAPIResponses objApiResponse = new EmailAPIResponses();
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
                Email_Templates obj = _context.Email_Templates.FirstOrDefault(x => x.EmailID == objEmail.EmailID);
                if (obj != null)
                {
                    obj = _context.Email_Templates.Find(objEmail.EmailID);
                    obj.Subject = objEmail.Subject;
                    obj.Purpose = objEmail.Purpose;
                    obj.FromAddress = objEmail.FromAddress;
                    obj.Body = objEmail.Body;
                    obj.Cc = objEmail.Cc;
                    obj.Bcc = objEmail.Bcc;
                    obj.Importance = objEmail.Importance;
                    obj.BodyFormat = objEmail.BodyFormat;
                    obj.MailFormat = objEmail.MailFormat;

                    _context.Email_Templates.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.UpdatedSuccess;
                    objApiResponse.Email_Templates = obj;
                }
                else if (objEmail != null)
                {
                    obj = new Email_Templates();
                    obj.Subject = objEmail.Subject;
                    obj.Purpose = objEmail.Purpose;
                    obj.FromAddress = objEmail.FromAddress;
                    obj.Body = objEmail.Body;
                    obj.Cc = objEmail.Cc;
                    obj.Bcc = objEmail.Bcc;
                    obj.Importance = objEmail.Importance;
                    obj.BodyFormat = objEmail.BodyFormat;
                    obj.MailFormat = objEmail.MailFormat;
                    _context.Email_Templates.Add(objEmail);
                    _context.Entry(objEmail).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.Email_Templates = objEmail;
                }
                else
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.ModelStateInvalid;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        // DELETE: api/Email_Templates/5
        [HttpDelete("{id}")]
        public EmailAPIResponses DeleteEmail_Templates(string id, string LoginKey)
        {
            EmailAPIResponses objApiResponse = new EmailAPIResponses();
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

                var email_Templates = _context.Email_Templates.Find(id);
                if (email_Templates == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Not Found";
                    return objApiResponse;
                }

                _context.Email_Templates.Remove(email_Templates);
                _context.SaveChanges();
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = Messages.SavedSuccess;
                objApiResponse.Email_Templates = email_Templates;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        private bool Email_TemplatesExists(String Email)
        {
            return _context.Email_Templates.Any(e => e.EmailID == Email);
        }
        [HttpPost("EmailSendToSingleUser")]
        public EmailSendResponses EmailSendToSingleUser([FromBody] Email_Notification objEmail, string ToEmail, string LoginKey)
        {
            EmailSendResponses objApiResponse = new EmailSendResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);

                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                var SendEmail = _context.Email_Notification.Where(x => x.To_Email == ToEmail);
                if (SendEmail == null)
                {
                    objApiResponse.Status = "00";
                    objApiResponse.Message = "Nothing found.";
                    return objApiResponse;
                }
                EmailSend objSendEmail = new EmailSend();
                objSendEmail.To_Email = objEmail.To_Email;
                objSendEmail.From_Email = objEmail.From_Email;
                objSendEmail.Email_Content = objEmail.Email_Content;
                objSendEmail.Subject = objEmail.Subject;
                _context.Email_Notification.Attach(objEmail);
                _context.Entry(objEmail).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                _context.SaveChanges();

                objApiResponse.Status = "00";
                objApiResponse.Message = Messages.SavedSuccess;
                objApiResponse.Email = objSendEmail;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
        // GET: api/Email_Templates/5
        [HttpGet("MailingListById")]
        public MailingListResponses MailingListById(int id, string LoginKey)
        {
            MailingListResponses objApiResponse = new MailingListResponses();
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

                var email_Templates = _context.MailingList.Find(id);
                MailingList obj = new MailingList();
                obj.MailingList_ID = id;
                obj.Email = email_Templates.Email;
                obj.CreatedOn = email_Templates.CreatedOn;
                obj.Referer = email_Templates.Referer;
                if (email_Templates == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "email not found.";
                    return objApiResponse;
                }

                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = "email found.";
                objApiResponse.MailingLists = obj;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
        // POST: api/Pool_Master
        [HttpPost("EditMailingList")]
        public MailingListResponses EditMailingList([FromBody] MailingList objMailing, string LoginKey)
        {
            MailingListResponses objApiResponse = new MailingListResponses();
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


                MailingList objEmail = _context.MailingList.FirstOrDefault(x => x.Email == objMailing.Email);
                if (objEmail != null && objMailing.MailingList_ID == 0)
                {
                    objEmail.Email = objMailing.Email;
                    objEmail.MailingList_ID = objMailing.MailingList_ID;
                    objEmail.Referer = objMailing.Referer;
                    objEmail.Active = objMailing.Active;

                    _context.MailingList.Add(objEmail);
                    _context.Entry(objEmail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.MailingLists = objEmail;
                    return objApiResponse;
                }
                MailingList obj = _context.MailingList.FirstOrDefault(x => x.MailingList_ID == objMailing.MailingList_ID);
                if (obj != null)
                {

                    obj.Email = objMailing.Email;
                    obj.MailingList_ID = objMailing.MailingList_ID;
                    obj.Referer = objMailing.Referer;
                    obj.Active = objMailing.Active;

                    _context.MailingList.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.MailingLists = obj;
                }
                else if (objMailing != null && objMailing.MailingList_ID == 0)
                {
                    obj = new MailingList();
                    obj.Email = objMailing.Email;
                    obj.CreatedOn = DateTime.Now;
                    obj.Referer = objMailing.Referer;
                    obj.Active = objMailing.Active;

                    obj.MailingList_ID = objMailing.MailingList_ID;
                    _context.MailingList.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.MailingLists = obj;
                }
                else
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.ModelStateInvalid;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
        // DELETE: api/Email_Templates/5
        [HttpDelete("DeleteMailing")]
        public MailingListResponses DeleteMailing(int id, string LoginKey)
        {
            MailingListResponses objApiResponse = new MailingListResponses();
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

                var objmailing = _context.MailingList.Find(id);
                if (objmailing == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Not Found";
                    return objApiResponse;
                }

                _context.MailingList.Remove(objmailing);
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
        [HttpGet("GetMailingList")]
        public IEnumerable<MailingList> GetMailingList()
        {

            return _context.MailingList;
        }

        [HttpGet("DownloadEmailList")]
        public IEnumerable<string> DownloadEmailList(string LoginKey)
        {
            TokenHelper tk = new TokenHelper(_context);
            if (!tk.ValidateToken(LoginKey))
            {
                return null;
            }
            return _context.MailingList.Select(x => x.Email).ToList();
        }

        [HttpPost("UpdateEmailPreference")]
        public EmailPreferenceAPIResponse UpdateEmailPreference(string LoginKey, bool AddToMailingList)
        {
            EmailPreferenceAPIResponse objApiResponse = new EmailPreferenceAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                Token objToken = tk.GetTokenByKey(LoginKey);

                if (objToken == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                Members members = _context.Members.FirstOrDefault(x => x.Member_Id == objToken.UserId);
                if (members != null)
                {

                    MailingList objEmail = _context.MailingList.Where(x => x.Email == members.Email_Address).FirstOrDefault();
                    if(objEmail != null)
                    {
                        if(!AddToMailingList)
                        {
                            _context.MailingList.Remove(objEmail);
                            objApiResponse.Status = Messages.APIStatusSuccess;
                            objApiResponse.Message = "Your email has been removed from our mailing list.";
                            objApiResponse.EmailPreference = false;
                        }
                        else
                        {
                            objApiResponse.EmailPreference = true;
                            objApiResponse.Status = Messages.APIStatusSuccess;
                            objApiResponse.Message = "Great! Your email has been added to our mailing list.";
                        }

                    }
                    else
                    {
                        if (AddToMailingList)
                        {
                            objEmail = new MailingList();
                            objEmail.Email = members.Email_Address;
                            objEmail.CreatedOn = DateTime.Now;
                            objEmail.Referer = "clubbigfun.com";
                            objEmail.Active = true;


                            _context.MailingList.Add(objEmail);
                            _context.Entry(objEmail).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            _context.SaveChanges();

                            objApiResponse.Status = Messages.APIStatusSuccess;
                            objApiResponse.Message = "Great! Your email has been added to our mailing list.";
                            objApiResponse.EmailPreference = true;
                        }
                        
                    }
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