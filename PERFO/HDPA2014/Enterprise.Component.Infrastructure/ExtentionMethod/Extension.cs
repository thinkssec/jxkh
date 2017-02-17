using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.Common;
using System.Reflection;
using System.IO;

namespace Enterprise.Component.Infrastructure
{
    /// <summary>
    /// @各种扩展类 主要为EasyUI提供Json数据
    /// </summary>
    public static class Extension
    {
        #region "DataTable扩展方法 主要为EasyUI提供Json"
        /// <summary>
        /// 将数据表转换成Json
        /// </summary>
        /// <param name="dt">System.Data.DataTable</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>Json</returns>
        public static string ToJsonForEasyUiGrid(this DataTable dt, int page, int pageSize)
        {
            return ExcuteResultToJson(dt, page, pageSize);    
        }

        /// <summary>
        /// 将数据表转换成Json
        /// </summary>
        /// <param name="dt">System.Data.DataTable</param>
        /// <returns>Json</returns>
        public static string ToJsonForEasyUiList(this DataTable dt)
        {
            return ExcuteResultToJson(dt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt">System.Data.DataTable</param>
        /// <param name="idField">Id字段</param>
        /// <param name="textField">Text字段</param>
        /// <param name="parentIdField">父Id字段</param>
        /// <param name="rootIdValue">根节点的ParentId</param>
        /// <param name="selecedId">默认选择Id</param>
        /// <returns></returns>
        public static string ToJsonForEasyuiComboTree(this DataTable dt, string idField, string textField, string parentIdField, string rootIdValue, string selecedId)
        {
            //add by qw 2013.7.6 增加缓存处理 start
            //end
            return ExcuteResultToComboTreeJson(dt, idField, textField, parentIdField, rootIdValue, selecedId);
        }


        public static string CleanJson(this object str)
        {
            if (str != null)
                return str.ToString().Replace("\\", "\\\\").Replace("\t", "\\\\t").Replace("\r", "\\\\r").Replace("\n", "\\\\n").Replace("\"", "\\\"");
            else
                return "";
        }

        public static string ToJsonForTree(this DataTable dt, string idField, string textField, string parentIdField, string rootIdValue)
        {
            string JsonData = "[";
            DataRow[] drs = dt.Select(parentIdField + "='" + rootIdValue + "'");
            foreach (var d in drs)
            {
                JsonData += "{";

                JsonData += "\"text\":\"" + d[textField].ToString() + "\",";
                JsonData += "\"id\":\"" + d[idField].ToString() + "\",";
                DataRow[] childdrs = dt.Select(parentIdField + "='" + d[idField] + "'");
                if (childdrs.Count() > 0)
                {
                    JsonData += "\"children\":";
                    JsonData += ToJsonForTree(dt, idField, textField, parentIdField, d[idField].ToString());
                    JsonData += "},";
                }
                else
                {
                    JsonData = JsonData.Remove(JsonData.Length - 1);
                    JsonData += "},";
                }
            }
            if (JsonData.Length > 1)
                JsonData = JsonData.Remove(JsonData.Length - 1);
            JsonData += "]";
            return JsonData;
        }

        public static string ToJsonForCombobox(this DataTable dt, string idField, string textField)
        {
            string JsonData = "[";            
            foreach (DataRow d in dt.Rows)
            {
                JsonData += "{";
                JsonData += "\"text\":\"" + d[textField].ToString() + "\",";
                JsonData += "\"id\":\"" + d[idField].ToString() + "\"},";                
            }
            if (JsonData.Length > 1)
                JsonData = JsonData.Remove(JsonData.Length - 1);
            JsonData += "]";
            return JsonData;
        }


        ///new by zy 2014/6/6 star
        public static string ToJsonForComboGrid(this DataTable dt, string idField, string textField,string textField1,string textField2)
        {
            string JsonData = "[";
            foreach (DataRow d in dt.Rows)
            {
                JsonData += "{";
                JsonData += "\"text\":\"" + d[textField].ToString() + "\",";
                //JsonData += "\"id\":\"" + d[idField].ToString() + "\"},";
                JsonData += "\"id\":\"" + d[idField].ToString() + "\",";
                JsonData += "\"text1\":\"" + d[textField1].ToString() + "\",";
                JsonData += "\"text2\":\"" + d[textField2].ToString() + "\"},";
            }
            if (JsonData.Length > 1)
                JsonData = JsonData.Remove(JsonData.Length - 1);
            JsonData += "]";
            return JsonData;
        }
        ///end
        public static string ToJsonForTree(this DataTable dt, string idField, string textField, string parentIdField, string rootIdValue, string urlField)
        {
            string JsonData = "[";
            DataRow[] drs = dt.Select(parentIdField + "='" + rootIdValue + "'");
            foreach (var d in drs)
            {
                JsonData += "{";

                JsonData += "\"text\":\"" + d[textField].ToRequestString().CleanJson() + "\",";
                JsonData += "\"id\":\"" + d[idField].ToString() + "\",";

                string url = "";
                if (!string.IsNullOrEmpty(urlField) && !urlField.Contains('#'))
                {
                    url = d[urlField].ToString();
                }
                else
                    url = urlField.Remove(0, 1);
                JsonData += "\"attributes\":{\"url\":\"" + url.ToRequestString().CleanJson() + "\"},";

                DataRow[] childdrs = dt.Select(parentIdField + "='" + d[idField] + "'");
                if (childdrs.Count() > 0)
                {
                    JsonData += "\"children\":";
                    JsonData += ToJsonForTree(dt, idField, textField, parentIdField, d[idField].ToString(), urlField);
                    JsonData += "},";
                }
                else
                {
                    JsonData = JsonData.Remove(JsonData.Length - 1);
                    JsonData += "},";
                }
            }
            if (JsonData.Length > 1)
                JsonData = JsonData.Remove(JsonData.Length - 1);
            JsonData += "]";
            return JsonData;
        }

        public static string ToJsonForTree(this DataTable dt, string idField, string textField, string parentIdField, string rootIdValue, string urlField,string enableField)
        {
            string JsonData = "[";
            DataRow[] drs = dt.Select(parentIdField + "='" + rootIdValue + "'");
            foreach (var d in drs)
            {
                JsonData += "{";
                //最终图标文件
                string icon = "";
                //关键节点图标
                string key = d["keyNode"].ToRequestString(); //关键节点
                string status = d["runstatus"].ToRequestString();//状态节点 1表示已通过 0表示可点击但未通过 null表示不可点击
                //如果是关键节点
                if (key == "1")
                {
                    //关键节点 状态为通过 显示绿色图标
                    if (status == "1")
                    {
                        icon = "icon-projectpass";
                    }
                    else
                    {
                        //不可点击 显示灰色图标
                        if (status == "")
                        {
                            icon = "icon-projectnotekeylocked";
                        }
                        else
                        {
                            //可点击未通过显示亮起图标
                            icon = "icon-projectnotekey";
                        }
                    }
                }
                else
                {
                    //常规节点 不可点击 显示灰色图标
                    if (status == "")
                        icon = "icon-projectnotelocked";
                    else
                        icon = "icon-projectnote";
                }
                
                               
                JsonData += "\"text\":\"" + d[textField].ToRequestString().CleanJson() + "\",";
                JsonData += "\"id\":\"" + d[idField].ToString() + "\",";
                JsonData += "\"iconCls\":\"" + icon + "\",";
                string url = "";
                //如果状态为空 说明不允许点击 则禁用链接
                if (!string.IsNullOrEmpty(status))
                {
                    if (!string.IsNullOrEmpty(urlField) && !urlField.Contains('#'))
                    {
                        url = d[urlField].ToString();
                    }
                    else
                        url = urlField.Remove(0, 1);
                }                
                JsonData += "\"attributes\":{\"url\":\"" + url.ToRequestString().CleanJson() + "\"},";

                DataRow[] childdrs = dt.Select(parentIdField + "='" + d[idField] + "'");
                if (childdrs.Count() > 0)
                {
                    JsonData += "\"children\":";
                    JsonData += ToJsonForTree(dt, idField, textField, parentIdField, d[idField].ToString(), urlField,enableField);
                    JsonData += "},";
                }
                else
                {
                    JsonData = JsonData.Remove(JsonData.Length - 1);
                    JsonData += "},";
                }
            }
            if (JsonData.Length > 1)
                JsonData = JsonData.Remove(JsonData.Length - 1);
            JsonData += "]";
            return JsonData;
        }
        

        public static string ToJson<T>(this T data)
        {

            try
            {

                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(data.GetType());

                using (MemoryStream ms = new MemoryStream())
                {

                    serializer.WriteObject(ms, data);

                    return Encoding.UTF8.GetString(ms.ToArray());

                }

            }

            catch
            {

                return null;

            }

        }    

        #endregion

        #region 自定义扩展一个DistinctBy

        /// <summary>
        /// 自定义DISTINCTBY方法
        /// </summary>
        /// <typeparam name="TSource">源对象泛型</typeparam>
        /// <typeparam name="TKey">键名泛型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="keySelector">键名</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        #endregion

        #region "将Datatable输出为html表格 注意大数据请不要使用该方法 尽量使用分页datatable调用"
        public static string ToHtmlTable(this DataTable dt)
        {
            string html = "<table class=\"htmlgrid\">";
            html += "<tr>";
            foreach (DataColumn dc in dt.Columns)
            {                
                html += "<th>"+dc.ColumnName+"</th>";                
            }
            html += "</tr>";
            
            foreach (DataRow dr in dt.Rows)
            {
                html += "<tr>";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    html += "<td>" + dr[i] + "</td>";
                }
                html += "</tr>";
            }
            
            html += "</table>";
            return html;
        }

        /// <summary>
        /// 将IList 转换为htmltable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToHtmlTable<T>(this IList<T> list)
        {
            string html = "";
            if (list.Count > 0)
            {
                html += "<table class=\"htmlgrid\">";
                html += "<tr>";
                System.Reflection.PropertyInfo[] props = list[0].GetType().GetProperties();
                foreach (var p in props)
                {
                    html += "<th>" + p.Name + "</th>";
                }
                html += "</tr>";
                foreach (var q in list)
                {
                    html += "<tr>";
                    for(int i=0;i<props.Count();i++)
                    {
                        html += "<td>" + GetPropertyValue(q,props[i].Name) + "</td>";
                    }
                    html += "</tr>";
                }
                html += "</table>";
            }
            
            return html;
        }

        public static object GetPropertyValue(object info, string field)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            return property.First().GetValue(info, null);
        } 

        #endregion
 
        #region "字符串扩展类"

        /// <summary>
        /// 将数据库中的文件路径描述转换为HTML
        /// </summary>
        /// <param name="dbFilesString">文件名*文件路径|文件名*文件路径.....</param>
        /// <returns></returns>
        public static string ToAttachHtml(this string dbFilesString)
        {
            string html = "<ul>";
            if (!string.IsNullOrEmpty(dbFilesString))
            {
                string[] str = dbFilesString.Split('|');
                for (int i = 0; i < str.Length; i++)
                {

                    string[] tmp = str[i].Split('*');
                    string ext = System.IO.Path.GetExtension(Utility.GetFileExt(tmp.Last()));                   
                    html += string.Format("<li><a class=\"easyui-linkbutton\" data-options=\"iconCls:'icon-" + ext.Replace(".", "") + "',plain:true\" href=\"{1}\" target='_blank'>{0}</a></li>",
                        tmp.First(),
                        tmp.Last()
                        );
                }
                html += "</ul>";
            }
            return html;
        }

        /// <summary>
        /// 输出一个附件时的HTML显示格式
        /// </summary>
        /// <param name="filePath">附件路径</param>
        /// <returns></returns>
        public static string ToAttachHtmlByOne(this object filePath)
        {
            string str = "<a href='/Module/Kh/FileDownload.aspx?url={0}'><img src='/Resources/OA/site_skin/default/img/page.png'  border=\"0\"/></a>";
            if (string.IsNullOrEmpty(filePath.ToRequestString()))
            {
                return "";
            }
            else
            {
                return string.Format(str, FileHelper.Encrypt(filePath.ToRequestString()));
            }
        }

        /// <summary>
        /// 输出一个附件时的下载显示格式
        /// 文件保存路径：指定目录/年度/report/文件名
        /// </summary>
        /// <param name="dataId">数据ID</param>
        /// <param name="fileRealName">文件真实名称</param>
        /// <param name="year">年度</param>
        /// <returns></returns>
        public static string ToDownloadHtmlByOne(this object dataId, string fileRealName, int year)
        {
            string filePath = WebKeys.ProjectReportPath + "/" + year + "/report/" + dataId + ".doc";
            fileRealName = fileRealName + ".doc";
            string str = "<a href='/Modules/Common/EntDisk/Content/Ashx/FileDownload.ashx?url={0}&fn={1}'><img src='/Resources/Styles/icon/download.png'  border=\"0\"/></a>";
            return string.Format(str, FileHelper.Encrypt(filePath.ToRequestString()),
                    FileHelper.Encrypt(fileRealName));
        }

        /// <summary>
        /// 检测URL路径对应的文件是否存在
        /// 文件保存路径：指定目录/年度/report/文件名
        /// </summary>
        /// <param name="dataId">数据ID</param>
        /// <param name="fileRealName">文件真实名称</param>
        /// <param name="year">年度</param>
        /// <returns></returns>
        public static bool ChkUrlFileIsExists(this object dataId, string fileRealName, int year)
        {
            string filePath = WebKeys.ProjectReportPath + "/" + year + "/report/" + dataId + ".doc";
            return FileControl.ChkUrlFileIsExists(filePath);
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string str)
        {
            CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(str);
        }

        /// <summary>
        /// 字符串转换 如果为null 返回空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToRequestString(this object obj)
        {
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }

        public static DateTime ToDateTime(this object obj)
        {
            DateTime dt = new DateTime();
            if (obj != null)
            {
                DateTime.TryParse(obj.ToRequestString(), out dt);
            }
            return dt;
        }

        public static DateTime? ToDateTimeNullabel(this object obj)
        {
            DateTime dt = new DateTime();
            if (obj.ToRequestString() != "")
            {              
                
                DateTime.TryParse(obj.ToRequestString(), out dt);                
                return dt;
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// 输出日期格式字串
        /// 格式：yyyy-MM-dd
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToDateYMDFormat(this object obj)
        {
            string ymd = string.Empty;
            if (obj != null)
            {
                DateTime dt = new DateTime();
                DateTime.TryParse(obj.ToRequestString(), out dt);
                if (dt != null && dt.Year > 1)
                {
                    ymd = dt.ToString("yyyy-MM-dd");
                }
            }
            return ymd;
        }

        /// <summary>
        /// 输出日期格式字串
        /// 格式：formatter
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public static string ToDateYMDFormat(this object obj, char formatter)
        {
            string ymd = string.Empty;
            if (obj != null)
            {
                DateTime dt = new DateTime();
                DateTime.TryParse(obj.ToRequestString(), out dt);
                if (dt != null && dt.Year > 1)
                {
                    ymd = dt.ToString("yyyy" + formatter + "MM" + formatter + "dd");
                }
            }
            return ymd;
        }

        /// <summary>
        /// 获取字符串的真实长度：1个汉字为2
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static int RealLength(this string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 截取字符串 用...代替
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="intLen"></param>
        /// <returns></returns>
        public static string CutString(this string strInput, int? intLen)
        {            
            if (intLen == null||intLen==0)
                return strInput;
            strInput = (strInput != null) ? strInput.Trim() : "";
            byte[] myByte = System.Text.Encoding.Default.GetBytes(strInput);
            if (myByte.Length > intLen)
            {
                string resultStr = "";
                for (int i = 0; i < strInput.Length; i++)
                {
                    byte[] tempByte = System.Text.Encoding.Default.GetBytes(resultStr);
                    if (tempByte.Length < intLen)
                    {

                        resultStr += strInput.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                return resultStr + "...";
            }
            else
            {
                return strInput;
            }
        }

        /// <summary>
        /// 将字符串按指定的格式分割 返回(1,2,3,4) 方便查询
        /// </summary>
        /// <param name="str"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string IdsToInId(this string str,char split)
        {
            string strWhere = "";
            //获取该栏目所有的文章的数量用于分页程序  
            if (!str.Equals(""))
            {
                string[] ids = str.Split(split);
                foreach (string id in ids)
                {
                    strWhere += id + ",";
                }
                if (strWhere.Length > 0)
                {
                    strWhere = " (" + strWhere.Remove(strWhere.Length - 1) + ")";
                }
            }
            return strWhere;
        }

        /// <summary>
        /// 将对象转换为用字符chr连接的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static string ToJoin<T>(this IList<T> list,char chr)
        {
            string str = "";
            foreach (var q in list)
            {
                str += q.ToRequestString() + chr;
            }
            if (str.Length > 1)
            {
                str = str.TrimEnd(chr);
            }
            return str;
        }

        /// <summary>
        /// 将object转换为Int32,如果转换失败返回null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int? ToNullOrInt(this object str)
        {
            int r=0;
            if (!Object.Equals(str, null))
            {
                if (int.TryParse(str.ToString(), out r))
                {
                    return r;
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 转为字符串（可去除双引号 回车符）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToStr(this object str)
        {
            if (!Object.Equals(str, null))
            {
                string ss = str.ToString().Replace("\"", "\\'");
                ss = ss.Replace("\r\n", "<br/>");
                ss = ss.Replace("<", "&lt;");
                return ss;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 将object转换为Int32,如果转换失败返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this object str)
        {
            int r = 0;
            if (!Object.Equals(str, null))
            {
                if (int.TryParse(str.ToString(), out r))
                {
                    return r;
                }
                else
                    return 0;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 将object转换为Int32,如果转换失败返回null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal? ToNullOrDecimal(this object str)
        {
            decimal r = 0;
            if (!Object.Equals(str, null))
            {
                if (decimal.TryParse(str.ToString(), out r))
                {
                    return r;
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将object转换为Decimal,如果转换失败返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object str)
        {
            decimal r = 0;
            if (!Object.Equals(str, null))
            {
                if (decimal.TryParse(str.ToString(), out r))
                {
                    return r;
                }
                else
                    return 0;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 将object转换为double,如果转换失败返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this object str)
        {
            double r = 0;
            if (!Object.Equals(str, null))
            {
                if (double.TryParse(str.ToString(), out r))
                {
                    return r;
                }
                else
                    return 0;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 去除空格后检测是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToTrim(this object str)
        {
            bool isR = true;

            if (str == null || str.ToString().Trim() == null)
            {
                isR = false;
            }

            return isR;
        }

       

        #endregion

        #region "私有方法"
        /// <summary>
        /// 将Datable中指定页码的数据转换为Json格式
        /// </summary>
        /// <param name="dt">System.Data.Datatable</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        private static string ExcuteResultToJson(DataTable dt, int page, int pageSize)
        {            
            string JsonData="";
            int rowbegin = (page - 1) * pageSize;
            int rowend = page * pageSize;
            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            if (rowbegin <= dt.Rows.Count)
            {
                JsonData = "{\"total\":" + dt.Rows.Count + ",\"rows\":[";
                for (int i = rowbegin; i <= rowend - 1; i++)
                {
                    JsonData += "{";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        string content = Microsoft.JScript.GlobalObject.escape(dt.Rows[i][dc.ColumnName].ToString());
                        JsonData += "\"" + dc.ColumnName + "\":\"" + content + "\",";
                    }
                    JsonData = JsonData.Remove(JsonData.Length - 1);
                    JsonData += "},";

                }
                JsonData = JsonData.Remove(JsonData.Length - 1);
                JsonData += "]}";
            }
            return JsonData;
        }

        /// <summary>
        /// 将DataTable转换为Json格式
        /// </summary>
        /// <param name="dt">System.Data.DataTable</param>
        /// <returns></returns>
        private static string ExcuteResultToJson(DataTable dt)
        {
            string JsonData = "";
            JsonData = "[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                JsonData += "{";
                foreach (DataColumn dc in dt.Columns)
                {
                    string content = dt.Rows[i][dc.ColumnName].ToString().CleanJson();
                    JsonData += "\"" + dc.ColumnName + "\":\"" + content + "\",";
                }
                JsonData = JsonData.Remove(JsonData.Length - 1);
                JsonData += "},";
            }
            JsonData = JsonData.Remove(JsonData.Length - 1);
            JsonData += "]";
            return JsonData;
        }

        /// <summary>
        /// 将DataTable转换为Json格式
        /// </summary>
        /// <param name="dt">System.Data.DataTable</param>
        /// <param name="idField">Value字段</param>
        /// <param name="textField">text字段</param>
        /// <param name="parentIdField">父Id字段</param>
        /// <param name="rootIdValue"></param>
        /// <param name="selectedId">选择Id</param>
        /// <returns></returns>
        private static string ExcuteResultToComboTreeJson(DataTable dt, string idField, string textField, string parentIdField, string rootIdValue,string selectedId)
        {
            string[] selected_ids = selectedId.Split(',');
            string JsonData = "[";
            string tmp = "";
            if (rootIdValue == "")
                tmp = " is null";
            else
                tmp = "='"+ rootIdValue+"'";

            DataRow[] drs = dt.Select(parentIdField + tmp);
            foreach (var d in drs)
            {
                DataRow[] childdrs = dt.Select(parentIdField + "='" + d[idField] + "'");
                JsonData += "{";
                JsonData += "\"id\":\"" + d[idField].ToRequestString() + "\",";
                if (selected_ids.Contains(d[idField].ToRequestString()))
                    JsonData += "\"checked\":true,";
                JsonData += "\"text\":" + "\"" + d[textField].ToRequestString() + "\"";
                if (childdrs.Count() > 0)
                {
                    JsonData += ",\"children\":";
                    JsonData += ExcuteResultToComboTreeJson(dt, idField, textField, parentIdField, d[idField].ToString(), selectedId);
                    JsonData += "},";
                }
                else
                {
                    JsonData += "},";
                }
            }
            JsonData = JsonData.Remove(JsonData.Length - 1);
            JsonData += "]";
            return JsonData;
        }

        #endregion

        #region 将领导签字转换成图片

        /// <summary>
        /// 电子签名
        /// </summary>
        /// <param name="username">rtx登录名</param>
        /// <param name="top">签名位置距离页面顶端</param>
        /// <param name="left">距离页面左边</param>
        /// <param name="width">图片宽度，高度按比例自动缩放</param>
        /// <returns></returns>
        public static string QianMing(this string username,int top,int left,int width)
        {
            //签名图片路径
            string imgurl="\\Styles\\images\\qianming\\"+username+".gif";
            return "<div style='position:relative;top:" + top + "px;left:" + left + "px'><img src='" + imgurl + "' alt='' border=0 width=" + width + "/></div>";
        }

        #endregion      

        #region "将List<string> 转换为 in (string,string)"
        /// <summary>
        /// 将List (/string) 转换为 in (string,string)
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns>(0,string,string)</returns>
        public static string ToInSQLString(this List<string> Ids)
        {
            string ids = "('0',";
            foreach (string id in Ids)
            {
                ids += "'" + id + "',";
            }
            ids = ids.Remove(ids.Length - 1);
            ids += ")";
            return ids;
        }

        /// <summary>
        /// 将List (/string) 转换为 string,string
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns>string,string</returns>
        public static string ToEnableSplitString(this List<string> Ids)
        {
            string ids = "";
            foreach (string id in Ids)
            {
                ids += "" + id + ",";
            }
            ids = ids.Remove(ids.Length - 1);
            return ids;
        }

        #endregion

        #region "GridView选择行的ID"
        /// <summary>
        /// 返回gridview选择的ID集合
        /// </summary>
        /// <param name="gv">gridview</param>
        /// <param name="IdIndex">ID所在的列索引</param>
        /// <param name="checkboxPreId">Checkbox的ID前缀</param>
        /// <param name="checkbox_index">Checkbox的列索引</param>
        /// <returns></returns>
        public static List<string> GetSelectedId(this GridView gv, int IdIndex, string checkboxPreId, int checkbox_index)
        {
            List<string> MessageIds = new List<string>();
            foreach (GridViewRow row in gv.Rows)
            {
                CheckBox cb = (CheckBox)row.Cells[checkbox_index].FindControl(checkboxPreId);
                if (cb.Checked)
                {
                    MessageIds.Add(row.Cells[IdIndex].Text.ToString());
                }
            }
            return MessageIds;
        }
        #endregion

        #region DropDownList Year 数据初始化 GridView数据初始化
        /// <summary>
        /// 下拉菜单年份初始化
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="startYear">起始年</param>
        /// <param name="endYear">终止年</param>
        /// <param name="selectedYear">选中年</param>
        public static void BindYears(this DropDownList ddl,int startYear,int endYear,int selectedYear)
        {
            ddl.Items.Clear();
            for (int i = startYear; i <= endYear; i++)
            {
                ddl.Items.Add(new ListItem(i.ToString()));
            }
            ListItem liyear = ddl.Items.FindByValue(selectedYear.ToString());
            if (liyear != null)
                liyear.Selected = true;
        }


        public static void BindList(this DropDownList ddl, IList<object> list, string textField, string valueField)
        {
            ddl.DataSource = list;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
        }
        #endregion
        
        #region DataReader转换泛型集合
        /// <summary>
        /// 将DataReader转换成泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IList<T> ExecuteReaderList<T>(this DbDataReader reader)
        {
            try
            {
                IList<T> list = ToList<T>(reader);
                return list;
            }
            finally { }
        }

        private static IList<T> ToList<T>(DbDataReader reader)
        {
            Type type = typeof(T);
            IList<T> list = null;
            if (type.IsValueType || type == typeof(string))
                list = CreateValue<T>(reader, type);
            else
                list = CreateObject<T>(reader, type);
            reader.Dispose();
            reader.Close();
            return list;
        }

        private static IList<T> CreateObject<T>(DbDataReader reader, Type type)
        {
            IList<T> list = new List<T>();
            PropertyInfo[] properties = type.GetProperties();
            string name = string.Empty;
            while (reader.Read())
            {
                T local = Activator.CreateInstance<T>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    name = reader.GetName(i);
                    foreach (PropertyInfo info in properties)
                    {
                        if (name.Equals(info.Name))
                        {
                            //某些时候手工修改数据库可能发生不可转换的数据类型
                            if (reader[info.Name] != DBNull.Value)
                            {
                                info.SetValue(local, Convert.ChangeType(reader[info.Name], info.PropertyType), null);
                            }
                            break;
                        }
                    }
                }
                list.Add(local);
            }
            return list;
        }

        private static IList<T> CreateValue<T>(DbDataReader reader, Type type)
        {
            IList<T> list = new List<T>();
            while (reader.Read())
            {
                T local = (T)Convert.ChangeType(reader[0], type, null);
                list.Add(local);
            }
            return list;
        }

        #endregion

        #region  List->Datatable 转换
        public static DataTable ToDataTable<T>(this IList<T> entitys)
        {
            if (entitys == null || entitys.Count <= 0)
            {
                return null;
            }
            DataTable dtReturn = new DataTable();

            // column names

            PropertyInfo[] oProps = null;

            // Could add a check to verify that there is an element 0

            foreach (T rec in entitys)
            {

                // Use reflection to get property names, to create table, Only first time, others will follow

                if (oProps == null)
                {

                    oProps = ((Type)rec.GetType()).GetProperties();

                    foreach (PropertyInfo pi in oProps)
                    {

                        Type colType = pi.PropertyType; if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {

                            colType = colType.GetGenericArguments()[0];

                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));

                    }

                }

                DataRow dr = dtReturn.NewRow(); foreach (PropertyInfo pi in oProps)
                {

                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);

                }

                dtReturn.Rows.Add(dr);

            }

            return (dtReturn);

        }
        #endregion

        #region DateTime类型转换

        /// <summary>
        /// 周一
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToThisWeekFirstDay(this DateTime dt)
        {
            return dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));
        }
        /// <summary>
        /// 周日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToThisWeekLastDay(this DateTime dt)
        {
            return dt.ToThisWeekFirstDay().AddDays(6);
        }
        /// <summary>
        /// 当年第一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToThisYearFirstDay(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1); 
        }

        /// <summary>
        /// 当年最后一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToThisYearLastDay(this DateTime dt)
        {
            return new DateTime(dt.Year, 12, 31); 
        }

        /// <summary>
        /// 月初
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToThisMonthFirstDay(this DateTime dt)
        {
            return dt.AddDays(1 - dt.Day);  //            
        }

        /// <summary>
        /// 月末
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToThisMonthLastDay(this DateTime dt)
        {
            return dt.ToThisMonthFirstDay().AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 季度初
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToThisQuarterFirstDay(this DateTime dt)
        {
            return dt.AddMonths(0 - (dt.Month - 1) % 3).AddDays(1 - dt.Day);
        }

        /// <summary>
        /// 季度末
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToThisQuarterLastDay(this DateTime dt)
        {
            return dt.ToThisQuarterFirstDay().AddMonths(3).AddDays(-1);  
        }



        #endregion

    }

}
