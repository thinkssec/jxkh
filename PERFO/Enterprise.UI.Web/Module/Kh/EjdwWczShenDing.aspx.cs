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
using Enterprise.Model.Perfo;

namespace Enterprise.UI.Web.Kh
{

    /// <summary>
    ///二级单位完成值审定页面
    /// </summary>
    public partial class EjdwWczShenDing : PageBase
    {

        /// <summary>
        /// 定量指标明细-服务类
        /// </summary>
        KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
        KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//打分指标服务类
        KhJgbmdfbService jgbmdfSrv = new KhJgbmdfbService();//机关部门打分表
        KhKhglService khglSrv = new KhKhglService();//考核管理
        KhNdxsService ndxsSrv = new KhNdxsService();//经营难度系数

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected decimal EjdwHeji = 0M;//二级单位合计得分
        protected decimal LdbzLhzbHeji = 0M;//领导班子量化指标合计得分
        protected decimal LdbzDfzbHeji = 0M;//领导班子量化指标合计得分

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
                Hid_TabTitle.Value = TabTitle = "二级单位考核指标";
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
            //1==二级单位考核指标
            //定量
            List<KhDlzbmxModel> dlzbList = dlzbmxSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            var ejdwDlzbLst = dlzbList.Where(p => p.KHDX == ((int)WebKeys.KaoheType.二级单位).ToString());
            //打分
            List<KhDfzbmxModel> dfzbList = dfzbmxSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDfzbmxModel>;
            var ejdwDfzbLst = dfzbList.Where(p => p.KHDX == ((int)WebKeys.KaoheType.二级单位).ToString());
            //合成数据集
            List<PerfoSuperModel> dataList = new List<PerfoSuperModel>();
            dataList.AddRange(ejdwDlzbLst);
            dataList.AddRange(ejdwDfzbLst);
            if (dlzbList.Count > 0)
            {
                if (dlzbList.Count(p => p.WCZSHRQ == null) > 0)
                {
                    Lbl_Msg.Text = "该单位还未完成审核!您现在还不能审定!";
                    Pnl_Edit.Visible = false;
                    Btn_Add.Visible = false;
                }
                else if (dlzbList.Count(p => p.WCZSDRQ == null) == 0)
                {
                    Lbl_Msg.Text = "该单位已审定完成!";
                }
                else
                {
                    Lbl_Msg.Text = "";
                }
            }
            GridView1.DataSource = dataList;
            GridView1.DataBind();
            Utility.GroupRows(GridView1, 1);

            //2==领导班子考核指标
            //定量
            var ldbzDlzbLst = dlzbList.Where(p => p.KHDX == ((int)WebKeys.KaoheType.二级单位).ToString());
            //打分
            var ldbzDfzbLst = dfzbList.Where(p => p.KHDX == ((int)WebKeys.KaoheType.领导班子).ToString());
            //合成数据集
            List<PerfoSuperModel> ldbzDataList = new List<PerfoSuperModel>();
            ldbzDataList.AddRange(ldbzDlzbLst);
            ldbzDataList.AddRange(ldbzDfzbLst);
            //GridView2.DataSource = ldbzDataList;
            //GridView2.DataBind();
            //Utility.GroupRows(GridView2, 1);
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
                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                HiddenField hid = e.Row.FindControl("Hid_ID") as HiddenField;
                PerfoSuperModel supperM = e.Row.DataItem as PerfoSuperModel;
                if (supperM.GetType().IsAssignableFrom(typeof(KhDlzbmxModel)))
                {
                    //定量指标
                    KhDlzbmxModel model = supperM as KhDlzbmxModel;
                    hid.Value = model.ID;

                    //得分累计
                    if (model.ZbsxModel.JxzrsZb.ZZBXZ == "主指标")
                        LdbzLhzbHeji += model.SJDF.ToDecimal();

                    //指标类别 1
                    e.Row.Cells[1].Text = model.LhzbModel.Zbxx.YJZBMC;
                    //指标名称 2
                    e.Row.Cells[2].Text = "<a href=\"javascript:showInfo('指标说明','" +
                        model.LhzbModel.ZBSM.ToStr() + "<hr/>" + model.LhzbModel.PFBZ.ToStr() + "','info');\">"
                        + ((model.ZbsxModel.JxzrsZb.ZZBXZ == "辅助指标") ? model.LhzbModel.GradeSymbol + model.LhzbModel.Zbxx.ZBMC : model.LhzbModel.Zbxx.ZBMC) + "</a>";
                    //指标性质 考核权重 3
                    e.Row.Cells[3].Text = model.ZbsxModel.JxzrsZb.ZZBXZ + "<br/>〖" 
                        + (model.ZbsxModel.SXQZ.ToDecimal() * 100).ToString("f1") + "%〗";
                    //考核目标值4
                    if (model.MBZ != null)
                    {
                        e.Row.Cells[4].Text = model.MBZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                        e.Row.Cells[4].ToolTip = model.MBZBZ;
                    }
                    //审核完成值5
                    e.Row.Cells[5].Text = (model.WCZSHZ != null) ? model.WCZSHZ.Value.ToString("f2") + model.LhzbModel.JSDW : "";
                    //审核说明6
                    e.Row.Cells[6].Text = model.WCZSHBZ;
                    //审定完成值 7
                    e.Row.Cells[7].Text = (model.WCZ != null) ? model.WCZ.Value.ToString("f2") + model.LhzbModel.JSDW : "";
                    //审定说明 8
                    e.Row.Cells[8].Text = model.WCZBZ;
                    //本项得分9
                    e.Row.Cells[9].Text = model.SJDF.ToRequestString();
                }
                else if (supperM.GetType().IsAssignableFrom(typeof(KhDfzbmxModel)))
                {
                    //打分指标
                    KhDfzbmxModel dfModel = supperM as KhDfzbmxModel;
                    hid.Value = dfModel.DFZBID;
                    //得分累计
                    LdbzDfzbHeji += dfModel.SJDF.ToDecimal();
                    //指标类别 1
                    e.Row.Cells[1].Text = dfModel.DfzbModel.Zbxx.YJZBMC;
                    //指标名称 2
                    e.Row.Cells[2].Text = "<a href=\"javascript:showInfo('指标说明','" +
                        dfModel.DfzbModel.PFLX.ToStr() + "<hr/>" + dfModel.DfzbModel.PFBZ.ToStr() + "','info');\">"
                        + ((dfModel.ZbsxModel.JxzrsZb.ZZBXZ == "辅助指标") ? "﹄﹄" + dfModel.DfzbModel.Zbxx.ZBMC : dfModel.DfzbModel.Zbxx.ZBMC) + "</a>";
                    if (Pnl_Edit.Visible == true)
                    {
                        //打分理由 3
                        
                            e.Row.Cells[3].Text = Utility.GetTextBox("TxtGv2" + 3 + "_" + (e.Row.RowIndex + 1),
                            dfModel.DFBZ, 3, (e.Row.RowIndex + 1), "string", false, "", "width:100%;");
                            //本项得分9
                            e.Row.Cells[9].Text = Utility.GetTextBox("TxtGv2" + 9 + "_" + (e.Row.RowIndex + 1),
                                        (dfModel.SJDF), 9, (e.Row.RowIndex + 1), "number", true,
                                        "class=\"easyui-numberbox\" precision=\"0\"", "width:80px;");
                    }
                    else
                    {
                        //打分理由 3
                        e.Row.Cells[3].Text = dfModel.DFBZ;
                        //本项得分9
                        e.Row.Cells[9].Text = dfModel.SJDF.ToRequestString();
                    }
                    e.Row.Cells[3].ColumnSpan = 6;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Justify;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                //合计
                var ndxsModel = ndxsSrv.GetListByKhid(Ddl_Kaohe.SelectedValue).
                    FirstOrDefault(p=>p.JGBM == Ddl_Danwei.SelectedValue.ToInt());
                decimal jyndxs = 1.0M;
                if (ndxsModel != null)
                {
                    jyndxs = ndxsModel.NDXS.ToDecimal();
                    e.Row.Cells[2].Text = jyndxs.ToRequestString();
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                }
                e.Row.Cells[0].ColumnSpan = 2;
                e.Row.Cells[0].Text = "经营难度系数：";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].ColumnSpan = 6;
                e.Row.Cells[3].Text = "得分小计：";
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                decimal ldbzZdf = LdbzLhzbHeji * jyndxs + LdbzDfzbHeji;
                e.Row.Cells[9].Text = string.Format("<font color='red'>{0}</font>", ldbzZdf.ToString("f2"));
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
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
                bool isMainZb = false;
                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                HiddenField hid = e.Row.FindControl("Hid_ID") as HiddenField;
                PerfoSuperModel supperM = e.Row.DataItem as PerfoSuperModel;
                if (supperM.GetType().IsAssignableFrom(typeof(KhDlzbmxModel)))
                {
                    //定量指标
                    KhDlzbmxModel model = supperM as KhDlzbmxModel;
                    hid.Value = model.ID;

                    //得分累计
                    if (model.ZbsxModel.JxzrsZb.ZZBXZ == "主指标")
                    {
                        isMainZb = true;
                        EjdwHeji += model.SJDF.ToDecimal();
                    }

                    //指标类别 1
                    e.Row.Cells[1].Text = model.LhzbModel.Zbxx.YJZBMC;
                    //指标名称 2
                    e.Row.Cells[2].Text = "<a href=\"javascript:showInfo('指标说明','" +
                        model.LhzbModel.ZBSM.ToStr() + "<hr/>" + model.LhzbModel.PFBZ.ToStr() + "','info');\">"
                        + ((model.ZbsxModel.JxzrsZb.ZZBXZ == "辅助指标") ? model.LhzbModel.GradeSymbol + model.LhzbModel.Zbxx.ZBMC : model.LhzbModel.Zbxx.ZBMC) + "</a>";
                    //指标性质 考核权重 3
                    e.Row.Cells[3].Text = model.ZbsxModel.JxzrsZb.ZZBXZ + "<br/>〖" 
                        + (model.ZbsxModel.SXQZ.ToDecimal() * 100).ToString("f1") + "%〗";
                    //考核目标值4
                    if (model.MBZ != null)
                    {
                        e.Row.Cells[4].Text = model.MBZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                        e.Row.Cells[4].ToolTip = model.MBZBZ;
                    }
                    //审核完成值5
                    e.Row.Cells[5].Text = (model.WCZSHZ != null) ? model.WCZSHZ.Value.ToString("f2") + model.LhzbModel.JSDW : "";
                    //审核说明6
                    e.Row.Cells[6].Text = model.WCZSHBZ;

                    if (Pnl_Edit.Visible == true)
                    {
                        string bgColor = (model.ZbsxModel.JxzrsZb.ZZBXZ == "辅助指标") ? "background-color:#eeeeee" : "";
                        //审定完成值 7
                        IList<KhDlzbmxModel> child = dlzbmxSrv.GetChildren(model.KHID.ToRequestString(), model.JGBM.ToRequestString(), model.ZBBM);
                        if (child.Count == 0)
                        {
                            e.Row.Cells[7].Text = Utility.GetTextBox("TxtGv1" + 7 + "_" + (e.Row.RowIndex + 1),
                                    (model.WCZ), 7, (e.Row.RowIndex + 1), "number", true,
                                    "class=\"easyui-numberbox\" precision=\"2\"", "width:80px;" + bgColor);
                            //审定说明 8
                            e.Row.Cells[8].Text = Utility.GetTextBox("TxtGv1" + 8 + "_" + (e.Row.RowIndex + 1),
                            model.WCZBZ, 8, (e.Row.RowIndex + 1), "string", false, "", "width:220px;" + bgColor);
                            //本项得分9
                            e.Row.Cells[9].Text = Utility.GetTextBox("TxtGv1" + 9 + "_" + (e.Row.RowIndex + 1),
                                        (model.SJDF), 9, (e.Row.RowIndex + 1), "number", true,
                                        "class=\"easyui-numberbox\" precision=\"2\"", "width:80px;" + bgColor);
                        }
                    }
                    else
                    {
                        //审定完成值 7
                        e.Row.Cells[7].Text = (model.WCZ != null) ? model.WCZ.Value.ToString("f2") + model.LhzbModel.JSDW : "";
                        //审定说明 8
                        e.Row.Cells[8].Text = model.WCZBZ;
                        //本项得分9
                        e.Row.Cells[9].Text = (isMainZb) ? string.Format("<b>{0}</b>", model.SJDF) : model.SJDF.ToRequestString();
                    }
                }
                else if (supperM.GetType().IsAssignableFrom(typeof(KhDfzbmxModel)))
                {
                    //打分指标
                    KhDfzbmxModel dfModel = supperM as KhDfzbmxModel;
                    hid.Value = dfModel.DFZBID;

                    //得分累计
                    EjdwHeji += dfModel.SJDF.ToDecimal();

                    //指标类别 1
                    e.Row.Cells[1].Text = dfModel.DfzbModel.Zbxx.YJZBMC;
                    //指标名称 2
                    e.Row.Cells[2].Text = "<a href=\"javascript:showInfo('指标说明','" +
                        dfModel.DfzbModel.PFLX.ToStr() + "<hr/>" + dfModel.DfzbModel.PFBZ.ToStr() + "','info');\">" 
                        + dfModel.DfzbModel.Zbxx.ZBMC + "</a>";

                    if (Pnl_Edit.Visible == true)
                    {
                        //打分理由 3
                        e.Row.Cells[3].Text = Utility.GetTextBox("TxtGv1" + 3 + "_" + (e.Row.RowIndex + 1),
                        dfModel.DFBZ, 3, (e.Row.RowIndex + 1), "string", false, "", "width:100%;");
                        //本项得分9
                        e.Row.Cells[9].Text = Utility.GetTextBox("TxtGv1" + 9 + "_" + (e.Row.RowIndex + 1),
                                    (dfModel.SJDF), 9, (e.Row.RowIndex + 1), "number", true,
                                    "class=\"easyui-numberbox\" precision=\"0\"", "width:80px;");
                    }
                    else
                    {
                        //打分理由 3
                        e.Row.Cells[3].Text = dfModel.DFBZ;
                        //本项得分9
                        e.Row.Cells[9].Text = "<b>" + dfModel.SJDF.ToRequestString() + "</b>";
                    }
                    e.Row.Cells[3].ColumnSpan = 6;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Justify;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].ColumnSpan = 9;
                e.Row.Cells[0].Text = "得分小计：";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                //合计
                e.Row.Cells[9].Text = string.Format("<font color='red'>{0}</font>", EjdwHeji.ToString("f2"));
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        /// <summary>
        /// 计算指标得分并保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            //定量
            List<KhDlzbmxModel> dlzbList = dlzbmxSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            //打分
            List<KhDfzbmxModel> dfzbList = dfzbmxSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDfzbmxModel>;
            string key = string.Empty;
            //1==二级单位
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                HiddenField hid = gvr.FindControl("Hid_ID") as HiddenField;
                string ID = hid.Value;
                if (ID.StartsWith("LH"))
                {
                    //定量指标
                    KhDlzbmxModel model = dlzbList.FirstOrDefault(p => p.ID == ID);//dlzbmxSrv.GetSingle(ID);
                    if (model == null)
                        continue;
                    model.DB_Option_Action = WebKeys.UpdateAction;

                    //审定完成值 7
                    key = "TxtGv1" + 7 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.WCZ = model.WCZSDZ = Request.Form[key].ToDecimal();
                    }
                    //审定说明 8
                    key = "TxtGv1" + 8 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.WCZBZ = model.WCZSDBZ = Request.Form[key];
                    }
                    //本项得分9
                    key = "TxtGv1" + 9 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.SJDF = Request.Form[key].ToDecimal();
                    }
                    dlzbmxSrv.Execute(model);
                }
                else if (ID.StartsWith("DF"))
                {
                    //打分指标
                    KhDfzbmxModel dfModel = dfzbList.FirstOrDefault(p=>p.DFZBID == ID);
                    if (dfModel == null)
                        continue;
                    dfModel.DB_Option_Action = WebKeys.UpdateAction;
                    //理由3
                    key = "TxtGv1" + 3 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        dfModel.DFBZ = Request.Form[key];
                    }
                    //打分值9
                    key = "TxtGv1" + 9 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        dfModel.SJDF = dfModel.DFSZ = Request.Form[key].ToDecimal();
                    }
                    dfzbmxSrv.Execute(dfModel);
                }
            }
            //2==领导班子
            //foreach (GridViewRow gvr in GridView2.Rows)
            //{
            //    HiddenField hid = gvr.FindControl("Hid_ID") as HiddenField;
            //    string ID = hid.Value;
            //    if (ID.StartsWith("DF"))
            //    {
            //        //打分指标
            //        KhDfzbmxModel dfModel = dfzbList.FirstOrDefault(p => p.DFZBID == ID); //dfzbmxSrv.GetSingle(ID);
            //        if (dfModel == null)
            //            continue;
            //        dfModel.DB_Option_Action = WebKeys.UpdateAction;
            //        //理由3
            //        key = "TxtGv2" + 3 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            dfModel.DFBZ = Request.Form[key];
            //        }
            //        //打分值9
            //        key = "TxtGv2" + 9 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            dfModel.SJDF = dfModel.DFSZ = Request.Form[key].ToDecimal();
            //        }
            //        dfzbmxSrv.Execute(dfModel);
            //    }
            //}
            Utility.ShowMsg(Page, "系统提示", "保存得分成功！", 100, "show");
            Pnl_Edit.Visible = false;
            BindGrid();
        }

        /// <summary>
        /// 正式提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            //定量
            List<KhDlzbmxModel> dlzbList = dlzbmxSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            //打分
            List<KhDfzbmxModel> dfzbList = dfzbmxSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDfzbmxModel>;
            string key = string.Empty;
            //1==二级单位
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                HiddenField hid = gvr.FindControl("Hid_ID") as HiddenField;
                string ID = hid.Value;
                if (ID.StartsWith("LH"))
                {
                    //定量指标
                    KhDlzbmxModel model = dlzbList.FirstOrDefault(p => p.ID == ID);// dlzbmxSrv.GetSingle(ID);
                    if (model == null)
                        continue;
                    model.DB_Option_Action = WebKeys.UpdateAction;

                    //审定完成值 7
                    key = "TxtGv1" + 7 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.WCZ = model.WCZSDZ = Request.Form[key].ToDecimal();
                    }
                    //审定说明 8
                    key = "TxtGv1" + 8 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.WCZBZ = model.WCZSDBZ = Request.Form[key];
                    }
                    //本项得分9
                    key = "TxtGv1" + 9 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.SJDF = Request.Form[key].ToDecimal();
                    }
                    model.WCZSDRQ = DateTime.Now;
                    dlzbmxSrv.Execute(model);
                }
                else if (ID.StartsWith("DF"))
                {
                    //打分指标
                    KhDfzbmxModel dfModel = dfzbList.FirstOrDefault(p => p.DFZBID == ID);// dfzbmxSrv.GetSingle(ID);
                    if (dfModel == null)
                        continue;
                    dfModel.DB_Option_Action = WebKeys.UpdateAction;
                    //理由3
                    key = "TxtGv1" + 3 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        dfModel.DFBZ = Request.Form[key];
                    }
                    //打分值9
                    key = "TxtGv1" + 9 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        dfModel.SJDF = dfModel.DFSZ = Request.Form[key].ToDecimal();
                    }
                    dfzbmxSrv.Execute(dfModel);
                }
            }
            //2==领导班子
            //foreach (GridViewRow gvr in GridView2.Rows)
            //{
            //    HiddenField hid = gvr.FindControl("Hid_ID") as HiddenField;
            //    string ID = hid.Value;
            //    if (ID.StartsWith("DF"))
            //    {
            //        //打分指标
            //        KhDfzbmxModel dfModel = dfzbList.FirstOrDefault(p => p.DFZBID == ID);//dfzbmxSrv.GetSingle(ID);
            //        if (dfModel == null)
            //            continue;
            //        dfModel.DB_Option_Action = WebKeys.UpdateAction;
            //        //理由3
            //        key = "TxtGv2" + 3 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            dfModel.DFBZ = Request.Form[key];
            //        }
            //        //打分值9
            //        key = "TxtGv2" + 9 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            dfModel.SJDF = dfModel.DFSZ = Request.Form[key].ToDecimal();
            //        }
            //        dfzbmxSrv.Execute(dfModel);
            //    }
            //}
            //3==关闭消息
            if (!string.IsNullOrEmpty(MSGID))
            {
                msgService.CloseMessage(MSGID);
            }
            else
            {
                msgService.CloseMessage(userModel, Ddl_Kaohe.SelectedValue.ToInt(), currentModule.MID, Ddl_Danwei.SelectedValue);
            }
            Utility.ShowMsg(Page, "系统提示", "得分审定完成！", 100, "show");
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
            BindGrid();
        }

        #endregion


        #region 专用方法区


        #endregion

    }
}