using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// 机构管理
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:45
    /// </summary>
    [Serializable]
    public class SysBmjgModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///机构编码
        /// </summary>
        public virtual int JGBM
        {
            get;
            set;
        }

        /// <summary>
        ///机构名称
        /// </summary>
        public virtual string JGMC
        {
            get;
            set;
        }

        /// <summary>
        ///机构类型
        /// </summary>
        public virtual string JGLX
        {
            get;
            set;
        }

        /// <summary>
        ///是否考核 1=是 0=否
        /// </summary>
        public virtual string SFKH
        {
            get;
            set;
        }

        /// <summary>
        ///显示序号
        /// </summary>
        public virtual string XSXH
        {
            get;
            set;
        }

        /// <summary>
        ///备注==同级排序
        /// </summary>
        public virtual string BZ
        {
            get;
            set;
        }

        #endregion
    }

}
