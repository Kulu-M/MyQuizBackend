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
    public class QuestionsController : Controller
    {
        #region GET

        // GET api/questions
        [HttpGet]
        public IActionResult GetAllQuestions()
        {
            using (var db = new EF_DB_Context())
            {
                var question = from q in db.Question select q;
                if (!question.Any()) return BadRequest("No data present!");
                foreach (var q in question)
                {
                    q.fillValues();
                }
                return Ok(JsonConvert.SerializeObject(question));
            }
        }

        // GET api/questions/:id
        [HttpGet("{id}")]
        public IActionResult GetSingleQuestionById(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var q = db.Question.FirstOrDefault(qq => qq.Id == id);
                if (q == null) return BadRequest("No data present!");
                q.fillValues();
                return Ok(JsonConvert.SerializeObject(q));
            }
        }

        #endregion GET

        #region POST

        // POST api/questions
        [HttpPost]
        public IActionResult CreateOrUpdateQuestion([FromBody]JObject value)
        {
            if (value == null) return BadRequest();
            Question question;
            try
            {
                question = JsonConvert.DeserializeObject<Question>(value.ToString());
            }
            catch (Exception)
            {
                return BadRequest("Could not deserialize!");
            }
            
            using (var db = new EF_DB_Context())
            {
                var existingQuestion = db.Question.First(q => q.Id == question.Id);
                if (existingQuestion == null)
                {
                    //Question is new
                    saveQuestionWithAnswerOptionsToDatabase(question);
                    return Ok(JsonConvert.SerializeObject(question));
                }
                else
                {
                    //Question already exists => update
                    removeQuestionWithAnswerOptionsFromDatabase(question);
                    saveQuestionWithAnswerOptionsToDatabase(question);
                    return Ok(JsonConvert.SerializeObject(question));
                }
            }
        }

        #endregion POST

       #region DELETE
        
        // DELETE api/questions/id
        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var questionToDelete = db.Question.FirstOrDefault(q => q.Id == id);
                if (questionToDelete == null) return BadRequest("No data present!");
                removeQuestionWithAnswerOptionsFromDatabase(questionToDelete);
                return Ok(JsonConvert.SerializeObject(questionToDelete));
            }
        }

        #endregion DELETE

        #region LOGIC

        public static void saveQuestionWithAnswerOptionsToDatabase(Question question)
        {
            using (var db = new EF_DB_Context())
            {
                db.Question.Add(question);
                db.SaveChanges();

                foreach (var answerOption in question.AnswerOptions)
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
            }
        }

        public static void removeQuestionWithAnswerOptionsFromDatabase(Question q)
        {
            using (var db = new EF_DB_Context())
            {
                var questionAnswerOptionsToDelete = (from a in db.QuestionAnswerOption
                                                        where a.QuestionId == q.Id
                                                        select a);

                //Delete from AnswerOption
                db.AnswerOption.RemoveRange(
                    db.AnswerOption.Where(a => questionAnswerOptionsToDelete.Any(a2 => a2.AnswerOptionId == a.Id)));

                //Delete from QuestionAnswerOption
                db.QuestionAnswerOption.RemoveRange(questionAnswerOptionsToDelete);
  
                //Delete from Question
                db.Question.Remove(q);

                db.SaveChanges();
            }
        }

        #endregion LOGIC
    }
}
