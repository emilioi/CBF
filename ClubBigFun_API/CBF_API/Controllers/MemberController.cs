using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public MemberController(CbfDbContext context)
        {
            _context = context;
        }

        #region Admin API

        [HttpGet("UserNameExist/{UserName}")]
        public AdminListAPIResponses AdminListAPIResponses(string UserName, String LoginKey)
        {
            AdminListAPIResponses objApiResponse = new AdminListAPIResponses();
            try
            {
                Members objUsersExits = new Members();
                if (objUsersExits == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "already exist";
                    return objApiResponse;
                }
                else
                {
                    var objusers = _context.Members.FirstOrDefault(x => x.Login_Id == UserName);
                    if (objusers != null)
                    {
                        objApiResponse.Status = Messages.APIStatusError;
                        objApiResponse.Message = "login id already exist";
                        //return true;
                    }
                    else
                    {
                        objApiResponse.Status = Messages.APIStatusSuccess;
                        objApiResponse.Message = "sucess";
                        return objApiResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }
        [HttpGet("EmailExist/{UserType}")]
        public UserListAPIResponses EmailExist(string email, string LoginKey)
        {
            UserListAPIResponses objApiResponse = new UserListAPIResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                var objusers = _context.Members.FirstOrDefault(x => x.Email_Address == email);
                if (objusers != null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "email already exist";
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

        // GET: api/Users
        [HttpGet("GetAll")]
        public UserListAPIResponses GetUsers(string LoginKey, int PageNo = 1, string reference = "")
        {
            UserListAPIResponses objApiResponse = new UserListAPIResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                List<SqlParameter> lstSql = new List<SqlParameter>();
                lstSql.Add(new SqlParameter("@PageSize", GNF.PageSize));
                lstSql.Add(new SqlParameter("@PageNo", PageNo));
                var users = new List<Members>();
                Token objToken = tk.GetTokenByKey(LoginKey);
                if (objToken != null)
                {
                    Administrators obj = _context.Administrators.Where(x => x.Member_Id == objToken.UserId).FirstOrDefault();
                    if (obj != null && obj.Admin_Type == "GroupAdmin")
                    {
                        List<int> lstMemberLink = _context.MemberAdminLink.Where(x => x.Admin_ID == objToken.UserId).Select(y => y.Member_ID).ToList();
                        if (!string.IsNullOrEmpty(reference) && reference != "All")
                        {

                            users = _context.Members.Where(x => x.Reference == reference && lstMemberLink.Contains(x.Member_Id)).ToList();
                        }
                        else
                        {
                            users = _context.Members.Where(x => lstMemberLink.Contains(x.Member_Id)).ToList();
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(reference) && reference != "All")
                        {
                            users = _context.Members.Where(x => x.Reference == reference).ToList();
                        }
                        else
                        {
                            users = _context.Members.ToList();
                        }
                    }
                }
                if (users == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Nothing found.";
                    return objApiResponse;
                }
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = "user found"; //users.Count + " users found.";
                objApiResponse.Users = users;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<APIResponses> GetUsers([FromRoute] int id, string LoginKey)
        {
            UserAPIResponses objApiResponse = new UserAPIResponses();
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

                var users = await _context.Members.FindAsync(id);

                if (users == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "User not found.";
                    return objApiResponse;
                }

                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = "User found.";
                objApiResponse.Users = users;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;

        }



        // POST: api/Users
        [HttpPost]
        public UserAPIResponses PostUsers([FromBody] Members objUser, string LoginKey)
        {
            UserAPIResponses objApiResponse = new UserAPIResponses();
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
                string EmailValidation = ValidationHelper.IsValidEmail(objUser.Email_Address);
                if (!string.IsNullOrEmpty(EmailValidation))
                {
                    objApiResponse.Message = EmailValidation;
                    objApiResponse.Status = Messages.APIStatusError;
                    return objApiResponse;
                }
                Members obj = new Members();
                if (objUser != null && objUser.Member_Id > 0)
                {
                    obj = _context.Members.Find(objUser.Member_Id);
                    obj.Login_Id = objUser.Login_Id;
                    obj.Phone_Number = objUser.Phone_Number;
                    obj.Address_Line1 = objUser.Address_Line1;
                    obj.Address_Line2 = objUser.Address_Line2;
                    obj.City = objUser.City;
                    obj.Country = objUser.Country;
                    obj.DTS = DateTime.Now;
                    obj.Email_Address = objUser.Email_Address;
                    obj.First_Name = objUser.First_Name;
                    // obj.Image_Name = objUser.Image_Name;
                    //obj.Image_Url = objUser.Image_Url;
                    // obj.Is_Active = false;
                    obj.Is_Deleted = false;
                    //refere
                    obj.Reference = objUser.Reference;
                    obj.Is_Temp_Password = false;
                    obj.Last_Name = objUser.Last_Name;
                    obj.Password = objUser.Password;
                    obj.Business_Phone = objUser.Business_Phone;
                    obj.Zip_Code = objUser.Zip_Code;
                    obj.State = objUser.State;
                    obj.Country = objUser.Country;
                    obj.Fax_Number = objUser.Fax_Number;
                    obj.Gender = objUser.Gender;
                    obj.User_Type = objUser.User_Type;
                    _context.Members.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.UpdatedSuccess;
                    objApiResponse.Users = obj;
                }
                else if (objUser != null && objUser.Member_Id == 0)
                {
                    //New changes as per requirement
                    Members objEMailCheck = _context.Members.FirstOrDefault(x => x.Email_Address == objUser.Email_Address);
                    if (objEMailCheck != null)
                    {
                        objApiResponse.Message = "This Email is already Registered with us";
                        objApiResponse.Status = Messages.APIStatusError;
                        return objApiResponse;
                    }
                    Members objUserIdCheck = _context.Members.FirstOrDefault(x => x.Login_Id == objUser.Login_Id);
                    if (objUserIdCheck != null)
                    {
                        objApiResponse.Message = "This Login ID is already Registered with us";
                        objApiResponse.Status = Messages.APIStatusError;
                        return objApiResponse;
                    }
                    obj.Login_Id = objUser.Login_Id;
                    obj.Phone_Number = objUser.Phone_Number;
                    obj.Address_Line1 = objUser.Address_Line1;
                    obj.Address_Line2 = objUser.Address_Line2;
                    obj.City = objUser.City;
                    obj.Country = objUser.Country;
                    obj.DTS = DateTime.Now;
                    obj.Email_Address = objUser.Email_Address;
                    obj.First_Name = objUser.First_Name;
                    // obj.Image_Name = objUser.Image_Name;
                    // obj.Image_Url = objUser.Image_Url;
                    // obj.Is_Active = false;
                    //refere
                    obj.Reference = objUser.Reference;
                    obj.Is_Deleted = false;
                    obj.Last_Name = objUser.Last_Name;
                    obj.Password = objUser.Password;
                    obj.Business_Phone = objUser.Business_Phone;
                    obj.Zip_Code = objUser.Zip_Code;
                    obj.State = objUser.State;
                    obj.Country = objUser.Country;
                    obj.Fax_Number = objUser.Fax_Number;
                    obj.Gender = objUser.Gender;
                    obj.User_Type = objUser.User_Type;

                    _context.Members.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.Users = obj;
                    try
                    {
                        NotificationHelper.SendNotificationOnRegistration(objUser, _context);
                        GNF.SaveMailingList(objUser, _context);
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

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public APIResponses DeleteUsers([FromRoute] int id, string LoginKey)
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
                if (!ModelState.IsValid)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.ModelStateInvalid;
                    return objApiResponse;
                }

                var users = _context.Members.Find(id);
                if (users == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "User not found.";
                    return objApiResponse;
                }
                //if (users.User_Type != eRoleType.Member)
                //{
                //    objApiResponse.Status = Messages.APIStatusError;
                //    objApiResponse.Message = "User of type " + users.User_Type + " can not be deleted.";
                //    return objApiResponse;
                //}
                _context.Members.Remove(users);
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

        private bool UsersExists(int id)
        {
            return _context.Members.Any(e => e.Member_Id == id);
        }
        [HttpPost("MemberFileUpload")]
        public MemberUploadResponses MemberFileUpload(IFormFile file, string LoginKey, int Member_Id, string UserType)
        {
            MemberUploadResponses objApiResponse = new MemberUploadResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {

                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.SessionExpired;
                    return objApiResponse;
                }

                Token objtoken = tk.GetTokenByKey(LoginKey);
                string folderPath = @"temp/";
                DirectoryInfo di = new DirectoryInfo(folderPath);
                if (!di.Exists)
                {
                    di.Create();
                }
                var stream = file.OpenReadStream();
                var fileName = file.FileName;
                var fullName = folderPath + fileName;
                using (FileStream fs = System.IO.File.Create(fullName, (int)stream.Length))
                {
                    byte[] bytesInStream = new byte[stream.Length];
                    stream.Read(bytesInStream, 0, bytesInStream.Length);
                    fs.Write(bytesInStream, 0, bytesInStream.Length);
                }
                // int User_Id = GNF.GetUserId();
                Members objProfile = _context.Members.Find(Member_Id);

                objProfile.Image_Url = fileName;
                objProfile.Image_Name = fullName;
                objProfile.User_Type = UserType;
                _context.Members.Attach(objProfile);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse; //null just to make error free
        }
        [HttpPost("MemberFileUploadBase64")]
        public TeamUploadResponses MemberFileUploadBase64([FromBody] ImageBase64 image, string LoginKey, int Member_Id)
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
                Members member = _context.Members.Find(Member_Id);
                if (member != null && image != null && !string.IsNullOrEmpty(image.fileExtention) && !string.IsNullOrEmpty(image.base64image))
                {
                    string fileName = Member_Id + "-" + ImageUniqueId + "." + image.fileExtention;

                    //Backend
                    string folderPath = GlobalConfig.MemberProfileUploadUrlBackend;// @"../cbfAdmin/assets/profile/members/";
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
                    string folderPathpub = GlobalConfig.MemberProfileUploadUrl;// @"../cbfpublic/assets/profile/members/";

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
                    Members objProfile = _context.Members.Find(Member_Id);

                    objProfile.Image_Url = fileName;
                    objProfile.Image_Name = image.fileName;
                    _context.Members.Attach(objProfile);
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

        [HttpGet("GetUsersbyAlterName")]
        public Member_AlertsResponses GetUsersbyAlertName(string Alter_Name, string LoginKey)
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

                var Member_Alerts = _context.Member_Alerts.FirstOrDefault(x => x.Alert_Name == Alter_Name);
                if (Member_Alerts == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Alter Name not Found.";
                    return objApiResponse;
                }
                Member_Alerts obj = new Member_Alerts();
                obj.Alert_Id = Member_Alerts.Alert_Id;
                obj.Alert_Name = Member_Alerts.Alert_Name;
                obj.Is_AfterLogin = Member_Alerts.Is_AfterLogin;
                obj.ReminderStart = Member_Alerts.ReminderStart;
                obj.ReminderEnd = Member_Alerts.ReminderEnd;
                obj.One_TimeReminder = Member_Alerts.One_TimeReminder;


                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = "Alter Name Found";
                objApiResponse.Member_Alerts = Member_Alerts;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;

        }
        [HttpPost("PostUsersbyAlterName")]
        public Member_AlertsResponses PostUsersbyAlertName([FromBody] Member_Alerts objUser, string Alert_Name, string LoginKey)
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
                Member_Alerts obj = _context.Member_Alerts.FirstOrDefault(x => x.Alert_Name == Alert_Name);
                if (objUser != null)
                {
                    // Member_Alerts obj = new Member_Alerts();

                    // obj.Alert_Id = objUser.Alert_Id;
                    obj.Alert_Name = objUser.Alert_Name;
                    obj.Is_AfterLogin = objUser.Is_AfterLogin;
                    obj.ReminderStart = objUser.ReminderStart;
                    obj.ReminderEnd = objUser.ReminderEnd;
                    obj.One_TimeReminder = objUser.One_TimeReminder;
                    _context.Member_Alerts.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.UpdatedSuccess;
                    objApiResponse.Member_Alerts = obj;
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
        [HttpGet("MemberSearchByReferal")]
        public UserListAPIResponses MemberSearchByReferal(string LoginKey, string referel)
        {
            UserListAPIResponses objApiResponse = new UserListAPIResponses();
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
                List<Members> objmember = (from m in _context.Members
                                           where m.Reference.Contains(referel)
                                           orderby m.Reference, m.First_Name, m.Last_Name ascending
                                           select m).ToList();

                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = Messages.SavedSuccess;
                objApiResponse.Users = objmember;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("MemberSearch")]
        public UserListAPIResponses MemberSearch(string LoginKey, string name = "", int? IsVerified = null)
        {
            UserListAPIResponses objApiResponse = new UserListAPIResponses();
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
                List<Members> objmember = new List<Members>();
                Token objToken = tk.GetTokenByKey(LoginKey);
                if (objToken != null)
                {
                    Administrators obj = _context.Administrators.Where(x => x.Member_Id == objToken.UserId).FirstOrDefault();
                    if (obj != null && obj.Admin_Type == "GroupAdmin")
                    {
                        List<int> lstMemberLink = _context.MemberAdminLink.Where(x => x.Admin_ID == objToken.UserId).Select(y => y.Member_ID).ToList();
                        objmember = (from m in _context.Members
                                     where (m.First_Name.Contains(name) || m.Last_Name.Contains(name)) && lstMemberLink.Contains(m.Member_Id)
                                     orderby m.First_Name, m.Last_Name ascending
                                     select m).ToList();

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(name) && IsVerified != null)
                        {
                            objmember = (from m in _context.Members
                                         where (m.First_Name.Contains(name) || m.Last_Name.Contains(name) || m.Email_Address.Contains(name))
                                         && (m.Is_Active == (IsVerified == 1 ? true : false))
                                         orderby m.First_Name, m.Last_Name ascending
                                         select m).ToList();
                        }
                        else if (string.IsNullOrEmpty(name) && IsVerified != null)
                        {
                            objmember = (from m in _context.Members
                                         where (m.Is_Active == (IsVerified == 1 ? true : false))
                                         orderby m.First_Name, m.Last_Name ascending
                                         select m).ToList();
                        }
                        else
                        {
                            objmember = (from m in _context.Members
                                         where (m.First_Name.Contains(name) || m.Last_Name.Contains(name) || m.Email_Address.Contains(name))
                                         orderby m.First_Name, m.Last_Name ascending
                                         select m).ToList();
                        }
                    }
                }
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = Messages.SavedSuccess;
                objApiResponse.Users = objmember;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("MemberFilter")]
        public UserListAPIResponses MemberFilter([FromBody]FilterSortingRequest filterObject, string LoginKey, string reference="All")
        {
            UserListAPIResponses objApiResponse = new UserListAPIResponses();
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
                List<Members> objmember = new List<Members>();
                Token objToken = tk.GetTokenByKey(LoginKey);
                if (objToken != null)
                {
                    Administrators obj = _context.Administrators.Where(x => x.Member_Id == objToken.UserId).FirstOrDefault();
                    if (obj != null && obj.Admin_Type == "GroupAdmin")
                    {

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(reference) && reference != "All")
                        {
                            objmember = (from m in _context.Members
                                         where m.Reference.Contains(reference)
                                         orderby m.Reference, m.First_Name, m.Last_Name ascending
                                         select m).ToList();
                        }
                        else
                        {
                            objmember = _context.Members.OrderByDescending(x => x.Member_Id).ToList();
                        }

                        if (filterObject != null && filterObject.IsSorting)
                        {
                            switch (filterObject.ShortByName)
                            {
                                case "Login_Id":
                                    
                                    if (filterObject.IsAscending)
                                    {
                                        objmember = objmember.OrderBy(x => x.Login_Id).ToList();
                                    }
                                    else
                                    {
                                        objmember = objmember.OrderByDescending(x => x.Login_Id).ToList();
                                    }
                                    break;
                                case "First_Name":
                                    if (filterObject.IsAscending)
                                    {
                                        objmember = objmember.OrderBy(x => x.First_Name).ToList();
                                    }
                                    else
                                    {
                                        objmember = objmember.OrderByDescending(x => x.First_Name).ToList();
                                    }
                                    break;
                                case "Last_Name":
                                    if (filterObject.IsAscending)
                                    {
                                        objmember = objmember.OrderBy(x => x.Last_Name).ToList();
                                    }
                                    else
                                    {
                                        objmember = objmember.OrderByDescending(x => x.Last_Name).ToList();
                                    }
                                    break;
                                case "Email_Address":
                                    if (filterObject.IsAscending)
                                    {
                                        objmember = objmember.OrderBy(x => x.Email_Address).ToList();
                                    }
                                    else
                                    {
                                        objmember = objmember.OrderByDescending(x => x.Email_Address).ToList();
                                    }
                                    break;
                                case "Agent":
                                    if (filterObject.IsAscending)
                                    {
                                        objmember = objmember.OrderBy(x => x.Reference).ToList();
                                    }
                                    else
                                    {
                                        objmember = objmember.OrderByDescending(x => x.Reference).ToList();
                                    }
                                    break;
                                case "Verified":
                                    if (filterObject.IsAscending)
                                    {
                                        objmember = objmember.OrderBy(x => x.Is_Active).ToList();
                                    }
                                    else
                                    {
                                        objmember = objmember.OrderByDescending(x => x.Is_Active).ToList();
                                    }
                                    break;
                                default:
                                    objmember = objmember.OrderByDescending(x => x.Member_Id).ToList();
                                    break;
                            }

                        }
                         
                        if (filterObject != null && filterObject.IsFilter && !string.IsNullOrEmpty(filterObject.FilterByValue))
                        {
                            switch (filterObject.FilterByName)
                            {
                                case "Login_Id":
                                    objmember = objmember.Where(x => x.Login_Id.Contains(filterObject.FilterByValue)).ToList();
                                    break;
                                case "First_Name":
                                    objmember = objmember.Where(x => x.First_Name.Contains(filterObject.FilterByValue)).ToList();
                                    break;
                                case "Last_Name":
                                    objmember = objmember.Where(x => x.Last_Name.Contains(filterObject.FilterByValue)).ToList();

                                    break;
                                case "Email_Address":
                                    objmember = objmember.Where(x => x.Email_Address.Contains(filterObject.FilterByValue)).ToList();

                                    break;
                                case "Agent":
                                    objmember = objmember.Where(x => x.Reference.Contains(filterObject.FilterByValue)).ToList();

                                    break;
                                case "Verified":
                                    objmember = objmember.Where(x => x.Is_Active == (filterObject.FilterByValue == "true"? true:false)).ToList();
                                    break;
                                default:
                                    objmember = objmember.ToList();
                                    break;
                            }
                        }



                    }
                }
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = Messages.SavedSuccess;
                objApiResponse.Users = objmember;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("GetReferenceList")]
        public ReferenceListAPIResponse GetReferenceList(string LoginKey)
        {
            ReferenceListAPIResponse objApiResponse = new ReferenceListAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                List<ReferenceList> ReferenceLists = new List<ReferenceList>();
                Token objToken = tk.GetTokenByKey(LoginKey);
                if (objToken != null)
                {
                    Administrators obj = _context.Administrators.Where(x => x.Member_Id == objToken.UserId).FirstOrDefault();
                    if (obj != null && obj.Admin_Type == "GroupAdmin")
                    {
                        ReferenceLists = _context.Query<ReferenceList>().FromSql("exec SP_getAllReferenceListByGroupAdmin @GroupAdminID=" + objToken.UserId).ToList();
                    }
                    else
                    {
                        ReferenceLists = _context.Query<ReferenceList>().FromSql("exec getAllReferenceList").ToList();
                    }
                }
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.ReferenceList = ReferenceLists;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);

            }
            return objApiResponse;
        }

        #endregion

        #region API Public Module
        // GET: api/Users
        [HttpPost("VerifyMember")]
        public UserAPIResponses VerifyMember(string LoginKey, int Member_Id, bool IsVerified)
        {
            UserAPIResponses objApiResponse = new UserAPIResponses();
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
                Members obj = _context.Members.Find(Member_Id);
                if (obj != null)
                {
                    if (IsVerified)
                    {
                        obj.Is_Active = true;
                        obj.Is_Temp_Password = true;
                        obj.Password = GNF.RandomPassword(6);
                    }




                    obj.Is_Active = IsVerified;
                    _context.Members.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.UpdatedSuccess;
                    objApiResponse.Users = obj;
                    if (IsVerified)
                    {
                        try
                        {
                            ////Please also send password in link
                            ////Send email
                            //string token = obj.Member_Id + "|" + obj.Password;
                            //string PublicUrl = "http://cbfpublic-nowgray-com.nt1-p2stl.ezhostingserver.com/reset-password?token=" + token;// Configuration["WebUrlPublic"];\
                            NotificationHelper.SendMemberVerificationEmail(obj, _context);

                        }
                        catch (Exception ex)
                        {

                        }
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

        // GET: api/Users
        [HttpPost("AcceptedRules")]
        public UserAPIResponses AcceptedRules(string LoginKey, int Member_Id, bool RulesAccepted)
        {
            UserAPIResponses objApiResponse = new UserAPIResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                //if (!ModelState.IsValid)
                //{
                //    objApiResponse.Status = Messages.APIStatusError;
                //    objApiResponse.Message = Messages.ModelStateInvalid;
                //    return objApiResponse;
                //}
                Members obj = _context.Members.Find(Member_Id);
                if (obj != null)
                {
                    if (RulesAccepted)
                    {
                        obj.Rules_Accepted = true;
                    }

                    obj.Rules_Accepted = RulesAccepted;
                    _context.Members.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.UpdatedSuccess;
                    objApiResponse.Users = obj;
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

        #endregion


        // GET: api/Users
        [HttpGet("GetAllByPoolID")]
        public UserListAPIResponses GetAllByPoolID(string LoginKey, int PoolID = 1)
        {
            UserListAPIResponses objApiResponse = new UserListAPIResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                //List<SqlParameter> lstSql = new List<SqlParameter>();
                //lstSql.Add(new SqlParameter("@PageSize", GNF.PageSize));
                //lstSql.Add(new SqlParameter("@PageNo", 1));
                var users = new List<Members>();
                //var users = _context.Members.FromSql("SP_GetMemberList @PageNo= @PageNo, @PageSize= @PageSize", lstSql.ToArray()).ToList();
                objApiResponse.Message = "success";
                List<int> membersIds = _context.SurvEntries.Where(m => m.PoolID == PoolID).Select(y => y.MemberID).ToList();
                if (membersIds != null)
                {
                    users = _context.Members.Where(x => membersIds.Contains(x.Member_Id)).ToList();
                }

                if (users == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Nothing found.";
                    return objApiResponse;
                }
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = "user found"; //users.Count + " users found.";
                objApiResponse.Users = users;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

    }
}