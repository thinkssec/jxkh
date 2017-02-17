using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// 功能模块表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:45
    /// </summary>
    [Serializable]
    public class SysModuleModel : PerfoSuperModel
    {

        #region 代码生成器

        /// <summary>
        ///模块ID
        /// </summary>
        public virtual string MID
        {
            get;
            set;
        }

        /// <summary>
        ///功能名称
        /// </summary>
        public virtual string MNAME
        {
            get;
            set;
        }

        /// <summary>
        ///链接页面
        /// </summary>
        public virtual string MURL
        {
            get;
            set;
        }

        /// <summary>
        ///是否禁用 1=是 0=否
        /// </summary>
        public virtual string DISABLE
        {
            get;
            set;
        }

        /// <summary>
        ///父功能页面ID
        /// </summary>
        public virtual string MPARENTID
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
        ///备注
        /// </summary>
        public virtual string BZ
        {
            get;
            set;
        }

        /// <summary>
        ///映射路径
        /// </summary>
        public virtual string WEBURL
        {
            get;
            set;
        }

        #endregion

    }

}
