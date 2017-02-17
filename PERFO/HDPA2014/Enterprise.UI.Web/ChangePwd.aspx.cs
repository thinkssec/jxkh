using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web
{
    /// <summary>
    /// 修改口令页面
    /// </summary>
    public partial class ChangePwd : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("Loginout.aspx");
            }
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            SysUserService userService = new SysUserService();
            SysUserModel userModel = userService.GetSingle(Page.User.Identity.Name);
            string errmsg = "";
            if (String.IsNullOrEmpty(TextBox1.Text.Trim()))
            {
                errmsg = "原密码不能为空!";
            }
            else
            {
                if (TextBox2.Text != TextBox3.Text)
                {
                    errmsg = "您输入的两次密码不一致!";
                }
                else
                {
                    if (userModel.PASSWORD != DESEncrypt.Encrypt(TextBox1.Text))
                    {
                        errmsg = "原密码不正确!";
                    }
                    else
                    {
                        userModel.PASSWORD = DESEncrypt.Encrypt(TextBox2.Text);
                        userModel.DB_Option_Action = WebKeys.UpdateAction;
                        userService.Execute(userModel);
                        //显示提示
                        ClientScript.RegisterStartupScript(GetType(), "msg", "<script>ShowInfo('提示','口令更新成功');</script>");
                    }
                }
            }
            Label1.Text = errmsg;
        }
    }
}