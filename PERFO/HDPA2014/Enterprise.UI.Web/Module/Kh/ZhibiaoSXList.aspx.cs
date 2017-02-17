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
    /// 考核指标筛选维护页面
    /// </summary>
    public partial class ZhibiaoSXList : PageBase
    {

        /// <summary>
        /// 指标筛选-服务类
        /// </summary>
        KhZbsxService zbsxSrv = new KhZbsxService();
        KhKhglService khglSrv = new KhKhglService();//考核管理
        KhJxzrsService jxzrsSrv = new KhJxzrsService();//绩效责任书

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected List<KhZbsxModel> ZbsxList = new List<KhZbsxModel>();//筛选后的指标集合

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
            //1==提取指定考核下的所有筛选指标数据
            ZbsxList = zbsxSrv.GetListByKaohe(Ddl_Kaohe.SelectedValue) as List<KhZbsxModel>;

            //2==根据指定条件提取所有绩效责任书单位
            var khModel = khglSrv.GetSingle(Ddl_Kaohe.SelectedValue);
            if (khModel != null)
            {
                //3==提取考核类型相同的绩效责任书
                var jxzrsLst = jxzrsSrv.GetListByNd_Bmjg_Khlx(khModel.KHND, Ddl_Danwei.SelectedValue, khModel.LXID);
                GridView1.DataSource = jxzrsLst;
                GridView1.DataBind();
            }
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //单位
            Ddl_Danwei.DataSource = bmjgService.GetBmjgTreeLst(false).Where(p => !p.JGLX.Contains("职能")).ToList();
            Ddl_Danwei.DataTextField = "JGMC";
            Ddl_Danwei.DataValueField = "JGBM";
            Ddl_Danwei.DataBind();
            if (Jgbm > 0)
            {
                Ddl_Danwei.SelectedValue = Jgbm.ToString();
            }
            else
            {
                Ddl_Danwei.SelectedValue = userModel.JGBM.ToString();
            }

            //考核期
            var kaohe = khglSrv.GetKhListForValid().Where(p => p.LXID == "LX2014A").ToList();
            khglSrv.BindSSECDropDownListForKaohe(Ddl_Kaohe, kaohe);
            if (!string.IsNullOrEmpty(Khid))
            {
                Ddl_Kaohe.SelectedValue = Khid;
            }
            else if (kaohe.Count > 0) {
                Ddl_Kaohe.SelectedValue = kaohe.First().KHID.ToString();//最近一次考核
            }

            //提示
            Lbl_Msg.Text = "考核指标筛选页面中的操作功能较多且关键！请务必仔细操作！";
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
                KhJxzrsModel model = e.Row.DataItem as KhJxzrsModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //单位名称 1
                e.Row.Cells[1].Text = model.Bmjg.JGMC;
                //机构类型 2
                e.Row.Cells[2].Text = model.Bmjg.JGLX;
                //考核类型 3
                e.Row.Cells[3].Text = model.KhKind.LXMC;
                //考核年度 4
                e.Row.Cells[4].Text = model.SZND.ToRequestString();
                //指标数量 5
                int zbCount = ZbsxList.Count(p => p.SXJGBM == model.JGBM);
                e.Row.Cells[5].Text = ((zbCount == 0) ? string.Format("<font color='red'>{0}</font>", zbCount) : string.Format("<font color='blue'>{0}</font>", zbCount));
                //权重合计 6
                e.Row.Cells[6].Text = ZbsxList.Where(p => p.SXJGBM == model.JGBM && p.JxzrsZb.ZZBXZ == "主指标").Sum(p => p.SXQZ).Value.ToString("P");
                
            }
        }

        /// <summary>
        /// 行命令处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int bm = e.CommandArgument.ToInt();
            switch (e.CommandName)
            {
                case "bianji":
                    if (bm > 0)
                    {
                        //转入修改
                        string url = string.Format("KH={0}&BM={1}", Ddl_Kaohe.SelectedValue, bm);
                        Response.Redirect("~/Module/Kh/ZhibiaoSXEdit.aspx?" + url, true);
                    }
                    break;
                case "shanchu":
                    if (bm > 0)
                    {
                        //删除数据 1=考核数据 2=删除筛选指标
                        if (khglSrv.DeleteAllDataByKhidAndJgbm(Ddl_Kaohe.SelectedValue, bm.ToString()))
                        {
                            zbsxSrv.DeleteByKhidAndJgbm(Ddl_Kaohe.SelectedValue, bm.ToString());
                            Utility.ShowMsg(Page, "系统提示", "删除该单位数据成功!", 100, Utility.MsgShowType.show.ToString());
                        }
                        BindGrid();
                    }
                    break;
                case "append"://追加指标
                    if (bm > 0)
                    {
                        khglSrv.AppendKaoheZhibiao(Ddl_Kaohe.SelectedValue, bm.ToString());
                        Utility.ShowMsg(Page, "系统提示", "该单位追加指标成功!", 100, Utility.MsgShowType.show.ToString());
                    }
                    break;
                case "faqi"://重新发起考核
                    if (bm > 0)
                    {
                        khglSrv.LaunchKaoheByJgbm(Ddl_Kaohe.SelectedValue, bm.ToString());
                        Utility.ShowMsg(Page, "系统提示", "该单位已重新开始考核!", 100, Utility.MsgShowType.show.ToString());
                    }
                    break;
                case "caiwu"://财务基础数据
                    if (bm > 0)
                    {
                        KhCwjcsjService cwjcsjSrv = new KhCwjcsjService();//财务基础数据表
                        var kaohe = khglSrv.GetSingle(Ddl_Kaohe.SelectedValue);
                        if (kaohe != null) {
                            cwjcsjSrv.InitJcsjDataBySzndAndJgbm(kaohe.KHND.ToInt(), bm);
                            cwjcsjSrv.InitJcsjDataBySzndAndJgbm(kaohe.KHND.ToInt() - 1, bm);
                            Utility.ShowMsg(Page, "系统提示", "该单位的基础数据表已成功初始化!", 100, Utility.MsgShowType.show.ToString());
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 显示编辑面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            string url = string.Format("KH={0}&BM={1}", Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue);
            Response.Redirect("~/Module/Kh/ZhibiaoSXEdit.aspx?" + url, true);
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
        /// 考核期切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Kaohe_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 正式发起考核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_FQKH_Click(object sender, EventArgs e)
        {
            khglSrv.LaunchKaohe(Ddl_Kaohe.SelectedValue);
            Utility.ShowMsg(Page, "系统提示", "发起考核成功", 100, Utility.MsgShowType.show.ToString());
        }

        /// <summary>
        /// 追加指标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Append_Click(object sender, EventArgs e)
        {
            khglSrv.AppendKaoheZhibiao(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue);
            Utility.ShowMsg(Page, "系统提示", "追加指标成功!", 100, Utility.MsgShowType.show.ToString());
        }

        /// <summary>
        /// 清除已有考核数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Remove_Click(object sender, EventArgs e)
        {
            khglSrv.DeleteAllDataByKhid(Ddl_Kaohe.SelectedValue);
            Utility.ShowMsg(Page, "系统提示", "清除考核数据成功!", 100, Utility.MsgShowType.show.ToString());
        }

        #endregion


    }
}