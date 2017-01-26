using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MYVote.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


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
        public IActionResult RegisterDevice([FromBody]JObject value)
        {
            var registration = JsonConvert.DeserializeObject<RegistrationDevice>(value.ToString());
            
            using (var db = new EF_DB_Context())
            {
                if (!string.IsNullOrWhiteSpace(registration.token))
                {
                    var check = db.Device.FirstOrDefault(d => d.PushUpToken == registration.token);
                    if (check != null)
                    {
                        return Ok(JsonConvert.SerializeObject(check));
                    }
                }

                var device = new Device {PushUpToken = registration.token};

                if (string.IsNullOrWhiteSpace(registration.password) || registration.password == Constants.adminPassword)
                {                        
                    device.IsAdmin = 1;
                }
                else
                {
                    device.IsAdmin = 0;
                }
                db.Device.Add(device);
                db.SaveChanges();
                return Ok(JsonConvert.SerializeObject(device));
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
