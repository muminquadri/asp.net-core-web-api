using CityInfo.Api.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Services
{
    public class CloudMailService:IMailService
    {
        public string _mailTo = "admin@mycompany.com";
        public string _mailFrom = "noreply@mycompany.com";

        public void Send(string subject, string message)
        {
            //send mail o/p to debug window
            Debug.WriteLine($"Mail sent from {_mailFrom} to {_mailTo}, with CloudMailService");
            Debug.WriteLine($"Subject:{subject}");
            Debug.WriteLine($"Message:{message}");
        }
    }
}
