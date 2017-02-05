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
                ga.fillWithValues();
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
            givenAnswer.fillWithValues();
            return Ok(JsonConvert.SerializeObject(givenAnswer));
        }

        #endregion GET

        #region POST


        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        #endregion POST

        #region DELETE

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion DELETE

        #region LOGIC

        #endregion LOGIC
    }
}
