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
    public partial class GzydkhEdit : PageBase
    {

        /// <summary>
        /// 考核管理服务类
        /// </summary>
        KhKhglService khglSrv = new KhKhglService();
        protected string Khid = (string)Utility.sink("KHID", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected string Khdzbid = (string)Utility.sink("KHDZBID", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);
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
                bool isUpdate = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
                LnkBtn_Upd.Visible = isUpdate;
                haveDel = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Del);
                
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
            //List<KhKhglModel> list = null;
            //if (!string.IsNullOrEmpty(Txt_Khmc_Search.Text))
            //{
            //    list = khglSrv.GetKhListByYear(Ddl_Niandu.SelectedValue).Where(p => p.KHMC.Contains(Txt_Khmc_Search.Text)).ToList();
            //}
            //else
            //{
            //    list = khglSrv.GetKhListByYear(Ddl_Niandu.SelectedValue) as List<KhKhglModel>;
            //}
            //GridView1.DataSource = list.Where(p => p.LXID== "LX2014B").ToList();
            //GridView1.DataBind();
            Hid_KHID.Value = Khid;
            Hid_KHDZBID.Value = Khdzbid;
            List<SysBmjgModel> jgList = null;
            SysBmjgService dzbSrv = new SysBmjgService();
            jgList = dzbSrv.GetList().Where(p=>p.SFKH=="1"&&p.JGLX=="职能部门").OrderBy(o => o.PX).ToList();
            GridView2.DataSource = jgList;
            GridView2.DataBind();
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {

        }

        /// <summary>
        /// 初始化清空
        /// </summary>
        protected void Clear()
        {
            
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
            KhsGzydService srv = new KhsGzydService();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SysBmjgModel jg = e.Row.DataItem as SysBmjgModel;
                KhsGzydModel m = new KhsGzydModel();
                m = srv.GetList().Where(p => p.LOGINID == this.userModel.LOGINID && p.KHID == Hid_KHID.Value.ToInt() && p.JGBM == jg.JGBM).FirstOrDefault();
                if (m != null) {
                    TextBox pf = (TextBox)e.Row.Cells[2].FindControl("Txt_PF");
                    TextBox bz = (TextBox)e.Row.Cells[3].FindControl("Txt_BZ");
                    pf.Text = m.PF.ToRequestString();
                    bz.Text = m.BZ;
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
                KhsGzydService mydSrv = new KhsGzydService();
                foreach (GridViewRow row in GridView2.Rows)
                {
                    TextBox pf = (TextBox)row.Cells[2].FindControl("Txt_PF");
                    TextBox bz = (TextBox)row.Cells[3].FindControl("Txt_BZ");
                    string str_Id = GridView2.DataKeys[row.RowIndex].Value.ToString();
                    KhsGzydModel md = mydSrv.GetList().Where(p => p.LOGINID == this.userModel.LOGINID && p.KHID == Hid_KHID.Value.ToInt() && p.JGBM == str_Id.ToInt()).FirstOrDefault();
                    if (md == null)
                    {
                        md = new KhsGzydModel();
                        md.ID = Guid.NewGuid().ToRequestString();
                        md.DB_Option_Action = WebKeys.InsertAction;
                        md.KHID = Hid_KHID.Value.ToInt();
                        md.KHDZBID = Hid_KHDZBID.Value;
                        md.JGBM = str_Id.ToInt();
                        md.LOGINID = this.userModel.LOGINID;
                        md.LRSJ = DateTime.Now;
                    }
                    else {
                        md.DB_Option_Action = WebKeys.UpdateAction;
                    }
                    md.PF = pf.Text.ToDecimal();
                    md.BZ = bz.Text;
                    mydSrv.Execute(md);
                }
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
            //string url = string.Format("BB={0}&BM={1}&ND={2}", Bbmc, Jgbm, Sznd);
            Response.Redirect("~/Module/Kh/GzydkhList.aspx", true);
        }

        /// <summary>
        /// 显示编辑面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Clear();
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