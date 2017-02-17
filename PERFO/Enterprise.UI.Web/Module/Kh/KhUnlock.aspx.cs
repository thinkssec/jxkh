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
    /// 数据解锁操作页面
    /// </summary>
    public partial class KhUnlock : PageBase
    {

        KhUnlockService unlockSrv = new KhUnlockService();//数据锁定服务类
        KhKhglService khglSrv = new KhKhglService();//考核管理服务类

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
                bool isInsert = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
                LnkBtn_All.Visible = isInsert;
                LnkBtn_Lock.Visible = isInsert;
                LnkBtn_UnAll.Visible = isInsert;
                LnkBtn_UnLock.Visible = isInsert;
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
            //数据解锁
            GridView1.DataSource = unlockSrv.GetListByKhid(Ddl_Kaohe.SelectedValue);
            GridView1.DataBind();
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
                KhUnlockModel model = e.Row.DataItem as KhUnlockModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //单位名称 1
                e.Row.Cells[1].Text = model.Bmjg.JGMC;
                //机构类型 2
                e.Row.Cells[2].Text = model.Bmjg.JGLX;
                //数据状态 3
                if (model.SDBZ == "1")
                {
                    e.Row.Cells[3].Text = "<img src=\"/Resources/Images/lock.png\" title=\"已锁定\" alt=\"已锁定\"/>";
                }
                else
                {
                    e.Row.Cells[3].Text = "<img src=\"/Resources/Images/lock_unlock.png \" title=\"未锁定\" alt=\"未锁定\"/>";
                }
                //提交人   4
                e.Row.Cells[4].Text = model.CZZ;
                //提交时间 5
                e.Row.Cells[5].Text = (model.TJSJ != null) ? model.TJSJ.Value.ToString("yyyy年MM月dd日 HH:mm") : "";
                
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
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_All_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox cb = (CheckBox)row.Cells[0].FindControl("CheckBox1");
                cb.Checked = true;
            }
        }

        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_UnAll_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox cb = (CheckBox)row.Cells[0].FindControl("CheckBox1");
                cb.Checked = false;
            }
        }

        /// <summary>
        /// 锁定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Lock_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox cb = (CheckBox)row.Cells[0].FindControl("CheckBox1");
                if (cb.Checked)
                {
                    string SID = GridView1.DataKeys[row.RowIndex].Values["SID"].ToRequestString();
                    var q = unlockSrv.GetSingle(SID);
                    if (q != null)
                    {
                        q.DB_Option_Action = WebKeys.UpdateAction;
                        q.SDBZ = "1";
                        q.TJSJ = DateTime.Now;
                        q.CZZ = userModel.USERNAME;
                        unlockSrv.Execute(q);
                    }
                }
            }
            BindGrid();
        }

        /// <summary>
        /// 解除锁定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_UnLock_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox cb = (CheckBox)row.Cells[0].FindControl("CheckBox1");
                if (cb.Checked)
                {
                    string SID = GridView1.DataKeys[row.RowIndex].Values["SID"].ToRequestString();
                    var q = unlockSrv.GetSingle(SID);
                    if (q != null)
                    {
                        q.DB_Option_Action = WebKeys.UpdateAction;
                        q.SDBZ = "0";
                        q.CZZ = null;
                        q.TJSJ = null;
                        unlockSrv.Execute(q);
                    }
                }
            }
            BindGrid();
        }

        #endregion

        

    }
}