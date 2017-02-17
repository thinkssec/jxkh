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
    ///二级单位完成值审核页面
    /// </summary>
    public partial class EjdwWczShenhe : PageBase
    {

        /// <summary>
        /// 定量指标明细-服务类
        /// </summary>
        KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
        KhLhzbjcsjService lhzbjcsjSrv = new KhLhzbjcsjService();//量化指标基础数据
        KhJgbmdfbService jgbmdfSrv = new KhJgbmdfbService();//机关部门打分表
        KhKhglService khglSrv = new KhKhglService();//考核管理
        KhSjsbService sjsbSrv = new KhSjsbService();//数据上报

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected List<KhLhzbjcsjModel> LhzbjcsjList = null;//量化指标基础数据集合
        protected List<KhSjsbModel> SjsbList = null;//数据上报集合
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
                TabTitle = "考核指标";
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
            if (!string.IsNullOrEmpty(Hid_TabTitle.Value))
            {
                TabTitle = Hid_TabTitle.Value;
            }

            if (!ChkUserLuruPermission(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue.ToInt(), userModel))
            {
                Lbl_Msg.Text = "对不起,您的权限目前为【查看】!";
                Pnl_Edit.Visible = false;
                Btn_Add.Visible = false;
                Btn_Jcsj.Visible = false;
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
                if (dlzbList.Count(p => p.WCZSDRQ == null) == 0)
                {
                    Lbl_Msg.Text = "完成值已审定通过!不能再修改了!";
                    Pnl_Edit.Visible = false;
                    Btn_Add.Visible = false;
                    Btn_Jcsj.Visible = false;
                }
                else if (dlzbList.Count(p => p.WCZSQRQ == null) > 0)
                {
                    Lbl_Msg.Text = "该单位还未正式提交完成值!您还不能审核!";
                    Pnl_Edit.Visible = false;
                    Btn_Add.Visible = false;
                    Btn_Jcsj.Visible = false;
                }
            }

            //2==关联性指标
            var glzbList = dlzbList.Where(p => !string.IsNullOrEmpty(p.LhzbModel.JSBDS) && p.LhzbjcsjLst.Count > 0).ToList();
            GridView2.DataSource = glzbList;
            GridView2.DataBind();

            //3==提取当前用户的所有测评单位
            SjsbList = sjsbSrv.GetListByKhid(Ddl_Kaohe.SelectedValue) as List<KhSjsbModel>;
            List<KhJgbmdfbModel> myDfDanWeiLst =
                jgbmdfSrv.GetListByKhidAndDfz(Ddl_Kaohe.SelectedValue, userModel).
                Where(p => !string.IsNullOrEmpty(p.ID)).ToList();
            var oneBmgjList = myDfDanWeiLst.DistinctBy(p => p.JGBM).ToList();
            GridView3.DataSource = oneBmgjList;
            GridView3.DataBind();
            //4==友好提示
            if (oneBmgjList.Count > 0)
            {
                Lbl_Msg.Text += "&nbsp;&nbsp;友情提示：您还需审核的单位数量为〖" + oneBmgjList.Count(p => p.DFSJ == null) + "〗";
            }
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
        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhJgbmdfbModel model = e.Row.DataItem as KhJgbmdfbModel;
                //单位名称 1
                string url = string.Format("KH={0}&BM={1}", model.KHID, model.JGBM);
                string bmmc = SysBmjgService.GetBmjgName(model.JGBM.Value);
                e.Row.Cells[1].Text = string.Format("<a href='{0}'>{1}</a>",
                    GetPageUrlAndParams("?" + url).TrimStart("~".ToCharArray()), bmmc);
                //状态 2
                e.Row.Cells[2].Text = (model.DFSJ != null) ? "已审核" : "<font color='red'>未审核</font>";
                //日期 3
                e.Row.Cells[3].Text = model.DFSJ.ToDateYMDFormat();
                //考核文件 4
                if (SjsbList != null)
                {
                    KhSjsbModel sjsb = SjsbList.FirstOrDefault(p => p.JGBM == model.JGBM.Value);
                    if (sjsb != null && !string.IsNullOrEmpty(sjsb.SBFJ))
                    {
                        if (ChkUserLuruPermission(Ddl_Kaohe.SelectedValue, sjsb.Bmjg.JGBM, userModel))
                        {
                            e.Row.Cells[4].Text = sjsb.SBFJ.ToAttachHtmlByOne();
                        }
                        else
                        {
                            e.Row.Cells[4].Text = "已上报";
                        }
                    }
                    else
                    {
                        e.Row.Cells[4].Text = "未上报";
                    }
                }
            }
        }

        /// <summary>
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            KhDlzbmxModel model = e.Row.DataItem as KhDlzbmxModel;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //指标类别 1
                e.Row.Cells[1].Text = model.LhzbModel.Zbxx.YJZBMC;
                //指标名称 2
                e.Row.Cells[2].Text = model.LhzbModel.Zbxx.ZBMC;
                //计算关系式和数据 3
                bool isValid = IsShr(model, "", userModel);
                e.Row.Cells[3].Text = getTableForGlzb(model, isValid);
                //计算完成值 4
                if (model.WCZ != null)
                {
                    e.Row.Cells[4].Text = model.WCZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                }
                else if (model.WCZSHZ != null)
                {
                    e.Row.Cells[4].Text = model.WCZSHZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                }
                else if (model.WCZSQZ != null)
                {
                    e.Row.Cells[4].Text = model.WCZSQZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                }
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
                //序号
                if (Pnl_Edit.Visible == true)
                {
                    CheckBox chk = e.Row.FindControl("Chk_XH") as CheckBox;
                    if (chk != null)
                    {
                        chk.Text = (e.Row.DataItemIndex + 1).ToString();
                        chk.Checked = false;
                    }
                }
                else
                {
                    e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
                }
                //指标类别 1
                e.Row.Cells[1].Text = model.LhzbModel.Zbxx.YJZBMC;
                //指标名称 2
                e.Row.Cells[2].Text = "<a href=\"javascript:showInfo('指标说明','" +
                    model.LhzbModel.ZBSM.ToStr() + "<hr/>" + model.LhzbModel.PFBZ.ToStr() + "<hr/>" +
                    ((model.LhzbModel.Jsgz != null) ? "〖" + model.LhzbModel.Jsgz.GZMC + "," + Server.HtmlEncode(model.LhzbModel.Jsgz.GZBDS.ToStr()) + "〗" : "") + "','info');\">"
                    + ((model.ZbsxModel.JxzrsZb.ZZBXZ == "辅助指标") ? model.LhzbModel.GradeSymbol + model.LhzbModel.Zbxx.ZBMC : model.LhzbModel.Zbxx.ZBMC) + "</a>";
                //指标性质 考核权重 3
                e.Row.Cells[3].Text = model.ZbsxModel.JxzrsZb.ZZBXZ + "<br/>〖" + (model.ZbsxModel.SXQZ.ToDecimal() * 100).ToString("f1") + "%〗";
                //考核目标值4
                if (model.MBZ != null)
                {
                    e.Row.Cells[4].Text = model.MBZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                    e.Row.Cells[4].ToolTip = model.MBZBZ;
                }
                //上报完成值5
                e.Row.Cells[5].Text = (model.WCZSQZ != null) ? model.WCZSQZ.Value.ToString("f2") + model.LhzbModel.JSDW : "";
                //完成值说明6
                e.Row.Cells[6].Text = model.WCZSQBZ;

                if (model.WCZ != null)
                {
                    //最终完成值7
                    e.Row.Cells[7].Text = model.WCZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                    //说明8
                    e.Row.Cells[8].Text = model.WCZBZ;
                }
                else if (model.WCZSHZ != null)
                {
                    //审核完成值7
                    e.Row.Cells[7].Text = model.WCZSHZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                    //审核值说明8
                    e.Row.Cells[8].Text = model.WCZSHBZ;
                }
                //本项实际得分9
                e.Row.Cells[9].Text = ((model.SJDF != null) ? model.SJDF.Value.ToString("f2") : "");
                e.Row.Cells[9].ToolTip = model.DFJSQK;//得分计算情况
                //操作权限 10
                Image img = e.Row.FindControl("Image1") as Image;
                if (IsShr(model, "", userModel))
                {
                    img.ImageUrl = "/Resources/Images/yes.png";//有权限
                    img.ToolTip = getShenheInfo(model);
                    if (model.WCZSDRQ == null)
                    {
                        //还未完成审定，则可修改
                        if (Pnl_Edit.Visible == true)
                        {
                            string bgColor = (model.ZbsxModel.JxzrsZb.ZZBXZ == "辅助指标") ? "background-color:#eeeeee" : "";
                            //考核目标值 4
                            e.Row.Cells[4].Text = Utility.GetTextBox("Txt" + 4 + "_" + (e.Row.RowIndex + 1),
                                    (model.MBZ), 4, (e.Row.RowIndex + 1), "number", true,
                                    "class=\"easyui-numberbox\" precision=\"2\"", "width:80px;" + bgColor);
                            //审核完成值 7
                            e.Row.Cells[7].Text = Utility.GetTextBox("Txt" + 7 + "_" + (e.Row.RowIndex + 1),
                                ((model.WCZSHZ == null) ? model.WCZSQZ : model.WCZSHZ), 7, (e.Row.RowIndex + 1), "number", true,
                                    "class=\"easyui-numberbox\" precision=\"2\"", "width:80px;" + bgColor);
                            //审核说明 8
                            e.Row.Cells[8].Text = Utility.GetTextBox("Txt" + 8 + "_" + (e.Row.RowIndex + 1),
                            model.WCZSHBZ, 8, (e.Row.RowIndex + 1), "string", false, "", "width:220px;" + bgColor);
                            //本项实际得分9
                            e.Row.Cells[9].Text = Utility.GetTextBox("Txt" + 9 + "_" + (e.Row.RowIndex + 1),
                                        (model.SJDF), 9, (e.Row.RowIndex + 1), "number", true,
                                        "class=\"easyui-numberbox\" precision=\"2\"", "width:80px;" + bgColor);
                        }
                    }
                }
                else
                {
                    img.ImageUrl = "/Resources/Images/no.png";//无权限
                    img.ToolTip = getShenheInfo(model);
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
        /// 基础数据审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Upd_Click(object sender, EventArgs e)
        {
            string url = string.Format("KH={0}&BM={1}", Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue);
            string backUrl = PageUrl.TrimStart('~') + "?" + url;
            backUrl = Base64.Base64Encode(backUrl);
            Response.Redirect("/M.K.EjdwJcsjData?BackUrl=" + backUrl + "&" + url, true);
        }

        /// <summary>
        /// 计算指标得分并保存
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

                //重新计算标志
                CheckBox chk = gvr.FindControl("Chk_XH") as CheckBox;
                if (chk != null)
                {
                    model.IsCalculate = chk.Checked;
                }

                model.DB_Option_Action = WebKeys.UpdateAction;
                //目标值 4
                key = "Txt" + 4 + "_" + (gvr.RowIndex + 1);
                model.MBZ = Request.Form[key].ToNullOrDecimal();
                //审核完成值 7
                key = "Txt" + 7 + "_" + (gvr.RowIndex + 1);
                model.WCZ = model.WCZSHZ = Request.Form[key].ToNullOrDecimal();
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                   
                //}
                //审核说明 8
                key = "Txt" + 8 + "_" + (gvr.RowIndex + 1);
                model.WCZBZ = model.WCZSHBZ = Request.Form[key];
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                   
                //}
                //本项实际得分9
                key = "Txt" + 9 + "_" + (gvr.RowIndex + 1);
                model.SJDF = Request.Form[key].ToNullOrDecimal();
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
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
                glzb.ZBSDZ = glzb.ZBZ = Request.Form[key].ToNullOrDecimal();
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                lhzbjcsjList.Add(glzb);
                lhzbjcsjSrv.Execute(glzb);
            }
            //如果标志位不为空，则要全部重新计算
            //bool isCalculateAll = true;// (!string.IsNullOrEmpty(Hid_Recount.Value));
            //3==计算关联指标的完成值
            foreach (var dlzb in khdlzbList)
            {
                if (dlzb.LhzbjcsjLst.Count > 0 && dlzb.IsCalculate)
                {
                    //有关联指标，则要代入关系式计算完成值
                    var glzbs = lhzbjcsjList.Where(p => p.ID == dlzb.ID).ToList();
                    if (dlzb.LhzbModel.JSBDS.IndexOf('=') > -1)
                    {
                        dlzb.WCZ = dlzb.WCZSHZ = dlzbmxSrv.CalculateGlzbsWcz(dlzb, glzbs);
                    }
                }
            }
            //4==根据审核完成值计算各指标的实际得分并提交完成数据
            dlzbmxSrv.CalDlzbmxSjdfAndSave(khdlzbList, true);

            Utility.ShowMsg(Page, "系统提示", "保存审核完成值成功！", 100, "show");
            //Pnl_Edit.Visible = false;
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
                //目标值 4
                key = "Txt" + 4 + "_" + (gvr.RowIndex + 1);
                model.MBZ = Request.Form[key].ToNullOrDecimal();
                //审核完成值 7
                key = "Txt" + 7 + "_" + (gvr.RowIndex + 1);
                model.WCZ = model.WCZSHZ = Request.Form[key].ToNullOrDecimal();
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                //审核说明 8
                key = "Txt" + 8 + "_" + (gvr.RowIndex + 1);
                model.WCZBZ = model.WCZSHBZ = Request.Form[key];
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                //本项实际得分9
                key = "Txt" + 9 + "_" + (gvr.RowIndex + 1);
                model.SJDF = Request.Form[key].ToNullOrDecimal();
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                model.WCZSHRQ = DateTime.Now;//正式保存
                //====正式保存时才记录当前用户的审核情况
                var dfzModel = model.KhJgbmdfbLst.FirstOrDefault(p => p.DFZ == userModel.JGBM.ToRequestString());
                if (dfzModel != null)
                {
                    dfzModel.DB_Option_Action = WebKeys.UpdateAction;
                    //审核说明
                    dfzModel.DFSM = model.WCZBZ;
                    dfzModel.DFSJ = DateTime.Now;
                    //保存
                    jgbmdfSrv.Execute(dfzModel);
                }
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
                glzb.ZBSDZ = glzb.ZBZ = Request.Form[key].ToNullOrDecimal();
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                lhzbjcsjList.Add(glzb);
                lhzbjcsjSrv.Execute(glzb);
            }
            ////3==计算关联指标的完成值
            //foreach (var dlzb in khdlzbList)
            //{
            //    if (dlzb.LhzbjcsjLst.Count > 0)
            //    {
            //        //有关联指标，则要代入关系式计算完成值
            //        var glzbs = lhzbjcsjList.Where(p => p.ID == dlzb.ID).ToList();
            //        if (dlzb.LhzbModel.JSBDS.IndexOf('=') > -1)
            //        {
            //            dlzb.WCZ = dlzb.WCZSHZ = dlzbmxSrv.CalculateGlzbsWcz(dlzb, glzbs);
            //        }
            //    }
            //}
            //4==只保存结果不再重新计算
            dlzbmxSrv.ExecuteByList(khdlzbList);
            //5==关闭消息
            if (!string.IsNullOrEmpty(MSGID))
            {
                msgService.CloseMessage(MSGID);
            }
            else
            {
                msgService.CloseMessage(userModel, Ddl_Kaohe.SelectedValue.ToInt(), "1013", Ddl_Danwei.SelectedValue);
            }

            Utility.ShowMsg(Page, "系统提示", "指标审核完成！", 100, "show");
            Pnl_Edit.Visible = false;
            Pnl_EditGlzb.Visible = false;
            BindGrid();
        }

        /// <summary>
        /// 获取基础表数据
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
            Pnl_EditGlzb.Visible = true;
            BindGrid();
            Lbl_Msg.Text += "    说明：‘全选’时可以对选定的指标重新计算得分！";
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
        /// <param name="model">定量指标</param>
        /// <param name="isValid">可编辑标志</param>
        /// <returns></returns>
        private string getTableForGlzb(KhDlzbmxModel model, bool isValid)
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
                if (Pnl_Edit.Visible == true && isValid)
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