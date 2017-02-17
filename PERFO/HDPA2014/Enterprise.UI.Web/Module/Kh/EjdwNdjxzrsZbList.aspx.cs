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
    /// 二级单位年度绩效责任书指标列表页面
    /// </summary>
    public partial class EjdwNdjxzrsZbList : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        KhJxzrszbService jxzrsZbSrv = new KhJxzrszbService();
        KhJxzrsService zrsSrv = new KhJxzrsService();//责任书服务类
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
                //Btn_Add.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
                //bool isUpdate = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
                //GridView1.Columns[GridView1.Columns.Count - 1].Visible = isUpdate;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDdl();
                BindGrid();
                TabTitle = "二级单位";
            }
        }

        #region 专用方法区

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindGrid() 
        {

            //1==提取责任书
            var q = zrsSrv.GetListByNdAndBmjg(Ddl_Niandu.SelectedValue, Ddl_Danwei.SelectedValue);
            if (q != null && q.Count > 0)
            {
                if (q.First().ZRSZT == "1")
                {
                    Lbl_Msg.Text = "该单位的绩效责任书已下达!";
                    GridView1.Visible = true;
                    GridView2.Visible = true;

                    var list = jxzrsZbSrv.GetValidListBySearch(Ddl_Niandu.SelectedValue, Ddl_Danwei.SelectedValue);
                    //二级单位
                    GridView1.DataSource = list.Where(p => p.Lhzb != null ||
                        (p.Dfzb != null && !p.Dfzb.Zbxx.YJZBMC.Contains("加减分"))).ToList();
                    GridView1.DataBind();
                    //领导班子
                    GridView2.DataSource = list.Where(p => p.Lhzb != null ||
                        (p.Dfzb != null && !p.Dfzb.Zbxx.YJZBMC.Contains("加分"))).ToList();
                    GridView2.DataBind();

                    Utility.GroupRows(GridView1, 2);//合并
                    Utility.GroupRows(GridView2, 2);//合并
                }
                else
                {
                    Lbl_Msg.Text = "该单位的绩效责任书还在制定中!";
                    GridView1.Visible = false;
                    GridView2.Visible = false;
                }
            }
            else if (!string.IsNullOrEmpty(Ddl_Danwei.SelectedValue))
            {
                Lbl_Msg.Text = "该单位还未允许制定绩效责任书!";
                GridView1.Visible = false;
                GridView2.Visible = false;
            }
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
                //责任书名称 1
                e.Row.Cells[1].Text = model.Jxzrs.ZRSMC;
                //指标类别 2 指标名称 3
                if (model.Lhzb != null) {
                    e.Row.Cells[2].Text = model.Lhzb.Zbxx.YJZBMC;
                    e.Row.Cells[3].Text = "<a href=\"javascript:showInfo('指标说明','" +
                        model.Lhzb.ZBSM.ToStr() + "<hr/>" + model.Lhzb.PFBZ.ToStr() + "','info');\">" +
                        ((model.ZZBXZ == "辅助指标") ? model.Lhzb.GradeSymbol + model.Lhzb.Zbxx.ZBMC : model.Lhzb.Zbxx.ZBMC) + "</a>";
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
                    e.Row.Cells[3].Text = "<a href=\"javascript:showInfo('指标说明','" +
                        model.Dfzb.PFLX.ToStr() + "<hr/>" + model.Dfzb.PFBZ.ToStr() + "','info');\">" + model.Dfzb.Zbxx.ZBMC + "</a>";
                    //分值 6
                    e.Row.Cells[6].Text = model.ZZBFZ.ToString();
                }
            }
        }

        /// <summary>
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhJxzrszbModel model = e.Row.DataItem as KhJxzrszbModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //责任书名称 1
                e.Row.Cells[1].Text = model.Jxzrs.ZRSMC;
                //指标类别 2 指标名称 3
                if (model.Lhzb != null)
                {
                    e.Row.Cells[2].Text = model.Lhzb.Zbxx.YJZBMC;
                    e.Row.Cells[3].Text = "<a href=\"javascript:showInfo('指标说明','" +
                        model.Lhzb.ZBSM.ToStr() + "<hr/>" + model.Lhzb.PFBZ.ToStr() + "','info');\">" 
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
                    e.Row.Cells[3].Text = "<a href=\"javascript:showInfo('指标说明','" +
                        model.Dfzb.PFLX.ToStr() + "<hr/>" + model.Dfzb.PFBZ.ToStr() + "','info');\">" + model.Dfzb.Zbxx.ZBMC + "</a>";
                    //分值 6
                    e.Row.Cells[6].Text = model.ZZBFZ.ToString();
                }
            }
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

        #endregion

        

    }
}