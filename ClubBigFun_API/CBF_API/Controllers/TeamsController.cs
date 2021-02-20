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
using System.IO;

namespace CBF_API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public TeamsController(CbfDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public TeamListResponsesList GetNFLTeam(string LoginKey)
        {
            TeamListResponsesList objApiResponse = new TeamListResponsesList();
            string ValidationMsg = ValidationHelper.GetErrorListFromModelState(ModelState);
            if (!string.IsNullOrEmpty(ValidationMsg))
            {
                objApiResponse.Status = Messages.APIStatusError;
                objApiResponse.Message = ValidationMsg;
                return objApiResponse;
            }
            if (!ModelState.IsValid)
            {
                objApiResponse.Status =  Messages.APIStatusError;
                objApiResponse.Message = Messages.ModelStateInvalid;
                return objApiResponse;
            }
           
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {

                    objApiResponse.Status =  Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                var NFLTeam = _context.NFLTeam.OrderBy(x=> x.SportType).ToList();
                objApiResponse.Message = "found";
                objApiResponse.Status =  Messages.APIStatusSuccess;
                objApiResponse.TeamList = NFLTeam;
            }
            catch (Exception ex)
            {
               objApiResponse.Message = "Exception: "+ ex.Message; objApiResponse.Status = Messages.APIStatusError;  GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }


        [HttpPost("CreateUpdateTeams")]
        public TeamListResponses PostTeamList([FromBody] TeamListApi objTeamList, string LoginKey)
        {
            TeamListResponses objApiResponse = new TeamListResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {

                    objApiResponse.Status =  Messages.APIStatusError;
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
                    objApiResponse.Status =  Messages.APIStatusError;
                    objApiResponse.Message = Messages.ModelStateInvalid;
                    return objApiResponse;
                }

                //File upload

                //if (objTeamList.TeamLogo != null)
                //{
                //    string folderPath = @"temp/";
                //    DirectoryInfo di = new DirectoryInfo(folderPath);
                //    if (!di.Exists)
                //    {
                //        di.Create();
                //    }
                //    var stream = objTeamList.TeamLogo.OpenReadStream();
                //    var fileName = objTeamList.TeamLogo.FileName;
                //    var fullName = folderPath + fileName;
                //    using (FileStream fs = System.IO.File.Create(fullName, (int)stream.Length))
                //    {
                //        byte[] bytesInStream = new byte[stream.Length];
                //        stream.Read(bytesInStream, 0, (int)bytesInStream.Length);
                //        fs.Write(bytesInStream, 0, bytesInStream.Length);
                //    }

                //}
                NFLTeam obj = _context.NFLTeam.FirstOrDefault(x => x.Team_Id == objTeamList.TeamID);
                if (obj != null)
                {
                    obj = _context.NFLTeam.Find(objTeamList.TeamID);
                    obj.SportType = objTeamList.SportType;
                    obj.Team_Id = objTeamList.TeamID;
                    obj.Team_Name = objTeamList.TeamName;
                    obj.Abbreviation = objTeamList.TeamNameShort;
                    //if (objTeamList.TeamLogo != null)
                    //{
                    //    obj.LogoImageSrc = objTeamList.TeamLogo.FileName;
                    //}
                    _context.NFLTeam.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    objApiResponse.Status =  Messages.APIStatusSuccess;
                    objApiResponse.Message =  Messages.UpdatedSuccess;
                    objApiResponse.TeamList = obj;
                }
                else if (objTeamList != null && objTeamList.TeamID == 0)
                {
                    obj = new NFLTeam();
                    obj.SportType = objTeamList.SportType;
                    obj.Team_Id = _context.NFLTeam.Max(x => x.Team_Id) + 1;
                    obj.Team_Name = objTeamList.TeamName;

                    obj.Abbreviation = objTeamList.TeamNameShort;
                    //if (objTeamList.TeamLogo != null)
                    //{
                    //    obj.LogoImageSrc = objTeamList.TeamLogo.FileName;
                    //}
                    _context.NFLTeam.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();

                    objApiResponse.Status =  Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.TeamList = obj;
                }
                else
                {
                    objApiResponse.Status =  Messages.APIStatusError;
                    objApiResponse.Message = Messages.ModelStateInvalid;
                }
            }
            catch (Exception ex)
            {
               objApiResponse.Message = "Exception: "+ ex.Message; objApiResponse.Status = Messages.APIStatusError;  GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }


        [HttpDelete]
        public async Task<APIResponses> DeleteNFLTeam(int id, string LoginKey)
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
                    objApiResponse.Status =  Messages.APIStatusError;
                    objApiResponse.Message = "Invalid data supplied.";
                    return objApiResponse;
                }
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {

                    objApiResponse.Status =  Messages.APIStatusError;
                    objApiResponse.Message =Messages.InvalidToken;
                    return objApiResponse;
                }
                var NFLTeam = await _context.NFLTeam.FindAsync(id);
                if (NFLTeam == null)
                {
                    objApiResponse.Status =  Messages.APIStatusError;
                    objApiResponse.Message = "No team not found!";
                    return objApiResponse;
                }

                _context.NFLTeam.Remove(NFLTeam);
                await _context.SaveChangesAsync();
                objApiResponse.Status =  Messages.APIStatusSuccess;
                objApiResponse.Message = "Deleted successfully";
                return objApiResponse;
            }
            catch (Exception ex)
            {
               objApiResponse.Message = "Exception: "+ ex.Message; objApiResponse.Status = Messages.APIStatusError;  GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("LogoUpload")]
       // [Consumes("multipart/form-data")]
        public async Task<TeamUploadResponses> TeamFileUpload([FromForm]IFormFile file, string LoginKey, int TeamID)
        {
            TeamUploadResponses objApiResponse = new TeamUploadResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {

                    objApiResponse.Status =  Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                Token objtoken = tk.GetTokenByKey(LoginKey);
                string folderPath = @"../cbfAdmin-uat/assets/logos/";
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
                    stream.Read(bytesInStream, 0, (int)bytesInStream.Length);
                    fs.Write(bytesInStream, 0, bytesInStream.Length);
                }
                // int User_Id = GNF.GetUserId();
                NFLTeam objProfile = _context.NFLTeam.Find(TeamID);

                objProfile.LogoImageSrc = fileName;
                //objProfile.Logo = fullName;
                _context.NFLTeam.Attach(objProfile);
                await _context.SaveChangesAsync();

                objApiResponse.Status =  Messages.APIStatusSuccess;
                objApiResponse.Message = "Logo uploaded successfully.";
                objApiResponse.FileName = fileName;
            }
            catch (Exception ex)
            {
               objApiResponse.Message = "Exception: "+ ex.Message; objApiResponse.Status = Messages.APIStatusError;  GNF.SaveException(ex, _context);
            }
            return objApiResponse; //null just to make error free
        }

        [HttpPost("LogoUploadBase64")]
        public TeamUploadResponses LogoUploadBase64([FromBody] ImageBase64 image, string LoginKey, int TeamID)
        {
            TeamUploadResponses objApiResponse = new TeamUploadResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {

                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                NFLTeam team = _context.NFLTeam.Find(TeamID);
                if (team != null)
                {
                    string fileName = team.Team_Name + " " + team.City + "." + image.fileExtention;
                    try
                    {
                        string folderPath = GlobalConfig.TeamUploadUrlAdmin;// @"../cbfAdmin-uat/assets/logos/";
                        //string folderPath = @"../cbfAdmin/assets/logos/";
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
                    }
                    catch(Exception ex)
                    {

                    }
                    try
                    {
                        string folderPathpublic = GlobalConfig.TeamUploadUrlPublic;// @"../cbfAdmin-uat/assets/logos/";
                       
                        DirectoryInfo di = new DirectoryInfo(folderPathpublic);
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


                        string file = Path.Combine(folderPathpublic, fileName);

                        if (bytes.Length > 0)
                        {
                            using (var stream = new FileStream(file, FileMode.Create))
                            {
                                stream.Write(bytes, 0, bytes.Length);
                                stream.Flush();
                            }
                        }
                    }
                    catch(Exception ex)
                    { }
                    //var stream = file.OpenReadStream();
                    //var fileName = file.FileName;
                    //var fullName = folderPath + fileName;
                    //using (FileStream fs = System.IO.File.Create(fullName, (int)stream.Length))
                    //{
                    //    byte[] bytesInStream = new byte[stream.Length];
                    //    stream.Read(bytesInStream, 0, (int)bytesInStream.Length);
                    //    fs.Write(bytesInStream, 0, bytesInStream.Length);
                    //}
                    // int User_Id = GNF.GetUserId();
                    NFLTeam objProfile = _context.NFLTeam.Find(TeamID);

                    objProfile.LogoImageSrc = fileName;
                    //objProfile.Logo = fullName;
                    _context.NFLTeam.Attach(objProfile);
                    _context.SaveChanges();

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = "Logo uploaded successfully.";
                    objApiResponse.FileName = fileName;
                }
            }
            catch (Exception ex)
            {
               objApiResponse.Message = "Exception: "+ ex.Message; objApiResponse.Status = Messages.APIStatusError;  GNF.SaveException(ex, _context);
            }
            return objApiResponse; //null just to make error free
        }

    }

    
}