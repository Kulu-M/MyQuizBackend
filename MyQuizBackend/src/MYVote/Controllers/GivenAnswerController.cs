﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyQuizBackend.Classes;
using MYVote.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyQuizBackend.Controllers
{
    [Route("api/[controller]")]
    public class GivenAnswerController : Controller
    {
        private readonly IVoteConnector _voteConnector;
        public GivenAnswerController(IVoteConnector connector) {
            _voteConnector = connector;
        }
        #region GET

        // GET api/givenAnswer
        [HttpGet]
        public IActionResult GetAllGivenAnswers([FromQuery] int groupId, [FromQuery] int singleTopicId)
        {
            var givenAnswers = new List<GivenAnswer>();
            using (var db = new EF_DB_Context())
            {
                givenAnswers = (from g in db.GivenAnswer select g).ToList();
            }
            if (!givenAnswers.Any()) return BadRequest("No data present!");

            if (groupId != 0)
            {
                givenAnswers = givenAnswers.Where(x => x.GroupId != groupId).ToList() ;
            }

            if (singleTopicId != 0)
            {
                givenAnswers = givenAnswers.Where(x => x.SingleTopicId != singleTopicId).ToList();
            }
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

        // GET api/givenAnswer/latest
        [HttpGet("latest")]
        public IActionResult GetLatestQuestionForDevice()
        {
            var deviceID = DeviceAuthentification.getClientIDfromHeader(Request);
            if (deviceID < 0) return BadRequest();

            List<GivenAnswer> givenAnswerListForClient;

            using (var db = new EF_DB_Context())
            {
                var DeviceGroupList = from temp in db.DeviceGroup where temp.DeviceId == deviceID select temp;
                var GroupList = db.Group.Where(temp => DeviceGroupList.Any(temp2 => temp2.GroupId == temp.Id));
                givenAnswerListForClient = db.GivenAnswer.Where(temp => GroupList.Any(temp2 => temp2.Id == temp.GroupId)).ToList();
            }
            return Ok(JsonConvert.SerializeObject(givenAnswerListForClient));
        }

        #endregion GET

        #region POST

        // POST api/givenanswer
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateGivenAnswer([FromBody] JObject value)
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
                if (givenAnswer.Device != null && givenAnswer.AnswerOption != null)
                {
                    //Check if answeroption and client is filled - if yes it comes from client and needs to be pushed to supervisor via socket
                    try {
                        // Get socketHandler for specific surveyId
                        var surveyId = (int)givenAnswer.SurveyId;
                        var socketHandler = _voteConnector.GetSocketHandlers()[surveyId];
                        // Send the new givenAnswer to WebSocketClient (Supervisor Application)
                        await socketHandler.SendGivenAnswer(value.ToString());
                    } catch (Exception) {
                        return BadRequest("There is no survey with this ID currently");
                    }
                }
                saveGivenAnswerToDatabase(givenAnswer);
            } else
            {                
                //givenAnswer already exists in Database
                // this  should not be possible since the givenanswer id is generated when saving a new one to DB
                return BadRequest("This GivenAnswer already exists!");
            }
            return Ok(JsonConvert.SerializeObject(givenAnswer));
        }

        // POST api/start/{surveyTimeInSeconds}
        [HttpPost("start/{seconds}")]
        public IActionResult PublishGivenAnswerToClients(int seconds, [FromBody] JObject value)
        {
            if (seconds < 0) return BadRequest("No negative times possible!");

            List<GivenAnswer> newGivenAnswers;
            if (value == null) return BadRequest("Empty body");
            try
            {
                newGivenAnswers = JsonConvert.DeserializeObject<List<GivenAnswer>>(value.ToString());
            }
            catch (Exception)
            {
                return BadRequest("Could not deserialize!");
            }
            // create random surveyId since AutoIncrement only work for the PrimaryKey in sqlite
            var rnd = new Random();
            var surveyId = rnd.Next(1,int.MaxValue);
            // TODO: maybe check if surveyId exists in DB and create a new one if it does

            foreach(var ga in newGivenAnswers) {
                // Timestamp to know when survey ends
                ga.TimeStamp = (Time.ConvertToUnixTimestamp(DateTime.Now) + seconds).ToString();
                // Add surveyId to each GivenAnswer
                ga.SurveyId = surveyId;
            }

            //publish these givenanswers to clients via push notification

            // send GivenAnswers with added surveyId back to supervisor so he can create a websocket connection with this surveyId
            // ws://localhost:5000/ws/{surveyId}
            return Ok(JsonConvert.SerializeObject(newGivenAnswers));
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
