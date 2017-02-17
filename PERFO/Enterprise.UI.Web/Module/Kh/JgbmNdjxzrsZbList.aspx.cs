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
    /// 机关部门年度绩效责任书指标列表页面
    /// </summary>
    public partial class JgbmNdjxzrsZbList : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        KhJxzrszbService jxzrsZbSrv = new KhJxzrszbService();
        KhJxzrsService zrsSrv = new KhJxzrsService();//责任书服务类
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected string Sznd = (string)Utility.sink("ND", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//所在年度
        protected List<KhJxzrszbModel> JxzrszbList = null;
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
                TabTitle = "机关部门";
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
                    //GridView2.Visible = true;

                    //绩效责任书指标
                    JxzrszbList = jxzrsZbSrv.GetValidListBySearch(Ddl_Niandu.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhJxzrszbModel>;
                    //机关部门
                    GridView1.DataSource = JxzrszbList.Where(p => p.Lhzb != null ||
                        (p.Dfzb != null && !p.Dfzb.Zbxx.YJZBMC.Contains("连带"))).ToList();
                    GridView1.DataBind();
                    //负责人
                    //GridView2.DataSource = JxzrszbList;
                    //GridView2.DataBind();

                    Utility.GroupRows(GridView1, 2);//合并
                    Utility.GroupRows(GridView1, 3);//合并
                    //Utility.GroupRows(GridView2, 2);//合并
                    Utility.GroupRows(GridView1, 3);//合并
                }
                else
                {
                    Lbl_Msg.Text = "该单位的绩效责任书还在制定中!";
                    GridView1.Visible = false;
                    //GridView2.Visible = false;
                }
            }
            else if (!string.IsNullOrEmpty(Ddl_Danwei.SelectedValue))
            {
                Lbl_Msg.Text = "该单位还未允许制定绩效责任书!";
                GridView1.Visible = false;
                //GridView2.Visible = false;
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
            List<SysBmjgModel> bmjgTreeList = bmjgService.GetBmjgTreeLst(false).Where(p => p.XSXH.Length > 2 && !p.JGLX.Contains("基层")).ToList();
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
                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //责任书名称 1
                e.Row.Cells[1].Text = model.Jxzrs.ZRSMC;
                //量化指标
                if (model.Lhzb != null)
                {
                    //序号
                    if (model.LHZBBM != zblx)
                    {
                        zbRowIndex = 1;
                        zblx = model.LHZBBM;
                    }
                    e.Row.Cells[0].Text = (zbRowIndex++).ToString();

                    //考核指标类型与权重2
                    e.Row.Cells[2].Text = model.Lhzb.Zbxx.ZBMC + "<br/>〖" + model.ZZBQZ.Value.ToString("P") + "〗";
                    //考核主要内容3
                    e.Row.Cells[3].Text = "<div style='width: auto;padding: 2px;overflow:auto;'>" + model.JGKHNR + "</div>";
                    //分值4
                    e.Row.Cells[4].Text = model.ZZBFZ.ToRequestString();
                    //考核目标5
                    e.Row.Cells[5].Text = "<div style='width: auto;padding: 2px;overflow:auto;'>" + model.JGKHMB + "</div>";
                    //完成时间6
                    e.Row.Cells[6].Text = "<div style='width: auto;padding: 2px;overflow:auto;'>" + model.JGWCSJ + "</div>";
                    //评分标准7
                    e.Row.Cells[7].Text = "<div style='width: auto;padding: 2px;overflow:auto;'>" + model.JGPFBZ + "</div>";
                }
                //打分指标
                else if (model.Dfzb != null)
                {
                    //序号
                    if (model.Dfzb.Zbxx.YJZBMC != zblx)
                    {
                        zbRowIndex = 1;
                        zblx = model.Dfzb.Zbxx.YJZBMC;
                    }
                    e.Row.Cells[0].Text = (zbRowIndex++).ToString();

                    e.Row.Cells[2].Text = model.Dfzb.Zbxx.YJZBMC;
                    e.Row.Cells[3].Text = model.Dfzb.Zbxx.ZBMC;
                    //分值 4
                    //e.Row.Cells[4].ColumnSpan = 4;
                    e.Row.Cells[4].Text = model.ZZBFZ.ToString();
                    //e.Row.Cells[5].Visible = false;
                    //e.Row.Cells[6].Visible = false;
                    //e.Row.Cells[7].Visible = false;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].ColumnSpan = 3;
                //e.Row.Cells[0].Text = "∑分值合计=〖年度重点工作任务+部门履职〗：";
                e.Row.Cells[0].Text = "∑分值合计=";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                string hjz = JxzrszbList.Where(p => !string.IsNullOrEmpty(p.LHZBBM)).Sum(p => p.ZZBFZ).ToRequestString();
                e.Row.Cells[3].Text = string.Format("<font color='red'>{0}</font>", hjz);
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].ColumnSpan = 4;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
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
                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //责任书名称 1
                e.Row.Cells[1].Text = model.Jxzrs.ZRSMC;
                //量化指标
                if (model.Lhzb != null)
                {
                    //序号
                    if (model.LHZBBM != zblx)
                    {
                        zbRowIndex = 1;
                        zblx = model.LHZBBM;
                    }
                    e.Row.Cells[0].Text = (zbRowIndex++).ToString();

                    //考核指标类型与权重2
                    e.Row.Cells[2].Text = model.Lhzb.Zbxx.ZBMC + "<br/>〖" + model.ZZBQZ.Value.ToString("P") + "〗";
                    //考核主要内容3
                    e.Row.Cells[3].Text = "<div style='width: auto;padding: 2px;overflow:auto;'>" + model.JGKHNR + "</div>";
                    //分值4
                    e.Row.Cells[4].Text = model.ZZBFZ.ToRequestString();
                    //考核目标5
                    e.Row.Cells[5].Text = "<div style='width: auto;padding: 2px;overflow:auto;'>" + model.JGKHMB + "</div>";
                    //完成时间6
                    e.Row.Cells[6].Text = "<div style='width: auto;padding: 2px;overflow:auto;'>" + model.JGWCSJ + "</div>";
                    //评分标准7
                    e.Row.Cells[7].Text = "<div style='width: auto;padding: 2px;overflow:auto;'>" + model.JGPFBZ + "</div>";
                }
                //打分指标
                else if (model.Dfzb != null)
                {
                    //序号
                    if (model.Dfzb.Zbxx.YJZBMC != zblx)
                    {
                        zbRowIndex = 1;
                        zblx = model.Dfzb.Zbxx.YJZBMC;
                    }
                    e.Row.Cells[0].Text = (zbRowIndex++).ToString();

                    e.Row.Cells[2].Text = model.Dfzb.Zbxx.YJZBMC;
                    e.Row.Cells[3].Text = model.Dfzb.Zbxx.ZBMC;
                    //分值 4
                    //e.Row.Cells[4].ColumnSpan = 4;
                    e.Row.Cells[4].Text = model.ZZBFZ.ToString();
                    //e.Row.Cells[5].Visible = false;
                    //e.Row.Cells[6].Visible = false;
                    //e.Row.Cells[7].Visible = false;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].ColumnSpan = 3;
                e.Row.Cells[0].Text = "∑分值合计=〖年度重点工作任务+部门履职〗：";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                string hjz = JxzrszbList.Where(p => !string.IsNullOrEmpty(p.LHZBBM)).Sum(p => p.ZZBFZ).ToRequestString();
                e.Row.Cells[3].Text = string.Format("<font color='red'>{0}</font>", hjz);
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].ColumnSpan = 4;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
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