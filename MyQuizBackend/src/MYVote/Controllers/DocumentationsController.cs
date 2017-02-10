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
    public class DocumentationsController : Controller
    {
        // GET: api/documentations
        [HttpGet]
        public FileStreamResult GetDocumentations()
        {
            Response.Headers.Add("content-disposition", "attachment; filename=Documentations.zip");
            var pathToDocus = Path.Combine(Startup._iHostingEnv.ContentRootPath, @"Logs\Documentations.zip");

            return File(new FileStream(pathToDocus, FileMode.Open),
                "application/zip");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
