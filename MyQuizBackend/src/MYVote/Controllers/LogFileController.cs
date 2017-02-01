using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MYVote;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyQuizBackend.Controllers
{
    [Route("api/[controller]")]
    public class LogFileController : Controller
    {
        // GET: api/logfile
        [HttpGet]
        public FileStreamResult GetLogFile()
        {
            Response.Headers.Add("content-disposition", "attachment; filename=ExceptionLogFile.txt");
            var pathToLogFile = Path.Combine(Startup._iHostingEnv.ContentRootPath, @"Logs\ExceptionLogFile.txt");

            return File(new FileStream(pathToLogFile, FileMode.Open),
                "application/octet-stream");
        }
    }
}
