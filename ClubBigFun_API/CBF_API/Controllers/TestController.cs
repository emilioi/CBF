using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBF_API.Helpers;
using CBF_API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CBF_API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly CbfDbContext _context;
        private IHostingEnvironment _env;
        public TestController(CbfDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet("NFLRequest")]
        public APIResponses NFLRequest()
        {
            APIResponses objResponse = new APIResponses();

            objResponse.Message = "API is okay";
            objResponse.Status =  Messages.APIStatusSuccess;
            try
            {
                NHL_API_Helper objNFL = new NHL_API_Helper(_context);
                //objNFL.RequestSeasonalAPI2();
               objNFL.Get_NHLSchedule_API();
                //  objNFL.RequestBoxScoreAPI();
            }
            catch (Exception ex)
            {
                objResponse.Message = "API is not good. " + ex.Message;
                objResponse.Status =  Messages.APIStatusError;
            }
            return objResponse;
        }

        [HttpGet("TestEmail")]
        public bool TestEmail()
        {
            Email_Templates templates = _context.Email_Templates.FirstOrDefault(x => x.EmailID == "MemberRegistration");

            string subject = templates.Subject;
            string body = "";
            CbfDbContext db = new CbfDbContext();

            string message = templates.Body;
            message = message.Replace("#Email#", "akshaymishra.babu@gmail.com");
            body = message;
            EmailHelper objEMail = new EmailHelper(_env);
            objEMail.SendMailDirect("akshaymishra.babu@gmail.com", subject, message, true);
            return true;
        }
        [HttpGet("WinnerUpdates")]
        public bool WinnerUpdates()
        {


            return true;
        }
    }
}