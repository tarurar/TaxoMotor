namespace TM.SP.AppPages.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;

    #region [interfaces]
    public interface IValidator
    {
        bool Execute(params object[] paramsList);
    }
    #endregion

    #region [classes]
    public class Validator : IValidator
    {
        #region [fields]
        protected SPWeb _web;
        #endregion

        #region [methods]
        protected Validator() { }
        public Validator(SPWeb web)
        {
            _web = web;
        }

        public virtual bool Execute(params object[] paramsList) 
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    #endregion
}
