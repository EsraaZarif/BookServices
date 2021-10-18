using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.ModelView
{
    public class ResponseModelView
    {
        public List<SessionsScheduled> Sessions { get; set; }
        public class SessionsScheduled
        {
            public int ChapterNumber { get; set; }
            public List<string> SessionsDate{ get; set; }
        }
    }
}
