using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 待办箱
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhMessageModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///待办ID
        /// </summary>
        public virtual string MSGID
        {
            get;
            set;
        }

        /// <summary>
        ///考核ID
        /// </summary>
        public virtual int? KHID
        {
            get;
            set;
        }

        /// <summary>
        ///登录ID
        /// </summary>
        public virtual string LOGINID
        {
            get;
            set;
        }

        /// <summary>
        ///事项名称
        /// </summary>
        public virtual string DBMC
        {
            get;
            set;
        }

        /// <summary>
        ///待办说明
        /// </summary>
        public virtual string DBSM
        {
            get;
            set;
        }

        /// <summary>
        ///待办链接
        /// </summary>
        public virtual string DBLJ
        {
            get;
            set;
        }

        /// <summary>
        ///接收日期
        /// </summary>
        public virtual DateTime? JSRQ
        {
            get;
            set;
        }

        /// <summary>
        ///当前状态
        /// </summary>
        public virtual string DQZT
        {
            get;
            set;
        }

        /// <summary>
        ///完成日期
        /// </summary>
        public virtual DateTime? WCRQ
        {
            get;
            set;
        }

        /// <summary>
        /// 模块ID
        /// </summary>
        public virtual string MID
        {
            get;
            set;
        }

        /// <summary>
        /// 发送人
        /// </summary>
        public virtual string FSR
        {
            get;
            set;
        }

        #endregion

        #region 关联数据项

        /// <summary>
        /// 考核期对象
        /// </summary>
        public virtual KhKhglModel Kaohe { get; set; }

        /// <summary>
        /// 用户MODEL
        /// </summary>
        public virtual SysUserModel User { get; set; }

        #endregion
    }

}