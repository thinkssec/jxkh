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
    ///机关部门考核自评页面
    /// </summary>
    public partial class JgbmWczZiping : PageBase
    {

        /// <summary>
        /// 定量指标明细-服务类
        /// </summary>
        KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
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
            if (userModel.JGBM != Ddl_Danwei.SelectedValue.ToInt() && userModel.ROLEID != "paadmin")
            {
                Lbl_Msg.Text = "对不起,该功能为单位自评使用!您的权限目前为【查看】!";
                Pnl_Edit.Visible = false;
                Btn_Add.Visible = false;
            }
            else
            {
                Lbl_Msg.Text = "提示：点击考核目标列可以查看评分标准!";
            }

            //1==考核指标
            DlzbmxList = dlzbmxSrv.GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            GridView1.DataSource = DlzbmxList;
            GridView1.DataBind();
            Utility.GroupRows(GridView1, 1);

            if (DlzbmxList.Count > 0)
            {
                if (DlzbmxList.Count(p => p.WCZSDRQ == null) == 0)
                {
                    Lbl_Msg.Text = "打分值已审定通过!不能再修改了!";
                    Pnl_Edit.Visible = false;
                    Btn_Add.Visible = false;
                }
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
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhDlzbmxModel model = e.Row.DataItem as KhDlzbmxModel;

                //序号
                if (model.ZBBM != zblx)
                {
                    zbRowIndex = 1;
                    zblx = model.ZBBM;
                }
                e.Row.Cells[0].Text = (zbRowIndex++).ToString();

                //指标类别 1
                e.Row.Cells[1].Text = model.LhzbModel.Zbxx.ZBMC;//以名称为类别
                //其考核权重直接引用指标筛选时的权重
                e.Row.Cells[1].Text += "<br/>〖" + Convert.ToDecimal(model.ZbsxModel.SXQZ * 100).ToString("f1") + "%〗";
                //考核主要内容 2
                e.Row.Cells[2].Text = "<div style='width: 180px;padding: 2px;overflow:auto;'>" 
                    + model.ZbsxModel.JxzrsZb.JGKHNR + "</div>";
                //分值3
                e.Row.Cells[3].Text = model.ZbsxModel.SXFZ.ToRequestString();
                //考核目标4
                e.Row.Cells[4].Text =
                    "<div style='width: 200px;padding: 2px;overflow:auto;cursor:hand;text-decoration:underline;' onclick=\"showInfo('评分标准','" 
                    + model.ZbsxModel.JxzrsZb.JGWCSJ + "<hr/>" + model.ZbsxModel.JxzrsZb.JGPFBZ + "');\">" 
                    + model.ZbsxModel.JxzrsZb.JGKHMB + "</div>";

                var zpDfmodel = model.KhJgbmdfbLst.FirstOrDefault(p => p.DFZ == model.JGBM.ToRequestString());
                if (zpDfmodel != null)
                {
                    if (Pnl_Edit.Visible == true)
                    {
                        //本单位可以编辑---------------------------------------
                        //完成率5
                        string txtId5 = "Txt" + 5 + "_" + (e.Row.RowIndex + 1);
                        string txtId6 = "Txt" + 6 + "_" + (e.Row.RowIndex + 1);
                        e.Row.Cells[5].Text =
                            "<input class=\"easyui-slider\" style=\"width:70px;padding: 4px;\" data-options=\"showTip:true,value:'"
                            + zpDfmodel.WCJD.ToInt() + "',tipFormatter: function(value){return value+'%';},onChange: function(value){var standVal='" + model.ZbsxModel.SXFZ.ToDecimal() + "';$('#" + txtId5 + "').val(value);$('#" + txtId6 + "').numberbox('setValue', Math.round(value/10*standVal)/10);}\"/>"
                            + "<input type='hidden' id='" + txtId5 + "' name='" + txtId5 + "' value='" + zpDfmodel.WCJD + "'/>";
                            //Utility.GetTextBox("Txt" + 5 + "_" + (e.Row.RowIndex + 1),
                            //        (zpDfmodel.WCJD), 5, (e.Row.RowIndex + 1), "number", true,
                            //        "class=\"easyui-numberbox\" min=\"0\" max=\"100\" precision=\"0\"", "width:80px;");
                        //自评得分6
                        e.Row.Cells[6].Text = Utility.GetTextBox(txtId6, zpDfmodel.KHDF, 6, (e.Row.RowIndex + 1), "number", true,
                                    "class=\"easyui-numberbox\" min=\"0\" max=\""
                                    + ((model.ZbsxModel.SXFZ.ToDecimal() > 0) ? model.ZbsxModel.SXFZ.ToDecimal() : 100.0M)
                                    + "\" precision=\"2\"", "width:80px;text-align: right;");
                        //自评说明7
                        e.Row.Cells[7].Text = Utility.GetTextBox("Txt" + 7 + "_" + (e.Row.RowIndex + 1), 
                            zpDfmodel.DFSM, 7, (e.Row.RowIndex + 1), "string", false, "", "width:220px;");
                    }
                    else
                    {
                        //完成率5
                        e.Row.Cells[5].Text = zpDfmodel.WCJD.ToRequestString() + ((zpDfmodel.WCJD != null) ? "%" : "");
                        //自评得分6
                        e.Row.Cells[6].Text = zpDfmodel.KHDF.ToRequestString();
                        //自评说明7
                        e.Row.Cells[7].Text = zpDfmodel.DFSM;
                        e.Row.Cells[7].ToolTip = "打分日期：" + zpDfmodel.DFSJ.ToDateYMDFormat();
                    }

                    HjDefen += zpDfmodel.KHDF.ToDecimal();
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
            List<KhDlzbmxModel> list = dlzbmxSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            string key = string.Empty;
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                //ID
                string ID = GridView1.DataKeys[gvr.RowIndex].Values["ID"].ToRequestString();
                KhDlzbmxModel model = list.FirstOrDefault(p => p.ID == ID);// dlzbmxSrv.GetSingle(Ddl_Kaohe.SelectedValue, ID);
                if (model == null || model.KhJgbmdfbLst == null)
                    continue;
                model.DB_Option_Action = WebKeys.UpdateAction;
                //由于打分是多人操作，只有审定时才能计算出总得分，这里只记录当前用户的打分成绩
                var dfzModel = model.KhJgbmdfbLst.FirstOrDefault(p => p.DFZ == model.JGBM.ToRequestString());
                if (dfzModel != null)
                {
                    dfzModel.DB_Option_Action = WebKeys.UpdateAction;
                    //完成进度5
                    key = "Txt" + 5 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.WCZSQZ = model.WCZ = dfzModel.WCJD = Request.Form[key].ToInt();
                    }
                    //自评得分6
                    key = "Txt" + 6 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        dfzModel.KHDF = Request.Form[key].ToDecimal();
                    }
                    //自评说明7
                    key = "Txt" + 7 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.WCZSQBZ = model.WCZBZ = dfzModel.DFSM = Request.Form[key];
                    }
                    dfzModel.DFSJ = DateTime.Now;
                    //保存该打分
                    if (jgbmdfSrv.Execute(dfzModel))
                    {
                        model.WCZSQRQ = DateTime.Now;
                        dlzbmxSrv.Execute(model);
                        Utility.ShowMsg(Page, "系统提示", "自评打分保存成功!", 100, "show");
                    }
                }
            }
            //关闭消息
            if (!string.IsNullOrEmpty(MSGID))
            {
                msgService.CloseMessage(MSGID);
            }
            else
            {
                msgService.CloseMessage(userModel, Ddl_Kaohe.SelectedValue.ToInt(), currentModule.MID, Ddl_Danwei.SelectedValue);
            }
            //给审核人发送待办事务---------------------------------------------
            //上级领导打分和同级打分
            var bmdfLst = jgbmdfSrv.GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue)
                .DistinctBy(p => p.DFZ).ToList();
            var userList = userService.GetUserListForValid();
            foreach (var q in bmdfLst)
            {
                if (q.JGBM.ToString() == q.DFZ) continue;
                if (q.DFZLX == ((int)WebKeys.DFUserType.上级领导).ToString())
                {
                    //机关部门考核，上级打分
                    var u = userList.FirstOrDefault(p => p.LOGINID == q.DFZ);
                    if (u != null)
                    {
                        msgService.SendMsgToUers(u.LOGINID, q.JGBM.Value,
                        "机关部门上级测评", "需要您进行【" + Ddl_Kaohe.SelectedItem.Text + "】【" + SysBmjgService.GetBmjgName(q.JGBM.Value) + "】的打分操作!",
                        string.Format("/Module/Kh/JgbmWczShangji.aspx?BM={0}&KH={1}", q.JGBM, q.KHID),
                        "1114", q.JGBM.ToString());
                    }
                }
                else if (q.DFZLX == ((int)WebKeys.DFUserType.同级部门).ToString() ||
                    q.DFZLX == ((int)WebKeys.DFUserType.同级二级单位).ToString())
                {
                    //机关部门考核，同级打分
                    var u = userList.FirstOrDefault(p => p.JGBM.ToString() == q.DFZ && p.DUTY.Contains("员工"));
                    if (u != null)
                    {
                        msgService.SendMsgToUers(u.LOGINID, q.JGBM.Value,
                        "机关部门同级测评", "需要您进行【" + Ddl_Kaohe.SelectedItem.Text + "】【" + SysBmjgService.GetBmjgName(q.JGBM.Value) + "】的打分操作!",
                        string.Format("/Module/Kh/JgbmWczTongji.aspx?BM={0}&KH={1}", q.JGBM, q.KHID),
                        "1115", q.JGBM.ToString());
                    }
                }
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

   
        #endregion

    }
}