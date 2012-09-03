using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VersionUpdaterSrv
{
    public partial class frmErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (true)
            {
                ShowDetailedError();
            }
        }

        private void ShowDetailedError()
        {
            Exception _lastError = Server.GetLastError();
            if (_lastError == null)
            {
                lblMainError.Text = "Cannot get info about current error. Server.GetLastError() == null;";
                return;
            }
            else
            {
                lblMainError.Text = _lastError.ToString();
                Exception _baseEx = _lastError.GetBaseException();
                if (_baseEx != null)
                {
                    divBaseException.Visible = true;
                    lblBaseException.Text = _baseEx.ToString();
                }
                                Exception _innerEx = _lastError.InnerException;
                if (_innerEx != null)
                {
                    divInnerException.Visible = true;
                    lblInnerException.Text = _innerEx.ToString();
                }
            }
        }
    }
}