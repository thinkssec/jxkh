using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// 文件名:  KhJgbmkhdfService.cs
    /// 功能描述: 业务逻辑层-机关部门考核得分表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/28 16:45:02
    /// </summary>
    public class KhJgbmkhdfService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhJgbmkhdfData dal = new KhJgbmkhdfData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJgbmkhdfModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhJgbmkhdfModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhJgbmkhdfModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhJgbmkhdfModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 批量执行SQL脚本
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public bool ExcuteSQLByList(List<string> sqls)
        {
            bool isOk = true;
            foreach (var sql in sqls)
            {
               isOk = dal.ExecuteSQL(sql);
            }
            return isOk;
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhJgbmkhdfModel model)
        {
            return dal.Execute(model);
        }


        /// <summary>
        /// 执行原生SQL
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
        /// <param name="models"></param>
        /// <returns></returns>
        public bool ExecuteByList(List<KhJgbmkhdfModel> models)
        {
            bool isOk = true;
            foreach (var model in models)
            {
               isOk = dal.Execute(model);
            }
            return isOk;
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 删除指定考核期下的得分数据
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <returns></returns>
        public bool DeleteJgbmkhdfData(string khid)
        {
            string sql = "delete from [PERFO_KH_JGBMKHDF] where KHID='" + khid + "'";
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// 获取指定考核期下的数据集合
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <returns></returns>
        public IList<KhJgbmkhdfModel> GetListByKhid(string khid)
        {
            string hql = "from KhJgbmkhdfModel p where p.KHID='" + khid + "' order by p.XMXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取指定考核期内的所有机关部门及负责人得分数据，确保单位的唯一性
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhJgbmkhdfModel> GetJgbmdfListByKhid(string khid)
        {
            var list = GetListByKhid(khid).Where(p => !string.IsNullOrEmpty(p.HZBZ)).DistinctBy(p => p.JGBM).ToList();
            return list;
        }

        /// <summary>
        /// 提取各单位的考核指标类型并导入得分表
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <returns></returns>
        public bool InitJgbmkhdfData(string khid)
        {
            List<KhJgbmkhdfModel> list = new List<KhJgbmkhdfModel>();
            KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();//定量指标服务类 
            KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//打分指标服务类
            KhSjsbService sjsbSrv = new KhSjsbService();//数据上报==考核的机构列表
            KhZbsxService zbsxSrv = new KhZbsxService();//指标筛选

            var bmjgLst = sjsbSrv.GetListByKhid(khid).OrderBy(p => p.Bmjg.XSXH).ToList();
            foreach (var bmjg in bmjgLst)
            {
                int index = 1;
                //1==从定量指标明细中取得所有指标
                var dlzbLst = dlzbmxSrv.GetListByKhidAndJgbm(khid,bmjg.JGBM.ToString()).
                    DistinctBy(p=>p.ZBBM).OrderBy(p=>p.ZbsxModel.SXXH).ToList();
                foreach(var dlzb in dlzbLst) 
                {
                    KhJgbmkhdfModel model = new KhJgbmkhdfModel();
                    model.DB_Option_Action = WebKeys.InsertAction;
                    model.DFID = CommonTool.GetGuidKey();
                    model.JGBM = bmjg.JGBM;
                    model.KHID = bmjg.KHID;
                    //量化指标，取指标名称
                    model.KHXMC = dlzb.ZbsxModel.JxzrsZb.Lhzb.Zbxx.ZBMC;
                    model.KHLX = ((int)WebKeys.KaoheType.机关部门).ToString();
                    model.XMXH = "01"+ CommonTool.BuZero_3(index++);
                    model.KHDWSL = dlzb.KhJgbmdfbLst.Count;
                    if (!list.Exists(p => p.JGBM == bmjg.JGBM && p.KHXMC == model.KHXMC))
                    {
                        list.Add(model);
                    }
                }

                //2==从打分指标明细中取得所有指标
                var dfzbLst = dfzbmxSrv.GetListByKhidAndJgbm(khid, bmjg.JGBM.ToString()).
                    DistinctBy(p => p.ZBBM).OrderBy(p => p.ZbsxModel.SXXH).ToList();
                foreach (var dfzb in dfzbLst)
                {
                    KhJgbmkhdfModel model = new KhJgbmkhdfModel();
                    model.DB_Option_Action = WebKeys.InsertAction;
                    model.DFID = CommonTool.GetGuidKey();
                    model.JGBM = bmjg.JGBM;
                    model.KHID = bmjg.KHID;
                    //打分指标，取一级分类名称
                    model.KHXMC = dfzb.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC;
                    if (model.KHXMC.Contains("机关作风"))
                    {
                        model.KHLX = ((int)WebKeys.KaoheType.机关部门).ToString();
                        model.XMXH = "02" + CommonTool.BuZero_3(index++);
                        model.KHDWSL = dfzb.KhJgbmdfbLst.Count;
                    }
                    else if (model.KHXMC.Contains("费用控制"))
                    {
                        model.KHLX = ((int)WebKeys.KaoheType.机关部门).ToString();
                        model.XMXH = "03" + CommonTool.BuZero_3(index++);
                        model.KHDWSL = dfzb.KhJgbmdfbLst.Count;
                    }
                    else if (model.KHXMC.Contains("连带"))
                    {
                        model.KHLX = ((int)WebKeys.KaoheType.部门负责人).ToString();
                        model.XMXH = "04" + CommonTool.BuZero_3(index++);
                        model.KHDWSL = dfzb.KhJgbmdfbLst.Count;
                    }
                    if (!list.Exists(p => p.JGBM == bmjg.JGBM && p.KHXMC == model.KHXMC))
                    {
                        list.Add(model);
                    }
                }
            }
            return ExecuteByList(list);
        }

        /// <summary>
        /// 计算指定考核期下的各单位得分
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <returns></returns>
        public bool CalculateJgbmkhdfData(string khid)
        {
            KhJgzfbService jgzfbSrv = new KhJgzfbService();//机关作风建设
            KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();//定量指标明细
            KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//打分指标明细;
            KhSjsbService sjsbSrv = new KhSjsbService();//数据上报==考核的机构列表
            ZbkDfzbService dfzbSrv = new ZbkDfzbService();//打分指标表
            /*
             1、从定量指标明细表中生成各单位指标的得分（按规则计算）
             2、从机关作风建设汇总表中提取数据
             3、从打分指标明细表中提取部门负责人的总部连带指标数据
             4、从打分指标明细表中提取各部门费用控制情况的数据
             */
            StringBuilder sqlSB = new StringBuilder();//更新脚本
            var bmjgLst = sjsbSrv.GetListByKhid(khid).OrderBy(p => p.Bmjg.XSXH).ToList();
            //一、计算各单位的总得分
            foreach (var bmjg in bmjgLst)
            {
                //1==定量指标评分是采用一定的规则（上级0.6，同级二级+部门0.2，自评0.2），要分别统计
                var dlzbLst = dlzbmxSrv.GetListByKhidAndJgbm(khid, bmjg.JGBM.ToString());//单位下的数据
                foreach (var dlzb in dlzbLst)
                {
                    if (dlzb.KhJgbmdfbLst.Count == 0) 
                        continue;
                    var lddfLst = dlzb.KhJgbmdfbLst.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.上级领导).ToString()).ToList();
                    var zpdfLst = dlzb.KhJgbmdfbLst.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.同级部门).ToString() && 
                        p.DFZ == p.JGBM.ToString()).ToList();
                    var tjdfLst = dlzb.KhJgbmdfbLst.Where(p => p.DFZLX == ((int)WebKeys.DFUserType.同级二级单位).ToString() ||
                        (p.DFZLX == ((int)WebKeys.DFUserType.同级部门).ToString() && p.DFZ != p.JGBM.ToString())).ToList();
                    //按当前单位的xsxh与评分单位或用户所在的单位的xsxh进行对比，求出其权重值
                    string bds = dlzb.ZbsxModel.JxzrsZb.Lhzb.Jsgz.GZBDS;
                    decimal ldqz = 0M;
                    decimal tjqz = 0M;
                    decimal zpqz = 0M;
                    if (!string.IsNullOrEmpty(bds))
                    {
                        string ldBds = bds.Replace("①", "SJLD");
                        ldqz = Utility.Eval(ldBds).ToDecimal();//上级权重
                        string tjBds = bds.Replace("①", "TJBM");
                        tjqz = Utility.Eval(tjBds).ToDecimal();//同级权重 
                        string zpBds = bds.Replace("①", "ZP");
                        zpqz = Utility.Eval(zpBds).ToDecimal();//自评权重
                    }
                    //三个集合分别求平均后再按权重相加
                    decimal zdf = 0M;
                    if (lddfLst.Count > 0)
                        zdf += lddfLst.Average(p => p.KHDF).ToDecimal() * ldqz;
                    if (zpdfLst.Count > 0)
                        zdf += zpdfLst.Average(p => p.KHDF).ToDecimal() * zpqz;
                    if (tjdfLst.Count > 0)
                        zdf += tjdfLst.Average(p => p.KHDF).ToDecimal() * tjqz;
                    dlzb.BFSFZ = zdf;//按权重合组合后分值
                    ////由于两类指标都分别设有权重，则其最终得分，还应该再与其标准分值的百分比再乘其权重
                    //decimal bzf = (dlzb.ZbsxModel.SXFZ.ToDecimal() != 0M) ? dlzb.ZbsxModel.SXFZ.Value : 1M;
                    //zdf = zdf * dlzb.ZbsxModel.SXQZ.ToDecimal();
                    //11.30 目前已经将权重值做为标准分分配到各指标，暂时不用再乘以权重了
                    dlzb.SJDF = zdf;//最终折算后得分(方便统计时直接相加即为实际得分了)
                    dlzb.DB_Option_Action = WebKeys.UpdateAction;
                    dlzbmxSrv.Execute(dlzb);
                }
                //2==提取定量指标的得分（年度重点+部门履职）
                List<KhDlzbmxModel> dlzbmxList = dlzbmxSrv.
                    GetListByKhidAndJgbm(bmjg.KHID.ToString(), bmjg.JGBM.ToString()) as List<KhDlzbmxModel>;
                var oneDlzbLst = dlzbmxList.DistinctBy(p => p.ZBBM).ToList();
                foreach (var dlzb in oneDlzbLst)
                {
                    
                    decimal hjz = dlzbmxList.Where(p => p.ZBBM == dlzb.ZBBM).Sum(p => p.SJDF).ToDecimal();
                    sqlSB.Append("update [PERFO_KH_JGBMKHDF] set KHDF='" + hjz + "' where JGBM='" + dlzb.JGBM + "' and KHID='" 
                        + dlzb.KHID + "' and KHXMC='" + dlzb.ZbsxModel.JxzrsZb.Lhzb.Zbxx.ZBMC + "';");
                }
                //3==从机关作风建设汇总表中提取数据
                var jgzf = jgzfbSrv.GetListByKhidAndJgbm(bmjg.KHID.ToString(), bmjg.JGBM.ToString()).FirstOrDefault();
                if (jgzf != null)
                {
                    var dfzb = dfzbSrv.GetSingle(jgzf.ZBBM);
                    sqlSB.Append("update [PERFO_KH_JGBMKHDF] set KHDF='" + jgzf.SJDF + "',BZSM='" + jgzf.ZDF 
                        + "' where JGBM='" + jgzf.JGBM + "' and KHID='" + jgzf.KHID + "' and KHXMC='" + dfzb.Zbxx.YJZBMC + "';");
                }
                //4==从打分指标明细表中提取部门负责人的总部连带指标数据
                var zbldzbLst = dfzbmxSrv.GetListByKhidAndKhdx(bmjg.KHID.ToString(), WebKeys.KaoheType.部门负责人).
                    Where(p => p.JGBM == bmjg.JGBM).ToList();
                if (zbldzbLst.Count > 0)
                {
                    var zfModel = zbldzbLst.First();
                    decimal zdf = zbldzbLst.Sum(p => p.SJDF).ToDecimal();
                    sqlSB.Append("update [PERFO_KH_JGBMKHDF] set KHDF='" + zdf + "' where JGBM='"
                        + zfModel.JGBM + "' and KHID='" + zfModel.KHID + "' and KHXMC='"
                            + zfModel.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC + "';");
                }
                //5==从打分指标明细表中提取各部门费用控制情况的数据
                var fykzqk = dfzbmxSrv.GetListByKhidAndKhdx(bmjg.KHID.ToString(), WebKeys.KaoheType.机关部门)
                    .FirstOrDefault(p => p.JGBM == bmjg.JGBM && p.DfzbModel.Zbxx.YJZBMC.Contains("费用控制"));
                if (fykzqk != null)
                {
                    sqlSB.Append("update [PERFO_KH_JGBMKHDF] set KHDF='" + fykzqk.SJDF.ToDecimal() + "' where JGBM='"
                        + bmjg.JGBM + "' and KHID='" + bmjg.KHID + "' and KHXMC='"
                            + fykzqk.DfzbModel.Zbxx.YJZBMC + "';");
                }
            }
            bool isResult = dal.ExecuteSQL(sqlSB.ToString());
            //二、计算各单位的总得分和负责人的总得分
            if (isResult)
            {
                sqlSB = new StringBuilder();
                //提取的数据中包括各部门及负责人的四类指标的得分数据
                var bmjgkhdfLst = GetListByKhid(khid);
                foreach (var bmjg in bmjgLst)
                {
                    //部门得分=重点工作+部门履职+机关作风+费用控制
                    decimal bmdf = bmjgkhdfLst.Where(p => p.JGBM == bmjg.JGBM && 
                        p.KHLX == ((int)WebKeys.KaoheType.机关部门).ToString()).Sum(p => p.KHDF).ToDecimal();
                    //负责人=部门得分+连带指标
                    decimal fzrdf = bmjgkhdfLst.Where(p => p.JGBM == bmjg.JGBM &&
                        p.KHLX == ((int)WebKeys.KaoheType.部门负责人).ToString()).Sum(p => p.KHDF).ToDecimal();
                    fzrdf = bmdf + fzrdf;
                    //记录更新SQL
                    sqlSB.Append("update [PERFO_KH_JGBMKHDF] set BMZDF='" + bmdf + "',FZRZDF='" + fzrdf + "' where JGBM='"
                        + bmjg.JGBM + "' and KHID='" + bmjg.KHID + "'");
                }
                //批量更新得分数据
                isResult = dal.ExecuteSQL(sqlSB.ToString());
                if (isResult)
                {
                    //三、计算排名 平均分 兑现系数
                    sqlSB = new StringBuilder();
                    //1=部门
                    var oneBmdfLst = GetListByKhid(khid).DistinctBy(p => p.JGBM).OrderByDescending(p => p.BMZDF).ToList();
                    decimal bmpjf = oneBmdfLst.Average(p=>p.BMZDF).ToDecimal();
                    for (int i = 0; i < oneBmdfLst.Count; i++)
                    {
                        if (bmpjf == 0) continue;
                        oneBmdfLst[i].BMPM = (i + 1);
                        oneBmdfLst[i].BMPJF = bmpjf;
                        oneBmdfLst[i].BMDXBS = oneBmdfLst[i].BMZDF / bmpjf;
                        oneBmdfLst[i].DB_Option_Action = WebKeys.UpdateAction;
                        //保存部门得分
                        sqlSB.Append("update [PERFO_KH_JGBMKHDF] set BMPM='" + oneBmdfLst[i].BMPM
                            + "',BMPJF='" + oneBmdfLst[i].BMPJF + "',BMDXBS='" + oneBmdfLst[i].BMDXBS + "' where JGBM='"
                        + oneBmdfLst[i].JGBM + "' and KHID='" + oneBmdfLst[i].KHID + "'");
                    }
                    //2==部门负责人
                    var oneBmfzrDfLst = GetListByKhid(khid).DistinctBy(p => p.JGBM).OrderByDescending(p => p.FZRZDF).ToList();
                    decimal bmFzrPjf = oneBmfzrDfLst.Average(p => p.FZRZDF).ToDecimal();
                    for (int i = 0; i < oneBmfzrDfLst.Count; i++)
                    {
                        if (bmFzrPjf == 0) continue;
                        oneBmfzrDfLst[i].FZRPM = (i + 1);
                        oneBmfzrDfLst[i].FZRPJF = bmFzrPjf;
                        oneBmfzrDfLst[i].FZRDXBS = oneBmfzrDfLst[i].FZRZDF / bmFzrPjf;
                        oneBmfzrDfLst[i].DB_Option_Action = WebKeys.UpdateAction;
                        //保存负责人得分
                        sqlSB.Append("update [PERFO_KH_JGBMKHDF] set FZRPM='" + oneBmfzrDfLst[i].FZRPM
                            + "',FZRPJF='" + oneBmfzrDfLst[i].FZRPJF + "',FZRDXBS='" + oneBmfzrDfLst[i].FZRDXBS + "' where JGBM='"
                        + oneBmfzrDfLst[i].JGBM + "' and KHID='" + oneBmfzrDfLst[i].KHID + "'");
                    }
                    //更新SQL
                    isResult = dal.ExecuteSQL(sqlSB.ToString());
                }
            }
            return isResult;
        }

        #endregion
    }

}
