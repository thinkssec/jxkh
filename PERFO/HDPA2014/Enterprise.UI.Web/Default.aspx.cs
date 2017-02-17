using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Component.Infrastructure;
using System.Text.RegularExpressions;
using System.Text;

namespace Enterprise.UI.Web
{
    /// <summary>
    /// 首页面
    /// </summary>
    public partial class Default : PageBase
    {

        /// <summary>
        /// 加载的页面路径
        /// </summary>
        protected string url = string.Empty;
        /// <summary>
        /// 用户的菜单风格
        /// </summary>
        protected string menu = (string)Utility.sink("menu", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//菜单风格

        /// <summary>
        /// 反向显示用户菜单名称
        /// </summary>
        public string ReverseUMenu
        {
            get
            {
                return (userModel.MENU == "1") ? 
                    WebKeys.MenuType.折叠菜单.ToString() : WebKeys.MenuType.树型菜单.ToString();
            }
        }

        /// <summary>
        /// 反向显示用户菜单值
        /// </summary>
        public string ReverseUMenuValue
        {
            get
            {
                return (userModel.MENU == "1") ? 
                    ((int)WebKeys.MenuType.折叠菜单).ToString() : ((int)WebKeys.MenuType.树型菜单).ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //1==用户信息
                showUserLoginInfo();
                //2==欢迎页加载路径
                url = "";
                if (userModel.ROLEID == "paadmin")
                {
                    url = "/Module/KaoheIndexNew.aspx";
                }
                else if (userModel.Bmjg.JGLX.Contains("职能"))
                {
                    url = "/Module/KaoheBumenIndexNew.aspx";
                }
                else if (userModel.Bmjg.JGLX.Contains("二级"))
                {
                    url = "/Module/KaoheDanweiIndexNew.aspx";
                }
                else if (userModel.DUTY == "油气田领导")
                {
                    url = "/Module/KaoheLeaderIndexNew.aspx";
                }
                //3==用户菜单风格
                if (menu == "1" || menu == "2")
                {
                    userModel.MENU = menu;
                    userModel.DB_Option_Action = WebKeys.UpdateAction;
                    userService.Execute(userModel);
                }

                //string str = "{投入资本收入贡献系数}={本年经营收入总额}*2/({期初实收资本}+{期初资本公积}+{期末实收资本}+{期末资本公积}+{期初补充流动资金}+{期末补充流动资金})*100";
                //string pattern = @"\{.*?\}";//匹配模式
                //Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                //MatchCollection matches = regex.Matches(str);
                //StringBuilder sb = new StringBuilder();//存放匹配结果
                //foreach (Match match in matches)
                //{
                //    sb.AppendLine(match.Value);
                //}
                //Response.Write(sb.ToString());
            }
        }

        /// <summary>
        /// 获取当前用户的身份信息
        /// </summary>
        /// <returns></returns>
        protected string GetUserInfo()
        {
            string userInfo = string.Empty;
            userInfo = string.Format("登录账号：{0}<br/>所属单位：{1}<br/>系统角色：{2}<br/>来路IP：{3}",
                userModel.LOGINID, userModel.Bmjg.JGMC, userModel.Role.ROLENAME, Utility.GetIPAddress());
            return userInfo;
        }

        /// <summary>
        /// 显示当前用户的最近一次登录信息
        /// </summary>
        private void showUserLoginInfo()
        {
            var q = visitService.GetLastVisitInfo(userModel.USERNAME);
            if (q != null && q.OPERATIONDATE != null)
            {
                Lbl_LastLogin.Text = q.OPERATIONDATE.Value.ToString("yyyy年MM月dd日 HH:mm:ss");
            }
        }

        /// <summary>
        /// 获取当前用户的菜单
        /// </summary>
        /// <returns></returns>
        protected string GetUserMenu()
        {
            StringBuilder sb = new StringBuilder();
            if (userModel.MENU == "1")
            {
                sb.Append("<ul id=\"treeUl\" class=\"easyui-tree\" animate=\"false\" dnd=\"false\">");
                sb.Append(SysUserService.LoadTreeMenu(userModel));
                sb.Append("</ul>");
            }
            else
            {
                sb.Append("<div id=\"accordionMenu\" class=\"easyui-accordion\" fit=\"true\" border=\"false\">");
                sb.Append(SysUserService.LoadAccordionMenu(userModel));
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        
    }
}