using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Enterprise.Component.Infrastructure
{
    /// <summary>
    /// 公共方法类
    /// </summary>
    public sealed partial class CommonTool
    {

        /// <summary>
        /// 产生空格
        /// </summary>
        /// <param name="cou"></param>
        /// <returns></returns>
        public static string GenerateBlankSpace(int cou)
        {
            string s = "├─";
            string blankStr = "";
            for (int i = 1; i < cou; i++)
            {
                blankStr += "　";
            }
            return blankStr + s;
        }

        /// <summary>
        /// 检测首尾字符是否为汉字
        /// </summary>
        /// <param name="zhcn">汉字串</param>
        /// <returns></returns>
        public static bool CheckUnicodeIsZhcn(string zhcn)
        {
            if (string.IsNullOrEmpty(zhcn)) return false;
            char[] c = zhcn.ToCharArray();
            if (c.Length > 0)
            {
                if (Regex.IsMatch(c[0].ToString(), @"[\u4e00-\u9fbb]+$") &&
                    Regex.IsMatch(c[c.Length - 1].ToString(), @"[\u4e00-\u9fbb]+$"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 从Request.ServerVariables["Path_Info"]中获取上下文路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GainContextPath(string path)
        {
            return GetValue(path, "/", "aspx");
        }

        /// <summary>
        /// 获取A标签中的内容
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetA_HTML(string html)
        {
            string innerHTML = string.Empty;
            Regex reg = new Regex(@"(?is)<a[^>]*?href=(['""\s]?)(?<href>[^'""\s]*)\1[^>]*?>(?<text>(?:(?!</?a\b).)*)");
            MatchCollection match = reg.Matches(html);
            for (int i = 0; i < match.Count; i++)
            {
                innerHTML = (match[i].Groups["text"].Value + "----" + match[i].Groups["href"].Value);
            }
            //Regex reg = new Regex(@"<a\s*[^>]*>([\s\S]+?)</a>", RegexOptions.IgnoreCase);
            //Match m = reg.Match(html);
            //while (m.Success)
            //{
            //    innerHTML = m.Result("$1");
            //    // 得到正则的括号里的内容，就是a的innerHTML
            //    innerHTML = Regex.Replace(innerHTML, @"<[^>]*>", "", RegexOptions.IgnoreCase);
            //    // 替换掉里面的html，只保留文字 
            //    m = m.NextMatch();
            //    // 循环匹配html里的下一个结果
            //}
            return innerHTML;
        }

        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="s">开始</param>
        /// <param name="e">结束</param>
        /// <returns></returns> 
        public static string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }

        /// <summary>
        /// 转代码下的\r\n为HTML的<br>
        /// </summary>
        /// <param name="conts"></param>
        /// <returns></returns>
        public static string StrToHTML(string conts)
        {
            string ss = conts;

            if (!string.IsNullOrEmpty(ss))
            {
                ss = ss.Replace("\r\n", "<br>");
            }

            return ss;
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
        /// <returns></returns>
        public static string GetTextBox(string name, object value, int colIndex, int rowIndex, string dataType, bool isData)
        {
            StringBuilder txtSB = new StringBuilder();
            //<input name='Txt4_" + drv["rowid"] + "' style=\"border:0px;display:none;\" datatype='number' value='" + drv["季度半年度目标值"] + "' type='text' runat='server' colnum='4' rownum='" + e.Row.Index + "' onkeypress = \"return regInput(this,String.fromCharCode(event.keyCode))\" onpaste = \"return regInput(this,window.clipboardData.getData('Text'))\" ondrop = \"return regInput(this,event.dataTransfer.getData('Text'))\"/>
            txtSB.Append(
                string.Format("<input name='{0}' value='{1}' datatype='{2}' colnum='{3}' rownum='{4}' style=\"border:0px;display:none;\" type='text' runat='server' ",
                name, ((value == null) ? "" : value.ToString()), dataType, colIndex, rowIndex));
            if (isData)
            {
                txtSB.Append(" onkeypress = \"return regInput(this,String.fromCharCode(event.keyCode))\" onpaste = \"return regInput(this,window.clipboardData.getData('Text'))\" ondrop = \"return regInput(this,event.dataTransfer.getData('Text'))\" ");
            }
            txtSB.Append(" />");

            return txtSB.ToString();
        }



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
        /// <returns></returns>
        public static string GetTextBox(string name, object value, int colIndex, int rowIndex, string dataType, bool isData, string extendAttr)
        {
            StringBuilder txtSB = new StringBuilder();
            if (!name.StartsWith("Txt"))
                name = "Txt" + colIndex + "_" + name;
            txtSB.Append(
                string.Format("<input name='{0}' value='{1}' datatype='{2}' colnum='{3}' rownum='{4}' style=\"border:0px;display:block;background-color:#ADFF2F;\" type='text' runat='server' ",
                name, ((value == null) ? "" : value.ToString()), dataType, colIndex, rowIndex));
            if (isData)
            {
                txtSB.Append(" onkeypress = \"return regInput(this,String.fromCharCode(event.keyCode))\" onpaste = \"return regInput(this,window.clipboardData.getData('Text'))\" ondrop = \"return regInput(this,event.dataTransfer.getData('Text'))\" ");
            }
            if (!string.IsNullOrEmpty(extendAttr))
            {
                txtSB.Append(" " + extendAttr + " ");
            }
            txtSB.Append(" />");

            return txtSB.ToString();
        }

        #endregion


        /// <summary>
        /// 检查是否有权限
        /// </summary>
        /// <param name="pm">角色权限</param>
        /// <param name="data">模块权限</param>
        /// <returns></returns>
        public static bool CheckPermission(int pm, int data)
        {
            //int pv = (int)Math.Pow(2, data);
            return (pm & data) == data;
        }

        /// <summary>
        /// 判断是否为闰年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            //采用布尔数据计算判断是否能整除
            bool yearleap = ((year % 400 == 0) || ((year % 4 == 0) && (year % 100 != 0)));
            return yearleap;
        }

        /// <summary>
        /// 获取星期几
        /// </summary>
        /// <param name="weekName"></param>
        /// <returns></returns>
        public static string WeekName(string weekName)
        {
            string week = string.Empty;
            switch (weekName)
            {
                case "Sunday":
                    week = "星期日";
                    break;
                case "Monday":
                    week = "星期一";
                    break;
                case "Tuesday":
                    week = "星期二";
                    break;
                case "Wednesday":
                    week = "星期三";
                    break;
                case "Thursday":
                    week = "星期四";
                    break;
                case "Friday":
                    week = "星期五";
                    break;
                case "Saturday":
                    week = "星期五";
                    break;
            }
            return week;
        }

        /// <summary>
        /// 返回指定月的最大日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetMonth_MaxDay(int year,int month)
        {
            int maxDay = 0;
            bool isLeap = IsLeapYear(year);
            switch (month)
            {
                case 1://
                    maxDay = 31;
                    break;
                case 2://
                    maxDay = (isLeap) ? 29 : 28;
                    break;
                case 3://
                    maxDay = 31;
                    break;
                case 4://
                    maxDay = 30;
                    break;
                case 5://
                    maxDay = 31;
                    break;
                case 6://
                    maxDay = 30;
                    break;
                case 7://
                    maxDay = 31;
                    break;
                case 8://
                    maxDay = 31;
                    break;
                case 9://
                    maxDay = 30;
                    break;
                case 10://
                    maxDay = 31;
                    break;
                case 11://
                    maxDay = 30;
                    break;
                case 12://
                    maxDay = 31;
                    break;
            }

            return maxDay;
        }


        ///// <summary>
        ///// 检测用户IP地址是否被允许
        ///// </summary>
        ///// <param name="ipLst"></param>
        ///// <param name="ipAddr"></param>
        ///// <returns></returns>
        //public static bool CheckIPIsAllow(List<SysIpModel> ipLst, string ipAddr)
        //{
        //    try
        //    {
        //        if (ipAddr == null) return false;
        //        bool isAllow;
        //        if (ipLst != null && ipLst.Count > 0)
        //        {
        //            foreach (SysIpModel ipM in ipLst)
        //            {
        //                isAllow = true;//都是允许访问的地址
        //                if (ipM.IPAddr.IndexOf("-") > 0)
        //                {
        //                    //是地址段
        //                    string[] ips = ipM.IPAddr.Split('-');
        //                    if (ips != null && ips.Length == 2)
        //                    {
        //                        string[] ip1 = ips[0].Split('.');
        //                        string[] ip2 = ips[1].Split('.');
        //                        string[] ipSource = ipAddr.Split('.');
        //                        int p1, p2, pS;
        //                        if (ipSource.Length == 4 && ip1.Length == 4 && ip2.Length == 4)
        //                        {
        //                            //从第二位开始判断
        //                            if (ipSource[0].Equals(ip1[0]) || ipSource[0].Equals(ip2[0]))
        //                            {
        //                                if (ipSource[1].Equals(ip1[1]) || ipSource[1].Equals(ip2[1]))
        //                                {
        //                                    if (ipSource[2].Equals(ip1[2]) || ipSource[2].Equals(ip2[2]))
        //                                    {
        //                                        if (ipSource[3].Equals(ip1[3]) || ipSource[3].Equals(ip2[3]))
        //                                        {
        //                                            return true && isAllow;
        //                                        }
        //                                        else if ("*".Equals(ip1[3]) || "*".Equals(ip2[3]))
        //                                        {
        //                                            return true && isAllow;
        //                                        }
        //                                    }
        //                                    else if ("*".Equals(ip1[2]) || "*".Equals(ip2[2]))
        //                                    {
        //                                        return true && isAllow;
        //                                    }
        //                                    else
        //                                    {
        //                                        p1 = Convert.ToInt16(ip1[2]);
        //                                        p2 = Convert.ToInt16(ip2[2]);
        //                                        pS = Convert.ToInt16(ipSource[2]);
        //                                        if (pS > p1 && pS < p2)
        //                                        {
        //                                            return true && isAllow;
        //                                        }
        //                                    }
        //                                }
        //                                else if ("*".Equals(ip1[1]) || "*".Equals(ip2[1]))
        //                                {
        //                                    return true && isAllow;
        //                                }
        //                                else
        //                                {
        //                                    p1 = Convert.ToInt16(ip1[1]);
        //                                    p2 = Convert.ToInt16(ip2[1]);
        //                                    pS = Convert.ToInt16(ipSource[1]);
        //                                    if (pS > p1 && pS < p2)
        //                                    {
        //                                        return true && isAllow;
        //                                    }
        //                                }
        //                            }

        //                        }

        //                    }
        //                }
        //                else
        //                {
        //                    //单个地址或单个IP段
        //                    if (ipM.IPAddr.IndexOf("*") > 0)
        //                    {
        //                        if (ipAddr.IndexOf(ipM.IPAddr.Substring(0, ipM.IPAddr.IndexOf("*"))) == 0)
        //                        {
        //                            //IP在所属段
        //                            return isAllow && true;
        //                        }
        //                    }
        //                    else if (ipAddr.Equals(ipM.IPAddr))
        //                    {
        //                        //IP相同
        //                        return isAllow && true;
        //                    }
        //                }
        //            }
        //        }
        //        else if (ipLst.Count == 0)
        //        {
        //            //缺省放行
        //            return true;
        //        }
        //    }
        //    catch (Exception ex) { }

        //    return false;
        //}

    }
}
