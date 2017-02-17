using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Component.Infrastructure;
using System.Text;

namespace Enterprise.UI.Web.Module.Kh
{
    /// <summary>
    /// 财务基础数据业务处理类
    /// </summary>
    public class KhJcsjDataHandler : IHttpHandler
    {
        /// <summary>
        /// 财务基础数据服务类
        /// </summary>
        KhCwjcsjService cwjcsjSrv = new KhCwjcsjService();

        public void ProcessRequest(HttpContext context)
        {
            string resultStr = string.Empty;
            context.Response.ContentType = "text/plain";
            string jgbm = context.Request.QueryString["BM"];
            string nf = context.Request.QueryString["NF"];
            string type = context.Request.QueryString["LX"];
            string data = context.Request.QueryString["DATA"];
            switch (type)
            {
                case "Select"://查询
                    resultStr = getCwjcsjDataJSON(jgbm, nf.ToInt());
                    break;
                case "Save"://保存
                    resultStr = saveCwjcsjData(jgbm, nf.ToInt(), data);
                    break;
            }
            context.Response.Write(resultStr);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 专用方法区

        /// <summary>
        /// 获取财务基础数据
        /// </summary>
        /// <param name="jgbm">单位编码</param>
        /// <param name="nf">年份</param>
        /// <returns></returns>
        private string getCwjcsjDataJSON(string jgbm, int nf)
        {
            StringBuilder json = new StringBuilder();
            var list = cwjcsjSrv.GetListByJgbmAndNF(jgbm, nf);
            json.Append("{\"total\":" + list.Count + ",\"rows\":[");//CharsetConverter.ToUnicode()
            foreach (var jcsj in list)
            {
                json.Append("{\"xh\": \"" + jcsj.XH + "\",\"zbmc\": \"" + jcsj.ZBMC
                    + "\",\"m1\": \"" + jcsj.M1.ToDecimal() + "\",\"m2\": \"" + jcsj.M2.ToDecimal() + "\",\"m3\": \"" + jcsj.M3.ToDecimal() + "\",\"m4\": \""
                    + jcsj.M4.ToDecimal() + "\",\"m5\": \"" + jcsj.M5.ToDecimal() + "\",\"m6\": \"" + jcsj.M6.ToDecimal()
                    + "\",\"m7\": \"" + jcsj.M7.ToDecimal() + "\",\"m8\": \"" + jcsj.M8.ToDecimal() + "\",\"m9\": \""
                    + jcsj.M9.ToDecimal() + "\",\"m10\": \"" + jcsj.M10.ToDecimal() + "\",\"m11\": \"" + jcsj.M11.ToDecimal()
                    + "\",\"m12\": \"" + jcsj.M12.ToDecimal()
                    + "\",\"TJZ\": \"" + (jcsj.ZBMC.Contains("累计") ? jcsj.LJZ.ToDecimal() : jcsj.PJZ.ToDecimal()) + "\"},");
            }
            return json.ToString().TrimEnd(',') + "]}";
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="jgbm">单位编码</param>
        /// <param name="nf">年份</param>
        /// <param name="data">数据串</param>
        /// <returns></returns>
        private string saveCwjcsjData(string jgbm, int nf, string data)
        {
            return "";
        }

        #endregion
    }
}