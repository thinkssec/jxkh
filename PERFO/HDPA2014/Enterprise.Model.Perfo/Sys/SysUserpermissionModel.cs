using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// 
    /// 创建人:代码生成器
    /// 创建时间:2014/12/1 9:32:18
    /// </summary>
    [Serializable]
    public class SysUserpermissionModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///用户登录名
        /// </summary>
        public virtual string LOGINID
        {
            get;
            set;
        }

        /// <summary>
        ///模块ID
        /// </summary>
        public virtual string MID
        {
            get;
            set;
        }

        /// <summary>
        ///权限值
        /// </summary>
        public virtual int? MODULEPERMISSION
        {
            get;
            set;
        }

        #endregion

        #region 自定义属性

        /// <summary>
        /// 模块对象
        /// </summary>
        public virtual SysModuleModel Module { get; set; }

        #endregion
    }

}
