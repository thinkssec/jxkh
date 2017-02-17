using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Enterprise.UI.Web.Module.KhX
{
    public partial class KhZbLr : System.Web.UI.Page
    {
        protected string id = "";//模板ID
        protected string bm = "", khdzbid = "", khid = "",back="";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["mbid"] != null && Request["mbid"].ToString() != "")
                id = Request["mbid"].ToString();
            if (!string.IsNullOrEmpty(Request["back"]))
                back = Request["back"];
            if (!string.IsNullOrEmpty(Request["bm"]))
                bm = Request["bm"];
            if (!string.IsNullOrEmpty(Request["id"]))
                khdzbid = Request["id"];
            if (!string.IsNullOrEmpty(Request["khid"]))
                khid = Request["khid"];
            if (bm == "" || id == "")
            {
                Response.Redirect("khjgview.aspx");
            }
        }
    }
}