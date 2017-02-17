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
using Enterprise.Service.Perfo.Khs;
using Enterprise.Model.Perfo.Khs;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.KhS
{
    /// <summary>
    /// 年度绩效责任书页面
    /// </summary>
    public partial class KhDzbList : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        KhsDzbService  jxzrsSrv = new KhsDzbService();

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
            List<KhsDzbModel> zrsList = null;
            zrsList = jxzrsSrv.GetList().Where(p => p.ZBMC != "基层班站考核指标").OrderBy(o => o.FZBM).OrderBy(o => o.PX).ToList();
            GridView1.DataSource = zrsList;
            GridView1.DataBind();
            Utility.GroupRows(GridView1, 1);
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            ////年度
            //Ddl_Niandu.Items.Clear();
            //Ddl_SZND.Items.Clear();
            //for (int i = 2014; i <= DateTime.Now.Year + 1; i++)
            //{
            //    Ddl_SZND.Items.Add(new ListItem(i + "年", i.ToString()));
            //    Ddl_Niandu.Items.Add(new ListItem(i + "年", i.ToString()));
            //}
            //Ddl_SZND.SelectedValue = DateTime.Now.Year.ToString();
            //Ddl_Niandu.SelectedValue = DateTime.Now.Year.ToString();
            
            ////单位
            //Ddl_Danwei.DataSource = bmjgService.GetBmjgTreeLst(false);
            //Ddl_Danwei.DataTextField = "JGMC";
            //Ddl_Danwei.DataValueField = "JGBM";
            //Ddl_Danwei.DataBind();

            //负责部门
            var bmjgLst6 = bmjgService.GetSameLevelBmjg(6).ToList();
            //var bmjgLst4 = bmjgService.GetSameLevelBmjg(4).Where(p=>!string.IsNullOrEmpty(p.JGLX)).ToList();
            //bmjgLst6.AddRange(bmjgLst4);
            bmjgService.BindSSECDropDownListForBmjg(Ddl_FZBM, bmjgLst6);

            //考核类型
            //KhKindService kindSrv = new KhKindService();
            //var kindList =  kindSrv.GetList();
            //Ddl_LXID.DataSource = kindList;
            //Ddl_LXID.DataTextField = "LXMC";
            //Ddl_LXID.DataValueField = "LXID";
            //Ddl_LXID.DataBind();

            //Ddl_LXID_Search.DataSource = kindList;
            //Ddl_LXID_Search.DataTextField = "LXMC";
            //Ddl_LXID_Search.DataValueField = "LXID";
            //Ddl_LXID_Search.DataBind();
            //Ddl_LXID_Search.Items.Insert(0, new ListItem("", ""));
        }

        /// <summary>
        /// 初始化清空
        /// </summary>
        protected void Clear()
        {
            Hid_DZBID.Value = "";
            SetCntrlVisibility(LnkBtn_Ins, false);
            SetCntrlVisibility(LnkBtn_Upd, false);
            SetCntrlVisibility(LnkBtn_Del, false);
            Pnl_Edit.Visible = false;
            Lbl_Msg.Text = "";
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
                KhsDzbModel model = e.Row.DataItem as KhsDzbModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //end
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
            var model = jxzrsSrv.GetList().Where(p=>p.DZBID==e.CommandArgument.ToString()).FirstOrDefault();
            switch (e.CommandName)
            {
                case "bianji":
                    if (model != null)
                    {
                        ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
                        CommonTool.SetModelDataToForm(model, cont, "Txt_", true);
                        CommonTool.SetModelDataToForm(model, cont, "Ddl_", true);
                        Hid_DZBID.Value = model.DZBID.ToString();
                        Pnl_Edit.Visible = true;
                        SetCntrlVisibility(LnkBtn_Ins, false);
                        SetCntrlVisibility(LnkBtn_Upd, true);
                        SetCntrlVisibility(LnkBtn_Del, false);
                    }
                    break;
                case "shanchu":
                    if (model != null)
                    {
                        //删除数据
                        model.DB_Option_Action = WebKeys.DeleteAction;
                        jxzrsSrv.Execute(model);
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
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
            KhsDzbModel model = (KhsDzbModel)CommonTool.GetFormDataToModel(typeof(KhsDzbModel), cont);
            model.DZBID = Guid.NewGuid().ToRequestString();
            model.DB_Option_Action = WebKeys.InsertAction;
            //add
            if (model.FZBM != null &&
                jxzrsSrv.Execute(model))
            {
                //add by qw 2014.12.15 机关部门需要自行添加绩效指标，给他们的用户发送待办
                //var bm = bmjgService.GetSingle(model.FZBM.ToString());
                //if (bm != null)
                //{
                //    //提取负责单位的所有用户，并给员工级人员发送待办消息
                //    var u = userService.GetUserListForValid().FirstOrDefault(p => p.JGBM == model.FZBM && p.DUTY.Contains("员工"));
                //    if (u != null)
                //    {
                //        var khdw = bmjgService.GetSingle(model.JGBM.ToString());
                //        if (khdw.JGLX.Contains("职能"))
                //        {
                //            msgService.SendMsgToUers(u.LOGINID, bm.JGBM,
                //                "年度绩效责任指标制定", "需要您制定【" + SysBmjgService.GetBmjgName(model.JGBM.Value) + "】的【" + model.SZND + "】年度绩效责任指标!",
                //                string.Format("/Module/Kh/NdjxzrsJgbmZbList.aspx?BM={0}&ND={1}", model.JGBM, model.SZND),
                //                "1318", model.JGBM.ToString());
                //        }
                //        else if (khdw.JGLX.Contains("基层"))
                //        {
                //            msgService.SendMsgToUers(u.LOGINID, bm.JGBM,
                //                "年度绩效责任指标制定", "需要您制定【" + SysBmjgService.GetBmjgName(model.JGBM.Value) + "】的【" + model.SZND + "】年度绩效责任指标!",
                //                string.Format("/Module/Kh/NdjxzrsZbList.aspx?BM={0}&ND={1}", model.JGBM, model.SZND),
                //                "1312", model.JGBM.ToString());
                //        }
                //    }
                //}
                Utility.ShowMsg(Page, "提示", "添加操作成功！", 100, "show");
                //end
            }
            else
            {
                Utility.ShowMsg(Page, "系统消息", "注意：负责部门必须选择！！", 100, "show");
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
            if (!string.IsNullOrEmpty(Hid_DZBID.Value))
            {
                ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
                KhsDzbModel model = (KhsDzbModel)CommonTool.GetFormDataToModel(typeof(KhsDzbModel), cont);
                model.DB_Option_Action = WebKeys.UpdateAction;
                //update
                jxzrsSrv.Execute(model);
                Utility.ShowMsg(Page, "提示", "更新成功！", 100, "show");
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
            ////doing......关联表较多，后面再处理
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
            Lbl_Msg.Text = "小提示：注意选择负责部门!";
            
            BindGrid();
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
        /// 单位切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Danwei_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 考核类型选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_LXID_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion

    }
}