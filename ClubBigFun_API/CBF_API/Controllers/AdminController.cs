using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CBF_API.Helpers.enumeration;

namespace CBF_API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly CbfDbContext _context;

        public AdminController(CbfDbContext context)
        {
            _context = context;

        }
        [HttpGet("LoginIDExist")] //Minor Change in spelling 
        public APIResponses AdminNameExist(string AdminName, string LoginKey)
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


                var objAdmins = _context.Administrators.FirstOrDefault(x => x.Login_Id == AdminName);
                if (objAdmins != null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "login id already exist";
                    //return true;
                }
                else
                {
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "sucess";

                }
                //  objApiResponse.Status = "00";
                // objApiResponse.Administrators = objAdminsExits;

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }


            return objApiResponse;

        }
        [HttpGet("EmailExist/{Email}")]
        public AdminListAPIResponses EmailExist(string email, string LoginKey)
        {
            AdminListAPIResponses objApiResponse = new AdminListAPIResponses();
            TokenHelper tk = new TokenHelper(_context);
            try
            {
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                var objAdmins = _context.Administrators.FirstOrDefault(x => x.Email_Address == email);
                if (objAdmins != null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "email already exist";
                    return objApiResponse;
                }
                else
                {
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "sucess";
                    return objApiResponse;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        /// <summary>
        /// Filter data based on Admin Type (SuperAdmin/GroupAdmin/Admin)
        /// </summary>
        /// <param name="AdminType"></param>
        /// <returns></returns>
        [HttpGet("GetAll/{AdminType}")]
        public AdminListAPIResponses GetAdmins(string AdminType, string LoginKey)
        {
            AdminListAPIResponses objApiResponse = new AdminListAPIResponses();
            try
            {

                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {

                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                List<Administrators> administrators = new List<Administrators>();
                if (AdminType != "all")
                {
                    administrators = _context.Administrators.Where(x => x.Admin_Type.ToLower() == AdminType.ToLower()).ToList();

                }
                else
                {
                    administrators = _context.Administrators.ToList();

                }
                if (administrators == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Nothing found.";
                    return objApiResponse;
                }
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = administrators.Count + " Administrators found.";
                objApiResponse.Administrators = administrators;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        // GET: api/Administrators/5
        [HttpGet("{id}")]
        public async Task<APIResponses> GetAdmins([FromRoute] int id, string LoginKey)
        {
            AdminAPIResponses objApiResponse = new AdminAPIResponses();
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

                var Administrators = await _context.Administrators.FindAsync(id);

                if (Administrators == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Admin not found.";
                    return objApiResponse;
                }

                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = "Admin found.";
                objApiResponse.Administrators = Administrators;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }



        // POST: api/Administrators
        [HttpPost]
        public AdminAPIResponses PostAdmins([FromBody] Administrators objAdmin, string LoginKey)
        {
            AdminAPIResponses objApiResponse = new AdminAPIResponses();
            try
            {
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
                TokenHelper tk = new TokenHelper(_context);

                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                string EmailValidation = ValidationHelper.IsValidEmail(objAdmin.Email_Address);
                if (!string.IsNullOrEmpty(EmailValidation))
                {
                    objApiResponse.Message = EmailValidation;
                    objApiResponse.Status = Messages.APIStatusError;
                    return objApiResponse;
                }

                Administrators obj = new Administrators();
                if (objAdmin != null && objAdmin.Member_Id > 0)
                {
                    obj = _context.Administrators.Find(objAdmin.Member_Id);
                    obj.Login_Id = objAdmin.Login_Id;
                    //obj.Address_Line2 = objAdmin.Address_Line2;
                    //obj.City = objAdmin.City;
                    //obj.Country = objAdmin.Country;
                    obj.DTS = DateTime.Now;
                    obj.Email_Address = objAdmin.Email_Address;
                    obj.First_Name = objAdmin.First_Name;
                    // obj.Image_Name = objAdmin.Image_Name;
                    obj.Image_Url = objAdmin.Image_Url;
                    obj.Is_Active = true;
                    obj.Is_Deleted = false;
                    obj.Is_Temp_Password = false;
                    obj.Last_Name = objAdmin.Last_Name;
                    obj.Password = objAdmin.Password;
                    obj.Phone_Number = objAdmin.Phone_Number;
                    //Defaults


                    // obj.State = objAdmin.State;
                    //obj.User_Type = objAdmin.User_Type;
                    _context.Administrators.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.UpdatedSuccess;
                    objApiResponse.Administrators = obj;
                }
                else if (objAdmin != null && objAdmin.Member_Id == 0)
                {
                    //New changes as per requirement
                    Administrators objEMailCheck = _context.Administrators.FirstOrDefault(x => x.Email_Address == objAdmin.Email_Address);
                    if (objEMailCheck != null)
                    {
                        objApiResponse.Message = "This Email is already Registered with us";
                        objApiResponse.Status = Messages.APIStatusError;
                        return objApiResponse;
                    }
                    Administrators objUserIdCheck = _context.Administrators.FirstOrDefault(x => x.Login_Id == objAdmin.Login_Id);
                    if (objUserIdCheck != null)
                    {
                        objApiResponse.Message = "This Login ID is already Registered with us";
                        objApiResponse.Status = Messages.APIStatusError;
                        return objApiResponse;
                    }
                    obj = new Administrators();
                    obj.DTS = DateTime.Now;
                    obj.Email_Address = objAdmin.Email_Address;
                    obj.First_Name = objAdmin.First_Name;
                    obj.Image_Name = objAdmin.Image_Name;
                    obj.Image_Url = objAdmin.Image_Url;
                    obj.Is_Active = true;
                    obj.Is_Deleted = false;
                    obj.Last_Name = objAdmin.Last_Name;
                    obj.Password = objAdmin.Password;
                    obj.Phone_Number = objAdmin.Phone_Number;
                    obj.Login_Id = objAdmin.Login_Id;
                    obj.Admin_Type = objAdmin.Admin_Type;
                    obj.Is_Temp_Password = false;
                    obj.Is_Email_Verified = true;

                    obj.Is_Locked = false;


                    _context.Administrators.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.Administrators = obj;
                    try
                    {
                        NotificationHelper.SendNotificationAdminRegistration(obj, _context);

                    }
                    catch (Exception ex)
                    {

                    }
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

        // DELETE: api/Administrators/5
        [HttpDelete("{id}")]
        public async Task<APIResponses> DeleteAdmins([FromRoute] int id, string LoginKey)
        {
            APIResponses objApiResponse = new APIResponses();
            TokenHelper tk = new TokenHelper(_context);
            try
            {
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
                Token objToken = tk.GetTokenByKey(LoginKey);
                if (objToken != null)
                {
                    var Administrators = await _context.Administrators.FindAsync(id);
                    if (Administrators == null)
                    {
                        objApiResponse.Status = Messages.APIStatusError;
                        objApiResponse.Message = "Admin not found.";
                        return objApiResponse;
                    }
                    if (Administrators.Member_Id == objToken.UserId)
                    {
                        objApiResponse.Status = Messages.APIStatusError;
                        objApiResponse.Message = "You can not delete yourself.";
                        return objApiResponse;
                    }
                    _context.Administrators.Remove(Administrators);
                    await _context.SaveChangesAsync();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.DeleteSuccess;
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

        private bool AdminsExists(int id)
        {
            return _context.Administrators.Any(e => e.Member_Id == id);
        }
        [HttpPost("AdministratorsFileUploadBase64")]
        public TeamUploadResponses AdministratorsFileUploadBase64([FromBody] ImageBase64 image, string LoginKey, int Administrators_Id)
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
                Administrators Administrators = _context.Administrators.Find(Administrators_Id);
                if (Administrators != null && image != null && !string.IsNullOrEmpty(image.fileExtention) && !string.IsNullOrEmpty(image.base64image))
                {
                    string fileName = Administrators_Id + "-" + ImageUniqueId + "." + image.fileExtention;

                    string folderPath = GlobalConfig.AdminProfileUploadUrl;// @"../cbfAdmin/assets/profile/Administrators/";
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

                    Administrators objProfile = _context.Administrators.Find(Administrators_Id);

                    objProfile.Image_Url = fileName;
                    objProfile.Image_Name = image.fileName;
                    _context.Administrators.Attach(objProfile);
                    _context.SaveChanges();

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Image uploaded successfully.";
                    objApiResponse.FileName = fileName;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse; //null just to make error free
        }

        [HttpPost("AssignGroupAdmin")]
        public APIResponses AssignGroupAdmin([FromBody]List<int> members, string LoginKey, int GroupAdminId, bool remove = false)
        {
            APIResponses objApiResponse = new APIResponses();
            try
            {
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
                TokenHelper tk = new TokenHelper(_context);

                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }


                Administrators obj = _context.Administrators.Where(x => x.Member_Id == GroupAdminId).FirstOrDefault();
                if (obj != null && obj.Admin_Type == "GroupAdmin")
                {
                    if (remove)
                    {
                        foreach (int i in members)
                        {
                            MemberAdminLink objLink = _context.MemberAdminLink.FirstOrDefault(x => x.Admin_ID == GroupAdminId && x.Member_ID == i);
                            if (objLink != null)
                            {
                                _context.MemberAdminLink.Remove(objLink);
                            }
                        }
                    }
                    else
                    {
                        // _context.MemberAdminLink.RemoveRange(_context.MemberAdminLink.Where(x => x.Admin_ID == GroupAdminId).ToList());
                        // _context.SaveChanges();
                        foreach (int i in members)
                        {
                            MemberAdminLink objLink = _context.MemberAdminLink.FirstOrDefault(x => x.Admin_ID == GroupAdminId && x.Member_ID == i);
                            if (objLink != null)
                            {
                                
                                //objLink.Admin_ID = GroupAdminId;
                                //objLink.Member_ID = i;
                                //_context.MemberAdminLink.Add(objLink);
                                //_context.Entry(objLink)
                                //    .State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            }
                            else
                            {
                                objLink = new MemberAdminLink();
                                objLink.Admin_ID = GroupAdminId;
                                objLink.Member_ID = i;
                                _context.MemberAdminLink.Add(objLink);
                                _context.Entry(objLink)
                                    .State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            }
                        }
                    }
                    _context.SaveChanges();
                }
                else
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Not a valid group admin.";
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("GetAllMemberByGroupAdminID")]
        public UserListAPIResponses GetAllMemberByGroupAdminID(string LoginKey, int GroupAdminId)
        {
            UserListAPIResponses objApiResponse = new UserListAPIResponses();
            try
            {
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
                TokenHelper tk = new TokenHelper(_context);

                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }


                Administrators obj = _context.Administrators.Where(x => x.Member_Id == GroupAdminId).FirstOrDefault();
                if (obj != null && obj.Admin_Type == "GroupAdmin")
                {
                    List<int> lstMemberLink = _context.MemberAdminLink.Where(x => x.Admin_ID == GroupAdminId).Select(y => y.Member_ID).ToList();
                    List<Members> lstMembers = _context.Members.Where(x => lstMemberLink.Contains(x.Member_Id)).ToList();
                    objApiResponse.Users = lstMembers;
                }
                else
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Not a valid group admin.";
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