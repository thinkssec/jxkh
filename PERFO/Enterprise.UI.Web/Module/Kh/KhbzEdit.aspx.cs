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
using Enterprise.Service.Perfo.Khs;
using Enterprise.Model.Perfo.Khs;
using Enterprise.Service.Perfo.Khx;
using Enterprise.Model.Perfo.Khx;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Kh
{
    /// <summary>
    /// 发起考核页面
    /// </summary>
    public partial class KhbzEdit : PageBase
    {

        /// <summary>
        /// 考核管理服务类
        /// </summary>
        KhxMbService khmbSrv = new KhxMbService();
        KhsKhdzbService khdzbSrv = new KhsKhdzbService();
        protected string Id = (string)Utility.sink("Id", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);
        public string title = "";
        bool haveDel = false;
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
                 LnkBtn_Ins.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
                bool isUpdate = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
                LnkBtn_Upd.Visible = isUpdate;
                haveDel = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Del);
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
            KhsKhdzbModel m1 = null;
            KhsKhdzbService s1 = new KhsKhdzbService();
            Hid_KHDZBID.Value = Id;
            m1 = s1.GetList().Where(p=>p.ID==Id).FirstOrDefault();
            if (m1 != null) { 
                KhKhglService s2=new KhKhglService();
                KhsDzbService s3=new KhsDzbService();
                var m2 = s2.GetList().Where(p => p.KHID == m1.KHID).FirstOrDefault();
                var m3 = s3.GetList().Where(p => p.DZBID == m1.DZBID).FirstOrDefault();
                if (m2 != null && m3 != null) {
                    title = "    <span style='color:black'>" + m2.KHMC + "</span>&nbsp;&nbsp;&nbsp;&nbsp;" + m2.Kind.LXMC + "&nbsp;&nbsp;&nbsp;&nbsp;<span style='color:red'>" + m3.ZBMC + "</span>&nbsp;&nbsp;&nbsp;&nbsp;<span style='color:purple'>" + s1.GetStatus(m1) + "</span>";
                }
            }
            Lb_Title.Text = title;
            List<KhxMbModel > list = null;
            list = khmbSrv.GetList().Where(p => p.KHDZBID == Id).ToList();
            GridView1.DataSource = list;
            GridView1.DataBind();
            if (m1 != null) {
                if (m1.STATUS == 0 || m1.STATUS == 1) { 
                    GridView1.Columns[3].Visible = false;
                    LinkButton2.Visible = false;
                    Btn_Add.Visible = false;
                }
            }
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //应用版本
            ZbkBanbenService banbenSrv = new ZbkBanbenService();
            //Ddl_BBMC.DataSource = banbenSrv.GetList();
            //Ddl_BBMC.DataTextField = "BBMC";
            //Ddl_BBMC.DataValueField = "BBMC";
            //Ddl_BBMC.DataBind();
        }

        /// <summary>
        /// 初始化清空
        /// </summary>
        protected void Clear()
        {
            Hid_ID.Value = "";
            Txt_NAME.Text = "";
            SetCntrlVisibility(LnkBtn_Ins, false);
            SetCntrlVisibility(LnkBtn_Upd, false);
            SetCntrlVisibility(LnkBtn_Del, false);
            Pnl_Edit.Visible = false;
        }

        #endregion
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            //string url = string.Format("BB={0}&BM={1}&ND={2}", Bbmc, Jgbm, Sznd);
            Response.Redirect("~/Module/Kh/KhbzList.aspx", true);
        }


        protected void LnkBtn_Ok_Click(object sender, EventArgs e)
        {
            //string url = string.Format("BB={0}&BM={1}&ND={2}", Bbmc, Jgbm, Sznd);
            KhsKhdzbModel m1 = null;
            KhsKhdzbService s1 = new KhsKhdzbService();
            Hid_KHDZBID.Value = Id;
            m1 = s1.GetList().Where(p => p.ID == Id).FirstOrDefault();
            m1.STATUS = 0;
            m1.DB_Option_Action = WebKeys.UpdateAction;
            s1.Execute(m1);
            Response.Redirect("~/Module/Kh/KhbzList.aspx", true);
        }
        #region 事件处理区

        /// <summary>
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            List<SysBmjgModel> bmList = null;
            SysBmjgService bmSrv = new SysBmjgService();
            KhsKhdzbService khdzbSrv = new KhsKhdzbService();
            KhsKhdzbModel khm = khdzbSrv.GetList().Where(p => p.ID == Id).FirstOrDefault();
            KhKhglService khglSrv = new KhKhglService();
            KhKhglModel mmmm = khglSrv.GetList().Where(p => p.KHID == khm.KHID).FirstOrDefault();
            string jglx = "";
            if (mmmm.LXID == "LX2014A") jglx = "基层单位";
            if (mmmm.LXID == "LX2014B") jglx = "职能部门";
            bmList = bmSrv.GetList().Where(p => (p.JGLX == jglx) && p.SFKH == "1").OrderBy(o => o.JGLX).ToList();
            GridView2.DataSource = bmList;
            GridView2.DataBind();
            KhsDzbService dzbSrv = new KhsDzbService();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhxMbModel model = e.Row.DataItem as KhxMbModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
               
                //string html = "";
                //e.Row.Cells[3].Text = khdzbSrv.GetStatus(model); ;
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
            var model = khmbSrv.GetList().Where(p=>p.ID==e.CommandArgument.ToString()).FirstOrDefault();
            switch (e.CommandName)
            {
                case "bianji":
                    if (model != null)
                    {
                        ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
                        CommonTool.SetModelDataToForm(model, cont, "Txt_", true);
                        CommonTool.SetModelDataToForm(model, cont, "Ddl_", true);
                        CommonTool.SetModelDataToForm(model, cont, "Chk_", true);
                        Hid_ID.Value = model.ID;
                        Pnl_Edit.Visible = true;
                        SetCntrlVisibility(LnkBtn_Ins, false);
                        SetCntrlVisibility(LnkBtn_Upd, true);
                        KhsMbjgService khdzbSrv = new KhsMbjgService();
                        List<KhsMbjgModel> rl = khdzbSrv.GetList().Where(p => p.KHDZBID == model.KHDZBID&&p.MBID==model.ID).ToList();
                        foreach (GridViewRow row in GridView2.Rows)
                        {
                            CheckBox dfws = (CheckBox)row.Cells[1].FindControl("Cb_DZB");
                            TextBox px = (TextBox)row.Cells[2].FindControl("Txt_PX");
                            string str_Id = GridView2.DataKeys[row.RowIndex].Value.ToString();
                            KhsMbjgModel md = new KhsMbjgModel();
                            md = rl.Where(p => p.JGBM == str_Id.ToInt()).FirstOrDefault();
                            if (md != null)
                            {
                                dfws.Checked = true;
                                px.Text = md.PX.ToRequestString();
                            }
                            else
                            {
                                dfws.Checked = false;
                                px.Text = "0";
                            }
                        }
                    }
                    break;
                case "khnr":
                    Response.Redirect("~/Module/KhX/KhZbLr.aspx?mbid=" + model.ID+"&bm="+this.userModel.Bmjg.JGBM+"&id="+model.KHDZBID, true);
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
            KhxMbModel model = (KhxMbModel)CommonTool.GetFormDataToModel(typeof(KhxMbModel), cont);
            model.DB_Option_Action = WebKeys.InsertAction;
            model.ID = Guid.NewGuid().ToRequestString();
            if(khmbSrv.Execute(model)){
                KhsMbjgService khdzbSrv = new KhsMbjgService();
                khdzbSrv.DeleteById(model.ID ,model.KHDZBID);
                foreach (GridViewRow row in GridView2.Rows)
                {
                    CheckBox dfws = (CheckBox)row.Cells[1].FindControl("Cb_DZB");
                    if (!dfws.Checked) continue;
                    TextBox px = (TextBox)row.Cells[2].FindControl("Txt_PX");
                    string str_Id = GridView2.DataKeys[row.RowIndex].Value.ToString();
                    KhsMbjgModel md = new KhsMbjgModel();
                    md.ID = model.ID + str_Id;
                    md.KHDZBID = model.KHDZBID;
                    md.MBID = model.ID;
                    md.JGBM = str_Id.ToInt();
                    md.PX = px.Text.ToDecimal();
                    md.DB_Option_Action = WebKeys.InsertAction;
                    khdzbSrv.Execute(md);
                }
             };
            Clear();
            BindGrid();
            //model.KSSJ = Cal_KSSJ.SelectedDate;
            ////model.SFKC = (Chk_SFKC.Checked) ? "1" : "0";
            ////add
            //if (khglSrv.Execute(model))
            //{
            //    //添加运行节点
            //    //KhNoderunService runSrv = new KhNoderunService();
            //    //runSrv.InitNoderunData(model);
            //    KhsKhdzbService khdzbSrv = new KhsKhdzbService();
            //    khdzbSrv.DeleteByKhId(model.KHID+"");
            //    foreach (GridViewRow row in GridView2.Rows)
            //    {
            //        CheckBox dfws = (CheckBox)row.Cells[2].FindControl("Cb_DZB");
            //        if (!dfws.Checked) continue;
            //        TextBox px = (TextBox)row.Cells[3].FindControl("Txt_PX");
            //        string str_Id = GridView2.DataKeys[row.RowIndex].Value.ToString();
            //        KhsKhdzbModel md = new KhsKhdzbModel();
            //        md.ID = Guid.NewGuid().ToRequestString();
            //        md.KHID = model.KHID;
            //        md.DZBID = str_Id;
            //        md.PX = px.Text.ToDecimal();
            //        md.DB_Option_Action = WebKeys.InsertAction;
            //        khdzbSrv.Execute(md);
            //    }
            //}

            //Clear();
            //BindGrid();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Hid_ID.Value))
            {
                ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
                KhxMbModel model = (KhxMbModel)CommonTool.GetFormDataToModel(typeof(KhxMbModel), cont);
                model.DB_Option_Action = WebKeys.UpdateAction;
                if (khmbSrv.Execute(model))
                {
                    KhsMbjgService khdzbSrv = new KhsMbjgService();
                    khdzbSrv.DeleteById(model.ID ,model.KHDZBID);
                    foreach (GridViewRow row in GridView2.Rows)
                    {
                        CheckBox dfws = (CheckBox)row.Cells[1].FindControl("Cb_DZB");
                        if (!dfws.Checked) continue;
                        TextBox px = (TextBox)row.Cells[2].FindControl("Txt_PX");
                        string str_Id = GridView2.DataKeys[row.RowIndex].Value.ToString();
                        KhsMbjgModel md = new KhsMbjgModel();
                        md.ID = model.ID + str_Id;
                        md.MBID = model.ID;
                        md.KHDZBID = model.KHDZBID;
                        md.JGBM = str_Id.ToInt();
                        md.PX = px.Text.ToDecimal();
                        md.DB_Option_Action = WebKeys.InsertAction;
                        khdzbSrv.Execute(md);
                    }
                };
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
            if (!string.IsNullOrEmpty(Hid_ID.Value))
            {
                //doing......关联表较多，后面再处理
                KhxMbModel model = khmbSrv.GetList().Where(p=>p.ID==Hid_ID.Value).FirstOrDefault();
                if (model != null)
                {
                    model.DB_Option_Action = WebKeys.DeleteAction;
                    if (khmbSrv.Execute(model)) {
                        khdzbSrv.DeleteMb(model.ID);
                    };
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
        //protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        //{
        //    GobackPageUrl("~/Module/Kh/KhbzList.aspx");
        //}

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
            List<SysBmjgModel> bmList = null;
            SysBmjgService bmSrv = new SysBmjgService();
            //bmList = bmSrv.GetList().Where(p => (p.JGLX == "基层单位" || p.JGLX == "职能部门") && p.SFKH == "1").OrderBy(o => o.JGLX).ToList();
            KhsKhdzbService khdzbSrv = new KhsKhdzbService();
            KhsKhdzbModel khm = khdzbSrv.GetList().Where(p => p.ID == Id).FirstOrDefault();
            KhKhglService khglSrv = new KhKhglService();
            KhKhglModel mmmm = khglSrv.GetList().Where(p => p.KHID == khm.KHID).FirstOrDefault();
            string jglx = "";
            if (mmmm.LXID == "LX2014A") jglx = "基层单位";
            if (mmmm.LXID == "LX2014B") jglx = "职能部门";
            bmList = bmSrv.GetList().Where(p => (p.JGLX == jglx) && p.SFKH == "1").OrderBy(o => o.JGLX).ToList();
            GridView2.DataSource = bmList;
            GridView2.DataBind();
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