using Enterprise.Service.Perfo.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Enterprise.UI.Web.Module.KhX
{
    public partial class KhDwMb : System.Web.UI.Page
    {
        protected string khid = "", bm = "", khdzbid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["khid"]))
                khid = Request["khid"];
            if (!string.IsNullOrEmpty(Request["bm"]))
                bm = Request["bm"];
            if (!string.IsNullOrEmpty(Request["khdzbid"]))
                khdzbid = Request["khdzbid"];
            if (khid == "" || bm == "" || khdzbid == "")
            {
                Response.Redirect("KhjgView.aspx");
            }
        }
    }
}