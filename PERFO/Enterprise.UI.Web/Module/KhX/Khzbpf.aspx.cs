using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Enterprise.UI.Web.Module.KhX
{
    public partial class Khzbpf : System.Web.UI.Page
    {
        protected string khid = "", mbjgid = "",back="",audit="",dwdm="";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["khid"] != null)
                khid = Request["khid"].ToString();
            if (Request["audit"] != null)
                audit = Request["audit"].ToString();
            if (Request["back"] != null)
                back = Request["back"].ToString();
            if (Request["dwdm"] != null)
                dwdm = Request["dwdm"].ToString();
            if (!string.IsNullOrEmpty(Request["mbjgid"]))
                mbjgid = Request["mbjgid"];
            if ((khid == "" || mbjgid == "") && HttpContext.Current.Request.UrlReferrer != null)
            {
                Response.Redirect(HttpContext.Current.Request.UrlReferrer.ToString());
            }
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            
        }
    }
}