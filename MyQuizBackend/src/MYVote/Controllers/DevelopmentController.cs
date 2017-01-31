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
    public class DevelopmentController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            List<SingleTopic> singleTopicList = new List<SingleTopic>();

            var singleTopic = new SingleTopic();
            singleTopic.String = "Lloyd";

            var singleTopic2 = new SingleTopic();
            singleTopic2.String = "Marius";

            singleTopicList.Add(singleTopic);
            singleTopicList.Add(singleTopic2);

            return Ok(JsonConvert.SerializeObject(singleTopicList));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
