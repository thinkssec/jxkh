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
    ///机关部门考核同级打分页面
    /// </summary>
    public partial class JgbmWczTongji : PageBase
    {

        /// <summary>
        /// 定量指标明细-服务类
        /// </summary>
        KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
        /// <summary>
        /// 打分指标明细-服务类
        /// </summary>
        KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();
        KhJgbmdfbService jgbmdfSrv = new KhJgbmdfbService();//机关部门打分表
        KhKhglService khglSrv = new KhKhglService();//考核管理

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected List<KhDlzbmxModel> DlzbmxList = null;//定量指标明细集合
        protected decimal HjDefen = 0M;//合计得分
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
            //权限检测
            if (!ChkUserAuditPermission(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue.ToInt(), userModel))
            {
                Lbl_Msg.Text = "您的权限目前为【查看】!";
                Pnl_Edit.Visible = false;
                Btn_Add.Visible = false;
            }
            else
            {
                Lbl_Msg.Text = "提示：点击考核目标列可以查看评分标准!";
            }

            //1==考核指标
            DlzbmxList = dlzbmxSrv.GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue)
                .Where(p => (p.KhJgbmdfbLst.FirstOrDefault(c => c.DFZ == userModel.JGBM.ToString()
                        || c.DFZ == userModel.LOGINID) != null) && (p.ID ==
                    p.KhJgbmdfbLst.FirstOrDefault(c => c.DFZ == userModel.JGBM.ToString()
                        || c.DFZ == userModel.LOGINID).ID)).ToList();
            if (DlzbmxList.Count > 0)
            {
                if (DlzbmxList.Count(p => p.WCZSDRQ == null) == 0)
                {
                    Lbl_Msg.Text = "该单位的考核结果已审定通过!不能再修改了!";
                    Pnl_Edit.Visible = false;
                    Btn_Add.Visible = false;
                }
                else if (DlzbmxList.Count(p => p.WCZSQRQ == null) > 0)
                {
                    Lbl_Msg.Text = "该单位的自评打分还未完成!您现在还不能打分!";
                    Pnl_Edit.Visible = false;
                    Btn_Add.Visible = false;
                }
            }
            //2==打分指标
            List<KhDfzbmxModel> dfzbmxList = dfzbmxSrv.GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue)
                .Where(p => p.DfzbModel.Zbxx.YJZBMC.Contains("机关作风建设")).ToList();
            //合成数据集
            List<PerfoSuperModel> dataList = new List<PerfoSuperModel>();
            dataList.AddRange(DlzbmxList);
            dataList.AddRange(dfzbmxList);
            GridView1.DataSource = dataList;
            GridView1.DataBind();
            Utility.GroupRows(GridView1, 1);

            //2==提取当前用户的所有测评单位
            List<KhJgbmdfbModel> myDfDanWeiLst =
                jgbmdfSrv.GetListByKhidAndDfz(Ddl_Kaohe.SelectedValue, userModel) as List<KhJgbmdfbModel>;
            var oneBmgjList = myDfDanWeiLst.Where(p => p.JGBM != userModel.JGBM).DistinctBy(p => p.JGBM).ToList();
            GridView2.DataSource = oneBmgjList;
            GridView2.DataBind();
            
            //3==友好提示
            if (oneBmgjList.Count > 0)
            {
                Lbl_Msg.Text += "&nbsp;&nbsp;友情提示：您还需评测的单位数量为〖" + oneBmgjList.Count(p => p.KHDF == null) + "〗";
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
            List<SysBmjgModel> bmjgTreeList = bmjgService.GetBmjgTreeLst(false).
                Where(p => p.XSXH.Length > 2 && !p.JGLX.Contains("二级")).ToList();
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
            var kaohe = khglSrv.GetKhListForValid().Where(p => p.LXID == "LX2014B").ToList();
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
                KhJgbmdfbModel model = e.Row.DataItem as KhJgbmdfbModel;
                //单位名称 1
                string url = string.Format("KH={0}&BM={1}", model.KHID, model.JGBM);
                string bmmc = SysBmjgService.GetBmjgName(model.JGBM.Value);
                if (userModel.JGBM == model.JGBM.Value) 
                    bmmc += "(自评)";
                e.Row.Cells[1].Text = string.Format("<a href='{0}'>{1}</a>",
                    GetPageUrlAndParams("?" + url).TrimStart("~".ToCharArray()), bmmc);
                //测评状态 2
                e.Row.Cells[2].Text = (model.KHDF != null) ? "已测评" : "<font color='red'>未测评</font>";
                //测评日期 3
                e.Row.Cells[3].Text = model.DFSJ.ToDateYMDFormat();
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
                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

                //操作权限 9
                Image img = e.Row.FindControl("Image1") as Image;
                img.ImageUrl = "/Resources/Images/no.png";//无权限

                PerfoSuperModel supperM = e.Row.DataItem as PerfoSuperModel;
                if (supperM.GetType().IsAssignableFrom(typeof(KhDlzbmxModel)))
                {
                    //定量指标
                    KhDlzbmxModel model = supperM as KhDlzbmxModel;

                    //序号
                    if (model.ZBBM != zblx)
                    {
                        zbRowIndex = 1;
                        zblx = model.ZBBM;
                    }
                    e.Row.Cells[0].Text = (zbRowIndex++).ToString();

                    //8 ID
                    e.Row.Cells[8].Text = model.ID;

                    //指标类别 1
                    e.Row.Cells[1].Text = model.LhzbModel.Zbxx.ZBMC;//以名称为类别
                    //其考核权重直接引用指标筛选时的权重
                    e.Row.Cells[1].Text += "<br/>〖" + Convert.ToDecimal(model.ZbsxModel.SXQZ * 100).ToString("f1") + "%〗";
                    //考核主要内容 2
                    e.Row.Cells[2].Text = "<div style='width: 150px;padding: 2px;overflow:auto;'>"
                        + model.ZbsxModel.JxzrsZb.JGKHNR + "</div>";
                    //分值3
                    e.Row.Cells[3].Text = model.ZbsxModel.SXFZ.ToRequestString();
                    //考核目标4
                    e.Row.Cells[4].Text =
                        "<div style='width: 200px;padding: 2px;overflow:auto;cursor:hand;text-decoration:underline;' onclick=\"showInfo('评分标准','"
                        + model.ZbsxModel.JxzrsZb.JGWCSJ + "<hr/>" + model.ZbsxModel.JxzrsZb.JGPFBZ + "');\">"
                        + model.ZbsxModel.JxzrsZb.JGKHMB + "</div>";
                    //完成率5
                    e.Row.Cells[5].Text = (model.WCZ != null) ? model.WCZ.Value.ToString("f1") + "%" : "";

                    var zpDfModel = model.KhJgbmdfbLst.FirstOrDefault(p => p.DFZ == model.JGBM.ToRequestString());
                    var tjcpDfmodel = model.KhJgbmdfbLst.FirstOrDefault(p => p.DFZ == userModel.JGBM.ToRequestString());
                    if (tjcpDfmodel != null)
                    {
                        if (Pnl_Edit.Visible == true &&
                        tjcpDfmodel.JGBM.ToRequestString() != tjcpDfmodel.DFZ)
                        {
                            img.ImageUrl = "/Resources/Images/yes.png";//有权限
                            //完成率5
                            string txtId5 = "Txt" + 5 + "_" + (e.Row.RowIndex + 1);
                            string txtId6 = "Txt" + 6 + "_" + (e.Row.RowIndex + 1);
                            e.Row.Cells[5].Text =
                                "<input class=\"easyui-slider\" style=\"width:70px;padding: 4px;\" data-options=\"showTip:true,value:'"
                                + tjcpDfmodel.WCJD.ToInt() + "',tipFormatter: function(value){return value+'%';},onChange: function(value){var standVal='" + model.ZbsxModel.SXFZ.ToDecimal() + "';$('#" + txtId5 + "').val(value);$('#" + txtId6 + "').numberbox('setValue', Math.round(value/10*standVal)/10);}\"/>"
                                + "<input type='hidden' id='" + txtId5 + "' name='" + txtId5 + "' value='" + tjcpDfmodel.WCJD + "'/>";
                            //测评得分6
                            e.Row.Cells[6].Text = Utility.GetTextBox(txtId6, tjcpDfmodel.KHDF, 6, (e.Row.RowIndex + 1), "number", true,
                                        "class=\"easyui-numberbox\" min=\"0\" max=\""
                                        + ((model.ZbsxModel.SXFZ.ToDecimal() > 0) ? model.ZbsxModel.SXFZ.ToDecimal() : 100.0M)
                                        + "\" precision=\"2\"", "width:80px;text-align: right;");
                            //测评说明7
                            e.Row.Cells[7].Text = Utility.GetTextBox("Txt" + 7 + "_" + (e.Row.RowIndex + 1),
                                tjcpDfmodel.DFSM, 7, (e.Row.RowIndex + 1), "string", false, "", "width:220px;");
                        }
                        else
                        {
                            if (tjcpDfmodel.JGBM.ToRequestString() != tjcpDfmodel.DFZ)
                                img.ImageUrl = "/Resources/Images/yes.png";//有权限
                            //完成率 5
                            e.Row.Cells[5].Text = (tjcpDfmodel.WCJD != null) ? tjcpDfmodel.WCJD.Value + "%" : "";
                            //测评得分6
                            e.Row.Cells[6].Text = tjcpDfmodel.KHDF.ToRequestString();
                            //测评说明7
                            e.Row.Cells[7].Text = tjcpDfmodel.DFSM;
                            e.Row.Cells[7].ToolTip = "打分日期：" + tjcpDfmodel.DFSJ.ToDateYMDFormat();
                        }

                        HjDefen += tjcpDfmodel.KHDF.ToDecimal();

                        //显示自评打分的情况
                        if (zpDfModel != null)
                        {
                            e.Row.Cells[5].ToolTip = "本项自评完成率：" + zpDfModel.WCJD + "%,自评打分：" + zpDfModel.KHDF;
                        }
                    }
                }
                else if (supperM.GetType().IsAssignableFrom(typeof(KhDfzbmxModel)))
                {
                    //打分指标
                    KhDfzbmxModel dfModel = supperM as KhDfzbmxModel;

                    //序号
                    if (dfModel.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC != zblx)
                    {
                        zbRowIndex = 1;
                        zblx = dfModel.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC;
                    }
                    e.Row.Cells[0].Text = (zbRowIndex++).ToString();

                    //8
                    e.Row.Cells[8].Text = dfModel.DFZBID;
                    //指标类别 1
                    e.Row.Cells[1].Text = dfModel.DfzbModel.Zbxx.YJZBMC;
                    //指标名称 2
                    e.Row.Cells[2].Text = "<a href=\"javascript:showInfo('指标说明','" +
                        dfModel.DfzbModel.PFLX + "<hr/>" + dfModel.DfzbModel.PFBZ + "','info');\">" + dfModel.DfzbModel.Zbxx.ZBMC + "</a>";

                    e.Row.Cells[3].ColumnSpan = 3;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;

                    var tjDfmodel = dfModel.KhJgbmdfbLst.FirstOrDefault(p => p.DFZ == userModel.JGBM.ToRequestString());
                    if (tjDfmodel != null)
                    {
                        if (Pnl_Edit.Visible == true &&
                            tjDfmodel.JGBM.ToRequestString() != tjDfmodel.DFZ)
                        {
                            img.ImageUrl = "/Resources/Images/yes.png";//有权限
                            //测评得分6
                            e.Row.Cells[6].Text = getHiddenInputAndStarRaty(6 + "_" + (e.Row.RowIndex + 1), tjDfmodel.KHDF.ToDecimal());
                            //测评说明7
                            e.Row.Cells[7].Text = Utility.GetTextBox("Txt" + 7 + "_" + (e.Row.RowIndex + 1),
                                tjDfmodel.DFSM, 7, (e.Row.RowIndex + 1), "string", false, "", "width:220px;");
                        }
                        else
                        {
                            if (tjDfmodel.JGBM.ToRequestString() != tjDfmodel.DFZ) 
                                img.ImageUrl = "/Resources/Images/yes.png";//有权限
                            //测评得分6
                            e.Row.Cells[6].Text = tjDfmodel.KHDF.ToRequestString();
                            //测评说明7
                            e.Row.Cells[7].Text = tjDfmodel.DFSM;
                            e.Row.Cells[7].ToolTip = "打分日期：" + tjDfmodel.DFSJ.ToDateYMDFormat();
                        }
                    }
                }
                
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].ColumnSpan = 6;
                e.Row.Cells[0].Text = "∑合计得分=";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Text = string.Format("<font color='red'>{0}</font>", HjDefen);
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
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

        }

        /// <summary>
        /// 正式提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            bool isOk = true;
            string key = string.Empty;
            List<KhDlzbmxModel> list = dlzbmxSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                string ID = gvr.Cells[8].Text;
                if (ID.StartsWith("LH"))
                {
                    //定量指标
                    KhDlzbmxModel model = list.FirstOrDefault(p=>p.ID == ID);//dlzbmxSrv.GetSingle(Ddl_Kaohe.SelectedValue, ID);
                    if (model == null || model.KhJgbmdfbLst == null)
                        continue;
                    //由于打分是多人操作，只有审定时才能计算出总得分，这里只记录当前用户的打分成绩
                    var dfzModel = model.KhJgbmdfbLst.FirstOrDefault(p => p.DFZ == userModel.JGBM.ToRequestString());
                    if (dfzModel != null)
                    {
                        dfzModel.DB_Option_Action = WebKeys.UpdateAction;
                        //完成率5
                        key = "Txt" + 5 + "_" + (gvr.RowIndex + 1);
                        if (!string.IsNullOrEmpty(Request.Form[key]))
                        {
                            dfzModel.WCJD = Request.Form[key].ToInt();
                        }
                        //测评得分6
                        key = "Txt" + 6 + "_" + (gvr.RowIndex + 1);
                        if (!string.IsNullOrEmpty(Request.Form[key]))
                        {
                            dfzModel.KHDF = Request.Form[key].ToDecimal();
                        }
                        //测评说明7
                        key = "Txt" + 7 + "_" + (gvr.RowIndex + 1);
                        if (!string.IsNullOrEmpty(Request.Form[key]))
                        {
                            dfzModel.DFSM = Request.Form[key];
                        }
                        dfzModel.DFSJ = DateTime.Now;
                        //保存该打分
                        if (!jgbmdfSrv.Execute(dfzModel))
                        {
                            isOk = false;
                        }                        
                    }
                }
                else if (ID.StartsWith("DF")) 
                {
                    //打分指标
                    KhDfzbmxModel model = dfzbmxSrv.GetSingle(Ddl_Kaohe.SelectedValue, ID);
                    if (model == null || model.KhJgbmdfbLst == null)
                        continue;
                    //由于打分是多人操作，只有审定时才能计算出总得分，这里只记录当前用户的打分成绩
                    var dfzModel = model.KhJgbmdfbLst.FirstOrDefault(p => p.DFZ == userModel.JGBM.ToRequestString());
                    if (dfzModel != null)
                    {
                        dfzModel.DB_Option_Action = WebKeys.UpdateAction;
                        //测评得分6
                        key = "Txt" + 6 + "_" + (gvr.RowIndex + 1);
                        if (!string.IsNullOrEmpty(Request.Form[key]))
                        {
                            dfzModel.KHDF = Request.Form[key].ToDecimal();
                        }
                        //测评说明7
                        key = "Txt" + 7 + "_" + (gvr.RowIndex + 1);
                        if (!string.IsNullOrEmpty(Request.Form[key]))
                        {
                            dfzModel.DFSM = Request.Form[key];
                        }
                        dfzModel.DFSJ = DateTime.Now;
                        //保存该打分
                        if (!jgbmdfSrv.Execute(dfzModel))
                        {
                            isOk = false;
                        }
                    }
                }                
            }
            if (isOk)
            {
                //关闭消息
                if (!string.IsNullOrEmpty(MSGID))
                {
                    msgService.CloseMessage(MSGID);
                }
                else
                {
                    msgService.CloseMessage(userModel, Ddl_Kaohe.SelectedValue.ToInt(), currentModule.MID, Ddl_Danwei.SelectedValue);
                }
                Utility.ShowMsg(Page, "系统提示", "测评打分保存成功!", 100, "show");
            }
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

        /// <summary>
        /// 生成隐藏域控件
        /// </summary>
        /// <param name="inputName">名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        private string getHiddenInputAndStarRaty(string inputName, decimal val)
        {
            int score = 0;
            if (val == 0M) val = 1;//缺省为良好
            string htmlStr = "<input name='Txt" + inputName + "' value='" + val + "' type='hidden' runat='server' />";
            if (val == 1.2M) score = 4;
            else if (val == 1.0M) score = 3;
            else if (val == 0.8M) score = 2;
            else if (val == 0.6M) score = 1;
            htmlStr += "<div id='Star" + inputName + "' class=\"raty\" data-score='" + score + "' style='width:90px;'></div>";
            return htmlStr;
        }

        #endregion

    }
}