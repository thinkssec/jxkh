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
    /// 机关部门及负责人考核完成值审定操作页面
    /// </summary>
    public partial class JgbmWczShending : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        KhJgbmkhdfService jgbmkhdfSrv = new KhJgbmkhdfService();//考核得分
        KhKhglService khglSrv = new KhKhglService();//考核管理
        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected KhKhglModel Kaohe = null;//考核期
        protected List<KhJgbmkhdfModel> JgbmkhdfList = null;//考核得分数据集

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
                LnkBtn_InitData.Visible = LnkBtn_Calculate.Visible 
                   = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.View);
                bool isAudit = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Audit);
                LnkBtn_Edit.Visible = isAudit;
            }
            if (e.UserPm != null)
            {
                LnkBtn_InitData.Visible = LnkBtn_Calculate.Visible
                   = Utility.CheckPermission(Convert.ToInt64(e.UserPm.MODULEPERMISSION), (long)WebKeys.PermissionType.View);
                bool isAudit = Utility.CheckPermission(Convert.ToInt64(e.UserPm.MODULEPERMISSION), (long)WebKeys.PermissionType.Audit);
                LnkBtn_Edit.Visible = isAudit;
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
            var list = jgbmkhdfSrv.GetListByKhid(Ddl_Kaohe.SelectedValue);
            if (list.Count == 0)
            {
                //先进行一次数据初始化
                jgbmkhdfSrv.InitJgbmkhdfData(Ddl_Kaohe.SelectedValue);
            }
            else if (!string.IsNullOrEmpty(list.First().HZBZ))
            {
                Lbl_Msg.Text = "本期考核结果已发布了!";
            }
            //数据集
            JgbmkhdfList = jgbmkhdfSrv.GetListByKhid(Ddl_Kaohe.SelectedValue) as List<KhJgbmkhdfModel>;
            //机关部门
            var bmdfLst = JgbmkhdfList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.机关部门).ToString())
                .DistinctBy(p => p.JGBM).OrderBy(p => p.BMPM).ThenBy(p => p.Bmjg.BZ).ToList();
            GridView1.DataSource = bmdfLst;
            GridView1.DataBind();
            if (bmdfLst.Count > 0 && !string.IsNullOrEmpty(bmdfLst.First().HZBZ))
            {
                Lbl_Msg.Text += " 【机关部门】 ";
            }
            //负责人
            var fzrdfLst = JgbmkhdfList.DistinctBy(p => p.JGBM).OrderBy(p => p.FZRPM).
                ThenBy(p => p.Bmjg.BZ).ToList();
            GridView2.DataSource = fzrdfLst;
            GridView2.DataBind();
            if (fzrdfLst.Count > 0 && fzrdfLst.First().HZBZ == "2")
            {
                Lbl_Msg.Text += " 【负责人】 ";
            }
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
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

            Lbl_Msg.Text = "您可以汇总和修改各部门的得分!";
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
                KhJgbmkhdfModel model = e.Row.DataItem as KhJgbmkhdfModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

                //处室名称1
                e.Row.Cells[1].Text = SysBmjgService.GetBmjgName(model.JGBM);
                
                //重点工作完成情况得分2
                var zdgz = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("重点"));
                if (zdgz != null)
                {
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[2].Text = 
                            Utility.GetTextBox("Txt_" + 2 + "_" + (e.Row.RowIndex + 1), zdgz.KHDF, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[2].Text = "<a href=\"#\" onclick=\"showWin('" + zdgz.KHID + "','"
                            + zdgz.JGBM + "','','ZDGZ');\">" + zdgz.KHDF + "</a>";
                    }
                }

                //部门履职考核得分3
                var bmlz = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("部门履职"));
                if (bmlz != null)
                {
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[3].Text =
                            Utility.GetTextBox("Txt_" + 3 + "_" + (e.Row.RowIndex + 1), bmlz.KHDF, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        e.Row.Cells[3].Text = "<a href=\"#\" onclick=\"showWin('" + bmlz.KHID + "','"
                            + bmlz.JGBM + "','','BMLZ');\">" + bmlz.KHDF + "</a>";                        
                    }
                }

                ////机关作风考核得分4
                //机关作风建设加分4
                var jgzfjx = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("机关作风"));
                if (jgzfjx != null)
                {
                    if (Pnl_Edit.Visible)
                    {
                        //e.Row.Cells[4].Text =
                        //    Utility.GetTextBox("Txt_" + 4 + "_" + (e.Row.RowIndex + 1), jgzfjx.BZSM, 0, 0,
                        //    "number", true, "", "width:80px;");
                        e.Row.Cells[4].Text =
                            Utility.GetTextBox("Txt_" + 4 + "_" + (e.Row.RowIndex + 1), jgzfjx.KHDF, 0, 0,
                            "number", true, "", "width:80px;");
                    }
                    else
                    {
                        //e.Row.Cells[4].Text = string.Format("<a href=\"javascript:parent.addTab('/M.K.JgbmJgzfHz?KH={0}','作风建设打分汇总');\">{1}</a>", jgzfjx.KHID, jgzfjx.BZSM);
                        e.Row.Cells[4].Text = string.Format("<a href=\"javascript:parent.addTab('/M.K.JgbmJgzfHz?KH={0}','作风建设打分汇总');\">{1}</a>", jgzfjx.KHID, jgzfjx.KHDF);
                    }
                }

                //费用控制情况 5
                var fykzqk = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("费用控制"));
                if (fykzqk != null)
                {
                    e.Row.Cells[5].Text = fykzqk.KHDF.ToRequestString();
                }

                if (Pnl_Edit.Visible)
                {
                    e.Row.Cells[6].Text =
                        Utility.GetTextBox("Txt_" + 6 + "_" + (e.Row.RowIndex + 1), model.BMZDF, 0, 0,
                        "number", true, "", "width:80px;");
                    e.Row.Cells[7].Text =
                        Utility.GetTextBox("Txt_" + 7 + "_" + (e.Row.RowIndex + 1), model.BMPM, 0, 0,
                        "number", true, "", "width:80px;");
                    e.Row.Cells[8].Text =
                        Utility.GetTextBox("Txt_" + 8 + "_" + (e.Row.RowIndex + 1), model.BMDXBS, 0, 0,
                        "number", true, "", "width:80px;");
                }
                else
                {
                    //最终考核得分6
                    e.Row.Cells[6].Text = model.BMZDF.ToRequestString();
                    //排名 7
                    e.Row.Cells[7].Text = model.BMPM.ToRequestString();
                    //考核兑现系数 8
                    e.Row.Cells[8].Text = model.BMDXBS.ToRequestString();
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "平均分数：";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[0].ColumnSpan = 6;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                var bmdf = JgbmkhdfList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.机关部门).ToString()).First();
                //.DistinctBy(p => p.JGBM).ToList();
                if (Pnl_Edit.Visible)
                {
                    e.Row.Cells[6].Text =
                        Utility.GetTextBox("Txt_BMPJF", bmdf.BMPJF, 0, 0, "number", true, "", "width:80px;");
                }
                else
                {
                    e.Row.Cells[6].Text = bmdf.BMPJF.ToRequestString();//.Average(p=>p.BMZDF).ToRequestString();
                }
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
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
                KhJgbmkhdfModel model = e.Row.DataItem as KhJgbmkhdfModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

                //处室名称1
                e.Row.Cells[1].Text = SysBmjgService.GetBmjgName(model.JGBM);

                //重点工作完成情况得分2
                var zdgz = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("重点"));
                if (zdgz != null)
                {
                    e.Row.Cells[2].Text = zdgz.KHDF.ToRequestString();
                }

                //部门履职考核得分3
                var bmlz = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("部门履职"));
                if (bmlz != null)
                {
                    e.Row.Cells[3].Text = bmlz.KHDF.ToRequestString();
                }

                ////机关作风考核得分4
                //机关作风建设加分4
                var jgzfjx = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("机关作风"));
                if (jgzfjx != null)
                {
                    //e.Row.Cells[4].Text = jgzfjx.BZSM.ToRequestString();
                    e.Row.Cells[4].Text = jgzfjx.KHDF.ToRequestString();
                }

                //费用控制情况 5
                var fykzqk = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("费用控制"));
                if (fykzqk != null)
                {
                    e.Row.Cells[5].Text = fykzqk.KHDF.ToRequestString();
                }

                //部门得分6
                e.Row.Cells[6].Text = model.BMZDF.ToRequestString();
                //连带指标得分7
                //原因说明8
                var ldzb = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("连带"));
                if (ldzb != null)
                {
                    if (Pnl_Edit.Visible)
                    {
                        e.Row.Cells[7].Text =
                            Utility.GetTextBox("TxtFzr_" + 7 + "_" + (e.Row.RowIndex + 1), ldzb.KHDF, 0, 0, 
                            "number", true, "", "width:80px;");
                        e.Row.Cells[8].Text =
                            Utility.GetTextBox("TxtFzr_" + 8 + "_" + (e.Row.RowIndex + 1), ldzb.BZSM, 0, 0,
                            "string", false, "", "width:120px;");
                    }
                    else
                    {
                        e.Row.Cells[7].Text = ldzb.KHDF.ToRequestString();
                        e.Row.Cells[8].Text = "<div style='width: 120px;padding: 2px;overflow:auto;'>" + ldzb.BZSM + "</div>";
                    }
                }

                if (Pnl_Edit.Visible)
                {
                    e.Row.Cells[9].Text =
                        Utility.GetTextBox("TxtFzr_" + 9 + "_" + (e.Row.RowIndex + 1), model.FZRZDF, 0, 0,
                        "number", true, "", "width:80px;");
                    e.Row.Cells[10].Text =
                        Utility.GetTextBox("TxtFzr_" + 10 + "_" + (e.Row.RowIndex + 1), model.FZRPM, 0, 0,
                        "number", true, "", "width:80px;");
                    e.Row.Cells[11].Text =
                        Utility.GetTextBox("TxtFzr_" + 11 + "_" + (e.Row.RowIndex + 1), model.FZRDXBS, 0, 0,
                        "number", true, "", "width:80px;");
                }
                else
                {
                    //最终结果9
                    e.Row.Cells[9].Text = model.FZRZDF.ToRequestString();
                    //排名10
                    e.Row.Cells[10].Text = model.FZRPM.ToRequestString();
                    //兑现倍数11
                    e.Row.Cells[11].Text = model.FZRDXBS.ToRequestString();
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "平均分数：";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[0].ColumnSpan = 9;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                var fzrdf = JgbmkhdfList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.机关部门).ToString()).First();
                //.DistinctBy(p => p.JGBM).ToList();
                if (Pnl_Edit.Visible)
                {
                    e.Row.Cells[9].Text =
                        Utility.GetTextBox("TxtFzr_FZRPJF", fzrdf.FZRPJF, 0, 0, "number", true, "", "width:80px;");
                }
                else
                {
                    e.Row.Cells[9].Text = fzrdf.FZRPJF.ToRequestString();//.Average(p=>p.BMZDF).ToRequestString();
                }
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        /// <summary>
        /// 计算各部门得分并更新到数据表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Calculate_Click(object sender, EventArgs e)
        {
            jgbmkhdfSrv.CalculateJgbmkhdfData(Ddl_Kaohe.SelectedValue);
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
            if (jgbmkhdfSrv.DeleteJgbmkhdfData(Ddl_Kaohe.SelectedValue))
            {
                jgbmkhdfSrv.InitJgbmkhdfData(Ddl_Kaohe.SelectedValue);
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
            string updSql = "update [PERFO_KH_JGBMKHDF] set ";
            //1==部门
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                //KHID,JGBM
                string KHID = GridView1.DataKeys[gvr.RowIndex].Values["KHID"].ToRequestString();
                string JGBM = GridView1.DataKeys[gvr.RowIndex].Values["JGBM"].ToRequestString();
                //重点工作完成情况得分2
                key = "Txt_" + 2 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    string sql = updSql;
                    sql += " KHDF='" + Request.Form[key] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHXMC='{2}';",
                    KHID, JGBM, "年度重点工作任务");
                    sqlSB.Append(sql);
                }
                //部门履职考核得分3
                key = "Txt_" + 3 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    string sql = updSql;
                    sql += " KHDF='" + Request.Form[key] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHXMC='{2}';",
                    KHID, JGBM, "部门履职");
                    sqlSB.Append(sql);
                }
                //机关作风考核得分4 //机关作风建设加分5
                string key4 = "Txt_" + 4 + "_" + (gvr.RowIndex + 1);
                string key5 = "Txt_" + 5 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key4]) && !string.IsNullOrEmpty(Request.Form[key5]))
                {
                    string sql = updSql;
                    sql += " KHDF='" + Request.Form[key5] + "',BZSM='" + Request.Form[key4] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHXMC='{2}';",
                    KHID, JGBM, "机关作风建设");
                    sqlSB.Append(sql);
                }

                //按单位和考核ID更新>>>>>>>>>>>>>>>>>>>>>>>>>
                //最终考核得分6
                key = "Txt_" + 6 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    string sql = updSql;
                    sql += " BMZDF='" + Request.Form[key] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}';",KHID, JGBM);
                    sqlSB.Append(sql);
                }
                //排名 7
                key = "Txt_" + 7 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    string sql = updSql;
                    sql += " BMPM='" + Request.Form[key] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}';", KHID, JGBM);
                    sqlSB.Append(sql);
                }
                //考核兑现系数 8
                key = "Txt_" + 8 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    string sql = updSql;
                    sql += " BMDXBS='" + Request.Form[key] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}';", KHID, JGBM);
                    sqlSB.Append(sql);

                }
                //部门平均分 Txt_BMPJF
                key = "Txt_BMPJF";
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    string sql = updSql;
                    sql += " BMPJF='" + Request.Form[key] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}';", KHID, JGBM);
                    sqlSB.Append(sql);
                }
            }
            
            //2==负责人
            foreach (GridViewRow gvr in GridView2.Rows)
            {
                //KHID,JGBM
                string KHID = GridView1.DataKeys[gvr.RowIndex].Values["KHID"].ToRequestString();
                string JGBM = GridView1.DataKeys[gvr.RowIndex].Values["JGBM"].ToRequestString();

                //连带指标得分7
                key = "TxtFzr_" + 7 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    string sql = updSql;
                    sql += " KHDF='" + Request.Form[key] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHXMC='{2}';",
                    KHID, JGBM, "连带指标");
                    sqlSB.Append(sql);
                }
                //原因说明8
                key = "TxtFzr_" + 8 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    string sql = updSql;
                    sql += " BZSM='" + Request.Form[key] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}' and KHXMC='{2}';",
                    KHID, JGBM, "连带指标");
                    sqlSB.Append(sql);
                }

                //按单位和考核ID更新>>>>>>>>>>>>>>>>>>>>>>>>>
                //最终结果9
                key = "TxtFzr_" + 9 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    string sql = updSql;
                    sql += " FZRZDF='" + Request.Form[key] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}';", KHID, JGBM);
                    sqlSB.Append(sql);
                }
                //排名10
                key = "TxtFzr_" + 10 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    string sql = updSql;
                    sql += " FZRPM='" + Request.Form[key] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}';", KHID, JGBM);
                    sqlSB.Append(sql);
                }
                //兑现倍数11
                key = "TxtFzr_" + 11 + "_" + (gvr.RowIndex + 1);
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    string sql = updSql;
                    sql += " FZRDXBS='" + Request.Form[key] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}';", KHID, JGBM);
                    sqlSB.Append(sql);
                }
                //负责人平均分 
                key = "TxtFzr_FZRPJF";
                if (!string.IsNullOrEmpty(Request.Form[key]))
                {
                    string sql = updSql;
                    sql += " FZRPJF='" + Request.Form[key] + "' ";
                    sql += string.Format(" where KHID='{0}' and JGBM='{1}';", KHID, JGBM);
                    sqlSB.Append(sql);
                }   
            }
            //3==保存数据
            if (jgbmkhdfSrv.ExecuteSQL(sqlSB.ToString()))
            {
                //根据用户选择的发布类型，正式发布考核成绩并锁定
                //更新状态位为HZBZ=1，保存操作人和时间
                string sql = updSql;
                int v = 0;
                foreach (ListItem item in ChkBox_FBLX.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value == "BM") v = 1;
                        else if (item.Value == "FZR") v = 2;
                    }
                }
                if (v > 0)
                {
                    sql += string.Format(" CZR='{0}',TJSJ='{1}',HZBZ='{2}' ", userModel.USERNAME, DateTime.Now.ToDateYMDFormat(), v);
                    sql += string.Format(" where KHID='{0}';", Ddl_Kaohe.SelectedValue);
                    if (jgbmkhdfSrv.ExecuteSQL(sql))
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

    }
}