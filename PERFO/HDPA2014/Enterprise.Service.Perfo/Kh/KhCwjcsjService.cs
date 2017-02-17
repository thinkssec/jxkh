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
    /// 文件名:  KhCwjcsjService.cs
    /// 功能描述: 业务逻辑层-财务基础数据表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/12/13 10:23:51
    /// </summary>
    public class KhCwjcsjService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhCwjcsjData dal = new KhCwjcsjData();

        /// <summary>
        /// 财务基础数据项
        /// </summary>
        public static readonly string[] JcsjZbs = new string[] { 
                "营业总收入（累计）",
                "其中：对外创收（累计）",
                "营业总成本（累计）",
                "管理费用（累计）",
                "其中：（1）办公费（累计）",
                "（2）差旅费（累计）",
                "（3）会议费（累计）",
                "（4）业务招待费（累计）",
                "（5）车辆使用费（累计）",
                "（6）出国人员经费（累计）",
                "财务费用（累计）",
                "其中：利息支出（累计）",
                "营业利润（累计）",
                "营业外收入（累计）",
                "营业外支出（累计）",
                "利润总额（累计）",
                "净利润（累计）",
                "人工成本（累计）",
                "固定资产折旧（累计）",
                "无形资产摊销（累计）",
                "长期待摊费用摊销（累计）",
                "科研经费（累计）",
                "局批复的专项补贴（累计）",
                "公益保障性业务总支出（累计）",
                "非在职人员费用（累计）",
                "公益保障性业务总收入（累计）",
                "资产总额（平均）",
                "固定资产原值（平均）",
                "在建工程（平均）",
                "无形资产原值（平均）",
                "长期待摊费用原值（平均）",
                "应付票据（平均）",
                "应付账款（平均）",
                "预收账款（平均）",
                "应付职工薪酬（平均）",
                "应缴税费（平均）",
                "应付利息（平均）",
                "其他应付款（平均）",
                "内部拨入款（平均）",
                "其他流动负债（平均）",
                "专项应付款（平均）",
                "实收资本（平均）",
                "资本公积（平均）",
                "补充流动资金（平均）",
                "EVA（累计）"
            };

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhCwjcsjModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhCwjcsjModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhCwjcsjModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhCwjcsjModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhCwjcsjModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 获取指定的单位编码和年份的基础数据
        /// </summary>
        /// <param name="jgbm">单位编码</param>
        /// <param name="nf">年份</param>
        /// <returns></returns>
        public IList<KhCwjcsjModel> GetListByJgbmAndNF(string jgbm, int nf)
        {
            string hql = "from KhCwjcsjModel p where p.JGBM='" + jgbm + "' and p.NF='" + nf + "' order by p.NF, p.XH";
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 初始化指标考核年份下的指定单位基础数据
        /// </summary>
        /// <param name="yy">年份</param>
        /// <param name="jgbm">单位编码</param>
        /// <returns></returns>
        public bool InitJcsjDataBySzndAndJgbm(int yy, int jgbm)
        {
            bool isOk = DeleteJcsjData(yy, jgbm);
            for (int i = 0; i < JcsjZbs.Length; i++)
            {
                KhCwjcsjModel jcsjM = new KhCwjcsjModel();
                jcsjM.DB_Option_Action = WebKeys.InsertAction;
                jcsjM.CWZBID = CommonTool.GetGuidKey();
                jcsjM.ZBMC = JcsjZbs[i];
                jcsjM.XH = (i + 1);
                jcsjM.NF = yy;
                jcsjM.JGBM = jgbm;
                isOk = Execute(jcsjM);
            }
            return isOk;
        }

        /// <summary>
        /// 删除指定单位的所属年份的数据
        /// </summary>
        /// <param name="yy">年份</param>
        /// <param name="jgbm">单位编码</param>
        /// <returns></returns>
        public bool DeleteJcsjData(int yy, int jgbm)
        {
            string sql = "delete from [PERFO_KH_CWJCSJ] where JGBM='" + jgbm + "' and NF='" + yy + "';";
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// 计算指定单位和年份的EVA总额数据
        /// </summary>
        /// <param name="jgbm">单位编码</param>
        /// <param name="yy">年份</param>
        /// <returns></returns>
        public bool CalcAndSaveEVA(int jgbm, int yy)
        {
            /*
             {EVA（累计）}={净利润（累计）}+({其中：利息支出（累计）}+{科研经费（累计）})*0.75-{营业外收入（累计）}+{营业外支出（累计）}-（{资产总额（平均）}-{应付票据（平均）}-{应付账款（平均）}-{预收账款（平均）}-{应付职工薪酬（平均）}-{应缴税费（平均）}-{应付利息（平均）}-{其他应付款（平均）}-{内部拨入款（平均）}-{其他流动负债（平均）}-{专项应付款（平均）}-{在建工程（平均）})*0.055
             思路：
             * 1、提取指定单位和年度的基础数据；
             * 2、根据EVA公式替换各指标值，并计算EVA
             * 3、保存计算结果
             */
            var jcsjLst = GetListByJgbmAndNF(jgbm.ToString(), yy);
            string evabds = "{净利润（累计）}+({其中：利息支出（累计）}+{科研经费（累计）})*0.75-{营业外收入（累计）}+{营业外支出（累计）}-({资产总额（平均）}-{应付票据（平均）}-{应付账款（平均）}-{预收账款（平均）}-{应付职工薪酬（平均）}-{应缴税费（平均）}-{应付利息（平均）}-{其他应付款（平均）}-{内部拨入款（平均）}-{其他流动负债（平均）}-{专项应付款（平均）}-{在建工程（平均）})*0.055";
            foreach (var jcsj in jcsjLst)
            {
                if (jcsj.ZBMC.Contains("累计"))
                {
                    evabds = evabds.Replace("{" + jcsj.ZBMC + "}", "(" + jcsj.LJZ.ToDecimal().ToString() + ")");
                }
                else if (jcsj.ZBMC.Contains("平均"))
                {
                    evabds = evabds.Replace("{" + jcsj.ZBMC + "}", "(" + jcsj.PJZ.ToDecimal().ToString() + ")");
                }
            }
            decimal eva = Utility.Eval(evabds).ToDecimal();
            //保存EVA数据
            decimal pjz = eva / 12;
            string sql = string.Format(
                "update [PERFO_KH_CWJCSJ] set LJZ='{0}',M1='{1}',M2='{2}',M3='{3}',M4='{4}',M5='{5}',M6='{6}',M7='{7}',M8='{8}',M9='{9}',M10='{10}',M11='{11}',M12='{12}' where ZBMC='EVA（累计）' and JGBM='{13}' and NF='{14}'",
                eva, pjz, pjz, pjz, pjz, pjz, pjz, pjz, pjz, pjz, pjz, pjz, pjz, jgbm, yy);
            return dal.ExecuteSQL(sql);
        }

        #endregion
    }

}
