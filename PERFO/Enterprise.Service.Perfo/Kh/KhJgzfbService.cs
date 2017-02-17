using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Service.Perfo.Sys;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// 文件名:  KhJgzfbService.cs
    /// 功能描述: 业务逻辑层-机关作风建设考核汇总表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/28 16:45:02
    /// </summary>
    public class KhJgzfbService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhJgzfbData dal = new KhJgzfbData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJgzfbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhJgzfbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhJgzfbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhJgzfbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhJgzfbModel model)
        {
            return dal.Execute(model);
        }

        /// <summary>
        /// 执行原生SQL操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecuteBySQL(string sql)
        {
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// 删除指定考核下的数据
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public bool DeleteByKhid(string khid)
        {
            string sql = "delete from PERFO_KH_JGZFB where KHID='" + khid + "'";
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// 批量执行添加、修改、删除操作
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool ExecuteByList(List<KhJgzfbModel> models)
        {
            bool isOk = true;
            foreach (var model in models)
            {
                isOk = dal.Execute(model);
            }
            return isOk;
        }

        #endregion

        /// <summary>
        /// 更新机关作风建设的得分信息
        /// </summary>
        /// <param name="khid"></param>
        /// <param name="jgbm"></param>
        /// <param name="zbbm"></param>
        /// <param name="dfzlx"></param>
        /// <param name="zfkhdf"></param>
        /// <param name="zdf"></param>
        /// <param name="pm"></param>
        /// <param name="jjf"></param>
        /// <returns></returns>
        public bool UpdateJGZF(int khid, int jgbm, string zbbm, string dfzlx, 
            decimal zfkhdf, decimal zdf, int pm, decimal jjf)
        {
            /*
             四步：
             * 1、同级部门打分的结果
             * 2、同级二级单位的结果
             * 3、上级打分的结果
             * 4、总得分、排名、加分结果
             */
            //KHID  JGBM  ZBBM  DFZLX
            string sql =
                string.Format("update [PERFO_KH_JGZFB] set ZFKHDF='{0}',ZDF='{1}',ZFPM='{2}',SJDF='{3}' "
                + " where KHID='{4}' and JGBM='{5}' and ZBBM='{6}' and DFZLX='{7}';", 
                zfkhdf, zdf, pm, jjf, khid, jgbm, zbbm, dfzlx);
            return ExecuteBySQL(sql);
        }

        /// <summary>
        /// 获取指定考核ID对应的数据集
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhJgzfbModel> GetListByKhid(string khid)
        {
            string hql = "from KhJgzfbModel p where p.KHID='" + khid + "' order by p.DFZBXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取指定考核期和单位对应的数据集
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public IList<KhJgzfbModel> GetListByKhidAndJgbm(string khid, string jgbm)
        {
            string hql = "from KhJgzfbModel p where p.KHID='" + khid + "' and p.JGBM='" + jgbm + "' order by p.DFZBXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 完成机关作风建设数据的初始化
        /// </summary>
        /// <param name="dfzbmxLst">打分指标明细</param>
        /// <param name="sjsbLst">数据上报单位集合</param>
        /// <returns></returns>
        public bool InitJgzfData(List<KhDfzbmxModel> dfzbmxLst, List<KhSjsbModel> sjsbLst)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            /*
             1=以数据上报中的单位循环提取打分指标集各单位的指标
             2=判断各指标的打分者，依次把打分指标存入
             */
            int index = 1;
            List<KhJgzfbModel> list = new List<KhJgzfbModel>();
            foreach (var sjsb in sjsbLst)
            {
                //1==提取单位下的所有指标
                var jgDfzbLst = dfzbmxLst.Where(p => p.JGBM == sjsb.JGBM);
                foreach (var dfzb in jgDfzbLst)
                {
                    //2==提取各指标对应的打分者，并依次处理
                    foreach (var m in dfzb.KhJgbmdfbLst)
                    {
                        //3==分析各打分者分属上级用户，同级部门，同级二级单位
                        int jgbm = m.DFZ.ToInt();
                        if (jgbm > 0)
                        {
                            //同级机构
                            var bmjg = bmjgSrv.GetSingle(jgbm.ToString());
                            if (m.DFZLX == ((int)WebKeys.DFUserType.同级部门).ToString() || bmjg.JGLX.Contains("职能"))
                            {
                                //同级部门 02
                                KhJgzfbModel model = new KhJgzfbModel();
                                model.DB_Option_Action = WebKeys.InsertAction;
                                model.KHID = dfzb.KHID.Value;
                                model.ZBBM = dfzb.ZBBM;
                                model.ZBMC = dfzb.ZbsxModel.JxzrsZb.Dfzb.Zbxx.ZBMC;
                                model.JGBM = dfzb.JGBM.Value;
                                model.DFZSL = dfzb.KhJgbmdfbLst.Count;
                                model.ZFID = CommonTool.GetGuidKey();
                                model.DFZLX = ((int)WebKeys.DFUserType.同级部门).ToString();
                                model.DFZBXH = "02" + CommonTool.BuZero_3(index++);
                                if (!list.Exists(p => p.ZBBM == model.ZBBM &&
                                    p.DFZLX.ToInt() == (int)WebKeys.DFUserType.同级部门))
                                {
                                    list.Add(model);
                                }
                            }
                            else if (m.DFZLX == ((int)WebKeys.DFUserType.同级二级单位).ToString() || bmjg.JGLX.Contains("二级"))
                            {
                                //同级二级单位 03
                                KhJgzfbModel model = new KhJgzfbModel();
                                model.DB_Option_Action = WebKeys.InsertAction;
                                model.KHID = dfzb.KHID.Value;
                                model.ZBBM = dfzb.ZBBM;
                                model.ZBMC = dfzb.ZbsxModel.JxzrsZb.Dfzb.Zbxx.ZBMC;
                                model.JGBM = dfzb.JGBM.Value;
                                model.DFZSL = dfzb.KhJgbmdfbLst.Count;
                                model.ZFID = CommonTool.GetGuidKey();
                                model.DFZLX = ((int)WebKeys.DFUserType.同级二级单位).ToString();
                                model.DFZBXH = "03" + CommonTool.BuZero_3(index++);
                                if (!list.Exists(p => p.ZBBM == model.ZBBM &&
                                    p.DFZLX.ToInt() == (int)WebKeys.DFUserType.同级二级单位))
                                {
                                    list.Add(model);
                                }
                            }
                        }
                        else
                        {
                            //上级领导 01
                            KhJgzfbModel model = new KhJgzfbModel();
                            model.DB_Option_Action = WebKeys.InsertAction;
                            model.KHID = dfzb.KHID.Value;
                            model.ZBBM = dfzb.ZBBM;
                            model.ZBMC = dfzb.ZbsxModel.JxzrsZb.Dfzb.Zbxx.ZBMC;
                            model.JGBM = dfzb.JGBM.Value;
                            model.DFZSL = dfzb.KhJgbmdfbLst.Count;
                            model.ZFID = CommonTool.GetGuidKey();
                            model.DFZLX = ((int)WebKeys.DFUserType.上级领导).ToString();
                            model.DFZBXH = "01" + CommonTool.BuZero_3(index++);
                            if (!list.Exists(p => p.ZBBM == model.ZBBM &&
                                p.DFZLX.ToInt() == (int)WebKeys.DFUserType.上级领导))
                            {
                                list.Add(model);
                            }
                        }
                    }
                }
            }
            return ExecuteByList(list);
        }

        /// <summary>
        /// 计算机关作风指标各部门的得分
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <returns></returns>
        public bool CalculateJgzfDF(string khid)
        {
            /*
             1、提取当前考核下的所有作风建设打分记录
             2、从机关部门打分表中提取对应记录，进行分析统计得分
             */
            bool isOk = true;
            KhJgbmdfbService jgbmdfSrv = new KhJgbmdfbService();//机关部门打分表
            KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//打分指标明细
            //1==提取打分指标表中的 机关作风建设 指标
            List<KhDfzbmxModel> zbmxList = dfzbmxSrv.GetListByKhidAndKhdx(khid, WebKeys.KaoheType.机关部门)
                .Where(p=>p.ZbsxModel.JxzrsZb.Dfzb.Zbxx.YJZBMC.Contains("机关作风")).ToList();
            var jgzfbList = GetListByKhid(khid) as List<KhJgzfbModel>;
            foreach (var jgzf in jgzfbList)
            {
                var dfzb = zbmxList.FirstOrDefault(p => p.JGBM == jgzf.JGBM && p.ZBBM == jgzf.ZBBM);
                if (dfzb == null || dfzb.KhJgbmdfbLst.Count == 0) continue;
                var dfzLst = dfzb.KhJgbmdfbLst.Where(p => p.DFZLX == jgzf.DFZLX && p.KHDF != null).ToList();
                //2==合计其相同打分者类型的打分结果与统计其数量
                if (dfzLst.Count > 0)
                {
                    jgzf.ZFKHDF = dfzLst.Sum(p => p.KHDF) / dfzLst.Count * 100M;//得分
                    jgzf.DFZSL = dfzLst.Count;//打分者数量
                    jgzf.DB_Option_Action = WebKeys.UpdateAction;
                    isOk = Execute(jgzf);
                }
            }
            //3==分项得分统计完成后，还要合成最终得分和排名、加分情况
            jgzfbList = GetListByKhid(khid) as List<KhJgzfbModel>;
            var jgbms = jgzfbList.DistinctBy(p => p.JGBM).ToList();//一个单位一条记录
            List<KhJgzfbModel> jgbmsLst = new List<KhJgzfbModel>();
            foreach (var jgbm in jgbms)
            {
                jgbm.DB_Option_Action = WebKeys.UpdateAction;
                int dfzlxCount = 0;//打分者类型数量
                var zpLst = jgzfbList.Where(p => p.JGBM == jgbm.JGBM && 
                    p.DFZLX == ((int)WebKeys.DFUserType.同级部门).ToString()).ToList();
                decimal zpDefen = 0M;
                if (zpLst.Count > 0) {
                    dfzlxCount += 1;
                    zpDefen = zpLst.Sum(p => p.ZFKHDF).ToDecimal() / zpLst.Count;
                }

                var ejdwLst = jgzfbList.Where(p => p.JGBM == jgbm.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.同级二级单位).ToString()).ToList();
                decimal ejdwDefen = 0M;
                if (ejdwLst.Count > 0)
                {
                    dfzlxCount += 1;
                    ejdwDefen = ejdwLst.Sum(p => p.ZFKHDF).ToDecimal() / ejdwLst.Count;
                }

                var ldLst = jgzfbList.Where(p => p.JGBM == jgbm.JGBM &&
                    p.DFZLX == ((int)WebKeys.DFUserType.上级领导).ToString()).ToList();
                decimal ldDefen = 0M;
                if (ldLst.Count > 0)
                {
                    dfzlxCount += 1;
                    ldDefen = ldLst.Sum(p => p.ZFKHDF).ToDecimal() / ldLst.Count;
                }

                //计算总得分和平均
                if (dfzlxCount > 0)
                {
                    //总得分=三种类型的平均数之和再取平均
                    jgbm.ZDF = (zpDefen + ejdwDefen + ldDefen) / dfzlxCount;
                    jgbmsLst.Add(jgbm);
                }
            }
            //4==对各单位的总得分进行排序，添加排名和加分值
            /*
             * 将各级测评得分通过计算汇总形成评价结果（平均），
             * 对排第1-5位的加5分，
             * 排6-10位的加3分，排11-15位的加2分，排16-20位的加1分。
             */
            jgbmsLst = jgbmsLst.OrderByDescending(p => p.ZDF).ToList();
            for (int i = 0; i < jgbmsLst.Count; i++ )
            {
                jgbmsLst[i].ZFPM = (i + 1);
                if (i < 5) jgbmsLst[i].SJDF = 5;
                else if (i >= 5 && i < 10) jgbmsLst[i].SJDF = 3;
                else if (i >= 10 && i < 15) jgbmsLst[i].SJDF = 2;
                else if (i >= 15 && i < 20) jgbmsLst[i].SJDF = 1;
                else jgbmsLst[i].SJDF = 0;

                string sql = string.Format(
                    "update [PERFO_KH_JGZFB] set ZDF='{0}',ZFPM='{1}',SJDF='{2}' where KHID='{3}' and JGBM='{4}';",
                    jgbmsLst[i].ZDF, jgbmsLst[i].ZFPM, jgbmsLst[i].SJDF, jgbmsLst[i].KHID, jgbmsLst[i].JGBM);
                ExecuteBySQL(sql);
            }
            return isOk;
        }
    }

}
