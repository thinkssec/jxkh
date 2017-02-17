using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 考核管理
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:45
    /// </summary>
    [Serializable]
    public class KhKhglModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///考核ID
        /// </summary>
        public virtual int KHID
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
        ///考核名称
        /// </summary>
        public virtual string KHMC
        {
            get;
            set;
        }

        /// <summary>
        ///考核年度
        /// </summary>
        public virtual string KHND
        {
            get;
            set;
        }

        /// <summary>
        /// 版本名称
        /// </summary>
        public virtual string BBMC
        {
            get;
            set;
        }

        /// <summary>
        ///考核周期
        /// </summary>
        public virtual string KHZQ
        {
            get;
            set;
        }

        /// <summary>
        ///开始时间
        /// </summary>
        public virtual DateTime? KSSJ
        {
            get;
            set;
        }

        /// <summary>
        ///关闭时间
        /// </summary>
        public virtual DateTime? GBSJ
        {
            get;
            set;
        }

        /// <summary>
        ///考核状态
        /// </summary>
        public virtual string KHZT
        {
            get;
            set;
        }

        /// <summary>
        ///考核发起人
        /// </summary>
        public virtual string FQR
        {
            get;
            set;
        }

        /// <summary>
        ///是否可查
        /// </summary>
        public virtual string SFKC
        {
            get;
            set;
        }

        /// <summary>
        ///考核月度
        /// </summary>
        public virtual int? KHYD
        {
            get;
            set;
        }

        /// <summary>
        ///考核季度
        /// </summary>
        public virtual int? KHJD
        {
            get;
            set;
        }

        #endregion

        #region 关联属性

        /// <summary>
        /// 考核类型实例
        /// </summary>
        public virtual KhKindModel Kind { get; set; }

        #endregion
    }

}
