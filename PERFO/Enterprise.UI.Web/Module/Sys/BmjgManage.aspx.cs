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
using Enterprise.Model.Perfo.Sys;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Sys
{

    /// <summary>
    /// 机构管理页面
    /// </summary>
    public partial class BmjgManage : PageBase
    {

        /// <summary>
        /// 部门机构服务类
        /// </summary>
        SysBmjgService bmjgSrv = new SysBmjgService();

        #region 权限检查
        
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDDL();
                BindGrid();
            }
        }

        /// <summary>
        /// 绑定上级机构
        /// </summary>
        protected void BindDDL()
        {
            Ddl_XSXH.DataSource = bmjgSrv.GetBmjgTreeLst(false);
            Ddl_XSXH.DataTextField = "JGMC";
            Ddl_XSXH.DataValueField = "XSXH";
            Ddl_XSXH.DataBind();
        }

        #region 绑定表格

        /// <summary>
        /// 绑定表格
        /// </summary>
        protected void BindGrid()
        {
            List<SysBmjgModel> bmjgList = null;
            //机构名称查询
            if (!string.IsNullOrEmpty(Txt_JGMC_Search.Text))
            {
                bmjgList = bmjgSrv.GetBmjgTreeLst(true).Where(p => p.JGMC.Contains(Txt_JGMC_Search.Text)).OrderBy(p => p.XSXH).ToList();
            }
            else
            {
                if (Rdl_OrderBy.SelectedValue.Contains("同级"))
                {
                    bmjgList = bmjgSrv.GetBmjgTreeLst(true).Where(p=>!string.IsNullOrEmpty(p.JGLX)).OrderBy(p => p.JGLX).ThenBy(p=>p.BZ).ToList();
                    GridView1.Columns[GridView1.Columns.Count - 1].Visible = true;
                }
                else
                {
                    bmjgList = bmjgSrv.GetBmjgTreeLst(true).OrderBy(p => p.XSXH).ToList();
                    GridView1.Columns[GridView1.Columns.Count - 1].Visible = true;
                }
            }

            if (bmjgList != null)
            {
                GridView1.DataSource = bmjgList;
                GridView1.DataBind();
            }
        }
        #endregion


        #region 隔行换色
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SysBmjgModel model = e.Row.DataItem as SysBmjgModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#dddddd'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //序号
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();

                //if (model.XSXH.Length <= 4)
                //{
                //    e.Row.Cells[5].Text = "";
                //}
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
            if (!String.IsNullOrEmpty(Hid_JGBM.Value))
            {
                SysBmjgModel model = bmjgSrv.GetSingle(Hid_JGBM.Value);
                if (model == null)
                {
                    model = new SysBmjgModel();
                    model.JGBM = Hid_JGBM.Value.ToInt();
                    model.XSXH = getXsxh(Ddl_XSXH.SelectedValue);
                }
                model.DB_Option_Action = WebKeys.InsertOrUpdateAction;
                model.JGMC = Txt_JGMC.Text;
                model.JGLX = Ddl_JGLX.SelectedValue;
                model.BZ = Txt_BZ.Text;
                model.SFKH = (Chk_SFKH.Checked) ? "1" : "0";
                //submit
                bmjgSrv.Execute(model);
            }
            clearText();
            BindGrid();         
        }

        /// <summary>
        /// 清空内容
        /// </summary>
        private void clearText()
        {
            Txt_JGMC.Text = "";
            Txt_JGMC_Search.Text = "";
            Txt_BZ.Text = "";
            Hid_JGBM.Value = "";
            btn_save.Text = "保存";
            Pnl_Edit.Visible = false;
        }

        #endregion


        #region 操作事件 编辑/改变状态/删除
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SysBmjgModel model = null;
            switch (e.CommandName)
            {
                case "bianji":
                    model = bmjgSrv.GetSingle(e.CommandArgument.ToString());
                    if (model != null)
                    {
                        if (model.XSXH.Length > 2)
                        {
                            Ddl_XSXH.SelectedValue = model.XSXH.Substring(0, model.XSXH.Length - 2);
                            Ddl_XSXH.Enabled = true;
                        }
                        else
                        {
                            Ddl_XSXH.Enabled = false;
                        }
                        Txt_JGMC.Text = model.JGMC;
                        Txt_BZ.Text = model.BZ;
                        Ddl_JGLX.SelectedValue = model.JGLX;
                        Chk_SFKH.Checked = (model.SFKH == "1");
                        Hid_JGBM.Value = model.JGBM.ToString();
                        Pnl_Edit.Visible = true;
                    }
                    break;
                case "shanchu":
                    model = new SysBmjgModel();
                    model.DB_Option_Action = WebKeys.DeleteAction;
                    model.JGBM = e.CommandArgument.ToInt();
                    bmjgSrv.Execute(model);
                    BindGrid();
                    break;
                case "up":
                    model = bmjgSrv.GetSingle(e.CommandArgument.ToString());
                    if (model != null)
                    {
                        bmjgUpOneGrade(model.XSXH, model.BZ);
                    }
                    BindGrid();
                    break;
                case "down":
                    model = bmjgSrv.GetSingle(e.CommandArgument.ToString());
                    if (model != null)
                    {
                        bmjgDownOneGrade(model.XSXH, model.BZ);
                    }
                    BindGrid();
                    break;
                default:
                    btn_save.Text = "保存";
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
            Ddl_XSXH.Enabled = true;
            int jgbm = bmjgSrv.GetList().Max(p => p.JGBM) + 1;
            Hid_JGBM.Value = jgbm.ToString();
            BindGrid();
        }

        /// <summary>
        /// 获取当前单位的显示序号
        /// </summary>
        /// <param name="parentXsxh"></param>
        /// <returns></returns>
        private string getXsxh(string parentXsxh)
        {
            string xsxh = string.Empty;
            var bmjgLst = bmjgSrv.GetList();
            var parentBmjg = bmjgLst.FirstOrDefault(p => p.XSXH == parentXsxh);
            if (parentBmjg != null)
            {
                var subBmjgLst =  bmjgLst.Where(p => p.XSXH.StartsWith(parentXsxh) && p.XSXH.Length == parentXsxh.Length + 2);
                if (subBmjgLst != null && subBmjgLst.Count<SysBmjgModel>() > 0)
                {
                    var lastBmjg = subBmjgLst.OrderBy(p => p.XSXH).Last();
                    if (lastBmjg != null)
                    {
                        int xh = lastBmjg.XSXH.Substring(parentXsxh.Length).ToInt() + 1;
                        xsxh = parentXsxh + CommonTool.BuZero_2(xh);
                    }
                    else
                    {
                        xsxh = parentXsxh + "01";
                    }
                }
                else
                {
                    xsxh = parentXsxh + "01";
                }
            }
            return xsxh;
        }

        /// <summary>
        /// 单位上调一级
        /// </summary>
        /// <param name="xsxh"></param>
        /// <param name="bz"></param>
        private void bmjgUpOneGrade(string xsxh, string bz)
        {
            if (Rdl_OrderBy.SelectedValue.Contains("同级"))
            {
                var bmjgLst = bmjgSrv.GetList().Where(p => p.XSXH.Length == xsxh.Length).OrderBy(p=>p.BZ).ToList();
                int currIndex = bmjgLst.FindIndex(p => p.XSXH == xsxh);
                if (currIndex > 0 && bmjgLst.Count > 0)
                {
                    var prevBmjg = bmjgLst[currIndex - 1];
                    var currBmjg = bmjgLst[currIndex];
                    currBmjg.BZ = prevBmjg.BZ;
                    currBmjg.DB_Option_Action = WebKeys.UpdateAction;
                    bmjgSrv.Execute(currBmjg);
                    prevBmjg.BZ = bz;
                    prevBmjg.DB_Option_Action = WebKeys.UpdateAction;
                    bmjgSrv.Execute(prevBmjg); 
                }
            }
            else
            {
                int xh = xsxh.Substring(xsxh.Length - 2).ToInt();
                string parentXsxh = xsxh.Substring(0, xsxh.Length - 2);
                var bmjgLst = bmjgSrv.GetList().Where(p => p.XSXH.StartsWith(parentXsxh) && p.XSXH.Length == xsxh.Length).OrderBy(p => p.XSXH).ToList();
                if (xh > 1 && bmjgLst.Count >= xh)
                {
                    var prevBmjg = bmjgLst[xh - 2];//索引以0起，序号以1起
                    var currBmjg = bmjgLst[xh - 1];
                    currBmjg.XSXH = prevBmjg.XSXH;
                    currBmjg.DB_Option_Action = WebKeys.UpdateAction;
                    bmjgSrv.Execute(currBmjg);
                    prevBmjg.XSXH = xsxh;
                    prevBmjg.DB_Option_Action = WebKeys.UpdateAction;
                    bmjgSrv.Execute(prevBmjg);
                }
            }
        }

        /// <summary>
        /// 单位下调一级
        /// </summary>
        /// <param name="xsxh"></param>
        /// <param name="bz"></param>
        private void bmjgDownOneGrade(string xsxh, string bz)
        {
            if (Rdl_OrderBy.SelectedValue.Contains("同级"))
            {
                var bmjgLst = bmjgSrv.GetList().Where(p => p.XSXH.Length == xsxh.Length).OrderBy(p => p.BZ).ToList();
                int currIndex = bmjgLst.FindIndex(p => p.XSXH == xsxh);
                if (currIndex < bmjgLst.Count - 1)
                {
                    var nextBmjg = bmjgLst[currIndex + 1];
                    var currBmjg = bmjgLst[currIndex];
                    currBmjg.BZ = nextBmjg.BZ;
                    currBmjg.DB_Option_Action = WebKeys.UpdateAction;
                    bmjgSrv.Execute(currBmjg);
                    nextBmjg.BZ = bz;
                    nextBmjg.DB_Option_Action = WebKeys.UpdateAction;
                    bmjgSrv.Execute(nextBmjg);
                }
            }
            else
            {
                int xh = xsxh.Substring(xsxh.Length - 2).ToInt();
                string parentXsxh = xsxh.Substring(0, xsxh.Length - 2);
                var bmjgLst = bmjgSrv.GetList().Where(p => p.XSXH.StartsWith(parentXsxh) && p.XSXH.Length == xsxh.Length).OrderBy(p => p.XSXH).ToList();
                if (xh < bmjgLst.Count)
                {
                    var nextBmjg = bmjgLst[xh];//索引以0起，序号以1起
                    var currBmjg = bmjgLst[xh - 1];
                    currBmjg.XSXH = nextBmjg.XSXH;
                    currBmjg.DB_Option_Action = WebKeys.UpdateAction;
                    bmjgSrv.Execute(currBmjg);
                    nextBmjg.XSXH = xsxh;
                    nextBmjg.DB_Option_Action = WebKeys.UpdateAction;
                    bmjgSrv.Execute(nextBmjg);
                }
            }
        }

        /// <summary>
        /// 排序显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Rdl_OrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

    }
}