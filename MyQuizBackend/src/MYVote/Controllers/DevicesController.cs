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
            RegistrationDevice registration;

            if (value == null) return BadRequest();

            try
            {
                registration = JsonConvert.DeserializeObject<RegistrationDevice>(value.ToString());
            }
            catch (Exception)
            {
                return BadRequest("Cannot deserialize your input!");
            }
            
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
                device.PushUpToken = registration.token;
                db.Device.Add(device);
                db.SaveChanges();
                return Ok(JsonConvert.SerializeObject(device));
            }
        }

        // POST api/devices
        [HttpPost("{id}/groups")]
        public IActionResult DeviceEnterGroup([FromBody]JObject value)
        {
            var deviceID = DeviceAuthentification.getClientIDfromHeader(Request);
            
            Group postedGroup; 

            if (value == null) return BadRequest();

            try
            {
                postedGroup = JsonConvert.DeserializeObject<Group>(value.ToString());
            }
            catch (Exception)
            {
                return BadRequest("Cannot deserialize your input!");
            }

            using (var db = new EF_DB_Context())
            {
                var deviceGroup = new DeviceGroup();
                var checkGroup = db.Group.FirstOrDefault(g => g.EnterGroupPin == postedGroup.EnterGroupPin);
                var checkDevice = db.Device.FirstOrDefault(d => d.Id == deviceID);
                if (checkGroup != null && checkDevice != null)
                {
                    if (postedGroup.EnterGroupPin == checkGroup.EnterGroupPin)
                    {
                        deviceGroup.DeviceId = deviceID;
                        deviceGroup.GroupId = checkGroup.Id;
                        db.DeviceGroup.Add(deviceGroup);
                        db.SaveChanges();
                    }
                }
                else if (checkGroup == null)
                {
                    return BadRequest("Group does not exist!");
                }
                else if (checkDevice == null)
                {
                    return BadRequest("Device is not registrated please sign up first!");
                }
                return Ok(JsonConvert.SerializeObject(deviceGroup));
            }
        }

        #endregion POST

        #region DELETE

        // DELETE api/devices/id
        [HttpDelete("{id}")]
        public IActionResult DeleteDevice(int id)
        {
            var deviceID = DeviceAuthentification.getClientIDfromHeader(Request);
            if (deviceID < 0 || DeviceAuthentification.authenticateAdminDeviceByDeviceID(deviceID) == false) return Unauthorized();

            using (var db = new EF_DB_Context())
            {
                var check = db.Device.FirstOrDefault(d => d.Id == id);
                if (check == null) return BadRequest("Id not found!");
                db.Device.Remove(check);
                db.SaveChanges();
                return Ok(JsonConvert.SerializeObject(check));
            }
        }

        #endregion DELETE
    }
}
