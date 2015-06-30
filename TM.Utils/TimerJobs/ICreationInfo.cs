using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Utils.TimerJobs
{
    public interface ICreationInfo
    {
        Type Type { get; set; }
        string Name { get; set; }
        string Title { get; set; }
        SPSchedule Schedule { get; set; }
    }
}
