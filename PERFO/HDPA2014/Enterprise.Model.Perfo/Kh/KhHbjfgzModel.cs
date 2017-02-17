using Enterprise.Model.Perfo.Zbk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 合并计分规则表
    /// 创建人:代码生成器
    /// 创建时间:2014/12/2 13:41:05
    /// </summary>
    [Serializable]
    public class KhHbjfgzModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        /// 合并计分ID
        /// </summary>
        public virtual string HBJFID
        {
            get;
            set;
        }

        /// <summary>
        ///合并计分名称
        /// </summary>
        public virtual string HBJFMC
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
        ///关联单位
        /// </summary>
        public virtual string HBJFDW
        {
            get;
            set;
        }

        /// <summary>
        ///关联规则ID
        /// </summary>
        public virtual string GZID
        {
            get;
            set;
        }

        #endregion

        #region 关联项

        /// <summary>
        /// 计算规则信息
        /// </summary>
        public virtual ZbkJsgzModel Jsgz { get; set; }

        #endregion
    }

}
