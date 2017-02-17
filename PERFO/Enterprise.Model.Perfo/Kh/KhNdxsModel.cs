using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 经营难度系数
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhNdxsModel : PerfoSuperModel
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
        ///机构编码
        /// </summary>
        public virtual int JGBM
        {
            get;
            set;
        }

        /// <summary>
        ///难度系数
        /// </summary>
        public virtual decimal? NDXS
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

        /// <summary>
        /// 考核管理实例
        /// </summary>
        public virtual KhKhglModel Kaohe { get; set; }

        #endregion
    }

}
