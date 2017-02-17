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
    /// 机关部门机关作风建设打分详细页面
    /// </summary>
    public partial class JgbmJgzfDetail : PageBase
    {

        /// <summary>
        /// 机关作风服务类
        /// </summary>
        KhJgzfbService jgzfSrv = new KhJgzfbService();
        KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//打分指标明细服务类
        KhKhglService khglSrv = new KhKhglService();//考核管理服务类

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected string Jgbm = (string)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//机构编码
        protected string Type = (string)Utility.sink("TYPE", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//类型（互评、上级、二级单位）

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
            }
        }

        #region 专用方法区

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindGrid()
        {
            
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //考核期信息
            var kaohe = khglSrv.GetSingle(Khid);
            if (kaohe != null)
            {
                Lbl_Kaohe.Text = kaohe.KHMC;
            }
            //部门
            var bmjg = bmjgService.GetSingle(Jgbm);
            if (bmjg != null)
            {
                Lbl_Cpbm.Text = bmjg.JGMC;
            }
        }

        #endregion


        #region 事件处理区

        /// <summary>
        /// 获取机关部门的机关作风建设打分详情
        /// </summary>
        /// <returns></returns>
        protected string GetJgbmTable()
        {
            StringBuilder sb = new StringBuilder();
            //获取数据
            var jgzfbLst = jgzfSrv.GetListByKhid(Khid).Where(p=>p.JGBM == Jgbm.ToInt()).ToList();

            if (Type == ((int)WebKeys.DFUserType.同级部门).ToString())
            {
                var hpList = jgzfbLst.
                    Where(p => p.DFZLX == ((int)WebKeys.DFUserType.同级部门).ToString()).OrderBy(p => p.DFZBXH).ToList();
                int hpColspan = hpList.DistinctBy(p => p.ZBBM).Count();
            }
            else if (Type == ((int)WebKeys.DFUserType.同级二级单位).ToString())
            {
                var ejdwList = jgzfbLst.
                    Where(p => p.DFZLX == ((int)WebKeys.DFUserType.同级二级单位).ToString()).OrderBy(p => p.DFZBXH).ToList();
                int ejdwColspan = ejdwList.DistinctBy(p => p.ZBBM).Count();
            }
            else if (Type == ((int)WebKeys.DFUserType.上级领导).ToString())
            {
                var ldList = jgzfbLst.
                    Where(p => p.DFZLX == ((int)WebKeys.DFUserType.上级领导).ToString()).OrderBy(p => p.DFZBXH).ToList();
                int ldColspan = ldList.DistinctBy(p => p.ZBBM).Count();
            }
            return sb.ToString();
        }

        #endregion

    }
}