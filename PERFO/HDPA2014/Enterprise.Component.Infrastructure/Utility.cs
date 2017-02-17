using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Enterprise.Component.Infrastructure
{
    /// <summary>
    /// 公共类库
    /// </summary>
    public class Utility
    {

        /// <summary>
        /// 获取webconfig配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConfigAppSetting(string name)
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings[name].ToRequestString();
        }

        /// <summary>
        /// 过滤一般的字符串
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        public static string FilterValue(string strFilter)
        {
            if (strFilter == null)
                return "";
            string returnValue = strFilter;
            string[] filterChar = new string[] { "\'", ",", "(", ")", ">", "<", "=", ";", "\"" };
            for (int i = 0; i < filterChar.Length; i++)
            {
                returnValue = returnValue.Replace(filterChar[i], "");
            }
            return returnValue.Trim(' ');
        }

        /// <summary>
        /// 获取网站目录下的文件的扩展名 不带.
        /// </summary>
        /// <param name="fileAbsPath">web绝对路径.使用了Server.MapPath</param>
        /// <returns></returns>
        public static string GetFileExt(string fileAbsPath)
        {
            return System.IO.Path.GetExtension(HttpContext.Current.Server.MapPath(fileAbsPath));
        }

        #region GridView相关

        /// <summary>
        /// 合并GridView中某列相同信息的行（单元格） 
        /// </summary>
        /// <param name="GridView1">GridView</param>
        /// <param name="cellNum">第几列</param>
        public static void GroupRows(GridView GridView1, int cellNum)
        {
            int i = 0, rowSpanNum = 1;
            while (i < GridView1.Rows.Count - 1)
            {
                GridViewRow gvr = GridView1.Rows[i];

                for (++i; i < GridView1.Rows.Count; i++)
                {
                    GridViewRow gvrNext = GridView1.Rows[i];
                    if (gvr.Cells[cellNum].Text == gvrNext.Cells[cellNum].Text && 
                        !string.IsNullOrEmpty(gvr.Cells[cellNum].Text))
                    {
                        gvrNext.Cells[cellNum].Visible = false;
                        rowSpanNum++;
                    }
                    else
                    {
                        gvr.Cells[cellNum].RowSpan = rowSpanNum;
                        rowSpanNum = 1;
                        break;
                    }

                    if (i == GridView1.Rows.Count - 1)
                    {
                        gvr.Cells[cellNum].RowSpan = rowSpanNum;
                    }
                }
            }
        }

        #endregion

        #region 提取大括号中的所有内容

        /// <summary>
        /// 根据匹配模式提取符串表达式字中的所有匹配值
        /// </summary>
        /// <param name="bds">字符串表达式</param>
        /// <param name="pattern">匹配模式</param>
        /// <returns></returns>
        public static List<string> GetMatchValues(string bds, string pattern)
        {
            List<string> matchValues = new List<string>();
            //string str = "{投入资本收入贡献系数}={本年经营收入总额}*2/({期初实收资本}+{期初资本公积}+{期末实收资本}+{期末资本公积}+{期初补充流动资金}+{期末补充流动资金})*100";
            //string pattern = @"\{.*?\}";//匹配模式
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(bds);
            //StringBuilder sb = new StringBuilder();//存放匹配结果
            foreach (Match match in matches)
            {
                //sb.AppendLine(match.Value);
                matchValues.Add(match.Value);
            }
            return matchValues;
        }

        #endregion

        #region 四则混合运算--调用计算公式

        ///<summary> 
        ///四则混合运算 
        ///</summary> 
        ///<param name="s">计算表达式</param> 
        ///<returns>计算结果</returns>
        ///<example> 
        ///<code> 
        ///objectx=Mathx.Eval("1+2*2-6/2"); 
        ///</code> 
        ///</example>
        public static object Eval(string s)
        {
            try
            {
                //MSScriptControl.ScriptControlClass sc = new MSScriptControl.ScriptControlClass();
                //sc.Language = "javascript";
                //return sc.Eval(s);
                Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
                return Microsoft.JScript.Eval.JScriptEvaluate(s, ve);
                //Microsoft.JScript.Vsa.VsaEngine v = new Microsoft.JScript.Vsa.VsaEngine();
                //v.InitVsaEngine("", new Microsoft.JScript.Vsa.BaseVsaSite());
                //return Microsoft.JScript.Eval.JScriptEvaluate(s, v);
            }
            catch { }
            return null;
        }

        #endregion

        /// <summary>
        /// 检查是否有权限
        /// </summary>
        /// <param name="pm">角色权限</param>
        /// <param name="data">模块权限</param>
        /// <returns></returns>
        public static bool CheckPermission(Int64 pm, Int64 data)
        {
            int pv = (int)Math.Pow(2, data);
            return (pm & pv) == pv;
        }

        /// <summary>
        /// 获取客户端浏览器的信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string GetCustomExplorerInfo(HttpRequest request, string order)
        {
            string s = string.Empty;
            System.Web.HttpBrowserCapabilities browser = request.Browser;
            switch(order) {
                case "1":
                    s = string.Format("[{0}({1})-{2}]", browser.Browser, browser.Version, browser.Type);
                    break;
                default:
                    s = "Browser Capabilites<br/>"
                        + "Type=" + browser.Type + "<br/>"
                        + "Name=" + browser.Browser + "<br/>"
                        + "Version=" + browser.Version + "<br/>"
                        + "Majior Version=" + browser.MajorVersion + "<br/>"
                        + "Platform=" + browser.Platform + "<br/>"
                        + "Is Beta=" + browser.Beta + "<br/>"
                        + "Is AOL=" + browser.AOL + "<br/>"
                        + "Is Crawler=" + browser.Crawler + "<br/>"
                        + "is Win16=" + browser.Win16 + "<br/>"
                        + "is Win32=" + browser.Win32 + "<br/>"
                        + "Supports Frames=" + browser.Frames + "<br/>"
                        + "Supports Tables=" + browser.Tables + "<br/>"
                        + "Supports Cookies=" + browser.Cookies + "<br/>"
                        + "Supports VBScript=" + browser.VBScript + "<br/>"
                        + "Supports JavaApplets=" + browser.JavaApplets + "<br/>"
                        + "Supports ActiveX Controls=" + browser.ActiveXControls + "<br/>";
                    break;
            }            
            return s;
        }

        /// <summary>
        /// 根据 User Agent 获取操作系统名称
        /// </summary>
        public static string GetOSNameByUserAgent(string userAgent)
        {
            string osVersion = "未知";

            if (userAgent.Contains("NT 6.0"))
            {
                osVersion = "Windows Vista/Server 2008";
            }
            else if (userAgent.Contains("NT 5.2"))
            {
                osVersion = "Windows Server 2003";
            }
            else if (userAgent.Contains("NT 5.1"))
            {
                osVersion = "Windows XP";
            }
            else if (userAgent.Contains("NT 5"))
            {
                osVersion = "Windows 2000";
            }
            else if (userAgent.Contains("NT 4"))
            {
                osVersion = "Windows NT4";
            }
            else if (userAgent.Contains("Me"))
            {
                osVersion = "Windows Me";
            }
            else if (userAgent.Contains("98"))
            {
                osVersion = "Windows 98";
            }
            else if (userAgent.Contains("95"))
            {
                osVersion = "Windows 95";
            }
            else if (userAgent.Contains("Mac"))
            {
                osVersion = "Mac";
            }
            else if (userAgent.Contains("Unix"))
            {
                osVersion = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                osVersion = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                osVersion = "SunOS";
            }
            return osVersion;
        }

        #region 产生文本框

        /// <summary>
        /// 生成文本输入框
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <param name="value">控件值</param>
        /// <param name="colIndex">列索引</param>
        /// <param name="rowIndex">行索引</param>
        /// <param name="dataType">数据类型（number,string）</param>
        /// <param name="isData">是否数字验证</param>
        /// <param name="extendAttr">扩展属性</param>
        /// <param name="style">扩展样式</param>
        /// <returns></returns>
        public static string GetTextBox(string name, object value, int colIndex, int rowIndex, string dataType, bool isData, string extendAttr, string style)
        {
            StringBuilder txtSB = new StringBuilder();
            txtSB.Append(
                string.Format("<input id='{5}' name='{0}' value='{1}' datatype='{2}' colnum='{3}' rownum='{4}' style=\"border:0px;display:block;background-color:#ADFF2F;" + style + "\" type='text' runat='server' ",
                name, ((value == null) ? "" : value.ToString()), dataType, colIndex, rowIndex, name));
            if (isData)
            {
                txtSB.Append(" onfocus=\"this.select();\" onkeypress = \"return regInput(this,String.fromCharCode(event.keyCode))\" onpaste = \"return regInput(this,window.clipboardData.getData('Text'))\" ondrop = \"return regInput(this,event.dataTransfer.getData('Text'))\" ");
            }
            if (!string.IsNullOrEmpty(extendAttr))
            {
                txtSB.Append(" " + extendAttr + " maxlength=\"100\" ");
            }
            txtSB.Append(" />");

            return txtSB.ToString();
        }

        #endregion


        #region "获取登陆用户UserId"
        
        ///// <summary>
        ///// 登录用户ID
        ///// </summary>
        //private static int IDENTIFY_USERID = 0;

        /// <summary>
        /// 用于保存当前用户的信息
        /// </summary>
        public static Hashtable SessionIDHash = new Hashtable();

        /// <summary>
        /// 默认标题长度
        /// </summary>
        public static int? TitleLength { get { return System.Web.Configuration.WebConfigurationManager.AppSettings["TitleLength"].ToNullOrInt(); } }

        /// <summary>
        /// 获取登陆用户UserID,如果未登陆为0
        /// </summary>
        public static int Get_UserId
        {
            get
            {
                int IDENTIFY_USERID = 0;
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        int.TryParse(HttpContext.Current.User.Identity.Name, out IDENTIFY_USERID);
                    }
                }
                return IDENTIFY_USERID;
            }
        }
        #endregion

        #region "获取用户Form提交字段值"
        /// <summary>
        /// 获取post和get提交值
        /// </summary>
        /// <param name="InputName">字段名</param>
        /// <param name="Method">post和get</param>
        /// <param name="MaxLen">最大允许字符长度 0为不限制</param>
        /// <param name="MinLen">最小字符长度 0为不限制</param>
        /// <param name="DataType">字段数值类型 int 和str和dat不限为为空</param>
        /// <returns></returns>
        public static object sink(string InputName, MethodType Method, int MaxLen, int MinLen, DataType DataType)
        {
            HttpContext getRequest = HttpContext.Current;
            string TempValue = "";

            #region "获取提交字段数据TempValue"
            if (Method == MethodType.Post)//从表单提交数据
            {
                if (getRequest.Request.Form[InputName] != null)
                {
                    TempValue = getRequest.Request.Form[InputName].ToString();
                }
            }
            else if (Method == MethodType.Get)//从Url传递数据
            {
                if (getRequest.Request.QueryString[InputName] != null)
                {
                    TempValue = getRequest.Request.QueryString[InputName].ToString();
                }
            }
            else //未知的数据提交方式
            {
                //弹出框提示方式
                //todo
            }
            #endregion

            #region "检测最大允许长度"
            if (MaxLen != 0)
            {
                if (TempValue.Length > MaxLen)
                {
                    //MessageBoxServices.ShowMessageBox(2, "输入数据格式验证失败", string.Format("{0}字段值：{1}超过系统允许长度{2}!", InputName, TempValue, MaxLen), Icon_Type.Error, "history.back();", UrlType.JavaScript);
                    //弹出框提示方式
                    //todo
                }
            }
            #endregion

            #region "检测提交最小字符数"
            if (MinLen != 0)
            {
                if (TempValue.Length < MinLen)
                {
                    //PGGSOA.COMMON.MessageBox.ShowMessageBox(2, "输入数据格式验证失败", string.Format("{0}字段值：{1}小于系统规定长度{2}!", InputName, TempValue, MinLen), PGGSOA.COMMON.Components.Icon_Type.Error, "history.back();", PGGSOA.COMMON.Components.UrlType.JavaScript);
                    //弹出框提示方式
                    //todo
                }
            }
            #endregion

            #region "检测数据类型"
            if (!string.IsNullOrEmpty(TempValue))
            {
                TempValue = TempValue.Trim();
                switch (DataType)
                {
                    case DataType.Int:
                        int IntTempValue = 0;
                        if (!int.TryParse(TempValue, out IntTempValue))
                        //MessageBoxServices.ShowMessageBox(2, "输入数据格式验证失败", string.Format("{0}字段值：{1}数据类型必需为Int型!", InputName, TempValue), Icon_Type.Error, "history.back();", UrlType.JavaScript);
                        //弹出框提示方式
                        //todo
                        {

                        }
                        return IntTempValue;

                    case DataType.Date:
                        DateTime DateTimeTempValue = DateTime.MaxValue;
                        if (!DateTime.TryParse(TempValue, out DateTimeTempValue))
                        //MessageBoxServices.ShowMessageBox(2, "输入数据格式验证失败", string.Format("{0}字段值：{1}数据类型必需为日期型!", InputName, TempValue), Icon_Type.Error, "history.back();", UrlType.JavaScript);
                        //弹出框提示方式
                        //todo
                        {

                        }
                        return DateTimeTempValue;

                    case DataType.Long:
                        long LongTempValue = long.MinValue;
                        if (!long.TryParse(TempValue, out LongTempValue))
                        //MessageBoxServices.ShowMessageBox(2, "输入数据格式验证失败", string.Format("{0}字段值：{1}数据类型必需为Log型!", InputName, TempValue), Icon_Type.Error, "history.back();", UrlType.JavaScript);
                        //弹出框提示方式
                        //todo
                        {

                        }
                        return LongTempValue;

                    case DataType.Double:
                        double DoubleTempValue = double.MinValue;
                        if (!double.TryParse(TempValue, out DoubleTempValue))
                        //MessageBoxServices.ShowMessageBox(2, "输入数据格式验证失败", string.Format("{0}字段值：{1}数据类型必需为Double型!", InputName, TempValue), Icon_Type.Error, "history.back();", UrlType.JavaScript);
                        //弹出框提示方式
                        //todo
                        {

                        }
                        return DoubleTempValue;

                    case DataType.CharAndNum:
                        if (!CheckRegEx(TempValue, "^[A-Za-z0-9]+$"))
                        //MessageBoxServices.ShowMessageBox(2, "输入数据格式验证失败", string.Format("{0}字段值：{1}数据类型必需为英文或数字!", InputName, TempValue), Icon_Type.Error, "history.back();", UrlType.JavaScript);
                        //弹出框提示方式
                        //todo
                        {

                        }
                        return TempValue;

                    case DataType.CharAndNumAndChinese:
                        if (!CheckRegEx(TempValue, "^[A-Za-z0-9\u00A1-\u2999\u3001-\uFFFD]+$"))
                        //MessageBoxServices.ShowMessageBox(2, "输入数据格式验证失败", string.Format("{0}字段值：{1}数据类型必需为英文或数字或中文!", InputName, TempValue), Icon_Type.Error, "history.back();", UrlType.JavaScript);
                        //弹出框提示方式
                        //todo
                        {

                        }
                        return TempValue;

                    case DataType.Email:
                        if (!CheckRegEx(TempValue, "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"))
                        //MessageBoxServices.ShowMessageBox(2, "输入数据格式验证失败", string.Format("{0}字段值：{1}数据类型必需为邮件地址!", InputName, TempValue), Icon_Type.Error, "history.back();", UrlType.JavaScript);
                        //弹出框提示方式
                        //todo
                        {

                        }
                        return TempValue;

                    case DataType.Decimal:
                        decimal DecimalTempValue = decimal.MinValue;
                        if (!decimal.TryParse(TempValue, out DecimalTempValue))
                        //MessageBoxServices.ShowMessageBox(2, "输入数据格式验证失败", string.Format("{0}字段值：{1}数据类型必需为Decimal类型!", InputName, TempValue), Icon_Type.Error, "history.back();", UrlType.JavaScript);
                        //弹出框提示方式
                        //todo
                        {

                        }
                        return DecimalTempValue;
                    case DataType.Float:
                        float FloatTempValue = float.MinValue;
                        if (!float.TryParse(TempValue, out FloatTempValue))
                        //MessageBoxServices.ShowMessageBox(2, "输入数据格式验证失败", string.Format("{0}字段值：{1}数据类型必需为Float类型!", InputName, TempValue), Icon_Type.Error, "history.back();", UrlType.JavaScript);
                        //弹出框提示方式
                        //todo
                        {

                        }
                        return FloatTempValue;
                    case DataType.Guid:
                        Guid guid = Guid.Empty;
                        if (!string.IsNullOrEmpty(TempValue))
                        {
                            guid = new Guid(TempValue);
                        }
                        return guid;
                    default:
                        return TempValue;
                }

            }
            else //判断值为空的情况
            {
                switch (DataType)
                {
                    case DataType.Int:
                        return 0;
                    case DataType.Date:
                        return DateTime.MaxValue;
                    case DataType.Long:
                        return long.MinValue;
                    case DataType.Double:
                        return (double)0;
                    case DataType.Decimal:
                        return (decimal)0;
                    case DataType.Float:
                        return (float)0;
                    case DataType.Guid:
                        return Guid.Empty;
                    default:
                        return TempValue;
                }
            }
            #endregion
        }

        #endregion

        #region "正式表达式验证"
        /// <summary>
        /// 正式表达式验证
        /// </summary>
        /// <param name="C_Value">验证字符</param>
        /// <param name="C_Str">正式表达式</param>
        /// <returns>符合true不符合false</returns>
        public static bool CheckRegEx(string C_Value, string C_Str)
        {
            Regex objAlphaPatt;
            objAlphaPatt = new Regex(C_Str, RegexOptions.Compiled);
            return objAlphaPatt.Match(C_Value).Success;
        }
        #endregion

        #region "获取用户IP地址"
        /// <summary>
        /// 获取用户IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {

            string user_IP = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    user_IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    user_IP = HttpContext.Current.Request.UserHostAddress;
                }
            }
            else
            {
                user_IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return user_IP;
        }
        #endregion

        #region "分页 每页记录数"
        /// <summary>
        /// 分页 每页记录数
        /// </summary>
        public static int PageSize
        {
            get
            {
                return 20;
            }
        }
        #endregion

        #region "MD5加密"
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string md5(string str, int code)
        {
            string strEncrypt = string.Empty;
            if (code == 16)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
            }

            if (code == 32)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }

            return strEncrypt;
        }
        #endregion

        #region "页面遮罩代码"
        /// <summary>
        /// 遮罩层html
        /// </summary>
        public static string MaskHtml
        {
            get
            {
                return "<div class='loaddiv' style=\"position:absolute;left:0px;top:0px;width:100%;height:100%;filter:Alpha(Opacity=30);opacity:0.3;background-color:#9D9D9D;z-index:101; text-align:center; vertical-align:middle;\"></div>"
                + "<div  class='loaddiv' style=\"position:absolute;font:11px '宋体';top:51%;left:50%;width:300px;height:200px;text-align:center; vertical-align:middle;background-color:#FFFFFF;padding:1px;line-height:22px;z-index:100;margin-top:-125px; margin-left:-170px;\">"
                + "<span style=\"color:Red; font-size:12px;\">Loading...</span><br/><img src=\"styles/images/wait.gif\" alt=\"\" /></div>";
            }
        }
        #endregion

        #region "加载Html模版"
        public string ReadHtml(string absFilePath)
        {
            string html = "";
            if (File.Exists(absFilePath))
                html = File.ReadAllText(absFilePath, Encoding.UTF8);
            return html;
        }
        #endregion

        #region 去除HTML标签

        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }

        #endregion

        #region "头部菜单web控件样式"
        /// <summary>
        /// 头部菜单web控件样式
        /// </summary>
        public static string HeadMenuTemplateTxt
        {
            get
            {
                //                return @"<!-- 头部菜单 Start -->
                //	        <table border='0' cellpadding='0' cellspacing='0' width='100%' align='center'>
                //              <tr>
                //                <td class='menubar_title'><img border='0' src='{0}' align='absmiddle' hspace='3' vspace='3'>&nbsp;{1}</td>
                //                <td class='menubar_readme_text' valign='bottom'><img src='{2}' align='absMiddle' border='0' />&nbsp;{3}</td>
                //              </tr>
                //              <tr><td height='1px' style='background-color:#ccc;' colspan='2'>    </td></tr>
                //              <tr>
                //                <td height='27px' class='menubar_function_text'></td>
                //                <td class='menubar_menu_td' align='right'>{4}</td>
                //              </tr>              
                //            </table>
                //        <!-- 头部菜单 End -->
                //        ";

                return @"<!-- 头部菜单 Start -->
	        <table border='0' cellpadding='0' cellspacing='0' width='100%' align='center'>             
              <tr>                               
                 <td class='menubar_menu_td' align='right'>{4}</td>
              </tr>                         
            </table>
        <!-- 头部菜单 End -->
        ";
            }
        }

        /// <summary>
        /// 头部菜单web控件样式
        /// </summary>
        public static string HeadMenuTemplateTxt1
        {
            get
            {
                //                return @"<!-- 头部菜单 Start -->
                //	        <table border='0' cellpadding='0' cellspacing='0' width='100%' align='center'>
                //              <tr>
                //                <td class='menubar_title'><img border='0' src='{0}' align='absmiddle' hspace='3' vspace='3'>&nbsp;{1}</td>
                //                <td class='menubar_readme_text' valign='bottom'><img src='{2}' align='absMiddle' border='0' />&nbsp;{3}</td>
                //              </tr>
                //              <tr><td height='1px' style='background-color:#ccc;' colspan='2'>    </td></tr>
                //              <tr>
                //                <td height='27px' class='menubar_function_text'></td>
                //                <td class='menubar_menu_td' align='right'>{4}</td>
                //              </tr>              
                //            </table>
                //        <!-- 头部菜单 End -->
                //        ";

                return @"<!-- 头部菜单 Start -->
	        <div class='toolbars'>{4}</div>
        <!-- 头部菜单 End -->
        ";
            }
        }

        #endregion

        #region "获取页面url"
        /// <summary>
        /// 获取当前访问页面地址
        /// </summary>
        public static string GetScriptName
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            }
        }

        /// <summary>
        /// 检测当前url是否包含指定的字符
        /// </summary>
        /// <param name="sChar">要检测的字符</param>
        /// <returns></returns>
        public static bool CheckScriptNameChar(string sChar)
        {
            bool rBool = false;
            if (GetScriptName.ToLower().LastIndexOf(sChar) >= 0)
                rBool = true;
            return rBool;
        }

        /// <summary>
        /// 获取当前页面的扩展名
        /// </summary>
        public static string GetScriptNameExt
        {
            get
            {
                return GetScriptName.Substring(GetScriptName.LastIndexOf(".") + 1);
            }
        }

        /// <summary>
        /// 获取当前访问页面地址参数
        /// </summary>
        public static string GetScriptNameQueryString
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["QUERY_STRING"].ToString();
            }
        }

        /// <summary>
        /// 获取当前访问页面Url
        /// </summary>
        public static string GetScriptUrl
        {
            get
            {
                return GetScriptNameQueryString == "" ? GetScriptName : string.Format("{0}?{1}", GetScriptName, GetScriptNameQueryString);
            }
        }

        /// <summary>
        /// 返回当前页面目录的url
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        public static string GetHomeBaseUrl(string FileName)
        {
            string Script_Name = GetScriptName;
            return string.Format("{0}/{1}", Script_Name.Remove(Script_Name.LastIndexOf("/")), FileName);
        }

        /// <summary>
        /// 返回当前网站网址
        /// </summary>
        /// <returns></returns>
        public static string GetHomeUrl()
        {
            return HttpContext.Current.Request.Url.Authority;
        }

        /// <summary>
        /// 获取当前访问文件物理目录
        /// </summary>
        /// <returns>路径</returns>
        public static string GetScriptPath
        {
            get
            {
                string Paths = HttpContext.Current.Request.ServerVariables["PATH_TRANSLATED"].ToString();
                return Paths.Remove(Paths.LastIndexOf("\\"));

            }
        }

        /// <summary>
        /// 获得页面文件名和参数名
        /// </summary>
        public static string GetScriptNameUrl
        {
            get
            {
                string Script_Name = GetScriptName;
                Script_Name = Script_Name.Substring(Script_Name.LastIndexOf("/") + 1);
                Script_Name += "?" + GetScriptNameQueryString;
                return Script_Name;
            }
        }

        /// <summary>
        /// 获得页面文件的访问路径
        /// </summary>
        public static string GetScriptPathUrl
        {
            get
            {
                string Script_Name = GetScriptName;
                Script_Name = Script_Name.Substring(0, Script_Name.LastIndexOf("/") + 1);
                return Script_Name;
            }
        }

        /// <summary>
        /// 获得当前页面的文件名
        /// </summary>
        public static string GetScriptFileName
        {
            get
            {
                string Script_Name = GetScriptName;
                Script_Name = Script_Name.Substring(Script_Name.LastIndexOf("/") + 1);
                return Script_Name;
            }
        }

        #endregion

        #region "根据文件扩展名获取当前目录下的文件列表"
        /// <summary>
        /// 根据文件扩展名获取当前目录下的文件列表
        /// </summary>
        /// <param name="FileExt">文件扩展名</param>
        /// <returns>返回文件列表</returns>
        public static List<string> GetDirFileList(string FileExt)
        {
            List<string> FilesList = new List<string>();
            string[] Files = Directory.GetFiles(GetScriptPath, string.Format("*.{0}", FileExt));
            foreach (string var in Files)
            {
                FilesList.Add(System.IO.Path.GetFileName(var).ToLower());
            }
            return FilesList;
        }
        #endregion

        #region "权限类型 枚举"
        /// <summary>
        /// 权限类型枚举
        /// </summary>
        public enum PopedomType
        {
            /// <summary>
            /// 列表/查看
            /// </summary>
            List = 2,
            /// <summary>
            /// 新增
            /// </summary> 
            New = 4,
            /// <summary>
            /// 修改
            /// </summary>
            Edit = 8,
            /// <summary>
            /// 删除
            /// </summary>
            Delete = 16,
            /// <summary>
            /// 排序
            /// </summary>
            Orderby = 32,
            /// <summary>
            /// 打印
            /// </summary>
            Print = 64,
            /// <summary>
            /// 备用A
            /// </summary>
            A = 128,
            /// <summary>
            /// 备用B
            /// </summary>
            B = 256
        }
        #endregion

        #region "链接类型 枚举"
        /// <summary>
        /// 链接类型枚举
        /// </summary>
        public enum UrlType : byte
        {
            /// <summary>
            /// 超级链接
            /// </summary>
            Href,
            /// <summary>
            /// JavaScript 脚本
            /// </summary>
            JavaScript,
            /// <summary>
            /// VBScript 脚本
            /// </summary>
            VBScript
        }
        #endregion

        #region "表单数据获取方式 枚举"
        /// <summary>
        /// 表单数据获取方式
        /// </summary>
        public enum MethodType
        {
            /// <summary>
            /// Post方式
            /// </summary>
            Post = 1,
            /// <summary>
            /// Get方式
            /// </summary>
            Get = 2
        }
        #endregion

        #region "数据类型 枚举"
        /// <summary>
        /// 数据类型
        /// </summary>
        public enum DataType
        {
            /// <summary>
            /// 字符
            /// </summary>
            Str = 1,
            /// <summary>
            /// 日期时间
            /// </summary>
            Date = 2,
            /// <summary>
            /// 整型
            /// </summary>
            Int = 3,
            /// <summary>
            /// 长整型
            /// </summary>
            Long = 4,
            /// <summary>
            /// 双精度小数
            /// </summary>
            Double = 5,
            /// <summary>
            /// 只限字符和数字
            /// </summary>
            CharAndNum = 6,
            /// <summary>
            /// 只限邮件地址
            /// </summary>
            Email = 7,
            /// <summary>
            /// 只限字符和数字和中文
            /// </summary>
            CharAndNumAndChinese = 8,
            /// <summary>
            /// 小类型的精度类型
            /// </summary>
            Decimal = 9,
            /// <summary>
            /// 浮点型数据
            /// </summary>
            Float = 10,
            /// <summary>
            /// Guid
            /// </summary>
            Guid = 11,

        }

        #endregion

        #region "语言类型 枚举"
        /// <summary>
        /// 数据类型
        /// </summary>
        public enum LanguageType
        {
            /// <summary>
            /// 俄文
            /// </summary>
            ru,
            /// <summary>
            /// 中文
            /// </summary>
            zhcn,
            /// <summary>
            /// 英文
            /// </summary>
            en 
        }

        #endregion

        #region "格式化Oracle插入日期格式"
        /// <summary>
        /// 格式化Oracle插入日期格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string FormatOracleDate(DateTime dt)
        {
            return string.Format("to_date('{0}','YYYY-MM-DD HH24:MI:SS')", dt);
        }

        #endregion

        #region "EasyUI弹出窗口、消息、弹出消息"

        /// <summary>
        /// 消息显示风格
        /// </summary>
        public enum MsgShowType
        {
            show,
            slide,
            fade
        }

        /// <summary>
        /// 显示错误信息提示框：EasyUI_Messager
        /// </summary>
        /// <param name="page">this.page</param>
        /// <param name="title">Title</param>
        /// <param name="msg">错误信息</param>
        public static void ShowMsg(System.Web.UI.Page page, string title, string msg)
        {
            page.ClientScript.RegisterStartupScript(page.ClientScript.GetType(), "msg", "<script>$(document).ready(function() {$.messager.alert('"+title+"','" + msg + "','error')});</script> ");
        }

        /// <summary>
        /// 显示提示信息支持高度和样式
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        /// <param name="hh">高度</param>
        /// <param name="showType">显示样式</param>
        public static void ShowMsg(System.Web.UI.Page page, string title, string msg, int hh, string showType)
        {
            page.ClientScript.RegisterStartupScript(page.ClientScript.GetType(), "msg", "<script>$(document).ready(function() {$.messager.show({title: '" + title + "',msg: '" + msg + "',height: '" + hh + "',showType: '" + showType + "'});});</script> ");
        }
        
        /// <summary>
        /// 显示信息提示框：EasyUI_Messager
        /// </summary>
        /// <param name="page">this.page</param>
        /// <param name="msg">提示信息</param>
        public static void ShowMsg(System.Web.UI.Page page, string msg)
        {
            page.ClientScript.RegisterStartupScript(page.ClientScript.GetType(), "msg", "<script>$(document).ready(function() {$.messager.alert('操作提示','" + msg + "')});</script> ");
        }

        /// <summary>
        /// 弹出信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="title">窗口标题</param>
        /// <param name="msg">消息</param>
        /// <param name="Redirecturl">点击确认后转去特定地址</param>
        public static void ShowMsg(System.Web.UI.Page page, string title , string msg, string Redirecturl)
        {
            if (!string.IsNullOrEmpty(Redirecturl) && Redirecturl.StartsWith("Project"))
            {
                ShowMsgThenReloadTree(page, title, msg, Redirecturl);
                return;
            }
            HttpContext.Current.Response.Clear();
            RepsonseWriteHeader();
            HttpContext.Current.Response.Write("<script>$(document).ready(function() {$.messager.alert('"+title+"','" + msg + "','info',function(){location.href='"+Redirecturl+"';})});</script> ");
            //page.ClientScript.RegisterStartupScript(page.ClientScript.GetType(), "msg",
            //"<script>" + "window.location.href='/error.aspx?msg=" + msg + "&url=" + Redirecturl + "&title=" + title + "'; " +

            //"</script>"
            //);
            HttpContext.Current.Response.Write("</body></html>");
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 清空当前页面输出 html文件头
        /// </summary>
        public static void RepsonseWriteHeader()
        {
            HttpContext.Current.Response.Clear();
            string str = "<html><head><title></title>"
                +"<link rel=\"stylesheet\" type=\"text/css\" href=\"/Resources/Styles/css/main.css\" />"
                +"<link rel=\"stylesheet\" type=\"text/css\" href=\"/Resources/easyui1.2.6/themes/gray/easyui.css\" />"
                +"<link rel=\"stylesheet\" type=\"text/css\" href=\"/Resources/easyui1.2.6/themes/icon.css\" />"
                +"<script type=\"text/javascript\" src=\"/Resources/OA/jquery/jquery-1.8.0.min.js\"></script> "
                + "<script type=\"text/javascript\" src=\"/Resources/OA/easyui1.32/jquery.easyui.min.js\"></script> "
                + "<script type=\"text/javascript\" src=\"/Resources/OA/easyui1.32/locale/easyui-lang-zh_CN.js\"></script>"
                +"</head><body>";
                HttpContext.Current.Response.Write(str);
                
            
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="page"></param>
        ///// <param name="url"></param>
        ///// <param name="title"></param>
        ///// <param name="width"></param>
        ///// <param name="height"></param>
        ///// <param name="modal"></param>
        //public static void Show_Window(System.Web.UI.Page page,string url , string title,string width,string height,bool modal)
        //{
        //    page.ClientScript.RegisterStartupScript(page.ClientScript.GetType(), "msg", @"<script> $('#win').window({ title: "+title+",width: "+width+",        height: "+height+",        modal: "+modal.ToString()+",        shadow: false,        closed: false    });    $('#win').window('open');    $('#win').append(\"<iframe src='" + url + "' frameborder='0' scrolling='no' border='0' width='100%' height='100%'></iframe>\");</script> ");
        //}

        /// <summary>
        /// 关闭弹出窗口：close_win方法在EasyuiWin.js内
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rtnValue">返回值</param>
        /// <param name="refresh">刷新父页面:如果需要返回值请不要刷新页面</param>
        public static void CloseWin(System.Web.UI.Page page, string rtnValue,bool refresh)
        {
            page.ClientScript.RegisterStartupScript(page.ClientScript.GetType(), "msg", "<script>close_win('"+rtnValue+"','"+refresh+"');</script> ");
        }
        #endregion

        #region "格式化工作流中节点类型"

        /// <summary>
        /// 流程节点类型 0 审批节点，1 通知节点，2 结束节点
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public static string GetWorkFlowType(int typeId)
        {
            string str = "";
            switch (typeId)
            {
                case 0:
                    str = "审批节点";
                    break;
                case 1:
                    str = "通知节点";
                    break;
                case 2:
                    str = "结束节点";
                    break;
            }
            return str;
        }

        /// <summary>
        /// 流程节点办理对象类型 0 指定成员，1 指定职务，2 申请人的部门管理人、3 申请人的部门主管、4流程申请人
        /// </summary>
        /// <param name="objecttypeId"></param>
        /// <returns></returns>
        public static string GetWorkFlowObjectType(int objecttypeId)
        {
            string str = "";
            switch (objecttypeId)
            {
                case 0:
                    str = "指定成员";
                    break;
                case 1:
                    str = "指定职务";
                    break;
                case 2:
                    str = "申请人的部门管理人";
                    break;
                case 3:
                    str = "申请人的部门主管";
                    break;
                case 4:
                    str = "流程申请人";
                    break;
            }
            return str;
        }

        #endregion

        #region "获取流程类型"
        /// <summary>
        /// 获取流程类型
        /// </summary>
        /// <param name="typeId">类型编号</param>
        /// <returns></returns>
        public static string GetWorkFlowName(int typeId)
        {
            string str = "";
            switch (typeId)
            {
                case 1:
                    str= "公文流转";
                    break;
            }
            return str;
        }

        #endregion

        #region " 绑定年份和月份到DropDownList"
        public static void bindYear(System.Web.UI.WebControls.DropDownList ddl_nf)
        {
            ddl_nf.Items.Clear();
            int thisyear = Convert.ToInt32(DateTime.Now.Year.ToString().Trim());
            int preyear = thisyear - 10;
            for (int year = preyear; year <= thisyear + 10; year++)
            {
                ddl_nf.Items.Add(year.ToString());
            }
        }

        public static void bindMonth(System.Web.UI.WebControls.DropDownList ddl_yf)
        {
            ddl_yf.Items.Clear();
            int thismonth = Convert.ToInt32(DateTime.Now.Month.ToString().Trim());
            for (int i = 1; i <= 12; i++)
            {
                ddl_yf.Items.Add(i.ToString());
            }
        }
        #endregion

        #region "根据当前日期获取农历日期"
        /// <summary>
        /// /*无效方法 不能返回期待值*/
        /// 根据当前日期获取农历日期
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCNDate()
        {
            CNDate cn = new CNDate(DateTime.Now);
            DateTime dt = cn.time;
            return dt;
        }

        #endregion

        #region "关闭弹出框并刷新界面"
        /// <summary>
        /// 关闭弹出框并刷新界面
        /// </summary>
        /// <param name="page"></param>
        public static void Closedg(System.Web.UI.Page page)
        {
            ClientScriptManager cs = page.ClientScript;
            cs.RegisterStartupScript(typeof(string), "TabJs", "<script language='javascript'>alert('操作成功！');var api = frameElement.api;var w = api.opener;w.location.reload();</script>");
        }

        #endregion

        #region "根据字符判断人员数"
        /// <summary>
        /// 根据字符判断人员数
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static int GetUserNum(string users)
        {
            if (string.IsNullOrEmpty(users)) return 0;
            users = users.TrimStart(',').TrimEnd(',');
            if (!string.IsNullOrEmpty(users))
            {
                string[] auser = users.Split(',');
                return auser.Length;
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region "设置语言"
        /// <summary>
        /// 当前语言(默认language)
        /// </summary>
        public static LanguageType Language
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["Language"] == null)
                {
                    if (HttpContext.Current.Session != null && 
                        HttpContext.Current.Session[WebKeys.LangName] != null)
                    {
                        return (LanguageType)Enum.Parse(typeof(LanguageType),
                            HttpContext.Current.Session[WebKeys.LangName].ToString());
                    }
                    return LanguageType.zhcn;
                }
                else
                {
                    return (LanguageType)Enum.Parse(typeof(LanguageType), 
                        HttpContext.Current.Request.Cookies["Language"].Value.ToString());
                }
            }
            set 
            {
                HttpContext.Current.Session[WebKeys.LangName] = value.ToString();
                HttpContext.Current.Response.Cookies["Language"].Value = value.ToString(); 
            }
        }

        #endregion

        #region 根据语言类型返回CSS样式的后缀名

        /// <summary>
        /// 返回非中文语言类型的样式表后缀名称
        /// </summary>
        /// <param name="css"></param>
        /// <returns></returns>
        public static string CssSuffix(string css)
        {
            if (Utility.Language != Utility.LanguageType.zhcn)
            {
                return string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}.css\" />", css.TrimEnd(".css".ToCharArray()) + "_" + Utility.Language.ToString());
            }
            return string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />", css);
        }

        #endregion

        #region 根据语言类型返回JS脚本的后缀名

        /// <summary>
        /// 返回非中文语言类型的JS脚本后缀名称
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public static string JsSuffix(string js)
        {
            if (Utility.Language != Utility.LanguageType.zhcn)
            {
                return string.Format("<script type=\"text/javascript\" src=\"{0}.js\"></script>", js.TrimEnd(".js".ToCharArray()) + "_" + Utility.Language.ToString());
            }
            return string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", js);
        }

        #endregion

        #region 判定是否包含中文字符
        /// <summary>
        /// 判断字符串是否包含中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasZhcn_C(string str)
        {
            Regex regexChinese = new Regex(@"\p{IsCJKUnifiedIdeographs}");
            return regexChinese.IsMatch(str.ToString());
        }

        /// <summary>
        /// 判断字符串是否包含俄文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasRu_C(string str)
        {
            Regex regexCyrlics = new Regex(@"\p{IsCyrillic}");
            return regexCyrlics.IsMatch(str.ToString());
        }

        /// <summary>
        /// 是否全是中文字符串
        /// </summary>
        /// <param name="InputText"></param>
        /// <returns></returns>
        public static bool IsZhcn(String InputText)
        {
            System.Text.RegularExpressions.Regex regxs = new System.Text.RegularExpressions.Regex("^[\u4E00-\u9FA5]|[\uFE30-\uFFA0]$");
            return regxs.IsMatch(InputText);
        }

        #endregion

        /// <summary>
        /// 弹出信息,点击后重新加载项目运行树
        /// </summary>
        /// <param name="page"></param>
        /// <param name="title">窗口标题</param>
        /// <param name="msg">消息</param>
        /// <param name="Redirecturl">点击确认后转去特定地址</param>
        public static void ShowMsgThenReloadTree(System.Web.UI.Page page, string title, string msg, string Redirecturl)
        {
            string refreshScript = "parent.$('#ProjectRunningTree').tree('reload');parent.$('#progressTable').treegrid('reload');";
            HttpContext.Current.Response.Clear();
            RepsonseWriteHeader();
            HttpContext.Current.Response.Write("<script>$(document).ready(function() {$.messager.alert('" + title + "','" + msg + "','info',function(){"+refreshScript+"location.href='" + Redirecturl + "';})});</script> ");
            HttpContext.Current.Response.Write("</body></html>");
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 绑定枚举类型到DropDownList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="ddl"></param>
        public static void BindDropDownListByEnumType<T>(T t,DropDownList ddl)
        {
            string[] names = Enum.GetNames(typeof(T));
            int[] values = (int[])Enum.GetValues(typeof(T));
            for (int i = 0; i < names.Length; i++)
            {
                ddl.Items.Add(new ListItem(names[i], values[i].ToString()));

            }
        }

    }
}
