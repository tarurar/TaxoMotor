using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SP.PlumsailFormsBackup.Timers
{
    public class BackupJob: SPJobDefinition
    {
        #region [fields]
        private static readonly string AllowBackupKeyName = "PlumsailFormsBackup_Allow";
        private static readonly string BackupPathKeyName = "PlumsailFormsBackup_Path";
        private static readonly string BackupSaveHistoryKeyName = "PlumsailFormsBackup_SaveHistory";
        private static readonly string[] FormTypes = { "New", "Display", "Edit" };
        public static readonly string SystemName = "PlumsailFormsBackup_TimerJob";
        #endregion

        #region [methods]
        public BackupJob() { }

        public BackupJob(string jobName, SPService service): base(jobName, service, null, SPJobLockType.None)
        {
            Title = StringsRes.BackupTimerTitle;
        }

        public BackupJob(string jobName, SPWebApplication webApp): base(jobName, webApp, null, SPJobLockType.Job)
        {
            Title = StringsRes.BackupTimerTitle;
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                var webApp = Parent as SPWebApplication;
                foreach (SPSite siteCollection in webApp.Sites)
                {
                    try
                    {
                        foreach (SPWeb web in siteCollection.AllWebs)
                        {
                            ExecuteJobForWeb(web);
                        }
                    }
                    finally
                    {
                        siteCollection.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format(StringsRes.BackupTimerCommonErrFmt, ex.Message));
            }
        }

        public static bool WebBooleanProperty(SPWeb web, string propName)
        {
            var prop = web.AllProperties[propName];
            var propValue = prop == null;
            if (!propValue)
            {
                propValue = Convert.ToBoolean(prop);
            }

            return propValue;
        }

        public static DirectoryInfo EnsureDirectory(string rootPath, params string[] children)
        {
            List<string> correctChildren = new List<string>();
            // remove illegal characters
            const string pattern = "~|\"|#|%|&|\\*|:|<|>|\\?|\\/|\\\\|{|\\||}|\\W*$|^\\W*";
            correctChildren.Add(rootPath);
            children.ToList().ForEach(childPath => {
                string correctPath = Regex.Replace(childPath, pattern, " ").Trim();
                correctChildren.Add(correctPath);
            });

            var newFolderPath = Path.Combine(correctChildren.ToArray());
            return Directory.CreateDirectory(newFolderPath);
        }

        private void ExecuteJobForWeb(SPWeb web)
        {
            if (!WebBooleanProperty(web, AllowBackupKeyName)) return;

            var backupPath = (string)web.AllProperties[BackupPathKeyName];
            if (String.IsNullOrEmpty(backupPath))
            {
                var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                backupPath = EnsureDirectory(appDataFolder, StringsRes.BackupTimerDefaultFolderName).FullName;
            }

            var webTitlePartName = String.Format("{0} _{1}_", web.Title, web.ID);
            foreach (SPList list in web.Lists)
            {
                var dateTimeTitlePart = DateTime.Now.ToString("ddMMyyyy_hhmm");

                foreach (SPContentType ct in list.ContentTypes)
                {
                    FormTypes.ToList().ForEach(ft => {
                        var keyName = String.Format(StringsRes.BackupTimerFormNameFmt, list.ID, ct.Id, ft);
                        var formContent = (string)web.AllProperties[keyName];

                        if (!String.IsNullOrEmpty(formContent))
                        {
                            var saveHistory = WebBooleanProperty(web, BackupSaveHistoryKeyName);
                            var listTitle = saveHistory ? String.Format("{0}_{1}", list.Title, dateTimeTitlePart) : list.Title;

                            var folderPath = EnsureDirectory(backupPath, new[] { webTitlePartName, listTitle, ct.Name }).FullName;
                            var fileName = String.Format("{0}_{1}.xfds", ct.Name, ft);
                            File.WriteAllText(Path.Combine(folderPath, fileName), formContent, Encoding.Unicode);
                        }
                    });
                }
            }
        }

        #endregion
    }
}
