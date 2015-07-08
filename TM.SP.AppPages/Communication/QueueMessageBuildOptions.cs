using System;

namespace TM.SP.AppPages.Communication
{
    public struct QueueMessageBuildOptions
    {
        public int Method;
        public DateTime Date;
        public Guid ServiceGuid;
    }
}
