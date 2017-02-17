using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// 文件名:  KhEjdwkhdfService.cs
    /// 功能描述: 业务逻辑层-二级单位考核得分表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/12/2 13:41:05
    /// </summary>
    public class KhEjdwkhdfService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhEjdwkhdfData dal = new KhEjdwkhdfData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhEjdwkhdfModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhEjdwkhdfModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhEjdwkhdfModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhEjdwkhdfModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhEjdwkhdfModel model)
        {
            return dal.Execute(model);
        }

        /// <summary>
        /// 执行原生SQL操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ExecuteSQL(string sql)
        {
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// 批量执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ExecuteList(List<KhEjdwkhdfModel> models)
        {
            bool isOk = true;
            foreach (var model in models)
            {
                isOk = dal.Execute(model);
            }
            return isOk;
        }

        #endregion

        #region 自定义方法区

        /// <summary>
        /// 获取指定考核期内的所有二级单位与领导班子得分数据
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhEjdwkhdfModel> GetListByKhid(string khid)
        {
            string hql = "from KhEjdwkhdfModel p where p.KHID='" + khid + "' order by p.XMXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取指定考核期内的所有二级单位与领导班子得分数据，确保单位的唯一性
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhEjdwkhdfModel> GetEjdwdfListByKhid(string khid)
        {
            var list = GetListByKhid(khid).Where(p => !string.IsNullOrEmpty(p.HZBZ)).DistinctBy(p => p.JGBM).ToList();
            return list;
        }

        /// <summary>
        /// 删除指定考核期下的得分数据
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <returns></returns>
        public bool DeleteEjdwkhdfData(string khid)
        {
            string sql = "delete from [PERFO_KH_EJDWKHDF] where KHID='" + khid + "'";
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// 完成指定考核期下的数据初始化
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <returns></returns>
        public bool InitEjdwkhdfData(string khid)
        {
            List<KhEjdwkhdfModel> dataLst = new List<KhEjdwkhdfModel>();
            /*
             思路：
             1、提取各单位的定量指标数据，将指标的一级名称分类汇总成数据集，添加到得分表
             2、提取各单位的打分指标数据，按指标的一级名称分类汇总成数据集，添加到得分表
             3、提取数据上报表中的数据，按单位信息分别提取对应的数据
             4、提取合并计算的单位并添加合并计算记录
             */
            KhKhdfpxfwService pxfwSrv = new KhKhdfpxfwService();//排序范围
            KhNdxsService ndxsSrv = new KhNdxsService();//经营难度系数
            //定量指标
            KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
            //打分指标
            KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();
            //合并计分规则
            KhHbjfgzService hbjfgzSrv = new KhHbjfgzService();
            var hbjfgzLst = hbjfgzSrv.GetListByKhid(khid) as List<KhHbjfgzModel>;

            //数据上报单位
            KhSjsbService sjsbSrv = new KhSjsbService();
            var bmjgs = sjsbSrv.GetListByKhid(khid).OrderBy(p => p.Bmjg.XSXH).ToList();
            foreach (var bmjg in bmjgs)
            {
                int index = 1;
                //提取该单位的合并计分规则
                var hbjfModel = hbjfgzLst.FirstOrDefault(p => p.HBJFDW.Contains("," + bmjg.JGBM + ","));
                //1==提取指定单位的定量指标数据
                var dlzbLst = dlzbmxSrv.GetListByKhidAndJgbm(khid, bmjg.JGBM.ToString());
                //按指标的一级分类名称形成数据集
                var dlzbByYjzbmcLst = dlzbLst.DistinctBy(p => p.ZbsxModel.JxzrsZb.Lhzb.Zbxx.YJZBMC).
                    OrderBy(p => p.ZbsxModel.SXXH).ToList();
                foreach (var dlzb in dlzbByYjzbmcLst)
                {
                    KhEjdwkhdfModel khdfModel = new KhEjdwkhdfModel();
                    khdfModel.DB_Option_Action = WebKeys.InsertAction;
                    khdfModel.DFID = CommonTool.GetGuidKey();
                    khdfModel.JGBM = bmjg.JGBM;
                    khdfModel.KHID = bmjg.KHID;
                    khdfModel.KHXMC = dlzb.ZbsxModel.JxzrsZb.Lhzb.Zbxx.YJZBMC;
                    khdfModel.KHLX = dlzb.KHDX;
                    khdfModel.XMXH = "01" + CommonTool.BuZero_3(index++);
                    khdfModel.JGMC = bmjg.Bmjg.JGMC;
                    khdfModel.GSDWMC = pxfwSrv.GetGsdwmc(khdfModel.KHID.ToString(), bmjg.JGBM, khdfModel.KHLX);
                    khdfModel.HBJFID = (hbjfModel != null) ? hbjfModel.HBJFID : null;
                    var yjzbmcList = dlzbLst.Where(p => p.ZbsxModel.JxzrsZb.Lhzb.Zbxx.YJZBMC == khdfModel.KHXMC
                        && p.ZbsxModel.JxzrsZb.ZZBXZ == "主指标").ToList();
                    khdfModel.KHBZF = yjzbmcList.Sum(p => p.ZbsxModel.SXQZ) * 100;
                    khdfModel.KHDF = yjzbmcList.Sum(p => p.SJDF);
                    if (!dataLst.Exists(p => p.KHID == khdfModel.KHID && p.KHLX == khdfModel.KHLX && 
                        p.JGBM == khdfModel.JGBM && p.KHXMC == khdfModel.KHXMC))
                    {
                        dataLst.Add(khdfModel);
                    }
                }
                //2==提取指定单位的打分指标数据
                var ejdwDfzbLst = dfzbmxSrv.GetListByKhidAndKhdx(khid, WebKeys.KaoheType.二级单位).Where(p=>p.JGBM==bmjg.JGBM).ToList();
                var dfzbByYjzbmcLst = ejdwDfzbLst.DistinctBy(p => p.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC).
                    OrderBy(p => p.ZbsxModel.SXXH).ToList();
                foreach (var dfzb in dfzbByYjzbmcLst)
                {
                    KhEjdwkhdfModel khdfModel = new KhEjdwkhdfModel();
                    khdfModel.DB_Option_Action = WebKeys.InsertAction;
                    khdfModel.DFID = CommonTool.GetGuidKey();
                    khdfModel.JGBM = bmjg.JGBM;
                    khdfModel.KHID = bmjg.KHID;
                    khdfModel.KHXMC = dfzb.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC;
                    khdfModel.KHLX = dfzb.KHDX;
                    khdfModel.XMXH = "02" + CommonTool.BuZero_3(index++);
                    khdfModel.JGMC = bmjg.Bmjg.JGMC;
                    khdfModel.GSDWMC = pxfwSrv.GetGsdwmc(khdfModel.KHID.ToString(), bmjg.JGBM, khdfModel.KHLX);
                    khdfModel.HBJFID = (hbjfModel != null) ? hbjfModel.HBJFID : null;
                    khdfModel.BZSM = dfzb.DFBZ;
                    var yjzbmcList = ejdwDfzbLst.Where(p => p.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC == khdfModel.KHXMC).ToList();
                    khdfModel.KHDF = yjzbmcList.Sum(p => p.SJDF);
                    if (!dataLst.Exists(p => p.KHID == khdfModel.KHID && p.KHLX == khdfModel.KHLX &&
                        p.JGBM == khdfModel.JGBM && p.KHXMC == khdfModel.KHXMC))
                    {
                        dataLst.Add(khdfModel);
                    }
                }

                //3==提取所有单位领导班子的打分指标
                var ldbzDfzbLst = dfzbmxSrv.GetListByKhidAndKhdx(khid, WebKeys.KaoheType.领导班子).Where(p => p.JGBM == bmjg.JGBM).ToList();
                var ldzbDfzbByYjzbmcLst = ldbzDfzbLst.DistinctBy(p => p.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC).
                    OrderBy(p => p.ZbsxModel.SXXH).ToList();
                foreach (var ldbzDF in ldzbDfzbByYjzbmcLst)
                {
                    KhEjdwkhdfModel khdfModel = new KhEjdwkhdfModel();
                    khdfModel.DB_Option_Action = WebKeys.InsertAction;
                    khdfModel.DFID = CommonTool.GetGuidKey();
                    khdfModel.JGBM = bmjg.JGBM;
                    khdfModel.KHID = bmjg.KHID;
                    khdfModel.KHXMC = ldbzDF.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC;
                    khdfModel.KHLX = ldbzDF.KHDX;
                    khdfModel.XMXH = "03" + CommonTool.BuZero_3(index++);
                    khdfModel.BZSM = ldbzDF.YSSM;//约束说明
                    khdfModel.JGMC = bmjg.Bmjg.JGMC;
                    khdfModel.NDXS = ndxsSrv.GetNdxsByKhidAndJgbm(bmjg.KHID.ToString(), bmjg.JGBM);//经营难度系数
                    khdfModel.GSDWMC = pxfwSrv.GetGsdwmc(khdfModel.KHID.ToString(), bmjg.JGBM, khdfModel.KHLX);
                    khdfModel.HBJFID = (hbjfModel != null) ? hbjfModel.HBJFID : null;
                    var yjzbmcList = ldbzDfzbLst.Where(p => p.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC == khdfModel.KHXMC).ToList();
                    khdfModel.KHDF = yjzbmcList.Sum(p => p.SJDF);
                    if (!dataLst.Exists(p => p.KHID == khdfModel.KHID && p.KHLX == khdfModel.KHLX &&
                        p.JGBM == khdfModel.JGBM && p.KHXMC == khdfModel.KHXMC))
                    {
                        dataLst.Add(khdfModel);
                    }
                }

                //4==分析该单位是否为合并计分单位，如果是并处理
                if (hbjfModel != null)
                {
                    KhEjdwkhdfModel khdfModel = new KhEjdwkhdfModel();
                    khdfModel.DB_Option_Action = WebKeys.InsertAction;
                    khdfModel.DFID = CommonTool.GetGuidKey();
                    khdfModel.KHID = bmjg.KHID;
                    khdfModel.KHLX = ((int)WebKeys.KaoheType.二级单位).ToString();
                    khdfModel.XMXH = "00" + CommonTool.BuZero_3(index++);
                    khdfModel.HBJFID = khdfModel.KHXMC = khdfModel.JGMC = hbjfModel.HBJFID;
                    khdfModel.GSDWMC = pxfwSrv.GetGsdwmc(khdfModel.KHID.ToString(), bmjg.JGBM, khdfModel.KHLX);
                    khdfModel.ISHBJF = "1";
                    if (!dataLst.Exists(p => p.KHID == hbjfModel.KHID && p.KHXMC == khdfModel.KHXMC))
                    {
                        dataLst.Add(khdfModel);
                    }
                }
            }
            return ExecuteList(dataLst);
        }


        /// <summary>
        /// 计算指定考核期下的领导班子考核得分
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <returns></returns>
        public bool CalculateEjdwldbzKhdfData(string khid)
        {
            /*
             思路：
             * 1、首先要计算出二级单位的得分与排名
             * 2、从二级单位考核结果表中提取考核对象为二级单位和领导班子的数据
             * 3、提取合并计分规则
             * 4、重新计算合并计分的单位的得分数据
             * 5、依据当前考核的单位排序要求，对单位排名进行进行更新
             */
            bool isOk = true;
            KhKhdfpxfwService pxfwSrv = new KhKhdfpxfwService();//排序范围
            KhHbjfgzService hbjfgzSrv = new KhHbjfgzService();//合并计分规则
            KhCjqjService cjqjSrv = new KhCjqjService();//成绩区间
            StringBuilder sqls = new StringBuilder();
            string updSql = string.Empty;
            var allDataList = GetListByKhid(khid);//全部数据=二级单位+领导班子
            var dataList = allDataList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString()).ToList();
            //1==计算独立计分单位的 得分=(效益类+管理类)*经营难度系数+约束扣分+加减分------------------------------
            var oneEjdwLst = dataList.Where(p => p.JGBM != null && p.HBJFID == null).DistinctBy(p => p.JGBM).ToList();
            foreach (var ejdw in oneEjdwLst)
            {
                decimal dwFzrZdf = 0M;//总得分
                decimal jia_jian_df = 0M;//约束与加减分得分
                decimal xyl_gll_df = 0M;//量化指标得分
                decimal ndxs = (ejdw.NDXS.ToDecimal() > 0) ? ejdw.NDXS.Value : 1.0M;
                //效益类---二级单位
                var xylzb = allDataList.Where(p =>p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() 
                    && p.JGBM == ejdw.JGBM && p.KHXMC == "效益类").FirstOrDefault();
                if (xylzb != null)
                {
                    xyl_gll_df += xylzb.KHDF.ToDecimal();
                }
                //管理类---二级单位
                var gllzb = allDataList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString()
                    && p.JGBM == ejdw.JGBM && p.KHXMC == "管理类").FirstOrDefault();
                if (gllzb != null)
                {
                    xyl_gll_df += gllzb.KHDF.ToDecimal();
                }
                //约束扣分
                var ysxzb = dataList.Where(p => p.JGBM == ejdw.JGBM && p.KHXMC == "约束性指标").FirstOrDefault();
                if (ysxzb != null)
                {
                    jia_jian_df += ysxzb.KHDF.ToDecimal();
                }
                //加减分因素
                var jfzb = dataList.Where(p => p.JGBM == ejdw.JGBM && p.KHXMC == "加减分因素").FirstOrDefault();
                if (jfzb != null)
                {
                    jia_jian_df += jfzb.KHDF.ToDecimal();
                }
                dwFzrZdf = xyl_gll_df * ndxs + jia_jian_df;//总得分
                updSql = 
                    string.Format("update [PERFO_KH_EJDWKHDF] set FZRZDF='{0}' where KHID='{1}' and JGBM='{2}';",
                    dwFzrZdf, ejdw.KHID, ejdw.JGBM);
                sqls.Append(updSql);
            }
            if (!string.IsNullOrEmpty(sqls.ToString()))
                isOk = ExecuteSQL(sqls.ToString());
            if (isOk)
            {
                //2==计算合并计分的单位的得分--------------------------------------------------------------------
                List<string> hbjfmcLst = new List<string>();
                var hbjfEjdwLst = dataList.Where(p => p.JGBM != null && p.HBJFID != null).DistinctBy(p => p.JGBM).ToList();
                sqls = new StringBuilder();
                foreach (var ejdw in hbjfEjdwLst)
                {
                    //合并计算得分
                    if (ejdw.Hbjf != null && ejdw.Hbjf.Jsgz != null && !hbjfmcLst.Exists(p => p == ejdw.Hbjf.HBJFID))
                    {
                        decimal hjdwZdf = 0M;//总得分
                        decimal hjdwDlzb = 0M;//定量指标合计
                        decimal hjdwDfzbJiaFen = 0M;//打分指标合计 加分
                        decimal hjdwDfzbJianFen = 0M;//打分指标合计 减分
                        decimal ndxs = (ejdw.NDXS.ToDecimal() > 0) ? ejdw.NDXS.Value : 1.0M;
                        string[] jgbms = ejdw.Hbjf.HBJFDW.TrimStart(',').TrimEnd(',').Split(',');
                        foreach (var jgbm in jgbms)
                        {
                            string bds = ejdw.Hbjf.Jsgz.GZBDS.Replace("①", jgbm);
                            decimal qz = Utility.Eval(bds).ToDecimal();
                            hjdwDlzb += allDataList.
                                Where(p => p.JGBM == jgbm.ToInt() && (p.KHXMC == "效益类" || p.KHXMC == "管理类")).Sum(p => p.KHDF).ToDecimal() * qz;//与二级单位一样
                            hjdwDfzbJiaFen += dataList.
                                Where(p => p.JGBM == jgbm.ToInt() && (p.KHXMC == "加减分因素")).Sum(p => p.KHDF).ToDecimal();
                            hjdwDfzbJianFen += dataList.
                                Where(p => p.JGBM == jgbm.ToInt() && (p.KHXMC == "约束性指标")).Sum(p => p.KHDF).ToDecimal();
                        }

                        hjdwZdf = hjdwDlzb * ndxs + hjdwDfzbJiaFen + hjdwDfzbJianFen;//总得分
                        hbjfmcLst.Add(ejdw.Hbjf.HBJFID);//由于只一次就能把相关的得分都算出来，所以把剩余的相关单位过滤掉

                        //更新这几个单位的总得分 规则标志
                        //（1）==单位，只更新班子总得分
                        updSql = string.Format(
                            "update [PERFO_KH_EJDWKHDF] set FZRZDF='{0}' where KHID='{1}' and JGBM in ({2});",
                            hjdwZdf, ejdw.KHID, ejdw.Hbjf.HBJFDW.TrimStart(',').TrimEnd(','));
                        sqls.Append(updSql);
                        //（2）==合并计分项, 更新 其班子总得分 综合得分 加分+减分
                        updSql = string.Format(
                            "update [PERFO_KH_EJDWKHDF] set FZRZDF='{0}',FZRPJF='{1}',KHBZF='{2}',NDXS='{5}' where KHID='{3}' and KHXMC='{4}';",
                            hjdwZdf, hjdwDlzb, (hjdwDfzbJiaFen + hjdwDfzbJianFen), ejdw.KHID, ejdw.Hbjf.HBJFID, ejdw.NDXS);
                        sqls.Append(updSql);
                    }
                }
                if (!string.IsNullOrEmpty(sqls.ToString()))
                    isOk = ExecuteSQL(sqls.ToString());
                //3==计算各单位排名,并根据归属机构来排序----------------------------------------------------------------------
                if (isOk)
                {
                    //重新获取一次最新记录集合
                    var dataList3 = GetListByKhid(khid).Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString()).ToList();
                    var oneEjdwLst3 = dataList3.Where(p => p.JGBM != null).DistinctBy(p => p.JGBM).ToList();
                    var gsdwList = oneEjdwLst3.DistinctBy(p => p.GSDWMC).ToList();//归属上级单位
                    List<string> hbjfmcList = new List<string>();
                    foreach (var gsdw in gsdwList)
                    {
                        sqls = new StringBuilder();
                        var oneEjdwLstByGsdw = oneEjdwLst3.Where(p => p.GSDWMC == gsdw.GSDWMC).
                            OrderByDescending(p => p.FZRZDF).ToList();
                        int dwpm = 1;//排名
                        for (int i = 0; i < oneEjdwLstByGsdw.Count; i++)
                        {
                            var model = oneEjdwLstByGsdw[i];
                            if (model.Hbjf != null)
                            {
                                if (!hbjfmcList.Exists(p => p == model.HBJFID))
                                {
                                    //合并记分
                                    sqls.Append("update [PERFO_KH_EJDWKHDF] set FZRPM='" + (dwpm++)
                                        + "' where KHID='" + model.KHID + "' and HBJFID='" + model.HBJFID + "';");
                                    hbjfmcList.Add(model.HBJFID);
                                }
                            }
                            else
                            {
                                //独立记分
                                sqls.Append("update [PERFO_KH_EJDWKHDF] set FZRPM='" + (dwpm++)
                                    + "' where KHID='" + model.KHID + "' and JGBM='" + model.JGBM + "';");
                            }
                        }
                        Debuger.GetInstance().log(khid + "=====3计算各单位排名,并根据归属机构来排序====【" + sqls.ToString() + "】");
                        if (!string.IsNullOrEmpty(sqls.ToString()))
                            isOk = ExecuteSQL(sqls.ToString());
                        //4==领导班子兑现倍数计算==================================================
                        if (isOk)
                        {
                            //从参数配置表中提取各区间的分布数据，根据各班子的得分类型计算兑现倍数
                            //兑现倍数=SUM(区间min+(区间max-区间min)*(当前得分-同类型最低分)/(同类型max-同类型min))
                            List<KhCjqjModel> cjqjLst = cjqjSrv.GetListByKhid(khid) as List<KhCjqjModel>;
                            //重新获取一次最新记录集合
                            var dataList4 = GetListByKhid(khid).Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString() 
                                && !string.IsNullOrEmpty(p.FZRDFLB)).ToList();
                            var oneEjdwLst4 = dataList4.Where(p => p.JGBM != null).DistinctBy(p => p.JGBM).ToList();
                            sqls = new StringBuilder();
                            foreach (var ejdw in oneEjdwLst4)
                            {
                                var gradeLst = oneEjdwLst4.Where(p => p.FZRDFLB == ejdw.FZRDFLB).ToList();
                                var cjqj = cjqjLst.FirstOrDefault(p => p.QJDJ == ejdw.FZRDFLB);
                                if (cjqj != null)
                                {
                                    decimal qjMin = cjqj.LOWERV.ToDecimal();//区间min
                                    decimal qjMax = cjqj.UPPERV.ToDecimal();//区间max
                                    decimal dfMax = gradeLst.Max(p => p.FZRZDF).ToDecimal();//同类型max
                                    decimal dfMin = gradeLst.Min(p => p.FZRZDF).ToDecimal();//同类型min
                                    decimal dxsx = 0M;
                                    try
                                    {
                                        dxsx = qjMin + (qjMax - qjMin) * ((ejdw.FZRZDF.Value - dfMin) / (dfMax - dfMin));
                                    }
                                    catch { }
                                    if (!string.IsNullOrEmpty(ejdw.HBJFID))
                                    {
                                        //合并计分，同步更新
                                        sqls.Append("update [PERFO_KH_EJDWKHDF] set FZRDXBS='" + dxsx
                                        + "' where HBJFID='" + ejdw.HBJFID + "';");//HBJFID按考核期，具有唯一性
                                    }
                                    else
                                    {
                                        //单独更新
                                        sqls.Append("update [PERFO_KH_EJDWKHDF] set FZRDXBS='" + dxsx
                                        + "' where KHID='" + ejdw.KHID + "' and JGBM='" + ejdw.JGBM + "';");
                                    }
                                }
                            }
                            Debuger.GetInstance().log(khid + "=====4领导班子兑现倍数计算====【" + sqls.ToString() + "】");
                            if (!string.IsNullOrEmpty(sqls.ToString()))
                                isOk = ExecuteSQL(sqls.ToString());
                        }
                    }
                    
                }
            }
            return isOk;
        }


        /// <summary>
        /// 计算指定考核期下的二级单位考核得分
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <returns></returns>
        public bool CalculateEjdwkhdfData(string khid)
        {
            /*
             思路：
             * 1、从二级单位考核结果表中分别提取考核对象为二级单位和领导班子的数据
             * 2、提取合并计分规则
             * 3、重新计算合并计分的单位的得分数据
             * 4、依据当前考核的单位排序要求，对单位排名进行进行更新
             */
            bool isOk = true;
            KhKhdfpxfwService pxfwSrv = new KhKhdfpxfwService();//排序范围
            KhHbjfgzService hbjfgzSrv = new KhHbjfgzService();//合并计分规则
            StringBuilder sqls = new StringBuilder();
            string updSql = string.Empty;
            var dataList = GetListByKhid(khid).Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString()).ToList();
            //1==计算独立计分单位的 得分=效益类+管理类+约束+加分----------------------------------
            var oneEjdwLst = dataList.Where(p => p.JGBM != null && p.HBJFID == null).DistinctBy(p => p.JGBM).ToList();
            foreach (var ejdw in oneEjdwLst)
            {
                decimal dwzdf = 0M;
                decimal xyl_gll_df = 0M;
                //效益类
                var xylzb = dataList.Where(p => p.JGBM == ejdw.JGBM && p.KHXMC == "效益类").FirstOrDefault();
                if (xylzb != null)
                {
                    dwzdf += xylzb.KHDF.ToDecimal();
                    xyl_gll_df += xylzb.KHDF.ToDecimal();
                }
                //管理类
                var gllzb = dataList.Where(p => p.JGBM == ejdw.JGBM && p.KHXMC == "管理类").FirstOrDefault();
                if (gllzb != null)
                {
                    dwzdf += gllzb.KHDF.ToDecimal();
                    xyl_gll_df += gllzb.KHDF.ToDecimal();
                }
                //约束扣分
                var ysxzb = dataList.Where(p => p.JGBM == ejdw.JGBM && p.KHXMC == "约束性指标").FirstOrDefault();
                if (ysxzb != null)
                {
                    dwzdf += ysxzb.KHDF.ToDecimal();
                }
                //加分情况
                var jfzb = dataList.Where(p => p.JGBM == ejdw.JGBM && p.KHXMC == "加分指标").FirstOrDefault();
                if (jfzb != null)
                {
                    dwzdf += jfzb.KHDF.ToDecimal();
                }
                updSql =
                    string.Format("update [PERFO_KH_EJDWKHDF] set DWZDF='{0}' where KHID='{1}' and JGBM='{2}';",
                    dwzdf, ejdw.KHID, ejdw.JGBM);
                sqls.Append(updSql);
            }
            if (!string.IsNullOrEmpty(sqls.ToString()))
                isOk = ExecuteSQL(sqls.ToString());
            if (isOk)
            {
                //2==计算合并计分的单位的得分--------------------------------------------------------------------
                List<string> hbjfmcLst = new List<string>();
                var hbjfEjdwLst = dataList.Where(p => p.JGBM != null && p.HBJFID != null).DistinctBy(p => p.JGBM).ToList();
                sqls = new StringBuilder();
                foreach (var ejdw in hbjfEjdwLst)
                {
                    //合并计算得分
                    if (ejdw.Hbjf != null && ejdw.Hbjf.Jsgz != null && !hbjfmcLst.Exists(p => p == ejdw.Hbjf.HBJFID))
                    {
                        decimal hjdwZdf = 0M;//总得分
                        decimal hjdwDlzb = 0M;//定量指标合计
                        decimal hjdwDfzbJiaFen = 0M;//打分指标合计 加分
                        decimal hjdwDfzbJianFen = 0M;//打分指标合计 减分
                        string[] jgbms = ejdw.Hbjf.HBJFDW.TrimStart(',').TrimEnd(',').Split(',');
                        foreach (var jgbm in jgbms)
                        {
                            string bds = ejdw.Hbjf.Jsgz.GZBDS.Replace("①", jgbm);
                            decimal qz = Utility.Eval(bds).ToDecimal();
                            hjdwDlzb += dataList.
                                Where(p => p.JGBM == jgbm.ToInt() && (p.KHXMC == "效益类" || p.KHXMC == "管理类")).Sum(p => p.KHDF).ToDecimal() * qz;
                            hjdwDfzbJiaFen += dataList.
                                Where(p => p.JGBM == jgbm.ToInt() && (p.KHXMC == "加分指标")).Sum(p => p.KHDF).ToDecimal();
                            hjdwDfzbJianFen += dataList.
                                Where(p => p.JGBM == jgbm.ToInt() && (p.KHXMC == "约束性指标")).Sum(p => p.KHDF).ToDecimal();
                        }

                        hjdwZdf = hjdwDlzb + hjdwDfzbJiaFen + hjdwDfzbJianFen;//总得分
                        hbjfmcLst.Add(ejdw.Hbjf.HBJFID);//由于只一次就能把相关的得分都算出来，所以把剩余的相关单位过滤掉

                        //更新这几个单位的总得分 规则标志
                        //（1）==单位，只更新部门总得分
                        updSql = string.Format(
                            "update [PERFO_KH_EJDWKHDF] set DWZDF='{0}' where KHID='{1}' and JGBM in ({2});",
                            hjdwZdf, ejdw.KHID, ejdw.Hbjf.HBJFDW.TrimStart(',').TrimEnd(','));
                        sqls.Append(updSql);
                        //（2）==合并计分项, 更新 其部门总得分 考核得分 加分 减分
                        updSql = string.Format(
                            "update [PERFO_KH_EJDWKHDF] set DWZDF='{0}',KHDF='{1}',DWPJF='{2}',DWDXBS='{3}' where KHID='{4}' and KHXMC='{5}';",
                            hjdwZdf, hjdwDlzb, hjdwDfzbJiaFen, hjdwDfzbJianFen, ejdw.KHID, ejdw.Hbjf.HBJFID);
                        sqls.Append(updSql);
                    }
                }
                if (!string.IsNullOrEmpty(sqls.ToString()))
                    isOk = ExecuteSQL(sqls.ToString());

                //3==计算各单位排名,并根据归属机构来排序----------------------------------------------------------------------
                if (isOk)
                {
                    //重新获取一次最新记录集合
                    var dataList3 = GetListByKhid(khid).Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString()).ToList();
                    var oneEjdwLst3 = dataList3.Where(p => p.JGBM != null).DistinctBy(p => p.JGBM).ToList();
                    var gsdwList = oneEjdwLst3.DistinctBy(p => p.GSDWMC).ToList();//归属上级单位
                    List<string> hbjfmcList = new List<string>();
                    foreach (var gsdw in gsdwList)
                    {
                        sqls = new StringBuilder();
                        var oneEjdwLstByGsdw = oneEjdwLst3.Where(p => p.GSDWMC == gsdw.GSDWMC).
                            OrderByDescending(p => p.DWZDF).ToList();
                        int dwpm = 1;//排名
                        for (int i = 0; i < oneEjdwLstByGsdw.Count; i++)
                        {
                            var model = oneEjdwLstByGsdw[i];
                            if (model.Hbjf != null)
                            {
                                if (!hbjfmcList.Exists(p => p == model.HBJFID))
                                {
                                    //合并记分
                                    sqls.Append("update [PERFO_KH_EJDWKHDF] set DWPM='" + (dwpm++)
                                        + "' where KHID='" + model.KHID + "' and HBJFID='" + model.HBJFID + "';");
                                    hbjfmcList.Add(model.HBJFID);
                                }
                            }
                            else
                            {
                                //独立记分
                                sqls.Append("update [PERFO_KH_EJDWKHDF] set DWPM='" + (dwpm++)
                                    + "' where KHID='" + model.KHID + "' and JGBM='" + model.JGBM + "';");
                            }
                        }
                        if (!string.IsNullOrEmpty(sqls.ToString()))
                            isOk = ExecuteSQL(sqls.ToString());
                    }

                }
            }
            return isOk;
        }

        #endregion

    }

}
