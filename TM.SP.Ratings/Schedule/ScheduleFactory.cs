using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace TM.SP.Ratings.Schedule
{
    public static class ScheduleFactory
    {
        public static SPMinuteSchedule GetMinute()
        {
            return new SPMinuteSchedule
            {
                BeginSecond = 0,
                EndSecond = 59,
                Interval = 1
            };
        }

        public static SPHourlySchedule GetHourly()
        {
            return new SPHourlySchedule
            {
                BeginMinute = 0,
                EndMinute = 59
            };
        }
    }
}
