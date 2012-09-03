using System;
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
            //if (!IsPostBack) CreateRequest();
        }

        private void CreateRequest()
        {
            string _lastPageName = Request.Url.Segments[Request.Url.Segments.Length - 1];
            string _uri = Request.Url.AbsoluteUri.Replace(_lastPageName, "CheckVersion.aspx");
            WebRequest _request = WebRequest.Create(_uri);
            _request.Headers.Add(Constants.C_APPNAME, tbAppName.Text.Trim());
            _request.Headers.Add(Constants.C_APPVERSION, tbVersion.Text.Trim());
            if (chbForce.Checked)
            {
                _request.Headers.Add(Constants.C_FORCEDOWNLOAD, string.Empty);
            }
            int totalBytesRead = 0;
            string _downloadedVersion = string.Empty;
            string _filePath = Path.Combine(tbSavePath.Text, tbAppName.Text);
            using (var _httpResponse = _request.GetResponse())
            {
                int _errorCode = (_httpResponse.Headers.AllKeys.Contains(Constants.C_ERRORCODE))
                                     ? int.Parse(_httpResponse.Headers[Constants.C_ERRORCODE])
                                     : -1;
                if (_errorCode < 0)
                {
                    lblResult.Text = "Headers does not contain key \"" + Constants.C_ERRORCODE + "\"";
                    return;
                }
                else if (_errorCode > 0)
                {
                    string _errorMsg = (_httpResponse.Headers.AllKeys.Contains(Constants.C_ERRORMSG))
                                           ? _httpResponse.Headers[Constants.C_ERRORMSG]
                                           : "Headers does not contain key \"" + Constants.C_ERRORMSG + "\"";
                    if (!string.IsNullOrWhiteSpace(_errorMsg))
                    {
                        lblResult.Text = string.Format("ErrorCode: {0};      ErrorMessage: {1}", _errorCode, _errorMsg);
                        return;
                    }
                }
                else
                {
                    _downloadedVersion = _httpResponse.Headers[Constants.C_APPVERSION];
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
                lblResult.Text = "Резултат: Свален файл " + _filePath + " (" + totalBytesRead + "b.)";
                if (!string.IsNullOrWhiteSpace(_downloadedVersion))
                {
                    lblResult.Text += "; версия " + _downloadedVersion;
                }
            }
        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            CreateRequest();
        }
    }
}