﻿using System;
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
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Kh
{
    /// <summary>
    /// 发起考核页面
    /// </summary>
    public partial class KhfjList : PageBase
    {

        /// <summary>
        /// 考核管理服务类
        /// </summary>
        KhKhglService khglSrv = new KhKhglService();
        protected string khId = (string)Utility.sink("KHID", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);
        protected string Id = (string)Utility.sink("MBJGID", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);
        public string jgbm = (string)Utility.sink("JGBM", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);
        public string fjhtml = "";
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
            }
        }

        #region 专用方法区

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindGrid() 
        {
            if(!string.IsNullOrEmpty(jgbm))
            Lb_Title.Text = SysBmjgService.GetBmjgName(jgbm.ToInt());
            Hid_MBJGID.Value = Id;
            KhsMbjgService mbjgSrv = new KhsMbjgService();
            KhsMbjgModel Model = mbjgSrv.GetList().Where(p => p.ID == Hid_MBJGID.Value).FirstOrDefault();
            Txt_SCFJ.DBValue = Model.FVIEWNAMES.ToRequestString();
            Txt_SCFJ.Text = Model.FNAMES.ToRequestString();
            fjhtml = ToAttachHtml2(Model.FVIEWNAMES);
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            KhsMbjgService mbSrv = new KhsMbjgService();
            KhsMbjgModel Model = mbSrv.GetList().Where(p=>p.ID==Hid_MBJGID.Value).FirstOrDefault();
            Model.FNAMES = Txt_SCFJ.Text;
            Model.FVIEWNAMES = Txt_SCFJ.DBValue;
            Model.DB_Option_Action = WebKeys.UpdateAction;
            bool isOk = mbSrv.Execute(Model);
            Response.Redirect("~/Module/Kh/KhfjList.aspx?MBJGID=" + Id + "&JGBM=" + jgbm , true);
            //BindGrid();
        }

        public static string ToAttachHtml2(object article)
        {
            string dbFilesString = article.ToRequestString();

            string url = "<a href='/Module/Common/EntDisk/Content/Ashx/FileDownload.ashx?url={0}'><img src='/Resources/OA/site_skin/default/img/page.png'  border=\"0\"/></a>";
            string html = "<ul>";
            if (!string.IsNullOrEmpty(dbFilesString))
            {
                string[] str = dbFilesString.Split('|');
                for (int i = 0; i < str.Length; i++)
                {
                    string[] tmp = str[i].Split('*');
                    string ext = System.IO.Path.GetExtension(Utility.GetFileExt(tmp.Last()));
                    html += string.Format("<li><a class=\"easyui-linkbutton\" data-options=\"iconCls:'icon-" + ext.Replace(".", "") + "',plain:true\" href=\"/Module/Common/EntDisk/Content/Ashx/FileDownload.ashx?url={0}\" target='_blank'>{2}</a></li>",
                        tmp.Last(),
                        FileHelper.Encrypt(tmp.Last()),
                        tmp.First());
                }
                html += "</ul>";
            }
            return html;
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
            //SetCntrlVisibility(LnkBtn_Del, false);
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
                KhsMbjgModel model = e.Row.DataItem as KhsMbjgModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

                string sql = string.Format(@"select sum(score) sm from PERFO_KHX_RESULT where mbjgid='"+model.ID+"'");
                DataSet ds = khglSrv.getDsBySql(sql);
                string sm = "";
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    sm = r["SM"].ToRequestString();
                }
                e.Row.Cells[3].Text = "<a href='#' onclick=\"openwin('openwin','/Module/Kh/KhfjList.aspx?MBJGID=" + model.ID+ "','600','360')\">" + sm + "</a>";
                //考核类型
                //e.Row.Cells[2].Text = model.Kind.LXMC;
                ////考核状态
                //e.Row.Cells[6].Text = khglSrv.GetStatus(model);
                //if (e.Row.Cells[6].Text == "2")
                //{
                //    e.Row.Cells[6].Text = "<img src=\"/Resources/Images/lock_unlock.png \" title=\"进行中\" alt=\"进行中\"/>";
                //}
                //else if (e.Row.Cells[6].Text == "1")
                //{
                //    e.Row.Cells[6].Text = "<img src=\"/Resources/Images/lock.png\" title=\"已完成\" alt=\"已完成\"/>";
                //}
                //ImageButton ib = (ImageButton)e.Row.Cells[7].FindControl("ImageButton2");
                //ib.Visible = haveDel; 
            }
        }

        /// <summary>
        /// 换页操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //GridView1.PageIndex = e.NewPageIndex;;
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
            List<KhsDzbModel> dzbList = null;
            KhsDzbService dzbSrv = new KhsDzbService();
            dzbList = dzbSrv.GetList().OrderBy(o => o.FZBM).ToList();
            GridView2.DataSource = dzbList;
            GridView2.DataBind();
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
                        //Chk_SFKC.Checked = (model.SFKC == "1");
                        Cal_KSSJ.SelectedDate = (model.KSSJ != null) ? model.KSSJ.ToDateTime() : DateTime.Today;
                        Pnl_Edit.Visible = true;
                        SetCntrlVisibility(LnkBtn_Ins, false);
                        SetCntrlVisibility(LnkBtn_Upd, true);
                        //SetCntrlVisibility(LnkBtn_Del, true);
                        KhsKhdzbService khdzbSrv = new KhsKhdzbService();
                        List<KhsKhdzbModel> rl = khdzbSrv.GetList().Where(p => p.KHID == model.KHID).ToList();
                        foreach (GridViewRow row in GridView2.Rows)
                        {
                            CheckBox dfws = (CheckBox)row.Cells[2].FindControl("Cb_DZB");
                            TextBox px = (TextBox)row.Cells[3].FindControl("Txt_PX");
                            string str_Id = GridView2.DataKeys[row.RowIndex].Value.ToString();
                            KhsKhdzbModel md = new KhsKhdzbModel();
                            md=rl.Where(p => p.DZBID == str_Id).FirstOrDefault();
                            if (md != null)
                            {
                                dfws.Checked = true;
                                px.Text = md.PX.ToRequestString();
                            }
                            else {
                                dfws.Checked = false;
                                px.Text = "0";
                            }
                        }
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
                        KhsKhdzbService khdzbSrv = new KhsKhdzbService();
                        List<KhsKhdzbModel> rl = khdzbSrv.GetList().Where(p => p.KHID == model.KHID).ToList();
                        foreach (GridViewRow row in GridView2.Rows)
                        {
                            CheckBox dfws = (CheckBox)row.Cells[2].FindControl("Cb_DZB");
                            TextBox px = (TextBox)row.Cells[3].FindControl("Txt_PX");
                            string str_Id = GridView2.DataKeys[row.RowIndex].Value.ToString();
                            KhsKhdzbModel md = new KhsKhdzbModel();
                            md = rl.Where(p => p.DZBID == str_Id).FirstOrDefault();
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
            //model.SFKC = (Chk_SFKC.Checked) ? "1" : "0";
            //add
            if (khglSrv.Execute(model))
            {
                //添加运行节点
                //KhNoderunService runSrv = new KhNoderunService();
                //runSrv.InitNoderunData(model);
                KhsKhdzbService khdzbSrv = new KhsKhdzbService();
                khdzbSrv.DeleteByKhId(model.KHID+"");
                foreach (GridViewRow row in GridView2.Rows)
                {
                    CheckBox dfws = (CheckBox)row.Cells[2].FindControl("Cb_DZB");
                    if (!dfws.Checked) continue;
                    TextBox px = (TextBox)row.Cells[3].FindControl("Txt_PX");
                    string str_Id = GridView2.DataKeys[row.RowIndex].Value.ToString();
                    KhsKhdzbModel md = new KhsKhdzbModel();
                    md.ID = Guid.NewGuid().ToRequestString();
                    md.KHID = model.KHID;
                    md.DZBID = str_Id;
                    md.PX = px.Text.ToDecimal();
                    md.DB_Option_Action = WebKeys.InsertAction;
                    khdzbSrv.Execute(md);
                }
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
                //model.SFKC = (Chk_SFKC.Checked) ? "1" : "0";
                //update
                if (khglSrv.Execute(model)) {
                    KhsKhdzbService khdzbSrv = new KhsKhdzbService();
                    khdzbSrv.DeleteByKhId(model.KHID + "");
                    foreach (GridViewRow row in GridView2.Rows)
                    {
                        CheckBox dfws = (CheckBox)row.Cells[2].FindControl("Cb_DZB");
                        if (!dfws.Checked) continue;
                        TextBox px = (TextBox)row.Cells[3].FindControl("Txt_PX");
                        string str_Id = GridView2.DataKeys[row.RowIndex].Value.ToString();
                        KhsKhdzbModel md = new KhsKhdzbModel();
                        md.ID = Guid.NewGuid().ToRequestString();
                        md.KHID = model.KHID;
                        md.DZBID = str_Id;
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
            //SetCntrlVisibility(LnkBtn_Del, false);
            Pnl_Edit.Visible = true;
            List<KhsDzbModel> dzbList = null;
            KhsDzbService dzbSrv = new KhsDzbService();
            dzbList = dzbSrv.GetList().OrderBy(o => o.FZBM).ToList();
            GridView2.DataSource = dzbList;
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