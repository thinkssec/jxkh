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
    /// 年度绩效责任书指标维护页面
    /// </summary>
    public partial class NdjxzrsZbEdit : PageBase
    {

        /// <summary>
        /// 责任书指标服务类
        /// </summary>
        KhJxzrszbService jxzrsZbSrv = new KhJxzrszbService();
        KhJxzrsService zrsSrv = new KhJxzrsService();//责任书服务类
        ZbkLhzbService lhzbSrv = new ZbkLhzbService();//量化指标
        ZbkDfzbService dfzbSrv = new ZbkDfzbService();//打分指标

        protected string Bbmc = (string)Utility.sink("BB", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//版本名称
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected string Sznd = (string)Utility.sink("ND", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//所在年度
        protected int ZrsId;//责任书ID
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
                BindGrid(true);
            }
        }

        #region 专用方法区

        /// <summary>
        /// 绑定列表数据
        /// </summary>
        /// <param name="isReload">重新加载数据</param>
        protected void BindGrid(bool isReload)
        {
            //1==提取责任书ID
            var q = zrsSrv.GetListByNdAndBmjg(Sznd, Jgbm.ToRequestString());
            if (q != null && q.Count > 0)
            {
                //责任书名称
                Lbl_Zrsmc.Text = q.First().ZRSMC;
                ZrsId = q.First().ZRSID;
                if (q.First().ZRSZT == "1")
                {
                    Lbl_Msg.Text = "该单位的绩效责任书已下达!不能再修改指标!";
                    SetCntrlVisibility(LnkBtn_Ins, false);
                    return;
                }
                else
                {
                    Lbl_Msg.Text = "注意：请不要忘了钩选最左边的复选框才能有效!";
                }
            }
            else
            {
                Lbl_Msg.Text = "该单位还未允许制定绩效责任书!";
                SetCntrlVisibility(LnkBtn_Ins, false);
                return;
            }

            if (ZrsId > 0)
            {
                List<KhJxzrszbModel> list = null;
                if (isReload)
                {
                    //2==先提取责任书指标表中的数据
                    List<KhJxzrszbModel> zrszbList = jxzrsZbSrv.GetListBySearch(Sznd, Jgbm.ToRequestString(), Bbmc) as List<KhJxzrszbModel>;
                    //3==再从量化指标表和打分指标表中分别提取所需数据
                    List<ZbkLhzbModel> lhzbList = lhzbSrv.GetListForValidOrderByZbxh(Jgbm.ToRequestString(), Bbmc, false) as List<ZbkLhzbModel>;
                    List<ZbkDfzbModel> dfzbList = dfzbSrv.GetListForValid(Jgbm.ToRequestString(), Bbmc, false) as List<ZbkDfzbModel>;
                    //4==绑定
                    list = GainJxzrsZbList(zrszbList, lhzbList, dfzbList).OrderBy(p=>p.ZXSXH).ToList();
                    ViewState["PERFO_NDJXZRSZB"] = list;
                }
                else
                {
                    list = ViewState["PERFO_NDJXZRSZB"] as List<KhJxzrszbModel>;
                }
                GridView1.DataSource = list;
                GridView1.DataBind();
            }
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //年度
            Lbl_Niandu.Text = Sznd;
            //版本
            Lbl_Bbmc.Text = Bbmc;
            //单位
            Lbl_Danwei.Text = SysBmjgService.GetBmjgName(Jgbm);
        }

        /// <summary>
        /// 合成一个新的数据集合
        /// </summary>
        /// <param name="zrszbList">责任书指标集合</param>
        /// <param name="lhzbList">量化指标集合</param>
        /// <param name="dfzbList">打分指标集合</param>
        /// <returns></returns>
        protected List<KhJxzrszbModel> GainJxzrsZbList(List<KhJxzrszbModel> zrszbList, List<ZbkLhzbModel> lhzbList, List<ZbkDfzbModel> dfzbList)
        {
            List<KhJxzrszbModel> zbList = new List<KhJxzrszbModel>();
            int xh = 0;
            //量化指标
            foreach (var lhzb in lhzbList)
            {
                var zb = zrszbList.Find(p => p.LHZBBM == lhzb.LHZBBM);
                if (zb != null)
                {
                    if (zb.ZXSXH.ToInt() > xh) 
                        xh = zb.ZXSXH.Value;

                    zbList.Add(zb);
                }
                else
                {
                    //生成一个新指标（量化）
                    KhJxzrszbModel zbM = new KhJxzrszbModel();
                    zbM.ZRSZBID = "ZRSZB" + CommonTool.GetPkId();
                    zbM.Lhzb = lhzb;
                    zbM.LHZBBM = lhzb.LHZBBM;
                    zbM.ZZBQZ = lhzb.BJQZ;
                    zbM.ZZBDH = lhzb.ZBDH;
                    zbM.ZZBXZ = (lhzb.FZZB == "0") ? "主指标" : "辅助指标";
                    zbM.ZJSGXS = getZJSGXS(lhzb, lhzbList);//自动合成所有子指标
                    zbM.ZSJZB = lhzb.PARENTZBBM;
                    zbM.ZJGBM = Jgbm;
                    zbM.BBMC = Bbmc;
                    zbM.ZSZND = Sznd.ToInt();
                    zbM.ZXSXH = (++xh);
                    zbM.ZRSID = ZrsId;
                    zbList.Add(zbM);
                }
            }
            //打分指标
            foreach (var dfzb in dfzbList)
            {
                var zb = zrszbList.Find(p => p.DFZBBM == dfzb.DFZBBM);
                if (zb != null)
                {
                    if (zb.ZXSXH.ToInt() > xh)
                        xh = zb.ZXSXH.Value;

                    zbList.Add(zb);
                }
                else
                {
                    //生成一个新指标（打分）
                    KhJxzrszbModel zbM = new KhJxzrszbModel();
                    zbM.ZRSZBID = "ZRSZB" + CommonTool.GetPkId();
                    zbM.Dfzb = dfzb;
                    zbM.DFZBBM = dfzb.DFZBBM;
                    zbM.ZZBFZ = dfzb.JXFZ;
                    zbM.ZJSGXS = dfzb.DFBDS;
                    zbM.ZJGBM = Jgbm;
                    zbM.BBMC = Bbmc;
                    zbM.ZSZND = Sznd.ToInt();
                    zbM.ZXSXH = (++xh);
                    zbM.ZRSID = ZrsId;
                    zbList.Add(zbM);
                }
            }
            return zbList;
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
                KhJxzrszbModel model = e.Row.DataItem as KhJxzrszbModel;
                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

                //选择 0 
                CheckBox chk = e.Row.FindControl("CheckBox1") as CheckBox;
                chk.Checked = (model.ZTJRQ != null);
                //指标性质 3
                e.Row.Cells[3].Text = model.ZZBXZ;

                if (model.Lhzb != null)
                {
                    //指标类别 1 指标名称 2
                    e.Row.Cells[1].Text = model.Lhzb.Zbxx.YJZBMC;
                    e.Row.Cells[2].Text = model.Lhzb.Zbxx.ZBMC +
                        ((!string.IsNullOrEmpty(model.Lhzb.JSDW)) ? "(" + model.Lhzb.JSDW + ")" : "");
                    if (model.ZZBXZ == "辅助指标")
                    {
                        e.Row.Cells[2].Text = model.Lhzb.GradeSymbol + e.Row.Cells[2].Text;
                    }
                    //权重 4
                    string bgColor = "";
                    if (model.ZZBXZ == "主指标")
                    {
                        e.Row.Cells[4].Text = Utility.GetTextBox("Txt" + 4 + "_" + (e.Row.RowIndex + 1),
                        (model.ZZBQZ * 100), 4, (e.Row.RowIndex + 1), "number", true,
                        "class=\"easyui-numberbox\" min=\"0\" max=\"100\" precision=\"2\" onblur=\"Heji(this);\"", "width:55px;");
                        perSum += model.ZZBQZ.ToDecimal();
                    }
                    else
                    {
                        bgColor = "background-color:#eeeeee;";
                        e.Row.Cells[4].Text = Utility.GetTextBox("SubTxt" + 4 + "_" + (e.Row.RowIndex + 1),
                        (model.ZZBQZ * 100), 4, (e.Row.RowIndex + 1), "number", true,
                        "class=\"easyui-numberbox\" min=\"0\" max=\"100\" precision=\"2\"", "width:55px;" + bgColor);
                    }
                    //年度目标值 6
                    e.Row.Cells[6].Text = Utility.GetTextBox("Txt" + 6 + "_" + (e.Row.RowIndex + 1),
                         model.ZMBZ.ToRequestString(), 6, (e.Row.RowIndex + 1), "number", true,
                         "class=\"easyui-numberbox\" precision=\"2\"", "width:75px;" + bgColor);
                    //目标值说明 7
                    e.Row.Cells[7].Text = Utility.GetTextBox("Txt" + 7 + "_" + (e.Row.RowIndex + 1),
                         model.MBZBZ, 7, (e.Row.RowIndex + 1), "string", false, "", "width:110px;" + bgColor);
                    //计算关系式 8
                    if (model.Lhzb.FZZB == "0" || string.IsNullOrEmpty(model.Lhzb.JSBDS))
                    {
                        e.Row.Cells[8].Text = Utility.GetTextBox("Txt" + 8 + "_" + (e.Row.RowIndex + 1),
                        model.ZJSGXS, 8, (e.Row.RowIndex + 1), "string", false, "", "width:110px;" + bgColor);
                    }
                    else
                    {
                        e.Row.Cells[8].Text = "<a href=\"javascript:showInfo('计算关系式','" + model.Lhzb.JSBDS + "');\">[查看]</a>";
                    }
                    //指标代号 9
                    e.Row.Cells[9].Text = model.Lhzb.ZBDH;
                }
                else if (model.Dfzb != null)
                {
                    //指标类别 1 指标名称 2
                    e.Row.Cells[1].Text = model.Dfzb.Zbxx.YJZBMC;
                    e.Row.Cells[2].Text = model.Dfzb.Zbxx.ZBMC;
                    //分值 5
                    e.Row.Cells[5].Text = Utility.GetTextBox("Txt" + 5 + "_" + (e.Row.RowIndex + 1),
                        model.ZZBFZ.ToRequestString(), 5, (e.Row.RowIndex + 1), "number", true, "", "width:55px;");
                }

                //指标顺序号11
                e.Row.Cells[11].Text = Utility.GetTextBox("Txt" + 11 + "_" + (e.Row.RowIndex + 1),
                    model.ZXSXH, 11, (e.Row.RowIndex + 1), "number", true, "", "width:40px;");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "<input type='button' value='全选' onclick='selectAll();'/>";
                e.Row.Cells[1].ColumnSpan = 3;
                e.Row.Cells[1].Text = "主指标权重合计：";
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Text = "<div id='QzhjDiv'><font color='Red'>" + Convert.ToString(perSum * 100) + "%</font></div>";
            }
        }

        /// <summary>
        /// 行命令处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var list = ViewState["PERFO_NDJXZRSZB"] as List<KhJxzrszbModel>;
            if (list == null || list.Count == 0) 
                return;
            var model = list.Find(p=>p.ZRSZBID == e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                case "up":
                    if (model != null)
                    {
                        upOneGrade(model);
                    }
                    BindGrid(false);
                    break;
                case "down":
                    if (model != null)
                    {
                        downOneGrade(model);
                    }
                    BindGrid(false);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            string key = string.Empty;
            int xh = 0;
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                //检测是否有效
                CheckBox chkbox = gvr.FindControl("CheckBox1") as CheckBox;
                if (!chkbox.Checked)
                    continue;
                KhJxzrszbModel model = new KhJxzrszbModel();
                model.DB_Option_Action = WebKeys.InsertOrUpdateAction;
                //ZRSZBID,LHZBBM,DFZBBM,ZRSID,ZZBDH,ZZBXZ,ZSJZB
                string zrszbid = GridView1.DataKeys[gvr.RowIndex].Values["ZRSZBID"].ToRequestString();
                string lhzbbm = GridView1.DataKeys[gvr.RowIndex].Values["LHZBBM"].ToRequestString();
                string dfzbbm = GridView1.DataKeys[gvr.RowIndex].Values["DFZBBM"].ToRequestString();
                int zrsid = GridView1.DataKeys[gvr.RowIndex].Values["ZRSID"].ToInt();
                string zbdh = GridView1.DataKeys[gvr.RowIndex].Values["ZZBDH"].ToRequestString();
                string zbxz = GridView1.DataKeys[gvr.RowIndex].Values["ZZBXZ"].ToRequestString();
                string sjzb = GridView1.DataKeys[gvr.RowIndex].Values["ZSJZB"].ToRequestString();
                //ZRSZBID
                model.ZRSZBID = zrszbid;
                model.LHZBBM = (!string.IsNullOrEmpty(lhzbbm)) ? lhzbbm : null;
                model.DFZBBM = (!string.IsNullOrEmpty(dfzbbm)) ? dfzbbm : null;
                model.ZRSID = zrsid;
                model.ZTJRQ = DateTime.Now;
                model.ZJGBM = Jgbm;
                model.BBMC = Bbmc;
                model.ZSZND = Sznd.ToInt();
                model.ZZBDH = zbdh;
                model.ZZBXZ = zbxz;
                model.ZSJZB = sjzb;
                model.ZXSXH = ++xh;
                if (!string.IsNullOrEmpty(model.LHZBBM))
                {
                    //权重 4
                    key = "Txt" + 4 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.ZZBQZ = Request.Form[key].ToDecimal() / 100;
                    }
                    else
                    {
                        key = "SubTxt" + 4 + "_" + (gvr.RowIndex + 1);
                        model.ZZBQZ = Request.Form[key].ToDecimal() / 100;
                    }
                    //年度目标值 6
                    key = "Txt" + 6 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.ZMBZ = model.ZNCMBZ = Request.Form[key].ToDecimal();
                    }
                    else
                    {
                        model.ZMBZ = model.ZNCMBZ = null;
                    }
                    //目标值说明 7
                    key = "Txt" + 7 + "_" + (gvr.RowIndex + 1);
                    model.MBZBZ = Request.Form[key].ToRequestString();
                    //计算关系式 8
                    key = "Txt" + 8 + "_" + (gvr.RowIndex + 1);
                    model.ZJSGXS = Request.Form[key].ToRequestString();
                }
                else if (!string.IsNullOrEmpty(model.DFZBBM))
                {
                    //分值 5
                    key = "Txt" + 5 + "_" + (gvr.RowIndex + 1);
                    model.ZZBFZ = Request.Form[key].ToDecimal();
                }
                //指标顺序号11
                key = "Txt" + 11 + "_" + (gvr.RowIndex + 1);
                model.ZXSXH = Request.Form[key].ToInt();
                jxzrsZbSrv.Execute(model);
            }
            BindGrid(true);
            Utility.ShowMsg(Page, "提示", "更新成功！", 100, "show");
        }


        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            string url = string.Format("BB={0}&BM={1}&ND={2}", Bbmc, Jgbm, Sznd);
            Response.Redirect("~/Module/Kh/NdjxzrsZbList.aspx?" + url, true);
        }


        /// <summary>
        /// 上调一级
        /// </summary>
        /// <param name="currM"></param>
        private void upOneGrade(KhJxzrszbModel currM)
        {
            List<KhJxzrszbModel> list = ViewState["PERFO_NDJXZRSZB"] as List<KhJxzrszbModel>;
            if (list == null || list.Count == 0) return;
            int currIndex = list.FindIndex(p => p.ZRSZBID == currM.ZRSZBID);
            if (currIndex > 0 && list.Count > 1)
            {
                var prevM = list[currIndex - 1];
                if (prevM != null)
                {
                    int xsxh = currM.ZXSXH.ToInt();
                    currM.DB_Option_Action = WebKeys.UpdateAction;
                    currM.ZXSXH = prevM.ZXSXH;
                    list[currIndex - 1] = currM;
                    prevM.DB_Option_Action = WebKeys.UpdateAction;
                    prevM.ZXSXH = xsxh;
                    list[currIndex] = prevM;
                }
            }
            ViewState["PERFO_NDJXZRSZB"] = list;
        }

        /// <summary>
        /// 下调一级
        /// </summary>
        /// <param name="currM"></param>
        private void downOneGrade(KhJxzrszbModel currM)
        {
            List<KhJxzrszbModel> list = ViewState["PERFO_NDJXZRSZB"] as List<KhJxzrszbModel>;
            if (list == null || list.Count == 0) return;
            int currIndex = list.FindIndex(p => p.ZRSZBID == currM.ZRSZBID);
            if (currIndex < list.Count - 1)
            {
                var nextM = list[currIndex + 1];
                if (nextM != null)
                {
                    int xsxh = currM.ZXSXH.ToInt();
                    currM.DB_Option_Action = WebKeys.UpdateAction;
                    currM.ZXSXH = nextM.ZXSXH;
                    list[currIndex + 1] = currM;
                    nextM.DB_Option_Action = WebKeys.UpdateAction;
                    nextM.ZXSXH = xsxh;
                    list[currIndex] = nextM;
                }
            }
            ViewState["PERFO_NDJXZRSZB"] = list;
        }

        #endregion

        #region 专用方法

        /// <summary>
        /// 获取当前指标的所有子指标组合表达式
        /// </summary>
        /// <param name="lhzb"></param>
        /// <param name="zbList"></param>
        /// <returns></returns>
        private string getZJSGXS(ZbkLhzbModel lhzb, List<ZbkLhzbModel> zbList)
        {
            //合成其子指标
            string jsbds = "";
            var subList = zbList.Where(p => p.PARENTZBBM == lhzb.LHZBBM).OrderBy(p => p.ZBXH).ToList();
            if (subList.Count > 0)
            {
                jsbds = lhzb.ZBDH + "=";
                foreach (var subZb in subList)
                {
                    jsbds += subZb.ZBDH + "+";
                }
                jsbds = jsbds.TrimEnd('+');
            }
            return jsbds;
            //if (!string.IsNullOrEmpty(lhzb.JSBDS))
            //{
            //    return lhzb.JSBDS;
            //}
            //else
            //{
                
            //}
        }

        #endregion

    }
}