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

namespace Enterprise.UI.Web.Kh
{

    /// <summary>
    /// 参数设置页面
    /// </summary>
    public partial class ParameterSetting : PageBase
    {

        KhNdxsService ndxsSrv = new KhNdxsService();//难度系数
        KhCjqjService cjqjSrv = new KhCjqjService();//成绩区间
        KhKhglService khglSrv = new KhKhglService();//考核管理
        KhJxzrsService zrsSrv = new KhJxzrsService();//绩效责任书
        KhKhdfpxfwService khdfpxfwSrv = new KhKhdfpxfwService();//考核得分排序范围
        KhHbjfgzService hbjfgzSrv = new KhHbjfgzService();//合并计分规则

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
                //Btn_Add.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
                //bool isUpdate = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
                //GridView1.Columns[GridView1.Columns.Count - 1].Visible = isUpdate;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDdl();
                BindGrid();
                TabTitle = "经营难度系数";
            }
        }

        #region 专用方法区

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindGrid()
        {
            if (!string.IsNullOrEmpty(Hid_TabTitle.Value))
            {
                TabTitle = Hid_TabTitle.Value;
            }

            //1==难度系数
            GridView1.DataSource = ndxsSrv.GetListByKhid(Ddl_Kaohe.SelectedValue);
            GridView1.DataBind();

            //2==成绩区间分布
            GridView2.DataSource = cjqjSrv.GetListByKhidForEdit(Ddl_Kaohe.SelectedValue);
            GridView2.DataBind();

            //3==得分排名汇围设置
            var dfpxEjdw = khdfpxfwSrv.GetListByKhidAndKhdx(Ddl_Kaohe.SelectedValue, ((int)WebKeys.KaoheType.二级单位).ToString());
            var dfpxLdbz = khdfpxfwSrv.GetListByKhidAndKhdx(Ddl_Kaohe.SelectedValue, ((int)WebKeys.KaoheType.领导班子).ToString());
            List<KhKhdfpxfwModel> khdfpxLst = new List<KhKhdfpxfwModel>();
            khdfpxLst.AddRange(dfpxEjdw);
            khdfpxLst.AddRange(dfpxLdbz);
            GridView3.DataSource = khdfpxLst;
            GridView3.DataBind();
            
            //4==合并计算规则
            var hbjfgzLst = hbjfgzSrv.GetListByKhid(Ddl_Kaohe.SelectedValue);
            GridView4.DataSource = hbjfgzLst;
            GridView4.DataBind();

        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //考核期
            var kaohe = khglSrv.GetKhListForValid();
            khglSrv.BindSSECDropDownListForKaohe(Ddl_Kaohe, kaohe);
            if (kaohe.Count > 0)
            {
                Ddl_Kaohe.SelectedValue = kaohe.First().KHID.ToString();//最近一次考核
                Lbl_KHMC2.Text = Lbl_KHMC.Text = kaohe.First().KHMC;
            }

            //三个局级单位
            ChkBox_GSDW.Items.Clear();
            var juBmjgs = bmjgService.GetSameLevelBmjg(4).Where(p=>p.SFKH == "1").ToList();
            foreach (var ju in juBmjgs)
            {
                ChkBox_GSDW.Items.Add(new ListItem(ju.JGMC, ju.JGBM.ToString()));
            }

            //计算规则
            var kh = kaohe.FirstOrDefault();
            if (kh != null)
            {
                ZbkJsgzService jsgzSrv = new ZbkJsgzService();
                List<ZbkJsgzModel> jsgsList = jsgzSrv.GetListByBBMC(kh.BBMC).ToList();
                Ddl_GZID.Items.Clear();
                Ddl_GZID.Items.Add(new ListItem("", ""));
                foreach (ZbkJsgzModel jsgs in jsgsList)
                {
                    Ddl_GZID.Items.Add(new ListItem(jsgs.GZMC, jsgs.GZID));
                }
            }

            //合并计算单位
            ChkBox_HBJFDW.DataSource = bmjgService.GetList().Where(p => p.XSXH.Length > 4 && p.JGLX.Contains("二级")).OrderBy(p => p.XSXH).ToList();
            ChkBox_HBJFDW.DataTextField = "JGMC";
            ChkBox_HBJFDW.DataValueField = "JGBM";
            ChkBox_HBJFDW.DataBind();
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
                KhNdxsModel model = e.Row.DataItem as KhNdxsModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //归属单位 1
                e.Row.Cells[1].Text = (model.Bmjg.XSXH.Length > 4) ? SysBmjgService.GetBmjgNameByXsxh(model.Bmjg.XSXH, 4) : "";
                //单位名称 2
                e.Row.Cells[2].Text = model.Bmjg.JGMC;
                //机构类型 3
                e.Row.Cells[3].Text = model.Bmjg.JGLX;
                //考核类型 4
                e.Row.Cells[4].Text = model.Kaohe.Kind.LXMC;
                //难度系数 5
                e.Row.Cells[5].Text = Utility.GetTextBox("Txt" + 5 + "_" + (e.Row.RowIndex + 1),
                        model.NDXS, 5, (e.Row.RowIndex + 1), "number", true,
                        "class=\"easyui-numberbox\" min=\"1.0\" max=\"10.0\" precision=\"2\"", "width:60px;");
            }
        }

        /// <summary>
        /// 成绩区间分布行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhCjqjModel model = e.Row.DataItem as KhCjqjModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //区间等级 0
                e.Row.Cells[0].Text = Utility.GetTextBox("Txt" + 0 + "_" + (e.Row.RowIndex + 1),
                        model.QJDJ, 0, (e.Row.RowIndex + 1), "string", false, "", "width:60px;");
                //最大值 1
                e.Row.Cells[1].Text = Utility.GetTextBox("Txt" + 1 + "_" + (e.Row.RowIndex + 1),
                        model.UPPERV, 1, (e.Row.RowIndex + 1), "number", true, "", "width:60px;");
                //最小值 2
                e.Row.Cells[2].Text = Utility.GetTextBox("Txt" + 2 + "_" + (e.Row.RowIndex + 1),
                        model.LOWERV, 2, (e.Row.RowIndex + 1), "number", true, "", "width:60px;");
            }
        }

        /// <summary>
        /// 成绩区间行命令处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string qjdj = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "shanchu":
                    var q = cjqjSrv.GetListByKhid(Ddl_Kaohe.SelectedValue).FirstOrDefault(p => p.QJDJ == qjdj);
                    if (q != null)
                    {
                        q.DB_Option_Action = WebKeys.DeleteAction;
                        cjqjSrv.Execute(q);
                    }
                    BindGrid();
                    TabTitle = "成绩区间设置";
                    break;
            }
        }

        /// <summary>
        /// 考核期切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Kaohe_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 经营难度系数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            if (Chk_InitNdxs.Checked)
            {
                //1==获取责任书的所有单位
                var khModel = khglSrv.GetSingle(Ddl_Kaohe.SelectedValue);
                if (khModel != null)
                {
                    //2==提取考核类型相同的绩效责任书
                    var jxzrsLst = zrsSrv.GetListByNd_Bmjg_Khlx(khModel.KHND, "1", khModel.LXID);
                    //3==初始化难度系数
                    ndxsSrv.InitKhNdxsByKaoheAndJxzrs(khModel, jxzrsLst);
                }
                Chk_InitNdxs.Checked = false;
            }
            else
            {
                string key = string.Empty;
                foreach (GridViewRow gvr in GridView1.Rows)
                {
                    //JGBM,KHID
                    int JGBM = GridView1.DataKeys[gvr.RowIndex].Values["JGBM"].ToInt();
                    int KHID = GridView1.DataKeys[gvr.RowIndex].Values["KHID"].ToInt();
                    KhNdxsModel model = new KhNdxsModel();
                    model.DB_Option_Action = WebKeys.InsertOrUpdateAction;
                    model.KHID = KHID;
                    model.JGBM = JGBM;
                    //难度系数 5
                    key = "Txt" + 5 + "_" + (gvr.RowIndex + 1);
                    model.NDXS = Request.Form[key].ToDecimal();
                    ndxsSrv.Execute(model);
                }
                Utility.ShowMsg(Page, "提示", "难度系数设置成功！", 100, "show");
            }
            BindGrid();
            TabTitle = "经营难度系数";
        }

        /// <summary>
        /// 成绩分布区间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            string key = string.Empty;
            foreach (GridViewRow gvr in GridView2.Rows)
            {
                //KHID
                int KHID = GridView2.DataKeys[gvr.RowIndex].Values["KHID"].ToInt();
                KhCjqjModel model = new KhCjqjModel();
                model.DB_Option_Action = WebKeys.InsertOrUpdateAction;
                model.KHID = KHID;
                //区间等级 0
                key = "Txt" + 0 + "_" + (gvr.RowIndex + 1);
                model.QJDJ = Request.Form[key].ToRequestString();
                if (string.IsNullOrEmpty(model.QJDJ)) 
                    continue;
                //最大值 1
                key = "Txt" + 1 + "_" + (gvr.RowIndex + 1);
                model.UPPERV = Request.Form[key].ToDecimal();
                //最小值 2
                key = "Txt" + 2 + "_" + (gvr.RowIndex + 1);
                model.LOWERV = Request.Form[key].ToDecimal();
                cjqjSrv.Execute(model);
            }
            Utility.ShowMsg(Page, "提示", "成绩区间设置成功！", 100, "show");
            BindGrid();
            TabTitle = "成绩区间设置";
        }

        #endregion

        #region 得分排名范围设置

        protected void GridView3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "shanchu":
                    var q = khdfpxfwSrv.GetSingle(id);
                    if (q != null)
                    {
                        q.DB_Option_Action = WebKeys.DeleteAction;
                        khdfpxfwSrv.Execute(q);
                    }
                    BindGrid();
                    TabTitle = "排名范围设置";
                    break;
            }
        }

        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhKhdfpxfwModel model = e.Row.DataItem as KhKhdfpxfwModel;
                //排名范围 1
                e.Row.Cells[1].Text = getGsdwName(model.GSDW);
                //考核对象 2
                e.Row.Cells[2].Text = getKhdxName(model.KHDX.ToInt());
            }
        }

        /// <summary>
        /// 添加排名范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Dfpxfw_Click(object sender, EventArgs e)
        {
            KhKhdfpxfwModel model = new KhKhdfpxfwModel();
            model.DB_Option_Action = WebKeys.InsertAction;
            model.ID = CommonTool.GetGuidKey();
            var kaohe = khglSrv.GetSingle(Ddl_Kaohe.SelectedValue);
            model.KHID = kaohe.KHID;
            model.KHDX = Rdl_KHDX.SelectedValue;
            string gsdw = string.Empty;
            foreach (ListItem item in ChkBox_GSDW.Items)
            {
                if (item.Selected)
                    gsdw += item.Value + ",";
            }
            if (!string.IsNullOrEmpty(gsdw))
            {
                model.GSDW = "," + gsdw;
                khdfpxfwSrv.Execute(model);
                Utility.ShowMsg(Page, "提示", "排名范围设置成功！", 100, "show");
            }
            BindGrid();
            TabTitle = "排名范围设置";
        }

        /// <summary>
        /// 获取排名范围内的单位名称
        /// </summary>
        /// <param name="gsdw"></param>
        /// <returns></returns>
        private string getGsdwName(string gsdw)
        {
            string gsdwmc = string.Empty;
            string[] jgbms = gsdw.TrimStart(',').TrimEnd(',').Split(',');
            foreach (var dw in jgbms)
            {
                var bmjgModel = bmjgService.GetSingle(dw);
                if (bmjgModel != null)
                {
                    gsdwmc += bmjgModel.JGMC + "、";
                }
            }
            return gsdwmc.TrimEnd("、".ToCharArray());
        }

        /// <summary>
        /// 获取考核对象名称
        /// </summary>
        /// <param name="khdx"></param>
        /// <returns></returns>
        private string getKhdxName(int khdx)
        {
            switch (khdx)
            {
                case (int)WebKeys.KaoheType.二级单位:
                    return WebKeys.KaoheType.二级单位.ToString();
                case (int)WebKeys.KaoheType.领导班子:
                    return WebKeys.KaoheType.领导班子.ToString();
            }
            return "";
        }

        #endregion

        #region 合并计分规则设置

        /// <summary>
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhHbjfgzModel model = e.Row.DataItem as KhHbjfgzModel;
                //1 合并计分名称
                e.Row.Cells[1].Text = model.HBJFMC;
                //2 合并计分单位
                e.Row.Cells[2].Text = getGsdwName(model.HBJFDW);
                //3 配置公式名称
                e.Row.Cells[3].Text = (model.Jsgz != null) ? model.Jsgz.GZMC : "";
            }
        }

        /// <summary>
        /// 行命令操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView4_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string hbjfid = e.CommandArgument.ToString();
            var q = hbjfgzSrv.GetSingle(hbjfid);
            switch (e.CommandName)
            {
                case "shanchu":
                    if (q != null)
                    {
                        q.DB_Option_Action = WebKeys.DeleteAction;
                        hbjfgzSrv.Execute(q);
                    }
                    break;
                case "bianji":
                    if (q != null)
                    {
                        Txt_HBJFMC.Text = q.HBJFMC;
                        string[] dws = q.HBJFDW.TrimStart(',').TrimEnd(',').Split(',');
                        foreach(var dw in dws) {
                           var item = ChkBox_HBJFDW.Items.FindByValue(dw);
                           if (item != null)
                               item.Selected = true;
                        }
                        Hid_HBJFID.Value = q.HBJFID;
                        Ddl_GZID.SelectedValue = q.GZID;
                    }
                    break;
            }
            BindGrid();
            TabTitle = "合并计分规则";
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Hbjsgz_Click(object sender, EventArgs e)
        {
            KhHbjfgzModel model = null;
            if (!string.IsNullOrEmpty(Hid_HBJFID.Value))
            {
                //update
                model = hbjfgzSrv.GetSingle(Hid_HBJFID.Value);
                model.DB_Option_Action = WebKeys.UpdateAction;
            }
            else
            {
                //insert
                model = new KhHbjfgzModel();
                model.HBJFID = CommonTool.GetGuidKey();
                model.DB_Option_Action = WebKeys.InsertAction;
                model.KHID = Ddl_Kaohe.SelectedValue.ToInt();
            }
            model.GZID = Ddl_GZID.SelectedValue;
            model.HBJFMC = Txt_HBJFMC.Text;
            string hbjfdw = string.Empty;
            foreach (ListItem item in ChkBox_HBJFDW.Items)
            {
                if (item.Selected)
                {
                    hbjfdw += item.Value + ",";
                }
            }
            model.HBJFDW = (!string.IsNullOrEmpty(hbjfdw)) ? "," + hbjfdw : null;
            hbjfgzSrv.Execute(model);
            Utility.ShowMsg(Page, "提示", "合并计分规则操作成功！", 100, "show");
            BindGrid();
            TabTitle = "合并计分规则";
        }

        #endregion

    }
}