using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VersionUpdaterSrv
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckForRequestHeaders();
            }
        }
        private void CheckForRequestHeaders()
        {
            string _appName = Request.Headers[Constants.C_APPNAME];
            string _appVersion = Request.Headers[Constants.C_APPVERSION];
            if ((string.IsNullOrWhiteSpace(_appName)) || (string.IsNullOrWhiteSpace(_appVersion))) return;
            Debug.WriteLine(_appName + "; " + _appVersion);
            CheckApplicationData(_appName, _appVersion);
        }
        
        private void CheckApplicationData(string inAppName, string inAppVersion)
        {
            var _version = XMLHelper.Get_Last_Version(inAppName);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var _version = XMLHelper.Get_Last_Version("SmartCardSignerApplet.jar");
            Debug.WriteLine(_version);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Version _version = Version.Parse(tbVersion.Text);
            FileManager.UploadFile_to_Storage(FileUpload1.FileBytes, FileUpload1.FileName, _version, string.Empty);
            gvApplications.DataBind();
        }

        protected void gvApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                if (!(e.CommandSource is ImageButton)) return;
                GridViewRow _gridViewRow = (GridViewRow) ((e.CommandSource as ImageButton).Parent).Parent;
                string _appName = e.CommandArgument.ToString();
                var _dk = gvApplications.DataKeys[_gridViewRow.DataItemIndex];
                Version _ver = Version.Parse(_dk["Version"].ToString());
                DownloadFile(_appName, _ver);
            }
        }

        protected void DownloadFile(string inApplicationName, Version inVersion)
        {
            byte[] _bytes = FileManager.ReadFile_from_Storage(inApplicationName, inVersion);
            FileManager.WriteFileToResponse(_bytes, inApplicationName);
        }
    }
}
