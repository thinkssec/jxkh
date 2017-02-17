using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// 系统缓存管理
    /// 创建人:代码生成器
    /// 创建时间:	2014/10/31 10:02:24
    /// </summary>
    [Serializable]
    public class SysCacheModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///缓存项名称
        /// </summary>
        public virtual string CACHENAME
        {
            get;
            set;
        }

        /// <summary>
        ///用户名
        /// </summary>
        public virtual string USERNAME
        {
            get;
            set;
        }

        /// <summary>
        ///数据表名
        /// </summary>
        public virtual string TABLENAME
        {
            get;
            set;
        }

        /// <summary>
        /// 关联缓存项名称字串
        /// </summary>
        public virtual string RELATIONCACHE
        {
            get;
            set;
        }

        #endregion
    }

}
