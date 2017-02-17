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
    ///二级单位目标值审核页面
    /// </summary>
    public partial class EjdwMbzShenhe : PageBase
    {

        /// <summary>
        /// 定量指标明细-服务类
        /// </summary>
        KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
        KhJgbmdfbService jgbmdfSrv = new KhJgbmdfbService();//机关部门打分表
        KhKhglService khglSrv = new KhKhglService();//考核管理

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected KhKhglModel Kaohe = null;//考核期
        /// <summary>
        /// 网页参数
        /// </summary>
        public string UrlParam
        {
            get
            {
                string url = string.Format("KH={0}&BM={1}", Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue);
                return url;
            }
        }

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
                Btn_Add.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Audit);
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
            if (!ChkUserAuditPermission(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue.ToInt(), userModel))
            {
                Lbl_Msg.Text = "您的权限目前为【查看】!";
                Pnl_Edit.Visible = false;
                Btn_Add.Visible = false;
                Btn_Jcsj.Visible = false;
            }
            else
            {
                Lbl_Msg.Text = "";
            }
            //1==提取单位考核指标
            List<KhDlzbmxModel> dlzbList = dlzbmxSrv.GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            GridView1.DataSource = dlzbList;
            GridView1.DataBind();
            Utility.GroupRows(GridView1, 1);
            if (dlzbList.Count > 0)
            {
                if (dlzbList.Count(p => p.WCZSQRQ == null) == 0)
                {
                    Lbl_Msg.Text = "该单位已录入完成值!不能再修改目标值了!";
                    Pnl_Edit.Visible = false;
                    Btn_Add.Visible = false;
                    Btn_Jcsj.Visible = false;
                }
                else if (dlzbList.Count(p => p.MBZQRRQ == null) == 0)
                {
                    Lbl_Msg.Text = "目标值已审核!";
                }
            }

            //2==提取当前用户的所有测评单位
            List<KhJgbmdfbModel> myDfDanWeiLst =
                jgbmdfSrv.GetListByKhidAndDfz(Ddl_Kaohe.SelectedValue, userModel).
                Where(p => !string.IsNullOrEmpty(p.ID)).ToList();
            var oneBmgjList = myDfDanWeiLst.DistinctBy(p => p.JGBM).ToList();
            GridView2.DataSource = oneBmgjList;
            GridView2.DataBind();
            //3==友情提示
            if (string.IsNullOrEmpty(Lbl_Msg.Text))
                Lbl_Msg.Text += "&nbsp;&nbsp;友情提示：您可以审核的单位数量为〖" + oneBmgjList.Count() + "〗";
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
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
        }

        #endregion


        #region 事件处理区

        /// <summary>
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhJgbmdfbModel model = e.Row.DataItem as KhJgbmdfbModel;
                //单位名称 1
                string url = string.Format("KH={0}&BM={1}", model.KHID, model.JGBM);
                string bmmc = SysBmjgService.GetBmjgName(model.JGBM.Value);
                e.Row.Cells[1].Text = string.Format("<a href='{0}'>{1}</a>",
                    GetPageUrlAndParams("?" + url).TrimStart("~".ToCharArray()), bmmc);
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
                KhDlzbmxModel model = e.Row.DataItem as KhDlzbmxModel;
                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //指标类别 1
                e.Row.Cells[1].Text = model.LhzbModel.Zbxx.YJZBMC;
                //指标名称 2
                e.Row.Cells[2].Text = "<a href=\"javascript:showInfo('指标说明','" +
                    model.LhzbModel.ZBSM.ToStr() + "<hr/>" + model.LhzbModel.PFBZ.ToStr() + "','info');\">"
                    + ((model.ZbsxModel.JxzrsZb.ZZBXZ == "辅助指标") ? model.LhzbModel.GradeSymbol + model.LhzbModel.Zbxx.ZBMC : model.LhzbModel.Zbxx.ZBMC) + "</a>";
                //指标性质 考核权重 3
                e.Row.Cells[3].Text = model.ZbsxModel.JxzrsZb.ZZBXZ + "<br/>〖" + 
                    (model.ZbsxModel.SXQZ.ToDecimal() * 100).ToString("f1") + "%〗";
                //年度目标值 4
                if (model.ZbsxModel.JxzrsZb.ZMBZ != null)
                {
                    e.Row.Cells[4].Text = model.ZbsxModel.JxzrsZb.ZMBZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                }
                else
                {
                    //目标值说明 4
                    e.Row.Cells[4].Text = "<div style='width: 150px;padding: 2px;overflow:auto;'>" 
                        + model.ZbsxModel.JxzrsZb.MBZBZ +"</div>";
                }
                //申请目标值 5
                if (model.MBZSQZ != null)
                {
                    e.Row.Cells[5].Text = model.MBZSQZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                }
                //申请说明 6
                e.Row.Cells[6].Text = model.MBZSQBZ;

                //操作权限 9
                Image img = e.Row.FindControl("Image1") as Image;
                
                //检测当前用户权限
                if (IsShr(model, model.MBZQRR, userModel))
                {
                    img.ImageUrl = "/Resources/Images/yes.png";//有权限
                    if (Pnl_Edit.Visible)
                    {
                        if (model.MBZ == null) model.MBZ = model.MBZSQZ;
                        if (string.IsNullOrEmpty(model.MBZBZ)) model.MBZBZ = model.MBZSQBZ;
                        //审核目标值 7  
                        e.Row.Cells[7].Text = Utility.GetTextBox("Txt" + 7 + "_" + (e.Row.RowIndex + 1),
                                (model.MBZ), 7, (e.Row.RowIndex + 1), "number", true,
                                "class=\"easyui-numberbox\" precision=\"2\"", "width:85px;");
                        //审核说明 8
                        e.Row.Cells[8].Text = Utility.GetTextBox("Txt" + 8 + "_" + (e.Row.RowIndex + 1),
                        model.MBZBZ, 8, (e.Row.RowIndex + 1), "string", false, "", "width:240px;");
                    }
                    else
                    {
                        if (model.MBZ != null)
                        {
                            e.Row.Cells[7].Text = model.MBZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                        }
                        e.Row.Cells[8].Text = model.MBZBZ;
                    }
                }
                else
                {
                    img.ImageUrl = "/Resources/Images/no.png";//无权限
                    if (model.MBZ != null)
                    {
                        e.Row.Cells[7].Text = model.MBZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                    }
                    e.Row.Cells[8].Text = model.MBZBZ;
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
            
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            bool isOk = true;
            string key = string.Empty;
            List<KhDlzbmxModel> dlzbList = dlzbmxSrv.GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                //ID
                string ID = GridView1.DataKeys[gvr.RowIndex].Values["ID"].ToRequestString();
                KhDlzbmxModel model = dlzbList.FirstOrDefault(p => p.ID == ID);//dlzbmxSrv.GetSingle(ID);
                if (model == null) 
                    continue; 
                model.DB_Option_Action = WebKeys.UpdateAction;
                //审核目标值 7
                key = "Txt" + 7 + "_" + (gvr.RowIndex + 1);
                model.MBZ = Request.Form[key].ToNullOrDecimal();
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                //说明 8
                key = "Txt" + 8 + "_" + (gvr.RowIndex + 1);
                model.MBZBZ = Request.Form[key];
                //if (!string.IsNullOrEmpty(Request.Form[key]))
                //{
                    
                //}
                model.MBZQRRQ = DateTime.Now;
                if (!dlzbmxSrv.Execute(model))
                {
                    isOk = false;
                }
            }
            if (isOk)
            {
                //给相应单位发送待办消息，提示其录入完成值
                var u = userService.GetUserListForValid().
                    FirstOrDefault(p => p.JGBM.ToString() == Ddl_Danwei.SelectedValue && p.DUTY.Contains("员工"));
                if (u != null)
                {
                    //string loginId, int jgbm, string dbmc, string dbsm, string url, string mid, string fsr
                    msgService.SendMsgToUers(u.LOGINID, 
                        Ddl_Danwei.SelectedValue.ToInt(), 
                        "完成值录入", 
                        "【" + Ddl_Kaohe.SelectedItem.Text + "】需要您录入【"
                        + SysBmjgService.GetBmjgName(Ddl_Danwei.SelectedValue.ToInt()) + "】的完成值!",
                        string.Format("/M.K.WczLuru?KH={0}&BM={1}", Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue),
                        "1013", 
                        Ddl_Danwei.SelectedValue);
                }
                //关闭消息
                if (!string.IsNullOrEmpty(MSGID))
                {
                    msgService.CloseMessage(MSGID);
                }
                else
                {
                    msgService.CloseMessage(userModel, Ddl_Kaohe.SelectedValue.ToInt(), "1011", Ddl_Danwei.SelectedValue);
                }
            }
            Utility.ShowMsg(Page, "系统提示", "目标值审核完成！", 100, "show");
            Pnl_Edit.Visible = false;
            BindGrid();
        }
        
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            string url = string.Format("KH={0}&BM={1}", Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue);
            GobackPageUrl("?" + url);
        }

        /// <summary>
        /// 考核期选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Kaohe_SelectedIndexChanged(object sender, EventArgs e)
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
        /// 编辑操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = true;
            BindGrid();
        }

        /// <summary>
        /// 审核基础数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Upd_Click(object sender, EventArgs e)
        {
            string url = string.Format("KH={0}&BM={1}", Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue);
            string backUrl = PageUrl.TrimStart('~') + "?" + url;
            backUrl = Base64.Base64Encode(backUrl);
            Response.Redirect("/M.K.EjdwJcsjData?BackUrl=" + backUrl + "&" + url, true);
        }

        #endregion

        
    }
}