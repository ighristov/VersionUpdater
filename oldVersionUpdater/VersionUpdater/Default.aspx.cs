using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VersionUpdater
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Check_for_RequestParams();
            }
        }
        private void Check_for_RequestParams()
        {
            var _headers =
                Request.Headers.AllKeys.Where(k => (k.Equals(Constants.C_APPNAME, StringComparison.OrdinalIgnoreCase))
                                                   || (k.Equals(Constants.C_VERSION, StringComparison.OrdinalIgnoreCase))).ToList();
            if (_headers.Count() != 2) return;
            string _appName = Request.Headers[Constants.C_APPNAME];
            string _version = Request.Headers[Constants.C_VERSION];
            Debug.WriteLine(_appName + "; " + _version);
        }
    }
}
