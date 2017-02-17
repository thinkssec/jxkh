using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// 访问日志表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class SysVisitlogModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///ID
        /// </summary>
        public virtual string ID
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
        ///访问路径
        /// </summary>
        public virtual string VISITURL
        {
            get;
            set;
        }

        /// <summary>
        ///操作类型
        /// </summary>
        public virtual string OPERATIONTYPE
        {
            get;
            set;
        }

        /// <summary>
        ///操作时间
        /// </summary>
        public virtual DateTime? OPERATIONDATE
        {
            get;
            set;
        }

        /// <summary>
        ///来路IP
        /// </summary>
        public virtual string VISITIPADDR
        {
            get;
            set;
        }

        /// <summary>
        ///浏览器类型
        /// </summary>
        public virtual string BROWSERTYPE
        {
            get;
            set;
        }

        #endregion
    }

}
