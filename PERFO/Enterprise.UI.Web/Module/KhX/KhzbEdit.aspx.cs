using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Enterprise.UI.Web.Module.KhX
{
    public partial class KhzbEdit : System.Web.UI.Page
    {
        protected string zbid = "", mbid = "", type = "", bm = "", khdzbid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["zbid"] != null && Request["zbid"].ToString() != "")
            {
                zbid = Request["zbid"].ToString().Trim();
            }
            if (!string.IsNullOrEmpty(Request["mbid"]))
            {
                mbid = Request["mbid"].ToString().Trim();
            }
            if (!string.IsNullOrEmpty(Request["type"]))
            {
                type = Request["type"].ToString().Trim();
            }
            if (!string.IsNullOrEmpty(Request["bm"]))
            {
                bm = Request["bm"];
            }
            if (!string.IsNullOrEmpty(Request["khdzbid"]))
            {
                khdzbid = Request["khdzbid"];
            }
            if (mbid == "")
            {
                Response.Redirect("khjgview.aspx");
            }
        }
    }
}