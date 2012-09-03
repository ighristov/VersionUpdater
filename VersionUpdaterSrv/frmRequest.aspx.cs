using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VersionUpdaterSrv
{
    public partial class frmRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) CreateRequest();
        }
        private void CreateRequest()
        {
            string _lastPageName = Request.Url.Segments[Request.Url.Segments.Length - 1];
            string _uri = Request.Url.AbsoluteUri.Replace(_lastPageName, "Default.aspx");
            WebRequest _request = WebRequest.Create(_uri);
            _request.Headers.Add(Constants.C_APPNAME, "application.exe");
            _request.Headers.Add(Constants.C_APPVERSION, "123.45");
            var _response = _request.GetResponse();
            Response.Write(_response.ContentLength.ToString());
        }
    }
}