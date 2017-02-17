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
    /// 发起考核页面
    /// </summary>
    public partial class FQKaohe : PageBase
    {

        /// <summary>
        /// 考核管理服务类
        /// </summary>
        KhKhglService khglSrv = new KhKhglService();

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
                bool isUpdate = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
                LnkBtn_Upd.Visible = isUpdate;
                LnkBtn_Del.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Del); 
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
            List<KhKhglModel> list = null;
            if (!string.IsNullOrEmpty(Txt_Khmc_Search.Text))
            {
                list = khglSrv.GetKhListByYear(Ddl_Niandu.SelectedValue).Where(p => p.KHMC.Contains(Txt_Khmc_Search.Text)).ToList();
            }
            else
            {
                list = khglSrv.GetKhListByYear(Ddl_Niandu.SelectedValue) as List<KhKhglModel>;
            }
            GridView1.DataSource = list;
            GridView1.DataBind();
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //应用版本
            ZbkBanbenService banbenSrv = new ZbkBanbenService();
            Ddl_BBMC.DataSource = banbenSrv.GetList();
            Ddl_BBMC.DataTextField = "BBMC";
            Ddl_BBMC.DataValueField = "BBMC";
            Ddl_BBMC.DataBind();

            //年度
            Ddl_Niandu.Items.Clear();
            for (int i = 2014; i <= DateTime.Now.Year + 1; i++)
            {
                Ddl_Niandu.Items.Add(new ListItem(i + "年", i.ToString()));
            }
            Ddl_Niandu.SelectedValue = DateTime.Now.Year.ToString();

            //考核类型
            KhKindService kindSrv = new KhKindService();
            Ddl_LXID.DataSource = kindSrv.GetList();
            Ddl_LXID.DataTextField = "LXMC";
            Ddl_LXID.DataValueField = "LXID";
            Ddl_LXID.DataBind();
        }

        /// <summary>
        /// 初始化清空
        /// </summary>
        protected void Clear()
        {
            Hid_KHID.Value = "";
            Txt_KHMC.Text = "";
            Txt_KHND.Text = "";
            SetCntrlVisibility(LnkBtn_Ins, false);
            SetCntrlVisibility(LnkBtn_Upd, false);
            SetCntrlVisibility(LnkBtn_Del, false);
            Pnl_Edit.Visible = false;
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
                KhKhglModel model = e.Row.DataItem as KhKhglModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //考核类型
                e.Row.Cells[2].Text = model.Kind.LXMC;
                //考核状态
                if (e.Row.Cells[6].Text == "0")
                {
                    e.Row.Cells[6].Text = "<img src=\"/Resources/Images/lock_unlock.png \" title=\"进行中\" alt=\"进行中\"/>";
                }
                else
                {
                    e.Row.Cells[6].Text = "<img src=\"/Resources/Images/lock.png\" title=\"已完成\" alt=\"已完成\"/>";
                }
            }
        }

        /// <summary>
        /// 换页操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;;
            BindGrid();
        }

        /// <summary>
        /// 行命令处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var model = khglSrv.GetSingle(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                case "bianji":
                    if (model != null)
                    {
                        ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
                        CommonTool.SetModelDataToForm(model, cont, "Txt_", true);
                        CommonTool.SetModelDataToForm(model, cont, "Ddl_", true);
                        CommonTool.SetModelDataToForm(model, cont, "Chk_", true);
                        Hid_KHID.Value = model.KHID.ToString();
                        Chk_SFKC.Checked = (model.SFKC == "1");
                        Cal_KSSJ.SelectedDate = (model.KSSJ != null) ? model.KSSJ.ToDateTime() : DateTime.Today;
                        Pnl_Edit.Visible = true;
                        SetCntrlVisibility(LnkBtn_Ins, false);
                        SetCntrlVisibility(LnkBtn_Upd, true);
                        SetCntrlVisibility(LnkBtn_Del, true);
                    }
                    break;
                case "gbzt":
                    if (model != null)
                    {
                        //实现考核状态的切换
                        model.KHZT = (model.KHZT == "1") ? "0" : "1";
                        if (model.KHZT == "1")
                        {
                            model.GBSJ = DateTime.Now;
                        }
                        else
                        {
                            model.GBSJ = null;
                        }
                        model.DB_Option_Action = WebKeys.UpdateAction;
                        khglSrv.Execute(model);
                    }
                    BindGrid();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Find_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 版本切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Bbmc_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
            KhKhglModel model = (KhKhglModel)CommonTool.GetFormDataToModel(typeof(KhKhglModel), cont);
            model.DB_Option_Action = WebKeys.InsertAction;
            model.KSSJ = Cal_KSSJ.SelectedDate;
            model.SFKC = (Chk_SFKC.Checked) ? "1" : "0";
            //add
            if (khglSrv.Execute(model))
            {
                //添加运行节点
                KhNoderunService runSrv = new KhNoderunService();
                runSrv.InitNoderunData(model);
            }

            Clear();
            BindGrid();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Hid_KHID.Value))
            {
                ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
                KhKhglModel model = (KhKhglModel)CommonTool.GetFormDataToModel(typeof(KhKhglModel), cont);
                model.DB_Option_Action = WebKeys.UpdateAction;
                model.KSSJ = Cal_KSSJ.SelectedDate;
                model.SFKC = (Chk_SFKC.Checked) ? "1" : "0";
                //update
                khglSrv.Execute(model);
            }
            Clear();
            BindGrid();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Del_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Hid_KHID.Value))
            {
                //doing......关联表较多，后面再处理
                KhKhglModel model = khglSrv.GetSingle(Hid_KHID.Value);
                if (model != null && khglSrv.DeleteAllDataByKhid(Hid_KHID.Value))
                {
                    model.DB_Option_Action = WebKeys.DeleteAction;
                    khglSrv.Execute(model);
                }
            }
            Clear();
            BindGrid();
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

        /// <summary>
        /// 显示编辑面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Clear();
            SetCntrlVisibility(LnkBtn_Ins, true);
            SetCntrlVisibility(LnkBtn_Upd, false);
            SetCntrlVisibility(LnkBtn_Del, false);
            Pnl_Edit.Visible = true;
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

        #endregion
    }
}