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
        public string GetAllGroups()
        {
            using (var db = new EF_DB_Context())
            {
                var groups = from gr in db.Group select gr;
                return JsonConvert.SerializeObject(groups);
            }
        }

        // GET api/groups/id/topics
        [HttpGet("{id}/topics")]
        public string GetAllTopicsForId(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var singleTopicIdFinder = (from topicSingletopic in db.TopicSingleTopic where topicSingletopic.TopicId == id select topicSingletopic.SingleTopicId);

                var singleTopics = db.SingleTopic.Where(stp => singleTopicIdFinder.Any(stp2 => stp2 == stp.Id));

                return JsonConvert.SerializeObject(singleTopics);
            }
        }

        // GET api/groups/id/questions
        [HttpGet("{id}/questions")]
        public string GetAllFinalQuestionsForGroupId(int id)
        {
            using (var db = new EF_DB_Context())
            {
               var finalQuestionsForGroupId = db.FinalQuestion.Where(fq => fq.GroupId == id);

               return JsonConvert.SerializeObject(finalQuestionsForGroupId);
            }
        }

        #endregion GET

        #region POST

        // POST api/groups
        [HttpPost]
        public void CreateOrUpdateGroup([FromBody]JObject value)
        {
            var group = JsonConvert.DeserializeObject<Group>(value.ToString());
            using (var db = new EF_DB_Context())
            {
                var existingGroup = db.Group.First(gr => gr.Id == group.Id);
                if (existingGroup == null)
                {
                    db.Group.Add(group);
                    db.SaveChanges();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(group.Title))
                    {
                        existingGroup.Title = group.Title;
                    }
                    if (group.Topic != null)
                    {
                        existingGroup.Topic = group.Topic;
                    }
                    if (group.EnterGroupPin != null )
                    {
                        existingGroup.EnterGroupPin = group.EnterGroupPin;
                    }
                    db.SaveChanges();
                }
            }
        }

        // POST api/groups/{id}/questions/{questionId}/answers
        [HttpPost("{id}/questions/{questionId}/answers")]
        public void ClientAnswerInput(int id, int questionId, [FromBody]JObject value)
        {
            var givenAnswer = JsonConvert.DeserializeObject<GivenAnswer>(value.ToString());

            using (var db = new EF_DB_Context())
            {
                db.GivenAnswer.Add(givenAnswer);
                db.SaveChanges();
            }
        }

        [HttpPost("{id}/topics")]
        public void Post(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var topics = new Topic();
                topics.Name = "";
                db.Topic.Add(topics);
                db.SaveChanges();
            }
        }

        #endregion POST

        #region PUT

        // PUT api/groups/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #endregion PUT

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
