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
using Enterprise.Model.Perfo.Sys;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Sys
{
    /// <summary>
    /// 模块管理页面
    /// </summary>
    public partial class ModuleManage : PageBase
    {

        /// <summary>
        /// 模块服务类
        /// </summary>
        SysModuleService moduleSrv = new SysModuleService();

        /// <summary>
        /// 模块数据集合
        /// </summary>
        List<SysModuleModel> moduleList = new List<SysModuleModel>();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定数据列表
                clearText();
                BindGrid();
            }
        }

        /// <summary>
        /// 所属模块下拉控件
        /// </summary>
        /// <param name="root"></param>
        protected void Bindddl(string root)
        {
            ListItem list;
            List<SysModuleModel> parentLst = moduleList.Where(p => p.MPARENTID == root).ToList();
            foreach (SysModuleModel module in parentLst)
            {
                list = new ListItem();
                list.Text = module.MNAME;
                list.Value = module.MID;
                if (list.Value.Length > 2)
                {
                    for (int i = 0; i < list.Value.Length / 2; i++)
                    {
                        list.Text = "﹄" + list.Text;
                    }
                }
                Ddl_MPARENTID.Items.Add(list);
                Bindddl(list.Value);
            }
        }


        /// <summary>
        /// 绑定数据列表
        /// </summary>
        protected void BindGrid()
        {
            //1=绑定数据列表
            moduleList = moduleSrv.GetList().OrderBy(p => p.MID).ThenBy(p => p.XSXH).ToList();
            GridView1.DataSource = moduleList;
            GridView1.DataBind();

            //2=绑定当前模块下拉列表
            Ddl_MPARENTID.Items.Clear();
            ListItem list = new ListItem();
            list.Text = "@-" + "根节点";
            list.Value = "0";
            Ddl_MPARENTID.Items.Add(list);
            Bindddl("0");
        }

        /// <summary>
        /// 页索引变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        /// <summary>
        /// 数据行绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string moduleid = e.Row.Cells[1].Text;
                if (moduleid.Length > 2)
                {
                    for (int i = 0; i < moduleid.Length / 2; i++)
                    {
                        e.Row.Cells[2].Text = "═" + e.Row.Cells[2].Text;
                    }
                    e.Row.Cells[2].Text = "╘" + e.Row.Cells[2].Text;
                }

                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#dddddd'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
            }
        }


        /// <summary>
        /// 选择行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectId = GridView1.SelectedRow.Cells[1].Text;
            SysModuleModel zhcnModel = moduleSrv.GetSingle(selectId);
            if (zhcnModel != null)
            {
                CommonTool.SetModelDataToForm(zhcnModel, Page, "Txt_", true);
                CommonTool.SetModelDataToForm(zhcnModel, Page, "Ddl_", true);
                Hid_MID.Value = zhcnModel.MID;
                Chk_DISABLE.Checked = (zhcnModel.DISABLE != "0") ? true : false;
                Pnl_Edit.Visible = true;
            }
            //不允许变更节点
            Ddl_MPARENTID.Enabled = false;
            LnkBtn_Ins.Visible = false;
            LnkBtn_Upd.Visible = true;
            LnkBtn_Del.Visible = true;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            //中文
            SysModuleModel zhcnModel = (SysModuleModel)CommonTool.GetFormDataToModel(typeof(SysModuleModel), Page);
            zhcnModel.DB_Option_Action = WebKeys.InsertAction;
            zhcnModel.MID = getModuleId(zhcnModel.MPARENTID);
            zhcnModel.DISABLE = (Chk_DISABLE.Checked) ? "1" : "0";
            //add
            moduleSrv.Execute(zhcnModel);

            clearText();
            BindGrid();
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Hid_MID.Value))
            {
                //中文
                SysModuleModel zhcnModel = (SysModuleModel)CommonTool.GetFormDataToModel(typeof(SysModuleModel), Page);
                zhcnModel.DB_Option_Action = WebKeys.UpdateAction;
                zhcnModel.MID = Hid_MID.Value;
                zhcnModel.DISABLE = (Chk_DISABLE.Checked) ? "1" : "0";
                //update
                moduleSrv.Execute(zhcnModel);
            }

            clearText();
            BindGrid();
            //重新加载路由表
            Global.LoadUrlRoute();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Del_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Hid_MID.Value))
            {
                SysModuleModel model = new SysModuleModel();
                model.MID = Hid_MID.Value;
                model.DB_Option_Action = WebKeys.DeleteAction;
                moduleSrv.Execute(model);
            }
            clearText();
            BindGrid();
            //重新加载路由表
            Global.LoadUrlRoute();
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            if (currentModule != null)
            {
                string urlPrefix = "~/";
                if (!string.IsNullOrEmpty(currentModule.WEBURL.Trim()))
                {
                    Response.Redirect(urlPrefix + currentModule.WEBURL.TrimStart(urlPrefix.ToCharArray()));
                }
                else
                {
                    Response.Redirect(urlPrefix + currentModule.MURL.TrimStart(urlPrefix.ToCharArray()));
                }
            }
        }


        #region 私有方法

        /// <summary>
        /// 生成MODULE的ID
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private string getModuleId(string parentId)
        {
            string moduleId = string.Empty;
            int amount = moduleSrv.GetModuleAmount(parentId);
            //统一加10
            amount = amount + 10;
            if (parentId == "0")
            {
                moduleId = amount.ToString();
            }
            else
            {
                moduleId = parentId + amount.ToString();
            }
            return moduleId;
        }

        /// <summary>
        /// 清空输入框内容
        /// </summary>
        private void clearText()
        {
            Txt_XSXH.Text = "";
            Txt_BZ.Text = "";
            Chk_DISABLE.Checked = false;
            Txt_MURL.Text = "";
            Txt_WEBURL.Text = "";
            Ddl_MPARENTID.Enabled = true;
            Txt_MNAME.Text = "";
            Hid_MID.Value = "";
            LnkBtn_Ins.Visible = true;
            LnkBtn_Upd.Visible = false;
            LnkBtn_Del.Visible = false;
            Pnl_Edit.Visible = false;
        }

        #endregion

        /// <summary>
        /// 显示编辑面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            clearText();
            Pnl_Edit.Visible = true;
            Ddl_MPARENTID.Enabled = true;
            BindGrid();
        }

        


    }
}
