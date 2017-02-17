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
    /// 机关部门年度绩效责任书指标维护页面
    /// </summary>
    public partial class NdjxzrsJgbmZbEdit : PageBase
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
                BindGrid();
            }
        }

        #region 专用方法区

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindGrid()
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
                //2==先提取责任书指标表中的数据
                List<KhJxzrszbModel> zrszbList = jxzrsZbSrv.GetListBySearch(Sznd, Jgbm.ToRequestString(), Bbmc) as List<KhJxzrszbModel>;
                //3==再从量化指标表和打分指标表中分别提取所需数据
                List<ZbkLhzbModel> lhzbList = lhzbSrv.GetListForValidOrderByZbxh(Jgbm.ToRequestString(), Bbmc, false) as List<ZbkLhzbModel>;
                List<ZbkDfzbModel> dfzbList = dfzbSrv.GetListForValid(Jgbm.ToRequestString(), Bbmc, false) as List<ZbkDfzbModel>;
                //4==绑定
                GridView1.DataSource = GainJxzrsZbList(zrszbList, lhzbList, dfzbList);
                GridView1.DataBind();

                Utility.GroupRows(GridView1, 1);//合并
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
            //量化指标
            foreach (var lhzb in lhzbList)
            {
                var zb = zrszbList.Find(p => p.LHZBBM == lhzb.LHZBBM);
                if (zb != null)
                {
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
                    zbM.ZJSGXS = lhzb.JSBDS;
                    zbM.ZSJZB = lhzb.PARENTZBBM;
                    zbM.ZJGBM = Jgbm;
                    zbM.BBMC = Bbmc;
                    zbM.ZSZND = Sznd.ToInt();
                    //zbM.ZTJRQ = DateTime.Now;
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
                    //zbM.ZTJRQ = DateTime.Now;
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
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

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
                    //权重 4
                    e.Row.Cells[4].Text = Utility.GetTextBox("Txt" + 4 + "_" + (e.Row.RowIndex + 1),
                    (model.ZZBQZ * 100), 4, (e.Row.RowIndex + 1), "number", true,
                    " onblur=\"Heji(this);\" ", "width:100px;");//class=\"easyui-numberbox\" min=\"0\" max=\"100\" precision=\"2\" 
                    perSum += model.ZZBQZ.ToDecimal();
                }
                else if (model.Dfzb != null)
                {
                    //指标类别 1 指标名称 2
                    e.Row.Cells[1].Text = model.Dfzb.Zbxx.YJZBMC;
                    e.Row.Cells[2].Text = model.Dfzb.Zbxx.ZBMC;
                    //分值 5
                    e.Row.Cells[5].Text = Utility.GetTextBox("Txt" + 5 + "_" + (e.Row.RowIndex + 1),
                        model.ZZBFZ.ToRequestString(), 5, (e.Row.RowIndex + 1), "number", true, "", "width:100px;background-color:#eeeeee;");
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].ColumnSpan = 2;
                e.Row.Cells[1].Text = "主指标权重合计：";
                e.Row.Cells[2].Text = "<div id='QzhjDiv'><font color='Red'>" + Convert.ToString(perSum * 100) + "%</font></div>";
                e.Row.Cells[5].Visible = e.Row.Cells[6].Visible = e.Row.Cells[7].Visible = e.Row.Cells[8].Visible = e.Row.Cells[9].Visible = false;
            }
        }

        /// <summary>
        /// 行命令处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //var model = jxzrsSrv.GetSingle(e.CommandArgument.ToString());
            //switch (e.CommandName)
            //{
            //    case "bianji":
            //        if (model != null)
            //        {
            //            ContentPlaceHolder cont = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
            //            CommonTool.SetModelDataToForm(model, cont, "Txt_", true);
            //            CommonTool.SetModelDataToForm(model, cont, "Ddl_", true);
            //            Hid_ZRSID.Value = model.ZRSID.ToString();
            //            Txt_ZRSFJ.Text = Txt_ZRSFJ.Value = model.ZRSFJ;
            //            Pnl_Edit.Visible = true;
            //            SetCntrlVisibility(LnkBtn_Ins, false);
            //            SetCntrlVisibility(LnkBtn_Upd, true);
            //            SetCntrlVisibility(LnkBtn_Del, false);
            //        }
            //        break;
            //    case "shanchu":
            //        if (model != null)
            //        {
            //            //删除数据
            //            model.DB_Option_Action = WebKeys.DeleteAction;
            //            jxzrsSrv.Execute(model);
            //        }
            //        BindGrid();
            //        break;
            //    case "xiada"://下达
            //        if (model != null && 
            //            model.ZRSZT != "1")
            //        {
            //            model.DB_Option_Action = WebKeys.UpdateAction;
            //            model.ZRSZT = "1";
            //            model.XDRQ = DateTime.Now;
            //            jxzrsSrv.Execute(model);
            //        }
            //        BindGrid();
            //        break;
            //    default:
            //        break;
            //}
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
                model.ZXSXH = xh;
                xh += 100;//把间隔放大
                if (!string.IsNullOrEmpty(model.LHZBBM))
                {
                    //权重 4
                    key = "Txt" + 4 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        model.ZZBQZ = Request.Form[key].ToDecimal() / 100;
                    }
                    //对于量化指标则先提取与这个指标的相关数据，如果存在则只更新其权重，如果不存在还要提取当前最大序号+50
                    var lhzbLst = jxzrsZbSrv.GetListBySearch(model.ZSZND.ToRequestString(),
                        model.ZJGBM.ToRequestString(), model.BBMC).Where(p => !string.IsNullOrEmpty(p.LHZBBM)).ToList();
                    if (lhzbLst.FirstOrDefault(p => p.LHZBBM == model.LHZBBM) != null)
                    {
                        //指标已存在，只批量更新其权重即可
                        jxzrsZbSrv.UpdateZbqzByLhzbbm(model);
                        continue;
                    }
                    else
                    {
                        //不存在，则取最大序号与当前的序号比较，取大值
                        int maxXh = lhzbLst.Max(p =>p.ZXSXH).ToInt();
                        if (model.ZXSXH <= maxXh)
                        {
                            model.ZXSXH = maxXh + 100;
                            if (xh <= model.ZXSXH) xh += 100;
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(model.DFZBBM))
                {
                    model.ZXSXH += 100;//放大
                    //分值 5
                    key = "Txt" + 5 + "_" + (gvr.RowIndex + 1);
                    model.ZZBFZ = Request.Form[key].ToDecimal();
                }
                jxzrsZbSrv.Execute(model);
            }
            BindGrid();
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
            Response.Redirect("~/Module/Kh/NdjxzrsJgbmZbList.aspx?" + url, true);
        }

        #endregion

    }
}