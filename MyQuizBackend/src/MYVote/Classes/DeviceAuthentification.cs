using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MYVote.Models;
using Newtonsoft.Json;

namespace MyQuizBackend
{
    public class DeviceAuthentification
    {
        public static bool authenticateAdminDeviceByPassword(string inputPassword)
        {
            return inputPassword == Constants.adminPassword;
        }

        public static bool authenticateAdminDeviceByDeviceID(long deviceID)
        {
            using (var db = new EF_DB_Context())
            {
                var check = db.Device.FirstOrDefault(d => d.Id == deviceID);
                if (check != null && check.IsAdmin == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public static long getClientIDfromHeader(HttpRequest request)
        {
            long deviceID;
            try
            {
                return deviceID = Convert.ToInt64(request.Headers["DeviceID"].ToString());
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
