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

namespace Enterprise.UI.Web.Zbk
{
    /// <summary>
    /// 打分指标维护页面
    /// </summary>
    public partial class JjfzbbmManage : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        ZbkDfzbService dfzbSrv = new ZbkDfzbService();
        /// <summary>
        /// 指标信息服务类
        /// </summary>
        ZbkZbxxService zbxxSrv = new ZbkZbxxService();
        /// <summary>
        /// 版本服务类
        /// </summary>
        ZbkBanbenService banbenSrv = new ZbkBanbenService();
        /// <summary>
        /// 被考核机构服务类
        /// </summary>
        ZbkBdfjgService bdfjgSrv = new ZbkBdfjgService();
        /// <summary>
        /// 打分者服务类
        /// </summary>
        ZbkDfzService dfzSrv = new ZbkDfzService();

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
                Btn_Add.Visible = LnkBtn_Ins.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
                LnkBtn_Upd.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
                bool isDelete = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Del);
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = isDelete;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clearText();
                BindDDL();
                BindGrid();
            }
        }

        #region DDL

        protected void BindDDL()
        {
            //参与的机构
            List<SysBmjgModel> bmjgList = bmjgService.GetBmjgTreeLst(true) as List<SysBmjgModel>;
            //绑定被打分机构
            Chk_BKHJG.DataSource = bmjgList.Where(p => !string.IsNullOrEmpty(p.JGLX) && p.SFKH == "1")
                .OrderBy(p => p.JGLX).ThenBy(p => p.BZ).ToList();
            Chk_BKHJG.DataTextField = "JGMC";
            Chk_BKHJG.DataValueField = "JGBM";
            Chk_BKHJG.DataBind();

            //绑定打分者
            //Chk_DFZ.Items.Clear();
            Chk_DFZ.DataSource = bmjgList.Where(p => !string.IsNullOrEmpty(p.JGLX))
                .OrderBy(p => p.JGLX).ThenBy(p => p.BZ).ToList();
            Chk_DFZ.DataTextField = "JGMC";
            Chk_DFZ.DataValueField = "JGBM";
            Chk_DFZ.DataBind();
            Chk_DFZ.Items.Insert(0, new ListItem("〖油气田领导〗", "YQTLD"));
            Chk_DFZ.Items.Insert(1, new ListItem("〖分管领导〗", "FGLD"));
            //var znbmLst = bmjgList.Where(p=>p.JGLX.Contains("职能")).ToList();
            //foreach (var bm in znbmLst)
            //{
            //    Chk_DFZ.Items.Add(new ListItem(bm.JGMC, bm.JGBM.ToString()));
            //}

            //定性指标
            List<ZbkDfzbModel> dfzbList = dfzbSrv.GetList() as List<ZbkDfzbModel>;
            var dxzbxxLst = zbxxSrv.GetListForValid("定性指标");
            Ddl_ZBID.Items.Clear();
            foreach (var zb in dxzbxxLst)
            {
                if (!dfzbList.Exists(p => p.ZBID == zb.ZBID))
                {
                    Ddl_ZBID.Items.Add(new ListItem(string.Format("{0}※{1}※{2}※{3}", zb.YJZBMC, zb.EJZBMC, zb.SJZBMC, zb.ZBMC), zb.ZBID.ToString()));
                }
            }

            //应用版本
            Ddl_BBMC.DataSource = banbenSrv.GetList();
            Ddl_BBMC.DataTextField = "BBMC";
            Ddl_BBMC.DataValueField = "BBMC";
            Ddl_BBMC.DataBind();
            Ddl_BBMC_Search.DataSource = banbenSrv.GetList();
            Ddl_BBMC_Search.DataTextField = "BBMC";
            Ddl_BBMC_Search.DataValueField = "BBMC";
            Ddl_BBMC_Search.DataBind();

            //计算规则
            ZbkJsgzService jsgzSrv = new ZbkJsgzService();
            List<ZbkJsgzModel> jsgsList = jsgzSrv.GetListByBBMC(Ddl_BBMC.SelectedValue).ToList();
            Ddl_GZID.Items.Clear();
            Ddl_GZID.Items.Add(new ListItem("", ""));
            foreach (ZbkJsgzModel jsgs in jsgsList)
            {
                Ddl_GZID.Items.Add(new ListItem(jsgs.GZMC, jsgs.GZID));
            }

        }

        #endregion

        #region 绑定表格
        protected void BindGrid()
        {
            List<ZbkDfzbModel> dfzbList = null;
            if (!string.IsNullOrEmpty(Txt_Dfzb_Search.Text) && !string.IsNullOrEmpty(Ddl_Pflx_Search.SelectedValue))
            {
                dfzbList = dfzbSrv.GetListByBBMC(Ddl_BBMC_Search.Text).Where(p => p.Zbxx.ZBMC.Contains(Txt_Dfzb_Search.Text) && p.PFLX == Ddl_Pflx_Search.SelectedValue).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(Ddl_Pflx_Search.SelectedValue))
                {
                    dfzbList = dfzbSrv.GetListByBBMC(Ddl_BBMC_Search.Text).Where(p => p.PFLX == Ddl_Pflx_Search.SelectedValue).ToList();
                }
                else if (!string.IsNullOrEmpty(Txt_Dfzb_Search.Text))
                {
                    dfzbList = dfzbSrv.GetListByBBMC(Ddl_BBMC_Search.Text).Where(p => p.Zbxx.ZBMC.Contains(Txt_Dfzb_Search.Text)).ToList();
                }
                else
                {
                    dfzbList = dfzbSrv.GetListByBBMC(Ddl_BBMC_Search.Text).OrderBy(p => p.PFLX).ThenBy(p => p.Zbxx.SXH).ToList();
                }
            }
            GridView1.DataSource = dfzbList;
            GridView1.DataBind();
        }
        #endregion

        #region 隔行换色
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ZbkDfzbModel dataModel = e.Row.DataItem as ZbkDfzbModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //打分者 7
                e.Row.Cells[7].Text = "<a href='#' onclick=\"javascript:alert('" + GetDFZ(dataModel) + "');\">[查看]</a>";
                //查看详细8
                e.Row.Cells[8].Text = "<a href='#' onclick=\"javascript:showInfo('评分标准','"
                    + dataModel.PFBZ.ToStr() + "');\">[详细]</a>";//HttpUtility.UrlEncode(dataModel.PFBZ.ToStr())
                //计算规则10
                e.Row.Cells[10].Text = (dataModel.Jsgz != null) ? dataModel.Jsgz.GZMC : "";
            }
        }
        #endregion


        #region 操作事件

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            clearText();
            ZbkDfzbModel model = null;
            switch (e.CommandName)
            {
                case "bianji":
                    model = dfzbSrv.GetSingle(e.CommandArgument.ToString());
                    if (model != null) 
                    {
                        CommonTool.SetModelDataToForm(model, Page, "Txt_", true);
                        CommonTool.SetModelDataToForm(model, Page, "Ddl_", true);
                        CommonTool.SetModelDataToForm(model, Page, "Chk_", true);
                        //Txt_BDS.Text = model.DFBDS;
                        Hid_DFZBBM.Value = model.DFZBBM;
                        Txt_PFBZ.Value = model.PFBZ;
                        Lbl_ZBMC.Text = string.Format("{0}※{1}※{2}※{3}", model.Zbxx.YJZBMC, model.Zbxx.EJZBMC, 
                            model.Zbxx.SJZBMC, model.Zbxx.ZBMC);
                        Hid_ZBID2.Value = model.ZBID.ToString();
                        
                        //被考核机构
                        if (model.BdfjgLst != null)
                        {
                            List<ZbkBdfjgModel> bdfjgLst = new List<ZbkBdfjgModel>();
                            bdfjgLst.AddRange(model.BdfjgLst);
                                //bdfjgSrv.GetListByHQL("from ZbkBdfjgModel p where p.DFZBBM='"+model.DFZBBM+"'") as List<ZbkBdfjgModel>;
                            foreach (ListItem item in Chk_BKHJG.Items)
                            {
                                if (bdfjgLst.Exists(p => p.JGBM == item.Value.ToInt()))
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                        //打分者
                        if (model.DfzLst != null)
                        {
                            List<ZbkDfzModel> dfzLst = new List<ZbkDfzModel>();
                            dfzLst.AddRange(model.DfzLst);
                                //dfzSrv.GetListByHQL("from ZbkDfzModel p where p.DFZBBM='" + model.DFZBBM + "'") as List<ZbkDfzModel>;
                            foreach (ListItem item in Chk_DFZ.Items)
                            {
                                if (dfzLst.Exists(p => p.OPERATOR == item.Value))
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }
                    SetCntrlVisibility(LnkBtn_Ins, false);
                    SetCntrlVisibility(LnkBtn_Upd, true);
                    SetCntrlVisibility(LnkBtn_Del, false);
                    Pnl_Edit.Visible = true;
                    Lbl_ZBMC.Visible = true;
                    Ddl_ZBID.Visible = false;
                    break;
                case "shanchu":
                    dfzbSrv.DeleteDFZB(e.CommandArgument.ToString());
                    Pnl_Edit.Visible = false;
                    BindGrid();
                    break;
            }
        }

        //页面翻页
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;//分页BindData();
            BindGrid();
        }

        #endregion


        #region 按钮事件处理

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            ZbkDfzbModel model = (ZbkDfzbModel)CommonTool.GetFormDataToModel(typeof(ZbkDfzbModel), Page);
            model.DFZBBM = "DFZB" + CommonTool.GetPkId();
            model.DB_Option_Action = WebKeys.InsertAction;
            //model.DISABLE = (Chk_DISABLE.Checked) ? "1" : "0";
            //model.SFFJX = (Chk_SFFJX.Checked) ? "1" : "0";
            //model.DFBDS = Server.UrlDecode(Hid_DFBDS.Value);
            model.GZID = (string.IsNullOrEmpty(model.GZID)) ? null : model.GZID;
            model.PFBZ = Txt_PFBZ.Value;
            //add
            if (model.ZBID > 0 && dfzbSrv.Execute(model))
            {
                //被考核机构
                bdfjgSrv.DeleteByZbbm(model.DFZBBM);
                foreach (ListItem item in Chk_BKHJG.Items)
                {
                    if (item.Selected)
                    {
                        ZbkBdfjgModel bdfjg = new ZbkBdfjgModel();
                        bdfjg.DB_Option_Action = WebKeys.InsertAction;
                        bdfjg.DFZBBM = model.DFZBBM;
                        bdfjg.JGBM = item.Value.ToInt();
                        bdfjgSrv.Execute(bdfjg);
                    }
                }
                //打分者
                dfzSrv.DeleteByZbbm(model.DFZBBM);
                foreach (ListItem item in Chk_DFZ.Items)
                {
                    if (item.Selected)
                    {
                        ZbkDfzModel dfz = new ZbkDfzModel();
                        dfz.DB_Option_Action = WebKeys.InsertAction;
                        dfz.DFZBBM = model.DFZBBM;
                        dfz.OPERATOR = item.Value;
                        dfz.OPERTYPE = (item.Value.ToInt() > 0) ? "1" : "0";
                        dfzSrv.Execute(dfz);
                    }
                }
            }
            clearText();
            BindGrid();
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Hid_DFZBBM.Value))
            {
                ZbkDfzbModel model = (ZbkDfzbModel)CommonTool.GetFormDataToModel(typeof(ZbkDfzbModel), Page);
                model.DFZBBM = Hid_DFZBBM.Value;
                model.DB_Option_Action = WebKeys.UpdateAction;
                //model.DISABLE = (Chk_DISABLE.Checked) ? "1" : "0";
                //model.SFFJX = (Chk_SFFJX.Checked) ? "1" : "0";
                //model.DFBDS = Server.UrlDecode(Hid_DFBDS.Value);
                model.GZID = (string.IsNullOrEmpty(model.GZID)) ? null : model.GZID;
                model.PFBZ = Txt_PFBZ.Value;
                model.ZBID = Hid_ZBID2.Value.ToInt();
                //update
                if (dfzbSrv.Execute(model))
                {
                    //被考核机构
                    bdfjgSrv.DeleteByZbbm(model.DFZBBM);
                    foreach (ListItem item in Chk_BKHJG.Items)
                    {
                        if (item.Selected)
                        {
                            ZbkBdfjgModel bdfjg = new ZbkBdfjgModel();
                            bdfjg.DB_Option_Action = WebKeys.InsertAction;
                            bdfjg.DFZBBM = model.DFZBBM;
                            bdfjg.JGBM = item.Value.ToInt();
                            bdfjgSrv.Execute(bdfjg);
                        }
                    }
                    //打分者
                    dfzSrv.DeleteByZbbm(model.DFZBBM);
                    foreach (ListItem item in Chk_DFZ.Items)
                    {
                        if (item.Selected)
                        {
                            ZbkDfzModel dfz = new ZbkDfzModel();
                            dfz.DB_Option_Action = WebKeys.InsertAction;
                            dfz.DFZBBM = model.DFZBBM;
                            dfz.OPERATOR = item.Value;
                            dfz.OPERTYPE = (item.Value.ToInt() > 0) ? "1" : "0";
                            dfzSrv.Execute(dfz);
                        }
                    }
                }
            }
            clearText();
            BindGrid();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Del_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            GobackPageUrl("");
        }

        #endregion


        #region 私有方法

        /// <summary>
        /// 清空输入框内容
        /// </summary>
        private void clearText()
        {
            Txt_PFBZ.Value = "";
            Txt_JXFZ.Text = "";
            Chk_BKHJG.ClearSelection();
            Chk_DFZ.ClearSelection();
            Ddl_GZID.ClearSelection();
            //Txt_BDS.Text = "";
            //Hid_DFBDS.Value = "";
            Hid_ZBID2.Value = "";
            Txt_MAXV.Text = "";
            Txt_MINV.Text = "";
            SetCntrlVisibility(LnkBtn_Ins, true);
            SetCntrlVisibility(LnkBtn_Upd, false);
            SetCntrlVisibility(LnkBtn_Del, false);
            Pnl_Edit.Visible = false;

        }

        #endregion

        /// <summary>
        /// 显示添加面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Lbl_ZBMC.Visible = false;
            Ddl_ZBID.Visible = true;
            Pnl_Edit.Visible = true;
            SetCntrlVisibility(LnkBtn_Ins, true);
            SetCntrlVisibility(LnkBtn_Upd, false);
            SetCntrlVisibility(LnkBtn_Del, false);
            BindDDL();
            BindGrid();
        }

        /// <summary>
        /// 提取指定分类的所有打分指标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Pflx_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = false;
            BindGrid();
        }

        /// <summary>
        /// 指标查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Find_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = false;
            BindGrid();
        }

        /// <summary>
        /// 获取打分者信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected string GetDFZ(ZbkDfzbModel model)
        {
            StringBuilder sb = new StringBuilder();
            if (model.DfzLst != null)
            {
                foreach (var q in model.DfzLst)
                {
                    if (q.OPERTYPE == "1")
                    {
                        sb.Append(SysBmjgService.GetBmjgName(q.OPERATOR.ToInt()) + ",");
                    }
                    else
                    {
                        if (q.OPERATOR == "YQTLD")
                        {
                            sb.Append("油气田领导" + ",");
                        }
                        else if (q.OPERATOR == "FGLD")
                        {
                            sb.Append("分管领导" + ",");
                        }
                        else {
                            sb.Append("未知" + ",");
                        }
                    }
                }
            }
            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 指标版本选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_BBMC_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

    }
}