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
        #region GET

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

        #endregion GET

        #region POST

        // POST api/devices
        [HttpPost]
        public IActionResult RegisterDevice([FromBody]JObject value)
        {
            var registration = JsonConvert.DeserializeObject<RegistrationDevice>(value.ToString());
            
            using (var db = new EF_DB_Context())
            {
                //Device already in DB
                if (!string.IsNullOrWhiteSpace(registration.token))
                {
                    var check = db.Device.FirstOrDefault(d => d.PushUpToken == registration.token);
                    if (check != null)
                    {
                        return Ok(JsonConvert.SerializeObject(check));
                    }
                }

                //Device not in DB
                var device = new Device();
                if (!string.IsNullOrWhiteSpace(registration.password) && registration.password == Constants.adminPassword)
                {
                    device.IsAdmin = 1;
                }
                else if (!string.IsNullOrWhiteSpace(registration.password) && registration.password != Constants.adminPassword)
                {
                    return Unauthorized();
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

        #endregion POST

        #region DELETE

        // DELETE api/devices/id
        [HttpDelete("{id}")]
        public void DeleteDevice(int id)
        {
            var deviceID = Convert.ToInt64(Request.Headers["DeviceID"].ToString());
            if (DeviceAuthentification.authenticateAdminDeviceByDeviceID(deviceID) == false) return;

            using (var db = new EF_DB_Context())
            {
                var check = db.Device.FirstOrDefault(d => d.Id == id);
                if (check == null) return;
                db.Device.Remove(check);
                db.SaveChanges();
            }
        }

        #endregion DELETE
    }
}
