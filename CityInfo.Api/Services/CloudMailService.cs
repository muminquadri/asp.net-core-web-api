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
        public string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        public string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send(string subject, string message)
        {
            //send mail o/p to debug window
            Debug.WriteLine($"Mail sent from {_mailFrom} to {_mailTo}, with CloudMailService");
            Debug.WriteLine($"Subject:{subject}");
            Debug.WriteLine($"Message:{message}");
        }
    }
}
