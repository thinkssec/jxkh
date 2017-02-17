using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 考核指标汇总表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:53
    /// </summary>
    [Serializable]
    public class KhZbsxModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///筛选表ID
        /// </summary>
        public virtual string SXID
        {
            get;
            set;
        }

        /// <summary>
        ///责任书ID
        /// </summary>
        public virtual string ZRSZBID
        {
            get;
            set;
        }

        /// <summary>
        ///考核ID
        /// </summary>
        public virtual int KHID
        {
            get;
            set;
        }

        /// <summary>
        ///指标编码
        /// </summary>
        public virtual string SXZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///指标权重
        /// </summary>
        public virtual decimal? SXQZ
        {
            get;
            set;
        }

        /// <summary>
        ///指标分值
        /// </summary>
        public virtual decimal? SXFZ
        {
            get;
            set;
        }

        /// <summary>
        ///筛选显示序号
        /// </summary>
        public virtual int? SXXH
        {
            get;
            set;
        }

        /// <summary>
        ///所属机构
        /// </summary>
        public virtual int SXJGBM
        {
            get;
            set;
        }

        /// <summary>
        /// 暂时停用----基准分值
        /// 机关部门考核
        /// </summary>
        public virtual decimal? SXJZFZ
        {
            get;
            set;
        }

        #endregion

        #region 关联数据项

        /// <summary>
        /// 考核期
        /// </summary>
        public virtual KhKhglModel Kaohe { get; set; }

        /// <summary>
        /// 绩效责任书指标
        /// </summary>
        public virtual KhJxzrszbModel JxzrsZb { get; set; }

        #endregion

        /// <summary>
        ///所属机构
        /// </summary>
        public virtual string JGBM
        {
            get
            {
                return SXJGBM.ToString();
            }
        }

    }

}
