using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;
using System.Text;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Module.Zbk
{
    /// <summary>
    /// 后端数据处理页面
    /// </summary>
    public class WebDataHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string resultStr = string.Empty;
            context.Response.ContentType = "text/plain";
            string zblx = context.Request.QueryString["zblx"];
            string zbmc = context.Request.QueryString["zbmc"];
            resultStr = getZhibiaoJSON(zblx, zbmc); 
            context.Response.Write(resultStr);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 自定义方法区

        /// <summary>
        /// 获取相同类型的所有指标
        /// </summary>
        /// <param name="zblx">类型</param>
        /// <param name="zbmc">指标名称</param>
        /// <returns></returns>
        private string getZhibiaoJSON(string zblx,string zbmc)
        {
            StringBuilder json = new StringBuilder();
            //"{\"total\":2,\"rows\":["
            //     + "{\"ZBID\":\"FI-SW-01\",\"ZBMC\":\"生产经营总收入\",\"YJZBMC\":\"P\",\"EJZBMC\":\"P\",\"SJZBMC\":\"Large\"},"
            //     + "{\"ZBID\":\"K9-DL-01\",\"ZBMC\":\"利润\",\"YJZBMC\":\"P\",\"EJZBMC\":\"P\",\"SJZBMC\":\"Spotted Adult Female\"}]}";
            ZbkZbxxService zbxxSrv = new ZbkZbxxService();
            zblx = (zblx == "01" ? "定量指标" : "");
            var zblist = zbxxSrv.GetListForValid(zblx);
            if (!string.IsNullOrEmpty(zbmc))
            {
                zblist = zblist.Where(p => p.ZBMC.Contains(zbmc)).ToList();
            }
            if (zblist != null)
            {
                json.Append("{\"total\":" + zblist.Count + ",\"rows\":[");
                foreach (var zb in zblist)
                {
                    json.Append("{\"ZBID\":\"" + zb.ZBID + "\",\"ZBMC\":\"" + zb.ZBMC + "\",\"YJZBMC\":\"" 
                        + zb.YJZBMC + "\",\"EJZBMC\":\"" + zb.EJZBMC + "\",\"SJZBMC\":\"" + zb.SJZBMC + "\"},");
                }
            }
            return json.ToString().TrimEnd(',') + "]}";
        }

        #endregion

    }
}