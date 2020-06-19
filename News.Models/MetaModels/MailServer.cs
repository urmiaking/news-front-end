using System;
using System.Collections.Generic;
using System.Text;

namespace News.Models.MetaModels
{
    public class MailServer
    {
        public int Id { get; set; }
        public string ServerType { get; set; }
        public string Email { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
    }
}
