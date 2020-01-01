using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace DoggoWire.Services
{
    public static class Helpers
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ConvertEpoch(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }
    }
}
