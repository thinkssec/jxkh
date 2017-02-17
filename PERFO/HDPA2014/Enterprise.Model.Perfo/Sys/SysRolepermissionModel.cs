using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// 角色权限表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class SysRolepermissionModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///角色ID
        /// </summary>
        public virtual string ROLEID
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
        /// 总权限
        /// </summary>
        public virtual int? MODULEPERMISSION
        {
            get;
            set;
        }

        #endregion

        #region 自定义属性

        /// <summary>
        /// 角色对象
        /// </summary>
        public virtual SysRoleModel Role { get; set; }

        /// <summary>
        /// 模块对象
        /// </summary>
        public virtual SysModuleModel Module { get; set; }

        #endregion
    }

}
