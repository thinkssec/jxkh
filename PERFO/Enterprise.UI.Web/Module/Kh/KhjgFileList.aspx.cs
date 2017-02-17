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
    /// 考核结果文件维护页面
    /// </summary>
    public partial class KhjgFileList : PageBase
    {

        /// <summary>
        /// 考核结果文件-服务类
        /// </summary>
        KhZzjgfileService zzjgFileSrv = new KhZzjgfileService();
        KhKhglService khglSrv = new KhKhglService();//考核管理服务类

        protected string Sznd = (string)Utility.sink("ND", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID

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
                bool isUpdate = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
                Btn_Add.Visible = LnkBtn_Ins.Visible = isUpdate;
                GridView2.Columns[GridView2.Columns.Count - 1].Visible = isUpdate;
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
            //获取指定年度下的考核
            var khList = khglSrv.GetKhListByYear(Ddl_Niandu.SelectedValue);
            //已有结果文件的所有考核信息
            var zzjgList = zzjgFileSrv.GetListByYear(Ddl_Niandu.SelectedValue).ToList();
            Ddl_KHID.Items.Clear();
            foreach (var kh in khList)
            {
                if (!zzjgList.Exists(p => p.KHID == kh.KHID))
                {
                    Ddl_KHID.Items.Add(new ListItem(kh.KHMC, kh.KHID.ToString()));
                }
            }

            Lbl_Msg.Text = string.Format("{0}年度共有{1}期考核,已上传{2}期的考核结果文件!", Ddl_Niandu.SelectedValue,
                khList.Count, zzjgList.Count);

            //绑定数据
            GridView2.DataSource = zzjgList;
            GridView2.DataBind();
            
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //年度
            Ddl_Niandu.Items.Clear();
            for (int i = 2014; i <= DateTime.Now.Year + 1; i++)
            {
                Ddl_Niandu.Items.Add(new ListItem(i + "年", i.ToString()));
            }
            if (!string.IsNullOrEmpty(Sznd))
            {
                Ddl_Niandu.SelectedValue = Sznd;
            }
            else
            {
                Ddl_Niandu.SelectedValue = DateTime.Now.Year.ToString();
            }
        }

        #endregion


        #region 事件处理区

        /// <summary>
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhZzjgfileModel model = e.Row.DataItem as KhZzjgfileModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //考核名称
                e.Row.Cells[1].Text = khglSrv.GetSingle(model.KHID.ToString()).KHMC;
                //考核文件
                e.Row.Cells[2].Text = model.ZZJGFILE.ToAttachHtmlByOne();
            }
        }

        /// <summary>
        /// 行命令处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string khid = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "bianji":
                    var q = zzjgFileSrv.GetSingle(khid);
                    if (q != null)
                    {
                        Ddl_KHID.Visible = false;
                        Lbl_Khmc.Text = khglSrv.GetSingle(q.KHID.ToString()).KHMC;
                        Hid_KHID.Value = q.KHID.ToString();
                        Txt_ZZJGFILE.Value = Txt_ZZJGFILE.Text = q.ZZJGFILE;
                        Pnl_Edit.Visible = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            KhZzjgfileModel model = null;
            if (!string.IsNullOrEmpty(Hid_KHID.Value))
            {
                //update
                model = zzjgFileSrv.GetSingle(Hid_KHID.Value);
                if (model != null)
                {
                    model.DB_Option_Action = WebKeys.UpdateAction;
                    model.ZZJGFILE = Txt_ZZJGFILE.Value;
                }
            }
            else
            {
                //add
                model = new KhZzjgfileModel();
                model.DB_Option_Action = WebKeys.InsertAction;
                model.KHID = Ddl_KHID.SelectedValue.ToInt();
                model.ZZJGFILE = Txt_ZZJGFILE.Value;
            }
            if (zzjgFileSrv.Execute(model))
            {
                Utility.ShowMsg(Page, "系统提示", "考核结果文件上传成功!", 100, "show");
            }
            Pnl_Edit.Visible = false;
            Ddl_KHID.ClearSelection();
            Txt_ZZJGFILE.Text = Txt_ZZJGFILE.Value = "";
            Lbl_Msg.Text = "";
            BindGrid();
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            string url = string.Format("ND={0}", Ddl_Niandu.SelectedValue);
            GobackPageUrl("?" + url);
        }

        /// <summary>
        /// 年度切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Niandu_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = true;
            Lbl_Khmc.Visible = false;
        }

        #endregion

    }
}