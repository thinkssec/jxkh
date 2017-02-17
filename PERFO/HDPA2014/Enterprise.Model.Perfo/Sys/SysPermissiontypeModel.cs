using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// 权限类型表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class SysPermissiontypeModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///动作值
        /// </summary>
        public virtual int ACTIONKEY
        {
            get;
            set;
        }

        /// <summary>
        ///动作简称
        /// </summary>
        public virtual string ACTIONABBR
        {
            get;
            set;
        }

        /// <summary>
        ///动作名称
        /// </summary>
        public virtual string ACTIONNAME
        {
            get;
            set;
        }

        #endregion

        #region 自定义属性

        /// <summary>
        /// 角色对象
        /// </summary>
        public virtual SysRoleModel Role
        {
            get;
            set;
        }

        /// <summary>
        /// 模块对象
        /// </summary>
        public virtual SysModuleModel Module
        {
            get;
            set;
        }

        #endregion

    }

}
