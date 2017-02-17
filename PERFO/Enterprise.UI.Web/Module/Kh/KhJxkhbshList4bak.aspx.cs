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
using Enterprise.Service.Perfo.Khs;
using Enterprise.Model.Perfo.Khs;

using Enterprise.Component.Infrastructure;
using System.Text;

namespace Enterprise.UI.Web.Kh
{

    /// <summary>
    ///机关部门指标筛选维护页面
    /// </summary>
    public partial class KhJxkhbshList4bak : PageBase
    {

        /// <summary>
        /// 指标筛选-服务类
        /// </summary>
        KhZbsxService zbsxSrv = new KhZbsxService();
        KhKhglService khglSrv = new KhKhglService();//考核管理
        public KhsMbjgService mbjgSrv = new KhsMbjgService();
        KhJxzrszbService jxzrsZbSrv = new KhJxzrszbService();//责效责任书指标服务类
        KhJxzrsService zrsSrv = new KhJxzrsService();//责任书服务类
        ZbkLhzbService lhzbSrv = new ZbkLhzbService();//量化指标
        ZbkDfzbService dfzbSrv = new ZbkDfzbService();//打分指标
        public List<SysBmjgModel> jgL = new List<SysBmjgModel>();
        public List<KhsKhdzbModel> dzbL = new List<KhsKhdzbModel>();
        public Dictionary<string, Decimal> dic = new Dictionary<string, decimal>();
        public Dictionary<string, string> dic2 = new Dictionary<string, string>();

        public Dictionary<string, string> dic3 = new Dictionary<string, string>();
        protected string Khid = (string)Utility.sink("KHID", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        //protected string glc = (string)Utility.sink("glcqz", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected int dwdm = (int)Utility.sink("DWDM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected KhKhglModel Kaohe = null;
        protected List<KhZbsxModel> KhZbsxModelLst = null;//合成后的指标筛选集合
        public Dictionary<string,decimal> glc = new  Dictionary<string,decimal>();
        public Dictionary<string, decimal> wqx = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> jt = new  Dictionary<string,decimal>();
        public Dictionary<string, decimal> ysj = new  Dictionary<string,decimal>();
        public Dictionary<string, string> mbjg = new Dictionary<string, string>();
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
                //LnkBtn_Ins.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
               // LinkButton1.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Hid_KHID.Value = Khid;
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
            SysBmjgService jgSrv = new SysBmjgService();
            KhsKhdzbService dzbkhSrv = new KhsKhdzbService();
            KhKhglService khSrv = new KhKhglService();
            KhsDzbService dzbSrv = new KhsDzbService();
            KhsQzService qzSrv = new KhsQzService();
            jgL = jgSrv.GetList().Where(p => p.JGLX == "职能部门" && p.SFKH == "1" && p.JGBM == dwdm).OrderBy(o => o.PX).ToList();
            dzbL = dzbkhSrv.GetList().Where(p => p.KHID == Khid.ToInt()).OrderBy(o => o.PX).ToList();
//            string sql = string.Format(@"select c.id,
//                                           (select ZBMC from perfo_khs_dzb where dzbid = c.dzbid) name,
//                                           d.jgbm,
//                                           sum(score) score
//                                      from PERFO_KHS_KHDZB c
//                                      left join perfo_khx_mb b
//                                        on b.khdzbid = c.id
//                                      left join perfo_khs_mbjg d
//                                        on b.id = d.mbid
//                                      left join PERFO_KHX_RESULT a
//                                        on a.DWMBID = d.id
//                                     where c.khid = "+Khid+" group by c.id, d.jgbm, c.px, c.dzbid order by c.px");

            string sql = string.Format(@"select * from VW_SCORE where khid=" + Khid + " and jgbm=" + dwdm);
            DataSet ds = khSrv.getDsBySql(sql);
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                //dic.Add(r["ID"].ToRequestString() + r["JGBM"].ToRequestString(), r["SCORE"].ToDecimal());

                if (r["JGBM"].ToRequestString() == r["FZBM"].ToRequestString())
                    dic.Add(r["ID"].ToRequestString() + r["JGBM"].ToRequestString(), r["AVGSCORE"].ToDecimal());
                else
                    dic.Add(r["ID"].ToRequestString() + r["JGBM"].ToRequestString(), r["SCORE"].ToDecimal());
            }
            List<KhsDzbModel> dzb = new List<KhsDzbModel>();
            dzb = dzbSrv.GetList().ToList();
            foreach (var m in dzb) {
                dic2.Add(m.DZBID,m.ZBMC);
            }

            sql = @"select t.khdzbid,t.id
                      from PERFO_KHS_MBJG t, perfo_kh_khgl a, perfo_khs_khdzb b
                     where a.khid = b.khid
                     and b.id=t.khdzbid
                     and a.khid="+Khid+" and t.jgbm="+dwdm;
            ds = khSrv.getDsBySql(sql);
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                dic3.Add(r["KHDZBID"].ToRequestString(), r["ID"].ToRequestString());
            }

            var l1 = qzSrv.GetList().Where(p => p.KHID == Hid_KHID.Value.ToInt() && p.JGBM == 132).ToList();
            foreach (var m in l1)
            {
                glc.Add(m.KHDZBID, m.QZ == null ? 0 : m.QZ.Value);
            }
            sql = "select t.* from PERFO_KHS_MBJG t,perfo_khs_khdzb a,perfo_kh_khgl b where b.khid=" + Khid + " and a.khid=b.khid and t.khdzbid=a.id and t.jgbm=" + dwdm;
            List<KhsMbjgModel> mbLs = mbjgSrv.GetListBySQL(sql).ToList();
            foreach (var m in mbLs)
            {
                mbjg.Add(m.KHDZBID, m.STATUS);
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

            string[] glcqz=Request["glcqz"].ToRequestString().Split(',');
            string[] wqxqz = Request["wqxqz"].ToRequestString().Split(',');
            string[] jtqz = Request["jtqz"].ToRequestString().Split(',');
            string[] ysjqz = Request["ysjqz"].ToRequestString().Split(',');
            SysBmjgService jgSrv = new SysBmjgService();
            KhsKhdzbService dzbkhSrv = new KhsKhdzbService();
            KhsQzService qzSrv=new KhsQzService();
            jgL = jgSrv.GetList().Where(p => p.JGLX == "职能部门" && p.SFKH == "1").OrderBy(o => o.PX).ToList();
            dzbL = dzbkhSrv.GetList().Where(p => p.KHID == Khid.ToInt()).OrderBy(o => o.PX).ToList();
            int j = 0;
            foreach (var m in dzbL) {
                qzSrv.DeleteById(m.ID);
                int i = 0;
                foreach (var jg in jgL) {
                    KhsQzModel n = new KhsQzModel();
                    n.QZID = Guid.NewGuid().ToRequestString();
                    n.KHDZBID = m.ID;
                    n.JGBM = jg.JGBM;
                    n.KHID = Hid_KHID.Value.ToInt();
                    n.DB_Option_Action = WebKeys.InsertAction;
                    if (!string.IsNullOrEmpty(glcqz[j]))
                        n.QZ = glcqz[j].ToDecimal();
                   
                    qzSrv.Execute(n);
                    i++;
                }
                j++;
            }

            BindGrid();
            Utility.ShowMsg(Page, "提示", "系数保存成功！", 100, "show");
        }
        protected void Btn_No_Click(object sender, EventArgs e)
        {
            KhsMbjgService mbjgSrv = new KhsMbjgService();
            List<string> user = new List<string>();
            SysUserService usrv = new SysUserService();
            KhKhglModel khm = khglSrv.GetList().Where(p => p.KHID == Khid.ToInt()).FirstOrDefault();
            string[] cbs = Hid_CB.Value.Split(',');
            foreach (var cb in cbs)
            {
                if (string.IsNullOrEmpty(cb)) continue;
                string str_Id = cb;
                KhsMbjgModel md = mbjgSrv.GetList().Where(p => p.ID == str_Id).FirstOrDefault();
                md.STATUS = "42";
                md.DB_Option_Action = WebKeys.UpdateAction;
                if (mbjgSrv.Execute(md))
                {
                    //List<SysUserModel> s = usrv.GetList().Where(p => p.DUTY == "基层机关").ToList();
                    //if (s != null)
                    //{
                    //    foreach (var u in s)
                    //    {
                    //        user.Add(u.LOGINID);
                    //    }
                    //}
                }
            }
            BindGrid();
        }
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            //string url = string.Format("BB={0}&BM={1}&ND={2}", Bbmc, Jgbm, Sznd);
            Response.Redirect("~/Module/Kh/KhJgfgshList.aspx?KHID="+Khid, true);
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
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