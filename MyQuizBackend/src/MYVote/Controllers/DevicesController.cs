using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MYVote.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyQuizBackend.Controllers
{
    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        // GET: api/devices
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "devices", "controller" };
        }

        // GET api/devices/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/devices
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }
        [HttpPost("{id}/1")]
        public void Post(int id)
        {
            using (var db = new APIVoteDbContext())
            {
                var device = new Device();
                device.PushUpToken = "1234";
                db.Device.Add(device);
                db.SaveChanges();

            }
        }

        // PUT api/devices/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/devices/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
