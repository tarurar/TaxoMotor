using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using CamlexNET;
using Microsoft.SharePoint.Utilities;

namespace TM.Utils
{
    public static class Calendar
    {
        #region [fields]
        private static readonly string CalendarUrlConfigKey = "ProductionCalendarListUrl";
        #endregion
      
        #region [methods]
        /// <summary>
        /// Calculates finish date for the start date and work days needed.
        /// Supports only simple events/events for the whole day.
        /// Doesn't support recurrent events.
        /// </summary>
        /// <param name="web">SPWeb where calendar list located</param>
        /// <param name="startDate">Date to cope with</param>
        /// <param name="workDaysCount">The number of work days need to be added to the start date</param>
        /// <returns>Finish date</returns>
        public static DateTime CalcFinishDate(SPWeb web, DateTime startDate, int workDaysCount)
        {
            var calendarUrl = Config.GetConfigValue(Config.GetConfigItem(web, CalendarUrlConfigKey)).ToString();
            var calendarList = web.GetListOrBreak(calendarUrl);

            int countedDays = 0;
            DateTime dateToValidate = startDate.Date;
            while (countedDays < workDaysCount)
            {
                var camlDate = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dateToValidate);
                var weekdayCaml = Camlex.Query().Where(x =>
                    x["EventDate"] <= ((DataTypes.DateTime)camlDate).IncludeTimeValue()
                    && x["EndDate"] >= ((DataTypes.DateTime)camlDate).IncludeTimeValue()
                    && (string)x["Category"] == StringsRes.CalendarHoliday).ToString();
                var holidayCaml = Camlex.Query().Where(x =>
                    x["EventDate"] <= ((DataTypes.DateTime)camlDate).IncludeTimeValue()
                    && x["EndDate"] >= ((DataTypes.DateTime)camlDate).IncludeTimeValue()
                    && (string)x["Category"] == StringsRes.CalendarWorkHours).ToString();

                var isWeekend = dateToValidate.DayOfWeek == DayOfWeek.Saturday || dateToValidate.DayOfWeek == DayOfWeek.Sunday;
                var caml = isWeekend ? holidayCaml : weekdayCaml;
                var items = calendarList.GetItems(new SPQuery { Query = caml });

                if (isWeekend && items.Count > 0) {
                    countedDays++;
                }
                if (!isWeekend && items.Count == 0) {
                    countedDays++;
                }

                dateToValidate = dateToValidate.AddDays(1);
            }

            return dateToValidate.AddDays(-1);
        }
        #endregion
    }
}
