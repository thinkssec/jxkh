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
using System.Text;

namespace Enterprise.UI.Web.Kh
{

    /// <summary>
    ///二级单位完成值录入页面
    /// </summary>
    public partial class EjdwWczLuru : PageBase
    {

        /// <summary>
        /// 定量指标明细-服务类
        /// </summary>
        KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
        KhLhzbjcsjService lhzbjcsjSrv = new KhLhzbjcsjService();//量化指标基础数据
        KhKhglService khglSrv = new KhKhglService();//考核管理

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected KhKhglModel Kaohe = null;//考核期
        protected List<KhLhzbjcsjModel> LhzbjcsjList = null;//量化指标基础数据集合
        /// <summary>
        /// 网页参数
        /// </summary>
        public string UrlParam
        {
            get
            {
                string url = string.Format("KH={0}&BM={1}", Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue);
                return url;
            }
        }

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
                Btn_Add.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Hid_TabTitle.Value = TabTitle = "考核指标";
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

            //标签页选择
            if (!string.IsNullOrEmpty(Hid_TabTitle.Value))
                TabTitle = Hid_TabTitle.Value;

            if (!ChkUserLuruPermission(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue.ToInt(), userModel))
            {
                Lbl_Msg.Text = "对不起,您的权限目前为【查看】!";
                Pnl_Edit.Visible = false;
                //Pnl_EditGlzb.Visible = false;
                Btn_Add.Visible = false;
                //Btn_Jcsj.Visible = false;
            }

            //关联基础数据
            LhzbjcsjList = lhzbjcsjSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhLhzbjcsjModel>;

            //1==考核指标
            List<KhDlzbmxModel> dlzbList = dlzbmxSrv.GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            GridView1.DataSource = dlzbList;
            GridView1.DataBind();
            Utility.GroupRows(GridView1, 1);
            Lbl_Msg.Text = "注：资金单位为万元。";

            if (dlzbList.Count > 0)
            {
                if (dlzbList.Count(p => p.WCZSHRQ == null) == 0)
                {
                    Lbl_Msg.Text = "完成值已审核通过!不能再修改了!";
                    Pnl_Edit.Visible = false;
                    //Pnl_EditGlzb.Visible = false;
                    Btn_Add.Visible = false;
                    //Btn_Jcsj.Visible = false;
                }
                else if (dlzbList.Count(p => p.MBZQRRQ == null) > 0)
                {
                    Lbl_Msg.Text = "目标值还未审核通过!";
                    Pnl_Edit.Visible = false;
                    //Pnl_EditGlzb.Visible = false;
                    Btn_Add.Visible = false;
                    //Btn_Jcsj.Visible = false;
                }
            }

            //2==关联性指标
            //var glzbList = dlzbList.Where(p => !string.IsNullOrEmpty(p.LhzbModel.JSBDS) && p.LhzbjcsjLst.Count > 0).ToList();
            //GridView2.DataSource = glzbList;
            //GridView2.DataBind();
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //单位
            List<SysBmjgModel> parentBmjgLst = bmjgService.GetSameLevelBmjg(4) as List<SysBmjgModel>;
            int[] jgbms = (from c in parentBmjgLst select c.JGBM).ToArray();
            List<SysBmjgModel> bmjgTreeList = bmjgService.GetBmjgTreeLst(false).Where(p => p.XSXH.Length > 2 && !p.JGLX.Contains("职能")).ToList();
            bmjgService.BindSSECDropDownListForBmjg(Ddl_Danwei, bmjgTreeList, jgbms);
            if (Jgbm > 0)
            {
                Ddl_Danwei.SelectedValue = Jgbm.ToString();
            }
            else
            {
                Ddl_Danwei.SelectedValue = userModel.JGBM.ToString();
            }

            //考核期
            var kaohe = khglSrv.GetKhListForValid().Where(p => p.LXID == "LX2014A").ToList();
            khglSrv.BindSSECDropDownListForKaohe(Ddl_Kaohe, kaohe);
            if (!string.IsNullOrEmpty(Khid))
            {
                Ddl_Kaohe.SelectedValue = Khid;
            }
            else if (kaohe.Count > 0)
            {
                Ddl_Kaohe.SelectedValue = kaohe.First().KHID.ToString();//最近一次考核
            }

            Lbl_Msg.Text = "";
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
                KhDlzbmxModel model = e.Row.DataItem as KhDlzbmxModel;
                //指标类别 1
                e.Row.Cells[1].Text = model.LhzbModel.Zbxx.YJZBMC;
                //指标名称 2
                e.Row.Cells[2].Text = model.LhzbModel.Zbxx.ZBMC;
                //计算关系式和数据 3
                e.Row.Cells[3].Text = getTableForGlzb(model);
            }
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
                KhDlzbmxModel model = e.Row.DataItem as KhDlzbmxModel;
                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //指标类别 1
                e.Row.Cells[1].Text = model.LhzbModel.Zbxx.YJZBMC;
                //指标名称 2
                e.Row.Cells[2].Text = "<a href=\"javascript:showInfo('指标说明','" +
                    model.LhzbModel.ZBSM.ToStr() + "<hr/>" + model.LhzbModel.PFBZ.ToStr() + "','info');\">"
                    + ((model.ZbsxModel.JxzrsZb.ZZBXZ == "辅助指标") ? model.LhzbModel.GradeSymbol + model.LhzbModel.Zbxx.ZBMC : model.LhzbModel.Zbxx.ZBMC) + "</a>";
                //指标性质 考核权重 3
                e.Row.Cells[3].Text = model.ZbsxModel.JxzrsZb.ZZBXZ + "<br/>〖" + (model.ZbsxModel.SXQZ.ToDecimal() * 100).ToString("f1") + "%〗";
                ////考核权重% 4
                //e.Row.Cells[4].Text = (model.ZbsxModel.SXQZ.ToDecimal() * 100) + "%";

                //考核目标值 5
                if (model.MBZ != null) {
                    e.Row.Cells[5].Text = model.MBZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                }
                //目标值说明 6
                e.Row.Cells[6].Text = model.MBZBZ;

                if (model.WCZSHRQ != null && model.WCZ != null)
                {
                    //完成值 7
                    e.Row.Cells[7].Text = model.WCZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                    //完成值说明 8
                    e.Row.Cells[8].Text = model.WCZBZ;
                }
                else
                {
                    //完成值 7
                    e.Row.Cells[7].Text = (model.WCZSQZ != null) ? model.WCZSQZ.Value.ToString("f2") + model.LhzbModel.JSDW : "";
                    //完成值说明 8
                    e.Row.Cells[8].Text = model.WCZSQBZ;
                }
                //操作权限 9
                Image img = e.Row.FindControl("Image1") as Image;
                if (model.WCZSHRQ == null)
                {
                    img.ImageUrl = "/Resources/Images/lock_unlock.png";
                    img.ToolTip = getShenheInfo(model);
                    //还未审核
                    if (Pnl_Edit.Visible == true)
                    {
                        string bgColor = (model.ZbsxModel.JxzrsZb.ZZBXZ == "辅助指标") ? "background-color:#eeeeee" : "";
                        IList<KhDlzbmxModel> child=dlzbmxSrv.GetChildren(model.KHID.ToRequestString(),model.JGBM.ToRequestString(),model.ZBBM);
                        
                        if (model.LhzbModel.JSBDS==null||model.LhzbModel.JSBDS.IndexOf('=') == -1 || model.LhzbjcsjLst.Count == 0)
                        {
                            if (child.Count == 0)
                            {
                                //完成值 7
                                e.Row.Cells[7].Text = Utility.GetTextBox("Txt" + 7 + "_" + (e.Row.RowIndex + 1),
                                        (model.WCZSQZ), 7, (e.Row.RowIndex + 1), "number", true,
                                        "class=\"easyui-numberbox\" precision=\"2\"", "width:80px;" + bgColor);
                            }
                        }
                        //完成值说明 8
                        if (child.Count == 0)
                        {
                            e.Row.Cells[8].Text = Utility.GetTextBox("Txt" + 8 + "_" + (e.Row.RowIndex + 1),
                            model.WCZSQBZ, 8, (e.Row.RowIndex + 1), "string", false, "", "width:220px;" + bgColor);
                        }
                    }
                }
                else
                {
                    img.ImageUrl = "/Resources/Images/lock.png";
                    img.ToolTip = getShenheInfo(model);
                    //审核已通过
                    //完成值 7
                    e.Row.Cells[7].Text = (model.WCZ != null) ? model.WCZ.Value.ToString("f2") + model.LhzbModel.JSDW : "";
                    //完成值说明 8
                    e.Row.Cells[8].Text = model.WCZBZ;
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
            
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            string key = string.Empty;
            //1==考核指标
            List<KhDlzbmxModel> khdlzbList = new List<KhDlzbmxModel>();
            List<KhDlzbmxModel> dlzbList = dlzbmxSrv.GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                //ID
                string ID = GridView1.DataKeys[gvr.RowIndex].Values["ID"].ToRequestString();
                KhDlzbmxModel model = dlzbList.FirstOrDefault(p => p.ID == ID);//dlzbmxSrv.GetSingle(ID);
                if (model == null) 
                    continue; 
                model.DB_Option_Action = WebKeys.UpdateAction;

                //完成值 7
                key = "Txt" + 7 + "_" + (gvr.RowIndex + 1);
                model.WCZSQZ = Request.Form[key].ToNullOrDecimal();
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                //完成值说明 8
                key = "Txt" + 8 + "_" + (gvr.RowIndex + 1);
                model.WCZSQBZ = Request.Form[key];
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                model.WCZSQR = userModel.LOGINID;
                khdlzbList.Add(model);
            }
            //2==关联指标
            var glzbList = lhzbjcsjSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue);
            List<KhLhzbjcsjModel> lhzbjcsjList = new List<KhLhzbjcsjModel>();
            foreach (var glzb in glzbList)
            {
                glzb.DB_Option_Action = WebKeys.UpdateAction;
                key = "Txt" + glzb.JCZBID;
                glzb.ZBSBZ = glzb.ZBZ = Request.Form[key].ToNullOrDecimal();
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                lhzbjcsjList.Add(glzb);
                lhzbjcsjSrv.Execute(glzb);
            }
            //3==计算关联指标的完成值
            foreach (var dlzb in khdlzbList)
            {
                if (dlzb.LhzbjcsjLst.Count > 0)
                {
                    //有关联指标，则要代入关系式计算完成值
                    var glzbs = lhzbjcsjList.Where(p => p.ID == dlzb.ID).ToList();
                    if (dlzb.LhzbModel.JSBDS.IndexOf('=') > -1)
                    {
                        dlzb.WCZSQZ = dlzbmxSrv.CalculateGlzbsWcz(dlzb, glzbs);
                    }
                }
            }
            //4==提交完成数据
            dlzbmxSrv.ExecuteByList(khdlzbList);
            Utility.ShowMsg(Page, "系统提示", "保存完成值成功！", 100, "show");
            Pnl_Edit.Visible = false;
            //Pnl_EditGlzb.Visible = false;
            BindGrid();
        }

        /// <summary>
        /// 正式提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            List<string> shrs = new List<string>();
            string key = string.Empty;
            //1==考核指标
            List<KhDlzbmxModel> khdlzbList = new List<KhDlzbmxModel>();
            List<KhDlzbmxModel> dlzbList = dlzbmxSrv.GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                //ID
                string ID = GridView1.DataKeys[gvr.RowIndex].Values["ID"].ToRequestString();
                KhDlzbmxModel model = dlzbList.FirstOrDefault(p => p.ID == ID);//dlzbmxSrv.GetSingle(ID);
                if (model == null)
                    continue;
                model.DB_Option_Action = WebKeys.UpdateAction;

                //完成值 7
                key = "Txt" + 7 + "_" + (gvr.RowIndex + 1);
                model.WCZSQZ = Request.Form[key].ToNullOrDecimal();
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                //完成值说明 8
                key = "Txt" + 8 + "_" + (gvr.RowIndex + 1);
                model.WCZSQBZ = Request.Form[key];
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                model.WCZSQR = userModel.LOGINID;
                model.WCZSQRQ = DateTime.Now;
                khdlzbList.Add(model);//暂存
                if (model.KhJgbmdfbLst != null)
                {
                    foreach (var q in model.KhJgbmdfbLst)
                    {
                        if (!shrs.Exists(p => p == q.DFZ))
                        {
                            shrs.Add(q.DFZ);//审核人信息
                        }
                    }
                }
            }
            //2==关联指标
            var glzbList = lhzbjcsjSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue);
            List<KhLhzbjcsjModel> lhzbjcsjList = new List<KhLhzbjcsjModel>();
            foreach (var glzb in glzbList)
            {
                glzb.DB_Option_Action = WebKeys.UpdateAction;
                key = "Txt" + glzb.JCZBID;
                glzb.ZBSBZ = glzb.ZBZ = Request.Form[key].ToNullOrDecimal();
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                lhzbjcsjList.Add(glzb);//暂存
                lhzbjcsjSrv.Execute(glzb);
            }
            //3==计算关联指标的完成值
            foreach (var dlzb in khdlzbList)
            {
                if (dlzb.LhzbjcsjLst.Count > 0)
                {
                    //有关联指标，则要代入关系式计算完成值
                    var glzbs = lhzbjcsjList.Where(p => p.ID == dlzb.ID).ToList();
                    if (dlzb.LhzbModel.JSBDS.IndexOf('=') > -1)
                    {
                        dlzb.WCZSQZ = dlzbmxSrv.CalculateGlzbsWcz(dlzb, glzbs);
                    }
                }
            }
            //4==提交完成数据
            dlzbmxSrv.ExecuteByList(khdlzbList);

            //5==给审核人发送待办事务-------------------------------------------------------
            if (shrs.Count > 0)
            {
                msgService.SendMsgToUers(shrs, Ddl_Danwei.SelectedValue.ToInt(), Ddl_Kaohe.SelectedValue,
                    "完成值审核", "需要您审核【" + SysBmjgService.GetBmjgName(Ddl_Danwei.SelectedValue.ToInt()) + "】的完成值!",
                    string.Format("/M.K.WczShenhe?KH={0}&BM={1}", Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue),
                    currentModule.MID, Ddl_Danwei.SelectedValue);
            }
            //6==正式提交，则上报状态置为锁定
            KhUnlockService unlockSrv = new KhUnlockService();
            var unlockQ = unlockSrv.GetListByKhid(Ddl_Kaohe.SelectedValue).
                FirstOrDefault(p => p.JGBM == Ddl_Danwei.SelectedValue.ToInt());
            if (unlockQ != null)
            {
                unlockQ.DB_Option_Action = WebKeys.UpdateAction;
                unlockQ.TJSJ = DateTime.Now;
                unlockQ.SDBZ = "1";
                unlockQ.CZZ = userModel.USERNAME;
                unlockSrv.Execute(unlockQ);
            }
            Utility.ShowMsg(Page, "系统提示", "已正式提交完成值!请等待审核确认!", 100, "show");
            Pnl_Edit.Visible = false;
            //Pnl_EditGlzb.Visible = false;
            BindGrid();
        }
        
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            string url = string.Format("KH={0}&BM={1}", Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue);
            GobackPageUrl("?" + url);
        }

        /// <summary>
        /// 考核期选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Kaohe_SelectedIndexChanged(object sender, EventArgs e)
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
        /// 编辑操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = true;
            //Pnl_EditGlzb.Visible = true;
            BindGrid();
        }

        /// <summary>
        /// 录入基础数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Jcsj_Click(object sender, EventArgs e)
        {
            string url = string.Format("KH={0}&BM={1}", Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue);
            string backUrl = PageUrl.TrimStart('~') + "?" + url;
            backUrl = Base64.Base64Encode(backUrl);
            Response.Redirect("/M.K.EjdwJcsjData?BackUrl=" + backUrl + "&" + url, true);
        }

        /// <summary>
        /// 获取基础数据表中的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_GetData_Click(object sender, EventArgs e)
        {
            /*
             将指定单位和考核期下的基础指标数据，从财务基础表中提取相关的数据并填充
             */
            dlzbmxSrv.SetCwjcsjDataByKhid_Jgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue);
            Utility.ShowMsg(Page, "系统提示", "提取财务基础数据完成!", 100, "show");
            BindGrid();
        }

        #endregion


        #region 专用方法区

        /// <summary>
        /// 获取目标值审核信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string getShenheInfo(KhDlzbmxModel model)
        {
            StringBuilder shr = new StringBuilder();
            shr.Append("完成值审核人:");
            if (model.KhJgbmdfbLst != null)
            {
                foreach (var q in model.KhJgbmdfbLst)
                {
                    if (q.DFZ.ToInt() > 0)
                    {
                        //机构
                        shr.Append(SysBmjgService.GetBmjgName(q.DFZ.ToInt()) + "|");
                    }
                    else
                    {
                        //指定用户
                        shr.Append(SysUserService.GetUserNameByLoginId(q.DFZ));
                    }
                }
            }
            return shr.ToString();
        }

        /// <summary>
        /// 生成关联性指标的展示HTML表格
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string getTableForGlzb(KhDlzbmxModel model)
        {
            var glzbList = LhzbjcsjList.Where(p => p.ID == model.ID).OrderBy(p => p.XH).ToList();
            StringBuilder tableSB = new StringBuilder();
            tableSB.Append("<div style='width: 800px;padding: 2px;overflow:auto;'>");
            tableSB.Append("<table>");
            tableSB.Append("<tr>");
            tableSB.Append("<td colspan='" + glzbList.Count + "'>" + model.LhzbModel.JSBDS + "</td>");
            tableSB.Append("</tr>");
            tableSB.Append("<tr>");
            foreach (var q in glzbList)
            {
                tableSB.Append("<td>" + q.ZBMC + "</td>");
            }
            tableSB.Append("</tr>");
            tableSB.Append("<tr>");
            foreach (var q in glzbList)
            {
                if (Pnl_Edit.Visible == true)
                {
                    tableSB.Append("<td>" +
                        Utility.GetTextBox("Txt" + q.JCZBID, q.ZBZ, 0, 0, "number", true,
                                "class=\"easyui-numberbox\" precision=\"2\"", "width:90px;") + "</td>");
                }
                else
                {
                    tableSB.Append("<td>" + 
                        ((q.ZBZ != null) ? q.ZBZ.Value.ToString("f2") : "") + "</td>");
                }
            }
            tableSB.Append("</tr>");
            tableSB.Append("</table>");
            tableSB.Append("</div>");

            return tableSB.ToString();
        }

        #endregion


    }
}