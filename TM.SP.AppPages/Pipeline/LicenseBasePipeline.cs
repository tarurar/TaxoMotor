using System;
using Microsoft.SharePoint;
using TM.SP.BCSModels.Taxi;

namespace TM.SP.AppPages.Pipeline
{
    public class LicenseBasePipeline: IPipeline<License>
    {
        public virtual License GetNext()
        {
            throw new NotImplementedException();
        }

        public virtual void PutBack(License element)
        {
            throw new NotImplementedException();
        }
    }
}
