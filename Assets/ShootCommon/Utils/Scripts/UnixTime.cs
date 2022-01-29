using System;

namespace Packages.Utils.Scripts
{
    public static class UnixTime
    {
        public static int ConvertTime(DateTime time)
        {
            int unixTime = (int)(time - new DateTime(1970, 1, 1)).TotalSeconds;
            return unixTime;
        }
        
        public static int GetDays(DateTime time)
        {
            int unixTime = (int)(time - new DateTime(1970, 1, 1)).TotalDays;
            return unixTime;
        }
    }
}