using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VersionUpdaterSrv
{
    public partial class frmXMLStorage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Load_XMLFile();
            }
        }
        private void Load_XMLFile()
        {
            tbXML.Text = XMLHelper.XMLFilesDescr.ToString();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            bool _dataOK = true;
            if (string.IsNullOrWhiteSpace(FileUpload1.FileName))
            {
                lblSelectFile.Visible = true;
                FileUpload1.Focus();
                _dataOK = false;
            }
            if (!_dataOK) return;
            try
            {
                tbXML.Text = string.Empty;
                string _encodedStr = Convert.ToBase64String(FileUpload1.FileBytes);
                FileManager.Save_New_XMLFile(_encodedStr);
                XMLHelper.ReloadXMLFile();
                Load_XMLFile();
            }
            catch (Exception ex)
            {
                if (ex is IOException)
                {
                    
                }
                throw;
            }
        }
    }
}