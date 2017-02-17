using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 考核制度汇编
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhKindhbModel : PerfoSuperModel
    {

        #region 代码生成器

        /// <summary>
        ///文件ID
        /// </summary>
        public virtual int WJID
        {
            get;
            set;
        }

        /// <summary>
        ///考核类型ID
        /// </summary>
        public virtual string LXID
        {
            get;
            set;
        }

        /// <summary>
        ///文件名称
        /// </summary>
        public virtual string WJMC
        {
            get;
            set;
        }

        /// <summary>
        ///在线浏览
        /// </summary>
        public virtual string ZXLL
        {
            get;
            set;
        }

        /// <summary>
        ///文件附件
        /// </summary>
        public virtual string WJFJ
        {
            get;
            set;
        }

        /// <summary>
        ///文件状态
        /// </summary>
        public virtual string WJZT
        {
            get;
            set;
        }

        /// <summary>
        ///添加日期
        /// </summary>
        public virtual DateTime? TJRQ
        {
            get;
            set;
        }

        #endregion

        #region 关联项

        /// <summary>
        /// 考核类型实例
        /// </summary>
        public virtual KhKindModel Kind { get; set; }

        #endregion

    }

}
