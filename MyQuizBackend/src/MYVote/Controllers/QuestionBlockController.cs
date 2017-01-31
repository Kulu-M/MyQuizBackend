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
    public class QuestionBlockController : Controller
    {
        #region GET

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        #endregion GET

        #region POST

        // POST api/values
        [HttpPost]
        public IActionResult PostNewQuestionBlock([FromBody] JObject value)
        {
            if (value == null) return BadRequest();
            var questionBlock = JsonConvert.DeserializeObject<QuestionBlock>(value.ToString());

            using (var db = new EF_DB_Context())
            {
                db.QuestionBlock.Add(questionBlock);
                db.SaveChanges();

                foreach (var question in questionBlock.questionList)
                {
                    db.Question.Add(question);
                    db.SaveChanges();

                    foreach (var answerOption in question.answerList)
                    {
                        db.AnswerOption.Add(answerOption);
                        db.SaveChanges();
                        db.QuestionAnswerOption.Add(new QuestionAnswerOption
                        {
                            AnswerOptionId = answerOption.Id,
                            QuestionId = question.Id
                        });
                        db.SaveChanges();
                    }

                    db.QuestionQuestionBlock.Add(new QuestionQuestionBlock
                    {
                        QuestionBlockId = questionBlock.Id,
                        QuestionId = question.Id
                    });
                    db.SaveChanges();
                }
            }
            return Ok(JsonConvert.SerializeObject(questionBlock));
        }

        #endregion POST

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
