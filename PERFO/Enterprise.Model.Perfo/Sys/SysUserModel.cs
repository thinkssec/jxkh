using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// 用户表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class SysUserModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///登录ID
        /// </summary>
        public virtual string LOGINID
        {
            get;
            set;
        }

        /// <summary>
        ///机构编码
        /// </summary>
        public virtual int JGBM
        {
            get;
            set;
        }

        /// <summary>
        ///角色ID
        /// </summary>
        public virtual string ROLEID
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
        ///密码
        /// </summary>
        public virtual string PASSWORD
        {
            get;
            set;
        }

        /// <summary>
        ///是否禁用 1=是
        /// </summary>
        public virtual string DISABLE
        {
            get;
            set;
        }

        /// <summary>
        ///IP地址
        /// </summary>
        public virtual string IPADDR
        {
            get;
            set;
        }

        /// <summary>
        ///职务
        /// </summary>
        public virtual string DUTY
        {
            get;
            set;
        }

        /// <summary>
        ///添加日期
        /// </summary>
        public virtual DateTime? ADDDATE
        {
            get;
            set;
        }

        /// <summary>
        ///用户菜单风格
        /// </summary>
        public virtual string MENU
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string BZ
        {
            get;
            set;
        }

        #endregion


        #region 自定义属性

        /// <summary>
        /// 角色
        /// </summary>
        public virtual SysRoleModel Role { get; set; }

        /// <summary>
        /// 所属机构
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        /// <summary>
        /// 分管的机构信息集合
        /// </summary>
        public virtual IList<SysFgbmjgModel> FgbmjgLst { get; set; }

        #endregion

    }

}
