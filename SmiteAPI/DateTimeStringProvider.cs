using System;
using System.Collections.Generic;
using System.Text;

namespace SmiteAPI
{
    internal class DateTimeStringProvider
    {
        public static string SmiteUtcNow => DateTime.UtcNow.ToString("yyyyMMddHHmmss");
    }
}
