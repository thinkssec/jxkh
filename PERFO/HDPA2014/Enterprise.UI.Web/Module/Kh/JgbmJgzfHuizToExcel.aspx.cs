using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
    /// 机关作风建设数据表导出页面
    /// </summary>
    public partial class JgbmJgzfHuizToExcel : System.Web.UI.Page
    {

        /// <summary>
        /// 机关作风服务类
        /// </summary>
        KhJgzfbService jgzfSrv = new KhJgzfbService();
        KhSjsbService sjsbSrv = new KhSjsbService();//数据上报服务类
        KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//打分指标明细服务类
        KhKhglService khglSrv = new KhKhglService();//考核管理

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected KhKhglModel Kaohe = null;//考核期
        protected List<KhJgzfbModel> JgzfbList = null;//数据集
        //string content = "<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"></head><body><table>aaa</table></body></html>";

        protected void Page_Load(object sender, EventArgs e)
        {

            //输出的应用类型
            Response.ContentType = "application/vnd.ms-excel";
            //设定编码方式，若输出的excel有乱码，可优先从编码方面解决Response.Charset = "gb2312";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            //关闭ViewState，此属性在Page中
            EnableViewState = false;
            //filenames是自定义的文件名
            Response.AppendHeader("Content-Disposition", "attachment;filename=jgzf_data.xls");
            


            ////content是步骤1的html，注意是string类型
            //Response.Write(content);
            //Response.End();
        }

        #region 专用方法区

        /// <summary>
        /// 获取机关部门的考核结果表格
        /// </summary>
        /// <returns></returns>
        protected string GetJgbmTable()
        {
            if (string.IsNullOrEmpty(Khid)) 
                return "";
            
            StringBuilder sb = new StringBuilder();
            //获取数据
            JgzfbList = jgzfSrv.GetListByKhid(Khid) as List<KhJgzfbModel>;
            List<KhJgzfbModel> bmjgJgzfList = JgzfbList.DistinctBy(p => p.JGBM).OrderBy(p => p.ZFPM).
                ThenBy(p => p.Bmjg.BZ).ToList();//形成各部门只一条记录的集合

            var hpList = JgzfbList.
                Where(p => p.DFZLX == ((int)WebKeys.DFUserType.同级部门).ToString()).
                DistinctBy(p => p.ZBBM).OrderBy(p => p.DFZBXH).ToList();
            int hpColspan = hpList.Count();

            var ejdwList = JgzfbList.
                Where(p => p.DFZLX == ((int)WebKeys.DFUserType.同级二级单位).ToString()).
                DistinctBy(p => p.ZBBM).OrderBy(p => p.DFZBXH).ToList();
            int ejdwColspan = ejdwList.Count();

            var ldList = JgzfbList.
                Where(p => p.DFZLX == ((int)WebKeys.DFUserType.上级领导).ToString()).
                DistinctBy(p => p.ZBBM).OrderBy(p => p.DFZBXH).ToList();
            int ldColspan = ldList.Count();

            //生成表头-----------------------------------------------------------
            sb.Append("<table><tr><td colspan=\"" + (hpColspan + ejdwColspan + ldColspan + 5) + "\" style=\"font-family:Times New Roman;font-size:11.0pt;font-weight:700;text-align:center;vertical-align:middle;height:45px;\">机关作风建设打分汇总表</td></tr></table>");
            //表头
            sb.Append("<table class=\"tb\" id=\"Table4\" border=\"1\">");
            sb.Append("<tr>");
            sb.Append("<th style=\"width: 40px;\" class=\"td-bold\" rowspan=\"2\">序号</th>");
            sb.Append("<th class=\"td-bold\" rowspan=\"2\">单位名称</th>");
            sb.Append("<th class=\"td-bold\" " + ((hpColspan > 1) ? " colspan=\"" + hpColspan + "\" " : "") + ">作风建设（互评）</th>");
            sb.Append("<th class=\"td-bold\" " + ((ejdwColspan > 1) ? " colspan=\"" + ejdwColspan + "\" " : "") + ">作风建设（二级单位）</th>");
            sb.Append("<th class=\"td-bold\" " + ((ldColspan > 1) ? " colspan=\"" + ldColspan + "\" " : "") + ">作风建设（领导）</th>");
            sb.Append("<th class=\"td-bold\" rowspan=\"2\">得分汇总</th>");
            sb.Append("<th class=\"td-bold\" rowspan=\"2\">排名</th>");
            sb.Append("<th class=\"td-bold\" rowspan=\"2\">加分</th>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            foreach (var hp in hpList)
            {
                sb.Append("<th class=\"td-bold\">" + hp.ZBMC + "</th>");
            }
            foreach (var ejdw in ejdwList)
            {
                sb.Append("<th class=\"td-bold\">" + ejdw.ZBMC + "</th>");
            }
            foreach (var ld in ldList)
            {
                sb.Append("<th class=\"td-bold\">" + ld.ZBMC + "</th>");
            }
            sb.Append("</tr>");

            //生成数据================================================================
            int index = 1;
            foreach (var jgzf in bmjgJgzfList)
            {
                var bmjgHpLst = JgzfbList.Where(p => p.JGBM == jgzf.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.同级部门).ToString()).OrderBy(p => p.DFZBXH).ToList();//互评
                var bmjgEjdwLst = JgzfbList.Where(p => p.JGBM == jgzf.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.同级二级单位).ToString()).OrderBy(p => p.DFZBXH).ToList();//二级单位
                var bmjgLdLst = JgzfbList.Where(p => p.JGBM == jgzf.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.上级领导).ToString()).OrderBy(p => p.DFZBXH).ToList();//上级
                //数据
                sb.Append("<tr>");
                sb.Append("<th>" + (index++) + "</th>");
                sb.Append("<th>" + (SysBmjgService.GetBmjgName(jgzf.JGBM)) + "</th>");

                //可进行编辑操作
                foreach (var hp in bmjgHpLst)
                {
                    sb.Append("<th>" + hp.ZFKHDF + "</th>");
                }
                foreach (var ejdw in bmjgEjdwLst)
                {
                    sb.Append("<th>" + ejdw.ZFKHDF + "</th>");
                }
                foreach (var ld in bmjgLdLst)
                {
                    sb.Append("<th>" + ld.ZFKHDF + "</th>");
                }

                sb.Append("<th>" + jgzf.ZDF + "</th>");
                sb.Append("<th>" + jgzf.ZFPM + "</th>");
                sb.Append("<th>" + jgzf.SJDF + "</th>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        #endregion
    }
}