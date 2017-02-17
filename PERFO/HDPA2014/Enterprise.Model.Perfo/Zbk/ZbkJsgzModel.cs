using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// 计算规则配置
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkJsgzModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///规则ID
        /// </summary>
        public virtual string GZID
        {
            get;
            set;
        }

        /// <summary>
        ///版本名称
        /// </summary>
        public virtual string BBMC
        {
            get;
            set;
        }

        /// <summary>
        ///规则名称
        /// </summary>
        public virtual string GZMC
        {
            get;
            set;
        }

        /// <summary>
        ///表达式
        /// </summary>
        public virtual string GZBDS
        {
            get;
            set;
        }

        /// <summary>
        ///规则逻辑处理的方法名称
        /// </summary>
        public virtual string METHODNAME
        {
            get;
            set;
        }

        /// <summary>
        ///封顶值
        /// </summary>
        public virtual decimal? MAXV
        {
            get;
            set;
        }

        /// <summary>
        ///保底值
        /// </summary>
        public virtual decimal? MINV
        {
            get;
            set;
        }

        /// <summary>
        ///上限值
        /// </summary>
        public virtual decimal? UPPER
        {
            get;
            set;
        }

        /// <summary>
        ///下限值
        /// </summary>
        public virtual decimal? LOWER
        {
            get;
            set;
        }

        /// <summary>
        /// 旧表ID
        /// </summary>
        public virtual string OLDID
        {
            get;
            set;
        }

        #endregion
    }

}
