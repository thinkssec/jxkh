using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

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
    /// 二级单位及领导班子考核结果审定操作页面
    /// </summary>
    public partial class EjdwKhjgShending : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        KhEjdwkhdfService ejdwkhdfSrv = new KhEjdwkhdfService();//二级单位考核得分表
        KhKhglService khglSrv = new KhKhglService();//考核管理
        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected KhKhglModel Kaohe = null;//考核期
        protected List<KhEjdwkhdfModel> EjdwkhdfList = null;//考核得分数据集

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
               LnkBtn_InitData.Visible = LnkBtn_Edit.Visible = LnkBtn_Calculate.Visible 
                   = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Hid_TabTitle.Value = TabTitle = "二级单位";
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

            var list = ejdwkhdfSrv.GetListByKhid(Ddl_Kaohe.SelectedValue);
            if (list.Count == 0)
            {
                //先进行一次数据初始化
                ejdwkhdfSrv.InitEjdwkhdfData(Ddl_Kaohe.SelectedValue);
            }
            else if (!string.IsNullOrEmpty(list.First().HZBZ))
            {
                Lbl_Msg.Text = "本期考核结果已发布了!";
            }
            //数据集
            EjdwkhdfList = ejdwkhdfSrv.GetListByKhid(Ddl_Kaohe.SelectedValue) as List<KhEjdwkhdfModel>;
            
            //二级单位
            var ejdwDataLst = EjdwkhdfList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() || p.ISHBJF == "1")
                .DistinctBy(p => p.JGMC).OrderBy(p => p.GSDWMC).ThenBy(p => p.DWPM).ToList();
            GridView1.DataSource = ejdwDataLst;
            GridView1.DataBind();
            Utility.GroupRows(GridView1, 0);
            Utility.GroupRows(GridView1, 1);
            if (ejdwDataLst.Count > 0 && !string.IsNullOrEmpty(ejdwDataLst.First().HZBZ))
            {
                Lbl_Msg.Text += " 【二级单位】 ";
            }

            //领导班子
            var ldbzDataLst = EjdwkhdfList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString() || p.ISHBJF == "1")
                .DistinctBy(p => p.JGMC).OrderBy(p => p.GSDWMC).ThenBy(p => p.FZRDFLB).ThenBy(p => p.DWPM).ToList();
            //GridView2.DataSource = ldbzDataLst;
            //GridView2.DataBind();
            //Utility.GroupRows(GridView2, 0);
            //Utility.GroupRows(GridView2, 1);
            //Utility.GroupRows(GridView2, 2);
            if (ldbzDataLst.Count > 0 && ldbzDataLst.First().HZBZ == "2")
            {
                Lbl_Msg.Text += " 【领导班子】 ";
            }

        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
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

            Lbl_Msg.Text = "您可以汇总和修改各单位的得分!";
        }

        #endregion


        #region 事件处理区

        /// <summary>
        /// 生成二级单位表头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //判断创建的行是否为表头行
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DynamicTHeaderHepler dHelper = new DynamicTHeaderHepler();
                //"归属单位#序号#单位名称#效益类指标|标准分,考核得分#管理类指标|标准分,考核得分#综合得分#扣分情况#加分情况#最终得分#排名"
                string header = "归属单位#序号#单位名称#管理类指标|标准分,考核得分#专项考核类指标|标准分,考核得分#综合得分#扣分情况#加分情况#最终得分#排名";
                dHelper.SplitTableHeader(e.Row, header);
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
                KhEjdwkhdfModel model = e.Row.DataItem as KhEjdwkhdfModel;

                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

                if (model.ISHBJF == "1")
                {
                    //合并计分单位汇总-----------------------------
                    //归属单位0
                    e.Row.Cells[0].Text = model.GSDWMC;
                    //序号1
                    e.Row.Cells[1].Text = (model.DWPM != null) ? model.DWPM.ToString() : (e.Row.RowIndex + 1).ToString();
                    //单位名称2
                    e.Row.Cells[2].Text = (model.Hbjf != null) ? model.Hbjf.HBJFMC : "";
                    //综合得分7
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[7].Text = Utility.GetTextBox("Txt_" + 7 + "_" + (e.Row.RowIndex + 1), model.KHDF, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[7].Text = model.KHDF.ToRequestString();
                    }
                    //约束扣分8
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[8].Text = Utility.GetTextBox("Txt_" + 8 + "_" + (e.Row.RowIndex + 1), model.DWDXBS, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[8].Text = model.DWDXBS.ToRequestString();
                    }
                    //加分情况9
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[9].Text = Utility.GetTextBox("Txt_" + 9 + "_" + (e.Row.RowIndex + 1), model.DWPJF, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[9].Text = model.DWPJF.ToRequestString();
                    }
                    //最终得分10
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[10].Text = Utility.GetTextBox("Txt_" + 10 + "_" + (e.Row.RowIndex + 1), model.DWZDF, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[10].Text = model.DWZDF.ToRequestString();
                    }
                    //排名11
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[11].Text = Utility.GetTextBox("Txt_" + 11 + "_" + (e.Row.RowIndex + 1), model.DWPM, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[11].Text = model.DWPM.ToRequestString();
                    }
                }
                else if (!string.IsNullOrEmpty(model.HBJFID))
                {
                    //---合并计算得分的单位之一----------------
                    //归属单位0
                    e.Row.Cells[0].Text = model.GSDWMC;
                    //序号1
                    e.Row.Cells[1].Text = (model.DWPM != null) ? model.DWPM.ToString() : (e.Row.RowIndex + 1).ToString();
                    //单位名称2
                    e.Row.Cells[2].Text = model.JGMC;
                    //按类型提取数据
                    decimal dlzbZhdf = 0M;
                    var xylzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "专项考核类").FirstOrDefault();
                    if (xylzb != null)
                    {
                        //效益类标准分3
                        e.Row.Cells[5].Text = xylzb.KHBZF.ToRequestString();
                        //效益类得分4
                        if (Pnl_Edit.Visible)
                        {
                            e.Row.Cells[6].Text = Utility.GetTextBox("Txt_" + 6 + "_" + (e.Row.RowIndex + 1), xylzb.KHDF, 0, 0,
                                "number", true, "", "width:80px;");
                        }
                        else
                        {
                            e.Row.Cells[6].Text = xylzb.KHDF.ToRequestString();
                        }
                        dlzbZhdf += xylzb.KHDF.ToDecimal();
                    }
                    var gllzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "管理类").FirstOrDefault();
                    if (gllzb != null)
                    {
                        //管理类 标准分5
                        e.Row.Cells[3].Text = gllzb.KHBZF.ToRequestString();
                        //管理类得分6
                        if (Pnl_Edit.Visible)
                        {
                            e.Row.Cells[4].Text = Utility.GetTextBox("Txt_" + 4 + "_" + (e.Row.RowIndex + 1), gllzb.KHDF, 0, 0,
                                "number", true, "", "width:80px;");
                        }
                        else
                        {
                            e.Row.Cells[4].Text = gllzb.KHDF.ToRequestString();
                        }
                        dlzbZhdf += gllzb.KHDF.ToDecimal();
                    }
                    //综合得分7
                    e.Row.Cells[7].Text = (dlzbZhdf > 0) ? dlzbZhdf.ToString() : "";
                    //约束扣分8
                    var ysxzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "约束性指标").FirstOrDefault();
                    if (ysxzb != null)
                    {
                        if (Pnl_Edit.Visible)
                        {
                            e.Row.Cells[8].Text = Utility.GetTextBox("Txt_" + 8 + "_" + (e.Row.RowIndex + 1), ysxzb.KHDF, 0, 0,
                                "number", true, "", "width:80px;");
                        }
                        else
                        {
                            e.Row.Cells[8].Text = ysxzb.KHDF.ToRequestString();
                        }
                    }
                    //加分情况9
                    var jfzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "加分指标").FirstOrDefault();
                    if (jfzb != null)
                    {
                        if (Pnl_Edit.Visible)
                        {
                            e.Row.Cells[9].Text = Utility.GetTextBox("Txt_" + 9 + "_" + (e.Row.RowIndex + 1), jfzb.KHDF, 0, 0,
                                "number", true, "", "width:80px;");
                        }
                        else
                        {
                            e.Row.Cells[9].Text = jfzb.KHDF.ToRequestString();
                        }
                    }
                }
                else
                {
                    //---独立计算得分的单位--------------------
                    //归属单位0
                    e.Row.Cells[0].Text = model.GSDWMC;
                    //序号1
                    e.Row.Cells[1].Text = (model.DWPM != null) ? model.DWPM.ToString() : (e.Row.RowIndex + 1).ToString();
                    //单位名称2
                    e.Row.Cells[2].Text = model.JGMC;

                    //按类型提取数据
                    decimal dlzbZhdf = 0M;
                    var xylzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "专项考核类").FirstOrDefault();
                    if (xylzb != null)
                    {
                        //效益类标准分3
                        e.Row.Cells[5].Text = xylzb.KHBZF.ToRequestString();
                        //效益类得分4
                        if (Pnl_Edit.Visible)
                        {
                            e.Row.Cells[6].Text = Utility.GetTextBox("Txt_" + 6 + "_" + (e.Row.RowIndex + 1), xylzb.KHDF, 0, 0,
                                "number", true, "", "width:80px;");
                        }
                        else
                        {
                            e.Row.Cells[6].Text = xylzb.KHDF.ToRequestString();
                        }
                        dlzbZhdf += xylzb.KHDF.ToDecimal();
                    }

                    var gllzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "管理类").FirstOrDefault();
                    if (gllzb != null)
                    {
                        //管理类 标准分5
                        e.Row.Cells[3].Text = gllzb.KHBZF.ToRequestString();
                        //管理类得分6
                        if (Pnl_Edit.Visible)
                        {
                            e.Row.Cells[4].Text = Utility.GetTextBox("Txt_" + 4 + "_" + (e.Row.RowIndex + 1), gllzb.KHDF, 0, 0,
                                "number", true, "", "width:80px;");
                        }
                        else
                        {
                            e.Row.Cells[4].Text = gllzb.KHDF.ToRequestString();
                        }
                        dlzbZhdf += gllzb.KHDF.ToDecimal();
                    }
                    //综合得分7
                    e.Row.Cells[7].Text = (dlzbZhdf > 0) ? dlzbZhdf.ToString() : "";
                    //约束扣分8
                    var ysxzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "约束性指标").FirstOrDefault();
                    if (ysxzb != null)
                    {
                        if (Pnl_Edit.Visible)
                        {
                            e.Row.Cells[8].Text = Utility.GetTextBox("Txt_" + 8 + "_" + (e.Row.RowIndex + 1), ysxzb.KHDF, 0, 0,
                                "number", true, "", "width:80px;");
                        }
                        else
                        {
                            e.Row.Cells[8].Text = ysxzb.KHDF.ToRequestString();
                        }
                    }
                    //加分情况9
                    var jfzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "加分指标").FirstOrDefault();
                    if (jfzb != null)
                    {
                        if (Pnl_Edit.Visible)
                        {
                            e.Row.Cells[9].Text = Utility.GetTextBox("Txt_" + 9 + "_" + (e.Row.RowIndex + 1), jfzb.KHDF, 0, 0,
                                "number", true, "", "width:80px;");
                        }
                        else
                        {
                            e.Row.Cells[9].Text = jfzb.KHDF.ToRequestString();
                        }
                    }
                    //最终得分10
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[10].Text = Utility.GetTextBox("Txt_" + 10 + "_" + (e.Row.RowIndex + 1), model.DWZDF, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[10].Text = model.DWZDF.ToRequestString();
                    }
                    //排名11
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[11].Text = Utility.GetTextBox("Txt_" + 11 + "_" + (e.Row.RowIndex + 1), model.DWPM, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[11].Text = model.DWPM.ToRequestString();
                    }
                }
            }
        }

        /// <summary>
        /// 生成领导班子表头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //判断创建的行是否为表头行
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DynamicTHeaderHepler dHelper = new DynamicTHeaderHepler();
                //"类别#归属单位#序号#单位名称#效益类指标|标准分,考核得分#管理类指标|标准分,考核得分#综合得分#经营难度系数#加减分情况#最终得分#排名#兑现倍数"
                //string header = "类别#归属单位#序号#单位名称#管理类指标|标准分,考核得分#专项考核类指标|标准分,考核得分#综合得分#经营难度系数#加减分情况#最终得分#排名#兑现倍数";
                string header = "归属单位#序号#单位名称#管理类指标|标准分,考核得分#专项考核类指标|标准分,考核得分#综合得分#经营难度系数#加减分情况#最终得分#排名";
                dHelper.SplitTableHeader(e.Row, header);
            }
        }

        /// <summary>
        /// 负责人行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhEjdwkhdfModel model = e.Row.DataItem as KhEjdwkhdfModel;

                if (model.ISHBJF == "1")
                {
                    //合并计分单位汇总------------------------------
                    //类别 0 
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[0].Text = getLdbzDflbSelect("TxtLdbz_" + 0 + "_" + (e.Row.RowIndex + 1), model.FZRDFLB);
                    }
                    else
                    {
                        e.Row.Cells[0].Text = model.FZRDFLB;
                    }
                    //归属单位 1
                    e.Row.Cells[1].Text = model.GSDWMC;
                    //序号 2
                    e.Row.Cells[2].Text = (model.FZRPM != null) ? model.FZRPM.ToString() : (e.Row.RowIndex + 1).ToString();
                    //单位名称3
                    e.Row.Cells[3].Text = (model.Hbjf != null) ? model.Hbjf.HBJFMC : "";
                    //综合得分8
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[8].Text = Utility.GetTextBox("TxtLdbz_" + 8 + "_" + (e.Row.RowIndex + 1), model.KHDF, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[8].Text = model.KHDF.ToRequestString();
                    }
                    //经营难度系数 9
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[9].Text = Utility.GetTextBox("TxtLdbz_" + 9 + "_" + (e.Row.RowIndex + 1), model.NDXS, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[9].Text = model.NDXS.ToRequestString();
                    }
                    //加减分情况 10
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[10].Text = Utility.GetTextBox("TxtLdbz_" + 10 + "_" + (e.Row.RowIndex + 1), model.KHBZF, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[10].Text = model.KHBZF.ToRequestString();
                    }
                    //最终得分11
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[11].Text = Utility.GetTextBox("TxtLdbz_" + 11 + "_" + (e.Row.RowIndex + 1), model.FZRZDF, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[11].Text = model.FZRZDF.ToRequestString();
                    }
                    //排名12
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[12].Text = Utility.GetTextBox("TxtLdbz_" + 12 + "_" + (e.Row.RowIndex + 1), model.FZRPM, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[12].Text = model.FZRPM.ToRequestString();
                    }
                    //兑现倍数 13
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[13].Text = Utility.GetTextBox("TxtLdbz_" + 13 + "_" + (e.Row.RowIndex + 1), model.FZRDXBS, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[13].Text = model.FZRDXBS.ToRequestString();
                    }
                }
                else if (!string.IsNullOrEmpty(model.HBJFID))
                {
                    //---合并计算得分的单位之一----------------------
                    //类别 0 
                    e.Row.Cells[0].Text = model.FZRDFLB;
                    //归属单位 1
                    e.Row.Cells[1].Text = model.GSDWMC;
                    //序号 2
                    e.Row.Cells[2].Text = (model.FZRPM != null) ? model.FZRPM.ToString() : (e.Row.RowIndex + 1).ToString();
                    //单位名称3
                    e.Row.Cells[3].Text = model.JGMC;

                    //按类型提取数据
                    decimal dlzbZhdf = 0M;
                    var xylzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "专项考核类").FirstOrDefault();
                    if (xylzb != null)
                    {
                        //效益类标准分4
                        e.Row.Cells[6].Text = xylzb.KHBZF.ToRequestString();
                        //效益类得分5
                        e.Row.Cells[7].Text = xylzb.KHDF.ToRequestString();
                        dlzbZhdf += xylzb.KHDF.ToDecimal();
                    }

                    var gllzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "管理类").FirstOrDefault();
                    if (gllzb != null)
                    {
                        //管理类 标准分6
                        e.Row.Cells[4].Text = gllzb.KHBZF.ToRequestString();
                        //管理类得分7
                        e.Row.Cells[5].Text = gllzb.KHDF.ToRequestString();
                        dlzbZhdf += gllzb.KHDF.ToDecimal();
                    }
                    //综合得分8
                    e.Row.Cells[8].Text = (dlzbZhdf > 0) ? dlzbZhdf.ToString() : "";

                    //加减分情况 10
                    string jjfV = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString() && p.JGBM == model.JGBM
                            && (p.KHXMC == "加减分因素" || p.KHXMC == "约束性指标")).Sum(p => p.KHDF).ToRequestString();
                    var ysxzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString() && p.JGBM == model.JGBM
                            && (p.KHXMC == "约束性指标")).FirstOrDefault();
                    e.Row.Cells[10].Text = jjfV + getLdbzYssm(ysxzb);
                    e.Row.Cells[10].ToolTip = (ysxzb != null) ? ysxzb.BZSM.ToRequestString() : "";
                }
                else
                {
                    //---独立计算得分的单位----------------------------
                    //类别 0 
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[0].Text = getLdbzDflbSelect("TxtLdbz_" + 0 + "_" + (e.Row.RowIndex + 1), model.FZRDFLB);
                    }
                    else
                    {
                        e.Row.Cells[0].Text = model.FZRDFLB;
                    }
                    //归属单位 1
                    e.Row.Cells[1].Text = model.GSDWMC;
                    //序号 2
                    e.Row.Cells[2].Text = (model.FZRPM != null) ? model.FZRPM.ToString() : (e.Row.RowIndex + 1).ToString();
                    //单位名称3
                    e.Row.Cells[3].Text = model.JGMC;

                    //按类型提取数据
                    decimal dlzbZhdf = 0M;
                    var xylzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "专项考核类").FirstOrDefault();
                    if (xylzb != null)
                    {
                        //效益类标准分4
                        e.Row.Cells[6].Text = xylzb.KHBZF.ToRequestString();
                        //效益类得分5
                        e.Row.Cells[7].Text = xylzb.KHDF.ToRequestString();
                        dlzbZhdf += xylzb.KHDF.ToDecimal();
                    }

                    var gllzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "管理类").FirstOrDefault();
                    if (gllzb != null)
                    {
                        //管理类 标准分6
                        e.Row.Cells[4].Text = gllzb.KHBZF.ToRequestString();
                        //管理类得分7
                        e.Row.Cells[5].Text = gllzb.KHDF.ToRequestString();
                        dlzbZhdf += gllzb.KHDF.ToDecimal();
                    }
                    //综合得分8
                    e.Row.Cells[8].Text = (dlzbZhdf > 0) ? dlzbZhdf.ToString() : "";
                    //经营难度系数 9
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[9].Text = Utility.GetTextBox("TxtLdbz_" + 9 + "_" + (e.Row.RowIndex + 1), model.NDXS, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[9].Text = model.NDXS.ToRequestString();
                    }
                    //加减分情况 10
                    string jjfV = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString() && p.JGBM == model.JGBM
                            && (p.KHXMC == "加减分因素" || p.KHXMC == "约束性指标")).Sum(p => p.KHDF).ToRequestString();
                    var ysxzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString() && p.JGBM == model.JGBM
                            && (p.KHXMC == "约束性指标")).FirstOrDefault();
                    e.Row.Cells[10].Text = jjfV + getLdbzYssm(ysxzb);
                    e.Row.Cells[10].ToolTip = (ysxzb != null) ? ysxzb.BZSM.ToRequestString() : "";
                    //if (Pnl_Edit.Visible)
                    //{
                    //    e.Row.Cells[10].Text = Utility.GetTextBox("TxtLdbz_" + 10 + "_" + (e.Row.RowIndex + 1), jjfV, 0, 0,
                    //        "number", true, "", "width:80px;");
                    //}
                    //else
                    //{
                        
                    //}
                    //最终得分11
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[11].Text = Utility.GetTextBox("TxtLdbz_" + 11 + "_" + (e.Row.RowIndex + 1), model.FZRZDF, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[11].Text = model.FZRZDF.ToRequestString();
                    }
                    //排名12
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[12].Text = Utility.GetTextBox("TxtLdbz_" + 12 + "_" + (e.Row.RowIndex + 1), model.FZRPM, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[12].Text = model.FZRPM.ToRequestString();
                    }
                    //兑现倍数 13
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[13].Text = Utility.GetTextBox("TxtLdbz_" + 13 + "_" + (e.Row.RowIndex + 1), model.FZRDXBS, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[13].Text = model.FZRDXBS.ToRequestString();
                    }
                }
            }
        }

        /// <summary>
        /// 计算各二级单位得分并更新到数据表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Calculate_Click(object sender, EventArgs e)
        {
            ejdwkhdfSrv.CalculateEjdwkhdfData(Ddl_Kaohe.SelectedValue);
            BindGrid();
            TabTitle = "二级单位";
        }

        /// <summary>
        /// 计算领导班子得分并更新到数据表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_CalculateLdbz_Click(object sender, EventArgs e)
        {
            ejdwkhdfSrv.CalculateEjdwldbzKhdfData(Ddl_Kaohe.SelectedValue);
            BindGrid();
            TabTitle = "领导班子";
        }

        /// <summary>
        /// 考核期切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Kaohe_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 得分审定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Edit_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = true;
            BindGrid();
        }

        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_InitData_Click(object sender, EventArgs e)
        {
            if (ejdwkhdfSrv.DeleteEjdwkhdfData(Ddl_Kaohe.SelectedValue))
            {
                ejdwkhdfSrv.InitEjdwkhdfData(Ddl_Kaohe.SelectedValue);
            }
            BindGrid();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            StringBuilder sqlSB = new StringBuilder();
            string key = string.Empty;
            string updSql = "update PERFO_KH_EJDWKHDF set ";
            //1==二级单位========================================================================================
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                //DFID,KHID,JGBM,KHLX,HBJFID,ISHBJF
                string DFID = GridView1.DataKeys[gvr.RowIndex].Values["DFID"].ToRequestString();
                string KHID = GridView1.DataKeys[gvr.RowIndex].Values["KHID"].ToRequestString();
                string JGBM = GridView1.DataKeys[gvr.RowIndex].Values["JGBM"].ToRequestString();
                string KHLX = GridView1.DataKeys[gvr.RowIndex].Values["KHLX"].ToRequestString();
                string HBJFID = GridView1.DataKeys[gvr.RowIndex].Values["HBJFID"].ToRequestString();
                string ISHBJF = GridView1.DataKeys[gvr.RowIndex].Values["ISHBJF"].ToRequestString();
                if (!string.IsNullOrEmpty(ISHBJF))
                {
                    //合并计分汇总项，只有一条记录直接更新--------------------------------------
                    //综合得分7  KHDF
                    key = "Txt_" + 7 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " KHDF='" + Request.Form[key] + "' ";
                        sql += string.Format(" where DFID='{0}';", DFID);
                        sqlSB.Append(sql);
                    }
                    //约束扣分8  DWDXBS
                    key = "Txt_" + 8 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " DWDXBS='" + Request.Form[key] + "' ";
                        sql += string.Format(" where DFID='{0}';", DFID);
                        sqlSB.Append(sql);
                    }
                    //加分情况9  DWPJF
                    key = "Txt_" + 9 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " DWPJF='" + Request.Form[key] + "' ";
                        sql += string.Format(" where DFID='{0}';", DFID);
                        sqlSB.Append(sql);
                    }
                    //最终得分10  DWZDF
                    key = "Txt_" + 10 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " DWZDF='" + Request.Form[key] + "' ";
                        sql += string.Format(" where DFID='{0}';", DFID);
                        sqlSB.Append(sql);
                    }
                    //排名11
                    key = "Txt_" + 11 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " DWPM='" + Request.Form[key] + "' ";
                        sql += string.Format(" where DFID='{0}';", DFID);
                        sqlSB.Append(sql);
                    }
                }
                else if (!string.IsNullOrEmpty(HBJFID))
                {
                    //合并计分子项---------------------------------------------------------------
                    //效益类得分4
                    key = "Txt_" + 4 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " KHDF='" + Request.Form[key] + "' ";
                        sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHLX='{2}' and KHXMC='{3}';",
                            KHID, JGBM, KHLX, "效益类");
                        sqlSB.Append(sql);
                    }
                    //管理类得分6
                    key = "Txt_" + 6 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " KHDF='" + Request.Form[key] + "' ";
                        sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHLX='{2}' and KHXMC='{3}';",
                            KHID, JGBM, KHLX, "管理类");
                        sqlSB.Append(sql);
                    }
                    //约束扣分8
                    key = "Txt_" + 8 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " KHDF='" + Request.Form[key] + "' ";
                        sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHLX='{2}' and KHXMC='{3}';",
                            KHID, JGBM, KHLX, "约束性指标");
                        sqlSB.Append(sql);
                    }
                    //加分情况9
                    key = "Txt_" + 9 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " KHDF='" + Request.Form[key] + "' ";
                        sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHLX='{2}' and KHXMC='{3}';",
                            KHID, JGBM, KHLX, "加分指标");
                        sqlSB.Append(sql);
                    }
                }
                else
                {
                    //独立计分单位---------------------------------------------------------------
                    //效益类得分4
                    key = "Txt_" + 4 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " KHDF='" + Request.Form[key] + "' ";
                        sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHLX='{2}' and KHXMC='{3}';",
                            KHID, JGBM, KHLX, "效益类");
                        sqlSB.Append(sql);
                    }
                    //管理类得分6
                    key = "Txt_" + 6 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " KHDF='" + Request.Form[key] + "' ";
                        sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHLX='{2}' and KHXMC='{3}';",
                            KHID, JGBM, KHLX, "管理类");
                        sqlSB.Append(sql);
                    }
                    //约束扣分8
                    key = "Txt_" + 8 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " KHDF='" + Request.Form[key] + "' ";
                        sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHLX='{2}' and KHXMC='{3}';",
                            KHID, JGBM, KHLX, "约束性指标");
                        sqlSB.Append(sql);
                    }
                    //加分情况9
                    key = "Txt_" + 9 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " KHDF='" + Request.Form[key] + "' ";
                        sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHLX='{2}' and KHXMC='{3}';",
                            KHID, JGBM, KHLX, "加分指标");
                        sqlSB.Append(sql);
                    }
                    //最终得分10
                    key = "Txt_" + 10 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " DWZDF='" + Request.Form[key] + "' ";
                        sql += string.Format(" where KHID='{0}' and JGBM='{1}';",
                            KHID, JGBM);
                        sqlSB.Append(sql);
                    }
                    //排名11
                    key = "Txt_" + 11 + "_" + (gvr.RowIndex + 1);
                    if (!string.IsNullOrEmpty(Request.Form[key]))
                    {
                        string sql = updSql;
                        sql += " DWPM='" + Request.Form[key] + "' ";
                        sql += string.Format(" where KHID='{0}' and JGBM='{1}';",
                            KHID, JGBM);
                        sqlSB.Append(sql);
                    }
                }
            }

            //2==领导班子========================================================================================
            //foreach (GridViewRow gvr in GridView2.Rows)
            //{
            //    //DFID,KHID,JGBM,KHLX,HBJFID,ISHBJF
            //    string DFID = GridView1.DataKeys[gvr.RowIndex].Values["DFID"].ToRequestString();
            //    string KHID = GridView1.DataKeys[gvr.RowIndex].Values["KHID"].ToRequestString();
            //    string JGBM = GridView1.DataKeys[gvr.RowIndex].Values["JGBM"].ToRequestString();
            //    string KHLX = GridView1.DataKeys[gvr.RowIndex].Values["KHLX"].ToRequestString();
            //    string HBJFID = GridView1.DataKeys[gvr.RowIndex].Values["HBJFID"].ToRequestString();
            //    string ISHBJF = GridView1.DataKeys[gvr.RowIndex].Values["ISHBJF"].ToRequestString();
            //    if (!string.IsNullOrEmpty(ISHBJF))
            //    {
            //        //合并计分汇总项，只有一条记录直接更新--------------------------------------
            //        //类别 0  FZRDFLB
            //        key = "TxtLdbz_" + 0 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            string sql = updSql;
            //            sql += " FZRDFLB='" + Request.Form[key] + "' ";
            //            sql += string.Format(" where DFID='{0}' or HBJFID='{1}';", DFID, HBJFID);
            //            sqlSB.Append(sql);
            //        }
            //        //综合得分8 KHDF
            //        key = "TxtLdbz_" + 8 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            string sql = updSql;
            //            sql += " KHDF='" + Request.Form[key] + "' ";
            //            sql += string.Format(" where DFID='{0}';", DFID);
            //            sqlSB.Append(sql);
            //        }
            //        //经营难度系数 9 NDXS
            //        key = "TxtLdbz_" + 9 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            string sql = updSql;
            //            sql += " NDXS='" + Request.Form[key] + "' ";
            //            sql += string.Format(" where DFID='{0}' or HBJFID='{1}';", DFID, HBJFID);
            //            sqlSB.Append(sql);
            //        }
            //        //加减分情况 10 KHBZF
            //        key = "TxtLdbz_" + 10 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            string sql = updSql;
            //            sql += " KHBZF='" + Request.Form[key] + "' ";
            //            sql += string.Format(" where DFID='{0}';", DFID);
            //            sqlSB.Append(sql);
            //        }
            //        //最终得分11 FZRZDF
            //        key = "TxtLdbz_" + 11 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            string sql = updSql;
            //            sql += " FZRZDF='" + Request.Form[key] + "' ";
            //            sql += string.Format(" where DFID='{0}' or HBJFID='{1}';", DFID, HBJFID);
            //            sqlSB.Append(sql);
            //        }
            //        //排名12 FZRPM
            //        key = "TxtLdbz_" + 12 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            string sql = updSql;
            //            sql += " FZRPM='" + Request.Form[key] + "' ";
            //            sql += string.Format(" where DFID='{0}' or HBJFID='{1}';", DFID, HBJFID);
            //            sqlSB.Append(sql);
            //        }
            //        //兑现倍数 13 FZRDXBS
            //        key = "TxtLdbz_" + 13 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            string sql = updSql;
            //            sql += " FZRDXBS='" + Request.Form[key] + "' ";
            //            sql += string.Format(" where DFID='{0}' or HBJFID='{1}';", DFID, HBJFID);
            //            sqlSB.Append(sql);
            //        }
            //    }
            //    else if (!string.IsNullOrEmpty(HBJFID))
            //    {
            //        //合并计分子项，其更新内容由汇总项负责同时更新------------------

            //    }
            //    else
            //    {
            //        //独立计分单位--------------------------------------------------
            //        //类别 0 
            //        key = "TxtLdbz_" + 0 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            string sql = updSql;
            //            sql += " FZRDFLB='" + Request.Form[key] + "' ";
            //            sql += string.Format(" where KHID='{0}' and JGBM='{1}';",
            //                KHID, JGBM);
            //            sqlSB.Append(sql);
            //        }
            //        //经营难度系数 9
            //        key = "TxtLdbz_" + 9 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            string sql = updSql;
            //            sql += " NDXS='" + Request.Form[key] + "' ";
            //            sql += string.Format(" where KHID='{0}' and JGBM='{1}';",
            //                KHID, JGBM);
            //            sqlSB.Append(sql);
            //        }
            //        //最终得分11
            //        key = "TxtLdbz_" + 11 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            string sql = updSql;
            //            sql += " FZRZDF='" + Request.Form[key] + "' ";
            //            sql += string.Format(" where KHID='{0}' and JGBM='{1}';",
            //                KHID, JGBM);
            //            sqlSB.Append(sql);
            //        }
            //        //排名12
            //        key = "TxtLdbz_" + 12 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            string sql = updSql;
            //            sql += " FZRPM='" + Request.Form[key] + "' ";
            //            sql += string.Format(" where KHID='{0}' and JGBM='{1}';",
            //                KHID, JGBM);
            //            sqlSB.Append(sql);
            //        }
            //        ////兑现倍数 13
            //        //key = "TxtLdbz_" + 13 + "_" + (gvr.RowIndex + 1);
            //        //if (!string.IsNullOrEmpty(Request.Form[key]))
            //        //{
            //        //    string sql = updSql;
            //        //    sql += " FZRDXBS='" + Request.Form[key] + "' ";
            //        //    sql += string.Format(" where KHID='{0}' and JGBM='{1}';",
            //        //        KHID, JGBM);
            //        //    sqlSB.Append(sql);
            //        //}
            //    }
            //}

            //3==保存数据
            Debuger.GetInstance().log(sqlSB.ToString());
            if (ejdwkhdfSrv.ExecuteSQL("begin "+sqlSB.ToString()+" end;"))
            {
                //根据用户选择的发布类型，正式发布考核成绩并锁定
                //更新状态位为HZBZ=1，保存操作人和时间
                string sql = updSql;
                int v = 0;
                foreach (ListItem item in ChkBox_FBLX.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value == "EJDW") v = 1;
                        else if (item.Value == "LDBZ") v = 2;
                    }
                }
                if (v > 0)
                {
                    sql += string.Format(" CZR='{0}',TJSJ=to_date('{1}','yyyy-mm-dd'),HZBZ='{2}' ", userModel.USERNAME, DateTime.Now.ToDateYMDFormat(), v);
                    sql += string.Format(" where KHID='{0}';", Ddl_Kaohe.SelectedValue);
                    if (ejdwkhdfSrv.ExecuteSQL("begin "+sql+" end;"))
                    {
                        //完成考核
                        khglSrv.FinishKaohe(Ddl_Kaohe.SelectedValue);
                    }
                }
                Utility.ShowMsg(Page, "系统提示", "操作成功！", 100, "show");
            }
            Pnl_Edit.Visible = false;
            BindGrid();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            string url = "?KH=" + Ddl_Kaohe.SelectedValue;
            GobackPageUrl(url);
        }

        #endregion

        #region 专用方法

        /// <summary>
        /// 获取领导
        /// </summary>
        /// <param name="ctrlName">控件名</param>
        /// <param name="v">值</param>
        /// <returns></returns>
        public string getLdbzDflbSelect(string ctrlName, string v)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<select size='1' name='" + ctrlName + "'>");
            sb.Append("<option value='A' " + ((v == "A") ? " selected" : "") + ">A</option>");
            sb.Append("<option value='B' " + ((v == "B") ? " selected" : "") + ">B</option>");
            sb.Append("<option value='C' " + ((v == "C") ? " selected" : "") + ">C</option>");
            sb.Append("<option value='D' " + ((v == "D") ? " selected" : "") + ">D</option>");
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取领导班子打分的约束说明
        /// </summary>
        /// <param name="yssm"></param>
        /// <returns></returns>
        public string getLdbzYssm(KhEjdwkhdfModel model)
        {
            string msgImg = string.Empty;
            if (model != null && !string.IsNullOrEmpty(model.BZSM))
            {
                if (model.BZSM.Contains("降") || model.BZSM.Contains("不"))
                {
                    msgImg = string.Format(
                        "<img src=\"/Resources/Images/send_2.png\" border=\"0\" onclick=\"showInfo('打分说明','{0}');\"/>", model.BZSM);
                }
            }
            return msgImg;
        }

        #endregion

    }
}