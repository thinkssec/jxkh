using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 数据解锁
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhUnlockModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///锁ID
        /// </summary>
        public virtual string SID
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
        ///操作者
        /// </summary>
        public virtual string CZZ
        {
            get;
            set;
        }

        /// <summary>
        ///操作者类型 1=机构 0=指定用户
        /// </summary>
        public virtual string CZZLX
        {
            get;
            set;
        }

        /// <summary>
        ///提交时间
        /// </summary>
        public virtual DateTime? TJSJ
        {
            get;
            set;
        }

        /// <summary>
        ///锁定标志
        /// </summary>
        public virtual string SDBZ
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

        #endregion

        #region 关联数据项

        /// <summary>
        /// 部门机构实例
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        #endregion
    }

}
