using System;

namespace Knight.Core
{
    public static class TimeAssist
    {
        private static int mTimeZoneSecondsOffset;
        public static long ClientServerOffset { get; private set; } = 0;
        public static long TimeZoneEpoch { get; private set; } = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        public static DateTime TimeZone { get; private set; } = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public const long SecondsPerMinute = 60;
        public const long SecondsPerHour = 3600;
        public const long SecondsPerDay = 86400;
        public const long SecondsPer99Day = 8553600;
        public const long HoursPreDay = 24;
        public const long SecondsToTicks = 10000000;
        public const long MillisecondToTicks = 10000;
        public static int TimeZoneSecondsOffset
        {
            get
            {
                return mTimeZoneSecondsOffset;
            }
            set
            {
                mTimeZoneSecondsOffset = value;
                TimeZoneEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(mTimeZoneSecondsOffset).Ticks;
            }
        }
        /// <summary>
        /// 服务器时间(一百纳秒)
        /// </summary>
        /// <returns></returns>
        public static long ServerNowTicks()
        {
            return DateTime.UtcNow.Ticks - TimeZoneEpoch + ClientServerOffset;
        }
        /// <summary>
        /// 服务器时间(毫秒)
        /// </summary>
        /// <returns></returns>
        public static long ServerNow()
        {
            return (DateTime.UtcNow.Ticks - TimeZoneEpoch + ClientServerOffset) / MillisecondToTicks;
        }
        /// <summary>
        /// 服务器时间(秒)
        /// </summary>
        /// <returns></returns>
        public static long ServerNowSeconds()
        {
            return (DateTime.UtcNow.Ticks - TimeZoneEpoch + ClientServerOffset) / SecondsToTicks;
        }
        /// <summary>
        /// 计算客户端与服务器时间差值
        /// </summary>
        /// <param name="nServerTimeSeconds"></param>
        public static void CalcClientServerOffset(long nServerTimeSeconds)
        {
            var nClientTicks = DateTime.UtcNow.Ticks - TimeZoneEpoch;
            var nServerTicks = nServerTimeSeconds * SecondsToTicks;
            ClientServerOffset = nServerTicks - nClientTicks;
        }
        /// <summary>
        /// 登陆前是客户端时间,登陆后是同步过的服务器时间
        /// </summary>
        /// <returns></returns>
        public static long Now()
        {
            return ServerNow();
        }
        /// <summary>
        /// 客户端时间戳(一百纳秒)
        /// </summary>
        /// <returns></returns>
        public static long ClientNowTicks()
        {
            return DateTime.UtcNow.Ticks - TimeZoneEpoch;
        }
        /// <summary>
        /// 客户端时间戳(毫秒)
        /// </summary>
        /// <returns></returns>
        public static long ClientNow()
        {
            return (DateTime.UtcNow.Ticks - TimeZoneEpoch) / MillisecondToTicks;
        }
        /// <summary>
        /// 客户端时间戳(秒)
        /// </summary>
        /// <returns></returns>
        public static long ClientNowSeconds()
        {
            return (DateTime.UtcNow.Ticks - TimeZoneEpoch) / SecondsToTicks;
        }
        /// <summary>
        /// 秒时间戳转日期
        /// </summary>
        /// <returns></returns>
        public static DateTime SecondsTimeStampToDateTime(long rTimeStamp)
        {
            return TimeZone.AddSeconds(rTimeStamp).ToLocalTime();
        }
        public static DateTime TicksTimeStampToDateTime(long nTicks)
        {
            return TimeZone.AddTicks(nTicks).ToLocalTime();
        }
    }
}