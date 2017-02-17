using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// 角色管理
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class SysRoleModel : PerfoSuperModel
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
        ///角色名称
        /// </summary>
        public virtual string ROLENAME
        {
            get;
            set;
        }

        /// <summary>
        ///是否禁用 1=是 0=否
        /// </summary>
        public virtual string ROLEDISABLE
        {
            get;
            set;
        }

        /// <summary>
        ///角色图片
        /// </summary>
        public virtual string ROLEPICTURE
        {
            get;
            set;
        }

        #endregion
    }

}
