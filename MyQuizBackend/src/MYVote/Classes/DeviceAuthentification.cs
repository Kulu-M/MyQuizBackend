using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQuizBackend
{
    public class DeviceAuthentification
    {
        public bool authenticateAdminDevice(int inputPassword)
        {
            return inputPassword == Constants.adminPassword;
        }

        public bool authenticateClientDevice(int deviceID)
        {
            //what we want to check here?
            return false;
        }

        
    }
}
