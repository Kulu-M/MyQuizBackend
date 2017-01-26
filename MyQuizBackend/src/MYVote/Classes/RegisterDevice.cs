using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYVote.Models
{
    public class RegistrationDevice
    {
        public string token;
        public int deviceID;
        public string password;

        public RegistrationDevice(string token, int deviceID, string password)
        {
            this.token = token;
            this.deviceID = deviceID;
            this.password = password;
        }
    }
}