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
    public class GroupsController : Controller
    {
        // GET: api/groups
        [HttpGet]
        public string Get()
        {
            //Get All Groups
            JsonSerializer serializer = new JsonSerializer();

            using (var db = new APIVoteDbContext())
            {
                var groups = (from x in db.Group select x);
                //groups = db.Group.First(g => g.Id >= 1);

                return JsonConvert.SerializeObject(groups);

            }
        }

        // GET api/groups/id/topics
        [HttpGet("{id}/topics")]
        public string Get(int id)
        {
            JsonSerializer serializer = new JsonSerializer();

            using (var db = new APIVoteDbContext())
            {
                var topics = (from x in db.Topic select x);
                //groups = db.Group.First(g => g.Id >= 1);

                return JsonConvert.SerializeObject(topics);

            }

        }

        // POST api/groups
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        [HttpPost("{id}/topics")]
        public void Post(int id)
        {
            using (var db = new APIVoteDbContext())
            {
                var topics = new Topic();
                topics.Id = 1;
                topics.Name = "";

                db.Topic.Add(topics);

                db.SaveChanges();


            }
        }

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
