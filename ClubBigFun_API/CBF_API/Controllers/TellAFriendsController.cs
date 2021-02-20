using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CBF_API.Models;
using CBF_API.Helpers;

namespace CBF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TellAFriendsController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public TellAFriendsController(CbfDbContext context)
        {
            _context = context;
        }

        // GET: api/TellAFriends
        [HttpGet]
        public TellAFriendListResponses GetTellAFriend(string LoginKey)
        {
            TellAFriendListResponses objApiResponse = new TellAFriendListResponses();
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
                var TellAFriendList = _context.TellAFriend.ToList();
                objApiResponse.Message = "found";
                objApiResponse.Status =  Messages.APIStatusSuccess;
                objApiResponse.TellFriend = TellAFriendList;
            }
            catch (Exception ex)
            {
               objApiResponse.Message = "Exception: "+ ex.Message; objApiResponse.Status = Messages.APIStatusError;  GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        // GET: api/TellAFriends/5
        [HttpGet("{id}")]
        public TellAFriendResponses GetTellAFriend([FromRoute] int id, string LoginKey)
        {
            TellAFriendResponses objApiResponse = new TellAFriendResponses();
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

                var tellAFriend = _context.TellAFriend.Find(id);

                if (tellAFriend == null)
                {
                    objApiResponse.Status =  Messages.APIStatusError;
                    objApiResponse.Message = "Nothing found.";
                    return objApiResponse;
                }

                objApiResponse.Status =  Messages.APIStatusSuccess;
                objApiResponse.Message = " found.";
                objApiResponse.TellFriend = tellAFriend;
            }
            catch (Exception ex)
            {
               objApiResponse.Message = "Exception: "+ ex.Message; objApiResponse.Status = Messages.APIStatusError;  GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        // PUT: api/TellAFriends/5
        //[HttpPut("EditTellAFriend{id}")]
        //public TellAFriendResponses EditTellAFriend([FromRoute] int id, [FromBody] TellAFriend tellAFriend, string LoginKey)
        //{
        //    TellAFriendResponses objApiResponse = new TellAFriendResponses();
        //    TokenHelper tk = new TokenHelper(_context);
        //    if (!tk.ValidateToken(LoginKey))
        //    {
        //        objApiResponse.Status =  Messages.StatusSuccess;
        //        objApiResponse.Message = "valid Token";
        //    }
        //    else
        //    {
        //        objApiResponse.Status =  Messages.StatusError;
        //        objApiResponse.Message = Messages.InvalidToken;
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        objApiResponse.Status = "11";
        //        objApiResponse.Message = Messages.ModelStateInvalid;
        //        return objApiResponse;
        //    }

        //    if (id != tellAFriend.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(tellAFriend).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TellAFriendExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/TellAFriends
        [HttpPost]
        public TellAFriendResponses PostTellAFriend([FromBody] TellAFriend objTellAFriend, string LoginKey)
        {
            TellAFriendResponses objApiResponse = new TellAFriendResponses();
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

                TellAFriend obj = _context.TellAFriend.FirstOrDefault(x => x.ID == objTellAFriend.ID);
                if (obj != null)
                {
                    obj = _context.TellAFriend.Find(objTellAFriend.ID);
                    //TellAFriend obj = new TellAFriend();
                    obj.Referer = objTellAFriend.Referer;
                    obj.YourEmail = objTellAFriend.YourEmail;
                    obj.YourName = objTellAFriend.YourName;
                    obj.FriendName = objTellAFriend.FriendName;
                    obj.FriendEmail = objTellAFriend.FriendEmail;

                    _context.TellAFriend.Attach(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    objApiResponse.Status =  Messages.APIStatusSuccess;
                    objApiResponse.Message =  Messages.UpdatedSuccess;
                    objApiResponse.TellFriend = objTellAFriend;
                }
                else if (objTellAFriend != null && objTellAFriend.ID == 0)
                {
                    obj = new TellAFriend();
                    obj.Referer = objTellAFriend.Referer;
                    obj.YourEmail = objTellAFriend.YourEmail;
                    obj.YourName = objTellAFriend.YourName;
                    obj.FriendName = objTellAFriend.FriendName;
                    obj.FriendEmail = objTellAFriend.FriendEmail;

                    _context.TellAFriend.Attach(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();

                    objApiResponse.Status =  Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.TellFriend = objTellAFriend;
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

        // DELETE: api/TellAFriends/5
        [HttpDelete("{id}")]
        public TellAFriendResponses DeleteTellAFriend([FromRoute] int id, string LoginKey)
        {
            TellAFriendResponses objApiResponse = new TellAFriendResponses();
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

                var tellAFriend = _context.TellAFriend.Find(id);
                if (tellAFriend == null)
                {
                    objApiResponse.Status =  Messages.APIStatusError;
                    objApiResponse.Message = "Not Found";
                    return objApiResponse;
                }

                _context.TellAFriend.Remove(tellAFriend);
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

        private bool TellAFriendExists(int id)
        {
            return _context.TellAFriend.Any(e => e.ID == id);
        }
    }
}