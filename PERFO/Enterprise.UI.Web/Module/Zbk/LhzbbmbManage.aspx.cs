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
using Enterprise.Model.Perfo.Sys;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Zbk
{
    /// <summary>
    /// 量化指标维护页面
    /// </summary>
    public partial class LhzbbmbManage : PageBase
    {

        /// <summary>
        /// 指标信息服务类
        /// </summary>
        ZbkZbxxService zbxxSrv = new ZbkZbxxService();
        /// <summary>
        /// 量化指标服务类
        /// </summary>
        ZbkLhzbService lhzbSrv = new ZbkLhzbService();
        /// <summary>
        /// 计算规则配置服务类
        /// </summary>
        ZbkJsgzService jsgzSrv = new ZbkJsgzService();
        /// <summary>
        /// 版本服务类
        /// </summary>
        ZbkBanbenService banbenSrv = new ZbkBanbenService();
        /// <summary>
        /// 量化指标集合
        /// </summary>
        List<ZbkLhzbModel> lhzbList = null;

        ZbkMbztbService mbztbSrv = new ZbkMbztbService();
        ZbkMbzshService mbzshSrv = new ZbkMbzshService();
        ZbkWcztbService wcztbSrv = new ZbkWcztbService();
        ZbkWczshdfService wczshdfSrv = new ZbkWczshdfService();
        /// <summary>
        /// 序号集合
        /// </summary>
        List<ZbkLhzbModel> xhList = new List<ZbkLhzbModel>();
        protected string Bbmc = (string)Utility.sink("BB", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//版本名称
        protected string Jgbm = (string)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//机构编码
        protected string UpLhzbbm = (string)Utility.sink("UP", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//上调
        protected string DownLhzbbm = (string)Utility.sink("DOWN", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//下调

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
                LnkBtn_Del.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Del);
            }
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                Clear();
                BindDDL();
            }
        }

        #region 绑定下拉菜单

        protected void BindDDL()
        {
            var bmjgLst = bmjgService.GetSameLevelBmjg(6);
            //被考核单位
            bmjgService.BindSSECDropDownListForBmjg(Ddl_Danwei, bmjgLst);
            if (!string.IsNullOrEmpty(Jgbm))
            {
                Ddl_Danwei.SelectedValue = Jgbm;
            }

            //应用版本
            Ddl_BBMC.DataSource = banbenSrv.GetList();
            Ddl_BBMC.DataTextField = "BBMC";
            Ddl_BBMC.DataValueField = "BBMC";
            Ddl_BBMC.DataBind();
            if (!string.IsNullOrEmpty(Bbmc))
            {
                Ddl_BBMC.SelectedValue = Bbmc;
            }

            var znbm_ejdw = bmjgService.GetSameLevelBmjg(6);
            //目标值填报
            cb_mb1.DataSource = znbm_ejdw.Where(p => p.JGLX == "基层单位"||p.JGLX=="职能部门");
            cb_mb1.DataTextField = "JGMC";
            cb_mb1.DataValueField = "JGBM";
            cb_mb1.DataBind();
            //目标值审核
            cb_mb2.DataSource = znbm_ejdw.Where(p => p.JGLX == "职能部门");
            cb_mb2.DataTextField = "JGMC";
            cb_mb2.DataValueField = "JGBM";
            cb_mb2.DataBind();
            //完成值填报
            cb_wc1.DataSource = znbm_ejdw.Where(p => p.JGLX == "基层单位" || p.JGLX == "职能部门");
            cb_wc1.DataTextField = "JGMC";
            cb_wc1.DataValueField = "JGBM";
            cb_wc1.DataBind();
            //完成值审核
            cb_wc2.DataSource = znbm_ejdw.Where(p => p.JGLX == "职能部门");
            cb_wc2.DataTextField = "JGMC";
            cb_wc2.DataValueField = "JGBM";
            cb_wc2.DataBind();

            //被打分机构
            cb_bdfjg.DataSource = znbm_ejdw;
            //cb_bdfjg.DataSource = znbm_ejdw.Where(p => p.JGLX == "职能部门");
            cb_bdfjg.DataTextField = "JGMC";
            cb_bdfjg.DataValueField = "JGBM";
            cb_bdfjg.DataBind();
            //打分者
            cb_dfz.DataSource = znbm_ejdw;
            cb_dfz.DataTextField = "JGMC";
            cb_dfz.DataValueField = "JGBM";
            cb_dfz.DataBind();
            cb_dfz.Items.Insert(0, new ListItem("〖本单位自评〗", "ZIPING"));
            cb_dfz.Items.Insert(1, new ListItem("〖分公司领导〗", "YQTLD"));
            cb_dfz.Items.Insert(2, new ListItem("〖分管领导〗", "FGLD"));

            //处理上调与下调
            if (!string.IsNullOrEmpty(UpLhzbbm))
            {
                Hid_LHZBBM.Value = UpLhzbbm;
                LnkBtn_Up_Click(null, null);
            }
            else if (!string.IsNullOrEmpty(DownLhzbbm))
            {
                Hid_LHZBBM.Value = DownLhzbbm;
                LnkBtn_Down_Click(null, null);
            }
            else
            {
                BindTree();
            }
        }
        #endregion


        #region BindTree

        /// <summary>
        /// 从指标表中提取所有量化指标
        /// </summary>
        protected void BindTree()
        {
            //计算规则
            List<ZbkJsgzModel> jsgsList = jsgzSrv.GetListByBBMC(Ddl_BBMC.SelectedValue).ToList();
            Ddl_GZID.Items.Clear();
            Ddl_GZID.Items.Add(new ListItem("", ""));
            foreach (ZbkJsgzModel jsgs in jsgsList)
            {
                Ddl_GZID.Items.Add(new ListItem(jsgs.GZMC, jsgs.GZID));
            }

            //提取量化指标信息
            lhzbList = lhzbSrv.GetListForValid(Ddl_Danwei.SelectedValue, Ddl_BBMC.SelectedValue, Chk_ShowAll.Checked).
                OrderBy(p => p.ZBXH).ToList();

            //提取定量指标信息
            List<ZbkZbxxModel> zbxxList = zbxxSrv.GetListForValid("定量指标") as List<ZbkZbxxModel>;
            var yjzbxxLst = zbxxList.Distinct<ZbkZbxxModel>(new FastPropertyComparer<ZbkZbxxModel>("YJZBMC"));

            //生成树型结构
            string tempErjiId = string.Empty;
            TreeView1.Nodes.Clear();
            foreach (ZbkZbxxModel zb in yjzbxxLst)
            {
                //一级节点
                TreeNode tnRoot = new TreeNode();
                tnRoot.Text = zb.YJZBMC;
                tnRoot.Value = "01-" + zb.YJZBMC;//定量指标
                TreeView1.Nodes.Add(tnRoot);
                //主指标
                var mainLhzbLst = lhzbList.Where(p => p.FZZB == "0" && p.Zbxx.YJZBMC == zb.YJZBMC);
                foreach (var m in mainLhzbLst)
                {
                    TreeNode mainNode = new TreeNode();
                    string gzsm = ((m.Jsgz != null) ? m.Jsgz.GZMC + "【" + Server.HtmlEncode(m.Jsgz.GZBDS) + "," + m.Jsgz.METHODNAME + "】" : "");
                    string paramStr = string.Format("/M.Z.Lhzb?BM={0}&BB={1}", Ddl_Danwei.SelectedValue, Ddl_BBMC.SelectedValue);
                    string xhtz = "&nbsp;&nbsp;〖<a href='" + (paramStr + "&UP=" + m.LHZBBM) + "'>↑</a>&nbsp;&nbsp;<a href='" + (paramStr + "&DOWN=" + m.LHZBBM) + "'>↓</a>〗";
                    mainNode.Text = m.Zbxx.ZBMC + (m.SFJY == "1" ? "(<font color=red>*</font>)" : "") +
                        ((!string.IsNullOrEmpty(m.GZID)) ? "(<a title='" + gzsm + "'>G</a>)" : "") + xhtz;
                    mainNode.Value = m.LHZBBM;
                    tnRoot.ChildNodes.Add(mainNode);
                    //以迭代形式循环提取所有下级关联指标
                    gainSubNodeForMainZb(lhzbList,mainNode);
                }
            }
            TreeView1.ExpandAll();
        }

        #endregion

        /// <summary>
        /// 版本选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_BBMC_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeView1.Nodes.Clear();
            BindTree();
            Clear();
            TreeView1.ExpandAll();
        }

        /// <summary>
        /// 单位选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Danwei_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeView1.Nodes.Clear();
            BindTree();
            Clear();
            TreeView1.ExpandAll();
        }

        /// <summary>
        /// 显示全部指标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Chk_ShowAll_CheckedChanged(object sender, EventArgs e)
        {
            TreeView1.Nodes.Clear();
            BindTree();
            Clear();
            TreeView1.ExpandAll();
        }

        /// <summary>
        /// 树型菜单选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            Clear();
            //提取量化指标信息
            lhzbList = lhzbSrv.GetListForValid(Ddl_Danwei.SelectedValue, Ddl_BBMC.SelectedValue,
                Chk_ShowAll.Checked) as List<ZbkLhzbModel>;
            //提取指定的指标
            string zbbm = TreeView1.SelectedNode.Value;
            var lhzbM = lhzbList.FirstOrDefault(p => p.LHZBBM == zbbm);
            if (lhzbM != null)
            {
                //添、修、删
                SetCntrlVisibility(LnkBtn_Ins, true);
                SetCntrlVisibility(LnkBtn_Upd, true);
                SetCntrlVisibility(LnkBtn_Del, true);
                SetCntrlVisibility(LnkBtn_Up, true);
                SetCntrlVisibility(LnkBtn_Down, true);

                CommonTool.SetModelDataToForm(lhzbM, Page, "Txt_", true);
                CommonTool.SetModelDataToForm(lhzbM, Page, "Ddl_", true);
                CommonTool.SetModelDataToForm(lhzbM, Page, "Chk_", true);
                CommonTool.SetModelDataToForm(lhzbM, Page, "Hid_", true);
                Hid_JSBDS.Value = Txt_BDS.Text = lhzbM.JSBDS;
                Txt_ZBMC.Text = lhzbM.Zbxx.ZBMC;
                Txt_BJQZ.Text = (lhzbM.BJQZ != null) ? (lhzbM.BJQZ.Value * 100).ToString() : "";
                if (!string.IsNullOrEmpty(lhzbM.PARENTZBBM)) 
                {
                    var parentM = lhzbList.FirstOrDefault(p => p.LHZBBM == lhzbM.PARENTZBBM);
                    if (parentM != null)
                    {
                        Lbl_PARENT.Text = parentM.Zbxx.ZBMC;
                        Hid_PARENTZBBM.Value = parentM.LHZBBM;
                    }
                    else
                    {
                        Lbl_PARENT.Text = lhzbM.Zbxx.YJZBMC;
                    }
                }
                else 
                {
                    Lbl_PARENT.Text = lhzbM.Zbxx.YJZBMC;
                }
                BindJiGouQuanXian(lhzbM);
            }
            else
            {
                //只添加
                SetCntrlVisibility(LnkBtn_Ins, true);
                SetCntrlVisibility(LnkBtn_Upd, false);
                SetCntrlVisibility(LnkBtn_Del, false);

                Lbl_PARENT.Text = TreeView1.SelectedNode.Text;
            }
            
        }

        /// <summary>
        /// 清空各项的值
        /// </summary>
        private void Clear()
        {
            cb_mb1.ClearSelection();
            cb_mb2.ClearSelection();
            cb_wc1.ClearSelection();
            cb_wc2.ClearSelection();
            cb_dfz.ClearSelection();
            cb_bdfjg.ClearSelection();
            Rd_DFJG.Checked = false;
            Rd_GLJG.Checked = false;
            Hid_LHZBBM.Value = "";
            Hid_PARENTZBBM.Value = "";
            Txt_ZBMC.Text = "";
            Txt_ZBSM.Text = "";
            Txt_PFBZ.Text = "";
            Txt_JSMS.Text = "";
            //Txt_ZBSM.Value = "";
            //Txt_PFBZ.Value = "";
            //Txt_JSMS.Value = "";
            Txt_JSDW.Text = "";
            Ddl_GZID.ClearSelection();
            Chk_SFJY.Checked = false;
            Chk_ISMBZ.Checked = false;
            Txt_ZBDH.Text = "";
            Chk_FZZB.Checked = false;
            Txt_BJQZ.Text = "";
            Txt_BDS.Text = "";
            Hid_JSBDS.Value = "";
            Hid_ZBID.Value = "";
            Txt_JZFS.Text = "";
            Lbl_Msg.Text = "";

            //先不显示操作按钮
            SetCntrlVisibility(LnkBtn_Ins, false);
            SetCntrlVisibility(LnkBtn_Upd, false);
            SetCntrlVisibility(LnkBtn_Del, false);
            SetCntrlVisibility(LnkBtn_Up, false);
            SetCntrlVisibility(LnkBtn_Down, false);
        }

        #region 按钮事件处理

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            //提取量化指标信息
            lhzbList = lhzbSrv.GetListForValid("", Ddl_BBMC.SelectedValue, Chk_ShowAll.Checked) as List<ZbkLhzbModel>;

            ZbkLhzbModel zhcnModel = (ZbkLhzbModel)CommonTool.GetFormDataToModel(typeof(ZbkLhzbModel), Page);
            zhcnModel.DB_Option_Action = WebKeys.InsertAction;
            if (string.IsNullOrEmpty(Hid_ZBID.Value))
            {
                Lbl_Msg.Text = "请先选择要添加的指标后再行提交!";
                return;
            }
            //if (lhzbList.Exists(p => p.ZBID == Hid_ZBID.Value.ToInt()))
            //{
            //    Lbl_Msg.Text = "该指标已存在了!";
            //    return;
            //}

            //检测Hid_LHZBBM是否为空，如果为空，则为主指标，否则为辅助指标
            if (!string.IsNullOrEmpty(Hid_LHZBBM.Value))
            {
                zhcnModel.PARENTZBBM = Hid_LHZBBM.Value;
                zhcnModel.FZZB = "1";

                //add by qw 2014.12.19 start 生成指标序号
                //有上级指标
                var parentZb = lhzbList.FirstOrDefault(p => p.LHZBBM == Hid_LHZBBM.Value);
                if (!string.IsNullOrEmpty(parentZb.ZBXH))
                {
                    var subzbLst = lhzbList.Where(p => p.ZBXH.StartsWith(parentZb.ZBXH) &&
                        p.ZBXH.Length == parentZb.ZBXH.Length + 3).ToList();
                    string maxZbxh = (subzbLst.Count > 0) ? subzbLst.Max(p => p.ZBXH).ToString() : "";
                    if (!string.IsNullOrEmpty(maxZbxh))
                    {
                        int xh = maxZbxh.Substring(parentZb.ZBXH.Length).ToInt();
                        zhcnModel.ZBXH = parentZb.ZBXH + CommonTool.BuZero_3(xh + 1);
                    }
                    else
                    {
                        zhcnModel.ZBXH = parentZb.ZBXH + CommonTool.BuZero_3(1);
                    }
                }
                //end
            }
            else
            {
                zhcnModel.PARENTZBBM = "";
                zhcnModel.FZZB = "0";

                //add by qw 2014.12.19 start 生成指标序号
                //没有Hid_LHZBBM表示是新指标，则直接取指标的一级分类名称
                var zb = zbxxSrv.GetSingle(Hid_ZBID.Value);
                if (zb != null)
                {
                    var tjzbLst = lhzbList.Where(p => p.Zbxx.YJZBMC == zb.YJZBMC && p.ZBXH.Length == 5).ToList();
                    string maxZbxh = (tjzbLst.Count > 0) ? tjzbLst.Max(p => p.ZBXH).ToString() : "";
                    if (!string.IsNullOrEmpty(maxZbxh) && maxZbxh.Length >= 5)
                    {
                        int xh = maxZbxh.Substring(2).ToInt();
                        zhcnModel.ZBXH = maxZbxh.Substring(0, 2) + CommonTool.BuZero_3(xh + 1);
                    }
                    else
                    {
                        zhcnModel.ZBXH = zb.SXH.Substring(0, 2) + CommonTool.BuZero_3(1);
                    }
                }
                //end
            }
            zhcnModel.LHZBBM = "LHZB" + CommonTool.GetPkId();
            zhcnModel.JSBDS = Server.UrlDecode(Hid_JSBDS.Value);
            zhcnModel.JSBDS = zhcnModel.JSBDS.Replace("\r\n", "").Replace("\n", "").Replace("<br/>", "");
            zhcnModel.GZID = (string.IsNullOrEmpty(zhcnModel.GZID)) ? null : zhcnModel.GZID;
            zhcnModel.BJQZ = (zhcnModel.BJQZ.Value / 100M);

            //add
            if (lhzbSrv.Execute(zhcnModel))
            {
                if (Rd_DFJG.Checked)
                {
                    #region 打分机构设置

                    //被打分机构==完成值填报
                    foreach (ListItem item in cb_bdfjg.Items)
                    {
                        if (item.Selected)
                        {
                            ZbkWcztbModel model = new ZbkWcztbModel();
                            model.DB_Option_Action = WebKeys.InsertAction;
                            model.LHZBBM = zhcnModel.LHZBBM;
                            model.JGBM = item.Value.ToInt();
                            wcztbSrv.Execute(model);
                        }
                    }

                    //打分者==完成值审核
                    foreach (ListItem item in cb_dfz.Items)
                    {
                        if (item.Selected)
                        {
                            ZbkWczshdfModel model = new ZbkWczshdfModel();
                            model.DB_Option_Action = WebKeys.InsertAction;
                            model.LHZBBM = zhcnModel.LHZBBM;
                            model.OPERATOR = item.Value;
                            model.OPERTYPE = "0";
                            wczshdfSrv.Execute(model);
                        }
                    }

                    #endregion
                }
                else if (Rd_GLJG.Checked)
                {
                    #region 关联机构

                    //目标值填报
                    foreach (ListItem item in cb_mb1.Items)
                    {
                        if (item.Selected)
                        {
                            ZbkMbztbModel model = new ZbkMbztbModel();
                            model.DB_Option_Action = WebKeys.InsertAction;
                            model.LHZBBM = zhcnModel.LHZBBM;
                            model.JGBM = item.Value.ToInt();
                            mbztbSrv.Execute(model);
                        }
                    }

                    //目标值审核，只能有一个
                    foreach (ListItem item in cb_mb2.Items)
                    {
                        if (item.Selected)
                        {
                            ZbkMbzshModel model = new ZbkMbzshModel();
                            model.DB_Option_Action = WebKeys.InsertAction;
                            model.LHZBBM = zhcnModel.LHZBBM;
                            model.OPERATOR = item.Value;
                            model.OPERTYPE = "1";
                            mbzshSrv.Execute(model);
                        }
                    }

                    //完成值填报
                    foreach (ListItem item in cb_wc1.Items)
                    {
                        if (item.Selected)
                        {
                            ZbkWcztbModel model = new ZbkWcztbModel();
                            model.DB_Option_Action = WebKeys.InsertAction;
                            model.LHZBBM = zhcnModel.LHZBBM;
                            model.JGBM = item.Value.ToInt();
                            wcztbSrv.Execute(model);
                        }
                    }

                    //完成值审核，只能有一个
                    foreach (ListItem item in cb_wc2.Items)
                    {
                        if (item.Selected)
                        {
                            ZbkWczshdfModel model = new ZbkWczshdfModel();
                            model.DB_Option_Action = WebKeys.InsertAction;
                            model.LHZBBM = zhcnModel.LHZBBM;
                            model.OPERATOR = item.Value;
                            model.OPERTYPE = "1";
                            wczshdfSrv.Execute(model);
                        }
                    }
                    #endregion
                }
            }
            Utility.ShowMsg(Page, "提示", "添加指标成功！", 100, "show");
            Clear();
            BindTree();
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            //提取量化指标信息
            lhzbList = lhzbSrv.GetListForValid(Ddl_Danwei.SelectedValue, Ddl_BBMC.SelectedValue, Chk_ShowAll.Checked) as List<ZbkLhzbModel>;
            ZbkLhzbModel updateModel = lhzbList.FirstOrDefault(p=>p.LHZBBM == Hid_LHZBBM.Value);
            ZbkLhzbModel zhcnModel = (ZbkLhzbModel)CommonTool.GetFormDataToModel(typeof(ZbkLhzbModel), Page);
            zhcnModel.DB_Option_Action = WebKeys.UpdateAction;
            if (string.IsNullOrEmpty(zhcnModel.PARENTZBBM))
            {
                zhcnModel.PARENTZBBM = "";
                zhcnModel.FZZB = "0";
            }
            if (updateModel != null && 
                updateModel.ZBID != zhcnModel.ZBID)
            {
                Lbl_Msg.Text = "指标出现了变化!不能执行此次修改!";
                return;
            }
            zhcnModel.JSBDS = Server.UrlDecode(Hid_JSBDS.Value);
            zhcnModel.JSBDS = zhcnModel.JSBDS.Replace("\r\n", "").Replace("\n", "").Replace("<br/>", "");
            zhcnModel.GZID = (string.IsNullOrEmpty(zhcnModel.GZID)) ? null : zhcnModel.GZID;
            zhcnModel.BJQZ = (zhcnModel.BJQZ.Value / 100M);
            zhcnModel.ZBXH = updateModel.ZBXH;//指标序号

            //update
            if (lhzbSrv.Execute(zhcnModel))
            {
                //先删除一次数据
                if (lhzbSrv.DeleteGljg(zhcnModel.LHZBBM))
                {
                    if (Rd_DFJG.Checked)
                    {
                        #region 打分机构设置

                        //被打分机构==完成值填报
                        foreach (ListItem item in cb_bdfjg.Items)
                        {
                            if (item.Selected)
                            {
                                ZbkWcztbModel model = new ZbkWcztbModel();
                                model.DB_Option_Action = WebKeys.InsertAction;
                                model.LHZBBM = zhcnModel.LHZBBM;
                                model.JGBM = item.Value.ToInt();
                                wcztbSrv.Execute(model);
                            }
                        }

                        //打分者==完成值审核
                        foreach (ListItem item in cb_dfz.Items)
                        {
                            if (item.Selected)
                            {
                                ZbkWczshdfModel model = new ZbkWczshdfModel();
                                model.DB_Option_Action = WebKeys.InsertAction;
                                model.LHZBBM = zhcnModel.LHZBBM;
                                model.OPERATOR = item.Value;
                                model.OPERTYPE = "0";
                                wczshdfSrv.Execute(model);
                            }
                        }

                        #endregion
                    }
                    else if (Rd_GLJG.Checked)
                    {
                        #region 关联机构

                        //目标值填报
                        foreach (ListItem item in cb_mb1.Items)
                        {
                            if (item.Selected)
                            {
                                ZbkMbztbModel model = new ZbkMbztbModel();
                                model.DB_Option_Action = WebKeys.InsertAction;
                                model.LHZBBM = zhcnModel.LHZBBM;
                                model.JGBM = item.Value.ToInt();
                                mbztbSrv.Execute(model);
                            }
                        }

                        //目标值审核，只能有一个
                        foreach (ListItem item in cb_mb2.Items)
                        {
                            if (item.Selected)
                            {
                                ZbkMbzshModel model = new ZbkMbzshModel();
                                model.DB_Option_Action = WebKeys.InsertAction;
                                model.LHZBBM = zhcnModel.LHZBBM;
                                model.OPERATOR = item.Value;
                                model.OPERTYPE = "1";
                                mbzshSrv.Execute(model);
                            }
                        }

                        //完成值填报
                        foreach (ListItem item in cb_wc1.Items)
                        {
                            if (item.Selected)
                            {
                                ZbkWcztbModel model = new ZbkWcztbModel();
                                model.DB_Option_Action = WebKeys.InsertAction;
                                model.LHZBBM = zhcnModel.LHZBBM;
                                model.JGBM = item.Value.ToInt();
                                wcztbSrv.Execute(model);
                            }
                        }

                        //完成值审核，只能有一个
                        foreach (ListItem item in cb_wc2.Items)
                        {
                            if (item.Selected)
                            {
                                ZbkWczshdfModel model = new ZbkWczshdfModel();
                                model.DB_Option_Action = WebKeys.InsertAction;
                                model.LHZBBM = zhcnModel.LHZBBM;
                                model.OPERATOR = item.Value;
                                model.OPERTYPE = "1";
                                wczshdfSrv.Execute(model);
                            }
                        }
                        #endregion
                    }
                }
            }
            Utility.ShowMsg(Page, "提示", "修改指标成功！", 100, "show");
            Clear();
            BindTree();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Del_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Hid_LHZBBM.Value))
                return;
            deleteNodeByZbbm(Hid_LHZBBM.Value);
            Utility.ShowMsg(Page, "提示", "删除指标成功！", 100, "show");
            Clear();
            BindTree();
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            GobackPageUrl("");
        }

        /// <summary>
        /// 重新排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_PX_Click(object sender, EventArgs e)
        {
            //提取量化指标信息
            lhzbList = lhzbSrv.GetListForValid("", Ddl_BBMC.SelectedValue, Chk_ShowAll.Checked).ToList();

            //提取定量指标信息
            List<ZbkZbxxModel> zbxxList = zbxxSrv.GetListForValid("定量指标") as List<ZbkZbxxModel>;
            //指标分类
            var yjzbxxLst = zbxxList.DistinctBy(p => p.YJZBMC).ToList();
            int xh = 1;//序号
            for (int i = 0; i < yjzbxxLst.Count(); i++)
            {
                //主指标，效益类
                var mainLhzbLst = lhzbList.Where(p => p.FZZB == "0" && p.Zbxx.YJZBMC == yjzbxxLst[i].YJZBMC).ToList();
                for (int j = 0; j < mainLhzbLst.Count(); j++)
                {
                    //效益类》成本
                    mainLhzbLst[j].ZBXH = mainLhzbLst[j].Zbxx.SXH.Substring(0, 2) + CommonTool.BuZero_3(xh++);
                    mainLhzbLst[j].DB_Option_Action = WebKeys.UpdateAction;
                    xhList.Add(mainLhzbLst[j]);
                    //以迭代形式循环提取所有下级关联指标
                    setLhzbZbxh(lhzbList, mainLhzbLst[j]);
                }
            }
            //完成序号更新
            lhzbSrv.UpdateZbxhByList(xhList);
            Utility.ShowMsg(Page, "提示", "更新指标序号成功！", 100, "show");
            BindTree();
        }

        #endregion


        #region 私有方法

        /// <summary>
        /// 获取指定指标下的所有子指标
        /// </summary>
        /// <param name="lhzbLst">量化指标集合</param>
        /// <param name="parent">父指标</param>
        private void setLhzbZbxh(List<ZbkLhzbModel> lhzbLst, ZbkLhzbModel parent)
        {
            var subZbLst = lhzbLst.Where(p => p.PARENTZBBM == parent.LHZBBM).ToList();
            for (int k = 0; k < subZbLst.Count(); k++)
            {
                subZbLst[k].ZBXH = parent.ZBXH + CommonTool.BuZero_3(k + 1);
                subZbLst[k].DB_Option_Action = WebKeys.UpdateAction;
                xhList.Add(subZbLst[k]);
                //迭代获取其所有子指标
                setLhzbZbxh(lhzbLst, subZbLst[k]);
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="lhzbbm"></param>
        private void deleteNodeByZbbm(string lhzbbm)
        {
            var list = lhzbSrv.GetListByHQL("from ZbkLhzbModel p where p.PARENTZBBM='" + lhzbbm + "'");
            foreach (var q in list)
            {
                deleteNodeByZbbm(q.LHZBBM);
            }
            ZbkLhzbModel deleteM = new ZbkLhzbModel();
            deleteM.DB_Option_Action = WebKeys.DeleteAction;
            deleteM.LHZBBM = lhzbbm;
            //delete
            if (lhzbSrv.DeleteGljg(deleteM.LHZBBM))
            {
                lhzbSrv.Execute(deleteM);
            }
        }

        /// <summary>
        /// 循环提取所有下级关联指标
        /// </summary>
        /// <param name="lhzbLst"></param>
        /// <param name="mainNode"></param>
        private void gainSubNodeForMainZb(List<ZbkLhzbModel> lhzbLst,TreeNode mainNode)
        {
            var subZbLst = lhzbLst.Where(p => p.PARENTZBBM == mainNode.Value);
            foreach (var q in subZbLst)
            {
                TreeNode node = new TreeNode();
                string gzsm = ((q.Jsgz != null) ? q.Jsgz.GZMC + "【" + Server.HtmlEncode(q.Jsgz.GZBDS) + "," + q.Jsgz.METHODNAME + "】" : "");
                string paramStr = string.Format("/M.Z.Lhzb?BM={0}&BB={1}", Ddl_Danwei.SelectedValue, Ddl_BBMC.SelectedValue);
                string xhtz = "&nbsp;&nbsp;〖<a href='" + (paramStr + "&UP=" + q.LHZBBM) + "'>↑</a>&nbsp;&nbsp;<a href='" + (paramStr + "&DOWN=" + q.LHZBBM) + "'>↓</a>〗";
                node.Text = q.Zbxx.ZBMC + (q.SFJY == "1" ? "<font color=red>*</font>" : "") +
                        ((!string.IsNullOrEmpty(q.GZID)) ? "(<a title='" + gzsm + "'>G</a>)" : "") + xhtz;
                node.Value = q.LHZBBM;
                mainNode.ChildNodes.Add(node);
                gainSubNodeForMainZb(lhzbLst, node);
            }
        }

        /// <summary>
        /// 完成目标与完成值相关机构或角色的绑定
        /// </summary>
        /// <param name="lhzbModel"></param>
        private void BindJiGouQuanXian(ZbkLhzbModel lhzbModel)
        {

            //目标值填报
            foreach (ZbkMbztbModel mbtb in lhzbModel.MbztbLst)
            {
                ListItem item = cb_mb1.Items.FindByValue(mbtb.JGBM.ToString());
                if (item != null) item.Selected = true;
            }

            //目标值审核
            foreach (ZbkMbzshModel mbsh in lhzbModel.MbzshLst)
            {
                ListItem item = cb_mb2.Items.FindByValue(mbsh.OPERATOR);
                if (item != null) item.Selected = true;
            }

            foreach (ZbkWcztbModel wctb in lhzbModel.WcztbLst)
            {
                //完成值填报
                ListItem item = cb_wc1.Items.FindByValue(wctb.JGBM.ToString());
                if (item != null) item.Selected = true;
                //被打分机构
                ListItem item2 = cb_bdfjg.Items.FindByValue(wctb.JGBM.ToString());
                if (item2 != null) item2.Selected = true;
            }

            foreach (ZbkWczshdfModel wcshdf in lhzbModel.WczshdfLst)
            {
                if (wcshdf.OPERTYPE == "1")
                {
                    //完成值审核
                    ListItem item = cb_wc2.Items.FindByValue(wcshdf.OPERATOR);
                    if (item != null) item.Selected = true;
                    Rd_GLJG.Checked = true;
                }
                else
                {
                    //打分者
                    ListItem item = cb_dfz.Items.FindByValue(wcshdf.OPERATOR);
                    if (item != null) item.Selected = true;
                    Rd_DFJG.Checked = true;
                }

            }

        }

        #endregion

        #region 调整序号

        /// <summary>
        /// 同级上调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Up_Click(object sender, EventArgs e)
        {
            //重新分配该单位下所有的指标编号，再进行上调
            lhzbList = lhzbSrv.GetListForValid(Ddl_Danwei.SelectedValue, Ddl_BBMC.SelectedValue, Chk_ShowAll.Checked)
                .OrderBy(p => p.ZBXH).ToList();
            var lhzb = lhzbSrv.GetSingle(Hid_LHZBBM.Value);
            if (lhzb != null && lhzb.ZBXH.Length >= 5)
            {
                //提取其同级的所有指标
                var tjzbLst = lhzbList.Where(p =>
                    p.ZBXH.StartsWith(lhzb.ZBXH.Substring(0, lhzb.ZBXH.Length - 3)) && p.ZBXH.Length == lhzb.ZBXH.Length).
                    OrderBy(p=>p.ZBXH).ToList();

                //交换序号
                string currSxh = lhzb.ZBXH;
                int currIndex = tjzbLst.FindIndex(p => p.LHZBBM == lhzb.LHZBBM);
                if (currIndex > 0 && tjzbLst.Count > 1)
                {
                    var prevZbxx = tjzbLst[currIndex - 1];
                    if (prevZbxx != null)
                    {
                        string prevSxh = prevZbxx.ZBXH;
                        prevZbxx.ZBXH = currSxh;
                        prevZbxx.DB_Option_Action = WebKeys.UpdateAction;
                        xhList.Add(prevZbxx);
                        exchangeZbxh(prevZbxx, prevSxh, lhzbList);
                        lhzb.ZBXH = prevSxh;
                        lhzb.DB_Option_Action = WebKeys.UpdateAction;
                        xhList.Add(lhzb);
                        exchangeZbxh(lhzb, currSxh, lhzbList);
                        //进行批量更新
                        lhzbSrv.UpdateZbxhByList(xhList);
                        Utility.ShowMsg(Page, "提示", "上调指标序号成功！", 100, "show");
                    }
                }
            }
            Clear();
            BindTree();
        }

        /// <summary>
        /// 同级下调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Down_Click(object sender, EventArgs e)
        {
            //重新分配该单位下所有的指标编号，再进行下调
            lhzbList = lhzbSrv.GetListForValid(Ddl_Danwei.SelectedValue, Ddl_BBMC.SelectedValue, Chk_ShowAll.Checked)
                .OrderBy(p => p.ZBXH).ToList();
            var lhzb = lhzbSrv.GetSingle(Hid_LHZBBM.Value);
            if (lhzb != null && lhzb.ZBXH.Length >= 5)
            {
                //提取其同级的所有指标
                var tjzbLst = lhzbList.Where(p =>
                    p.ZBXH.StartsWith(lhzb.ZBXH.Substring(0, lhzb.ZBXH.Length - 3)) && p.ZBXH.Length == lhzb.ZBXH.Length).
                    OrderBy(p => p.ZBXH).ToList();

                //交换序号
                string currSxh = lhzb.ZBXH;
                int currIndex = tjzbLst.FindIndex(p => p.LHZBBM == lhzb.LHZBBM);
                if (currIndex < tjzbLst.Count - 1)
                {
                    var nextZbxx = tjzbLst[currIndex + 1];
                    if (nextZbxx != null)
                    {
                        string nextSxh = nextZbxx.ZBXH;
                        nextZbxx.ZBXH = currSxh;
                        nextZbxx.DB_Option_Action = WebKeys.UpdateAction;
                        xhList.Add(nextZbxx);
                        exchangeZbxh(nextZbxx, nextSxh, lhzbList);
                        lhzb.ZBXH = nextSxh;
                        lhzb.DB_Option_Action = WebKeys.UpdateAction;
                        xhList.Add(lhzb);
                        exchangeZbxh(lhzb, currSxh, lhzbList);
                        //进行批量更新
                        lhzbSrv.UpdateZbxhByList(xhList);
                        Utility.ShowMsg(Page, "提示", "下调指标序号成功！", 100, "show");
                    }
                }
            }
            Clear();
            BindTree();
        }

        #endregion

        #region 更新序号操作

        /// <summary>
        /// 更新指标序号
        /// </summary>
        /// <param name="lhzb">指标对象</param>
        /// <param name="oldXh">原来的序号</param>
        /// <param name="lhzbLst">指标集合</param>
        private void exchangeZbxh(ZbkLhzbModel lhzb, string oldXh, List<ZbkLhzbModel> lhzbLst)
        {
            //提取当前指标的所有下级指标
            var subzbLst = lhzbLst.Where(p =>
                p.ZBXH.StartsWith(oldXh) && p.ZBXH.Length > oldXh.Length).OrderBy(p => p.ZBXH).ToList();
            foreach (var zb in subzbLst)
            {
                zb.DB_Option_Action = WebKeys.UpdateAction;
                zb.ZBXH = lhzb.ZBXH + zb.ZBXH.Substring(oldXh.Length);//换头部
                xhList.Add(zb);
            }
        }

        #endregion

    }
}