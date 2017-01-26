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
        public IActionResult RegisterDevice([FromBody]string value)
        {
            var registration = JsonConvert.DeserializeObject<RegistrationDevice>(value.ToString());
            var device = new Device();
            using (var db = new EF_DB_Context())
            {
                var existingDevice = registration.deviceID;
                var token = registration.token;
                var password = registration.password;

                var check = db.Device.First(d => d.Id == existingDevice).ToString();
                if (check != null)
                {
                    return Ok(JsonConvert.SerializeObject(device));
                }
                else
                {
                    device.Id = existingDevice;
                    device.PushUpToken = token;

                    if (password == Constants.adminPassword)
                    {                        
                        device.IsAdmin = "true";
                    }
                    else
                    {
                        device.IsAdmin = "false";
                    }
                    db.Device.Add(device);
                    db.SaveChanges();
                    return Ok(JsonConvert.SerializeObject(device));
                }
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
