using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VersionUpdaterSrv
{
    public partial class CheckVersion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReqRespManager.ProcessRequest_MakeResponse();
            }
        }
    }
}