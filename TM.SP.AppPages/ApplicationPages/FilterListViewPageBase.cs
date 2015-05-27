using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.AppPages.ApplicationPages
{
    public class FilterListViewPageBase: LayoutsPageBase
    {
        protected virtual bool ValidateParams()
        {
            throw new NotImplementedException();
        }

        protected virtual void LoadData()
        {
            throw new NotImplementedException();
        }

        protected virtual void InvalidParamsError()
        {
            throw new NotImplementedException();
        }
        protected override void OnLoad(EventArgs e)
        {
            if (ValidateParams())
            {
                LoadData();
            }
            else
            {
                InvalidParamsError();
            }
        }
    }
}
