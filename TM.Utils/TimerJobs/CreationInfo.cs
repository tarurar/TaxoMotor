using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Utils.TimerJobs
{
    public struct CreationInfo: ICreationInfo
    {
        private Type type;
        private string name;
        private string title;
        private SPSchedule schedule;

        public Type Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public Microsoft.SharePoint.SPSchedule Schedule
        {
            get
            {
                return schedule;
            }
            set
            {
                schedule = value;
            }
        }
    }

}
