using CamlexNET;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TM.SP.AppPages.Pipeline;
using TM.SP.AppPages.Strategies;
using TM.SP.BCSModels.Taxi;
using TM.Utils;
using TM.Utils.TimerJobs;

namespace TM.SP.AppPages.Timers
{
    class LicenseRequestsAutoSender: JobDefinition
    {
        /// <summary>
        /// Максимальное количество м/в запросов без ответов, при котором недопустимо отправлять новые м/в запросы
        /// </summary>
        private static int _traceListThreshold;
        /// <summary>
        /// Максимальное количество запросов, которое можно отправить за один запуск
        /// </summary>
        private static int _maxRequestsToMake;

        /// <summary>
        /// Количество запросов, на которые не были получены ответы за последние lastDaysInterval дней
        /// </summary>
        /// <param name="web">Сайт с запросами</param>
        /// <param name="lastDaysInterval">Временное окно в днях</param>
        /// <returns></returns>
        private static int GetUnansweredCount(SPWeb web, int lastDaysInterval)
        {
            var list = web.GetListOrBreak("Lists/OutcomeRequestStateList");
            var expressions = new List<Expression<Func<SPListItem, bool>>>
            {
                x => (bool)x["Tm_AnswerReceived"] == false,
                x => (DateTime)x["Created"] > DateTime.Now.AddDays(-lastDaysInterval)
            };

            var items = list.GetItems(new SPQuery
            {
                Query = Camlex.Query().WhereAll(expressions).ToString(),
                ViewAttributes = "Scope='Recursive'"
            });

            return items.Count;
        }

        private static void ReadConfiguration(SPWeb web)
        {
            _traceListThreshold = Config.GetConfigValueOrDefault<int>(web, "LicenseAutoSender.NotAnsweredThreshold");
            _maxRequestsToMake = Config.GetConfigValueOrDefault<int>(web, "LicenseAutoSender.RequestsPerHour");
        }

        public LicenseRequestsAutoSender() { }

        public LicenseRequestsAutoSender(string jobName, string jobTitle, SPService service): base(jobName, jobTitle, service) { }

        public LicenseRequestsAutoSender(string jobName, string jobTitle, SPWebApplication webapp) : base(jobName, jobTitle, webapp) { }

        protected override void Work(SPWeb web)
        {
            ReadConfiguration(web);
            var unanswered = GetUnansweredCount(web, 15);
            if (unanswered < _traceListThreshold)
            {
                var worker = new BasePipelineWorker<License>(new LicenseRequestsPipeline(web),
                    new LicenseRequestAutoSendStrategy(web));
                worker.RunMultiple(_maxRequestsToMake, null, null);
            }
        }
    }
}
