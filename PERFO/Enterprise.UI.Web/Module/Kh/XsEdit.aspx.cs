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
    ///机关部门指标筛选维护页面
    /// </summary>
    public partial class XsEdit : PageBase
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
        protected KhKhglModel Kaohe = null;
        protected List<KhZbsxModel> KhZbsxModelLst = null;//合成后的指标筛选集合
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
                LinkButton1.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
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
            ContentPlaceHolder tabOptionItem = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
            KhXsService xsSrv=new KhXsService();
            List<KhXsModel> bml = xsSrv.GetList().ToList();
            foreach (KhXsModel model in bml)
            {
                int deptid = model.DEPTID;
                TextBox jiguan = tabOptionItem.FindControl("Txt_" + deptid + "_jiguan") as TextBox;
                TextBox jiguanx = tabOptionItem.FindControl("Txtx_" + deptid + "_jiguan") as TextBox;
                if (jiguanx == null) continue;
                jiguan.Text = model.JIGUAN.ToRequestString();
                jiguanx.Text = model.JIGUANX.ToRequestString();

                TextBox yunxingjizu = tabOptionItem.FindControl("Txt_" + deptid + "_yunxingjizu") as TextBox;
                TextBox yunxingjizux = tabOptionItem.FindControl("Txtx_" + deptid + "_yunxingjizu") as TextBox;
                yunxingjizu.Text = model.YUNXINGJIZU.ToRequestString();
                yunxingjizux.Text = model.YUNXINGJIZUX.ToRequestString();

                TextBox rebeijizu = tabOptionItem.FindControl("Txt_" + deptid + "_rebeijizu") as TextBox;
                TextBox rebeijizux = tabOptionItem.FindControl("Txtx_" + deptid + "_rebeijizu") as TextBox;
                rebeijizu.Text = model.REBEIJIZU.ToRequestString();
                rebeijizux.Text = model.REBEIJIZUX.ToRequestString();

                TextBox lengbeijizu = tabOptionItem.FindControl("Txt_" + deptid + "_lengbeijizu") as TextBox;
                TextBox lengbeijizux = tabOptionItem.FindControl("Txtx_" + deptid + "_lengbeijizu") as TextBox;
                lengbeijizu.Text = model.LENGBEIJIZU.ToRequestString();
                lengbeijizux.Text = model.LENGBEIJIZUX.ToRequestString();

                TextBox daxingshuqizhan = tabOptionItem.FindControl("Txt_" + deptid + "_daxingshuqizhan") as TextBox;
                TextBox daxingshuqizhanx = tabOptionItem.FindControl("Txtx_" + deptid + "_daxingshuqizhan") as TextBox;
                daxingshuqizhan.Text = model.DAXINGSHUQIZHAN.ToRequestString();
                daxingshuqizhanx.Text = model.DAXINGSHUQIZHANX.ToRequestString();

                TextBox xiaoxingshuqizhan = tabOptionItem.FindControl("Txt_" + deptid + "_xiaoxingshuqizhan") as TextBox;
                TextBox xiaoxingshuqizhanx = tabOptionItem.FindControl("Txtx_" + deptid + "_xiaoxingshuqizhan") as TextBox;
                xiaoxingshuqizhan.Text = model.XIAOXINGSHUQIZHAN.ToRequestString();
                xiaoxingshuqizhanx.Text = model.XIAOXINGSHUQIZHANX.ToRequestString();

                TextBox gonglishu = tabOptionItem.FindControl("Txt_" + deptid + "_gonglishu") as TextBox;
                TextBox gonglishux = tabOptionItem.FindControl("Txtx_" + deptid + "_gonglishu") as TextBox;
                gonglishu.Text = model.GONGLISHU.ToRequestString();
                gonglishux.Text = model.GONGLISHUX.ToRequestString();


                DropDownList dimao = tabOptionItem.FindControl("Ddl_" + deptid + "_dimao") as DropDownList;
                TextBox dimaox = tabOptionItem.FindControl("Txtx_" + deptid + "_dimao") as TextBox;
                dimao.SelectedValue = model.DIMAO.ToDouble().ToRequestString();
                //dimaox.Text = model.DIMAOX.ToRequestString();

                DropDownList chengzhenhua = tabOptionItem.FindControl("Ddl_" + deptid + "_chengzhenhua") as DropDownList;
                TextBox chengzhenhuax = tabOptionItem.FindControl("Txtx_" + deptid + "_chengzhenhua") as TextBox;
                chengzhenhua.SelectedValue = model.CHENGZHENHUA.ToDouble().ToStr();

                //chengzhenhuax.Text = model.CHENGZHENHUAX.ToRequestString();
                //chengzhenhuax.Text ="0.005";

                TextBox fendui = tabOptionItem.FindControl("Txt_" + deptid + "_fendui") as TextBox;
                fendui.Text = model.FENDUI.ToRequestString();

                TextBox weixiudui = tabOptionItem.FindControl("Txt_" + deptid + "_weixiudui") as TextBox;
                TextBox weixiuduix = tabOptionItem.FindControl("Txtx_" + deptid + "_weixiudui") as TextBox;
                weixiudui.Text = model.WEIXIUD.ToRequestString();
                weixiuduix.Text = model.WEIXIUDX.ToRequestString();

                TextBox zjxs = tabOptionItem.FindControl("Txt_" + deptid + "_zjxs") as TextBox;
                TextBox xs = tabOptionItem.FindControl("Txt_" + deptid + "_xs") as TextBox;
                //zjxs.Text = model.ZJXS.ToRequestString();
                xs.Text = model.XS.ToRequestString();
                

                TextBox bz = tabOptionItem.FindControl("Txt_" + deptid + "_bz") as TextBox;
                TextBox bzx = tabOptionItem.FindControl("Txt_" + deptid + "_bzx") as TextBox;
                bz.Text = model.BZ.ToRequestString();
                bzx.Text = model.BZX.ToRequestString();


            }

        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //Kaohe = khglSrv.GetSingle(Khid);
            //if (Kaohe != null)
            //{
            //    //考核名称
            //    Lbl_Kaohe.Text = Kaohe.KHMC;
            //    //考核年度
            //    Lbl_Niandu.Text = Kaohe.KHND;
            //}
            ////单位
            //Lbl_Danwei.Text = SysBmjgService.GetBmjgName(Jgbm);
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
                if (model.JxzrsZb.Lhzb != null) {
                    //指标类型及权重 1
                    e.Row.Cells[1].Text = model.JxzrsZb.Lhzb.Zbxx.ZBMC + "<br/>〖" + model.JxzrsZb.ZZBQZ.Value.ToString("P") + "〗";//名称作为类型用
                    //考核主要内容 2
                    e.Row.Cells[2].Text = "<div style='width: auto;padding: 2px;overflow:auto;'>"
                        + model.JxzrsZb.JGKHNR + "<hr/>" + model.JxzrsZb.JGKHMB + "</div>";
                    //分值 5
                    e.Row.Cells[5].Text = Utility.GetTextBox("TxtLhzb" + 5 + "_" + (e.Row.RowIndex + 1),
                        model.SXFZ.ToRequestString(), 5, (e.Row.RowIndex + 1), "number", true,
                        "class=\"easyui-numberbox\" min=\"0\" max=\"200\" precision=\"1\" onblur=\"Heji(this);\"", "width:65px;");
                    perSum += model.SXFZ.ToDecimal();//累计分值
                }
                else if (model.JxzrsZb.Dfzb != null)
                {
                    //指标类别 1
                    e.Row.Cells[1].Text = model.JxzrsZb.Dfzb.Zbxx.YJZBMC;
                    //指标名称 2
                    e.Row.Cells[2].Text = model.JxzrsZb.Dfzb.Zbxx.ZBMC;
                    //分值 5
                    e.Row.Cells[5].Text = Utility.GetTextBox("Txt" + 5 + "_" + (e.Row.RowIndex + 1),
                        model.SXFZ.ToRequestString(), 5, (e.Row.RowIndex + 1), "number", true, "", "width:65px;background-color:#eeeeee;");
                }
                //显示序号 8 model.SXXH.ToRequestString()
                int xh = model.SXXH.ToInt();
                e.Row.Cells[8].Text = Utility.GetTextBox("Txt" + 8 + "_" + (e.Row.RowIndex + 1),
                    ((xh > 0) ? xh.ToString() : model.JxzrsZb.ZXSXH.ToRequestString()), 8, (e.Row.RowIndex + 1), 
                    "number", true, "class=\"easyui-numberbox\" min=\"1\" max=\"999\"", "width:55px;");

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                //e.Row.Cells[0].Text = "∑分值合计=〖年度重点工作任务+部门履职〗：";
                e.Row.Cells[0].Text = "∑分值合计=";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[0].ColumnSpan = 3;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[5].Text = "<div id='QzhjDiv'><font color='Red'>" + perSum + "</font></div>";
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

            ContentPlaceHolder tabOptionItem = Page.Controls[0].FindControl("ProjectPH") as ContentPlaceHolder;
            SysBmjgService bmSrv = new SysBmjgService();
            KhXsService xsSrv=new KhXsService();
            List<SysBmjgModel> bml= bmSrv.GetList().Where(p => p.JGLX == "基层单位").OrderBy(o=>o.JGBM).ToList();
            string[] str = strxs.Value.Split('|');
            Dictionary<string,string> dic=new Dictionary<string,string>();
            foreach (string s in str) {
                if (string.IsNullOrEmpty(s)) continue;
                string[] item = s.Split(':');
                dic.Add(item[0],item[1]);
            }

            foreach (SysBmjgModel bm in bml) {
                int deptid = bm.JGBM;
                TextBox jiguan = tabOptionItem.FindControl("Txt_" + deptid + "_jiguan") as TextBox;
                if (jiguan == null) continue;
                TextBox jiguanx = tabOptionItem.FindControl("Txtx_" + deptid + "_jiguan") as TextBox;

                TextBox yunxingjizu = tabOptionItem.FindControl("Txt_" + deptid + "_yunxingjizu") as TextBox;
                TextBox yunxingjizux = tabOptionItem.FindControl("Txtx_" + deptid + "_yunxingjizu") as TextBox;

                TextBox rebeijizu = tabOptionItem.FindControl("Txt_" + deptid + "_rebeijizu") as TextBox;
                TextBox rebeijizux = tabOptionItem.FindControl("Txtx_" + deptid + "_rebeijizu") as TextBox;

                TextBox lengbeijizu = tabOptionItem.FindControl("Txt_" + deptid + "_lengbeijizu") as TextBox;
                TextBox lengbeijizux = tabOptionItem.FindControl("Txtx_" + deptid + "_lengbeijizu") as TextBox;

                TextBox daxingshuqizhan = tabOptionItem.FindControl("Txt_" + deptid + "_daxingshuqizhan") as TextBox;
                TextBox daxingshuqizhanx = tabOptionItem.FindControl("Txtx_" + deptid + "_daxingshuqizhan") as TextBox;

                TextBox xiaoxingshuqizhan = tabOptionItem.FindControl("Txt_" + deptid + "_xiaoxingshuqizhan") as TextBox;
                TextBox xiaoxingshuqizhanx = tabOptionItem.FindControl("Txtx_" + deptid + "_xiaoxingshuqizhan") as TextBox;

                TextBox gonglishu = tabOptionItem.FindControl("Txt_" + deptid + "_gonglishu") as TextBox;
                TextBox gonglishux = tabOptionItem.FindControl("Txtx_" + deptid + "_gonglishu") as TextBox;

                DropDownList dimao = tabOptionItem.FindControl("Ddl_" + deptid + "_dimao") as DropDownList;
                TextBox dimaox = tabOptionItem.FindControl("Txtx_" + deptid + "_dimao") as TextBox;

                DropDownList chengzhenhua = tabOptionItem.FindControl("Ddl_" + deptid + "_chengzhenhua") as DropDownList;
                TextBox chengzhenhuax = tabOptionItem.FindControl("Txtx_" + deptid + "_chengzhenhua") as TextBox;

                TextBox fendui = tabOptionItem.FindControl("Txt_" + deptid + "_fendui") as TextBox;

                TextBox weixiudui = tabOptionItem.FindControl("Txt_" + deptid + "_weixiudui") as TextBox;
                TextBox weixiuduix = tabOptionItem.FindControl("Txtx_" + deptid + "_weixiudui") as TextBox;

                TextBox zjxs = tabOptionItem.FindControl("Txt_" + deptid + "_zjxs") as TextBox;
                TextBox xs = tabOptionItem.FindControl("Txt_" + deptid + "_xs") as TextBox;

                TextBox bz = tabOptionItem.FindControl("Txt_" + deptid + "_bz") as TextBox;
                TextBox bzx = tabOptionItem.FindControl("Txt_" + deptid + "_bzx") as TextBox;

                KhXsModel model =xsSrv.GetList().Where(p=>p.DEPTID==deptid).FirstOrDefault();
                if (model != null) model.DB_Option_Action = WebKeys.UpdateAction;
                else
                {
                    model = new KhXsModel();
                    model.DB_Option_Action=WebKeys.InsertAction;
                    model.DEPTID = deptid;
                }
                model.JIGUAN = jiguan.Text.ToInt();
                model.JIGUANX = jiguanx.Text.ToDecimal();
                //model.JIGUANX = model.JIGUAN*0;
                model.YUNXINGJIZU = yunxingjizu.Text.ToInt();
                model.YUNXINGJIZUX = yunxingjizux.Text.ToDecimal();
                //model.YUNXINGJIZUX = (model.YUNXINGJIZU * 0.05).ToDecimal();
                model.REBEIJIZU = rebeijizu.Text.ToInt();
                model.REBEIJIZUX = rebeijizux.Text.ToDecimal();
                //model.REBEIJIZUX = (model.REBEIJIZU * 0.03).ToDecimal();
                model.LENGBEIJIZU = lengbeijizu.Text.ToInt();
                model.LENGBEIJIZUX = lengbeijizux.Text.ToDecimal();
                //model.LENGBEIJIZUX = (model.LENGBEIJIZU * 0.02).ToDecimal();
                model.DAXINGSHUQIZHAN = daxingshuqizhan.Text.ToInt();
                model.DAXINGSHUQIZHANX = daxingshuqizhanx.Text.ToDecimal();
                //model.DAXINGSHUQIZHANX = (model.DAXINGSHUQIZHAN * 0.05).ToDecimal();
                model.XIAOXINGSHUQIZHAN = xiaoxingshuqizhan.Text.ToInt();
                model.XIAOXINGSHUQIZHANX = xiaoxingshuqizhanx.Text.ToDecimal();
                //model.XIAOXINGSHUQIZHANX = (model.XIAOXINGSHUQIZHAN * 0.03).ToDecimal();
                model.GONGLISHU = gonglishu.Text.ToDecimal();
                model.GONGLISHUX = gonglishux.Text.ToDecimal();

                model.DIMAO = dimao.SelectedValue.ToDecimal();
                model.DIMAOX = dimaox.Text.ToDecimal();
                model.CHENGZHENHUA = chengzhenhua.SelectedValue.ToDecimal();
                model.CHENGZHENHUAX = chengzhenhuax.Text.ToDecimal();
                model.FENDUI = fendui.Text.ToInt();
                model.WEIXIUD = weixiudui.Text.ToInt();
                model.WEIXIUDX = weixiuduix.Text.ToDecimal();
                model.ZJXS = zjxs.Text.ToDecimal();
                model.XS = xs.Text.ToDecimal();
                if (dic.ContainsKey(deptid + "")) { 
                    model.XS=dic[deptid+""].ToDecimal();
                }
                model.BZ = bz.Text;
                model.BZX = bzx.Text;
                xsSrv.Execute(model);
            }

            //foreach (GridViewRow gvr in GridView1.Rows)
            //{
            //    //检测是否有效
            //    CheckBox chkbox = gvr.FindControl("CheckBox1") as CheckBox;
            //    if (chkbox.Checked)
            //    {
            //        //主指标或打分指标
            //        //SXID,SXZBBM,ZRSZBID
            //        string SXID = GridView1.DataKeys[gvr.RowIndex].Values["SXID"].ToRequestString();
            //        string SXZBBM = GridView1.DataKeys[gvr.RowIndex].Values["SXZBBM"].ToRequestString();
            //        string ZRSZBID = GridView1.DataKeys[gvr.RowIndex].Values["ZRSZBID"].ToRequestString();
            //        KhZbsxModel model = new KhZbsxModel();
            //        model.DB_Option_Action = WebKeys.InsertOrUpdateAction;
            //        model.KHID = Khid.ToInt();
            //        model.SXID = SXID;
            //        model.SXZBBM = SXZBBM;
            //        model.ZRSZBID = ZRSZBID;
            //        model.SXJGBM = Jgbm;
            //        //权重 4
            //        key = "TxtLhzbQZ" + model.SXZBBM;
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            model.SXQZ = Request.Form[key].ToDecimal() / 100;
            //        }
            //        //分值 5
            //        key = "Txt" + 5 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            model.SXFZ = Request.Form[key].ToDecimal();
            //        }
            //        else
            //        {
            //            key = "TxtLhzb" + 5 + "_" + (gvr.RowIndex + 1);
            //            if (!string.IsNullOrEmpty(Request.Form[key]))
            //            {
            //                model.SXFZ = Request.Form[key].ToDecimal();
            //            }
            //        }
            //        //序号 8
            //        key = "Txt" + 8 + "_" + (gvr.RowIndex + 1);
            //        if (!string.IsNullOrEmpty(Request.Form[key]))
            //        {
            //            model.SXXH = Request.Form[key].ToInt();
            //        }
            //        if (zbsxSrv.Execute(model))
            //        {
            //            mainZbbms.Add(SXZBBM);//保存
            //        }
            //    }            
            //}
            BindGrid();
            Utility.ShowMsg(Page, "提示", "系数保存成功！", 100, "show");
        }


        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            string url = string.Format("KH={0}&BM={1}", Khid, Jgbm);
            Response.Redirect("~/Module/Kh/ZhibiaoJgbmSXList.aspx?" + url, true);
        }

        #endregion


        /// <summary>
        /// 生成指标类型权重的HTML表格
        /// </summary>
        /// <returns></returns>
        protected string GetTableForLhzb()
        {
            if (KhZbsxModelLst == null) 
                return "";
            var lhzbList = KhZbsxModelLst.Where(p => p.SXZBBM.StartsWith("LH")).DistinctBy(p=>p.SXZBBM).ToList();//所有量化指标
            StringBuilder tableSB = new StringBuilder();
            tableSB.Append("<table>");

            tableSB.Append("<tr>");
            tableSB.Append("<th width='180px'>指标类型名称</th><th width='120px'>权重%</th>");
            tableSB.Append("</tr>");
            
            foreach (var zrszb in lhzbList)
            {
                tableSB.Append("<tr>");
                tableSB.Append("<td>" + zrszb.JxzrsZb.Lhzb.Zbxx.ZBMC + "</td>");//名称做为类型用
                tableSB.Append("<td>" +
                        Utility.GetTextBox("TxtLhzbQZ" + zrszb.SXZBBM, (zrszb.SXQZ.ToDecimal() * 100), 0, 0, "number", true,
                                "class=\"easyui-numberbox\" precision=\"1\"", "width:90px;") + "</td>");
                tableSB.Append("</tr>");
            }
            tableSB.Append("</table>");

            return tableSB.ToString();
        }

    }
}