using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// 完成值审核及打分者
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkWczshdfModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        /// 操作ID
        /// </summary>
        public virtual int OPERID
        {
            get;
            set;
        }

        /// <summary>
        ///量化指标编码
        /// </summary>
        public virtual string LHZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///操作者
        /// </summary>
        public virtual string OPERATOR
        {
            get;
            set;
        }

        /// <summary>
        ///操作者类型 1=二级单位考核 0=机关部门考核
        /// </summary>
        public virtual string OPERTYPE
        {
            get;
            set;
        }

        /// <summary>
        ///操作权重
        /// </summary>
        public virtual decimal? OPERQZ
        {
            get;
            set;
        }

        #endregion
    }

}
