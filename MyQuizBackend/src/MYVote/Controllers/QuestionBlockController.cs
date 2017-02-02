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
        
        // GET api/questionBlock/
        [HttpGet]
        public IActionResult GetAllQuestionBlocks()
        {
            List<QuestionBlock> questionBlockList = new List<QuestionBlock>();
            using (var db = new EF_DB_Context())
            {
                foreach (var questionBlockInDb in db.QuestionBlock)
                {
                    var questionsFinder =
                    from q in db.QuestionQuestionBlock
                    where q.QuestionBlockId == questionBlockInDb.Id
                    select q.QuestionId;

                    var questionList = db.Question.Where(q => questionsFinder.Any(q2 => q2 == q.Id));

                    foreach (var question in questionList)
                    {
                        var answerOptionsFinder = from a in db.QuestionAnswerOption
                                                  where a.QuestionId == question.Id
                                                  select a.AnswerOptionId;

                        question.answerList = db.AnswerOption.Where(a => answerOptionsFinder.Any(a2 => a2 == a.Id)).ToList();
                    }

                    questionBlockInDb.questionList = questionList.ToList();
                    questionBlockList.Add(questionBlockInDb);
                }
                
                return Ok(JsonConvert.SerializeObject(questionBlockList));
            }
        }

        // GET api/questionBlock/:id
        [HttpGet("{id}")]
        public IActionResult GetQuestionBlockById(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var questionBlockInDb = db.QuestionBlock.FirstOrDefault(qb => qb.Id == id);
                if (questionBlockInDb == null) return BadRequest("Id not found!");

                var questionsFinder =
                    from q in db.QuestionQuestionBlock
                    where q.QuestionBlockId == questionBlockInDb.Id
                    select q.QuestionId;

                var questionList = db.Question.Where(q => questionsFinder.Any(q2 => q2 == q.Id));

                foreach (var question in questionList)
                {
                    var answerOptionsFinder = from a in db.QuestionAnswerOption
                        where a.QuestionId == question.Id
                        select a.AnswerOptionId;

                    question.answerList = db.AnswerOption.Where(a => answerOptionsFinder.Any(a2 => a2 == a.Id)).ToList();
                }
                questionBlockInDb.questionList = questionList.ToList();
                
                return Ok(JsonConvert.SerializeObject(questionBlockInDb));
            }
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
