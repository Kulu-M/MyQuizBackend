using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQuizBackend
{
    public class DeviceAuthentification
    {
        public bool authenticateAdminDevice(string inputPassword)
        {
            return inputPassword == Constants.adminPassword;
        }
    }
}
