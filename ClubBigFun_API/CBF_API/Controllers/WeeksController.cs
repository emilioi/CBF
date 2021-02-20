using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CBF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeksController : ControllerBase
    {
        private readonly CbfDbContext _context;

        public WeeksController(CbfDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetWeeksMenu")]
        public WeeksMenuAPIResponse GetWeeksMenu(string LoginKey)
        {
            WeeksMenuAPIResponse objApiResponse = new WeeksMenuAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                string Sql = "SELECT * from qrySurvPoolWeekCount";
                var WeekMenu = _context.Query<WeekMenu>().FromSql(Sql).ToList();
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.weekMenus = WeekMenu;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("GetPoolWeeksList")]
        public PoolMaintainceMappedResponses GetPoolWeeksList(string LoginKey, int Pool_ID)
        {
            PoolMaintainceMappedResponses objApiResponse = new PoolMaintainceMappedResponses();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                string Sql = "select a.*,b.Pool_Name from SurvPoolWeekList as a left join Pool_Master as b on a.PoolID = b.Pool_ID where a.PoolID = " + Pool_ID + " order by b.Pool_Name,a.WeekNumber";
                var survPoolWeekLists = _context.Query<SurvPoolWeekListMapped>().FromSql(Sql).ToList();
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.PoolMapped = survPoolWeekLists;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpGet("GetWeekNumbers")]
        public WeeksNumberAPIResponse GetWeekNumbers(string LoginKey, int Pool_ID)
        {
            WeeksNumberAPIResponse objApiResponse = new WeeksNumberAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }

                //string Sql = "select WeekNumber from survPoolWeekList where PoolID="+ Pool_ID + " order by WeekNumber";
                var weekNumbers = _context.survPoolWeekList.OrderBy(x => x.WeekNumber).Where(x => x.PoolID == Pool_ID).Select(x => x.WeekNumber).ToList();
                objApiResponse.Message = "success";
                objApiResponse.Status = Messages.APIStatusSuccess;
                objApiResponse.weekNumbers = weekNumbers;
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpPost("CreatePoolWeek")]
        public APIResponses CreatePoolWeek([FromBody] PoolWeekRequest objWeek, string LoginKey)
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


                SurvPoolWeekList survScheduleWeek = _context.survPoolWeekList.FirstOrDefault(x => x.WeekNumber == objWeek.Week_Number && x.PoolID == objWeek.Pool_ID);
                if (survScheduleWeek != null)
                {
                    survScheduleWeek.CutOff = objWeek.Cut_Off_Date;
                    survScheduleWeek.Start = objWeek.Start;
                    survScheduleWeek.Updated = DateTime.Now;
                    _context.survPoolWeekList.Add(survScheduleWeek);
                    _context.Entry(survScheduleWeek).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    if (survScheduleWeek.Start == true)
                    {
                        objApiResponse.Status = Messages.APIStatusSuccess;
                        objApiResponse.Message = "Week " + objWeek.Week_Number + " officially closed.";


                        //Default picks
                        //CommonController objCommon = new CommonController(_context);
                        //objCommon.CronPickDefaulted();
                    }
                    //else if (survScheduleWeek.Start == false)
                    //{
                    //    objApiResponse.Status = Messages.APIStatusSuccess;
                    //    objApiResponse.Message = "Week " + objWeek.Week_Number + " officially opened.";
                    //}
                    else
                    {
                        objApiResponse.Status = Messages.APIStatusSuccess;
                        objApiResponse.Message = Messages.UpdatedSuccess;
                    }
                }

                else
                {
                    //SurvPoolWeekList survPoolWeekList = _context.survPoolWeekList.FirstOrDefault(x => x.PoolID == objWeek.Pool_ID && x.WeekNumber == objWeek.Week_Number);

                    //if (survPoolWeekList != null)
                    //{
                    //    objApiResponse.Status = Messages.APIStatusError;
                    //    objApiResponse.Message = "This week is already exist.";
                    //    return objApiResponse;

                    //}
                    //else
                    //{
                    SurvPoolWeekList obj = new SurvPoolWeekList();
                    // obj.Pool_Name = objWeek.Pool_Name;
                    obj.PoolID = objWeek.Pool_ID;
                    obj.WeekNumber = objWeek.Week_Number;
                    obj.CutOff = objWeek.Cut_Off_Date;
                    obj.Start = objWeek.Start;

                    _context.survPoolWeekList.Add(obj);
                    _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();
                    //}  

                    objApiResponse.Status = Messages.APIStatusSuccess;
                    objApiResponse.Message = Messages.SavedSuccess;
                }

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Exception: " + ex.Message; objApiResponse.Status = Messages.APIStatusError; GNF.SaveException(ex, _context);
            }
            return objApiResponse;
        }

        [HttpDelete("DeletePoolWeek")]
        public APIResponses DeletePoolWeek(string LoginKey, int Pool_ID, int WeekNumber)
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


                //var weekNumbers = _context.survPoolWeekList.OrderBy(x => x.WeekNumber).Where(x => x.PoolID == Pool_ID).Select(x => x.WeekNumber).ToList();
                var SurvScheduleList = _context.SurvScheduleList.Where(x => x.PoolID == Pool_ID && x.WeekNumber == WeekNumber).ToList();
                if (SurvScheduleList != null)
                {
                    _context.SurvScheduleList.RemoveRange(SurvScheduleList);
                    _context.SaveChanges();
                }
                var weekNumbers = _context.survPoolWeekList.FirstOrDefault(x => x.PoolID == Pool_ID && x.WeekNumber == WeekNumber);
                if (weekNumbers != null)
                {
                    _context.survPoolWeekList.Remove(weekNumbers);
                    _context.SaveChanges();
                }
                objApiResponse.Message = "Week has been deleted!";
                objApiResponse.Status = Messages.APIStatusSuccess;

            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Week has not been deleted due to the folowing error(s): " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
            }
            return objApiResponse;
        }

        [HttpGet("GetPoolWeek")]
        public PoolWeekListAPIResponse GetPoolWeek(string LoginKey, int Pool_ID, int WeekNumber)
        {
            PoolWeekListAPIResponse objApiResponse = new PoolWeekListAPIResponse();
            try
            {
                TokenHelper tk = new TokenHelper(_context);
                if (!tk.ValidateToken(LoginKey))
                {
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.Message = Messages.InvalidToken;
                    return objApiResponse;
                }
                var weekNumbers = _context.survPoolWeekList.Where(x => x.PoolID == Pool_ID && x.WeekNumber == WeekNumber).ToList();
                if (weekNumbers != null)
                {
                    objApiResponse.Message = "No week found!";
                    objApiResponse.Status = Messages.APIStatusError;
                    objApiResponse.survPoolWeekLists = weekNumbers;
                }
                else
                {
                    objApiResponse.Message = "No week found!";
                    objApiResponse.Status = Messages.APIStatusError;
                }
            }
            catch (Exception ex)
            {
                objApiResponse.Message = "Week not found due to the folowing error(s): " + ex.Message;
                objApiResponse.Status = Messages.APIStatusError;
            }
            return objApiResponse;
        }

        //[HttpGet("")]
        //public PoolWeekListAPIResponse GetPoolWeek(string LoginKey, int Pool_ID, int WeekNumber)
        //{
        //    PoolWeekListAPIResponse objApiResponse = new PoolWeekListAPIResponse();
        //    try
        //    {
        //        TokenHelper tk = new TokenHelper(_context);
        //        if (!tk.ValidateToken(LoginKey))
        //        {
        //            objApiResponse.Status = Messages.APIStatusError;
        //            objApiResponse.Message = Messages.InvalidToken;
        //            return objApiResponse;
        //        }
        //        var weekNumbers = _context.survPoolWeekList.Where(x => x.PoolID == Pool_ID && x.WeekNumber == WeekNumber).ToList();
        //        if (weekNumbers != null)
        //        {
        //            objApiResponse.Message = "No week found!";
        //            objApiResponse.Status = Messages.APIStatusError;
        //            objApiResponse.survPoolWeekLists = weekNumbers;
        //        }
        //        else
        //        {
        //            objApiResponse.Message = "No week found!";
        //            objApiResponse.Status = Messages.APIStatusError;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objApiResponse.Message = "Week not found due to the folowing error(s): " + ex.Message;
        //        objApiResponse.Status = Messages.APIStatusError;
        //    }
        //    return objApiResponse;
        //}

    }

}