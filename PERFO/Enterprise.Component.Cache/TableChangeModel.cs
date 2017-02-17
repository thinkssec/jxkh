using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Component.Cache
{
    /// <summary>
    /// 数据表更新记录表-实体类
    /// </summary>
    [Serializable]
    public class TableChangeModel
    {

        /// <summary>
        /// 所有者名称
        /// </summary>
        public virtual string USERNAME
        {
            get;
            set;
        }

        /// <summary>
        /// 数据表名称
        /// </summary>
        public virtual string TABLENAME
        {
            get;
            set;
        }

        /// <summary>
        /// 变化ID
        /// </summary>
        public virtual decimal CHANGEID
        {
            get;
            set;
        }

        /// <summary>
        /// 变化时间
        /// </summary>
        public virtual DateTime CHANGETIME
        {
            get;
            set;
        }

    }
}
