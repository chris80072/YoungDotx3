using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungDotx3.Domain.Hallelujah
{
    public class Message
    {
        public string Id { get; set; } = string.Empty;

        private string _nickname = string.Empty;
        public string Nickname
        {
            get { return _nickname; }
            set { _nickname = value.Length > 20 ? value.Substring(0, 20) : value; }
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return _content; }
            set { _content = value.Length > 500 ? value.Substring(0, 500) : value; }
        }
        public DateTime CreateDate { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public bool IsDelete { get; set; } = false;

        public void SetIpAddress(string ip)
        {
            IpAddress = ip;
        }
    }
}
