using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu_System
{
    class Email
    {
        public string content;
        public string from;
        public string to;
        public List<Email> reply = new List<Email>();
    }
}
