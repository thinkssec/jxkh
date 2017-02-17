using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 量化指标考核表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhJxzrsModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///责任书ID
        /// </summary>
        public virtual int ZRSID
        {
            get;
            set;
        }

        /// <summary>
        ///机构编码
        /// </summary>
        public virtual int? JGBM
        {
            get;
            set;
        }

        /// <summary>
        ///责任书名称
        /// </summary>
        public virtual string ZRSMC
        {
            get;
            set;
        }

        /// <summary>
        ///责任书状态
        /// </summary>
        public virtual string ZRSZT
        {
            get;
            set;
        }

        /// <summary>
        ///所在年度
        /// </summary>
        public virtual int? SZND
        {
            get;
            set;
        }

        /// <summary>
        ///责任书附件
        /// </summary>
        public virtual string ZRSFJ
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
        ///提交日期
        /// </summary>
        public virtual DateTime? TJRQ
        {
            get;
            set;
        }

        /// <summary>
        ///负责部门
        /// </summary>
        public virtual int? FZBM
        {
            get;
            set;
        }

        /// <summary>
        ///下达日期
        /// </summary>
        public virtual DateTime? XDRQ
        {
            get;
            set;
        }

        #endregion

        #region 关联数据

        /// <summary>
        /// 责任书指标集合
        /// </summary>
        public virtual IList<KhJxzrszbModel> JxzrszbLst { get; set; }

        /// <summary>
        /// 所属机构
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        /// <summary>
        /// 负责责任书管理的机构
        /// </summary>
        public virtual SysBmjgModel FzBmjg { get; set; }

        /// <summary>
        /// 考核类型对象
        /// </summary>
        public virtual KhKindModel KhKind { get; set; }

        #endregion

    }

}
