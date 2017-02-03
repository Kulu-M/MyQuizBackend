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
    public class GroupsController : Controller
    {
        #region GET
        
        // GET api/groups
        [HttpGet]
        public IActionResult GetAllGroups()
        {
            using (var db = new EF_DB_Context())
            {
                var groups = from gr in db.Group select gr;
                return Ok(JsonConvert.SerializeObject(groups));
            }
        }

        // GET api/groups/id/topics
        [HttpGet("{id}/topics")]
        public IActionResult GetAllTopicsForId(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var singleTopicIdFinder = (from groupSingletopic in db.GroupSingleTopic where groupSingletopic.GroupId == id select groupSingletopic.SingleTopicId);

                var singleTopics = db.SingleTopic.Where(stp => singleTopicIdFinder.Any(stp2 => stp2 == stp.Id));

                return Ok(JsonConvert.SerializeObject(singleTopics));
            }
        }
        
        #endregion GET

        #region POST

        // POST api/groups
        [HttpPost]
        public IActionResult CreateOrUpdateGroup([FromBody]JObject value)
        {
            Group group;
            if (value == null) return BadRequest();
            try
            {
                group = JsonConvert.DeserializeObject<Group>(value.ToString());
            }
            catch (Exception)
            {
                return BadRequest("Could not deserialize!");
            }
            
            using (var db = new EF_DB_Context())
            {
                var existingGroup = db.Group.FirstOrDefault(gr => gr.Id == group.Id);
                if (existingGroup == null)
                {
                    db.Group.Add(group);
                    db.SaveChanges();
                }
                existingGroup.EnterGroupPin = group.EnterGroupPin;
                existingGroup.Title = group.Title;
                db.SaveChanges();
                return Ok(JsonConvert.SerializeObject(group));
            }
        }

        // POST api/groups/{id}/questions/{questionId}/answers
        [HttpPost("{id}/questions/{questionId}/answers")]
        public IActionResult ClientAnswerInput(int id, int questionId, [FromBody]JObject value)
        {
            var deviceID = DeviceAuthentification.getClientIDfromHeader(Request);
            if (deviceID < 0) return Unauthorized();

            GivenAnswer givenAnswer;
            if (value == null) return BadRequest();
            try
            {
                givenAnswer = JsonConvert.DeserializeObject<GivenAnswer>(value.ToString());
            }
            catch (Exception)
            {
                return BadRequest("Could not deserialize!");
            }
            if (givenAnswer == null) return BadRequest();

            using (var db = new EF_DB_Context())
            {
                db.GivenAnswer.Add(givenAnswer);
                db.SaveChanges();
            }
            return Ok(JsonConvert.SerializeObject(givenAnswer));
        }

        // POST api/groups/{id}/topics
        [HttpPost("{id}/topics")]
        public IActionResult PostNewTopicToGroup(int id, [FromBody]JArray value)
        {
            List<SingleTopic> listSingleTopics;
            if (value == null) return BadRequest();
            try
            {
                listSingleTopics = JsonConvert.DeserializeObject<List<SingleTopic>>(value.ToString());
            }
            catch (Exception)
            {
                return BadRequest("Could not deserialize!");
            }
            if (listSingleTopics == null || !listSingleTopics.Any()) return BadRequest();
            
            foreach (var singleTopic in listSingleTopics)
            {
                using (var db = new EF_DB_Context())
                {
                    //SaveChanges needs to be called inbetween to let the Database give the new entity an ID
                    db.SingleTopic.Add(singleTopic);
                    db.SaveChanges();

                    var groupSingle = new GroupSingleTopic();
                    groupSingle.GroupId = id;
                    groupSingle.SingleTopicId = singleTopic.Id;
                    db.GroupSingleTopic.Add(groupSingle);

                    db.SaveChanges();
                }
            }
            return Ok();
        }

        #endregion POST

        #region DELETE

        // DELETE api/groups/id
        [HttpDelete("{id}")]
        public IActionResult DeleteGroup(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var groupToDelete = db.Group.FirstOrDefault(gr => gr.Id == id);
                if (groupToDelete == null) return StatusCode(404);
                db.Group.Remove(groupToDelete);
                db.SaveChanges();
                return Ok(JsonConvert.SerializeObject(groupToDelete));
            }
        }

        #endregion DELETE
    }
}
