using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 打分指标考核表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:51
    /// </summary>
    [Serializable]
    public class KhDfzbmxModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///打分指标ID
        /// </summary>
        public virtual string DFZBID
        {
            get;
            set;
        }

        /// <summary>
        ///筛选表ID
        /// </summary>
        public virtual string SXID
        {
            get;
            set;
        }

        /// <summary>
        ///指标编码
        /// </summary>
        public virtual string ZBBM
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
        ///考核ID
        /// </summary>
        public virtual int? KHID
        {
            get;
            set;
        }

        /// <summary>
        ///打分值
        /// </summary>
        public virtual decimal? DFSZ
        {
            get;
            set;
        }

        /// <summary>
        ///打分说明
        /// </summary>
        public virtual string DFBZ
        {
            get;
            set;
        }

        /// <summary>
        ///打分日期
        /// </summary>
        public virtual DateTime? DFRQ
        {
            get;
            set;
        }

        /// <summary>
        ///打分者
        /// </summary>
        public virtual string DFZ
        {
            get;
            set;
        }

        /// <summary>
        ///得分
        /// </summary>
        public virtual decimal? SJDF
        {
            get;
            set;
        }

        /// <summary>
        ///约束说明
        /// </summary>
        public virtual string YSSM
        {
            get;
            set;
        }

        /// <summary>
        ///考核对象
        ///二级单位 = 1,
        ///领导班子 = 2,
        ///机关部门 = 3,
        ///部门负责人 = 4
        /// </summary>
        public virtual string KHDX
        {
            get;
            set;
        }

        #endregion

        #region 关联数据项

        /// <summary>
        /// 指标筛选MODEL
        /// </summary>
        public virtual KhZbsxModel ZbsxModel { get; set; }

        /// <summary>
        /// 打分指标MODEL
        /// </summary>
        public virtual ZbkDfzbModel DfzbModel { get; set; }

        /// <summary>
        /// 部门机构
        /// </summary>
        public virtual SysBmjgModel Danwei { get; set; }

        /// <summary>
        /// 审核人打分结果集合
        /// </summary>
        public virtual IList<KhJgbmdfbModel> KhJgbmdfbLst { get; set; }

        #endregion
    }

}
