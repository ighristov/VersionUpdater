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
            }
            else
            {
                lblSelectFile.Visible = false;
                lblEnterVersion.Visible = false;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            bool _dataOK = true;
            if (string.IsNullOrWhiteSpace(tbVersion.Text))
            {
                lblEnterVersion.Visible = true;
                lblEnterVersion.Focus();
                _dataOK = false;
            }
            if (string.IsNullOrWhiteSpace(FileUpload1.FileName))
            {
                lblSelectFile.Visible = true;
                FileUpload1.Focus();
                _dataOK = false;
            }
            if (!_dataOK) return;
            Version _version = Version.Parse(tbVersion.Text);
            FileManager.UploadFile_to_Storage(FileUpload1.FileBytes, FileUpload1.FileName, _version, string.Empty);
            gvApplications.DataBind();
        }

        protected void gvApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                if (!(e.CommandSource is LinkButton)) return;
                string _appName = e.CommandArgument.ToString();
                Read_and_Show_Versions(_appName);
            }
            else if (e.CommandName == "Download")
            {
                if (!(e.CommandSource is ImageButton)) return;
                GridViewRow _gridViewRow = (GridViewRow) ((e.CommandSource as ImageButton).Parent).Parent;
                string _appName = e.CommandArgument.ToString();
                var _dk = ((GridView)(_gridViewRow.Parent.Parent)).DataKeys[_gridViewRow.DataItemIndex];
                Version _ver = Version.Parse(_dk["Version"].ToString());
                ReqRespManager.DownloadFile(_appName, _ver);
            }
        }

        private void Read_and_Show_Versions(string inApplicationName)
        {
            lblVersionsCaption.Text = "Versions for application " + inApplicationName;
            var _verFiles = XMLHelper.GetVersions(inApplicationName);
            gvVersions.DataSource = _verFiles;
            gvVersions.DataBind();
        }

        protected void gvVersions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                if (!(e.CommandSource is LinkButton)) return;
                string _appName = e.CommandArgument.ToString();
                Read_and_Show_Versions(_appName);
            }
            else if (e.CommandName == "Download")
            {
                if (!(e.CommandSource is ImageButton)) return;
                GridViewRow _gridViewRow = (GridViewRow)((e.CommandSource as ImageButton).Parent).Parent;
                string _appName = e.CommandArgument.ToString();
                var _dk = ((GridView)(_gridViewRow.Parent.Parent)).DataKeys[_gridViewRow.DataItemIndex];
                Version _ver = Version.Parse(_dk["Version"].ToString());
                ReqRespManager.DownloadFile(_appName, _ver);
            }
        }
    }
}
