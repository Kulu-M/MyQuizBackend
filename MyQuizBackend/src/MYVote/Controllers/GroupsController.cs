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
        public string Get()
        {
            using (var db = new EF_DB_Context())
            {
                var groups = from gr in db.Group select gr;
                return JsonConvert.SerializeObject(groups);
            }
        }

        // GET api/groups/id/topics
        [HttpGet("{id}/topics")]
        public string Get(int id)
        {
            using (var db = new EF_DB_Context())
            {
                var singleTopicIdFinder = (from topicSingletopic in db.TopicSingleTopic where topicSingletopic.TopicId == id select topicSingletopic.SingleTopicId);

                var singleTopics = db.SingleTopic.Where(stp => singleTopicIdFinder.Any(stp2 => stp2 == stp.Id));

                return JsonConvert.SerializeObject(singleTopics);
            }
        }

        #endregion GET

        #region POST

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

        [HttpPost]
        public void Post([FromBody]string value)
        {
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

        // PUT api/groups/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/groups/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
