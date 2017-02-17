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
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Zbk
{

    /// <summary>
    /// 指标管理页面
    /// </summary>
    public partial class ZbManage : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        ZbkZbxxService zbxxSrv = new ZbkZbxxService();

        #region 权限检查
        
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            base.PermissionHandler += ZbManage_PermissionHandler;
        }

        void ZbManage_PermissionHandler(PageBase.PermissionEventArgs e)
        {
            if (e.Model != null)
            {
                LnkBtn_Ins.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
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
                BindDll();
                BindGrid();
            }
        }

        #region 绑定表格

        protected void BindDll()
        {
            List<ZbkZbxxModel> zbxxList = zbxxList = zbxxSrv.GetListByZblxAndYjzbmc(Ddl_Zblx_Search.Text, "") as List<ZbkZbxxModel>;
            var zbmcs = zbxxList.Distinct<ZbkZbxxModel>(new FastPropertyComparer<ZbkZbxxModel>("YJZBMC"));
            Ddl_Yjzbmc_Search.DataSource = zbmcs.OrderBy(p => p.YJZBMC);
            Ddl_Yjzbmc_Search.DataTextField = "YJZBMC";
            Ddl_Yjzbmc_Search.DataValueField = "YJZBMC";
            Ddl_Yjzbmc_Search.DataBind();
            Ddl_Yjzbmc_Search.Items.Insert(0, new ListItem("", ""));
        }

        /// <summary>
        /// 绑定表格
        /// </summary>
        protected void BindGrid()
        {
            List<ZbkZbxxModel> zbxxList = null;
            //指标名称查询
            if (!string.IsNullOrEmpty(Txt_ZBMC_Search.Text))
            {
                zbxxList = zbxxSrv.GetListByZblxAndYjzbmc(Ddl_Zblx_Search.Text, Ddl_Yjzbmc_Search.SelectedValue).
                    Where(p => p.ZBMC.Contains(Txt_ZBMC_Search.Text)).
                    OrderBy(p => p.ZBLX).ThenBy(p => p.SJZBMC).ThenBy(p => p.SXH).ToList();
            }
            else
            {
                zbxxList = zbxxSrv.GetListByZblxAndYjzbmc(Ddl_Zblx_Search.Text, Ddl_Yjzbmc_Search.SelectedValue).
                    OrderBy(p => p.ZBLX).ThenBy(p => p.SJZBMC).ThenBy(p => p.SXH).ToList();
            }

            if (zbxxList != null)
            {
                GridView1.DataSource = zbxxList;
                GridView1.DataBind();
            }
        }
        #endregion


        #region 隔行换色
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ZbkZbxxModel model = e.Row.DataItem as ZbkZbxxModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
            }
        }
        #endregion


        #region 保存操作

        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Hid_ZBID.Value))
            {
                ZbkZbxxModel model = zbxxSrv.GetSingle(Hid_ZBID.Value);
                if (model != null)
                {
                    model.DB_Option_Action = WebKeys.UpdateAction;
                    model.ZBID = Hid_ZBID.Value.ToInt();
                    model.ZBMC = Txt_ZBMC.Text;
                    model.YJZBMC = Txt_YJZBMC.Text;
                    model.EJZBMC = Txt_EJZBMC.Text;
                    model.SJZBMC = Txt_SJZBMC.Text;
                    model.ZBLX = Ddl_ZBLX.SelectedValue;
                    model.SXH = getXsxh(model);
                    model.ZBZT = (Chk_ZBZT.Checked) ? "1" : "0";
                    //submit
                    zbxxSrv.Execute(model);
                }
            }
            else
            {
                ZbkZbxxModel model = (ZbkZbxxModel)CommonTool.GetFormDataToModel(typeof(ZbkZbxxModel), Page);
                if (model != null)
                {
                    model.DB_Option_Action = WebKeys.InsertAction;
                    model.TJRQ = DateTime.Now;
                    model.ZBZT = (Chk_ZBZT.Checked) ? "1" : "0";
                    model.SXH = getXsxh(model);
                    //submit
                    zbxxSrv.Execute(model);
                }
            }
            clearText();
            BindGrid();         
        }

        /// <summary>
        /// 清空内容
        /// </summary>
        private void clearText()
        {
            Txt_SJZBMC.Text = "";
            Txt_ZBMC.Text = "";
            Txt_ZBMC_Search.Text = "";
            Txt_YJZBMC.Text = "";
            Txt_EJZBMC.Text = "";
            Hid_ZBID.Value = "";
            Pnl_Edit.Visible = false;
        }

        #endregion


        #region 操作事件 编辑/改变状态/删除
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ZbkZbxxModel model = null;
            switch (e.CommandName)
            {
                case "bianji":
                    model = zbxxSrv.GetSingle(e.CommandArgument.ToString());
                    if (model != null)
                    {
                        Ddl_ZBLX.SelectedValue = model.ZBLX;
                        Txt_ZBMC.Text = model.ZBMC;
                        Txt_YJZBMC.Text = model.YJZBMC;
                        Txt_EJZBMC.Text = model.EJZBMC;
                        Txt_SJZBMC.Text = model.SJZBMC;
                        Chk_ZBZT.Checked = (model.ZBZT == "1");
                        Hid_ZBID.Value = model.ZBID.ToString();
                        Pnl_Edit.Visible = true;
                    }
                    break;
                case "shanchu":
                    model = new ZbkZbxxModel();
                    model.DB_Option_Action = WebKeys.DeleteAction;
                    model.ZBID = e.CommandArgument.ToInt();
                    zbxxSrv.Execute(model);
                    BindGrid();
                    break;
                case "up":
                    model = zbxxSrv.GetSingle(e.CommandArgument.ToString());
                    if (model != null)
                    {
                        bmjgUpOneGrade(model);
                    }
                    BindGrid();
                    break;
                case "down":
                    model = zbxxSrv.GetSingle(e.CommandArgument.ToString());
                    if (model != null)
                    {
                        bmjgDownOneGrade(model);
                    }
                    BindGrid();
                    break;
                default:
                    break;
            }
        }
        #endregion


        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (currentModule != null)
            {
                string urlPrefix = "~/";
                if (!string.IsNullOrEmpty(currentModule.WEBURL.Trim()))
                {
                    Response.Redirect(urlPrefix + currentModule.WEBURL.TrimStart(urlPrefix.ToCharArray()));
                }
                else
                {
                    Response.Redirect(urlPrefix + currentModule.MURL.TrimStart(urlPrefix.ToCharArray()));
                }
            }
        }


        //页面翻页
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;//分页BindData();
            BindGrid();
        }

        /// <summary>
        /// 单位查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Find_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = false;
            BindGrid();
        }

        /// <summary>
        /// 显示添加界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = true;
            BindGrid();
        }

        /// <summary>
        /// 获取当前指标的显示序号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string getXsxh(ZbkZbxxModel model)
        {
            string xsxh = string.Empty;
            var zbxxLst = zbxxSrv.GetList().Where(p => p.ZBLX == model.ZBLX).OrderByDescending(p => p.SXH).ToList();
            if (zbxxLst != null && zbxxLst.Count > 0)
            {
                var lastZbxx = zbxxLst[0];
                int xh = lastZbxx.SXH.Substring(2).ToInt() + 1;
                if (model.ZBLX == "定量指标")
                {
                    xsxh = "01" + CommonTool.BuZero_3(xh); ;
                }
                else
                {
                    xsxh = "02" + CommonTool.BuZero_3(xh); ;
                }
            }
            else
            {
                if (model.ZBLX == "定量指标")
                {
                    xsxh = "01001";
                }
                else
                {
                    xsxh = "02001";
                }
            }
            return xsxh;
        }

        /// <summary>
        /// 上调一级
        /// </summary>
        /// <param name="model"></param>
        private void bmjgUpOneGrade(ZbkZbxxModel model)
        {
            string currSxh = model.SXH;
            var zbList = zbxxSrv.GetList().Where(p => p.ZBLX == model.ZBLX).OrderBy(p => p.SXH).ToList();
            int currIndex = zbList.FindIndex(p => p.SXH == model.SXH);
            if (currIndex > 0 && zbList.Count > 1)
            {
                var prevZbxx = zbList[currIndex - 1];
                if (prevZbxx != null)
                {
                    string prevSxh = prevZbxx.SXH;
                    prevZbxx.SXH = currSxh;
                    prevZbxx.DB_Option_Action = WebKeys.UpdateAction;
                    zbxxSrv.Execute(prevZbxx);
                    model.SXH = prevSxh;
                    model.DB_Option_Action = WebKeys.UpdateAction;
                    zbxxSrv.Execute(model);
                }
            }
        }

        /// <summary>
        /// 下调一级
        /// </summary>
        /// <param name="model"></param>
        private void bmjgDownOneGrade(ZbkZbxxModel model)
        {
            string currSxh = model.SXH;
            var zbList = zbxxSrv.GetList().Where(p => p.ZBLX == model.ZBLX).OrderBy(p => p.SXH).ToList();
            int currIndex = zbList.FindIndex(p => p.SXH == model.SXH);
            if (currIndex < zbList.Count - 1)
            {
                var nextZbxx = zbList[currIndex + 1];
                if (nextZbxx != null)
                {
                    string nextSxh = nextZbxx.SXH;
                    nextZbxx.SXH = currSxh;
                    nextZbxx.DB_Option_Action = WebKeys.UpdateAction;
                    zbxxSrv.Execute(nextZbxx);
                    model.SXH = nextSxh;
                    model.DB_Option_Action = WebKeys.UpdateAction;
                    zbxxSrv.Execute(model);
                }
            }
        }

        /// <summary>
        /// 指标类型选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Zblx_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 一级分类选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Yjzbmc_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

    }
}