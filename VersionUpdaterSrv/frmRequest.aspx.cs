﻿using System;
using System.Collections.Generic;
using System.IO;
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
            if (!IsPostBack)
            {
                PrepareForm();
            }
        }

        private void PrepareForm()
        {
            string _lastPageName = Request.Url.Segments[Request.Url.Segments.Length - 1];
            tbURL.Text = Request.Url.AbsoluteUri.Replace(_lastPageName, "CheckVersion.aspx");
            tbAppName.Text = string.Empty;
            tbVersion.Text = "1.0.0.0";
            tbSavePath.Text = @"c:\";
        }
        public static void CreateRequest(string inURL, string inAppName, string inAppVersion, string inSavePath, out string outResult)
        {
            WebRequest _request = WebRequest.Create(inURL);
            _request.Headers.Add(Constants.C_APPNAME, inAppName);
            _request.Headers.Add(Constants.C_APPVERSION, inAppVersion);
            int totalBytesRead = 0;
            string _filePath = Path.Combine(inSavePath, inAppName);
            using (var _httpResponse = _request.GetResponse())
            {
                int _errorCode = (_httpResponse.Headers.AllKeys.Contains(Constants.C_ERRORCODE))
                                     ? int.Parse(_httpResponse.Headers[Constants.C_ERRORCODE])
                                     : -1;
                if (_errorCode < 0)
                {
                    outResult = "Headers does not contain key \"" + Constants.C_ERRORCODE + "\"";
                    return;
                }
                else if (_errorCode > 0)
                {
                    string _errorMsg = (_httpResponse.Headers.AllKeys.Contains(Constants.C_ERRORMSG))
                                           ? _httpResponse.Headers[Constants.C_ERRORMSG]
                                           : "Headers does not contain key \"" + Constants.C_ERRORMSG + "\"";
                    if (!string.IsNullOrWhiteSpace(_errorMsg))
                    {
                        outResult = string.Format("ErrorCode: {0};      ErrorMessage: {1}", _errorCode, _errorMsg);
                        return;
                    }
                }
                else
                {
                    using (Stream responseStream = _httpResponse.GetResponseStream())
                    {
                        using (FileStream _localFileStream = new FileStream(_filePath, FileMode.Create))
                        {
                            byte[] buffer = new byte[255];
                            int bytesRead;
                            while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                totalBytesRead += bytesRead;
                                _localFileStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }
                outResult = "Резултат: Свален файл " + _filePath + " (" + totalBytesRead + "b.)";
            }
        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            string _result = string.Empty;
            CreateRequest(tbURL.Text, tbAppName.Text, tbVersion.Text, tbSavePath.Text, out _result);
            lblResult.Text = _result;
        }
    }
}