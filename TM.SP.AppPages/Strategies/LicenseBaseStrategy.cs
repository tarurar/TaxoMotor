using System;
using TM.SP.BCSModels.Taxi;

namespace TM.SP.AppPages.Strategies
{
    public class LicenseBaseStrategy: IPipelineStrategy<License>
    {
        public virtual void Handle(License element)
        {
            throw new NotImplementedException();
        }
    }
}
