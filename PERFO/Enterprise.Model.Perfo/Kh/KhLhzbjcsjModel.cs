using Enterprise.Model.Perfo.Zbk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 量化考核基础数据表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhLhzbjcsjModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///基础指标ID
        /// </summary>
        public virtual string JCZBID
        {
            get;
            set;
        }

        /// <summary>
        ///量化考核表ID
        /// </summary>
        public virtual string ID
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
        ///指标代号
        /// </summary>
        public virtual string ZBDH
        {
            get;
            set;
        }

        /// <summary>
        ///指标名称
        /// </summary>
        public virtual string ZBMC
        {
            get;
            set;
        }

        /// <summary>
        ///指标上报值
        /// </summary>
        public virtual decimal? ZBSBZ
        {
            get;
            set;
        }

        /// <summary>
        ///指标审核值
        /// </summary>
        public virtual decimal? ZBSHZ
        {
            get;
            set;
        }

        /// <summary>
        ///指标审定值
        /// </summary>
        public virtual decimal? ZBSDZ
        {
            get;
            set;
        }

        /// <summary>
        ///指标值
        /// </summary>
        public virtual decimal? ZBZ
        {
            get;
            set;
        }

        /// <summary>
        ///指标上报说明
        /// </summary>
        public virtual string ZBSBBZ
        {
            get;
            set;
        }

        /// <summary>
        ///指标审核说明
        /// </summary>
        public virtual string ZBSHBZ
        {
            get;
            set;
        }

        /// <summary>
        ///指标审定说明
        /// </summary>
        public virtual string ZBSDBZ
        {
            get;
            set;
        }

        /// <summary>
        ///顺序号
        /// </summary>
        public virtual int? XH
        {
            get;
            set;
        }

        #endregion

        #region 关联数据项

        ///// <summary>
        ///// 量化指标MODEL
        ///// </summary>
        //public virtual ZbkLhzbModel LhzbModel { get; set; }

        #endregion
    }

}