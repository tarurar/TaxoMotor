using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)] 
    public class SharepointFieldAttribute: Attribute
    {
        public string Name { get; private set; }

        public SharepointFieldAttribute(string name)
        {
            this.Name = name;
        }
    }
}
