using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.Permissions;
using System.Data;
using System.Configuration;
using Enterprise.Component.Infrastructure;

namespace Enterprise.Service.Perfo.Sys
{
    /// <summary>
    /// 文件名:  SlytADauthEntry.cs
    /// 功能描述: 统一调用胜利油田AD用户验证访问接口的服务类
    /// 创建人：qw
    /// 创建日期: 2015.3.31
    /// </summary>
    public sealed class SlytADauthEntry
    {

        /// <summary>
        /// 获取AD用户的信息
        /// </summary>
        /// <param name="uName">用户名</param>
        /// <param name="uPassword">口令</param>
        /// <returns></returns>
        public static ADUserInfo GetADUserInfo(string uName, string uPassword)
        {
            ADUserInfo userInfo = null;
            //uName = "sundeying391.slyt";
            //uPassword = "sundeying1981";
            string ldapUrl = ConfigurationManager.AppSettings["SlytLDAPUrl"];
            DirectoryEntry adRoot = new DirectoryEntry(ldapUrl, uName + ".trqi@sinopec.com", uPassword, AuthenticationTypes.None);
            DirectorySearcher mySearcher = new DirectorySearcher(adRoot);
            //mySearcher.Filter = ("(objectClass=user)"); //user表示用户，group表示组
            mySearcher.Filter = "(&(objectClass=user)(CN=" + uName + "))";
            SearchResultCollection searchResults = null;
            try
            {
                searchResults = mySearcher.FindAll();
                userInfo = new ADUserInfo();
                foreach (System.DirectoryServices.SearchResult resEnt in searchResults)
                {
                    DirectoryEntry user = resEnt.GetDirectoryEntry();
                    //foreach (string property in user.Properties.PropertyNames)
                    //{
                    //    Response.Write("字段名: " + property + "====" + user.Properties[property][0].ToString() + "<br/>");
                    //}
                    if (user.Properties.Contains("displayName"))
                    {
                        //用户姓名
                        userInfo.UserName = user.Properties["displayName"][0].ToString();
                    }
                    if (user.Properties.Contains("Name"))
                    {
                        //用户账号
                        userInfo.LoginName = user.Properties["Name"][0].ToString();
                        //user.Properties.Contains("sAMAccountName") //也能取到
                    }
                    if (user.Properties.Contains("mail"))
                    {
                        //邮箱
                        userInfo.Email = user.Properties["mail"][0].ToString();
                    }
                    if (user.Parent.Name != string.Empty && user.Parent.Name.IndexOf('=') > -1)
                    {
                        //获取用户所在的组织单位
                        userInfo.DeptName = user.Parent.Name.Split('=')[1];
                        //user.Properties.Contains("department") //也能取到
                    }
                    if (user.Properties.Contains("company"))
                    {
                        //中石化分公司
                        userInfo.Company = user.Properties["company"][0].ToString();
                    }
                }
                Debuger.GetInstance().log(string.Format("账户{0},{1},AD身份验证通过!", userInfo.LoginName, userInfo.UserName));
            }
            catch (Exception ex) 
            {
                string msg = string.Format("LDAP={0}|UN={1}|PWD={2}|ERRMSG={3}", ldapUrl, uName, uPassword, ex.Message);
                Debuger.GetInstance().log(msg);
            }
            return userInfo;
        }
    }

    /// <summary>
    /// 油田AD验证获取的用户信息
    /// </summary>
    [Serializable]
    public class ADUserInfo
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 所属部门名称
        /// </summary>
        public string DeptName{ get; set; }
        /// <summary>
        /// 所属中石化分公司
        /// </summary>
        public string Company { get; set; }
    }
}
