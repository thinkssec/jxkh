using Enterprise.Model.Perfo.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 二级单位考核得分表
    /// 创建人:代码生成器
    /// 创建时间:2014/12/2 13:41:05
    /// </summary>
    [Serializable]
    public class KhEjdwkhdfModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///得分表ID
        /// </summary>
        public virtual string DFID
        {
            get;
            set;
        }

        /// <summary>
        ///机构编码
        /// </summary>
        public virtual int? JGBM
        {
            get;
            set;
        }

        /// <summary>
        /// 机构名称
        /// 合并计分时显示合并规则名称
        /// </summary>
        public virtual string JGMC
        {
            get;
            set;
        }

        /// <summary>
        ///考核ID
        /// </summary>
        public virtual int KHID
        {
            get;
            set;
        }

        /// <summary>
        ///考核项名称
        /// </summary>
        public virtual string KHXMC
        {
            get;
            set;
        }

        /// <summary>
        ///考核标准分
        ///合并计分领导班子时保存加减分值
        /// </summary>
        public virtual decimal? KHBZF
        {
            get;
            set;
        }

        /// <summary>
        ///考核得分
        ///合并计分领导班子时保存综合得分
        /// </summary>
        public virtual decimal? KHDF
        {
            get;
            set;
        }

        /// <summary>
        ///考核类型
        /// </summary>
        public virtual string KHLX
        {
            get;
            set;
        }

        /// <summary>
        ///统计时间
        /// </summary>
        public virtual DateTime? TJSJ
        {
            get;
            set;
        }

        /// <summary>
        ///操作人
        /// </summary>
        public virtual string CZR
        {
            get;
            set;
        }

        /// <summary>
        ///考核项序号
        /// </summary>
        public virtual string XMXH
        {
            get;
            set;
        }

        /// <summary>
        ///考核单位数量
        /// </summary>
        public virtual int? KHDWSL
        {
            get;
            set;
        }

        /// <summary>
        ///单位总得分
        /// </summary>
        public virtual decimal? DWZDF
        {
            get;
            set;
        }

        /// <summary>
        ///单位平均分
        ///暂用作 （合并计分单位）加分合计
        /// </summary>
        public virtual decimal? DWPJF
        {
            get;
            set;
        }

        /// <summary>
        ///单位考核排名
        /// </summary>
        public virtual int? DWPM
        {
            get;
            set;
        }

        /// <summary>
        ///单位兑现倍数
        ///暂用作 （合并计分单位）减分合计
        /// </summary>
        public virtual decimal? DWDXBS
        {
            get;
            set;
        }

        /// <summary>
        ///备注说明
        /// </summary>
        public virtual string BZSM
        {
            get;
            set;
        }

        /// <summary>
        ///负责人总得分
        /// </summary>
        public virtual decimal? FZRZDF
        {
            get;
            set;
        }

        /// <summary>
        ///负责人平均分
        /// </summary>
        public virtual decimal? FZRPJF
        {
            get;
            set;
        }

        /// <summary>
        ///班子考核排名
        /// </summary>
        public virtual int? FZRPM
        {
            get;
            set;
        }

        /// <summary>
        ///班子得分类别
        /// </summary>
        public virtual string FZRDFLB
        {
            get;
            set;
        }

        /// <summary>
        ///兑现倍数
        /// </summary>
        public virtual decimal? FZRDXBS
        {
            get;
            set;
        }

        /// <summary>
        ///难度系数
        /// </summary>
        public virtual decimal? NDXS
        {
            get;
            set;
        }

        /// <summary>
        ///汇总标志
        /// </summary>
        public virtual string HZBZ
        {
            get;
            set;
        }

        /// <summary>
        ///合并计分规则
        /// </summary>
        public virtual string HBJFID
        {
            get;
            set;
        }

        /// <summary>
        ///合并计分项
        /// </summary>
        public virtual string ISHBJF
        {
            get;
            set;
        }

        /// <summary>
        /// 归属单位名称
        /// </summary>
        public virtual string GSDWMC
        {
            get;
            set;
        }

        #endregion

        #region 关联项

        /// <summary>
        /// 部门机构实例
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        /// <summary>
        /// 合并计分规则实例
        /// </summary>
        public virtual KhHbjfgzModel Hbjf { get; set; }

        #endregion
    }

}
