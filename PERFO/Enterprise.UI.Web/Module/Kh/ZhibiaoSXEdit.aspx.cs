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
    ///指标筛选维护页面
    /// </summary>
    public partial class ZhibiaoSXEdit : PageBase
    {

        /// <summary>
        /// 指标筛选-服务类
        /// </summary>
        KhZbsxService zbsxSrv = new KhZbsxService();
        KhKhglService khglSrv = new KhKhglService();//考核管理
        KhJxzrszbService jxzrsZbSrv = new KhJxzrszbService();//责效责任书指标服务类
        KhJxzrsService zrsSrv = new KhJxzrsService();//责任书服务类
        ZbkLhzbService lhzbSrv = new ZbkLhzbService();//量化指标
        ZbkDfzbService dfzbSrv = new ZbkDfzbService();//打分指标

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected KhKhglModel Kaohe = null;//考核期
        /// <summary>
        /// 百分比求和
        /// </summary>
        decimal perSum = 0;

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
            //1==提取考核MODEL
            if (Kaohe == null)
                Kaohe = khglSrv.GetSingle(Khid);
            var q = zrsSrv.GetModelByNdAndBmjg(Kaohe.KHND, Jgbm.ToRequestString());
            if (q != null)
            {
                //责任书
                if (q.ZRSZT == "0")
                {
                    Lbl_Msg.Text = "该单位的绩效责任书指标还未下达!";
                    SetCntrlVisibility(LnkBtn_Ins, false);
                    return;
                }
                else
                {
                    Lbl_Msg.Text = "注意：请不要忘了钩选最左边的复选框才能有效!";
                }

                //2==先提取指标筛选表中的数据
                List<KhZbsxModel> zbsxList = zbsxSrv.GetListByKaohe(Kaohe.KHID.ToString()) as List<KhZbsxModel>;

                //3==绑定列表
                List<KhJxzrszbModel> zrsZbList = q.JxzrszbLst.ToList();
                GridView1.DataSource = GainKhzbsxList(zbsxList, zrsZbList).OrderBy(p=>p.SXXH).ToList();
                GridView1.DataBind();

                Utility.GroupRows(GridView1, 1);
            }
            else
            {
                Lbl_Msg.Text = "对不起，未查询到该单位的绩效责任书!";
                SetCntrlVisibility(LnkBtn_Ins, false);
                return;
            }
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            Kaohe = khglSrv.GetSingle(Khid);
            if (Kaohe != null)
            {
                //考核名称
                Lbl_Kaohe.Text = Kaohe.KHMC;
                //考核年度
                Lbl_Niandu.Text = Kaohe.KHND;
            }
            //单位
            Lbl_Danwei.Text = SysBmjgService.GetBmjgName(Jgbm);
        }

        /// <summary>
        /// 合成一个新的集合
        /// </summary>
        /// <param name="zbsxList">指标筛选</param>
        /// <param name="mainZbList">责任书指标</param>
        /// <returns></returns>
        protected List<KhZbsxModel> GainKhzbsxList(List<KhZbsxModel> zbsxList, List<KhJxzrszbModel> mainZbList)
        {
            List<KhZbsxModel> khzbsxLst = new List<KhZbsxModel>();
            //绩效责任书指标
            foreach (var zrsZb in mainZbList)
            {
                var zb = zbsxList.Find(p => p.ZRSZBID == zrsZb.ZRSZBID);
                if (zb != null)
                {
                    khzbsxLst.Add(zb);
                }
                else
                {
                    //生成一个新指标
                    KhZbsxModel zbsxM = new KhZbsxModel();
                    zbsxM.SXID = "SXZB" + CommonTool.GetPkId();
                    zbsxM.Kaohe = Kaohe;
                    zbsxM.JxzrsZb = zrsZb;
                    zbsxM.ZRSZBID = zrsZb.ZRSZBID;
                    zbsxM.KHID = Khid.ToInt();
                    zbsxM.SXZBBM = (zrsZb.LHZBBM != null) ? zrsZb.LHZBBM : zrsZb.DFZBBM;
                    zbsxM.SXQZ=zrsZb.ZZBQZ;
                    zbsxM.SXFZ=zrsZb.ZZBFZ;
                    zbsxM.SXJGBM = Jgbm;
                    zbsxM.SXXH = zrsZb.ZXSXH;
                    khzbsxLst.Add(zbsxM);
                }
            }

            return khzbsxLst;
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
                KhZbsxModel model = e.Row.DataItem as KhZbsxModel;
                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //选择 0 
                CheckBox chk = e.Row.FindControl("CheckBox1") as CheckBox;
                chk.ToolTip = model.JxzrsZb.ZSJZB.ToRequestString();
                chk.Enabled = (string.IsNullOrEmpty(model.JxzrsZb.ZSJZB));
                chk.Checked = chk.Enabled;
                if (model.JxzrsZb.Lhzb != null) 
                {
                    //指标类别 1
                    e.Row.Cells[1].Text = model.JxzrsZb.Lhzb.Zbxx.YJZBMC;
                    //指标名称 2
                    e.Row.Cells[2].Text = model.JxzrsZb.Lhzb.Zbxx.ZBMC;
                    if (model.JxzrsZb.ZZBXZ == "辅助指标")
                    {
                        e.Row.Cells[2].Text = model.JxzrsZb.Lhzb.GradeSymbol + e.Row.Cells[2].Text;
                    }
                    //指标性质 3
                    e.Row.Cells[3].Text = model.JxzrsZb.ZZBXZ;
                    if (model.JxzrsZb.ZZBXZ == "主指标")
                    {
                        //权重 4
                        e.Row.Cells[4].Text = Utility.GetTextBox("Txt" + 4 + "_" + (e.Row.RowIndex + 1),
                            (model.SXQZ * 100), 4, (e.Row.RowIndex + 1), "number", true,
                            "class=\"easyui-numberbox\" min=\"0\" max=\"100\" precision=\"1\" onblur=\"Heji(this);\"", "width:55px;");
                        perSum += model.SXQZ.ToDecimal();//累计权重
                    }
                    else
                    {
                        e.Row.Cells[4].Text = Utility.GetTextBox("SubTxt" + 4 + "_" + (e.Row.RowIndex + 1),
                            (model.SXQZ * 100), 4, (e.Row.RowIndex + 1), "number", true,
                            "class=\"easyui-numberbox\" min=\"0\" max=\"100\" precision=\"1\" onblur=\"Heji(this);\"", "width:55px;background-color:#eeeeee;");
                    }
                    //计算关系式 6
                    e.Row.Cells[6].Text = model.JxzrsZb.ZJSGXS;
                    //指标代号 7
                    e.Row.Cells[7].Text = model.JxzrsZb.ZZBDH;
                    ////基准分值 9
                    //e.Row.Cells[9].Text = Utility.GetTextBox("Txt" + 9 + "_" + (e.Row.RowIndex + 1),
                    //    ((model.SXJZFZ != null) ? model.SXJZFZ : model.JxzrsZb.Lhzb.JZFS), 9, (e.Row.RowIndex + 1), "number", true,
                    //        "class=\"easyui-numberbox\" min=\"0\" max=\"999\" precision=\"1\"", "width:55px;");
                }
                else if (model.JxzrsZb.Dfzb != null)
                {
                    //指标类别 1
                    e.Row.Cells[1].Text = model.JxzrsZb.Dfzb.Zbxx.YJZBMC;
                    //指标名称 2
                    e.Row.Cells[2].Text = model.JxzrsZb.Dfzb.Zbxx.ZBMC;
                    //分值 5
                    e.Row.Cells[5].Text = Utility.GetTextBox("Txt" + 5 + "_" + (e.Row.RowIndex + 1),
                        model.SXFZ.ToRequestString(), 5, (e.Row.RowIndex + 1), "number", true, "", "width:55px;");
                }
                //显示序号 8 model.SXXH.ToRequestString()
                int xh = model.SXXH.ToInt();
                e.Row.Cells[8].Text = Utility.GetTextBox("Txt" + 8 + "_" + (e.Row.RowIndex + 1),
                    ((xh > 0) ? xh.ToString() : model.JxzrsZb.ZXSXH.ToRequestString()), 8, (e.Row.RowIndex + 1), 
                    "number", true, "class=\"easyui-numberbox\" min=\"1\" max=\"999\"", "width:55px;");

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = "主指标权重合计：";
                e.Row.Cells[2].ColumnSpan = 2;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Text = "<div id='QzhjDiv'><font color='Red'>" + Convert.ToString(perSum * 100) + "%</font></div>";
                e.Row.Cells[9].Visible = false;
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
            List<string> mainZbbms = new List<string>();
            if (Kaohe == null)
                Kaohe = khglSrv.GetSingle(Khid);
            List<KhJxzrszbModel> zrsZbList = jxzrsZbSrv.GetValidListBySearch(Kaohe.KHND, Jgbm.ToString()).ToList();
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                //检测是否有效
                CheckBox chkbox = gvr.FindControl("CheckBox1") as CheckBox;
                if (chkbox.Checked)
                {
                    //主指标或打分指标
                    //SXID,SXZBBM,ZRSZBID
                    string SXID = GridView1.DataKeys[gvr.RowIndex].Values["SXID"].ToRequestString();
                    string SXZBBM = GridView1.DataKeys[gvr.RowIndex].Values["SXZBBM"].ToRequestString();
                    string ZRSZBID = GridView1.DataKeys[gvr.RowIndex].Values["ZRSZBID"].ToRequestString();
                    KhZbsxModel model = new KhZbsxModel();
                    model.DB_Option_Action = WebKeys.InsertOrUpdateAction;
                    model.KHID = Khid.ToInt();
                    model.SXID = SXID;
                    model.SXZBBM = SXZBBM;
                    model.ZRSZBID = ZRSZBID;
                    model.SXJGBM = Jgbm;
                    //权重 4
                    key = "Txt" + 4 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.SXQZ = Request.Form[key].ToDecimal() / 100;
                    }
                    //分值 5
                    key = "Txt" + 5 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.SXFZ = Request.Form[key].ToDecimal();
                    }
                    //序号 8
                    key = "Txt" + 8 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.SXXH = Request.Form[key].ToInt();
                    }
                    ////基准分值 9
                    //key = "Txt" + 9 + "_" + (gvr.RowIndex + 1);
                    //if (!string.IsNullOrEmpty(Request.Form[key]))
                    //{
                    //    model.SXJZFZ = Request.Form[key].ToDecimal();
                    //}
                    if (zbsxSrv.Execute(model))
                    {
                        mainZbbms.Add(SXZBBM);//保存主指标
                        mainZbbms.AddRange(getSXZBBMs(SXZBBM, zrsZbList));//该指标下的所有子指标
                    }
                }
                else if (!string.IsNullOrEmpty(chkbox.ToolTip))
                {
                    //辅助指标,直接随主指标保存------------------
                    if (mainZbbms.Exists(p => p == chkbox.ToolTip))
                    {
                        //SXID,SXZBBM,ZRSZBID
                        string SXID = GridView1.DataKeys[gvr.RowIndex].Values["SXID"].ToRequestString();
                        string SXZBBM = GridView1.DataKeys[gvr.RowIndex].Values["SXZBBM"].ToRequestString();
                        string ZRSZBID = GridView1.DataKeys[gvr.RowIndex].Values["ZRSZBID"].ToRequestString();
                        KhZbsxModel model = new KhZbsxModel();
                        model.DB_Option_Action = WebKeys.InsertOrUpdateAction;
                        model.KHID = Khid.ToInt();
                        model.SXID = SXID;
                        model.SXZBBM = SXZBBM;
                        model.ZRSZBID = ZRSZBID;
                        model.SXJGBM = Jgbm;
                        //权重 4
                        key = "SubTxt" + 4 + "_" + (gvr.RowIndex + 1);
                        if (!string.IsNullOrEmpty(Request.Form[key]))
                        {
                            model.SXQZ = Request.Form[key].ToDecimal() / 100;
                        }
                        //分值 5
                        key = "Txt" + 5 + "_" + (gvr.RowIndex + 1);
                        if (!string.IsNullOrEmpty(Request.Form[key]))
                        {
                            model.SXFZ = Request.Form[key].ToDecimal();
                        }
                        //序号 8
                        key = "Txt" + 8 + "_" + (gvr.RowIndex + 1);
                        if (!string.IsNullOrEmpty(Request.Form[key]))
                        {
                            model.SXXH = Request.Form[key].ToInt();
                        }
                        ////基准分值 9
                        //key = "Txt" + 9 + "_" + (gvr.RowIndex + 1);
                        //if (!string.IsNullOrEmpty(Request.Form[key]))
                        //{
                        //    model.SXJZFZ = Request.Form[key].ToDecimal();
                        //}
                        zbsxSrv.Execute(model);//保存辅助指标
                    }
                }                    
                            
            }
            BindGrid();
            Utility.ShowMsg(Page, "提示", "指标筛选成功！", 100, "show");
        }


        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            string url = string.Format("KH={0}&BM={1}", Khid, Jgbm);
            Response.Redirect("~/Module/Kh/ZhibiaoSXList.aspx?" + url, true);
        }

        #endregion

        #region 专用方法区

        /// <summary>
        /// 获取指定量化指标下的所有子指标
        /// </summary>
        /// <param name="lhzbbm"></param>
        /// <param name="zrsZbList"></param>
        /// <returns></returns>
        private List<string> getSXZBBMs(string lhzbbm, List<KhJxzrszbModel> zrsZbList)
        {
            List<string> zbbms = new List<string>();
            var lst = zrsZbList.Where(p => p.ZSJZB == lhzbbm);
            foreach(var zb in lst) {
                if (!zbbms.Exists(p => p == zb.LHZBBM))
                {
                    zbbms.Add(zb.LHZBBM);
                    zbbms.AddRange(getSXZBBMs(zb.LHZBBM,zrsZbList));
                }
            }
            return zbbms;
        }

        #endregion

    }
}