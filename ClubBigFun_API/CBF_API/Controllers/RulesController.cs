using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CBF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public RulesController(CbfDbContext context)
        {
            _context = context;
        }

        // GET: api/Email_Templates
        [HttpGet]
        public IEnumerable<Rules> GetRules()
        {

            return _context.Rules;
        }

        // GET: api/Email_Templates/5
        [HttpGet("GetRulesById")]
        public RulesAPIResponses GetRulesById(int id, string LoginKey)
        {
            RulesAPIResponses objApiResponse = new RulesAPIResponses();
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

                var rules = _context.Rules.Find(id);

                if (rules == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Not Found";
                    return objApiResponse;
                }

                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = "Success";
                objApiResponse.Rule = rules;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("PostRules")]
        public RulesAPIResponses PostRules([FromBody] Rules objRules, string LoginKey)
        {
            RulesAPIResponses objApiResponse = new RulesAPIResponses();
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
                Rules obj = _context.Rules.FirstOrDefault(x => x.Rule_Id == objRules.Rule_Id);
                if (obj != null)
                {
                    obj = _context.Rules.Find(objRules.Rule_Id);
                    obj.Game_Type = objRules.Game_Type;
                    obj.Rule_Title = objRules.Rule_Title;
                    obj.Rule_Content = objRules.Rule_Content;

                    _context.Rules.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.UpdatedSuccess;
                    objApiResponse.Rule = obj;
                }
                else if (objRules != null)
                {
                    obj = new Rules();
                    obj.Game_Type = objRules.Game_Type;
                    obj.Rule_Title = objRules.Rule_Title;
                    obj.Rule_Content = objRules.Rule_Content;

                    _context.Rules.Add(objRules);
                    _context.Entry(objRules).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                    objApiResponse.Rule = objRules;
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

        // GET: api/Email_Templates/5
        [HttpGet("GetRulesByIdPublic")]
        public RulesAPIResponses GetRulesByIdPublic(int id)
        {
            RulesAPIResponses objApiResponse = new RulesAPIResponses();
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

                var rules = _context.Rules.Find(id);

                if (rules == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Not Found";
                    return objApiResponse;
                }

                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.Message = "Success";
                objApiResponse.Rule = rules;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }

            return objApiResponse;
        }


        // DELETE: api/Email_Templates/5
        [HttpDelete("DeleteRule")]
        public RulesAPIResponses DeleteRule(int id, string LoginKey)
        {
            RulesAPIResponses objApiResponse = new RulesAPIResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                var objRule = _context.Rules.Find(id);
                if (objRule == null)
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = "Not Found";
                    return objApiResponse;
                }

                _context.Rules.Remove(objRule);
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
    }
}