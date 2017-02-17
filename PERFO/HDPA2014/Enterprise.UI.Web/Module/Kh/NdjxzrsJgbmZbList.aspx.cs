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
    /// 年度绩效责任书-机关部门指标维护页面
    /// </summary>
    public partial class NdjxzrsJgbmZbList : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        KhJxzrszbService jxzrsZbSrv = new KhJxzrszbService();
        KhJxzrsService zrsSrv = new KhJxzrsService();

        protected string Bbmc = (string)Utility.sink("BB", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//版本名称
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected string Sznd = (string)Utility.sink("ND", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//所在年度
        protected List<KhJxzrszbModel> JxzrszbList = null;
        string zblx = string.Empty;//指标的类型 add by qw 2014.12.25 按高老师要求按类型为重置序号
        int zbRowIndex = 1;//指标序号

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
                LnkBtn_Ldsh.Visible = Btn_Add.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
                bool isUpdate = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
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
            var q = zrsSrv.GetListByNdAndBmjg(Ddl_Niandu.SelectedValue, Ddl_Danwei.SelectedValue).FirstOrDefault();
            if (q != null)
            {
                if (q.FZBM != userModel.JGBM && userModel.ROLEID != "paadmin")
                {
                    GridView1.Columns[GridView1.Columns.Count - 1].Visible = Btn_Add.Visible = LnkBtn_Ldsh.Visible = false;
                    Lbl_Msg.Text = "您当前只能查看!";
                }
                else if (q.ZRSZT == "1" && userModel.ROLEID != "paadmin")
                {
                    GridView1.Columns[GridView1.Columns.Count - 1].Visible = Btn_Add.Visible = LnkBtn_Ldsh.Visible = false;
                    Lbl_Msg.Text = "该单位责任书已下达!不能再修改了!";
                }
                else if (Btn_Add.Visible)
                {
                    Lbl_Msg.Text = "您可以进行指标的编辑!";
                    //add by qw 2014.12.15 按高老师要求，机关部门的指标要负责人先审核才能下达
                    if ((q.FZBM == userModel.JGBM && userModel.DUTY.Contains("负责")) || userModel.ROLEID == "paadmin")
                    {
                        LnkBtn_Xiada.Visible = true;
                        LnkBtn_Ldsh.Visible = false;
                    }
                    //end
                }
            }
            else
            {
                Lbl_Msg.Text = "该单位还未制定绩效责任书!";
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = Btn_Add.Visible = LnkBtn_Ldsh.Visible = false;
            }
            JxzrszbList = jxzrsZbSrv.GetListBySearch(Ddl_Niandu.SelectedValue, 
                Ddl_Danwei.SelectedValue, Ddl_Bbmc.SelectedValue) as List<KhJxzrszbModel>;
            GridView1.DataSource = JxzrszbList;
            GridView1.DataBind();
            Utility.GroupRows(GridView1, 2);//合并

            //add by qw 2014.12.25 按高老师要求，增加一个责任书列表 start
            if (string.IsNullOrEmpty(Ddl_Danwei.SelectedValue))
            {
                //提取所有二级单位的责任书信息
                List<KhJxzrsModel> zrsList = zrsSrv.GetListByNd_Bmjg_Khlx(Ddl_Niandu.SelectedValue, "1", "LX2014B") as List<KhJxzrsModel>;
                GridView2.DataSource = zrsList;
                GridView2.DataBind();
                GridView2.Visible = true;
                Btn_Back.Visible = false;
                Lbl_Msg.Text = "";
            }
            else
            {
                GridView2.Visible = false;
                Btn_Back.Visible = true;
            }
            //end
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
            Ddl_Niandu.SelectedValue = (string.IsNullOrEmpty(Sznd)) ? DateTime.Now.Year.ToString() : Sznd;
            
            //单位
            List<SysBmjgModel> parentBmjgLst = bmjgService.GetSameLevelBmjg(4) as List<SysBmjgModel>;
            int[] jgbms = (from c in parentBmjgLst select c.JGBM).ToArray();
            List<SysBmjgModel> bmjgTreeList = bmjgService.GetBmjgTreeLst(false).Where(p => p.XSXH.Length > 2 && !p.JGLX.Contains("二级")).ToList();
            bmjgService.BindSSECDropDownListForBmjg(Ddl_Danwei, bmjgTreeList, jgbms);
            if (Jgbm > 0)
            {
                Ddl_Danwei.SelectedValue = Jgbm.ToString();
            }
            else
            {
                Ddl_Danwei.SelectedValue = userModel.JGBM.ToString();
            }
            //权限检测
            if (zrsSrv.IsJxzrsFgbm(Ddl_Niandu.SelectedValue, Ddl_Danwei.SelectedValue, userModel) || userModel.ROLEID == "paadmin")
            {
                LnkBtn_Ldsh.Visible = Btn_Add.Visible = true;
            }
            else if (Ddl_Danwei.SelectedValue == userModel.JGBM.ToString())
            {
                LnkBtn_Ldsh.Visible = Btn_Add.Visible = false;//只由负责的部门操作
                Ddl_Danwei.Enabled = false;//只限于本单位
            }
            else
            {
                LnkBtn_Ldsh.Visible = Btn_Add.Visible = false;
            }

            //应用版本
            ZbkBanbenService banbenSrv = new ZbkBanbenService();
            Ddl_Bbmc.DataSource = banbenSrv.GetList();
            Ddl_Bbmc.DataTextField = "BBMC";
            Ddl_Bbmc.DataValueField = "BBMC";
            Ddl_Bbmc.DataBind();
            if (!string.IsNullOrEmpty(Bbmc))
                Ddl_Bbmc.SelectedValue = Bbmc;
        }

        #endregion


        #region 事件处理区

        /// <summary>
        /// 返回事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            string url = string.Format("?BB={0}&ND={1}", Ddl_Bbmc.SelectedValue, Ddl_Niandu.SelectedValue);
            GobackPageUrl(url);
        }

        /// <summary>
        /// 责任书列表行绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhJxzrsModel model = e.Row.DataItem as KhJxzrsModel;
                //责任书名称 1
                string pageName = string.Empty;
                if (model.Bmjg.JGLX.Contains("职能"))
                {
                    pageName = "M.K.NdjxzbJgbm";
                }
                else
                {
                    pageName = "M.K.Ndjxzb";
                }
                e.Row.Cells[1].Text =
                    string.Format("<a href='/{3}?BM={0}&ND={1}'>{2}</a>", model.JGBM, model.SZND, model.ZRSMC, pageName);
                //单位名称
                e.Row.Cells[2].Text = model.Bmjg.JGMC;
                //考核类型
                e.Row.Cells[3].Text = (model.KhKind != null) ? model.KhKind.LXMC : "";
                //负责部门
                e.Row.Cells[5].Text = (model.FzBmjg != null) ? model.FzBmjg.JGMC : "";
                //指标数量 6
                e.Row.Cells[6].Text = (model.JxzrszbLst != null) ? model.JxzrszbLst.Count.ToString() : "";
                //考核状态 7
                if (e.Row.Cells[7].Text == "0")
                {
                    e.Row.Cells[7].Text = "<img src=\"/Resources/Images/lock_unlock.png \" title=\"制定中\" alt=\"制定中\"/>";
                }
                else
                {
                    e.Row.Cells[7].Text = "<img src=\"/Resources/Images/lock.png\" title=\"已下达\" alt=\"已下达\"/>";
                }
            }
        }

        /// <summary>
        /// 责任书列表分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex; ;
            BindGrid();
        }

        /// <summary>
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhJxzrszbModel model = e.Row.DataItem as KhJxzrszbModel;
                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //责任书名称 1
                e.Row.Cells[1].Text = model.Jxzrs.ZRSMC;
                //量化指标
                if (model.Lhzb != null) {
                    //序号
                    if (model.LHZBBM != zblx)
                    {
                        zbRowIndex = 1;
                        zblx = model.LHZBBM;
                    }
                    e.Row.Cells[0].Text = (zbRowIndex++).ToString();
                    //考核指标类型与权重2
                    e.Row.Cells[2].Text = model.Lhzb.Zbxx.ZBMC + "<br/>〖" + model.ZZBQZ.Value.ToString("P") + "〗";
                    //考核主要内容3
                    e.Row.Cells[3].Text = "<div style='width: 200px;padding: 2px;overflow:auto;'>" + model.JGKHNR + "</div>";
                    //分值4
                    e.Row.Cells[4].Text = model.ZZBFZ.ToRequestString();
                    //考核目标5
                    e.Row.Cells[5].Text = "<div style='width: 180px;padding: 2px;overflow:auto;'>" + model.JGKHMB + "</div>";
                    //完成时间6
                    e.Row.Cells[6].Text = "<div style='width: 100px;padding: 2px;overflow:auto;'>" + model.JGWCSJ + "</div>";
                    //评分标准7
                    e.Row.Cells[7].Text = "<div style='width: 200px;padding: 2px;overflow:auto;'>" + model.JGPFBZ + "</div>";
                }
                //打分指标
                else if (model.Dfzb != null)
                {
                    //序号
                    if (model.Dfzb.Zbxx.YJZBMC != zblx)
                    {
                        zbRowIndex = 1;
                        zblx = model.Dfzb.Zbxx.YJZBMC;
                    }
                    e.Row.Cells[0].Text = (zbRowIndex++).ToString();
                    ImageButton addBtn = e.Row.FindControl("ImageButton1") as ImageButton;
                    addBtn.Visible = false;
                    ImageButton editBtn = e.Row.FindControl("ImageButton2") as ImageButton;
                    editBtn.Visible = false;
                    e.Row.Cells[2].Text = model.Dfzb.Zbxx.YJZBMC;
                    e.Row.Cells[3].Text = model.Dfzb.Zbxx.ZBMC;
                    //分值 4
                    //e.Row.Cells[4].ColumnSpan = 4;
                    e.Row.Cells[4].Text = model.ZZBFZ.ToString();
                    //e.Row.Cells[5].Visible = false;
                    //e.Row.Cells[6].Visible = false;
                    //e.Row.Cells[7].Visible = false;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].ColumnSpan = 3;
                e.Row.Cells[0].Text = "∑分值合计=〖年度重点工作任务+部门履职〗：";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                string hjz = JxzrszbList.Where(p => !string.IsNullOrEmpty(p.LHZBBM)).Sum(p => p.ZZBFZ).ToRequestString();
                e.Row.Cells[3].Text = string.Format("<font color='red'>{0}</font>", hjz);
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].ColumnSpan = 4;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
            }
        }

        /// <summary>
        /// 行命令处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var model = jxzrsZbSrv.GetSingle(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                case "add"://新加
                    if (model != null && model.Lhzb != null)
                    {
                        Pnl_Edit.Visible = true;
                        Lbl_ZBMC.Text = model.Lhzb.Zbxx.ZBMC;
                        Hid_ZRSZBID.Value = "";//添加时清空
                        Hid_LHZBBM.Value = model.LHZBBM;
                    }
                    break;
                case "bianji"://编辑
                    if (model != null && model.Lhzb != null)
                    {
                        Pnl_Edit.Visible = true;
                        Lbl_ZBMC.Text = model.Lhzb.Zbxx.ZBMC;
                        Hid_ZRSZBID.Value = model.ZRSZBID;
                        Hid_LHZBBM.Value = model.LHZBBM;
                        Hid_JGKHMB.Value = Txt_JGKHMB.Text = model.JGKHMB;
                        Hid_JGKHNR.Value = Txt_JGKHNR.Text = model.JGKHNR;
                        Hid_JGPFBZ.Value = Txt_JGPFBZ.Text = model.JGPFBZ;
                        Hid_JGWCSJ.Value = Txt_JGWCSJ.Text = model.JGWCSJ;
                        Hid_ZZBFZ.Value = Txt_ZZBFZ.Text = model.ZZBFZ.ToRequestString();
                    }
                    break;
                case "shanchu":
                    if (model != null)
                    {
                        //删除数据
                        model.DB_Option_Action = WebKeys.DeleteAction;
                        jxzrsZbSrv.DeleteJxzrsZbAndZbsx(model);
                    }
                    BindGrid();
                    break;
                case "up":
                    if (model != null)
                    {
                        upOneGrade(model);
                    }
                    BindGrid();
                    break;
                case "down":
                    if (model != null)
                    {
                        downOneGrade(model);
                    }
                    BindGrid();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 保存指标数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            KhJxzrszbModel model = null;
            List<KhJxzrszbModel> zbModelList = jxzrsZbSrv.GetListBySearch(
                Ddl_Niandu.SelectedValue, Ddl_Danwei.SelectedValue, Ddl_Bbmc.SelectedValue).
                Where(p => p.LHZBBM == Hid_LHZBBM.Value).ToList();
            if (zbModelList.Count > 0)
            {
                int maxXSXH = zbModelList.Max(p => p.ZXSXH).ToInt();//当前最大序号
                model = zbModelList.FirstOrDefault(p => p.ZRSZBID == Hid_ZRSZBID.Value);
                if (model != null)
                {
                    //更新
                    model.DB_Option_Action = WebKeys.UpdateAction;
                }
                else
                {
                    model = zbModelList.First();
                    model.DB_Option_Action = WebKeys.InsertAction;
                    model.ZRSZBID = "ZRSZB" + CommonTool.GetPkId();
                    model.ZTJRQ = DateTime.Now;
                    model.ZXSXH = maxXSXH + 1;
                }
                model.JGKHMB = Hid_JGKHMB.Value;
                model.JGKHNR = Hid_JGKHNR.Value;
                model.JGPFBZ = Hid_JGPFBZ.Value;
                model.JGWCSJ = Hid_JGWCSJ.Value;
                model.ZZBFZ = Hid_ZZBFZ.Value.ToDecimal();
                if (jxzrsZbSrv.Execute(model))
                {
                    Utility.ShowMsg(Page, "系统提示", "保存指标成功！", 100, "show");
                }
            }
            clear();
            Pnl_Edit.Visible = false;
            BindGrid();
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            string url = string.Format("BB={0}&BM={1}&ND={2}", Ddl_Bbmc.SelectedValue, Ddl_Danwei.SelectedValue, Ddl_Niandu.SelectedValue);
            Response.Redirect("~/Module/Kh/NdjxzrsJgbmZbList.aspx?" + url, true);
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
        /// 提请领导审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ldsh_Click(object sender, EventArgs e)
        {
            //add by qw 2014.12.15 机关部门需要自行添加绩效指标，给他们的用户发送待办
            var bm = bmjgService.GetSingle(Ddl_Danwei.SelectedValue);
            if (bm != null && bm.JGLX.Contains("职能"))
            {
                //提取该单位的所有用户，并给员工级人员发送待办消息
                var u = userService.GetUserListForValid().FirstOrDefault(p => p.JGBM == bm.JGBM && p.DUTY.Contains("负责"));
                if (u != null)
                {
                    msgService.SendMsgToUers(u.LOGINID, bm.JGBM,
                        "年度绩效责任指标审核", "需要您审核【" + bm.JGMC + "】的【" + Ddl_Niandu.SelectedValue + "】年度绩效责任指标!",
                        string.Format("/Module/Kh/NdjxzrsJgbmZbList.aspx?BM={0}&ND={1}&BB={2}", bm.JGBM, Ddl_Niandu.SelectedValue,
                        Ddl_Bbmc.SelectedValue),
                        currentModule.MID, Ddl_Danwei.SelectedValue);
                    //关闭当前用户的待办
                    msgService.CloseMessage(userModel, 0, currentModule.MID, Ddl_Danwei.SelectedValue);
                    Utility.ShowMsg(Page, "系统提示", "已提请领导审核！", 100, "show");
                }
            }
            //end
            BindGrid();
        }

        /// <summary>
        /// 显示编辑面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            string url = string.Format("BB={0}&BM={1}&ND={2}", 
                Ddl_Bbmc.SelectedValue, Ddl_Danwei.SelectedValue, Ddl_Niandu.SelectedValue);
            Response.Redirect("~/Module/Kh/NdjxzrsJgbmZbEdit.aspx?" + url, true);
        }

        /// <summary>
        /// 年度选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Niandu_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 单位选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Danwei_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 版本选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Bbmc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 下达操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Xiada_Click(object sender, EventArgs e)
        {
            var zrsModel = zrsSrv.GetListByNdAndBmjg(Ddl_Niandu.SelectedValue, Ddl_Danwei.SelectedValue).FirstOrDefault();
            if (zrsModel != null && zrsModel.ZRSZT != "1")
            {
                zrsModel.DB_Option_Action = WebKeys.UpdateAction;
                zrsModel.ZRSZT = "1";
                zrsModel.XDRQ = DateTime.Now;
                if (zrsSrv.Execute(zrsModel))
                {
                    LnkBtn_Xiada.Visible = false;
                    //关闭当前用户的待办
                    msgService.CloseMessage(userModel, 0, currentModule.MID, Ddl_Danwei.SelectedValue);
                    Utility.ShowMsg(Page, "系统提示", "绩效责任书下达成功！", 100, "show");
                }
            }
            BindGrid();
        }

        /// <summary>
        /// 上调一级
        /// </summary>
        /// <param name="currM"></param>
        private void upOneGrade(KhJxzrszbModel currM)
        {
            List<KhJxzrszbModel> list = jxzrsZbSrv.GetListBySearch(Ddl_Niandu.SelectedValue,
                Ddl_Danwei.SelectedValue, Ddl_Bbmc.SelectedValue) as List<KhJxzrszbModel>;
            int currIndex = list.FindIndex(p => p.ZRSZBID == currM.ZRSZBID);
            if (currIndex > 0 && list.Count > 1)
            {
                var prevM = list[currIndex - 1];
                if (prevM != null)
                {
                    int xsxh = currM.ZXSXH.ToInt();
                    currM.DB_Option_Action = WebKeys.UpdateAction;
                    currM.ZXSXH = prevM.ZXSXH;
                    jxzrsZbSrv.Execute(currM);
                    prevM.DB_Option_Action = WebKeys.UpdateAction;
                    prevM.ZXSXH = xsxh;
                    jxzrsZbSrv.Execute(prevM);
                }
            }
        }

        /// <summary>
        /// 下调一级
        /// </summary>
        /// <param name="currM"></param>
        private void downOneGrade(KhJxzrszbModel currM)
        {
            List<KhJxzrszbModel> list = jxzrsZbSrv.GetListBySearch(Ddl_Niandu.SelectedValue, 
                Ddl_Danwei.SelectedValue, Ddl_Bbmc.SelectedValue) as List<KhJxzrszbModel>;
            int currIndex = list.FindIndex(p => p.ZRSZBID == currM.ZRSZBID);
            if (currIndex < list.Count-1)
            {
                var nextM = list[currIndex + 1];
                if (nextM != null)
                {
                    int xsxh = currM.ZXSXH.ToInt();
                    currM.DB_Option_Action = WebKeys.UpdateAction;
                    currM.ZXSXH = nextM.ZXSXH;
                    jxzrsZbSrv.Execute(currM);
                    nextM.DB_Option_Action = WebKeys.UpdateAction;
                    nextM.ZXSXH = xsxh;
                    jxzrsZbSrv.Execute(nextM);
                }
            }
        }

        #endregion

        private void clear()
        {
            Lbl_ZBMC.Text = "";
            Hid_ZRSZBID.Value = "";
            Hid_LHZBBM.Value = "";
            Hid_JGKHMB.Value = Txt_JGKHMB.Text = "";
            Hid_JGKHNR.Value = Txt_JGKHNR.Text = "";
            Hid_JGPFBZ.Value = Txt_JGPFBZ.Text = "";
            Hid_JGWCSJ.Value = Txt_JGWCSJ.Text = "";
            Hid_ZZBFZ.Value = Txt_ZZBFZ.Text = "";
        }

    }
}