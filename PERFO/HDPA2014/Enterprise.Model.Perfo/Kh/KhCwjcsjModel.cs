using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 财务基础数据表
    /// 创建人:代码生成器
    /// 创建时间:2014/12/13 10:23:50
    /// </summary>
    [Serializable]
    public class KhCwjcsjModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///财务指标ID
        /// </summary>
        public virtual string CWZBID
        {
            get;
            set;
        }

        /// <summary>
        /// 指标名称
        /// </summary>
        public virtual string ZBMC
        {
            get;
            set;
        }

        /// <summary>
        ///序号
        /// </summary>
        public virtual int XH
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
        ///年份
        /// </summary>
        public virtual int? NF
        {
            get;
            set;
        }

        /// <summary>
        ///1月份
        /// </summary>
        public virtual decimal? M1
        {
            get;
            set;
        }

        /// <summary>
        ///2月份
        /// </summary>
        public virtual decimal? M2
        {
            get;
            set;
        }

        /// <summary>
        ///3月份
        /// </summary>
        public virtual decimal? M3
        {
            get;
            set;
        }

        /// <summary>
        ///4月份
        /// </summary>
        public virtual decimal? M4
        {
            get;
            set;
        }

        /// <summary>
        ///5月份
        /// </summary>
        public virtual decimal? M5
        {
            get;
            set;
        }

        /// <summary>
        ///6月份
        /// </summary>
        public virtual decimal? M6
        {
            get;
            set;
        }

        /// <summary>
        ///7月份
        /// </summary>
        public virtual decimal? M7
        {
            get;
            set;
        }

        /// <summary>
        ///8月份
        /// </summary>
        public virtual decimal? M8
        {
            get;
            set;
        }

        /// <summary>
        ///9月份
        /// </summary>
        public virtual decimal? M9
        {
            get;
            set;
        }

        /// <summary>
        ///10月份
        /// </summary>
        public virtual decimal? M10
        {
            get;
            set;
        }

        /// <summary>
        ///11月份
        /// </summary>
        public virtual decimal? M11
        {
            get;
            set;
        }

        /// <summary>
        ///12月份
        /// </summary>
        public virtual decimal? M12
        {
            get;
            set;
        }

        /// <summary>
        ///月份数
        /// </summary>
        public virtual int? SL
        {
            get;
            set;
        }

        /// <summary>
        ///平均值
        /// </summary>
        public virtual decimal? PJZ
        {
            get;
            set;
        }

        /// <summary>
        ///累计值
        /// </summary>
        public virtual decimal? LJZ
        {
            get;
            set;
        }

        /// <summary>
        ///备用1
        /// </summary>
        public virtual string BZ1
        {
            get;
            set;
        }

        /// <summary>
        ///备用2
        /// </summary>
        public virtual string BZ2
        {
            get;
            set;
        }

        /// <summary>
        ///备用数据1
        /// </summary>
        public virtual decimal? BZD1
        {
            get;
            set;
        }

        /// <summary>
        ///备用数据2
        /// </summary>
        public virtual decimal? BZD2
        {
            get;
            set;
        }

        #endregion
    }

}
