using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Module.Kh
{
    /// <summary>
    /// 与考核相关的数据提取操作
    /// </summary>
    public class KhDataHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string resultStr = string.Empty;
            context.Response.ContentType = "text/plain";
            string khid = context.Request.QueryString["KH"];
            string jgbm = context.Request.QueryString["BM"];
            string zbbm = context.Request.QueryString["ZB"];
            string type = context.Request.QueryString["LX"];
            string v = context.Request.QueryString["PV"];
            switch (type)
            {
                case "ZF1"://上级
                    resultStr = getJgzfDafenJSON(khid, jgbm, zbbm, WebKeys.DFUserType.上级领导);
                    break;
                case "ZF2"://同级部门
                    resultStr = getJgzfDafenJSON(khid, jgbm, zbbm, WebKeys.DFUserType.同级部门);
                    break;
                case "ZF3"://同级二级单位
                    resultStr = getJgzfDafenJSON(khid, jgbm, zbbm, WebKeys.DFUserType.同级二级单位);
                    break;
                case "ZDGZ"://重点工作
                    resultStr = getZdgzAndBmlzDafenJSON(khid, jgbm, "重点工作");
                    break;
                case "BMLZ"://部门履职
                    resultStr = getZdgzAndBmlzDafenJSON(khid, jgbm, "部门履职");
                    break;
                case "LX2014A"://二级单位考核
                    resultStr = getKaoheProcessJSON(type, v);
                    break;
                case "LX2014B"://机关部门考核
                    resultStr = getKaoheProcessJSON(type, v);
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

        #region 自定义方法

        /// <summary>
        /// 生成指定考核类型的节点信息
        /// </summary>
        /// <param name="khlx">考核类型</param>
        /// <param name="v">值</param>
        /// <returns></returns>
        private string getKaoheProcessJSON(string khlx, string v) 
        {
            StringBuilder json = new StringBuilder();
            KhNodesService nodeSrv = new KhNodesService();
            var list = nodeSrv.GetList().Where(p => p.LXID == khlx).OrderBy(p => p.JDXH).ToList();
            if (list != null)
            {
                json.Append("{\"total\":" + list.Count + ",\"rows\":[");
                foreach (var node in list)
                {
                    json.Append("{\"name\":\"" + node.JDMC + "\",\"progress\":" + ((node.JDZ <= v.ToInt()) ? "100" : "0") + "},");
                }
            }
            return json.ToString().TrimEnd(',') + "]}";
        }

        /// <summary>
        /// 获取机关作风建设得分情况
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="jgbm">机构编码</param>
        /// <param name="zbbm">指标编码</param>
        /// <param name="dfUserType">打分者类型</param>
        /// <returns></returns>
        private string getJgzfDafenJSON(string khid, string jgbm, string zbbm, WebKeys.DFUserType dfUserType)
        {
            StringBuilder json = new StringBuilder();
            //json.Append("{\"total\":2,\"rows\":["
            //     + "{\"XH\":\"01\",\"DWMC\":\"财务\",\"ZBMC\":\"aaa\",\"DF\":\"1.2\"},"
            //     + "{\"XH\":\"02\",\"DWMC\":\"财务22\",\"ZBMC\":\"bbbb\",\"DF\":\"0.8\"}");
            KhJgbmdfbService jgbmdfSrv = new KhJgbmdfbService();
            ZbkDfzbService dfzbSrv = new ZbkDfzbService();
            var list = jgbmdfSrv.GetDfzbListByKhid(khid).
                Where(p=>p.ZBBM == zbbm && p.JGBM == jgbm.ToInt() && p.DFZLX == ((int)dfUserType).ToString()).ToList();
            if (list != null)
            {
                int index = 1;
                string zbmc = dfzbSrv.GetSingle(zbbm).Zbxx.ZBMC;
                json.Append("{\"total\":" + list.Count + ",\"rows\":[");
                foreach (var zb in list)
                {
                    string dfz = (zb.DFZ.ToInt() > 0) ? SysBmjgService.GetBmjgName(zb.DFZ.ToInt()) : SysUserService.GetUserNameByLoginId(zb.DFZ);
                    json.Append("{\"XH\":\"" + (index++) + "\",\"DWMC\":\"" + dfz
                        + "\",\"ZBMC\":\"" + zbmc + "\",\"DF\":\"" + zb.KHDF + "\"},");
                }
            }
            return json.ToString().TrimEnd(',') + "]}";
        }

        /// <summary>
        /// 获取重点工作和部门履职得分情况
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="jgbm">机构编码</param>
        /// <param name="zbmc">指标名称</param>
        /// <returns></returns>
        private string getZdgzAndBmlzDafenJSON(string khid, string jgbm, string zbmc)
        {
            StringBuilder json = new StringBuilder();
            //json.Append("{\"total\":2,\"rows\":["
            //     + "{\"XH\":\"01\",\"DWMC\":\"财务1\",\"LD\":\"100\",\"TJ\":\"1.2\",\"ZP\":\"1.2\"},"
            //     + "{\"XH\":\"02\",\"DWMC\":\"财务2\",\"LD\":\"100\",\"TJ\":\"1.2\",\"ZP\":\"1.2\"}");
            KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
            var q = dlzbmxSrv.GetListByKhidAndJgbm(khid, jgbm).
                Where(p => p.ZbsxModel.JxzrsZb.Lhzb != null && 
                    p.ZbsxModel.JxzrsZb.Lhzb.Zbxx.ZBMC.Contains(zbmc)).FirstOrDefault();
            KhJgbmdfbService jgbmdfSrv = new KhJgbmdfbService();//打分表
            if (q != null)
            {
                int index = 1;
                var jhbmdfLst = q.KhJgbmdfbLst;//获取所有打分者
                var list = jgbmdfSrv.GetDlzbListByKhid(khid).Where(p=>p.JGBM == jgbm.ToInt()).ToList();
                json.Append("{\"total\":" + jhbmdfLst.Count + ",\"rows\":[");
                foreach (var zb in jhbmdfLst)
                {
                    string dfz = string.Empty;
                    decimal khdf = list.Where(p => p.DFZ == zb.DFZ && p.ZBBM == zb.ZBBM
                        ).Sum(p => p.KHDF).Value;//各小指标合计得分
                    if (zb.DFZLX == "1")
                    {
                        //上级
                        dfz = SysUserService.GetUserNameByLoginId(zb.DFZ);
                        json.Append("{\"XH\":\"" + (index++) + "\",\"DWMC\":\"" + dfz
                        + "\",\"LD\":\"" + khdf + "\",\"TJ\":\"\",\"ZP\":\"\"},");
                    }
                    else if (zb.DFZLX == "2")
                    {
                        //同级部门
                        if (zb.DFZ == jgbm)
                        {
                            //自评
                            dfz = SysBmjgService.GetBmjgName(jgbm.ToInt());
                            json.Append("{\"XH\":\"" + (index++) + "\",\"DWMC\":\"" + dfz
                            + "\",\"LD\":\"\",\"TJ\":\"\",\"ZP\":\"" + khdf + "\"},");
                        }
                        else
                        {
                            //互评
                            dfz = SysBmjgService.GetBmjgName(zb.DFZ.ToInt());
                            json.Append("{\"XH\":\"" + (index++) + "\",\"DWMC\":\"" + dfz
                            + "\",\"LD\":\"\",\"TJ\":\"" + khdf + "\",\"ZP\":\"\"},");
                        }
                    }
                    else if (zb.DFZLX == "3")
                    {
                        //同级二级单位
                        dfz = SysBmjgService.GetBmjgName(zb.DFZ.ToInt());
                        json.Append("{\"XH\":\"" + (index++) + "\",\"DWMC\":\"" + dfz
                        + "\",\"LD\":\"\",\"TJ\":\"" + khdf + "\",\"ZP\":\"\"},");
                    }                    
                }
            }
            return json.ToString().TrimEnd(',') + "]}";
        }

        #endregion

    }
}