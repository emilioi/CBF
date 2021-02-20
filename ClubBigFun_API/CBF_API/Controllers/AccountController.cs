using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static CBF_API.Helpers.enumeration;

namespace CBF_API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : Controller
    {
        private readonly CbfDbContext _context;

        public AccountController(CbfDbContext context)
        {
            _context = context;
        }


        [HttpPost("Register")]
        public RegisterViewModelResponse Register([FromBody]RegisterViewModel objRequest)
        {
            RegisterViewModel objRegis = objRequest;
            RegisterViewModelResponse objApiResponse = new RegisterViewModelResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    string Email = objRequest.Email_Address;
                    if (objRequest != null)
                    {
                        string EmailValidation = ValidationHelper.IsValidEmail(objRequest.Email_Address);
                        if (!string.IsNullOrEmpty(EmailValidation))
                        {
                            objApiResponse.Message = EmailValidation;
                            objApiResponse.Status = Messages.APIStatusError;
                            return objApiResponse;
                        }
                        Members objUser = _context.Members.FirstOrDefault(x => x.Email_Address == objRegis.Email_Address);
                        if (objUser == null)
                        {
                            objUser = new Members();
                            objUser.First_Name = objRegis.First_Name;
                            objUser.Last_Name = objRegis.Last_Name;
                            objUser.Email_Address = objRegis.Email_Address;
                            objUser.Login_Id = objRegis.Login_Id;
                            objUser.Phone_Number = objRegis.Phone_Number;
                            objUser.Password = objRegis.Password;
                            objUser.User_Type = eRoleType.Member;
                            //Defaults
                            objUser.Password = GNF.RandomPassword(6);
                            objUser.Is_Temp_Password = true;
                            objUser.Is_Email_Verified = false;
                            objUser.Is_Active = false;
                            objUser.Is_Deleted = false;
                            objUser.Is_Locked = false;
                            objUser.Login_Id = Email;
                            objUser.Reference = objRegis.Reference;

                            _context.Members.Add(objUser);
                            _context.Entry(objUser).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            _context.SaveChanges();
                            objApiResponse.Message = "Registration Successfully!";
                            objApiResponse.Status = Messages.APIStatusSuccess;
                            objApiResponse.Register = objRegis;


                            try
                            {
                                NotificationHelper.SendNotificationOnRegistration(objUser, _context);

                                GNF.SaveMailingList(objUser, _context);


                            }
                            catch (Exception ex)
                            {

                            }
                            //  NotificationHelper.AdminRegistration(objUser.Email_Address, objUser, objTemp);

                        }
                        else
                        {
                            ModelState.AddModelError("Email", "This Email or Login Id is already Registered with us");
                            objApiResponse.Message = "This Email or Login Id is already Registered with us";
                            objApiResponse.Status = Messages.APIStatusError;
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

        [HttpPost("login")]
        [Produces("application/json")]
        public LoginInfoResponse Login([FromBody]LoginRequest objLoginRequest)
        {
            TokenHelper tk = new TokenHelper(_context);

            LoginInfoResponse objApiResponse = new LoginInfoResponse();
            try
            {
                if (objLoginRequest == null)
                {
                    objApiResponse.Message = "Invalid Object.";
                    objApiResponse.Status = Messages.APIStatusError;
                    return objApiResponse;
                }
                Members objUser = _context.Members.Where(x => (x.Email_Address == objLoginRequest.Email_Address || x.Login_Id == objLoginRequest.Email_Address) 
                && string.Compare( x.Password , objLoginRequest.Password, false) ==0).FirstOrDefault();
                if (objUser != null && objUser.Member_Id > 0)
                {
                    if (!objUser.Is_Active || objUser.Is_Locked || objUser.Is_Temp_Password)
                    {
                        objUser.Last_Failed_Login = DateTime.Now;

                        _context.Members.Add(objUser);
                        _context.Entry(objUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();

                        objApiResponse.Message = "User is not verified or is locked or password has been changed.";
                        objApiResponse.Status = Messages.APIStatusError;
                        return objApiResponse;
                    }

                    string Key = tk.GenrateToken(objUser.Member_Id, "");
                    var objUserResponse = new { Entity_Id = objUser.Member_Id, FullName = objUser.First_Name + " " + objUser.Last_Name, Email = objUser.Email_Address };
                    objApiResponse.Key = Key;
                    objApiResponse.Member_Id = objUser.Member_Id;
                    objApiResponse.UserInfo = objUser;

                    objUser.Last_Login = DateTime.Now;
                    try
                    {
                        MailingList objEmail = _context.MailingList.Where(x => x.Email == objUser.Email_Address).FirstOrDefault();
                        if (objEmail != null)
                        {
                            objApiResponse.EmailPreference = true;
                        }
                        else
                        {
                            objApiResponse.EmailPreference = false;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    _context.Members.Add(objUser);
                    _context.Entry(objUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Login Successfully!";
                }
                else
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Login Failed. Invalid Credential.";
                    string Key = "";// TokenHelper.GenrateToken(0, "");
                    objApiResponse.Key = Key;
                    objApiResponse.Member_Id = 0;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        /// <summary>
        /// Administrators table used.
        /// </summary>
        /// <param name="objLoginRequest"></param>
        /// <returns></returns>
        [HttpPost("AdminLogin")]
        [Produces("application/json")]
        public AdminLoginInfoResponse AdminLogin([FromBody]LoginRequest objLoginRequest)
        {
            TokenHelper tk = new TokenHelper(_context);

            AdminLoginInfoResponse objApiResponse = new AdminLoginInfoResponse();
            try
            {
                if (objLoginRequest == null)
                {
                    objApiResponse.Message = "Invalid Object.";
                    objApiResponse.Status = Messages.APIStatusError;
                    return objApiResponse;
                }
                Administrators objUser = _context.Administrators.FirstOrDefault(x => (x.Email_Address == objLoginRequest.Email_Address || x.Login_Id == objLoginRequest.Email_Address)  && string.Compare(x.Password, objLoginRequest.Password, false) == 0);
                if (objUser != null && objUser.Member_Id > 0)
                {
                    if (objUser.Is_Temp_Password)
                    {
                        objUser.Last_Failed_Login = DateTime.Now;

                        _context.Administrators.Add(objUser);
                        _context.Entry(objUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();

                        objApiResponse.Message = "Please reset your password by using the forgot password link.";
                        objApiResponse.Status = Messages.APIStatusError;
                        return objApiResponse;
                    }
                    Administrators objEntity = _context.Administrators.FirstOrDefault(x => x.Member_Id == objUser.Member_Id);
                    string Key = tk.GenrateToken(objUser.Member_Id, "");
                    var objUserResponse = new { Entity_Id = objUser.Member_Id, FullName = objEntity.First_Name + " " + objEntity.Last_Name, Email = objUser.Email_Address };
                    objApiResponse.Key = Key;
                    objApiResponse.Member_Id = objUser.Member_Id;
                    objApiResponse.UserInfo = objUser;
                    objUser.Is_Active = true;
                    objUser.Last_Login = DateTime.Now;
                    _context.Administrators.Add(objUser);
                    _context.Entry(objUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Login Successfully!";
                }
                else
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Login Failed. Invalid Credential.";
                    string Key = "";// TokenHelper.GenrateToken(0, "");
                    objApiResponse.Key = Key;
                    objApiResponse.Member_Id = 0;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }


        [HttpPost("ResetPassword")]
        public ResetPasswordViewModelResponse ResetPassword(ResetPasswordViewModelResponse model, string LoginKey, string Member_Id)
        {
            ResetPasswordViewModelResponse objApiResponse = new ResetPasswordViewModelResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {

                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                if (model != null)
                {
                    string Email = model.ResetPassword.Email_Address;
                    if (Email != GNF.GetcurrentUserEmail())
                    {
                        if (model != null)
                        {
                            var obj_user = _context.Members.FirstOrDefault(x => x.Member_Id == model.ResetPassword.Member_Id);
                            if (obj_user == null)
                            {
                                objApiResponse.Status = Messages.APIStatusError;
                                objApiResponse.Message = "Reset Link Expired";
                                // Don't reveal that the user does not exist
                                return objApiResponse;
                            }
                            if (obj_user.Password == model.ResetPassword.Old_Password)
                            {
                                obj_user.Password = model.ResetPassword.Password;
                                obj_user.Is_Temp_Password = false;

                                _context.Members.Attach(obj_user);
                                _context.Entry(obj_user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                _context.SaveChanges();
                                //var callbackUrl = new Uri(@"http://localhost:51957/Account/login?user_Id=" + obj_user.User_Id);
                                //EmailHelper.SendMail(Email, "Thanks", "Your Password was reset", true);
                            }
                            //try
                            //{
                            //    NotificationHelper.SendNotificationMemberReset(obj_user, _context);

                            //}
                            //catch (Exception ex)
                            //{
                            //    objResponse.Message = "Exception: " + ex.Message;
                            //    objResponse.Status = Messages.APIStatusError;
                            //}
                        }
                    }
                }
                if (model != null)
                {
                    objApiResponse.Message = "success";
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.ResetPassword = model.ResetPassword;
                }

                else
                {
                    objApiResponse.Message = "Modelstate has error";
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.ResetPassword = null;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("AdminResetPassword")]
        public ResetPasswordViewModelResponse AdminResetPassword(ResetPasswordViewModel model, string LoginKey, string Member_Id)
        {
            ResetPasswordViewModelResponse objApiResponse = new ResetPasswordViewModelResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                if (model != null)
                {
                    var obj_user = _context.Administrators.FirstOrDefault(x => x.Email_Address == model.Email_Address);
                    if (obj_user == null)
                    {
                        objApiResponse.Message = "Invalid email address.";
                        // Don't reveal that the user does not exist
                        return objApiResponse;
                    }
                    if (obj_user.Password == model.Old_Password)
                    {
                        obj_user.Password = model.Password;

                        _context.Administrators.Attach(obj_user);
                        _context.Entry(obj_user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();
                        //var callbackUrl = new Uri(@"http://localhost:51957/Account/login?user_Id=" + obj_user.User_Id);
                        //EmailHelper.SendMail(Email, "Thanks for using ClosetOuline", "Your Password was reset", true);
                    }
                    try
                    {
                        NotificationHelper.SendNotificationAdminReset(obj_user, _context);

                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (model != null)
                {
                    objApiResponse.Message = Messages.AdminPasswordResetSuccess;
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.ResetPassword = model;
                }
                else
                {
                    objApiResponse.Message = "Modelstate has error";
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.ResetPassword = null;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("validateToken")]
        public APIResponses ValidateToken(string LoginKey)
        {
            APIResponses objApiResponse = new APIResponses();
            TokenHelper tk = new TokenHelper(_context);
            try
            {
                if (tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "valid Token";

                }
                else
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.SessionExpired;
                    return objApiResponse;
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
        [HttpPost("ForgotPassword")]
        public APIResponses ForgotPassword(ForgotPasswordViewModel model)
        {
            ForgotPasswordViewModelResponse objApiResponse = new ForgotPasswordViewModelResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    string Email = model.Email_Address;
                    if (!string.IsNullOrEmpty(Email))
                    {
                        if (model != null)
                        {
                            Members objUsers = new Members();
                            objUsers = _context.Members.FirstOrDefault(x => x.Email_Address == Email);

                            if (objUsers != null)
                            {
                                string code = GNF.RandomPassword(6);
                                objUsers.Is_Temp_Password = true;
                                objUsers.Password = code;
                                _context.Members.Attach(objUsers);
                                _context.Entry(objUsers).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                _context.SaveChanges();

                                //var callbackUrl = new Uri(@"http://localhost:5000/Account/ResetPassword?user_Id=" + objUsers.Member_Id + "&code=" + code);
                                //NotificationHelper.SendNotificationOnMemberConfirmation(Email, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>", true);

                                try
                                {

                                    NotificationHelper.SendNotificationMemberForget(objUsers, _context);

                                }
                                catch (Exception ex)
                                {
                                    objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                                }

                                objApiResponse.Message = Messages.ForgotPasswordEmailSent;
                                objApiResponse.Status = Messages.APIStatusSuccess;
                                objApiResponse.ForgotPassword = model;
                            }
                            else
                            {
                                objApiResponse.Message = Messages.ForgotPasswordInvalidEmail;
                                objApiResponse.Status = Messages.APIStatusError;
                                objApiResponse.ForgotPassword = model;

                            }
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

        [HttpPost("AdminForgotPassword")]
        public APIResponses AdminForgotPassword(ForgotPasswordViewModel model)
        {
            ForgotPasswordViewModelResponse objApiResponse = new ForgotPasswordViewModelResponse();
            try
            {
                var objUsers = _context.Administrators.FirstOrDefault(x => x.Email_Address == model.Email_Address);

                if (objUsers == null)
                {
                    objApiResponse.Message = "Invalid email. User is not register";
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.ForgotPassword = null;
                    return objApiResponse;
                }
                else
                {
                    string code = GNF.RandomPassword(6);

                    objUsers.Password = code;
                    objUsers.Is_Temp_Password = true;
                    _context.Administrators.Attach(objUsers);
                    _context.Entry(objUsers).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    try
                    {

                        NotificationHelper.SendNotificationAdminForget(objUsers, _context);

                    }
                    catch (Exception ex)
                    {
                        objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
                    }
                }
                if (model != null)
                {
                    objApiResponse.Message = "success";
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.ForgotPassword = model;
                }
                else
                {
                    objApiResponse.Message = "Modelstate has error";
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.ForgotPassword = null;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("ResetPasswordMagicLink")]
        public ResetPasswordViewModelPublicResponse ResetPasswordMagicLink(ResetPasswordViewModelPublic model)
        {
            ResetPasswordViewModelPublicResponse objApiResponse = new ResetPasswordViewModelPublicResponse();
            try
            {
                if (model != null)
                {
                    var obj_user = _context.Members.FirstOrDefault(x => x.Member_Id == model.Member_Id);
                    if (obj_user == null)
                    {
                        objApiResponse.Status = Messages.APIStatusError;
                        objApiResponse.Message = "Reset Link Expired";
                        // Don't reveal that the user does not exist
                        return objApiResponse;
                    }
                    if (obj_user != null && !obj_user.Is_Temp_Password)
                    {
                        objApiResponse.Status = Messages.APIStatusError;
                        objApiResponse.Message = "Reset Link Expired";
                        // Don't reveal that the user does not exist
                        return objApiResponse;
                    }
                    if (obj_user.Password == model.Old_Password)
                    {
                        obj_user.Password = model.Password;
                        obj_user.Is_Temp_Password = false;
                        obj_user.Is_Email_Verified = true;

                        _context.Members.Attach(obj_user);
                        _context.Entry(obj_user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();

                        NotificationHelper.sendNotificationPasswordResetSuccessfully(obj_user, _context);
                    }
                    else
                    {
                        objApiResponse.Status = Messages.APIStatusError;
                        objApiResponse.Message = "Reset Link Expired";
                        // Don't reveal that the user does not exist
                        return objApiResponse;
                    }
                }
                if (model != null)
                {
                    objApiResponse.Message = Messages.ResetPasswordSuccess;
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.ResetPassword = model;
                }
                else
                {
                    objApiResponse.Message = "Modelstate has error";
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.ResetPassword = null;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("AdminResetPasswordMagicLink")]
        public ResetPasswordViewModelPublicResponse AdminResetPasswordMagicLink(ResetPasswordViewModelPublic model)
        {
            ResetPasswordViewModelPublicResponse objApiResponse = new ResetPasswordViewModelPublicResponse();
            try
            {

                if (model != null)
                {

                    var obj_user = _context.Administrators.FirstOrDefault(x => x.Member_Id == model.Member_Id);
                    if (obj_user == null)
                    {
                        objApiResponse.Status = Messages.APIStatusError;
                        objApiResponse.Message = "Reset Link Expired";
                        // Don't reveal that the user does not exist
                        return objApiResponse;
                    }
                    if (obj_user != null && !obj_user.Is_Temp_Password)
                    {
                        objApiResponse.Status = Messages.APIStatusError;
                        objApiResponse.Message = "Reset Link Expired";
                        // Don't reveal that the user does not exist
                        return objApiResponse;
                    }
                    if (obj_user.Password == model.Old_Password)
                    {
                        obj_user.Password = model.Password;
                        obj_user.Is_Temp_Password = false;
                        obj_user.Is_Email_Verified = true;

                        _context.Administrators.Attach(obj_user);
                        _context.Entry(obj_user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();


                        NotificationHelper.sendNotificationPasswordResetSuccessfullyAdmin(obj_user, _context);
                    }
                    else
                    {
                        objApiResponse.Status = Messages.APIStatusError;
                        objApiResponse.Message = "Reset Link Expired";
                        // Don't reveal that the user does not exist
                        return objApiResponse;
                    }
                }
                if (model != null)
                {
                    objApiResponse.Message = Messages.ResetPasswordSuccess;
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.ResetPassword = model;
                }
                else
                {
                    objApiResponse.Message = "Modelstate has error";
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.ResetPassword = null;
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