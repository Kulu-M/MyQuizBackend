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
        #region Admin-Authentification

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

        #endregion Admin-Authentification

        #region Client-Authentification

        public static long getClientIDfromHeader(HttpRequest request)
        {
            try
            {
                return Convert.ToInt64(request.Headers["DeviceID"].ToString());
            }
            catch (Exception)
            {
                return -1;
            }
        }

        #endregion Client-Authentification
    }
}
