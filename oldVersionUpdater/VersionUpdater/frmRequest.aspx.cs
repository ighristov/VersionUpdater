using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VersionUpdater
{
    public partial class frmRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Server.Transfer("HTMLRequestPage.htm");
                //PerformRequest2();
            }
        }
        
        private void PerformRequest()
        {
            AddHeader(Constants.C_APPNAME, "app.exe");
            AddHeader(Constants.C_VERSION, "3.2.1");
            Server.Transfer("Default.aspx");
        }

        private void AddHeader(string inHeaderName, string inHeaderValue)
        {
            NameValueCollection _headers = HttpContext.Current.Request.Headers;
            Type _type = _headers.GetType();
            PropertyInfo _propInfo = _type.GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                
            //PropertyInfo _propInfo = _type.GetProperty("readOnly", System.Reflection.BindingFlags.IgnoreCase);// | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.FlattenHierarchy);
            _propInfo.SetValue(_headers, false, null);
            if (string.IsNullOrWhiteSpace(_headers[inHeaderName]))
            {
                _headers.Add(inHeaderName, inHeaderValue);   
            }
            else
            {
                _headers[inHeaderName] = inHeaderValue;
            }
        }

        private void PerformRequest2()
        {
            string _url = Request.Url.AbsoluteUri.Replace(Request.Url.Segments[Request.Url.Segments.Length - 1], string.Empty) + "Default.aspx";
            Uri _uri = new Uri(_url);
            WebRequest _request = WebRequest.Create(_uri);
            _request.Headers.Add(Constants.C_APPNAME, "app.exe");
            _request.Headers.Add(Constants.C_VERSION, "1.2.3");
            WebResponse _response = _request.GetResponse();
            Debug.Write(_response.ContentLength);
        }
    }











}