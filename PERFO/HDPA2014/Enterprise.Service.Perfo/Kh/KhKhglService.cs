using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;
using Enterprise.Service.Perfo.Sys;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Model.Perfo.Sys;
using System.Web.UI.WebControls;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// 文件名:  KhKhglService.cs
    /// 功能描述: 业务逻辑层-考核管理数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhKhglService
    {

        /// <summary>
        /// 量化指标机关部门打分-服务类
        /// </summary>
        KhJgbmdfbService jgbmdfbSrv = new KhJgbmdfbService();
        KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();//定量指标明细服务类
        KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//打分指标明细服务类
        KhUnlockService unlockSrv = new KhUnlockService();//数据锁定服务类
        KhSjsbService sjsbSrv = new KhSjsbService();//考核文件上报服务类
        KhMessageService msgSrv = new KhMessageService();//待办箱服务类
        KhJgbmkhdfService jgbmkhdfSrv = new KhJgbmkhdfService();//机关部门考核得分
        KhJxzrszbService jxzrszbSrv = new KhJxzrszbService();//绩效责任书指标
        KhEjdwkhdfService ejdwkhdfSrv = new KhEjdwkhdfService();//二级单位考核得分
        KhCwjcsjService cwjcsjSrv = new KhCwjcsjService();//财务基础数据表
        SysUserService userSrv = new SysUserService();//用户服务类
        
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhKhglData dal = new KhKhglData();
        /// <summary>
        /// 缓存项名称
        /// </summary>
        public string CacheKey
        {
            get 
            { 
                return KhKhglData.CacheClassKey; 
            }
        }

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhKhglModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhKhglModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhKhglModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhKhglModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhKhglModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 个性化主页面相关

        /// <summary>
        /// 获取最新一期的二级单位及领导班子考核中需要当前用户审核的指标信息
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public string GetEjdwKaoheInfoByJgbm(SysUserModel userModel)
        {
            string khInfo = string.Empty;
            if (userModel == null) 
                return "";
            
            if (WebKeys.EnableCaching)
            {
                khInfo = (string)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetEjdwKaoheInfoByJgbm_" + userModel.JGBM);
            }
            if (string.IsNullOrEmpty(khInfo))
            {
                StringBuilder sb = new StringBuilder();
                bool isAudit = false;
                //最近一期考核
                var lastKaohe = GetLastKaoheInfo("LX2014A");//二级单位及领导班子考核
                if (lastKaohe != null)
                {
                    sb.Append("<tr>");
                    sb.Append("<td colspan=\"6\" class=\"td-bold\">【" + lastKaohe.KHMC + "】需要您审核的指标</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<th style=\"width: 6%\" class=\"td-bold\">序号</th>");
                    sb.Append("<th style=\"width: 15%\" class=\"td-bold\">指标类型</th>");
                    sb.Append("<th style=\"width: 30%\" class=\"td-bold\">指标名称</th>");
                    sb.Append("<th style=\"width: 33%\" class=\"td-bold\">被审核单位</th>");
                    sb.Append("<th style=\"width: 8%\" class=\"td-bold\">应审核</th>");
                    sb.Append("<th style=\"width: 8%\" class=\"td-bold\">已完成</th>");
                    sb.Append("</tr>");
                    //提取考核期内的定量和定性指标，并按一级分类
                    var dlzbLst = dlzbmxSrv.GetListByKhid(lastKaohe.KHID.ToString()).
                        Where(p => ("," + p.WCZSHR + ",").Contains("," + userModel.JGBM + ",")).ToList();
                    var zbmcLst = dlzbLst.DistinctBy(p => p.LhzbModel.Zbxx.ZBMC).ToList();
                    var yjzbmcLst = dlzbLst.DistinctBy(p => p.LhzbModel.Zbxx.YJZBMC).ToList();
                    for (int i = 0; i < yjzbmcLst.Count; i++)
                    {
                        List<string> khjgbms = new List<string>();
                        sb.Append("<tr>");
                        sb.Append("<td>" + (i + 1) + "</td>");
                        sb.Append("<td>" + yjzbmcLst[i].LhzbModel.Zbxx.YJZBMC + "</td>");
                        var zbs = zbmcLst.Where(p => p.LhzbModel.Zbxx.YJZBMC == yjzbmcLst[i].LhzbModel.Zbxx.YJZBMC).ToList();
                        string lhzb = "";
                        string dwmc = "";
                        int yssl = 0;
                        int wcsl = 0;
                        foreach (var zb in zbs)
                        {
                            isAudit = true;
                            lhzb += zb.LhzbModel.Zbxx.ZBMC + "、";
                            if (!khjgbms.Exists(p => p == zb.JGBM.ToString()))
                            {
                                dwmc += SysBmjgService.GetBmjgName(zb.JGBM.Value) + "、";
                                khjgbms.Add(zb.JGBM.ToString());
                            }
                            yssl += zb.KhJgbmdfbLst.Count(p => p.DFZ == userModel.JGBM.ToString());
                            wcsl += zb.KhJgbmdfbLst.Count(p => p.DFZ == userModel.JGBM.ToString() && p.DFSJ != null);
                        }
                        sb.Append("<td style=\"text-align: left;\">" + lhzb.TrimEnd("、".ToCharArray()) + "</td>");
                        sb.Append("<td style=\"text-align: left;\">" + dwmc.TrimEnd("、".ToCharArray()) + "</td>");
                        sb.Append("<td><span class=\"label label-default\">" + yssl + "</span></td>");
                        sb.Append("<td><span class=\"label label-success\">" + wcsl + "</span></td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("<tr>");
                    //打分指标
                    var dfzbList = dfzbmxSrv.GetListByKhid(lastKaohe.KHID.ToString()).
                        Where(p => ("," + p.DFZ).Contains("," + userModel.JGBM + ",")).ToList();
                    var dfzbmcLst = dfzbList.DistinctBy(p => p.DfzbModel.Zbxx.ZBMC).ToList();
                    var dfyjzbmcLst = dfzbList.DistinctBy(p => p.DfzbModel.Zbxx.YJZBMC).ToList();
                    for (int i = 0; i < dfyjzbmcLst.Count; i++)
                    {
                        List<string> dfjgbms = new List<string>();
                        sb.Append("<tr>");
                        sb.Append("<td>" + (i + 1) + "</td>");
                        sb.Append("<td>" + dfyjzbmcLst[i].DfzbModel.Zbxx.YJZBMC + "</td>");
                        var zbs = dfzbmcLst.Where(p => p.DfzbModel.Zbxx.YJZBMC == dfyjzbmcLst[i].DfzbModel.Zbxx.YJZBMC).ToList();
                        string dfzb = "";
                        string dwmc = "";
                        int yssl = 0;
                        int wcsl = 0;
                        foreach (var zb in zbs)
                        {
                            isAudit = true;
                            dfzb += zb.DfzbModel.Zbxx.ZBMC + "、";
                            if (!dfjgbms.Exists(p => p == zb.JGBM.ToString()))
                            {
                                dwmc += SysBmjgService.GetBmjgName(zb.JGBM.Value) + "、";
                                dfjgbms.Add(zb.JGBM.ToString());
                            }
                            yssl += zb.KhJgbmdfbLst.Count(p => p.DFZ == userModel.JGBM.ToString());
                            wcsl += zb.KhJgbmdfbLst.Count(p => p.DFZ == userModel.JGBM.ToString() && p.KHDF != null);
                        }
                        sb.Append("<td style=\"text-align: left;\">" + dfzb.TrimEnd("、".ToCharArray()) + "</td>");
                        sb.Append("<td style=\"text-align: left;\">" + dwmc.TrimEnd("、".ToCharArray()) + "</td>");
                        sb.Append("<td><span class=\"label label-default\">" + yssl + "</span></td>");
                        sb.Append("<td><span class=\"label label-success\">" + wcsl + "</span></td>");
                        sb.Append("</tr>");
                    }
                }
                khInfo = (isAudit) ? sb.ToString() : "";
            }
            if (WebKeys.EnableCaching && !string.IsNullOrEmpty(khInfo))
            {
                //数据存入缓存系统
                CacheHelper.Add(typeof(KhKhglService), true, null, "GetEjdwKaoheInfoByJgbm", 
                    new object[] { userModel }, KhKhglData.CacheClassKey + "_GetEjdwKaoheInfoByJgbm_" + userModel.JGBM, khInfo);
            }
            return khInfo;
        }

        /// <summary>
        /// 获取最新一期的二级单位及领导班子考核信息
        /// </summary>
        /// <returns></returns>
        public string GetEjdwKaoheInfo()
        {
            string khInfo = string.Empty;
            if (WebKeys.EnableCaching)
            {
                khInfo = (string)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetEjdwKaoheInfo");
            }
            if (string.IsNullOrEmpty(khInfo))
            {
                StringBuilder sb = new StringBuilder();
                //最近一期考核
                var lastKaohe = GetLastKaoheInfo("LX2014A");//二级单位及领导班子考核
                if (lastKaohe != null)
                {
                    sb.Append("<tr>");
                    sb.Append("<th class=\"td-bold\" colspan=\"4\">" + lastKaohe.KHMC + "-进展统计</th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style=\"width: 25%\">参评单位</td>");
                    sb.Append(
                        "<td style=\"width: 25%\" class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计1')\">"
                        + "<span class=\"red\">"
                        + GetKaoheProcessResult(lastKaohe, "参评单位") + "</span></a></td>");
                    sb.Append("<td style=\"width: 25%\">考核值确认</td>");
                    sb.Append("<td style=\"width: 25%\" class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计1')\">"
                        + "<span class=\"red\">"
                        + GetKaoheProcessResult(lastKaohe, "考核值确认") + "</span></a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>数据填报</td>");
                    sb.Append("<td class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计1')\">"
                        + "<span class=\"green\">"
                        + GetKaoheProcessResult(lastKaohe, "数据填报") + "</span></a></td>");
                    sb.Append("<td>文件提交</td>");
                    sb.Append("<td class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计1')\">"
                        + "<span class=\"green\">"
                        + GetKaoheProcessResult(lastKaohe, "文件提交") + "</span></a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>已审核</td>");
                    sb.Append("<td class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计1')\">"
                        + "<span class=\"yellow\">"
                        + GetKaoheProcessResult(lastKaohe, "已审核") + "</span></a></td>");
                    sb.Append("<td>已审定</td>");
                    sb.Append("<td class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计1')\">"
                        + "<span class=\"blue\">"
                        + GetKaoheProcessResult(lastKaohe, "已审定") + "</span></a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>考核进度</td>");
                    sb.Append("<td colspan=\"3\">");
                    sb.Append("<div class=\"progress progress-striped progress-success active\">");
                    int processV = GetKaoheProcessValue(lastKaohe);
                    sb.Append("<div class=\"progress-bar\" style=\"width: " + processV
                        + "%;\" onclick=\"ShowProgress('LX2014A','" + processV + "');\">" + processV + "%</div>");
                    sb.Append("</div>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    khInfo = sb.ToString();
                }
            }
            if (WebKeys.EnableCaching && !string.IsNullOrEmpty(khInfo))
            {
                ////数据存入缓存系统
                //CacheHelper.Add(KhKhglData.CacheClassKey + "_GetEjdwKaoheInfo", khInfo);
                //数据存入缓存系统
                CacheHelper.Add(typeof(KhKhglService), true, null, "GetEjdwKaoheInfo", null, KhKhglData.CacheClassKey + "_GetEjdwKaoheInfo", khInfo);
            }
            return khInfo;
        }

        /// <summary>
        /// 获取最新一期的机关部门及负责人考核信息
        /// </summary>
        /// <returns></returns>
        public string GetJgbmKaoheInfo()
        {
            string khInfo = string.Empty;
            if (WebKeys.EnableCaching)
            {
                khInfo = (string)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetJgbmKaoheInfo");
            }
            if (string.IsNullOrEmpty(khInfo))
            {
                StringBuilder sb = new StringBuilder();
                //最近一期考核
                var lastKaohe = GetLastKaoheInfo("LX2014B");//机关部门及负责人考核
                if (lastKaohe != null)
                {
                    sb.Append("<tr>");
                    sb.Append("<th class=\"td-bold\" colspan=\"4\">" + lastKaohe.KHMC + "-进展统计</th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style=\"width: 25%\">参评部门</td>");
                    sb.Append("<td style=\"width: 25%\" class=\"show\">"
                            + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计2')\">"
                            + "<span class=\"red\">"
                            + GetKaoheProcessResult(lastKaohe, "参评部门") + "</span></a></td>");
                    sb.Append("<td style=\"width: 25%\">自评完成</td>");
                    sb.Append("<td style=\"width: 25%\" class=\"show\">"
                            + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计2')\">"
                            + "<span class=\"red\">"
                            + GetKaoheProcessResult(lastKaohe, "自评完成") + "</span></a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>上级测评</td>");
                    sb.Append("<td class=\"show\">"
                            + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计2')\">"
                            + "<span class=\"green\">"
                            + GetKaoheProcessResult(lastKaohe, "上级测评") + "</span></a></td>");
                    sb.Append("<td>同级测评</td>");
                    sb.Append("<td class=\"show\">"
                            + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计2')\">"
                            + "<span class=\"green\">"
                            + GetKaoheProcessResult(lastKaohe, "同级测评") + "</span></a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>文件提交</td>");
                    sb.Append("<td class=\"show\">"
                            + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计2')\">"
                            + "<span class=\"yellow\">"
                            + GetKaoheProcessResult(lastKaohe, "文件提交") + "</span></a></td>");
                    sb.Append("<td>结果审定</td>");
                    sb.Append("<td class=\"show\">"
                            + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计2')\">"
                            + "<span class=\"blue\">"
                            + GetKaoheProcessResult(lastKaohe, "结果审定") + "</span></a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>考核进度</td>");
                    sb.Append("<td colspan=\"3\">");
                    sb.Append("<div class=\"progress progress-striped progress-success active\">");
                    int processV = GetKaoheProcessValue(lastKaohe);
                    sb.Append("<div class=\"progress-bar\" style=\"width: " + processV
                        + "%;\" onclick=\"ShowProgress('LX2014B','" + processV + "');\">" + processV + "%</div>");
                    sb.Append("</div>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    khInfo = sb.ToString();
                }
            }
            if (WebKeys.EnableCaching && !string.IsNullOrEmpty(khInfo))
            {
                ////数据存入缓存系统
                //CacheHelper.Add(KhKhglData.CacheClassKey + "_GetJgbmKaoheInfo", khInfo);
                //数据存入缓存系统
                CacheHelper.Add(typeof(KhKhglService), true, null, "GetJgbmKaoheInfo", null, KhKhglData.CacheClassKey + "_GetJgbmKaoheInfo", khInfo);
            }
            return khInfo;
        }

        #endregion

        #region 考核进展统计相关

        /// <summary>
        /// 获取指定考核下的各统计项目的结果
        /// </summary>
        /// <param name="kaohe">考核实例</param>
        /// <param name="xmmc">统计项目名称</param>
        /// <returns></returns>
        public int GetKaoheProcessResult(KhKhglModel kaohe, string xmmc)
        {
            int? r = 0;
            if (WebKeys.EnableCaching)
            {
                r = (int?)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetKaoheProcessResult_" + kaohe.KHID + "_" + xmmc);
            }
            if (r == null || r.Value == 0)
            {
                if (kaohe.LXID == "LX2014A")
                {
                    //二级单位及领导班子
                    switch (xmmc)
                    {
                        case "参评单位":
                            r = sjsbSrv.GetListByKhid(kaohe.KHID.ToString()).Count;
                            break;
                        case "考核值确认"://目标值确认日期不为空
                            r = dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).
                                Where(p => p.MBZQRRQ != null).DistinctBy(p => p.JGBM).Count();
                            break;
                        case "数据填报":
                            r = dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).
                                Where(p => p.WCZSQRQ != null).DistinctBy(p => p.JGBM).Count();
                            break;
                        case "文件提交":
                            r = sjsbSrv.GetListByKhid(kaohe.KHID.ToString()).Where(p => p.SBSJ != null).Count();
                            break;
                        case "已审核":
                            r = dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).
                                Where(p => p.WCZSHRQ != null).DistinctBy(p => p.JGBM).Count();
                            break;
                        case "已审定":
                            r = dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).
                                Where(p => p.WCZSDRQ != null).DistinctBy(p => p.JGBM).Count();
                            break;
                    }
                }
                else if (kaohe.LXID == "LX2014B")
                {
                    var list = jgbmdfbSrv.GetListByKhid(kaohe.KHID.ToString());
                    //机关部门及负责人
                    switch (xmmc)
                    {
                        case "参评部门":
                            r = sjsbSrv.GetListByKhid(kaohe.KHID.ToString()).Count;
                            break;
                        case "自评完成"://从打分表中统计JGBM与DFZ一致的数量
                            r = list.Where(p => p.JGBM.ToString() == p.DFZ && p.KHDF != null).DistinctBy(p => p.JGBM).Count();
                            break;
                        case "上级测评"://打分者类型为1，同时KHDF不为空的单位的数量
                            r = list.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.上级领导).ToString() && p.KHDF != null)
                                .DistinctBy(p => p.JGBM).Count();
                            break;
                        case "同级测评":
                            r = list.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.同级二级单位).ToString() && p.KHDF != null)
                                .DistinctBy(p => p.JGBM).Count();
                            break;
                        case "文件提交":
                            r = sjsbSrv.GetListByKhid(kaohe.KHID.ToString()).Where(p => p.SBSJ != null).Count();
                            break;
                        case "结果审定":
                            r = jgbmkhdfSrv.GetListByKhid(kaohe.KHID.ToString()).
                                Where(p => !string.IsNullOrEmpty(p.HZBZ)).DistinctBy(p => p.JGBM).Count();
                            break;
                    }
                }
            }
            if (WebKeys.EnableCaching && r.ToInt() > 0)
            {
                //数据存入缓存系统
                CacheHelper.Add(KhKhglData.CacheClassKey + "_GetKaoheProcessResult_" + kaohe.KHID + "_" + xmmc, r);
            }
            return r.ToInt();
        }

        /// <summary>
        /// 获取指定考核期的进度值
        /// </summary>
        /// <param name="kaohe"></param>
        /// <returns></returns>
        public int GetKaoheProcessValue(KhKhglModel kaohe)
        {

            /*
             检测当前的考核的运行状态，并自动更新状态和显示进度值
             * 1、二级单位考核
             *  绩效责任书,20 30 40 50 60 85 100
                目标值确认,
                完成值录入,
                约束和加分指标打分,
                领导班子打分,
                完成值和得分审核,
                考核结果审定
             * 2、机关部门考核
             *  绩效责任书,20 40 60 80 90 100
                自评打分完成,
                同级打分完成,
                上级打分完成,
                连带指标打分,
                考核结果审定
             */
            int? r = 0;
            if (WebKeys.EnableCaching)
            {
                r = (int?)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetKaoheProcessValue_" + kaohe.KHID);
            }
            if (r == null || r.Value == 0)
            {
                if (kaohe.LXID == "LX2014A")
                {
                    //二级单位
                    if (jxzrszbSrv.GetListByNdAndBbmc(kaohe.KHND.ToString(), kaohe.BBMC).Count > 0)
                        r = 20;//绩效责任书
                    if (dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).Where(p => p.MBZQRRQ != null).Count() > 0)
                        r = 30;//目标值确认
                    if (dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).Where(p => p.WCZSQRQ != null).Count() > 0)
                        r = 40;//完成值录入
                    if (dfzbmxSrv.GetListByKhidAndKhdx(kaohe.KHID.ToString(),
                        WebKeys.KaoheType.二级单位).Where(p => p.DFRQ != null).Count() > 0)
                        r = 50;//约束和加分指标打分
                    if (dfzbmxSrv.GetListByKhidAndKhdx(kaohe.KHID.ToString(),
                        WebKeys.KaoheType.领导班子).Where(p => p.DFRQ != null).Count() > 0)
                        r = 60;//领导班子打分
                    if (dlzbmxSrv.GetListByKhid(kaohe.KHID.ToString()).Where(p => p.WCZSHRQ != null).Count() > 0)
                        r = 85;//完成值和得分审核
                    if (ejdwkhdfSrv.GetListByKhid(kaohe.KHID.ToString()).Where(p => !string.IsNullOrEmpty(p.HZBZ)).Count() > 0)
                        r = 100;//考核结果审定
                }
                else if (kaohe.LXID == "LX2014B")
                {
                    //机关部门
                    //绩效责任书,20 40 60 80 90 100
                    if (jxzrszbSrv.GetListByNdAndBbmc(kaohe.KHND.ToString(), kaohe.BBMC).Count > 0)
                        r = 20;//绩效责任书20
                    var list = jgbmdfbSrv.GetListByKhid(kaohe.KHID.ToString());
                    if (list.Where(p => p.JGBM.ToString() == p.DFZ && p.KHDF != null).DistinctBy(p => p.JGBM).Count() > 0)
                        r = 40;//自评打分完成,40
                    if (list.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.同级二级单位).ToString() && p.KHDF != null)
                                .DistinctBy(p => p.JGBM).Count() > 0)
                        r = 60;//同级打分完成,60
                    if (list.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.上级领导).ToString() && p.KHDF != null)
                                .DistinctBy(p => p.JGBM).Count() > 0)
                        r = 80;//上级打分完成,80
                    if (dfzbmxSrv.GetListByKhidAndKhdx(kaohe.KHID.ToString(),
                        WebKeys.KaoheType.部门负责人).Where(p => p.DFRQ != null).Count() > 0)
                        r = 90;//连带指标打分,90
                    if (jgbmkhdfSrv.GetListByKhid(kaohe.KHID.ToString()).
                                Where(p => !string.IsNullOrEmpty(p.HZBZ)).DistinctBy(p => p.JGBM).Count() > 0)
                        r = 100;//考核结果审定100
                }
            }
            if (WebKeys.EnableCaching && r.ToInt() > 0)
            {
                //数据存入缓存系统
                CacheHelper.Add(KhKhglData.CacheClassKey + "_GetKaoheProcessValue_" + kaohe.KHID, r);
            }
            return r.ToInt();
        }

        #endregion

        #region 自定义方法


        /// <summary>
        /// 提取指定类型下的最近一期考核考核（以开始时间为准）
        /// </summary>
        /// <param name="khlx">考核类型</param>
        /// <returns></returns>
        public KhKhglModel GetLastKaoheInfo(string khlx)
        {
            //string hql = "from KhKhglModel p where p.LXID='" + khlx + "' order by p.KSSJ desc";
            return GetKhListByLxidForIndex(khlx).FirstOrDefault();
        }

        /// <summary>
        /// 提取状态为进行中和考核开始时间为当前年度的所有考核
        /// </summary>
        /// <returns></returns>
        public IList<KhKhglModel> GetKhListForIndex()
        {
            List<KhKhglModel> khList = null;
            if (WebKeys.EnableCaching)
            {
                khList = (List<KhKhglModel>)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetKhListForIndex");
            }
            if (khList == null || khList.Count == 0)
            {
                string hql = "from KhKhglModel p where p.KHZT='0' or Year(p.KSSJ)='" + DateTime.Now.Year + "' order by p.KSSJ desc";
                khList = GetListByHQL(hql) as List<KhKhglModel>;
                if (khList.Count == 0)
                {
                    //二级单位考核
                    var ejdwkh = GetList().FirstOrDefault(p => p.LXID == "LX2014A");
                    if (ejdwkh != null)
                    {
                        khList.Add(ejdwkh);
                    }
                    //机关部门考核
                    var jgbmkh = GetList().FirstOrDefault(p => p.LXID == "LX2014B");
                    if (jgbmkh != null)
                    {
                        khList.Add(jgbmkh);
                    }
                }
                //存入缓存中
                if (WebKeys.EnableCaching && khList.Count > 0)
                {
                    ////数据存入缓存系统
                    //CacheHelper.Add(KhKhglData.CacheClassKey + "_GetKhListForIndex", khList);
                    //数据存入缓存系统
                    CacheHelper.Add(typeof(KhKhglService), true, null, "GetKhListForIndex", null, KhKhglData.CacheClassKey + "_GetKhListForIndex", khList);
                }
            }
            return khList;
        }

        /// <summary>
        /// 提取状态为进行中和考核开始时间为当前年度的指定考核类型的信息
        /// </summary>
        /// <param name="lxid">类型ID</param>
        /// <returns></returns>
        public IList<KhKhglModel> GetKhListByLxidForIndex(string lxid)
        {
            List<KhKhglModel> khList = null;
            if (WebKeys.EnableCaching)
            {
                khList = (List<KhKhglModel>)CacheHelper.GetCache(KhKhglData.CacheClassKey + "_GetKhListByLxidForIndex_" + lxid);
            }
            if (khList == null || khList.Count > 0)
            {
                string hql = "from KhKhglModel p where p.KHZT='0' or Year(p.KSSJ)='" + DateTime.Now.Year + "' order by p.KSSJ desc";
                khList = GetListByHQL(hql).Where(p => p.LXID == lxid).ToList();
                if (khList.Count == 0)
                {
                    if (lxid == "LX2014A")
                    {
                        //二级单位考核
                        var ejdwkh = GetList().FirstOrDefault(p => p.LXID == "LX2014A");
                        if (ejdwkh != null)
                        {
                            khList.Add(ejdwkh);
                        }
                    }
                    else if (lxid == "LX2014B")
                    {
                        //机关部门考核
                        var jgbmkh = GetList().FirstOrDefault(p => p.LXID == "LX2014B");
                        if (jgbmkh != null)
                        {
                            khList.Add(jgbmkh);
                        }
                    }
                }
                //存入缓存中
                if (WebKeys.EnableCaching && khList.Count > 0)
                {
                    ////数据存入缓存系统
                    //CacheHelper.Add(KhKhglData.CacheClassKey + "_GetKhListByLxidForIndex_" + lxid, khList);
                    //数据存入缓存系统
                    CacheHelper.Add(typeof(KhKhglService), true, null, "GetKhListByLxidForIndex", new object[] { lxid }, KhKhglData.CacheClassKey + "_GetKhListByLxidForIndex_" + lxid, khList);
                }
            }
            return khList;
        }

        /// <summary>
        /// 获取指定版本下的所有考核信息
        /// </summary>
        /// <param name="zbbb">版本名称</param>
        /// <returns></returns>
        public IList<KhKhglModel> GetKhListByZbbb(string zbbb)
        {
            string hql = "from KhKhglModel p where p.BBMC='" + zbbb + "' order by p.KSSJ desc";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取指定年度下的所有考核信息
        /// </summary>
        /// <param name="year">年度</param>
        /// <returns></returns>
        public IList<KhKhglModel> GetKhListByYear(string year)
        {
            string hql = "from KhKhglModel p where p.KHND='" + year + "' order by p.KSSJ desc";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取可查询的所有考核信息
        /// </summary>
        /// <returns></returns>
        public IList<KhKhglModel> GetKhListForValid()
        {
            //状态为可查询和进行中的考核=有效考核
            string hql = "from KhKhglModel p where p.SFKC='1' or p.KHZT='0' order by p.KHND desc, p.KSSJ desc";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 绑定带分组功能的考核期下拉控件
        /// </summary>
        /// <param name="ddl">下拉控件</param>
        /// <param name="kaoheLst">考核期</param>
        public void BindSSECDropDownListForKaohe(SSECDropDownList ddl, IList<KhKhglModel> kaoheLst)
        {
            ddl.Items.Clear();
            string khnd = string.Empty;
            foreach (var q in kaoheLst)
            {
                if (string.IsNullOrEmpty(khnd))
                {
                    ddl.Items.Add(new ListItem(q.KHND + "年度", "optgroup"));
                    ddl.Items.Add(new ListItem(q.KHMC, q.KHID.ToString()));
                    khnd = q.KHND;
                }
                else if (q.KHND == khnd)
                {
                    ddl.Items.Add(new ListItem(q.KHMC, q.KHID.ToString()));
                }
                else
                {
                    ddl.Items.Add(new ListItem(q.KHND + "年度", "optgroup"));
                    ddl.Items.Add(new ListItem(q.KHMC, q.KHID.ToString()));
                    khnd = q.KHND;
                }
            }
            //ddl.Items.Insert(0, new ListItem("所有考核期", ""));
        }

        #endregion

        #region 正式发起考核数据初始化

        /// <summary>
        /// 将新指标追加到指定的考核期和机构下
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="jgbm">机构编码</param>
        public void AppendKaoheZhibiao(string khid, string jgbm)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            var bmjgs = bmjgSrv.GetBmjgLisByParentJgbm(jgbm);
            foreach (var bmjg in bmjgs)
            {
                jgbm = bmjg.JGBM.ToString();
                //1==获取指标筛选表中的数据
                KhZbsxService zbsxSrv = new KhZbsxService();
                List<KhZbsxModel> zbsxLst = zbsxSrv.GetListByKaoheAndJgbm(khid, jgbm) as List<KhZbsxModel>;
                if (zbsxLst.Count == 0) 
                    continue;
                /*
                 * 思路：根据指标编码从考核表中检索已有数据，只追加未包含的指标项    
                 * 涉及到的数据表：
                 * PERFO_KH_UNLOCK 数据解锁表
                 * PERFO_KH_SJSB 考核文件上报表
                 * PERFO_KH_LHZBJCSJ 量化考核基础数据表
                 * PERFO_KH_JGBMDFB 机关部门打分表
                 * PERFO_KH_DLZBMX 定量指标考核表
                 * PERFO_KH_DFZBMX 打分指标考核表
                 */
                //2==追加初始数据
                var qUnloack = unlockSrv.GetListByKhid(khid).FirstOrDefault(p => p.JGBM == jgbm.ToInt());
                if (qUnloack == null)
                {
                    //数据解锁表
                    KhUnlockModel unlockM = new KhUnlockModel();
                    unlockM.DB_Option_Action = WebKeys.InsertAction;
                    unlockM.KHID = khid.ToInt();
                    unlockM.JGBM = jgbm.ToInt();
                    unlockM.SID = CommonTool.GetGuidKey();
                    unlockM.SDBZ = "0";
                    unlockSrv.Execute(unlockM);
                }
                var qSjsb = sjsbSrv.GetListByKhid(khid).FirstOrDefault(p => p.JGBM == jgbm.ToInt());
                if (qSjsb == null)
                {
                    //考核文件上报表
                    KhSjsbModel sjsbM = new KhSjsbModel();
                    sjsbM.DB_Option_Action = WebKeys.InsertAction;
                    sjsbM.KHID = khid.ToInt();
                    sjsbM.JGBM = jgbm.ToInt();
                    sjsbM.SBZT = "0";
                    sjsbSrv.Execute(sjsbM);
                }
                //3==追加到定量和打分指标考核表------------------------------------------------------------
                var dlzbmxList = dlzbmxSrv.GetListByKhidAndJgbm(khid, jgbm).ToList();//定量指标
                var dfzbmxList = dfzbmxSrv.GetListByKhidAndJgbm(khid, jgbm).ToList();//打分指标
                foreach (var xszb in zbsxLst)
                {
                    //该指标为新加
                    if (!dlzbmxList.Exists(p => p.ZBBM == xszb.SXZBBM) &&
                        !dfzbmxList.Exists(p => p.ZBBM == xszb.SXZBBM))
                    {
                        //加载指标到考核表
                        loadKhZbsxInKaohe(khid, xszb);
                    }
                    else
                    {
                        //指标已存在，则检查打分表和基础表是否已存在数据=======================
                        var dlzb = dlzbmxList.FirstOrDefault(p => p.ZBBM == xszb.SXZBBM);
                        if (dlzb != null)
                        {
                            //定量指标,进量化指标基础数据表
                            loadKhZbsxInJcsjData(khid, xszb, dlzb, null);
                        }
                        else
                        {
                            var dfzb = dfzbmxList.FirstOrDefault(p => p.ZBBM == xszb.SXZBBM);
                            if (dfzb != null)
                            {
                                //打分指标，进机关部门打分表
                                loadKhZbsxInJcsjData(khid, xszb, null, dfzb);
                            }
                        }
                    }
                }
                Debuger.GetInstance().log("AppendKaoheZhibiao(" + khid + "," + jgbm + ")==>追加定量和打分指标考核表数据成功!");
            }
        }

        /// <summary>
        /// 根据考核ID删除相关考核表的数据
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <returns></returns>
        public bool DeleteAllDataByKhid(string khid)
        {
            StringBuilder sqlSB = new StringBuilder();
            //==先删除已有数据
            sqlSB.Append("delete from [PERFO_KH_UNLOCK] where KHID='" + khid + "';");//数据解锁表
            sqlSB.Append("delete from [PERFO_KH_SJSB] where KHID='" + khid + "';");//考核文件上报表
            sqlSB.Append("delete from [PERFO_KH_ZZJG] where KHID='" + khid + "';");//考核结果表
            sqlSB.Append("delete from [PERFO_KH_ZZJGFILE] where KHID='" + khid + "';");//结果文件表
            sqlSB.Append("delete from [PERFO_KH_TJB] where KHID='" + khid + "';");//考核进度统计表 
            sqlSB.Append("delete from [PERFO_KH_LHZBJCSJ] where KHID='" + khid + "';");//量化考核基础数据表
            sqlSB.Append("delete from [PERFO_KH_JGBMDFB] where KHID='" + khid + "';");//机关部门打分表
            sqlSB.Append("delete from [PERFO_KH_DLZBMX] where KHID='" + khid + "';");//定量指标考核表
            sqlSB.Append("delete from [PERFO_KH_DFZBMX] where KHID='" + khid + "';");//打分指标考核表
            sqlSB.Append("delete from [PERFO_KH_MESSAGE] where KHID='" + khid + "';");//待办箱
            sqlSB.Append("delete from [PERFO_KH_JGZFB] where KHID='" + khid + "';");//机关建设作风表
            sqlSB.Append("delete from [PERFO_KH_JGBMKHDF] where KHID='" + khid + "';");//机关部门考核得分表
            sqlSB.Append("delete from [PERFO_KH_EJDWKHDF] where KHID='" + khid + "';");//二级单位考核得分表
            sqlSB.Append("delete from [PERFO_KH_HBJFGZ] where KHID='" + khid + "';");//合并计分规则表
            sqlSB.Append("delete from [PERFO_KH_KHDFPXFW] where KHID='" + khid + "';");//考核得分排序汇围表
            sqlSB.Append("delete from [PERFO_KH_NODERUN] where KHID='" + khid + "';");//考核节点运行表
            return dal.ExecuteSQL(sqlSB.ToString());
        }

        /// <summary>
        /// 启动考核并完成数据的初始化
        /// </summary>
        /// <param name="khid">考核ID</param>
        public void LaunchKaohe(string khid)
        {
            //先删除数据
            if (DeleteAllDataByKhid(khid))
            {
                Debuger.GetInstance().log("LaunchKaohe(" + khid + ")==>1==删除已有数据成功!");
            }
            //1==获取指标筛选表中的数据
            KhZbsxService zbsxSrv = new KhZbsxService();
            var khModel = GetSingle(khid);
            List<KhZbsxModel> zbsxLst = zbsxSrv.GetListByKaohe(khid) as List<KhZbsxModel>;
            var khBmjg = zbsxLst.Distinct<KhZbsxModel>(new FastPropertyComparer<KhZbsxModel>("JGBM"));
            if (khModel != null && zbsxLst.Count > 0)
            {
                khModel.DB_Option_Action = WebKeys.UpdateAction;
                khModel.KHZT = "0";//进行中
                khModel.BBMC = zbsxLst.First().JxzrsZb.BBMC;
                Execute(khModel);//更新版本名称
            }
            //2==添加初始数据
            KhUnlockService unlockSrv = new KhUnlockService();
            KhSjsbService sjsbSrv = new KhSjsbService();
            foreach (var q in khBmjg)
            {
                //bbmc = q.JxzrsZb.BBMC;
                //sznd = q.JxzrsZb.ZSZND.ToRequestString();
                //数据解锁表
                KhUnlockModel unlockM = new KhUnlockModel();
                unlockM.DB_Option_Action = WebKeys.InsertAction;
                unlockM.KHID = khid.ToInt();
                unlockM.JGBM = q.SXJGBM;
                unlockM.SID = CommonTool.GetGuidKey();
                unlockM.SDBZ = "0";
                unlockSrv.Execute(unlockM);
                //考核文件上报表
                KhSjsbModel sjsbM = new KhSjsbModel();
                sjsbM.DB_Option_Action = WebKeys.InsertAction;
                sjsbM.KHID = khid.ToInt();
                sjsbM.JGBM = q.SXJGBM;
                sjsbM.SBZT = "0";
                sjsbSrv.Execute(sjsbM);

                //4==添加财务基础数据--------------------------------
                if (khModel != null)
                {
                    var cwjcsjThisYear = cwjcsjSrv.GetListByJgbmAndNF(q.JGBM, khModel.KHND.ToInt());
                    if (cwjcsjThisYear == null || cwjcsjThisYear.Count == 0)
                    {
                        //添加考核年数据
                        cwjcsjSrv.InitJcsjDataBySzndAndJgbm(khModel.KHND.ToInt(), q.JGBM.ToInt());
                    }
                    var cwjcsjPrevYear = cwjcsjSrv.GetListByJgbmAndNF(q.JGBM, khModel.KHND.ToInt() - 1);
                    if (cwjcsjPrevYear == null || cwjcsjPrevYear.Count == 0)
                    {
                        //添加上一年数据
                        cwjcsjSrv.InitJcsjDataBySzndAndJgbm(khModel.KHND.ToInt() - 1, q.JGBM.ToInt());
                    }
                    Debuger.GetInstance().log("LaunchKaoheByJgbm(" + khid + "," + q.JGBM + ")==>4==添加财务基础数据成功!=====");
                    //5==发送待办消息-----------------------------------------------
                    //发起考核后，机关部门自评
                    if (khModel.LXID == "LX2014B")
                    {
                        //机关部门考核，自评
                        var u = userSrv.GetUserListForValid().FirstOrDefault(p => p.JGBM == q.SXJGBM && p.DUTY.Contains("员工"));
                        if (u != null)
                        {
                            msgSrv.SendMsgToUers(u.LOGINID, q.SXJGBM,
                            "机关部门自评", "需要您进行【" + khModel.KHMC + "】【" + u.Bmjg.JGMC + "】的自评打分操作!",
                            string.Format("/Module/Kh/JgbmWczZiping.aspx?BM={0}&KH={1}", u.JGBM, khModel.KHID),
                            "1113", u.JGBM.ToString());
                        }
                    }
                }        

            }
            Debuger.GetInstance().log("LaunchKaohe(" + khid + ")==>2==添加数据解锁和考核文件上报数据成功!");
            //3==定量和打分指标考核表----------------------------------------------------------------------
            //筛选的考核指标
            foreach (var xszb in zbsxLst)
            {
                //加载指标到考核表
                loadKhZbsxInKaohe(khid, xszb);
                #region 注释
                ////进 定量指标考核表
                //if (!string.IsNullOrEmpty(xszb.JxzrsZb.LHZBBM))
                //{
                //    switch (xszb.Kaohe.LXID)
                //    {
                //        case "LX2014A"://二级单位及领导班子考核
                //            dlzbmxSrv.LoadZrszbModelInDlzbmx(khid, WebKeys.KaoheType.二级单位, xszb);
                //            break;
                //        case "LX2014B"://机关部门及负责人考核
                //            dlzbmxSrv.LoadZrszbModelInDlzbmx(khid, WebKeys.KaoheType.机关部门, xszb);
                //            break;
                //        default:
                //            break;
                //    }
                //}
                ////进 打分指标考核表
                //else if (!string.IsNullOrEmpty(xszb.JxzrsZb.DFZBBM))
                //{

                //    switch (xszb.Kaohe.LXID)
                //    {
                //        case "LX2014A"://二级单位及领导班子考核
                //            if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("加减分"))
                //            {
                //                //加减分只领导班子考核
                //                dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.领导班子, xszb);
                //            }
                //            else
                //            {
                //                dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.二级单位, xszb);
                //                //约束性指标领导班子也考核
                //                if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("约束"))
                //                {
                //                    dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.领导班子, xszb);
                //                }
                //            }
                //            break;
                //        case "LX2014B"://机关部门及负责人考核
                //            if (xszb.JxzrsZb.Dfzb.Zbxx.ZBMC.Contains("总部连带"))
                //            {
                //                //只考核负责人
                //                dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.部门负责人, xszb);
                //            }
                //            else
                //            {
                //                dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.机关部门, xszb);
                //            }
                //            break;
                //        default:
                //            break;
                //    }
                //}
                #endregion
            }
            Debuger.GetInstance().log("LaunchKaohe(" + khid + ")==>3==添加定量和打分指标考核表数据成功!");
            //5==发送待办消息-------------------------------------------------------------
            //发起考核后，二级单位考核，提取所有参与的考核的职能部门，并给他们发送待办消息
            if (khModel != null && khModel.LXID == "LX2014A")
            {
                var lst = jgbmdfbSrv.GetDlzbListByKhid(khModel.KHID.ToString()).
                    Where(p => !string.IsNullOrEmpty(p.ID)).DistinctBy(p=>p.DFZ).ToList();
                foreach (var q in lst)
                {
                    int jgbm = q.DFZ.ToInt();
                    if (jgbm == 0) continue;
                    var u = userSrv.GetUserListForValid().FirstOrDefault(p => p.JGBM == jgbm && p.DUTY.Contains("员工"));
                    if (u != null)
                    {
                        msgSrv.SendMsgToUers(u.LOGINID, q.JGBM.Value,
                        "考核值设定", "需要您设定【" + khModel.KHMC + "】的部分单位的考核值!",
                        string.Format("/Module/Kh/EjdwMbzLuru.aspx?BM={0}&KH={1}", q.JGBM, khModel.KHID),
                        "1011", u.JGBM.ToString());
                    }
                }
            }
        }

        #endregion

        #region 限定机构和考核期的数据初始化

        /// <summary>
        /// 根据考核期和机构编码删除相关考核表的数据
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public bool DeleteAllDataByKhidAndJgbm(string khid, string jgbm)
        {
            /*
            * 涉及到的数据表：10个
            * PERFO_KH_UNLOCK 数据解锁表
            * PERFO_KH_SJSB 考核文件上报表
            * PERFO_KH_LHZBJCSJ 量化考核基础数据表
            * PERFO_KH_JGBMDFB 机关部门打分表
            * PERFO_KH_DLZBMX 定量指标考核表
            * PERFO_KH_DFZBMX 打分指标考核表
            * PERFO_KH_MESSAGE 待办消息
            * PERFO_KH_ZZJG 考核结果表
            * PERFO_KH_JGZFB 机关建设作风表
            * PERFO_KH_JGBMKHDF 机关部门考核得分表
             * PERFO_KH_EJDWKHDF 二级单位考核得分表
            */
            StringBuilder sqlSB = new StringBuilder();
            //==先删除已有数据
            sqlSB.Append("delete from [PERFO_KH_UNLOCK] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//数据解锁表
            sqlSB.Append("delete from [PERFO_KH_SJSB] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//考核文件上报表
            sqlSB.Append("delete from [PERFO_KH_ZZJG] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//考核结果表
            sqlSB.Append("delete from [PERFO_KH_LHZBJCSJ] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//量化考核基础数据表
            sqlSB.Append("delete from [PERFO_KH_JGBMDFB] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//机关部门打分表
            sqlSB.Append("delete from [PERFO_KH_DLZBMX] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//定量指标考核表
            sqlSB.Append("delete from [PERFO_KH_DFZBMX] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//打分指标考核表
            sqlSB.Append("delete from [PERFO_KH_MESSAGE] where KHID='" + khid + "' and FSR='" + jgbm + "';");//待办箱
            sqlSB.Append("delete from [PERFO_KH_JGZFB] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//机关建设作风表
            sqlSB.Append("delete from [PERFO_KH_JGBMKHDF] where KHID='" + khid + "' and JGBM='" + jgbm + "';");//机关部门考核得分表
            sqlSB.Append("delete from [PERFO_KH_EJDWKHDF] where KHID='" + khid + "';");//二级单位考核得分表
            return dal.ExecuteSQL(sqlSB.ToString());
        }

        /// <summary>
        /// 启动指定考核期和指定单位的绩效考核
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="jgbm">机构编码</param>
        public void LaunchKaoheByJgbm(string khid, string jgbm)
        {
            //先删除数据
            if (DeleteAllDataByKhidAndJgbm(khid, jgbm))
            {
                Debuger.GetInstance().log("LaunchKaoheByJgbm(" + khid + "," + jgbm + ")==>1==删除已有数据成功!");
            }
            //1==获取指标筛选表中的数据
            KhZbsxService zbsxSrv = new KhZbsxService();
            List<KhZbsxModel> zbsxLst = zbsxSrv.GetListByKaoheAndJgbm(khid, jgbm) as List<KhZbsxModel>;
            var khModel = GetSingle(khid);
            if (khModel != null && zbsxLst.Count > 0)
            {
                khModel.DB_Option_Action = WebKeys.UpdateAction;
                khModel.KHZT = "0";//进行中
                khModel.BBMC = zbsxLst.First().JxzrsZb.BBMC;
                Execute(khModel);//更新版本名称
            }
            //2==添加初始数据
            KhUnlockService unlockSrv = new KhUnlockService();
            KhSjsbService sjsbSrv = new KhSjsbService();
            //数据解锁表
            KhUnlockModel unlockM = new KhUnlockModel();
            unlockM.DB_Option_Action = WebKeys.InsertAction;
            unlockM.KHID = khid.ToInt();
            unlockM.JGBM = jgbm.ToInt();
            unlockM.SID = CommonTool.GetGuidKey();
            unlockM.SDBZ = "0";
            unlockSrv.Execute(unlockM);
            //考核文件上报表
            KhSjsbModel sjsbM = new KhSjsbModel();
            sjsbM.DB_Option_Action = WebKeys.InsertAction;
            sjsbM.KHID = khid.ToInt();
            sjsbM.JGBM = jgbm.ToInt();
            sjsbM.SBZT = "0";
            sjsbSrv.Execute(sjsbM);
            Debuger.GetInstance().log("LaunchKaoheByJgbm(" + khid + "," + jgbm + ")==>2==添加数据解锁和考核文件上报数据成功!");
            //3==定量和打分指标考核表----------------------------------------------------------------------
            //筛选的考核指标
            foreach (var xszb in zbsxLst)
            {
                //加载指标到考核表
                loadKhZbsxInKaohe(khid, xszb);
            }
            Debuger.GetInstance().log("LaunchKaoheByJgbm(" + khid + "," + jgbm + ")==>3==添加定量和打分指标考核表数据成功!");
            //4==添加财务基础数据---------------------------------------------------------------------------
            if (khModel != null)
            {
                var cwjcsjThisYear = cwjcsjSrv.GetListByJgbmAndNF(jgbm, khModel.KHND.ToInt());
                if (cwjcsjThisYear == null || cwjcsjThisYear.Count == 0)
                {
                    //添加考核年数据
                    cwjcsjSrv.InitJcsjDataBySzndAndJgbm(khModel.KHND.ToInt(), jgbm.ToInt());
                }
                var cwjcsjPrevYear = cwjcsjSrv.GetListByJgbmAndNF(jgbm, khModel.KHND.ToInt() - 1);
                if (cwjcsjPrevYear == null || cwjcsjPrevYear.Count == 0)
                {
                    //添加上一年数据
                    cwjcsjSrv.InitJcsjDataBySzndAndJgbm(khModel.KHND.ToInt() - 1, jgbm.ToInt());
                }
            }
            Debuger.GetInstance().log("LaunchKaoheByJgbm(" + khid + "," + jgbm + ")==>4==添加财务基础数据成功!");
            //5==发送待办消息-------------------------------------------------------------
            if (khModel != null)
            {
                //发起考核后，机关部门自评
                if (khModel.LXID == "LX2014B")
                {
                    //机关部门考核，自评
                    var u = userSrv.GetUserListForValid().FirstOrDefault(p => p.JGBM.ToString() == jgbm && p.DUTY.Contains("员工"));
                    if (u != null)
                    {
                        msgSrv.SendMsgToUers(u.LOGINID, u.JGBM,
                        "机关部门自评", "需要您进行【" + khModel.KHMC + "】【" + u.Bmjg.JGMC + "】的自评打分操作!",
                        string.Format("/Module/Kh/JgbmWczZiping.aspx?BM={0}&KH={1}", u.JGBM, khModel.KHID),
                        "1113", u.JGBM.ToString());
                    }
                }
                //发起考核后，二级单位考核，提取所有参与的考核的职能部门，并给他们发送待办消息
                else if (khModel.LXID == "LX2014A")
                {
                    var lst = jgbmdfbSrv.GetDlzbListByKhid(khModel.KHID.ToString()).
                        Where(p => !string.IsNullOrEmpty(p.ID)).DistinctBy(p => p.DFZ).ToList();
                    foreach (var q in lst)
                    {
                        var u = userSrv.GetUserListForValid().FirstOrDefault(p => p.JGBM.ToString() == q.DFZ && p.DUTY.Contains("员工"));
                        if (u != null)
                        {
                            msgSrv.SendMsgToUers(u.LOGINID, q.JGBM.Value,
                            "考核值设定", "需要您设定【" + khModel.KHMC + "】的部分单位的考核值!",
                            string.Format("/Module/Kh/EjdwMbzLuru.aspx?BM={0}&KH={1}", q.JGBM, khModel.KHID),
                            "1011", u.JGBM.ToString());
                        }
                    }
                }
            }
        }

        #endregion

        #region 专用方法区

        /// <summary>
        /// 加载部门打分和量化指标基础数据信息
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="xszb">指标筛选MODEL</param>
        /// <param name="dlzb">定量指标</param>
        /// <param name="dfzb">打分指标</param>
        private void loadKhZbsxInJcsjData(string khid, KhZbsxModel xszb, KhDlzbmxModel dlzb, KhDfzbmxModel dfzb)
        {
            //进 定量指标考核表
            if (dlzb != null)
            {
                switch (xszb.Kaohe.LXID)
                {
                    case "LX2014A"://二级单位及领导班子考核
                        dlzbmxSrv.LoadZrszbModelInDlzbmxByDlzb(khid, WebKeys.KaoheType.二级单位, xszb, dlzb);
                        break;
                    case "LX2014B"://机关部门及负责人考核
                        dlzbmxSrv.LoadZrszbModelInDlzbmxByDlzb(khid, WebKeys.KaoheType.机关部门, xszb, dlzb);
                        break;
                    default:
                        break;
                }
            }
            //进 打分指标考核表
            else if (dfzb != null)
            {
                switch (xszb.Kaohe.LXID)
                {
                    case "LX2014A"://二级单位及领导班子考核
                        if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("加减分"))
                        {
                            //加减分只领导班子考核
                            dfzbmxSrv.LoadZrszbModelInDfzbmxByDfzb(khid, WebKeys.KaoheType.领导班子, xszb, dfzb);
                        }
                        else
                        {
                            dfzbmxSrv.LoadZrszbModelInDfzbmxByDfzb(khid, WebKeys.KaoheType.二级单位, xszb, dfzb);
                            //约束性指标领导班子也考核
                            if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("约束"))
                            {
                                dfzbmxSrv.LoadZrszbModelInDfzbmxByDfzb(khid, WebKeys.KaoheType.领导班子, xszb, dfzb);
                            }
                        }
                        break;
                    case "LX2014B"://机关部门及负责人考核
                        if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("连带"))
                        {
                            //只考核负责人
                            dfzbmxSrv.LoadZrszbModelInDfzbmxByDfzb(khid, WebKeys.KaoheType.部门负责人, xszb, dfzb);
                        }
                        else
                        {
                            dfzbmxSrv.LoadZrszbModelInDfzbmxByDfzb(khid, WebKeys.KaoheType.机关部门, xszb, dfzb);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 加载指定的考核指标到相关的考核表中
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="xszb">指标筛选MODEL</param>
        private void loadKhZbsxInKaohe(string khid, KhZbsxModel xszb)
        {
            //进 定量指标考核表
            if (!string.IsNullOrEmpty(xszb.JxzrsZb.LHZBBM))
            {
                switch (xszb.Kaohe.LXID)
                {
                    case "LX2014A"://二级单位及领导班子考核
                        dlzbmxSrv.LoadZrszbModelInDlzbmx(khid, WebKeys.KaoheType.二级单位, xszb);
                        break;
                    case "LX2014B"://机关部门及负责人考核
                        dlzbmxSrv.LoadZrszbModelInDlzbmx(khid, WebKeys.KaoheType.机关部门, xszb);
                        break;
                    default:
                        break;
                }
            }
            //进 打分指标考核表
            else if (!string.IsNullOrEmpty(xszb.JxzrsZb.DFZBBM))
            {
                switch (xszb.Kaohe.LXID)
                {
                    case "LX2014A"://二级单位及领导班子考核
                        if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("加减分"))
                        {
                            //加减分只领导班子考核
                            dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.领导班子, xszb);
                        }
                        else
                        {
                            dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.二级单位, xszb);
                            //约束性指标领导班子也考核
                            if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("约束"))
                            {
                                dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.领导班子, xszb);
                            }
                        }
                        break;
                    case "LX2014B"://机关部门及负责人考核
                        if (xszb.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("连带"))
                        {
                            //只考核负责人
                            dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.部门负责人, xszb);
                        }
                        else
                        {
                            dfzbmxSrv.LoadZrszbModelInDfzbmx(khid, WebKeys.KaoheType.机关部门, xszb);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 考核完成
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public void FinishKaohe(string khid)
        {
            /*
             1、更新定量指标的审定标志和时间
             2、更新锁定表为锁定状态
             3、更新待办箱中所有与当前考核相关的未操作信息状态为已完成
             4、更新考核状态为完成
             */
            StringBuilder sqls = new StringBuilder();
            string[] cacheKeys = new string[] { 
                Enterprise.Data.Perfo.Kh.KhDlzbmxData.CacheClassKey,
                Enterprise.Data.Perfo.Kh.KhUnlockData.CacheClassKey,
                Enterprise.Data.Perfo.Kh.KhMessageData.CacheClassKey
            };
            sqls.Append("update [PERFO_KH_DLZBMX] set WCZSDRQ=CURRENT_TIMESTAMP where KHID='" + khid + "' and WCZSDRQ is null;");
            sqls.Append("update [PERFO_KH_UNLOCK] set SDBZ='1',TJSJ=CURRENT_TIMESTAMP where KHID='" + khid + "' and SDBZ='0';");
            sqls.Append("update [PERFO_KH_MESSAGE] set DQZT='1',WCRQ=CURRENT_TIMESTAMP where KHID='" + khid + "' and DQZT='0';");
            sqls.Append("update [PERFO_KH_KHGL] set KHZT='1',GBSJ=CURRENT_TIMESTAMP where KHID='" + khid + "' and KHZT='0';");
            if (dal.ExecuteSQL(sqls.ToString()))
            {
                //清缓存
                CacheHelper.RemoveCacheForClassKeys(cacheKeys);
            }
        }

        #endregion

        #region 提速相应方法

        /// <summary>
        /// 刷新缓存中的数据，用于首页面的提速
        /// </summary>
        public void RefreshCacheDataForIndex()
        {
            KhKindhbService hbSrv = new KhKindhbService();
            hbSrv.GetList();
            KhArticlesService tzSrv = new KhArticlesService();
            tzSrv.GetList();

            GetEjdwKaoheInfo();
            GetJgbmKaoheInfo();
        }

        #endregion

    }

}
