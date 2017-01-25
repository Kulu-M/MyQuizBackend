using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MYVote.Models;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyQuizBackend.Controllers
{
    [Route("api/[controller]")]
    public class QuestionsListsController : Controller
    {
        #region GET

        // GET: api/questionslists
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/questionslists/id
        [HttpGet("{id}")]
        public string GetPreparedFinalQuestion(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var finalQ = db.FinalQuestion.FirstOrDefault(fq => fq.Id == id);
                return JsonConvert.SerializeObject(finalQ);
            }
        }

        #endregion GET

        // POST api/questionslists
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/questionslists/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/questionslists/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
