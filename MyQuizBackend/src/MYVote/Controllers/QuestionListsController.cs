using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MYVote.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyQuizBackend.Controllers
{
    [Route("api/[controller]")]
    public class QuestionListsController : Controller
    {
        #region GET

        // GET api/questionLists/id
        [HttpGet("{id}")]
        public IActionResult GetPreparedFinalQuestion(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var finalQ = db.FinalQuestion.FirstOrDefault(fq => fq.Id == id);
                if (finalQ == null) return NotFound();
                return Ok(JsonConvert.SerializeObject(finalQ));
            }
        }

        #endregion GET

        // POST api/questionLists
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/questionLists/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #region DELETE

        // DELETE api/questionLists
        [HttpDelete]
        public void DeleteQuestionList([FromBody]JObject value)
        {
            var questionList = JsonConvert.DeserializeObject<List<Question>>(value.ToString());

            foreach (var q in questionList)
            {
                using (var db = new EF_DB_Context())
                {
                    var questionToDelete = db.Question.FirstOrDefault(questionInDb => questionInDb.Id == q.Id);
                    if (questionToDelete == null) continue;
                    db.Question.Remove(questionToDelete);
                    db.SaveChanges();
                }
            }
        }

        #endregion DELETE
    }
}
