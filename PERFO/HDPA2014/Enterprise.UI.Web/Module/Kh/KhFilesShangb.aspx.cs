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
    /// 考核数据文件上报维护页面
    /// </summary>
    public partial class KhFilesShangb : PageBase
    {

        /// <summary>
        /// 数据上报-服务类
        /// </summary>
        KhSjsbService sjsbSrv = new KhSjsbService();
        KhKhglService khglSrv = new KhKhglService();//考核管理服务类

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID

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
                bool isUpdate = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
                LnkBtn_Ins.Visible = isUpdate;
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = isUpdate;
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
            //1==二级单位
            GridView1.DataSource = sjsbSrv.GetListByKhid(Ddl_Kaohe.SelectedValue).
                Where(p => p.Bmjg.JGLX.Contains("二级")).ToList();
            GridView1.DataBind();            
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //考核期
            var kaoheLst = khglSrv.GetKhListForValid().Where(p => p.LXID == "LX2014A").ToList();
            khglSrv.BindSSECDropDownListForKaohe(Ddl_Kaohe, kaoheLst);
            if (!string.IsNullOrEmpty(Khid))
            {
                Ddl_Kaohe.SelectedValue = Khid;
            }
            else if (kaoheLst.Count > 0)
            {
                Ddl_Kaohe.SelectedValue = kaoheLst.First().KHID.ToString();//最近一次考核
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
                KhSjsbModel model = e.Row.DataItem as KhSjsbModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //单位名称 1
                e.Row.Cells[1].Text = model.Bmjg.JGMC;
                //机构类型 2
                e.Row.Cells[2].Text = model.Bmjg.JGLX;
                //考核文件及数量 3
                if (!string.IsNullOrEmpty(model.SBFJ))
                {
                    if (ChkUserLuruPermission(Ddl_Kaohe.SelectedValue, model.Bmjg.JGBM, userModel))
                    {
                        e.Row.Cells[3].Text = model.SBFJ.ToAttachHtmlByOne();
                    }
                    else
                    {
                        e.Row.Cells[3].Text = "已上报";
                    }
                }
                else
                {
                    e.Row.Cells[3].Text = "未上报";
                }
                //提交人   4
                e.Row.Cells[4].Text = model.SBR;
                //提交时间 5
                e.Row.Cells[5].Text = (model.SBSJ != null) ? model.SBSJ.Value.ToString("yyyy年MM月dd日 HH:mm") : "";
                //操作 6
                if (model.JGBM != userModel.JGBM && userModel.ROLEID != "paadmin")
                {
                    e.Row.Cells[6].Text = "";
                }

            }
        }

        /// <summary>
        /// 行命令处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string jgbm = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "bianji":
                    var q = sjsbSrv.GetListByKhid(Ddl_Kaohe.SelectedValue).FirstOrDefault(p => p.JGBM == jgbm.ToInt());
                    if (q != null)
                    {
                        Lbl_JGMC.Text = q.Bmjg.JGMC;
                        Hid_JGBM.Value = q.JGBM.ToString();
                        Txt_SBFJ.Text = Txt_SBFJ.Value = q.SBFJ;
                        Pnl_Edit.Visible = true;
                    }
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
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Hid_JGBM.Value))
            {
                var sjsbM = sjsbSrv.GetListByKhid(Ddl_Kaohe.SelectedValue).FirstOrDefault(p=>p.JGBM == Hid_JGBM.Value.ToInt());
                if (sjsbM != null)
                {
                    sjsbM.DB_Option_Action = WebKeys.UpdateAction;
                    sjsbM.SBZT = "1";
                    sjsbM.SBSJ = DateTime.Now;
                    sjsbM.SBR = userModel.USERNAME;
                    sjsbM.SBFJ = Txt_SBFJ.Value;
                    sjsbSrv.Execute(sjsbM);
                    Utility.ShowMsg(Page, "提示", "考核文件上报成功!", 100, "show");
                }
            }
            Pnl_Edit.Visible = false;
            Hid_JGBM.Value = "";
            Txt_SBFJ.Text = Txt_SBFJ.Value = "";
            Lbl_JGMC.Text = "";
            BindGrid();
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            string url = string.Format("KH={0}", Ddl_Kaohe.SelectedValue);
            GobackPageUrl("?" + url);
        }

        #endregion

    }
}