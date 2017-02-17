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
    /// 年度绩效责任书页面
    /// </summary>
    public partial class NdjxzrsList : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        KhJxzrsService jxzrsSrv = new KhJxzrsService();

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
            List<KhJxzrsModel> zrsList = null;
            if (!string.IsNullOrEmpty(Ddl_LXID_Search.SelectedValue))
            {
                zrsList = jxzrsSrv.GetListByNd_Bmjg_Khlx(Ddl_Niandu.SelectedValue, 
                    Ddl_Danwei.SelectedValue,Ddl_LXID_Search.SelectedValue) as List<KhJxzrsModel>;
            }
            else
            {
                zrsList = jxzrsSrv.GetListByNdAndBmjg(Ddl_Niandu.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhJxzrsModel>;
            }
            GridView1.DataSource = zrsList;
            GridView1.DataBind();
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //年度
            Ddl_Niandu.Items.Clear();
            Ddl_SZND.Items.Clear();
            for (int i = 2014; i <= DateTime.Now.Year + 1; i++)
            {
                Ddl_SZND.Items.Add(new ListItem(i + "年", i.ToString()));
                Ddl_Niandu.Items.Add(new ListItem(i + "年", i.ToString()));
            }
            Ddl_SZND.SelectedValue = DateTime.Now.Year.ToString();
            Ddl_Niandu.SelectedValue = DateTime.Now.Year.ToString();
            
            //单位
            Ddl_Danwei.DataSource = bmjgService.GetBmjgTreeLst(false);
            Ddl_Danwei.DataTextField = "JGMC";
            Ddl_Danwei.DataValueField = "JGBM";
            Ddl_Danwei.DataBind();

            //负责部门
            var bmjgLst6 = bmjgService.GetSameLevelBmjg(6).ToList();
            var bmjgLst4 = bmjgService.GetSameLevelBmjg(4).Where(p=>!string.IsNullOrEmpty(p.JGLX)).ToList();
            bmjgService.BindSSECDropDownListForBmjg(Ddl_JGBM, bmjgLst6);
            bmjgLst6.AddRange(bmjgLst4);
            bmjgService.BindSSECDropDownListForBmjg(Ddl_FZBM, bmjgLst6);

            //考核类型
            KhKindService kindSrv = new KhKindService();
            var kindList =  kindSrv.GetList();
            Ddl_LXID.DataSource = kindList;
            Ddl_LXID.DataTextField = "LXMC";
            Ddl_LXID.DataValueField = "LXID";
            Ddl_LXID.DataBind();

            Ddl_LXID_Search.DataSource = kindList;
            Ddl_LXID_Search.DataTextField = "LXMC";
            Ddl_LXID_Search.DataValueField = "LXID";
            Ddl_LXID_Search.DataBind();
            Ddl_LXID_Search.Items.Insert(0, new ListItem("", ""));
        }

        /// <summary>
        /// 初始化清空
        /// </summary>
        protected void Clear()
        {
            Hid_ZRSID.Value = "";
            Txt_ZRSMC.Text = "";
            Txt_ZRSFJ.Text = Txt_ZRSFJ.Value = "";
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
                KhJxzrsModel model = e.Row.DataItem as KhJxzrsModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //责任书名称 1
                string pageName = string.Empty;
                if (model.Bmjg.JGLX.Contains("职能"))
                {
                    pageName = "NdjxzrsJgbmZbList.aspx";
                }
                else
                {
                    pageName = "NdjxzrsZbList.aspx";
                }
                e.Row.Cells[1].Text =
                    string.Format("<a href='#' onclick=\"javascript:parent.addTab('/Module/Kh/{3}?BM={0}&ND={1}','责任书考核指标');\">{2}</a>",
                    model.JGBM, model.SZND, model.ZRSMC, pageName);
                //单位名称
                e.Row.Cells[2].Text = model.Bmjg.JGMC;
                //考核类型
                e.Row.Cells[3].Text = (model.KhKind != null) ? model.KhKind.LXMC : "";
                //负责部门
                e.Row.Cells[5].Text = (model.FzBmjg != null) ? model.FzBmjg.JGMC : "";
                //附件 6
                e.Row.Cells[6].Text = model.ZRSFJ.ToAttachHtmlByOne();
                //指标数量 7
                e.Row.Cells[7].Text = (model.JxzrszbLst != null) ? model.JxzrszbLst.Count.ToString() : "";
                //考核状态 8
                if (e.Row.Cells[8].Text == "0")
                {
                    e.Row.Cells[8].Text = "<img src=\"/Resources/Images/lock_unlock.png \" title=\"制定中\" alt=\"制定中\"/>";
                }
                else
                {
                    e.Row.Cells[8].Text = "<img src=\"/Resources/Images/lock.png\" title=\"已下达\" alt=\"已下达\"/>";
                }
                //增加权限判定，add by qw 2014.12.15 start
                if (model.FZBM != userModel.JGBM || !userModel.DUTY.Contains("负责"))
                {
                    if (userModel.ROLEID != "paadmin")
                    {
                        e.Row.Cells[10].Text = "";
                    }
                }
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
            var model = jxzrsSrv.GetSingle(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                case "bianji":
                    if (model != null)
                    {
                        ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
                        CommonTool.SetModelDataToForm(model, cont, "Txt_", true);
                        CommonTool.SetModelDataToForm(model, cont, "Ddl_", true);
                        Hid_ZRSID.Value = model.ZRSID.ToString();
                        Txt_ZRSFJ.Text = Txt_ZRSFJ.Value = model.ZRSFJ;
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
                        jxzrsSrv.DeleteJxzrsData(model);
                    }
                    BindGrid();
                    break;
                case "xiada"://下达
                    if (model != null && 
                        model.ZRSZT != "1")
                    {
                        model.DB_Option_Action = WebKeys.UpdateAction;
                        model.ZRSZT = "1";
                        model.XDRQ = DateTime.Now;
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
            KhJxzrsModel model = (KhJxzrsModel)CommonTool.GetFormDataToModel(typeof(KhJxzrsModel), cont);
            model.DB_Option_Action = WebKeys.InsertAction;
            model.TJRQ = DateTime.Now;
            model.ZRSFJ = Txt_ZRSFJ.Value;
            //add
            if (model.JGBM != null && model.FZBM != null &&
                jxzrsSrv.Execute(model))
            {
                //add by qw 2014.12.15 机关部门需要自行添加绩效指标，给他们的用户发送待办
                var bm = bmjgService.GetSingle(model.FZBM.ToString());
                if (bm != null)
                {
                    //提取负责单位的所有用户，并给员工级人员发送待办消息
                    var u = userService.GetUserListForValid().FirstOrDefault(p => p.JGBM == model.FZBM && p.DUTY.Contains("员工"));
                    if (u != null)
                    {
                        var khdw = bmjgService.GetSingle(model.JGBM.ToString());
                        if (khdw.JGLX.Contains("职能"))
                        {
                            msgService.SendMsgToUers(u.LOGINID, bm.JGBM,
                                "年度绩效责任指标制定", "需要您制定【" + SysBmjgService.GetBmjgName(model.JGBM.Value) + "】的【" + model.SZND + "】年度绩效责任指标!",
                                string.Format("/Module/Kh/NdjxzrsJgbmZbList.aspx?BM={0}&ND={1}", model.JGBM, model.SZND),
                                "1318", model.JGBM.ToString());
                        }
                        else if (khdw.JGLX.Contains("二级"))
                        {
                            msgService.SendMsgToUers(u.LOGINID, bm.JGBM,
                                "年度绩效责任指标制定", "需要您制定【" + SysBmjgService.GetBmjgName(model.JGBM.Value) + "】的【" + model.SZND + "】年度绩效责任指标!",
                                string.Format("/Module/Kh/NdjxzrsZbList.aspx?BM={0}&ND={1}", model.JGBM, model.SZND),
                                "1312", model.JGBM.ToString());
                        }
                    }
                }
                Utility.ShowMsg(Page, "提示", "添加操作成功！", 100, "show");
                //end
            }
            else
            {
                Utility.ShowMsg(Page, "系统消息", "注意：单位名称和负责部门必须选择！！", 100, "show");
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
            if (!string.IsNullOrEmpty(Hid_ZRSID.Value))
            {
                ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
                KhJxzrsModel model = (KhJxzrsModel)CommonTool.GetFormDataToModel(typeof(KhJxzrsModel), cont);
                model.DB_Option_Action = WebKeys.UpdateAction;
                model.TJRQ = DateTime.Now;
                if (model.ZRSZT == "1")
                {
                    model.XDRQ = DateTime.Now;
                }
                else
                {
                    model.XDRQ = null;
                }
                model.ZRSFJ = Txt_ZRSFJ.Value;
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
            Lbl_Msg.Text = "小提示：先选择具体的考核类型和单位，‘新增’时会自动填充内容!";
            //机构
            if (Ddl_JGBM.Items.FindByValue(Ddl_Danwei.SelectedValue) != null)
            {
                Ddl_JGBM.SelectedValue = Ddl_Danwei.SelectedValue;
                Txt_ZRSMC.Text = Ddl_JGBM.SelectedItem.Text + "年度绩效责任书";
                //mod by qw 2014.12.28 start 
                //职能部门的负责部门就是自己，二级单位的负责部门先订到绩效办公室
                var bmjg = bmjgService.GetSingle(Ddl_JGBM.SelectedValue);
                if (bmjg.JGLX.Contains("职能"))
                {
                    Ddl_FZBM.SelectedValue = Ddl_JGBM.SelectedValue;
                }
                else if (bmjg.JGLX.Contains("二级"))
                {
                    Ddl_FZBM.SelectedValue = "57";//绩效办公室
                }
                //end
            }
            //考核类型
            if (Ddl_LXID.Items.FindByValue(Ddl_LXID_Search.SelectedValue) != null)
            {
                Ddl_LXID.SelectedValue = Ddl_LXID_Search.SelectedValue;
            }
            //考核年度
            if (Ddl_SZND.Items.FindByValue(Ddl_Niandu.SelectedValue) != null)
            {
                Ddl_SZND.SelectedValue = Ddl_Niandu.SelectedValue;
            }
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