using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Enterprise.UI.Web.Module.KhX
{
    public partial class KhZbMb : System.Web.UI.Page
    {
        protected string khid = "", bm = "", id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["khid"]))
            {
                khid = Request["khid"];
            }
            if (!string.IsNullOrEmpty(Request["bm"]))
            {
                bm = Request["bm"];
            }
            if (!string.IsNullOrEmpty(Request["id"]))
                id = Request["id"];
            if (khid == "" || bm == "" || id == "")
            {
                Response.Redirect("KhjgView.aspx");
            }
        }
    }
}