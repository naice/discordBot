using System;
using System.Collections.Generic;
using System.Text;

namespace SmiteAPI.Model
{
    public class Session : ResponseBase
    {
        public string session_id { get; set; }
        public string timestamp { get; set; }
    }
}
