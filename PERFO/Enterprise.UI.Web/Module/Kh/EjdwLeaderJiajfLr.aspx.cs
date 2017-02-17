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
    ///二级单位领导班子加减分录入页面
    /// </summary>
    public partial class EjdwLeaderJiajfLr : PageBase
    {

        /// <summary>
        /// 打分指标明细-服务类
        /// </summary>
        KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();
        KhJgbmdfbService jgbmdfbSrv = new KhJgbmdfbService();//机关部门打分表
        KhKhglService khglSrv = new KhKhglService();//考核管理

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected string Dfzb = (string)Utility.sink("ZB", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//指标编码

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
               Btn_Add.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDdl();
                BindGrid();
            }
        }

        #region 专用方法区

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindGrid() 
        {
            //1==检测用户权限
            if (!jgbmdfbSrv.CheckUserPermessionForDfzb(Ddl_Kaohe.SelectedValue, Ddl_Zhibiao.SelectedValue, userModel))
            {
                Btn_Add.Visible = false;
                Pnl_Edit.Visible = false;
            }
            //2==绑定数据
            List<KhDfzbmxModel> dfzbList = dfzbmxSrv.GetListByKhidAndZhibiao(Ddl_Kaohe.SelectedValue,
                Ddl_Zhibiao.SelectedValue, WebKeys.KaoheType.领导班子) as List<KhDfzbmxModel>;
            GridView1.DataSource = dfzbList;
            GridView1.DataBind();
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //考核期
            var kaohe = khglSrv.GetKhListForValid().Where(p => p.LXID == "LX2014A").ToList();
            khglSrv.BindSSECDropDownListForKaohe(Ddl_Kaohe, kaohe);
            if (!string.IsNullOrEmpty(Khid))
            {
                Ddl_Kaohe.SelectedValue = Khid;
            }
            else if (kaohe.Count > 0)
            {
                Ddl_Kaohe.SelectedValue = kaohe.First().KHID.ToString();//最近一次考核
            }

            //指标
            var zhibiaoLst = dfzbmxSrv.GetListByKhidAndKhdx(Ddl_Kaohe.SelectedValue, WebKeys.KaoheType.领导班子);
            zhibiaoLst = zhibiaoLst.DistinctBy(p => p.ZBBM).ToList();//一个指标一条
            dfzbmxSrv.BindSSECDropDownListForDfzb(Ddl_Zhibiao, zhibiaoLst);
            if (!string.IsNullOrEmpty(Dfzb))
            {
                Ddl_Zhibiao.SelectedValue = Dfzb;
            }
            else
            {
                Ddl_Zhibiao.SelectedIndex = 1;//缺省第一个指标
            }
        }

        #endregion


        #region 事件处理区

        /// <summary>
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhDfzbmxModel model = e.Row.DataItem as KhDfzbmxModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

                //单位名称 1
                e.Row.Cells[1].Text = model.Danwei.JGMC;
                //指标名称 2
                e.Row.Cells[2].Text = "<a href='#' onclick=\"javascript:$.messager.alert('指标说明','" +
                    model.DfzbModel.PFBZ.ToStr() + "','info');\">" + model.DfzbModel.Zbxx.ZBMC + "</a>";
                //评分类型及分值 3
                e.Row.Cells[3].Text = model.DfzbModel.PFLX + "【" + model.DfzbModel.JXFZ + "】";
                //否决项 4
                if (model.DfzbModel.SFFJX == "1")
                {
                    e.Row.Cells[4].Text = "<img src=\"/Resources/Images/right.gif\" border=\"0\" />";
                }                
                if (Pnl_Edit.Visible)
                {
                    //打分值 5
                    e.Row.Cells[5].Text = Utility.GetTextBox("Txt" + 5 + "_" + (e.Row.RowIndex + 1),
                                (model.DFSZ), 5, (e.Row.RowIndex + 1), "number", true,
                                "class=\"easyui-numberbox\" precision=\"1\" " + getDafeiQujian(model.DfzbModel), "width:85px;");
                    //打分理由 6
                    e.Row.Cells[6].Text = Utility.GetTextBox("Txt" + 6 + "_" + (e.Row.RowIndex + 1),
                        model.DFBZ, 6, (e.Row.RowIndex + 1), "string", false, "", "width:380px;");
                    //约束规则 7
                    if (model.DfzbModel.Zbxx.YJZBMC.Contains("约束"))
                    {
                        //只约束指标选择
                        e.Row.Cells[7].Text = getYsgzSelectControl("Txt" + 7 + "_" + (e.Row.RowIndex + 1), model);
                    }
                }
                else
                {
                    //打分值 5
                    e.Row.Cells[5].Text = model.DFSZ.ToRequestString();
                    //打分理由 6
                    e.Row.Cells[6].Text = model.DFBZ.ToRequestString();
                    //约束规则 7
                    e.Row.Cells[7].Text = model.YSSM;
                }
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            string key = string.Empty;
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                //DFZBID
                string DFZBID = GridView1.DataKeys[gvr.RowIndex].Values["DFZBID"].ToRequestString();
                KhDfzbmxModel model = dfzbmxSrv.GetSingle(Ddl_Kaohe.SelectedValue, DFZBID);
                if (model == null)
                    continue;
                model.DB_Option_Action = WebKeys.UpdateAction;
                //打分值 5
                key = "Txt" + 5 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    model.SJDF = model.DFSZ = Request.Form[key].ToDecimal();
                }
                //打分理由 6
                key = "Txt" + 6 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    model.DFBZ = Request.Form[key];
                }
                //约束规则 7
                key = "Txt" + 7 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    model.YSSM = Request.Form[key];
                }
                model.DFRQ = DateTime.Now;
                if (dfzbmxSrv.Execute(model))
                {
                    //同时更新机关部门打分表中与当前打分者对应的记录
                    KhJgbmdfbModel m = model.KhJgbmdfbLst.FirstOrDefault(p => p.DFZ == userModel.JGBM.ToString() || p.DFZ == userModel.LOGINID);
                    if (m != null)
                    {
                        m.DB_Option_Action = WebKeys.UpdateAction;
                        m.DFSJ = DateTime.Now;
                        m.DFSM = model.DFBZ;
                        m.KHDF = model.DFSZ;
                        jgbmdfbSrv.Execute(m);
                    }
                }
            }
            Pnl_Edit.Visible = false;
            BindGrid();
            Utility.ShowMsg(Page, "提示", "保存打分结果成功!", 100, "show");
        }
        
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            string url = string.Format("KH={0}&ZB={1}", Ddl_Kaohe.SelectedValue, Ddl_Zhibiao.SelectedValue);
            GobackPageUrl("?" + url);
        }

        /// <summary>
        /// 考核期选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Kaohe_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 指标选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Zhibiao_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 显示编辑面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = true;
            BindGrid();
        }

        #endregion

        #region 专用方法

        /// <summary>
        /// 获取打分区间限定
        /// </summary>
        /// <param name="dfzb">打分指标</param>
        /// <returns></returns>
        private string getDafeiQujian(ZbkDfzbModel dfzb)
        {
            string dfqj = string.Empty;
            switch (dfzb.PFLX)
            {
                case "减分-":
                    dfqj = " max=\"0\" min=\"-" + dfzb.JXFZ + "\" ";
                    break;
                case "加分+":
                    dfqj = " max=\"" + dfzb.JXFZ + "\" min=\"0\" ";
                    break;
                case "加减分±":
                    dfqj = " max=\"" + dfzb.JXFZ + "\" min=\"-" + dfzb.JXFZ + "\" ";
                    break;
            }
            return dfqj;
        }

        /// <summary>
        /// 获取约束规则的下拉控件脚本
        /// </summary>
        /// <param name="ctrlName">控件名</param>
        /// <param name="model">KhDfzbmxModel</param>
        /// <returns></returns>
        private string getYsgzSelectControl(string ctrlName, KhDfzbmxModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<select name=\"" + ctrlName + "\"  size=\"1\">");
            sb.Append("<option></option>");
            sb.Append("<option" + ((model.YSSM == "只减分") ? " selected=\"selected\" " : "") + ">只减分</option>");
            sb.Append("<option" + ((model.YSSM == "降为D级") ? " selected=\"selected\" " : "") + ">降为D级</option>");
            sb.Append("<option" + ((model.YSSM == "降1级") ? " selected=\"selected\" " : "") + ">降1级</option>");
            sb.Append("<option" + ((model.YSSM == "降2级") ? " selected=\"selected\" " : "") + ">降2级</option>");
            sb.Append("<option" + ((model.YSSM == "降3级") ? " selected=\"selected\" " : "") + ">降3级</option>");
            sb.Append("<option" + ((model.YSSM == "不进入A级") ? " selected=\"selected\" " : "") + ">不进入A级</option>");
            sb.Append("</select>");

            return sb.ToString();
        }

        #endregion


    }
}