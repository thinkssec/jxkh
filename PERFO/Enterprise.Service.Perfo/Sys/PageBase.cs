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
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Component.Infrastructure;


namespace Enterprise.Service.Perfo.Sys
{

    /// <summary>
    /// 文件名: PageBase.cs
    /// 文件描述: 页面的基类。
    /// 用于扩展System.Web.UI.Page的功能：
    /// 1、可封装Request对象中的表单元素到MODEL。
    /// 2、实现页面错误的统一处理。
    /// 3、实现系统的权限控制。
    /// 4、记录访问日志。
    /// 5、保存查询条件。
    /// 6、设定公共信息内容。
    /// 7、实现多语言的转换
    /// 
    /// 创建者: 乔巍
    /// 创建日期: 2014.11.1
    /// </summary>
    public class PageBase : System.Web.UI.Page
    {

        #region 变量声明

        /// <summary>
        /// 用户服务类
        /// </summary>
        protected SysUserService userService = new SysUserService();
        /// <summary>
        /// 机构服务类
        /// </summary>
        protected SysBmjgService bmjgService = new SysBmjgService();
        /// <summary>
        /// 角色权限服务类
        /// </summary>
        protected SysRolepermissionService rpService = new SysRolepermissionService();
        /// <summary>
        /// 用户特有权限服务类
        /// </summary>
        protected SysUserpermissionService userPmService = new SysUserpermissionService();
        /// <summary>
        /// 模块服务类
        /// </summary>
        protected SysModuleService moduleService = new SysModuleService();
        /// <summary>
        /// 日志服务类
        /// </summary>
        protected SysVisitlogService visitService = new SysVisitlogService();
        /// <summary>
        /// 用户MODEL
        /// </summary>
        protected SysUserModel userModel;
        /// <summary>
        /// 当前页面对应的模块
        /// </summary>
        protected SysModuleModel currentModule;
        /// <summary>
        /// 权限处理委托
        /// </summary>
        public event PermissionEventHandler PermissionHandler;
        /// <summary>
        /// TAB选项卡名称
        /// </summary>
        protected string TabTitle = string.Empty;
        /// <summary>
        /// 消息服务类
        /// </summary>
        protected KhMessageService msgService = new KhMessageService();
        /// <summary>
        /// 待办消息ID
        /// </summary>
        public string MSGID
        {
            get
            {
                return (string)Utility.sink("MSGID", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);
            }
        }
        /// <summary>
        /// 获取当前模块的URL路径
        /// </summary>
        public string PageUrl
        {
            get
            {
                string url = string.Empty;
                if (currentModule != null)
                {
                    string urlPrefix = "~/";
                    if (!string.IsNullOrEmpty(currentModule.WEBURL.Trim()))
                    {
                        url = urlPrefix + currentModule.WEBURL.TrimStart(urlPrefix.ToCharArray());
                    }
                    else
                    {
                        url = urlPrefix + currentModule.MURL.TrimStart(urlPrefix.ToCharArray());
                    }
                }
                return url;
            }
        }

        #endregion

        #region 自定义权限委托

        /// <summary>
        /// 声明一个权限处理委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void PermissionEventHandler(PermissionEventArgs e);

        /// <summary>
        /// 委托传递的事件参数
        /// </summary>
        public class PermissionEventArgs : EventArgs
        {
            /// <summary>
            /// 当前用户的角色在指定模块下的权限
            /// </summary>
            private SysRolepermissionModel _rolePmModel;
            public SysRolepermissionModel Model
            {
                get { return _rolePmModel; }
                set { _rolePmModel = value; }
            }

            /// <summary>
            /// 当前用户在指定模块下的特有权限
            /// </summary>
            private SysUserpermissionModel _userPmModel;
            public SysUserpermissionModel UserPm
            {
                get { return _userPmModel; }
                set { _userPmModel = value; }
            }

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="model">角色权限</param>
            public PermissionEventArgs(SysRolepermissionModel model)
            {
                this._rolePmModel = model;
            }

            /// <summary>
            /// 初始化2
            /// </summary>
            /// <param name="rpm">角色权限</param>
            /// <param name="upm">用户特有权限</param>
            public PermissionEventArgs(SysRolepermissionModel rpm, SysUserpermissionModel upm)
            {
                this._rolePmModel = rpm;
                this._userPmModel = upm;
            }

        }

        #endregion


        #region 重写超类的方法


        /// <summary>
        /// 重写OnInit方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            //event
            //this.Error += new EventHandler(BasePage_Error);
            this.PreRender += new EventHandler(BasePage_PreRender);

            base.OnInit(e);
        }


        /// <summary>
        /// 重写OnLoad方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {

            //先执行父类方法，以进行权限判断
            BasePage_Load(e);

            base.OnLoad(e);
        }


        /// <summary>
        /// 页面加载时的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BasePage_Load(EventArgs e)
        {
            //检测用户的缺省语言类型,只针对登录页
            string urlPath = Request.ServerVariables["Path_Info"];
            if (urlPath.Contains("Login.aspx"))
            {
                Session[WebKeys.LangName] = "zhcn";
            }
            //当前用户信息
            userModel = userService.GetSingle(Page.User.Identity.Name);

            if (urlPath.ToUpper().Contains("Login.aspx".ToUpper()) ||
                urlPath.ToUpper().Contains("Loginout.aspx".ToUpper()) ||
                urlPath.ToUpper().Contains("Default.aspx".ToUpper()) ||
                urlPath.ToUpper().Contains("ChangeLanguage.aspx".ToUpper()) ||
                urlPath.ToUpper().Contains("Main".ToUpper()) ||
                urlPath.ToUpper().Contains("KaoheIndex".ToUpper()) ||
                urlPath.ToUpper().Contains("KaoheBumenIndex".ToUpper()) ||
                urlPath.ToUpper().Contains("KaoheDanweiIndex".ToUpper()) ||
                urlPath.ToUpper().Contains("KaoheLeaderIndex".ToUpper()) ||
                urlPath.ToUpper().Contains("KaoheShowInfo.aspx".ToUpper()) ||
                urlPath.ToUpper().Contains("Error.aspx".ToUpper()))
            {
                //不用验证
                return;
            }
            else if (!Page.User.Identity.IsAuthenticated)
            {
                Response.Write("<script>top.window.navigate('" + ResolveClientUrl("~/") + "Login.aspx');</script>");
                Response.End();
            }

            //用户权限判断与处理
            if (userModel != null)
            {
                //提取当前用户角色的所有权限
                List<SysRolepermissionModel> rpLst = rpService.GetList().Where(p => p.ROLEID == userModel.ROLEID).ToList();
                //提取当前用户的特有权限
                List<SysUserpermissionModel> userPmLst = 
                    userPmService.GetList().Where(p => p.LOGINID == userModel.LOGINID).ToList();
                //提取所有模块信息
                IList<SysModuleModel> moduleLst = moduleService.GetList();
                //获取当前路径中的页面名称
                string fileUrl = CommonTool.GainContextPath(urlPath);
                if (string.IsNullOrEmpty(fileUrl) && !string.IsNullOrEmpty(urlPath))
                {
                    fileUrl = urlPath.TrimStart('/');
                    if (urlPath.IndexOf('?') > 0) 
                    {
                        fileUrl = fileUrl.Substring(0, urlPath.IndexOf('?'));
                    }
                }
                currentModule = moduleLst.FirstOrDefault(p => p.MURL.ToUpper().Contains(fileUrl.ToUpper()) || p.WEBURL.ToUpper() == fileUrl.ToUpper());
                if (currentModule != null)
                {
                    SysRolepermissionModel rolePermission = rpLst.Find(p => p.MID == currentModule.MID);//角色具有
                    SysUserpermissionModel userPermission = userPmLst.Find(p => p.MID == currentModule.MID);//用户具有
                    if (rolePermission != null || userPermission != null)
                    {
                        //记入访问日志
                        SysVisitlogModel visitlog = new SysVisitlogModel();
                        visitlog.DB_Option_Action = WebKeys.InsertAction;
                        visitlog.ID = CommonTool.GetGuidKey();
                        visitlog.VISITURL = Utility.GetScriptUrl;
                        visitlog.VISITIPADDR = Utility.GetIPAddress();
                        visitlog.USERNAME = userModel.USERNAME;
                        visitlog.OPERATIONDATE = DateTime.Now;
                        visitlog.BROWSERTYPE = Utility.GetCustomExplorerInfo(Request, "1");
                        if (rolePermission != null)
                            visitlog.OPERATIONTYPE = rolePermission.Module.MNAME;
                        else if (userPermission != null)
                            visitlog.OPERATIONTYPE = userPermission.Module.MNAME;
                        visitService.Execute(visitlog);

                        //有访问权限，执行子类页面权限设置
                        if (PermissionHandler != null)
                        {
                            PermissionHandler(new PermissionEventArgs(rolePermission, userPermission));
                        }
                    }
                    else
                    {
                        //无访问权限
                        Response.Redirect(string.Format(ResolveClientUrl("~/") + "Error.aspx?msg={0}",
                            Server.UrlEncode(Trans("无访问权限"))));
                        Response.End();
                    }
                }
                else
                {
                    //无访问权限
                    Response.Redirect(string.Format(ResolveClientUrl("~/") + "Error.aspx?msg={0}",
                        Server.UrlEncode(Trans("无访问权限"))));
                    Response.End();
                }
            }

        }


        /// <summary>
        /// 页面呈现前的方法(可进行多语言转换)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BasePage_PreRender(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 页面元素单独调用方法
        /// </summary>
        /// <param name="zhcn">中文</param>
        /// <returns></returns>
        protected string Trans(string zhcn)
        {
            return zhcn;
        }

        /// <summary>
        /// 跳转回当前模块的URL地址
        /// </summary>
        /// <param name="urlParam"></param>
        protected void GobackPageUrl(string urlParam)
        {
            string pageUrl = string.Empty;
            if (currentModule != null)
            {
                string urlPrefix = "~/";
                if (!string.IsNullOrEmpty(currentModule.WEBURL.Trim()))
                {
                    pageUrl = urlPrefix + currentModule.WEBURL.TrimStart(urlPrefix.ToCharArray());
                }
                else
                {
                    pageUrl = urlPrefix + currentModule.MURL.TrimStart(urlPrefix.ToCharArray());
                }
                if (!string.IsNullOrEmpty(urlParam))
                {
                    if (pageUrl.IndexOf('?') > 0)
                    {
                        pageUrl += "&" + urlParam.TrimStart('?');
                    }
                    else
                    {
                        pageUrl += urlParam;
                    }
                }
            }

            if (!string.IsNullOrEmpty(pageUrl))
                Response.Redirect(pageUrl);
        }

        /// <summary>
        /// 获取当前模块的URL路径带参数
        /// </summary>
        /// <param name="urlParam">参数</param>
        /// <returns></returns>
        protected string GetPageUrlAndParams(string urlParam)
        {
            string pUrl = PageUrl;
            if (!string.IsNullOrEmpty(urlParam))
            {
                if (pUrl.IndexOf('?') > 0)
                {
                    pUrl += "&" + urlParam.TrimStart('?');
                }
                else
                {
                    pUrl += urlParam;
                }
            }
            return pUrl;
        }

        /// <summary>
        /// 获取样式表名称
        /// </summary>
        /// <param name="cssFileName"></param>
        /// <returns></returns>
        protected string GetCss(string cssFileName)
        {
            return cssFileName + ".css";
        }

        /// <summary>
        /// 设置控制的可见性
        /// </summary>
        /// <param name="cnl"></param>
        /// <param name="v"></param>
        protected void SetCntrlVisibility(WebControl cnl, bool v)
        {
            //当Enabled为false时，默认不可见
            if (cnl.Enabled)
            {
                cnl.Visible = v;
            }
            else
            {
                cnl.Visible = false;
            }
        }

        #region 权限控制相关

        /// <summary>
        /// 检测当前用户的有无数据录入操作权限
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="jgbm">补考核机构</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        protected bool ChkUserLuruPermission(string khid, int jgbm, SysUserModel user)
        {
            /*
             * 1、检测用户是否为本机构的人员
             * 2、检测用户是否为绩效管理员
             * 3、检测用户是否为完成值审核人之一
             */
            return (user.JGBM == jgbm || user.ROLEID == "paadmin" || KhJgbmdfbService.IsDfzUser(khid, jgbm, user));
        }

        /// <summary>
        /// 检测当前用户的有无数据审核操作权限
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="jgbm">补考核机构</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        protected bool ChkUserAuditPermission(string khid, int jgbm, SysUserModel user)
        {
            /*
             * 2、检测用户是否为绩效管理员
             * 3、检测用户是否为完成值审核人之一
             */
            return (user.ROLEID == "paadmin" || KhJgbmdfbService.IsDfzUser(khid, jgbm, user));
        }

        /// <summary>
        /// 检测用户是否为指标的审核人
        /// </summary>
        /// <param name="zb">定量指标</param>
        /// <param name="shzs">审核人信息字串</param>
        /// <param name="userModel">用户MODEL</param>
        /// <returns></returns>
        protected bool IsShr(KhDlzbmxModel zb, string shzs, SysUserModel userModel)
        {
            if (userModel.ROLEID == "paadmin") 
                return true;
            bool isValid = false;
            if (string.IsNullOrEmpty(shzs))
            {
                //审核人信息字串为空时，以完成值审核人权限为准
                isValid = KhJgbmdfbService.
                                    IsDfzForJgbmAndZbbm(zb.KHID.ToRequestString(), zb.JGBM.ToInt(), zb.ZBBM, userModel);
            }
            else
            {
                string[] users = shzs.Split(',').ToArray();
                foreach (var user in users)
                {
                    if (isValid) break;
                    switch (user)
                    {
                        case "YQTLD"://油气田领导
                            if (userModel.DUTY == "油气田领导")
                            {
                                isValid = KhJgbmdfbService.
                                    IsDfzForJgbmAndZbbm(zb.KHID.ToRequestString(), zb.JGBM.ToInt(), zb.ZBBM, userModel);
                            }
                            break;
                        case "FGLD"://分管领导
                            var q = userModel.FgbmjgLst.FirstOrDefault(p => p.JGBM == zb.JGBM);
                            isValid = (q != null);
                            break;
                        default://机构编码或是用户账号
                            isValid = (userModel.JGBM == user.ToInt() || userModel.LOGINID == user);
                            break;
                    }
                }
            }
            return isValid;
        }

        #endregion

        #endregion

    }

}
