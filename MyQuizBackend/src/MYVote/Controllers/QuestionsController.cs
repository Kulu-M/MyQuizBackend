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
    public class QuestionsController : Controller
    {
        #region GET

        // GET api/questions
        [HttpGet]
        public string GetAllQuestions()
        {
            using (var db = new EF_DB_Context())
            {
                var question = from q in db.Question select q;
                return JsonConvert.SerializeObject(question);
            }
        }

        // GET api/questions/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        #endregion GET

        #region POST

        // POST api/questions
        [HttpPost]
        public void CreateOrUpdateQuestion([FromBody]string value)
        {
            var question = JsonConvert.DeserializeObject<Question>(value.ToString());
            using (var db = new EF_DB_Context())
            {
                var existingQuestion = db.Question.First(q => q.Id == question.Id);
                if (existingQuestion == null)
                {
                    db.Question.Add(question);
                    db.SaveChanges();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(question.Text)) return;
                    existingQuestion.Text = question.Text;
                    db.SaveChanges();
                }
            }
        }

        #endregion POST

        // PUT api/questions/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #region DELETE
        
        // DELETE api/questions/id
        [HttpDelete("{id}")]
        public void DeleteQuestion(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var questionToDelete = db.Question.FirstOrDefault(q => q.Id == id);
                if (questionToDelete == null) return;
                db.Question.Remove(questionToDelete);
                db.SaveChanges();
            }
        }

        #endregion DELETE
    }
}
