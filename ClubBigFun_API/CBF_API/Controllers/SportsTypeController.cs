using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class SportsTypeController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public SportsTypeController(CbfDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IEnumerable<Sports_Type> GetSportsType()
        {
            return _context.Sports_Type;
        }

        [HttpPost("PostSportType")]
        public SportsTypeResponses PostSportType([FromBody] Sports_Type objSportType, string LoginKey)
        {
            SportsTypeResponses objApiResponse = new SportsTypeResponses();
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


                var sportType = _context.Sports_Type.FirstOrDefault(x => x.SportType == objSportType.SportType);
                if (sportType != null)
                {
                    sportType.SportTypeName = objSportType.SportTypeName;
                    _context.Sports_Type.Add(sportType);
                    _context.Entry(sportType).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    objApiResponse.Status =  Messages.APIStatusSuccess;
                    objApiResponse.Message =  Messages.UpdatedSuccess;
                    objApiResponse.SportsType = objSportType;
                }

                else if (objSportType != null && objSportType.SportType == 0)
                {
                    
                    if (_context.Sports_Type.ToList() == null || _context.Sports_Type.Count() <= 0)
                    {
                        objSportType.SportType = 1;
                    }
                    else
                    {
                        objSportType.SportType = _context.Sports_Type.Max(x => x.SportType) + 1;
                    }
                    _context.Sports_Type.Add(objSportType);
                    _context.Entry(objSportType).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();

                    objApiResponse.Status =  Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.SportsType = objSportType;
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
        [HttpDelete("DeleteSportsType")]
        public SportsTypeResponses DeleteSportsType( int id, string LoginKey)
        {
            SportsTypeResponses objApiResponse = new SportsTypeResponses();
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
                var objSportsType = _context.Sports_Type.Find(id);
                if (objSportsType == null)
                {
                    objApiResponse.Status =  Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                _context.Sports_Type.Remove(objSportsType);
                _context.SaveChanges();
                objApiResponse.Status =  Messages.APIStatusSuccess;
                objApiResponse.Message =  Messages.DeleteSuccess;;
            }
            catch (Exception ex)
            {
               objApiResponse.Message = "Exception: "+ ex.Message; objApiResponse.Status = Messages.APIStatusError;  GNF.SaveException(ex, _context);
            }
            return objApiResponse;

        }
    }
}