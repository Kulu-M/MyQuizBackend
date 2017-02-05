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
    public class GivenAnswerController : Controller
    {
        #region GET

        // GET api/givenAnswer
        [HttpGet]
        public IActionResult GetAllGivenAnswers()
        {
            var givenAnswers = new List<GivenAnswer>();
            using (var db = new EF_DB_Context())
            {
                givenAnswers = (from g in db.GivenAnswer select g).ToList();
            }
            if (!givenAnswers.Any()) return BadRequest("No data present!");

            foreach (var ga in givenAnswers)
            {
                ga.fillValues();
            }
            return Ok(JsonConvert.SerializeObject(givenAnswers));
        }

        // GET api/givenAnswer/:id
        [HttpGet("{id}")]
        public IActionResult GetGivenAnswerById(int id)
        {
            GivenAnswer givenAnswer;
            using (var db = new EF_DB_Context())
            {
                givenAnswer = db.GivenAnswer.FirstOrDefault();
                if (givenAnswer == null) return BadRequest("No data present!");
            }
            givenAnswer.fillValues();
            return Ok(JsonConvert.SerializeObject(givenAnswer));
        }

        #endregion GET

        #region POST
        
        // POST api/values
        [HttpPost]
        public IActionResult CreateOrUpdateGivenAnswer([FromBody] JObject value)
        {
            GivenAnswer givenAnswer;
            GivenAnswer existingGivenAnswer;
            if (value == null) return BadRequest();
            try
            {
                givenAnswer = JsonConvert.DeserializeObject<GivenAnswer>(value.ToString());
            }
            catch (Exception)
            {
                return BadRequest("Could not deserialize!");
            }

            using (var db = new EF_DB_Context())
            {
                existingGivenAnswer = db.GivenAnswer.FirstOrDefault(qb => qb.Id == givenAnswer.Id);
            }
            //givenAnswer is new
            if (existingGivenAnswer == null)
            {
                saveGivenAnswerToDatabase(givenAnswer);
            }

            //givenAnswer already exists in Database
            else
            {
                removeGivenAnswerFromDatabase(existingGivenAnswer);
                saveGivenAnswerToDatabase(givenAnswer);
            }
            return Ok(JsonConvert.SerializeObject(givenAnswer));
        }
        
        #endregion POST

        #region DELETE

        // DELETE api/givenAnswer/:id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var givenAnswerInDb = db.GivenAnswer.FirstOrDefault(qb => qb.Id == id);
                if (givenAnswerInDb == null) return BadRequest("Id not found!");

                removeGivenAnswerFromDatabase(givenAnswerInDb);

                return Ok("Deleted: " + id);
            }
        }

        #endregion DELETE

        #region LOGIC

        public void saveGivenAnswerToDatabase(GivenAnswer givenAnswer)
        {
            using (var db = new EF_DB_Context())
            {
                givenAnswer.fillIds();
                db.GivenAnswer.Add(givenAnswer);
                db.SaveChanges();
            }
        }

        private void removeGivenAnswerFromDatabase(GivenAnswer givenAnswer)
        {
            using (var db = new EF_DB_Context())
            {
                db.GivenAnswer.Remove(givenAnswer);
                db.SaveChanges();
            }
        }

        #endregion LOGIC
    }
}
