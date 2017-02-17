using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Component.Infrastructure;
using System.Text;

namespace Enterprise.UI.Web.Kh
{

    /// <summary>
    /// 机关作风建设打分汇总页面
    /// </summary>
    public partial class JgbmJgzfHuiz : PageBase
    {

        /// <summary>
        /// 机关作风服务类
        /// </summary>
        KhJgzfbService jgzfSrv = new KhJgzfbService();
        KhSjsbService sjsbSrv = new KhSjsbService();//数据上报服务类
        KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//打分指标明细服务类
        KhKhglService khglSrv = new KhKhglService();//考核管理

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected string Zfid = (string)Utility.sink("ZFID", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//ID
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构
        protected KhKhglModel Kaohe = null;//考核期
        protected List<KhJgzfbModel> JgzfbList = null;//数据集

        #region 权限检查

        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            base.PermissionHandler += Page_PermissionHandler;
        }

        void Page_PermissionHandler(PageBase.PermissionEventArgs e)
        {
            if (e.Model != null)
            {
                LnkBtn_InitData.Visible = LnkBtn_Calculate.Visible
                   = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.View);
                bool isAudit = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Audit);
                LnkBtn_Edit.Visible = isAudit;
            }
            if (e.UserPm != null)
            {
                LnkBtn_InitData.Visible = LnkBtn_Calculate.Visible
                   = Utility.CheckPermission(Convert.ToInt64(e.UserPm.MODULEPERMISSION), (long)WebKeys.PermissionType.View);
                bool isAudit = Utility.CheckPermission(Convert.ToInt64(e.UserPm.MODULEPERMISSION), (long)WebKeys.PermissionType.Audit);
                LnkBtn_Edit.Visible = isAudit;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDdl();
                BindGrid();
                TabTitle = "机关部门";
            }
        }

        #region 专用方法区

        /// <summary>
        /// 获取机关部门的考核结果表格
        /// </summary>
        /// <returns></returns>
        protected string GetJgbmTable()
        {
            StringBuilder sb = new StringBuilder();
            //获取数据
            JgzfbList = jgzfSrv.GetListByKhid(Ddl_Kaohe.SelectedValue) as List<KhJgzfbModel>;
            if (Jgbm > 0)
            {
                JgzfbList = JgzfbList.Where(p => p.JGBM == Jgbm).OrderBy(p => p.DFZBXH).ToList();
                Pnl_Edit.Visible = LnkBtn_InitData.Visible;
            }
            List<KhJgzfbModel> bmjgJgzfList = JgzfbList.DistinctBy(p => p.JGBM).OrderBy(p=>p.ZFPM).
                ThenBy(p => p.Bmjg.BZ).ToList();//形成各部门只一条记录的集合

            var hpList = JgzfbList.
                Where(p => p.DFZLX == ((int)WebKeys.DFUserType.同级部门).ToString()).
                DistinctBy(p => p.ZBBM).OrderBy(p=>p.DFZBXH).ToList();
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
            //表头
            sb.Append("<tr>");
            sb.Append("<th style=\"width: 40px;\" class=\"td-bold\" rowspan=\"2\">序号</th>");
            sb.Append("<th class=\"td-bold\" rowspan=\"2\">单位名称</th>");
            sb.Append("<th class=\"td-bold\" " + ((hpColspan > 1) ? " colspan=\"" + hpColspan + "\" " : "") + ">作风建设（互评）</th>");
            sb.Append("<th class=\"td-bold\" " + ((ejdwColspan > 1) ? " colspan=\"" + ejdwColspan + "\" " : "") + ">作风建设（二级单位）</th>");
            sb.Append("<th class=\"td-bold\" " + ((ldColspan > 1) ? " colspan=\"" + ldColspan + "\" " : "") + ">作风建设（领导）</th>");
            sb.Append("<th class=\"td-bold\" rowspan=\"2\">得分汇总</th>");
            sb.Append("<th class=\"td-bold\" rowspan=\"2\">排名</th>");
            sb.Append("<th class=\"td-bold\" rowspan=\"2\">加分</th>");
            sb.Append("<th class=\"td-bold\" rowspan=\"2\">操作</th>");
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
                var bmjgHpLst = JgzfbList.Where(p=>p.JGBM == jgzf.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.同级部门).ToString()).OrderBy(p => p.DFZBXH).ToList();//互评
                var bmjgEjdwLst = JgzfbList.Where(p => p.JGBM == jgzf.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.同级二级单位).ToString()).OrderBy(p => p.DFZBXH).ToList();//二级单位
                var bmjgLdLst = JgzfbList.Where(p => p.JGBM == jgzf.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.上级领导).ToString()).OrderBy(p => p.DFZBXH).ToList();//上级
                //数据
                sb.Append("<tr>");
                sb.Append("<td>" + (index++) + "</td>");
                sb.Append("<td>" + (SysBmjgService.GetBmjgName(jgzf.JGBM)) + "</td>");

                //可进行编辑操作
                foreach (var hp in bmjgHpLst)
                {
                    if (Pnl_Edit.Visible || (LnkBtn_InitData.Visible && Jgbm > 0))
                    {
                        string txtId = "Txt_" + hp.JGBM + "_" + hp.ZBBM + "_" + hp.DFZLX;
                        sb.Append("<td align='right'>" + Utility.GetTextBox(txtId, hp.ZFKHDF, 0, 0, "number", true,
                                    "class=\"easyui-numberbox\" min=\"0\" max=\"200\" precision=\"1\"", "width:80px;") + "</td>");
                    }
                    else
                    {
                        sb.Append("<td><a href=\"#\" onclick=\"showWin('" + hp.KHID + "','" + hp.JGBM + "','" 
                            + hp.ZBBM + "','ZF2');\">" + hp.ZFKHDF + "</a></td>");
                    }
                }
                foreach (var ejdw in bmjgEjdwLst)
                {
                    if (Pnl_Edit.Visible || (LnkBtn_InitData.Visible && Jgbm > 0))
                    {
                        string txtId = "Txt_" + ejdw.JGBM + "_" + ejdw.ZBBM + "_" + ejdw.DFZLX;
                        sb.Append("<td align='right'>" + Utility.GetTextBox(txtId, ejdw.ZFKHDF, 0, 0, "number", true,
                                    "class=\"easyui-numberbox\" min=\"0\" max=\"200\" precision=\"1\"", "width:80px;") + "</td>");
                    }
                    else
                    {
                        sb.Append("<td><a href=\"#\" onclick=\"showWin('" + ejdw.KHID + "','"
                            + ejdw.JGBM + "','" + ejdw.ZBBM + "','ZF3');\">" + ejdw.ZFKHDF + "</a></td>");
                    }
                }
                foreach (var ld in bmjgLdLst)
                {
                    if (Pnl_Edit.Visible || (LnkBtn_InitData.Visible && Jgbm > 0))
                    {
                        string txtId = "Txt_" + ld.JGBM + "_" + ld.ZBBM + "_" + ld.DFZLX;
                        sb.Append("<td align='right'>" + Utility.GetTextBox(txtId, ld.ZFKHDF, 0, 0, "number", true,
                                    "class=\"easyui-numberbox\" min=\"0\" max=\"200\" precision=\"1\"", "width:80px;") + "</td>");
                    }
                    else
                    {
                        sb.Append("<td><a href=\"#\" onclick=\"showWin('" + ld.KHID + "','"
                            + ld.JGBM + "','" + ld.ZBBM + "','ZF1');\">" + ld.ZFKHDF + "</a></td>");
                    }
                }

                if (Pnl_Edit.Visible || (LnkBtn_InitData.Visible && Jgbm > 0))
                {
                    string txtId = "Txt_" + jgzf.JGBM + "_ZDF";
                    sb.Append("<td align='right'>" + Utility.GetTextBox(txtId, jgzf.ZDF, 0, 0, "number", true,
                                "class=\"easyui-numberbox\" min=\"0\" max=\"200\" precision=\"1\"", "width:80px;") + "</td>");
                    txtId = "Txt_" + jgzf.JGBM + "_ZFPM";
                    sb.Append("<td align='right'>" + Utility.GetTextBox(txtId, jgzf.ZFPM, 0, 0, "number", true,
                                "class=\"easyui-numberbox\" min=\"0\" max=\"100\" precision=\"0\"", "width:80px;") + "</td>");
                    txtId = "Txt_" + jgzf.JGBM + "_SJDF";
                    sb.Append("<td align='right'>" + Utility.GetTextBox(txtId, jgzf.SJDF, 0, 0, "number", true,
                                "class=\"easyui-numberbox\" min=\"0\" max=\"100\" precision=\"1\"", "width:80px;") + "</td>");
                }
                else
                {
                    sb.Append("<td>" + jgzf.ZDF + "</td>");
                    sb.Append("<td>" + jgzf.ZFPM + "</td>");
                    sb.Append("<td>" + jgzf.SJDF + "</td>");
                }

                string url = "?KH=" + jgzf.KHID + "&BM=" + jgzf.JGBM;
                url = GetPageUrlAndParams(url).TrimStart('~');
                sb.Append("<td><a href='" + url + "'><img border='0' src='/Resources/Styles/icon/application_edit.png'/></a></td>");
                sb.Append("</tr>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindGrid() 
        {
            //提取作风建设指标数据，如果不存在则进行初始化
            JgzfbList = jgzfSrv.GetListByKhid(Ddl_Kaohe.SelectedValue) as List<KhJgzfbModel>;
            if (JgzfbList.Count == 0)
            {
                //1==提取打分指标表中的 机关作风建设 指标
                List<KhDfzbmxModel> zbmxList = dfzbmxSrv.GetListByKhidAndKhdx(Ddl_Kaohe.SelectedValue, WebKeys.KaoheType.机关部门)
                    .Where(p=>p.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("机关作风")).ToList();
                //2==提取本次考核的所有参与部门(数据上报表中就是所有机构)
                List<KhSjsbModel> sjsbList = sjsbSrv.GetListByKhid(Ddl_Kaohe.SelectedValue).
                    Where(p => p.Bmjg.JGLX.Contains("职能")).OrderBy(p => p.Bmjg.XSXH).ToList();
                //3==初始化
                jgzfSrv.DeleteByKhid(Ddl_Kaohe.SelectedValue);
                jgzfSrv.InitJgzfData(zbmxList, sjsbList);
            }
            if (LnkBtn_InitData.Visible)
                Lbl_Msg.Text = "您可以汇总和修改各部门的得分!";
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //考核期
            var kaohe = khglSrv.GetKhListForValid().Where(p => p.LXID == "LX2014B").ToList();
            khglSrv.BindSSECDropDownListForKaohe(Ddl_Kaohe, kaohe);
            if (!string.IsNullOrEmpty(Khid))
            {
                Ddl_Kaohe.SelectedValue = Khid;
            }
            else if (kaohe.Count > 0)
            {
                Ddl_Kaohe.SelectedValue = kaohe.First().KHID.ToString();//最近一次考核
            }
        }

        #endregion


        #region 事件处理区

        /// <summary>
        /// 计算各部门得分并更新到数据表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Calculate_Click(object sender, EventArgs e)
        {
            //汇总得分
            jgzfSrv.CalculateJgzfDF(Ddl_Kaohe.SelectedValue);            
        }

        /// <summary>
        /// 考核期切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Kaohe_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_InitData_Click(object sender, EventArgs e)
        {
            //1==提取打分指标表中的 机关作风建设 指标
            List<KhDfzbmxModel> zbmxList = dfzbmxSrv.GetListByKhidAndKhdx(Ddl_Kaohe.SelectedValue, WebKeys.KaoheType.机关部门)
                .Where(p => p.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("机关作风")).ToList();
            //2==提取本次考核的所有参与部门(数据上报表中就是所有机构)
            List<KhSjsbModel> sjsbList = sjsbSrv.GetListByKhid(Ddl_Kaohe.SelectedValue).
                Where(p => p.Bmjg.JGLX.Contains("职能")).OrderBy(p => p.Bmjg.XSXH).ToList();
            //3==初始化
            jgzfSrv.DeleteByKhid(Ddl_Kaohe.SelectedValue);
            jgzfSrv.InitJgzfData(zbmxList, sjsbList);
            Utility.ShowMsg(Page, "系统提示", "数据初化完成！", 100, "show");
        }

        /// <summary>
        /// 审核全部得分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Edit_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = true;
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            string url = "?KH=" + Ddl_Kaohe.SelectedValue;
            GobackPageUrl(url);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            //提取数据
            JgzfbList = jgzfSrv.GetListByKhid(Ddl_Kaohe.SelectedValue) as List<KhJgzfbModel>;
            if (Jgbm > 0)
            {
                JgzfbList = JgzfbList.Where(p => p.JGBM == Jgbm).OrderBy(p => p.DFZBXH).ToList();
            }
            List<KhJgzfbModel> bmjgJgzfList = JgzfbList.DistinctBy(p => p.JGBM).
                OrderBy(p => p.Bmjg.BZ).ToList();//形成各部门只一条记录的集合

            string key = string.Empty;
            foreach (var jgzf in bmjgJgzfList)
            {
                var bmjgHpLst = JgzfbList.Where(p=>p.JGBM == jgzf.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.同级部门).ToString()).OrderBy(p => p.DFZBXH).ToList();//互评
                var bmjgEjdwLst = JgzfbList.Where(p => p.JGBM == jgzf.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.同级二级单位).ToString()).OrderBy(p => p.DFZBXH).ToList();//二级单位
                var bmjgLdLst = JgzfbList.Where(p => p.JGBM == jgzf.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.上级领导).ToString()).OrderBy(p => p.DFZBXH).ToList();//上级
                //同级部门
                foreach (var model in bmjgHpLst)
                {
                    //获取数据并更新
                    //"Txt_" + JGBM + "_" + ZBBM + "_" + DFZLX
                    key = "Txt_" + model.JGBM + "_" + model.ZBBM + "_" + ((int)WebKeys.DFUserType.同级部门);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.ZFKHDF = Request.Form[key].ToDecimal(); 
                    }
                    //总分 排名 加分 同时更新
                    key = "Txt_" + model.JGBM + "_ZDF";
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.ZDF = Request.Form[key].ToDecimal();
                    }
                    key = "Txt_" + model.JGBM + "_ZFPM";
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.ZFPM = Request.Form[key].ToInt();
                    }
                    key = "Txt_" + model.JGBM + "_SJDF";
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.SJDF = Request.Form[key].ToDecimal();
                    }
                    model.DB_Option_Action = WebKeys.UpdateAction;
                    jgzfSrv.Execute(model);
                }
                //同级二级单位
                foreach (var model in bmjgEjdwLst)
                {
                    //获取数据并更新
                    //"Txt_" + JGBM + "_" + ZBBM + "_" + DFZLX
                    key = "Txt_" + model.JGBM + "_" + model.ZBBM + "_" + ((int)WebKeys.DFUserType.同级二级单位);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.ZFKHDF = Request.Form[key].ToDecimal();
                    }
                    //总分 排名 加分 同时更新
                    key = "Txt_" + model.JGBM + "_ZDF";
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.ZDF = Request.Form[key].ToDecimal();
                    }
                    key = "Txt_" + model.JGBM + "_ZFPM";
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.ZFPM = Request.Form[key].ToInt();
                    }
                    key = "Txt_" + model.JGBM + "_SJDF";
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.SJDF = Request.Form[key].ToDecimal();
                    }
                    model.DB_Option_Action = WebKeys.UpdateAction;
                    jgzfSrv.Execute(model);
                }
                //上级领导
                foreach (var model in bmjgLdLst)
                {
                    //获取数据并更新
                    //"Txt_" + JGBM + "_" + ZBBM + "_" + DFZLX
                    key = "Txt_" + model.JGBM + "_" + model.ZBBM + "_" + ((int)WebKeys.DFUserType.上级领导);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.ZFKHDF = Request.Form[key].ToDecimal();
                    }
                    //总分 排名 加分 同时更新
                    key = "Txt_" + model.JGBM + "_ZDF";
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.ZDF = Request.Form[key].ToDecimal();
                    }
                    key = "Txt_" + model.JGBM + "_ZFPM";
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.ZFPM = Request.Form[key].ToInt();
                    }
                    key = "Txt_" + model.JGBM + "_SJDF";
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.SJDF = Request.Form[key].ToDecimal();
                    }
                    model.DB_Option_Action = WebKeys.UpdateAction;
                    jgzfSrv.Execute(model);
                }
            }
            //跳转
            GobackPageUrl("?KH=" + Ddl_Kaohe.SelectedValue);
        }

        #endregion


        #region 数据导出

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Report_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Module/Kh/JgbmJgzfHuizToExcel.aspx?KH=" + Ddl_Kaohe.SelectedValue, true);
        }

        #endregion

    }
}