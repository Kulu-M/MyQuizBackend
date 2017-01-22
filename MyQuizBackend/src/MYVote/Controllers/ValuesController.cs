using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MYVote.Models;

namespace MYVote.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            using (var db = new APIVoteDbContext())
            {
                var device = new Device();
                device.Id = 1;
                device.PushUpToken = "First Device";
                db.Device.Add(device);

                var group = new Group();
                group.Id = 1;
                group.Title = "First Title Test";
                group.Topic = 1;
                group.EnterGroupPin = 12345;
                db.Group.Add(group);

                var deviceGroup = new DeviceGroup();
                deviceGroup.DeviceId = device.Id;
                deviceGroup.GroupId = group.Id;
                db.DeviceGroup.Add(deviceGroup);
                
                db.SaveChanges();

            }
            return new string[] { "value1", "value2" };
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
