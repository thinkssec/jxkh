using Enterprise.Model.Perfo.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 数据上报
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhSjsbModel : PerfoSuperModel
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
        ///上报状态
        /// </summary>
        public virtual string SBZT
        {
            get;
            set;
        }

        /// <summary>
        ///上报时间
        /// </summary>
        public virtual DateTime? SBSJ
        {
            get;
            set;
        }

        /// <summary>
        ///上报人
        /// </summary>
        public virtual string SBR
        {
            get;
            set;
        }

        /// <summary>
        ///附件
        /// </summary>
        public virtual string SBFJ
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
