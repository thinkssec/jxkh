using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Enterprise.UI.Web
{
    public partial class Loginout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                System.Web.Security.FormsAuthentication.SignOut();
                Session.Clear();
                Session.Abandon();
                //Response.Write("<script>window.navigate('/login.aspx');</script>");
                Response.Redirect("~/Login.aspx");
                Response.End();
            }
        }
    }
}