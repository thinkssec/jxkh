using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;
using System.Text;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Zbk
{

    /// <summary>
    /// 指标版本管理页面
    /// </summary>
    public partial class ZbbbManage : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        ZbkBanbenService banbenSrv = new ZbkBanbenService();
        /// <summary>
        /// 考核服务类
        /// </summary>
        KhKhglService kaoheSrv = new KhKhglService();

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
                LnkBtn_Upd.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
                bool isDelete = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Del);
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = isDelete;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        #region 绑定表格

        /// <summary>
        /// 绑定表格
        /// </summary>
        protected void BindGrid()
        {
            List<ZbkBanbenModel> zbbbList = banbenSrv.GetList() as List<ZbkBanbenModel>;
            //指标名称查询
            if (!string.IsNullOrEmpty(Txt_ZBBB_Search.Text))
            {
                zbbbList = zbbbList.Where(p => p.BBMC.Contains(Txt_ZBBB_Search.Text)).ToList();
            }
            GridView1.DataSource = zbbbList;
            GridView1.DataBind();
        }
        #endregion


        #region 隔行换色
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ZbkBanbenModel model = e.Row.DataItem as ZbkBanbenModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                ////序号
                //e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();

                //提取历次考核的信息
                Label lbl = e.Row.FindControl("Label1") as Label;
                lbl.Text = getHistoryKaohe(model.BBMC);
            }
        }
        #endregion


        /// <summary>
        /// 获取查询版本的历次考核信息
        /// </summary>
        /// <param name="zbbb"></param>
        /// <returns></returns>
        private string getHistoryKaohe(string zbbb)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table1\">");
            List<KhKhglModel> khList = kaoheSrv.GetKhListByZbbb(zbbb) as List<KhKhglModel>;
            foreach (var q in khList)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + q.KHMC + "</td>");
                sb.Append("<td>" + q.KSSJ.ToDateYMDFormat() + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }


        #region 保存操作

        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_save_Click(object sender, EventArgs e)
        {
            ZbkBanbenModel model = banbenSrv.GetSingle(Txt_BBMC.Text);
            if (model != null)
            {
                model.DB_Option_Action = WebKeys.UpdateAction;
                model.BBMC = Txt_BBMC.Text;
                if (Chk_QYSJ.Checked)
                {
                    model.QYSJ = DateTime.Now;
                }
                else
                {
                    model.QYSJ = null;
                }
            }
            else
            {
                model = new ZbkBanbenModel();
                model.DB_Option_Action = WebKeys.InsertAction;
                model.BBMC = Txt_BBMC.Text;
                if (Chk_QYSJ.Checked)
                {
                    model.QYSJ = DateTime.Now;
                }
                else
                {
                    model.QYSJ = null;
                }
            }
            //submit
            if (banbenSrv.Execute(model))
            {
                if (!string.IsNullOrEmpty(Ddl_HistoryZbbb.SelectedValue) && 
                    banbenSrv.DeleteZbkDataByBBMC(model.BBMC))
                {
                    //引用以前版本的数据
                    /*
                     1、计算规则配置
                     2、打分指标
                     3、量化指标
                     */
                    ZbkJsgzService jsgzSrv = new ZbkJsgzService();
                    var jsgzLst = jsgzSrv.GetListByBBMC(Ddl_HistoryZbbb.SelectedValue);
                    List<ZbkJsgzModel> jsgzList = new List<ZbkJsgzModel>();
                    foreach (var q in jsgzLst)
                    {
                        q.OLDID = q.GZID;
                        q.GZID = "GZ" + CommonTool.GetPkId();
                        q.BBMC = model.BBMC;
                        q.DB_Option_Action = WebKeys.InsertAction;
                        jsgzSrv.Execute(q);
                        jsgzList.Add(q);
                    }
                    ZbkDfzbService dfzbSrv = new ZbkDfzbService();//打分指标
                    ZbkDfzService dfzSrv = new ZbkDfzService();//打分者
                    ZbkBdfjgService bdfjgSrv = new ZbkBdfjgService();//被打分者
                    var dfzbLst = dfzbSrv.GetListByBBMC(Ddl_HistoryZbbb.SelectedValue);
                    foreach (var q in dfzbLst)
                    {
                        q.OLDID = q.DFZBBM;
                        q.DFZBBM = "DFZB" + CommonTool.GetPkId();
                        q.BBMC = model.BBMC;
                        if (!string.IsNullOrEmpty(q.GZID) && 
                            jsgzList.Exists(p => p.OLDID == q.GZID))
                        {
                            var jsgz = jsgzList.FirstOrDefault(p => p.OLDID == q.GZID);
                            if (jsgz != null)
                                q.GZID = jsgz.GZID;
                        }
                        q.DB_Option_Action = WebKeys.InsertAction;
                        //添加新指标
                        if (dfzbSrv.Execute(q))
                        {
                            //被打分机构
                            foreach (var bdfjg in q.BdfjgLst)
                            {
                                bdfjg.DFZBBM = q.DFZBBM;
                                bdfjg.DB_Option_Action = WebKeys.InsertAction;
                                bdfjgSrv.Execute(bdfjg);
                            }
                            //打分者
                            foreach (var dfz in q.DfzLst)
                            {
                                dfz.DFZBBM = q.DFZBBM;
                                dfz.DB_Option_Action = WebKeys.InsertAction;
                                dfzSrv.Execute(dfz);
                            }
                        }
                    }
                    ZbkLhzbService lhzbSrv = new ZbkLhzbService();//量化指标
                    ZbkMbztbService mbztbSrv = new ZbkMbztbService();//目标值填报
                    ZbkMbzshService mbzshSrv = new ZbkMbzshService();//目标值审核
                    ZbkWcztbService wcztbSrv = new ZbkWcztbService();//完成值填报
                    ZbkWczshdfService wczshdfSrv = new ZbkWczshdfService();//完成值审核
                    var lhzbLst = lhzbSrv.GetListByBBMC(Ddl_HistoryZbbb.SelectedValue)
                        .OrderBy(p => p.PARENTZBBM).ToList();
                    List<ZbkLhzbModel> lhzbList = new List<ZbkLhzbModel>();
                    foreach (var q in lhzbLst)
                    {
                        q.OLDID = q.LHZBBM;
                        q.LHZBBM = "LHZB" + CommonTool.GetPkId();
                        q.BBMC = model.BBMC;
                        //计算规则
                        if (!string.IsNullOrEmpty(q.GZID) &&
                            jsgzList.Exists(p => p.OLDID == q.GZID))
                        {
                            var jsgz = jsgzList.FirstOrDefault(p => p.OLDID == q.GZID);
                            if (jsgz != null)
                                q.GZID = jsgz.GZID;
                        }
                        lhzbList.Add(q);
                        //更新上级指标
                        if (!string.IsNullOrEmpty(q.PARENTZBBM) && 
                            lhzbList.Exists(p => p.OLDID == q.PARENTZBBM))
                        {
                            var lhzb = lhzbList.FirstOrDefault(p => p.OLDID == q.PARENTZBBM);
                            if (lhzb != null)
                            {
                                q.PARENTZBBM = lhzb.LHZBBM;
                            }
                        }
                        q.DB_Option_Action = WebKeys.InsertAction;
                        if (lhzbSrv.Execute(q))
                        {
                            //目标值填报
                            foreach (var mbztb in q.MbztbLst)
                            {
                                mbztb.LHZBBM = q.LHZBBM;
                                mbztb.DB_Option_Action = WebKeys.InsertAction;
                                mbztbSrv.Execute(mbztb);
                            }
                            //目标值审核
                            foreach (var mbzsh in q.MbzshLst)
                            {
                                mbzsh.LHZBBM = q.LHZBBM;
                                mbzsh.DB_Option_Action = WebKeys.InsertAction;
                                mbzshSrv.Execute(mbzsh);
                            }
                            //完成值填报
                            foreach (var wcztb in q.WcztbLst)
                            {
                                wcztb.LHZBBM = q.LHZBBM;
                                wcztb.DB_Option_Action = WebKeys.InsertAction;
                                wcztbSrv.Execute(wcztb);
                            }
                            //完成值审核
                            foreach (var wczshdf in q.WczshdfLst)
                            {
                                wczshdf.LHZBBM = q.LHZBBM;
                                wczshdf.DB_Option_Action = WebKeys.InsertAction;
                                wczshdfSrv.Execute(wczshdf);
                            }
                        }
                    }
                }
                Utility.ShowMsg(Page, "系统提示", "操作成功！", 100, "show");
            }
            clearText();
            BindGrid();
        }

        /// <summary>
        /// 清空内容
        /// </summary>
        private void clearText()
        {
            Txt_BBMC.Text = "";
            Txt_ZBBB_Search.Text = "";
            Pnl_Edit.Visible = false;
        }

        #endregion


        #region 操作事件 编辑/改变状态/删除
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ZbkBanbenModel model = null;
            switch (e.CommandName)
            {
                case "bianji":
                    var list = banbenSrv.GetList();
                    Ddl_HistoryZbbb.Items.Clear();
                    Ddl_HistoryZbbb.Items.Add(new ListItem("", ""));
                    foreach (var q in list)
                    {
                        if (q.BBMC != e.CommandArgument.ToString())
                        {
                            Ddl_HistoryZbbb.Items.Add(new ListItem(q.BBMC, q.BBMC));
                        }
                        else
                        {
                            Txt_BBMC.Text = q.BBMC;
                            Chk_QYSJ.Checked = (q.QYSJ == null) ? false : true;
                            Pnl_Edit.Visible = true;
                        }
                    }
                    break;
                case "shanchu":
                    model = new ZbkBanbenModel();
                    model.DB_Option_Action = WebKeys.DeleteAction;
                    model.BBMC = e.CommandArgument.ToString();
                    banbenSrv.Execute(model);
                    BindGrid();
                    break;
                default:
                    break;
            }
        }
        #endregion


        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            GobackPageUrl("");
        }


        //页面翻页
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;//分页BindData();
            BindGrid();
        }

        /// <summary>
        /// 单位查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Find_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 显示添加界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = true;
            BindGrid();
        }

    }
}