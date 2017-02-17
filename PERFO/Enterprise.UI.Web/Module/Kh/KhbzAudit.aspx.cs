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
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Kh
{
    /// <summary>
    /// 发起考核页面
    /// </summary>
    public partial class KhbzAudit : PageBase
    {

        /// <summary>
        /// 考核管理服务类
        /// </summary>
        KhKhglService khglSrv = new KhKhglService();
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
            List<KhKhglModel > list = null;
            KhKhglService khglSrv = new KhKhglService();
            string sql = "select a.* from perfo_kh_khgl a  where a.khzt=-1 or a.khzt=-2";
            list = khglSrv.GetListBySQL(sql).ToList();
            GridView1.DataSource = list;
            GridView1.DataBind();


            Utility.GroupRows(GridView2, 2);
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
            KhsDzbService dzbSrv = new KhsDzbService();
            KhsKhdzbService khdzbSrv = new KhsKhdzbService();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhKhglModel model = e.Row.DataItem as KhKhglModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //考核类型
                e.Row.Cells[2].Text = model.Kind.LXMC;
                CheckBox ib = (CheckBox)e.Row.Cells[1].FindControl("Cb_KHID");
                if (model.KHZT != "-1")
                    ib.Enabled = false;
                //考核状态
                e.Row.Cells[5].Text = khglSrv.GetStatus(model);
                //if (e.Row.Cells[6].Text == "2")
                //{
                //    e.Row.Cells[6].Text = "<img src=\"/Resources/Images/lock_unlock.png \" title=\"进行中\" alt=\"进行中\"/>";
                //}
                //else if (e.Row.Cells[6].Text == "1")
                //{
                //    e.Row.Cells[6].Text = "<img src=\"/Resources/Images/lock.png\" title=\"已完成\" alt=\"已完成\"/>";
                //}

                string sql = "select a.* from PERFO_KHS_DZB a ,PERFO_KHS_KHDZB b where a.dzbid=b.dzbid and b.khid=" + model.KHID + " and a.zbmc not in ('工作要点', '上级考核', '机关服务满意度指标')";
                List<KhsDzbModel> ls=dzbSrv.GetListBySQL(sql).ToList();
                string html = "<table  class=\"GridViewStyle\" cellspacing=\"0\" rules=\"all\" border=\"1\"   style=\"width:100%;border-collapse:collapse;\"><tr class=\"GridViewHeaderStyle\" ><th align='center' width='100px'>名称</th><th align='center' width='50px'>负责部门</th><th align='center' width='50px'>状态</th></tr>";
                foreach (KhsDzbModel v in ls) {
                    KhsKhdzbModel m = null;
                    m = khdzbSrv.GetList().Where(p => p.KHID == model.KHID && p.DZBID == v.DZBID).FirstOrDefault();
                    if (m == null) continue;
                    html += "<tr>";
                    html += "<td align='center'><a href='KhbzAudit2.aspx?Id=" + m.ID + "'>" + v.ZBMC + "</a></td><td align='center'>" + Enterprise.Service.Perfo.Sys.SysBmjgService.GetBmjgName(v.FZBM.ToInt()) + "</td><td align='center'>" + khdzbSrv .GetStatus(m)+ "</td>";
                    html += "</tr>";
                }
                html += "</table>";
                e.Row.Cells[6].Text = html;
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
            //var model = khglSrv.GetSingle(e.CommandArgument.ToString());
            //List<KhsDzbModel> dzbList = null;
            //KhsDzbService dzbSrv = new KhsDzbService();
            //dzbList = dzbSrv.GetList().OrderBy(o => o.FZBM).ToList();
            //GridView2.DataSource = dzbList;
            //GridView2.DataBind();
            //switch (e.CommandName)
            //{
            //    case "bianji":
            //        if (model != null)
            //        {
            //            ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
            //            CommonTool.SetModelDataToForm(model, cont, "Txt_", true);
            //            CommonTool.SetModelDataToForm(model, cont, "Ddl_", true);
            //            CommonTool.SetModelDataToForm(model, cont, "Chk_", true);
            //            Hid_KHID.Value = model.KHID.ToString();
            //            //Chk_SFKC.Checked = (model.SFKC == "1");
            //            Cal_KSSJ.SelectedDate = (model.KSSJ != null) ? model.KSSJ.ToDateTime() : DateTime.Today;
            //            Pnl_Edit.Visible = true;
            //            SetCntrlVisibility(LnkBtn_Ins, false);
            //            SetCntrlVisibility(LnkBtn_Upd, true);
            //            //SetCntrlVisibility(LnkBtn_Del, true);
            //            KhsKhdzbService khdzbSrv = new KhsKhdzbService();
            //            List<KhsKhdzbModel> rl = khdzbSrv.GetList().Where(p => p.KHID == model.KHID).ToList();
            //            foreach (GridViewRow row in GridView2.Rows)
            //            {
            //                CheckBox dfws = (CheckBox)row.Cells[2].FindControl("Cb_DZB");
            //                TextBox px = (TextBox)row.Cells[3].FindControl("Txt_PX");
            //                string str_Id = GridView2.DataKeys[row.RowIndex].Value.ToString();
            //                KhsKhdzbModel md = new KhsKhdzbModel();
            //                md=rl.Where(p => p.DZBID == str_Id).FirstOrDefault();
            //                if (md != null)
            //                {
            //                    dfws.Checked = true;
            //                    px.Text = md.PX.ToRequestString();
            //                }
            //                else {
            //                    dfws.Checked = false;
            //                    px.Text = "0";
            //                }
            //            }
            //        }
            //        break;
            //    case "gbzt":
            //        if (model != null)
            //        {
            //            //实现考核状态的切换
            //            model.KHZT = (model.KHZT == "1") ? "0" : "1";
            //            if (model.KHZT == "1")
            //            {
            //                model.GBSJ = DateTime.Now;
            //            }
            //            else
            //            {
            //                model.GBSJ = null;
            //            }
            //            model.DB_Option_Action = WebKeys.UpdateAction;
            //            khglSrv.Execute(model);
            //            KhsKhdzbService khdzbSrv = new KhsKhdzbService();
            //            List<KhsKhdzbModel> rl = khdzbSrv.GetList().Where(p => p.KHID == model.KHID).ToList();
            //            foreach (GridViewRow row in GridView2.Rows)
            //            {
            //                CheckBox dfws = (CheckBox)row.Cells[2].FindControl("Cb_DZB");
            //                TextBox px = (TextBox)row.Cells[3].FindControl("Txt_PX");
            //                string str_Id = GridView2.DataKeys[row.RowIndex].Value.ToString();
            //                KhsKhdzbModel md = new KhsKhdzbModel();
            //                md = rl.Where(p => p.DZBID == str_Id).FirstOrDefault();
            //                if (md != null)
            //                {
            //                    dfws.Checked = true;
            //                    px.Text = md.PX.ToRequestString();
            //                }
            //                else
            //                {
            //                    dfws.Checked = false;
            //                    px.Text = "0";
            //                }
            //            }
            //        }
            //        BindGrid();
            //        break;
            //    default:
            //        break;
            //}
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
           
            //ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
            //KhsKhbzModel model = (KhsKhbzModel)CommonTool.GetFormDataToModel(typeof(KhsKhbzModel), cont);
            //model.DB_Option_Action = WebKeys.InsertAction;
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

        protected void Btn_Ok_Click(object sender, EventArgs e)
        {
            KhKhglService khSrv = new KhKhglService();
            SysUserService uSrv = new SysUserService();
            SysModuleService moduleService = new SysModuleService();
            KhMessageService msgService = new KhMessageService();
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox ckid = (CheckBox)row.Cells[1].FindControl("Cb_KHID");
                if (!ckid.Checked) continue;
                string str_Id = GridView1.DataKeys[row.RowIndex].Value.ToString();
                KhKhglModel md = khSrv.GetSingle(str_Id);
                md.KHZT = "-2";
                md.DB_Option_Action = WebKeys.UpdateAction;
                khSrv.Execute(md);


                //string uri1 = "Module/Kh/KhbzList.asp";
                //IList<SysModuleModel> moduleLst = moduleService.GetList();
                //SysModuleModel module = moduleLst.FirstOrDefault(p => p.MURL.ToUpper().Contains(uri1.ToUpper()) || p.WEBURL.ToUpper() == uri1.ToUpper());
                //msgService.CloseMessage(md.KHID, module.MID);               
                //string sql = "select * from PERFO_SYS_USER a,PERFO_KHS_KHDZB b,PERFO_KHS_DZB c where b.dzbid=c.dzbid and c.fzbm=a.jgbm and b.khid=" + md.KHID + " and a.duty='打分人员'";
                //List<SysUserModel> uList = uSrv.GetListBySQL(sql).ToList();
                //if (uList.Count > 0)
                //{
                //    if (md.LXID == "LX2014A") { 
                //        string uri = "Module/Kh/KhJcdwdfList.asp";
                //        moduleLst = moduleService.GetList();
                //        module = moduleLst.FirstOrDefault(p => p.MURL.ToUpper().Contains(uri.ToUpper()) || p.WEBURL.ToUpper() == uri.ToUpper());
                //        msgService.SendMsgToUers(uList, 1, md.KHID + "",
                //            "开始对基层单位打分", "需要您对【" + md.KHMC + "】进行打分操作!",
                //            "/Module/Kh/KhJcdwdfList.aspx",
                //            module.MID, this.userModel.LOGINID);
                //    }
                //    if (md.LXID == "LX2014B")
                //    {

                //        string uri2 = "Module/Kh/KhJgbmdfList.asp";
                //        moduleLst = moduleService.GetList();
                //        module = moduleLst.FirstOrDefault(p => p.MURL.ToUpper().Contains(uri2.ToUpper()) || p.WEBURL.ToUpper() == uri2.ToUpper());
                //        msgService.SendMsgToUers(uList, 1, md.KHID + "",
                //            "开始对机关部门打分", "需要您对【" + md.KHMC + "】进行打分操作!",
                //            "/Module/Kh/KhJgbmdfList.aspx",
                //            module.MID, this.userModel.LOGINID);
                //    }
                //}

            }
            //ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
            //KhsKhbzModel model = (KhsKhbzModel)CommonTool.GetFormDataToModel(typeof(KhsKhbzModel), cont);
            //model.DB_Option_Action = WebKeys.InsertAction;
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
            //if (!string.IsNullOrEmpty(Hid_KHID.Value))
            //{
            //    ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
            //    KhsKhbzModel model = (KhsKhbzModel)CommonTool.GetFormDataToModel(typeof(KhsKhbzModel), cont);
            //    model.DB_Option_Action = WebKeys.UpdateAction;
            //    model.KSSJ = Cal_KSSJ.SelectedDate;
            //    //model.SFKC = (Chk_SFKC.Checked) ? "1" : "0";
            //    //update
            //    if (khglSrv.Execute(model)) {
            //        KhsKhdzbService khdzbSrv = new KhsKhdzbService();
            //        khdzbSrv.DeleteByKhId(model.KHID + "");
            //        foreach (GridViewRow row in GridView2.Rows)
            //        {
            //            CheckBox dfws = (CheckBox)row.Cells[2].FindControl("Cb_DZB");
            //            if (!dfws.Checked) continue;
            //            TextBox px = (TextBox)row.Cells[3].FindControl("Txt_PX");
            //            string str_Id = GridView2.DataKeys[row.RowIndex].Value.ToString();
            //            KhsKhdzbModel md = new KhsKhdzbModel();
            //            md.ID = Guid.NewGuid().ToRequestString();
            //            md.KHID = model.KHID;
            //            md.DZBID = str_Id;
            //            md.PX = px.Text.ToDecimal();
            //            md.DB_Option_Action = WebKeys.InsertAction;
            //            khdzbSrv.Execute(md);
            //        }
            //    };
            //}

            //Clear();
            //BindGrid();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Del_Click(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(Hid_KHID.Value))
            //{
            //    //doing......关联表较多，后面再处理
            //    KhsKhbzModel model = khglSrv.GetSingle(Hid_KHID.Value);
            //    if (model != null && khglSrv.DeleteAllDataByKhid(Hid_KHID.Value))
            //    {
            //        model.DB_Option_Action = WebKeys.DeleteAction;
            //        khglSrv.Execute(model);
            //    }
            //}
            //Clear();
            //BindGrid();
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