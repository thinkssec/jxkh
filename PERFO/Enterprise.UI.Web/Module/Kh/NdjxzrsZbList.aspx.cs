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
    /// 年度绩效责任书指标维护页面
    /// </summary>
    public partial class NdjxzrsZbList : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        KhJxzrszbService jxzrsZbSrv = new KhJxzrszbService();
        KhJxzrsService zrsSrv = new KhJxzrsService();

        protected string Bbmc = (string)Utility.sink("BB", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//版本名称
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected string Sznd = (string)Utility.sink("ND", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//所在年度

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
                bool isUpdate = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = isUpdate;
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
            var q = zrsSrv.GetListByNdAndBmjg(Ddl_Niandu.SelectedValue, Ddl_Danwei.SelectedValue).FirstOrDefault();
            if (q != null)
            {
                Lbl_Msg.Text = "";
                if (q.FZBM != userModel.JGBM && userModel.ROLEID != "paadmin")
                {
                    GridView1.Columns[GridView1.Columns.Count - 1].Visible = Btn_Add.Visible = false;
                    Lbl_Msg.Text = "您当前只能查看!";
                }
                else if (q.ZRSZT == "1" && userModel.ROLEID != "paadmin")
                {
                    GridView1.Columns[GridView1.Columns.Count - 1].Visible = Btn_Add.Visible = false;
                    Lbl_Msg.Text = "该单位责任书已下达!不能再修改了!";
                }
                else if (q.ZRSZT == "0")
                {
                    Lbl_Msg.Text = "提示：该单位可以制定绩效指标!";
                }
            }
            else
            {
                Lbl_Msg.Text = "该单位还未制定绩效责任书!";
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = Btn_Add.Visible = false;
            }
            GridView1.DataSource = jxzrsZbSrv.GetListBySearch(Ddl_Niandu.SelectedValue, 
                Ddl_Danwei.SelectedValue, Ddl_Bbmc.SelectedValue);
            GridView1.DataBind();
            Utility.GroupRows(GridView1, 2);//合并

            //add by qw 2014.12.25 按高老师要求，增加一个责任书列表 start
            if (string.IsNullOrEmpty(Ddl_Danwei.SelectedValue))
            {
                //提取所有二级单位的责任书信息
                List<KhJxzrsModel> zrsList = zrsSrv.GetListByNd_Bmjg_Khlx(Ddl_Niandu.SelectedValue, "1", "LX2014A") as List<KhJxzrsModel>;
                GridView2.DataSource = zrsList;
                GridView2.DataBind();
                GridView2.Visible = true;
                Btn_Back.Visible = false;
                Lbl_Msg.Text = "";
            }
            else
            {
                GridView2.Visible = false;
                Btn_Back.Visible = true;
            }
            //end
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //年度
            Ddl_Niandu.Items.Clear();
            for (int i = 2014; i <= DateTime.Now.Year + 1; i++)
            {
                Ddl_Niandu.Items.Add(new ListItem(i + "年", i.ToString()));
            }
            Ddl_Niandu.SelectedValue = (string.IsNullOrEmpty(Sznd)) ? DateTime.Now.Year.ToString() : Sznd;
            
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
            //权限检测
            if (zrsSrv.IsJxzrsFgbm(Ddl_Niandu.SelectedValue, Ddl_Danwei.SelectedValue, userModel) || userModel.ROLEID == "paadmin")
            {
                Btn_Add.Visible = true;
            }
            else if (Ddl_Danwei.SelectedValue == userModel.JGBM.ToString())
            {
                Btn_Add.Visible = true;
                Ddl_Danwei.Enabled = false;//只限于本单位
            }
            else
            {
                Btn_Add.Visible = false;
            }

            //应用版本
            ZbkBanbenService banbenSrv = new ZbkBanbenService();
            Ddl_Bbmc.DataSource = banbenSrv.GetList();
            Ddl_Bbmc.DataTextField = "BBMC";
            Ddl_Bbmc.DataValueField = "BBMC";
            Ddl_Bbmc.DataBind();
            if (!string.IsNullOrEmpty(Bbmc))
                Ddl_Bbmc.SelectedValue = Bbmc;
        }

        #endregion


        #region 事件处理区

        /// <summary>
        /// 返回事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Back_Click(object sender, EventArgs e)
        {
            string url = string.Format("?BB={0}&ND={1}", Ddl_Bbmc.SelectedValue, Ddl_Niandu.SelectedValue);
            GobackPageUrl(url);
        }

        /// <summary>
        /// 责任书列表行绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhJxzrsModel model = e.Row.DataItem as KhJxzrsModel;
                //责任书名称 1
                string pageName = string.Empty;
                if (model.Bmjg.JGLX.Contains("职能"))
                {
                    pageName = "M.K.NdjxzbJgbm";
                }
                else
                {
                    pageName = "M.K.Ndjxzb";
                }
                e.Row.Cells[1].Text =
                    string.Format("<a href='/{3}?BM={0}&ND={1}'>{2}</a>", model.JGBM, model.SZND, model.ZRSMC, pageName);
                //单位名称
                e.Row.Cells[2].Text = model.Bmjg.JGMC;
                //考核类型
                e.Row.Cells[3].Text = (model.KhKind != null) ? model.KhKind.LXMC : "";
                //负责部门
                e.Row.Cells[5].Text = (model.FzBmjg != null) ? model.FzBmjg.JGMC : "";
                //指标数量 6
                e.Row.Cells[6].Text = (model.JxzrszbLst != null) ? model.JxzrszbLst.Count.ToString() : "";
                //考核状态 7
                if (e.Row.Cells[7].Text == "0")
                {
                    e.Row.Cells[7].Text = "<img src=\"/Resources/Images/lock_unlock.png \" title=\"制定中\" alt=\"制定中\"/>";
                }
                else
                {
                    e.Row.Cells[7].Text = "<img src=\"/Resources/Images/lock.png\" title=\"已下达\" alt=\"已下达\"/>";
                }
            }
        }

        /// <summary>
        /// 责任书列表分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex; ;
            BindGrid();
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
                KhJxzrszbModel model = e.Row.DataItem as KhJxzrszbModel;
                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //责任书名称 1
                e.Row.Cells[1].Text = model.Jxzrs.ZRSMC;
                //指标类别 2 指标名称 3
                if (model.Lhzb != null) {
                    e.Row.Cells[2].Text = model.Lhzb.Zbxx.YJZBMC;
                    //e.Row.Cells[3].Text = model.Lhzb.Zbxx.ZBMC;
                    //if (model.ZZBXZ == "辅助指标")
                    //{
                    //    e.Row.Cells[3].Text = model.Lhzb.GradeSymbol + e.Row.Cells[3].Text;
                    //}
                    e.Row.Cells[3].Text = "<a href=\"javascript:showInfo('指标说明','" +
                    model.Lhzb.ZBSM.ToStr() + "<hr/>" + model.Lhzb.PFBZ.ToStr() + "<hr/>" +
                    ((model.Lhzb.Jsgz != null) ? "〖" + model.Lhzb.Jsgz.GZMC + "," + Server.HtmlEncode(model.Lhzb.Jsgz.GZBDS.ToStr()) + "〗" : "") + "<hr/>"
                    + getWczshrInfo(model.Lhzb.WczshdfLst) + "','info');\">"
                    + ((model.ZZBXZ == "辅助指标") ? model.Lhzb.GradeSymbol + model.Lhzb.Zbxx.ZBMC : model.Lhzb.Zbxx.ZBMC) + "</a>";

                    //权重 6
                    e.Row.Cells[6].Text = model.ZZBQZ.Value.ToString("P");
                    //目标值+计算单位 7
                    if (model.ZMBZ != null)
                    {
                        e.Row.Cells[7].Text = model.ZMBZ.Value.ToString("f2") + "(" + model.Lhzb.JSDW + ")";
                    }
                    //目标值说明 8
                    e.Row.Cells[8].Text = "<div style='width: 150px;padding: 2px;overflow:auto;'>" + model.MBZBZ + "</div>";
                    //计算关系式 9
                    e.Row.Cells[9].Text = "<div style='width: 180px;padding: 2px;overflow:auto;'>" + model.ZJSGXS + "</div>";
                }
                else if (model.Dfzb != null)
                {
                    e.Row.Cells[2].Text = model.Dfzb.Zbxx.YJZBMC;
                    e.Row.Cells[3].Text = model.Dfzb.Zbxx.ZBMC;
                    //分值 6
                    e.Row.Cells[6].Text = model.ZZBFZ.ToString();
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
            var model = jxzrsZbSrv.GetSingle(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                case "shanchu":
                    if (model != null)
                    {
                        //删除数据
                        model.DB_Option_Action = WebKeys.DeleteAction;
                        jxzrsZbSrv.DeleteJxzrsZbAndZbsx(model);
                    }
                    BindGrid();
                    break;
                case "up":
                    if (model != null)
                    {
                        upOneGrade(model);
                    }
                    BindGrid();
                    break;
                case "down":
                    if (model != null)
                    {
                        downOneGrade(model);
                    }
                    BindGrid();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Find_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 显示编辑面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            string url = string.Format("BB={0}&BM={1}&ND={2}", 
                Ddl_Bbmc.SelectedValue, Ddl_Danwei.SelectedValue, Ddl_Niandu.SelectedValue);
            Response.Redirect("~/Module/Kh/NdjxzrsZbEdit.aspx?" + url, true);
        }

        /// <summary>
        /// 年度选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Niandu_SelectedIndexChanged(object sender, EventArgs e)
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
        /// 版本选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Bbmc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 上调一级
        /// </summary>
        /// <param name="currM"></param>
        private void upOneGrade(KhJxzrszbModel currM)
        {
            List<KhJxzrszbModel> list = jxzrsZbSrv.GetListBySearch(Ddl_Niandu.SelectedValue,
                Ddl_Danwei.SelectedValue, Ddl_Bbmc.SelectedValue) as List<KhJxzrszbModel>;
            int currIndex = list.FindIndex(p => p.ZRSZBID == currM.ZRSZBID);
            if (currIndex > 0 && list.Count > 1)
            {
                var prevM = list[currIndex - 1];
                if (prevM != null)
                {
                    int xsxh = currM.ZXSXH.ToInt();
                    currM.DB_Option_Action = WebKeys.UpdateAction;
                    currM.ZXSXH = prevM.ZXSXH;
                    jxzrsZbSrv.Execute(currM);
                    prevM.DB_Option_Action = WebKeys.UpdateAction;
                    prevM.ZXSXH = xsxh;
                    jxzrsZbSrv.Execute(prevM);
                }
            }
        }

        /// <summary>
        /// 下调一级
        /// </summary>
        /// <param name="currM"></param>
        private void downOneGrade(KhJxzrszbModel currM)
        {
            List<KhJxzrszbModel> list = jxzrsZbSrv.GetListBySearch(Ddl_Niandu.SelectedValue, 
                Ddl_Danwei.SelectedValue, Ddl_Bbmc.SelectedValue) as List<KhJxzrszbModel>;
            int currIndex = list.FindIndex(p => p.ZRSZBID == currM.ZRSZBID);
            if (currIndex < list.Count-1)
            {
                var nextM = list[currIndex + 1];
                if (nextM != null)
                {
                    int xsxh = currM.ZXSXH.ToInt();
                    currM.DB_Option_Action = WebKeys.UpdateAction;
                    currM.ZXSXH = nextM.ZXSXH;
                    jxzrsZbSrv.Execute(currM);
                    nextM.DB_Option_Action = WebKeys.UpdateAction;
                    nextM.ZXSXH = xsxh;
                    jxzrsZbSrv.Execute(nextM);
                }
            }
        }

        /// <summary>
        /// 返回指标的审核单位名称
        /// </summary>
        /// <param name="shrs"></param>
        /// <returns></returns>
        private string getWczshrInfo(IList<ZbkWczshdfModel> shrs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var s in shrs)
            {
                if (s.OPERATOR.ToInt() > 0)
                {
                    sb.Append(SysBmjgService.GetBmjgName(s.OPERATOR.ToInt()) + "、");
                }
            }
            return sb.ToString().TrimEnd("、".ToCharArray());
        }

        #endregion       

    }
}