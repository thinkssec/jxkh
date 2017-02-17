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
    /// 二级单位财务基础数据录入页面
    /// </summary>
    public partial class EjdwJcsjData : PageBase
    {

        /// <summary>
        /// 财务基础数据-服务类
        /// </summary>
        protected KhCwjcsjService cwjcsjSrv = new KhCwjcsjService();
        protected KhKhglService khglSrv = new KhKhglService();//考核管理服务类
        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected string Jgbm = (string)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//机构编码
        protected string BackUrl = (string)Utility.sink("BackUrl", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//返回路径
        protected string Expand = (string)Utility.sink("Expand", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//展开标志
        protected int ThisYear = DateTime.Now.Year;//当前年份
        protected int PrevYear = DateTime.Now.Year - 1;//上一年
        protected string TableHeight = "340px;";//表格高度
        /// <summary>
        /// 当前页面URL路径
        /// </summary>
        public string Url
        {
            get
            {
                string urls = string.Format("?KH={0}&BM={1}&WinMax=1", Khid, Ddl_Danwei.SelectedValue);
                return GetPageUrlAndParams(urls).TrimStart('~');
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
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDdl();
                BindGrid();
                TabTitle = "本年数据〖" + ThisYear + "〗";
            }
        }

        #region 专用方法区

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindGrid()
        {
            //检测当前用户权限
            if (ChkUserLuruPermission(Khid, Ddl_Danwei.SelectedValue.ToInt(), userModel))
            {
                Pnl_Edit.Visible = true;
                Lbl_Msg.Text = "您可以进行编辑!";
            }
            else
            {
                Pnl_Edit.Visible = false;
                Lbl_Msg.Text = "您只能查看!";
            }

            //展开与收缩
            if (Expand == "1" || Chk_Expand.Checked)
            {
                TableHeight = "auto;";
                Chk_Expand.Checked = true;
            }
            else
            {
                TableHeight = "340px;";
                Chk_Expand.Checked = false;
            }

            //标签页选择
            if (!string.IsNullOrEmpty(Hid_TabTitle.Value))
                TabTitle = Hid_TabTitle.Value;

            //考核信息
            var kaohe = khglSrv.GetSingle(Khid);
            if (kaohe != null)
            {
                Hid_KHID.Value = kaohe.KHID.ToString();
                ThisYear = kaohe.KHND.ToInt();
                PrevYear = ThisYear - 1;
            }
            //单位
            var bmjg = bmjgService.GetSingle(Ddl_Danwei.SelectedValue);
            if (bmjg != null)
            {
                Hid_JGBM.Value = Jgbm = Ddl_Danwei.SelectedValue;
                //Lbl_DanWei.Text = bmjg.JGMC;
            }

            //返回路径
            if (string.IsNullOrEmpty(BackUrl))
            {
                BackUrl = PageUrl.TrimStart('~');
            }
            else
            {
                //解码
                BackUrl = Base64.Base64Decode(BackUrl);
            }
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {

            //单位信息
            List<SysBmjgModel> parentBmjgLst = bmjgService.GetSameLevelBmjg(4) as List<SysBmjgModel>;
            int[] jgbms = (from c in parentBmjgLst select c.JGBM).ToArray();
            List<SysBmjgModel> bmjgTreeList = bmjgService.GetBmjgTreeLst(false).Where(p => p.XSXH.Length > 2 && !p.JGLX.Contains("职能")).ToList();
            bmjgService.BindSSECDropDownListForBmjg(Ddl_Danwei, bmjgTreeList, jgbms);
            if (!string.IsNullOrEmpty(Jgbm))
            {
                Ddl_Danwei.SelectedValue = Jgbm;
            }
            else
            {
                Ddl_Danwei.SelectedValue = userModel.JGBM.ToString();
            }

            Lbl_Msg.Text = "";
        }

        #endregion


        #region 事件处理区

        /// <summary>
        /// 数据保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            //完成基础数据的更新
            KhCwjcsjService cwjcsjSrv = new KhCwjcsjService();
            //考核信息
            var kaohe = khglSrv.GetSingle(Hid_KHID.Value);
            if (kaohe == null) return;
            
            ThisYear = kaohe.KHND.ToInt();
            PrevYear = ThisYear - 1;

            //1==考核期年份数据-----------------------------------------------
            var thisYearLst = cwjcsjSrv.GetListByJgbmAndNF(Jgbm, ThisYear);
            if (!string.IsNullOrEmpty(Hid_JCSJData1.Value))
            {
                string[] rows = Hid_JCSJData1.Value.TrimEnd('|').Split('|');
                foreach (string row in rows)
                {
                    if (string.IsNullOrEmpty(row))
                        continue;
                    string[] cols = row.Split(':');
                    if (cols.Length == 14)
                    {
                        //提取数据
                        var jcsj = thisYearLst.FirstOrDefault(p => p.XH.ToString() == cols[0]);
                        if (jcsj.ZBMC.Contains("EVA")) 
                            continue; 
                        
                        int sl = 0;
                        decimal ljz = 0M;
                        if (jcsj != null)
                        {
                            jcsj.DB_Option_Action = WebKeys.UpdateAction;
                            if (!string.IsNullOrEmpty(cols[1]))
                            {
                                jcsj.M1 = cols[1].ToDecimal();
                                sl++;
                                ljz += jcsj.M1.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[2]))
                            {
                                jcsj.M2 = cols[2].ToDecimal();
                                sl++;
                                ljz += jcsj.M2.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[3]))
                            {
                                jcsj.M3 = cols[3].ToDecimal();
                                sl++;
                                ljz += jcsj.M3.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[4]))
                            {
                                jcsj.M4 = cols[4].ToDecimal();
                                sl++;
                                ljz += jcsj.M4.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[5]))
                            {
                                jcsj.M5 = cols[5].ToDecimal();
                                sl++;
                                ljz += jcsj.M5.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[6]))
                            {
                                jcsj.M6 = cols[6].ToDecimal();
                                sl++;
                                ljz += jcsj.M6.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[7]))
                            {
                                jcsj.M7 = cols[7].ToDecimal();
                                sl++;
                                ljz += jcsj.M7.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[8]))
                            {
                                jcsj.M8 = cols[8].ToDecimal();
                                sl++;
                                ljz += jcsj.M8.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[9]))
                            {
                                jcsj.M9 = cols[9].ToDecimal();
                                sl++;
                                ljz += jcsj.M9.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[10]))
                            {
                                jcsj.M10 = cols[10].ToDecimal();
                                sl++;
                                ljz += jcsj.M10.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[11]))
                            {
                                jcsj.M11 = cols[11].ToDecimal();
                                sl++;
                                ljz += jcsj.M11.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[12]))
                            {
                                jcsj.M12 = cols[12].ToDecimal();
                                sl++;
                                ljz += jcsj.M12.Value;
                            }
                            
                            //数量 累计 平均
                            jcsj.SL = sl;
                            jcsj.LJZ = ljz;
                            jcsj.PJZ = (sl > 0) ? (ljz / sl) : 0M;
                            //更新
                            cwjcsjSrv.Execute(jcsj);
                        }
                    }
                }

                //==计算当前年度EVA总额---------------
                cwjcsjSrv.CalcAndSaveEVA(Jgbm.ToInt(), ThisYear);
            }

            //2==上一年数据----------------------------------------------------
            var prevYearLst = cwjcsjSrv.GetListByJgbmAndNF(Jgbm, PrevYear);
            if (!string.IsNullOrEmpty(Hid_JCSJData2.Value))
            {
                string[] rows = Hid_JCSJData2.Value.TrimEnd('|').Split('|');
                foreach (string row in rows)
                {
                    if (string.IsNullOrEmpty(row))
                        continue;
                    string[] cols = row.Split(':');
                    if (cols.Length == 14)
                    {
                        //提取数据
                        var jcsj = prevYearLst.FirstOrDefault(p => p.XH.ToString() == cols[0]);
                        if (jcsj.ZBMC.Contains("EVA"))
                            continue; 
                        
                        int sl = 0;
                        decimal ljz = 0M;
                        if (jcsj != null)
                        {
                            jcsj.DB_Option_Action = WebKeys.UpdateAction;
                            if (!string.IsNullOrEmpty(cols[1]))
                            {
                                jcsj.M1 = cols[1].ToDecimal();
                                sl++;
                                ljz += jcsj.M1.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[2]))
                            {
                                jcsj.M2 = cols[2].ToDecimal();
                                sl++;
                                ljz += jcsj.M2.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[3]))
                            {
                                jcsj.M3 = cols[3].ToDecimal();
                                sl++;
                                ljz += jcsj.M3.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[4]))
                            {
                                jcsj.M4 = cols[4].ToDecimal();
                                sl++;
                                ljz += jcsj.M4.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[5]))
                            {
                                jcsj.M5 = cols[5].ToDecimal();
                                sl++;
                                ljz += jcsj.M5.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[6]))
                            {
                                jcsj.M6 = cols[6].ToDecimal();
                                sl++;
                                ljz += jcsj.M6.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[7]))
                            {
                                jcsj.M7 = cols[7].ToDecimal();
                                sl++;
                                ljz += jcsj.M7.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[8]))
                            {
                                jcsj.M8 = cols[8].ToDecimal();
                                sl++;
                                ljz += jcsj.M8.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[9]))
                            {
                                jcsj.M9 = cols[9].ToDecimal();
                                sl++;
                                ljz += jcsj.M9.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[10]))
                            {
                                jcsj.M10 = cols[10].ToDecimal();
                                sl++;
                                ljz += jcsj.M10.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[11]))
                            {
                                jcsj.M11 = cols[11].ToDecimal();
                                sl++;
                                ljz += jcsj.M11.Value;
                            }
                            if (!string.IsNullOrEmpty(cols[12]))
                            {
                                jcsj.M12 = cols[12].ToDecimal();
                                sl++;
                                ljz += jcsj.M12.Value;
                            }

                            //数量 累计 平均
                            jcsj.SL = sl;
                            jcsj.LJZ = ljz;
                            jcsj.PJZ = (sl > 0) ? (ljz / sl) : 0M;
                            //更新
                            cwjcsjSrv.Execute(jcsj);
                        }
                    }
                }

                //==计算上一年度EVA总额---------------
                cwjcsjSrv.CalcAndSaveEVA(Jgbm.ToInt(), PrevYear);
            }

            TabTitle = Hid_TabTitle.Value;
            string url = string.Format("KH={0}&BM={1}&BackUrl={2}&Rnd={3}", Khid, Jgbm, BackUrl, CommonTool.GetPkId());
            if (Chk_Expand.Checked)
            {
                url = url + "&Expand=1";
            }
            else
            {
                url = url + "&Expand=0";
            }
            GobackPageUrl("?" + url);
        }

        /// <summary>
        /// 单位切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Danwei_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabTitle = Hid_TabTitle.Value;
            string url = string.Format("KH={0}&BM={1}&BackUrl={2}&Rnd={3}", Khid, Ddl_Danwei.SelectedValue, BackUrl, CommonTool.GetPkId());
            if (Chk_Expand.Checked)
            {
                url = url + "&Expand=1";
            }
            else
            {
                url = url + "&Expand=0";
            }
            GobackPageUrl("?" + url);
        }

        #endregion

        /// <summary>
        /// 展开高度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Chk_Expand_CheckedChanged(object sender, EventArgs e)
        {
            if (Chk_Expand.Checked)
            {
                Response.Redirect(Url + "&Expand=1", true);
            }
            else
            {
                Response.Redirect(Url + "&Expand=0", true);
            }
        }

    }
}