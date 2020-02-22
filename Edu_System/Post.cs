using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu_System
{
    class Post
    {
        public string content;
        public string student;
        public List<Post> reply = new List<Post>();
    }
}
