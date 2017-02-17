using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// 指标管理
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkZbxxModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///指标ID
        /// </summary>
        public virtual int ZBID
        {
            get;
            set;
        }

        /// <summary>
        ///指标类型
        /// </summary>
        public virtual string ZBLX
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
        ///一级名称
        /// </summary>
        public virtual string YJZBMC
        {
            get;
            set;
        }

        /// <summary>
        ///二级名称
        /// </summary>
        public virtual string EJZBMC
        {
            get;
            set;
        }

        /// <summary>
        ///三级名称
        /// </summary>
        public virtual string SJZBMC
        {
            get;
            set;
        }

        /// <summary>
        ///顺序号
        /// </summary>
        public virtual string SXH
        {
            get;
            set;
        }

        /// <summary>
        ///指标状态 Y=有效 N=无效
        /// </summary>
        public virtual string ZBZT
        {
            get;
            set;
        }

        /// <summary>
        ///添加日期
        /// </summary>
        public virtual DateTime? TJRQ
        {
            get;
            set;
        }

        #endregion
    }

}
