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
    ///机关部门目标值录入页面
    /// </summary>
    public partial class JgbmMbzLuru : PageBase
    {

        /// <summary>
        /// 定量指标明细-服务类
        /// </summary>
        KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
        KhKhglService khglSrv = new KhKhglService();//考核管理

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected KhKhglModel Kaohe = null;//考核期

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
            List<KhDlzbmxModel> dlzbList = dlzbmxSrv.GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            if (dlzbList.Count > 0 && dlzbList.Count(p => p.MBZQRRQ == null) == 0)
            {
                Lbl_Msg.Text = "目标值已确认!";
                Pnl_Edit.Visible = false;
                Btn_Add.Visible = false;
            }
            else if (!ChkUserLuruPermission(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue.ToInt(), userModel))
            {
                Lbl_Msg.Text = "您的权限目前为【查看】!";
                Pnl_Edit.Visible = false;
                Btn_Add.Visible = false;
            }
            else
            {
                Lbl_Msg.Text = "";
            }
            GridView1.DataSource = dlzbList;
            GridView1.DataBind();
            Utility.GroupRows(GridView1, 1);
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //单位
            List<SysBmjgModel> parentBmjgLst = bmjgService.GetSameLevelBmjg(4) as List<SysBmjgModel>;
            int[] jgbms = (from c in parentBmjgLst select c.JGBM).ToArray();
            List<SysBmjgModel> bmjgTreeList = bmjgService.GetBmjgTreeLst(false).Where(p => p.XSXH.Length > 2 && !p.JGLX.Contains("二级")).ToList();
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
            var kaohe = khglSrv.GetKhListForValid().Where(p => p.LXID == "LX2014B").ToList();
            khglSrv.BindSSECDropDownListForKaohe(Ddl_Kaohe, kaohe);
            if (!string.IsNullOrEmpty(Khid))
            {
                Ddl_Kaohe.SelectedValue = Khid;
            }
            else if (kaohe.Count > 0)
            {
                Ddl_Kaohe.SelectedValue = kaohe.First().KHID.ToString();//最近一次考核
            }

            Lbl_Msg.Text = "";
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
                KhDlzbmxModel model = e.Row.DataItem as KhDlzbmxModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //指标类别 1
                e.Row.Cells[1].Text = model.LhzbModel.Zbxx.YJZBMC;
                //指标名称 2
                e.Row.Cells[2].Text = "<a href=\"javascript:showInfo('指标说明','" +
                    model.LhzbModel.ZBSM + "<hr/>" + model.LhzbModel.PFBZ + "','info');\">" + model.LhzbModel.Zbxx.ZBMC + "</a>";
                //指标性质 考核权重 3
                e.Row.Cells[3].Text = "〖" + (model.ZbsxModel.SXQZ.ToDecimal() * 100) + "%〗";//model.ZbsxModel.JxzrsZb.ZZBXZ + ;
                //年度目标值 4
                if (model.ZbsxModel.JxzrsZb.ZMBZ != null)
                {
                    e.Row.Cells[4].Text = model.ZbsxModel.JxzrsZb.ZMBZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                }
                else
                {
                    //目标值说明 4
                    e.Row.Cells[4].Text = model.ZbsxModel.JxzrsZb.MBZBZ;
                }
                //考核权重 5
                //e.Row.Cells[5].Text = (model.ZbsxModel.SXQZ.ToDecimal() * 100) + "%";
                //考核目标值 6
                if (model.MBZSQZ != null)
                {
                    e.Row.Cells[6].Text = model.MBZSQZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                }
                //考核目标值说明 7
                e.Row.Cells[7].Text = model.MBZSQBZ;

                //操作权限 8
                Image img = e.Row.FindControl("Image1") as Image;
                if (model.MBZQRRQ == null)
                {
                    img.ImageUrl = "/Resources/Images/lock_unlock.png";
                    img.ToolTip = getShenheInfo(model);
                    //还未审核
                    if (Pnl_Edit.Visible == true)
                    {
                        e.Row.Cells[6].Text = Utility.GetTextBox("Txt" + 6 + "_" + (e.Row.RowIndex + 1),
                                (model.MBZSQZ), 6, (e.Row.RowIndex + 1), "number", true,
                                "class=\"easyui-numberbox\" precision=\"2\"", "width:85px;");
                        e.Row.Cells[7].Text = Utility.GetTextBox("Txt" + 7 + "_" + (e.Row.RowIndex + 1),
                        model.MBZSQBZ, 7, (e.Row.RowIndex + 1), "string", false, "", "width:240px;");
                    }
                }
                else
                {
                    img.ImageUrl = "/Resources/Images/lock.png";
                    img.ToolTip = getShenheInfo(model);
                    //审核已通过
                    e.Row.Cells[6].Text = (model.MBZ != null) ? model.MBZ.Value.ToString("f2") + model.LhzbModel.JSDW : "";
                    e.Row.Cells[7].Text = model.MBZBZ;
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
            string key = string.Empty;
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                //ID
                string ID = GridView1.DataKeys[gvr.RowIndex].Values["ID"].ToRequestString();
                KhDlzbmxModel model = dlzbmxSrv.GetSingle(Ddl_Kaohe.SelectedValue, ID);
                if (model == null) 
                    continue; 
                model.DB_Option_Action = WebKeys.UpdateAction;

                //考核目标值 6
                key = "Txt" + 6 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    model.MBZSQZ = Request.Form[key].ToDecimal();
                }
                //考核目标值说明 7
                key = "Txt" + 7 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    model.MBZSQBZ = Request.Form[key];
                }
                dlzbmxSrv.Execute(model);
            }
            Pnl_Edit.Visible = false;
            BindGrid();
            Utility.ShowMsg(Page, "提示", "保存目标值成功！", 100, "show");
        }

        /// <summary>
        /// 正式提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            List<string> shrs = new List<string>();
            string key = string.Empty;
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                //ID
                string ID = GridView1.DataKeys[gvr.RowIndex].Values["ID"].ToRequestString();
                KhDlzbmxModel model = dlzbmxSrv.GetSingle(Ddl_Kaohe.SelectedValue, ID);
                if (model == null)
                    continue;
                model.DB_Option_Action = WebKeys.UpdateAction;

                //考核目标值 6
                key = "Txt" + 6 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    model.MBZSQZ = Request.Form[key].ToDecimal();
                }
                //考核目标值说明 7
                key = "Txt" + 7 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    model.MBZSQBZ = Request.Form[key];
                }
                //目标值确认人
                if (string.IsNullOrEmpty(model.MBZQRR) || model.MBZQRR=="FGLD")
                {
                    if (model.MBZQRR == "FGLD")
                    {
                        if (userService.GetUserByFgjgbm(model.JGBM.Value) == null)
                        {
                            //无分管领导，则不需要目标值确认
                            model.MBZ = model.MBZSQZ;
                            model.MBZBZ = model.MBZSQBZ;
                            model.MBZQRRQ = DateTime.Now;
                        }
                    }
                    else
                    {
                        //不需要目标值确认
                        model.MBZ = model.MBZSQZ;
                        model.MBZBZ = model.MBZSQBZ;
                        model.MBZQRRQ = DateTime.Now;
                    }
                }
                if (dlzbmxSrv.Execute(model) && model.MBZQRRQ == null)
                {
                    if (!string.IsNullOrEmpty(model.MBZQRR) && 
                        !shrs.Exists(p => p == model.MBZQRR))
                    {
                        shrs.Add(model.MBZQRR);//审核人信息
                    }
                }
            }
            //给目标值审核人发送待办事务-------------------------------------------------------
            if (shrs.Count > 0)
            {
                msgService.SendMsgToUers(shrs, Ddl_Danwei.SelectedValue.ToInt(), Ddl_Kaohe.SelectedValue,
                    "目标值审核", "需要您审核【" + SysBmjgService.GetBmjgName(Ddl_Danwei.SelectedValue.ToInt()) + "】的目标值!",
                    string.Format("/M.K.JgbmMbzSh?KH={0}&BM={1}", Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue),
                    currentModule.MID, Ddl_Danwei.SelectedValue);
            }
            Pnl_Edit.Visible = false;
            BindGrid();
            Utility.ShowMsg(Page, "提示", "已正式提交目标值!请等待审核确认!", 100, "show");
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
            Lbl_Msg.Text = "";
            BindGrid();
        }

        /// <summary>
        /// 单位选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Danwei_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lbl_Msg.Text = "";
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
            Lbl_Msg.Text = "";
            BindGrid();
        }

        #endregion


        #region 专用方法区

        /// <summary>
        /// 获取目标值审核信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string getShenheInfo(KhDlzbmxModel model)
        {
            string shr = "";
            if (!string.IsNullOrEmpty(model.MBZQRR))
            {

                switch (model.MBZQRR)
                {
                    case "YQTLD"://油气田领导
                        shr = "目标值审核人：油气田领导";
                        break;
                    case "FGLD"://分管领导
                        shr = "目标值审核人：分管领导";
                        break;
                    default:
                        shr = "目标值审核人：" + SysBmjgService.GetBmjgName(model.MBZQRR.ToInt());
                        break;
                }
            }
            return shr;
        }

        #endregion


    }
}