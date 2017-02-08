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
        
        // Get all Groups
        // GET api/groups
        [HttpGet]
        public IActionResult GetAllGroups()
        {
            using (var db = new EF_DB_Context())
            {
                var groups = from gr in db.Group select gr;

                foreach (var g in groups)
                {
                    var groupSingleTopics = from gst in db.GroupSingleTopic
                        where gst.GroupId == g.Id
                        select gst;

                    g.SingleTopics = db.SingleTopic.Where(st => groupSingleTopics.Any(st2 => st2.SingleTopicId == st.Id)).ToList();
                }
                return Ok(JsonConvert.SerializeObject(groups));
            }
        }

        // Get one Group by its ID
        // GET api/groups/id
        [HttpGet("{id}")]
        public IActionResult GetGroupById(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var g = db.Group.FirstOrDefault(gr => gr.Id == id);
                if (g == null) return BadRequest("No such group!");

                var groupSingleTopics = from gst in db.GroupSingleTopic
                                        where gst.GroupId == g.Id
                                        select gst;

                g.SingleTopics = db.SingleTopic.Where(st => groupSingleTopics.Any(st2 => st2.SingleTopicId == st.Id)).ToList();

                return Ok(JsonConvert.SerializeObject(g));
            }
        }

        //TODO Deprecated
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
                    //Group is new
                    saveNewGroupToDatabase(group);
                }
                else
                {
                    //Group is already in DB (--> update)
                    removeGroupFromDatabase(group);
                    saveNewGroupToDatabase(group);
                }
                return Ok(JsonConvert.SerializeObject(group));
            }
        }

        //TODO Deprecated
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

        // POST api/groups/{id}/questions/{questionId}/answers
        [HttpPost("{id}/questions/{questionId}/answers")]
        public IActionResult ClientAnswerInput(int id, int questionId, [FromBody]JObject value)
        {
            var deviceID = DeviceAuthentification.getClientIDfromHeader(Request);
            if (deviceID < 0) return BadRequest();

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

        #endregion POST

        #region DELETE

        // DELETE api/groups/id
        [HttpDelete("{id}")]
        public IActionResult DeleteGroup(int id)
        {
            var deviceID = DeviceAuthentification.getClientIDfromHeader(Request);
            if (deviceID < 0 || DeviceAuthentification.authenticateAdminDeviceByDeviceID(deviceID) == false) return Unauthorized();

            Group groupToDelete;
            using (var db = new EF_DB_Context())
            {
                //Get Group
                groupToDelete = db.Group.FirstOrDefault(gr => gr.Id == id);
                if (groupToDelete == null) return StatusCode(404);
            }

            removeGroupFromDatabase(groupToDelete, true);
            
            return Ok(JsonConvert.SerializeObject(groupToDelete));
            
        }

        #endregion DELETE

        #region LOGIC

        public void saveNewGroupToDatabase(Group group)
        {
            using (var db = new EF_DB_Context())
            {
                //Create new Group
                db.Group.Add(group);
                db.SaveChanges();

                foreach (var topic in group.SingleTopics)
                {
                    db.SingleTopic.Add(topic);
                    db.SaveChanges();

                    var groupSingleTopic = new GroupSingleTopic
                    {
                        GroupId = group.Id,
                        SingleTopicId = topic.Id
                    };
                    db.GroupSingleTopic.Add(groupSingleTopic);
                    db.SaveChanges();
                }
            }
        }

        public void removeGroupFromDatabase(Group groupToDelete, bool deleteDeviceAndDevicerelations = false)
        {
            using (var db = new EF_DB_Context())
            {
                //Get GroupSingleTopics to delete
                var groupSingleTopicsToDelete = from gst in db.GroupSingleTopic where gst.GroupId == groupToDelete.Id select gst;

                //Get SingleTopics to delete
                var singleTopicToDelete =
                    db.SingleTopic.Where(st => groupSingleTopicsToDelete.Any(st2 => st2.SingleTopicId == st.Id));

                //Delete from SingleTopic
                db.SingleTopic.RemoveRange(singleTopicToDelete);

                //Delete from GroupSingleTopic
                db.GroupSingleTopic.RemoveRange(groupSingleTopicsToDelete);

                //Delete relations with Device if Group gets deleted for good
                if (deleteDeviceAndDevicerelations)
                {
                    db.DeviceGroup.RemoveRange(db.DeviceGroup.Where(dg => dg.Id == groupToDelete.Id));
                }

                //Delete the Group
                db.Group.Remove(groupToDelete);

                db.SaveChanges();
            }
        }

        #endregion LOGIC
    }
}
