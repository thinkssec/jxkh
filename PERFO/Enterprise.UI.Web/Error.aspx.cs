using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Enterprise.UI.Web
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rmsg = Request["msg"];
            if (!string.IsNullOrEmpty(rmsg))
            {
                errmsg.Text = rmsg;
            }
            else
            {
                errmsg.Text = "未知的错误";
            }
        }
    }
}
