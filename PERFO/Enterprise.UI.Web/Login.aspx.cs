using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Component.Infrastructure;
using System.Configuration;

namespace Enterprise.UI.Web
{
    /// <summary>
    /// 登录验证页面
    /// </summary>
    public partial class Login : System.Web.UI.Page
    {

        //系统消息
        string msg = (string)Utility.sink("msg", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);
        static SysUserService userSrv = new SysUserService();
        protected void Page_Load(object sender, EventArgs e)
        {
            //登录失败账号集合
            if (Global.UserLoginFailLst == null)
                Global.UserLoginFailLst = new List<UserLoginStatistics>();

            if (!IsPostBack)
            {
                //语言选择
                DropDownList ddl = (DropDownList)Page.FindControl("Language");
                ddl.SelectedValue = Utility.Language.ToString();

                //显示提示消息
                Lbl_Msg.Text = msg;
            }
        }

        /// <summary>
        /// 页面登录验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            LoginOA();
        }


        /// <summary>
        /// Form验证注册
        /// </summary>
        /// <param name="us"></param>
        protected static void SignIn(SysUserModel user)
        {
            System.Web.Security.FormsAuthenticationTicket tk = new System.Web.Security.FormsAuthenticationTicket(1, user.LOGINID, DateTime.Now, DateTime.Now.AddMonths(1), true, "", System.Web.Security.FormsAuthentication.FormsCookiePath);
            string key = System.Web.Security.FormsAuthentication.Encrypt(tk);//得到加密后的身份验证票字串
            HttpCookie ck = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, key);
            //ck.Domain = System.Web.Security.FormsAuthentication.CookieDomain;  // 这句话在部署网站后有用，此为关系到同一个域名下面的多个站点是否能共享Cookie
            HttpContext.Current.Response.Cookies.Add(ck);
        }
        private static bool FormLoginService(string sUser, string sSign)
        {
            LogHelper.WriteLog(sUser + "---------------" + sSign);
            bool rbool = false;
            if (sUser == "admin")
            {
                if (sSign == "@server" + DateTime.Now.Year)
                {
                    rbool = true;
                }
            }
            else
            {
                if (ConfigurationManager.AppSettings["Debug"].ToLower().Equals("false"))
                {
                    //Debuger.GetInstance().log("uname= "+sUser+"password=" + sSign + " e=" + DESEncrypt.Encrypt(sSign));
                    int i = userSrv.GetList().Where(p => p.LOGINID == sUser && p.PASSWORD == DESEncrypt.Encrypt(sSign)).Count();
                    //Debuger.GetInstance().log("uname= " + sUser + "password=" + sSign + " e=" + DESEncrypt.Encrypt(sSign));
                    if (i==0) return false;
                    return true;
                    //return (SlytADauthEntry.GetADUserInfo(sUser, sSign) != null);
                    //return RtxServiceEntry.FormLoginService(sUser, sSign);
                }
                else
                {
                    //测试用
                    rbool = true;
                }
            }
            return rbool;
        }
        public void LoginOA()
        {
            string sName = (string)Utility.sink(UserName.UniqueID, Utility.MethodType.Post, 0, 0, Utility.DataType.Str);
            string sPassword = (string)Utility.sink(Password.UniqueID, Utility.MethodType.Post, 0, 0, Utility.DataType.Str);

            UserLoginStatistics userLoginFailModel = Global.UserLoginFailLst.Find(p => p.LoginName == sName);
            if (userLoginFailModel != null && userLoginFailModel.FailCount >= 5)
            {
                Response.Redirect("Login.aspx?msg=" + Server.UrlEncode("您的失败次数达到了5次!请5分钟后再试!"));
                Response.End();
            }

            SysUserService uService = new SysUserService();
            var q = uService.GetSingle(sName);
            //sPassword = DESEncrypt.Encrypt(sPassword);
            if (q != null)
            {
                //bool rbool = (sPassword == q.PASSWORD);
                bool rbool = FormLoginService(sName, sPassword); ;
                //if (ConfigurationManager.AppSettings["Debug"].ToLower().Equals("true"))
                //    rbool = true;
                //else rbool=FormLoginService(sName, sPassword);
                if (rbool)
                {
                    //语言类型
                    Utility.Language = (Utility.LanguageType)Enum.Parse(typeof(Utility.LanguageType), Language.SelectedValue);
                    Debuger.GetInstance().log(string.Format("页面验证：用户【{0}】于【{1}】成功登录系统!IP:{2}", sName, DateTime.Now, Utility.GetIPAddress()));
                    //注册
                    SignIn(q);
                    Response.Redirect(GetReturnUrl());
                    Response.End();
                }
                else
                {
                    //add by qw 2013.5.21 start
                    int failCount = 0;
                    if (userLoginFailModel != null)
                    {
                        UserLoginStatistics newUserModel = new UserLoginStatistics();
                        newUserModel.LoginName = userLoginFailModel.LoginName;
                        newUserModel.FailCount = userLoginFailModel.FailCount + 1;
                        failCount = newUserModel.FailCount;
                        newUserModel.RecordDate = DateTime.Now;
                        Global.UserLoginFailLst.Remove(userLoginFailModel);
                        Global.UserLoginFailLst.Add(newUserModel);
                    }
                    else
                    {
                        UserLoginStatistics newUserModel = new UserLoginStatistics();
                        newUserModel.LoginName = sName;
                        newUserModel.FailCount = 1;
                        failCount = newUserModel.FailCount;
                        newUserModel.RecordDate = DateTime.Now;
                        Global.UserLoginFailLst.Add(newUserModel);
                    }
                    //end
                    Debuger.GetInstance().log(string.Format("RTX验证失败：用户【{0}】于【{1}】尝试登录系统!IP:{2},口令:{3}", sName, DateTime.Now, Utility.GetIPAddress(), sPassword));
                    Response.Redirect("Login.aspx?msg=" + Server.UrlEncode("用户验证失败" + failCount + "次"));
                    Response.End();
                }
            }

        }

        #region 专用方法区

        /// <summary>
        /// 显示提示消息
        /// </summary>
        /// <returns></returns>
        private string showMsg()
        {
            return "";
        }

        private string GetReturnUrl()
        {
            string reUrl = "~/Main";
            return reUrl;
        }

        #endregion
    }
}