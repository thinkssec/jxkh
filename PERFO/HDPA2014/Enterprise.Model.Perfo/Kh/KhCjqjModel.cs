using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 成绩区间设置
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:51
    /// </summary>
    [Serializable]
    public class KhCjqjModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///考核ID
        /// </summary>
        public virtual int? KHID
        {
            get;
            set;
        }

        /// <summary>
        ///区间等级
        /// </summary>
        public virtual string QJDJ
        {
            get;
            set;
        }

        /// <summary>
        ///最大值
        /// </summary>
        public virtual decimal? UPPERV
        {
            get;
            set;
        }

        /// <summary>
        ///最小值
        /// </summary>
        public virtual decimal? LOWERV
        {
            get;
            set;
        }

        #endregion

        #region 关联数据项

        /// <summary>
        /// 考核期实例
        /// </summary>
        public virtual KhKhglModel Kaohe { get; set; }

        #endregion
    }

}
