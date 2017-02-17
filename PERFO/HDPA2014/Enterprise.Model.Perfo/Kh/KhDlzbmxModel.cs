using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 定量指标考核表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:51
    /// </summary>
    [Serializable]
    public class KhDlzbmxModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///量化考核表ID
        /// </summary>
        public virtual string ID
        {
            get;
            set;
        }

        /// <summary>
        ///筛选表ID
        /// </summary>
        public virtual string SXID
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
        ///指标编码
        /// </summary>
        public virtual string ZBBM
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
        ///年初目标值
        /// </summary>
        public virtual decimal? NCMBZ
        {
            get;
            set;
        }

        /// <summary>
        ///考核目标申请值
        /// </summary>
        public virtual decimal? MBZSQZ
        {
            get;
            set;
        }

        /// <summary>
        ///目标值申请说明
        /// </summary>
        public virtual string MBZSQBZ
        {
            get;
            set;
        }

        /// <summary>
        ///考核目标值
        /// </summary>
        public virtual decimal? MBZ
        {
            get;
            set;
        }

        /// <summary>
        ///目标值说明
        /// </summary>
        public virtual string MBZBZ
        {
            get;
            set;
        }

        /// <summary>
        ///目标值确认人
        /// </summary>
        public virtual string MBZQRR
        {
            get;
            set;
        }

        /// <summary>
        ///完成值申请值
        /// </summary>
        public virtual decimal? WCZSQZ
        {
            get;
            set;
        }

        /// <summary>
        ///完成值申请人
        /// </summary>
        public virtual string WCZSQR
        {
            get;
            set;
        }

        /// <summary>
        ///完成值申请说明
        /// </summary>
        public virtual string WCZSQBZ
        {
            get;
            set;
        }

        /// <summary>
        ///最终完成值
        /// </summary>
        public virtual decimal? WCZ
        {
            get;
            set;
        }

        /// <summary>
        /// 完成值说明
        /// </summary>
        public virtual string WCZBZ
        {
            get;
            set;
        }

        /// <summary>
        ///完成值审核值
        /// </summary>
        public virtual decimal? WCZSHZ
        {
            get;
            set;
        }

        /// <summary>
        ///完成值审核人
        /// </summary>
        public virtual string WCZSHR
        {
            get;
            set;
        }

        /// <summary>
        ///完成值审核说明
        /// </summary>
        public virtual string WCZSHBZ
        {
            get;
            set;
        }

        /// <summary>
        ///完成值审定值
        /// </summary>
        public virtual decimal? WCZSDZ
        {
            get;
            set;
        }

        /// <summary>
        ///完成值审定说明
        /// </summary>
        public virtual string WCZSDBZ
        {
            get;
            set;
        }

        /// <summary>
        ///完成值审定人
        /// </summary>
        public virtual string WCZSDR
        {
            get;
            set;
        }

        /// <summary>
        ///指标考核状态 
        ///0=未提交 1=已提交|未审核 2=已审核|未审定 3=完成审定
        /// </summary>
        public virtual string ZBKHZT
        {
            get;
            set;
        }

        /// <summary>
        ///目标值确认日期
        /// </summary>
        public virtual DateTime? MBZQRRQ
        {
            get;
            set;
        }

        /// <summary>
        ///完成值申请日期
        /// </summary>
        public virtual DateTime? WCZSQRQ
        {
            get;
            set;
        }

        /// <summary>
        ///完成值审核日期
        /// </summary>
        public virtual DateTime? WCZSHRQ
        {
            get;
            set;
        }

        /// <summary>
        ///完成值审定日期
        /// </summary>
        public virtual DateTime? WCZSDRQ
        {
            get;
            set;
        }

        /// <summary>
        ///百分数分值
        /// </summary>
        public virtual decimal? BFSFZ
        {
            get;
            set;
        }

        /// <summary>
        ///实际得分
        /// </summary>
        public virtual decimal? SJDF
        {
            get;
            set;
        }

        /// <summary>
        ///考核对象 
        ///二级单位 = 1,领导班子 = 2,机关部门 = 3,部门负责人 = 4
        /// </summary>
        public virtual string KHDX
        {
            get;
            set;
        }

        /// <summary>
        /// 得分计算情况描述
        /// </summary>
        public virtual string DFJSQK
        {
            get;
            set;
        }

        /// <summary>
        /// 是否重新计算--标志
        /// </summary>
        public virtual bool IsCalculate
        {
            get;
            set;
        }

        #endregion

        #region 关联数据项

        /// <summary>
        /// 指标筛选MODEL
        /// </summary>
        public virtual KhZbsxModel ZbsxModel { get; set; }

        /// <summary>
        /// 量化指标MODEL
        /// </summary>
        public virtual ZbkLhzbModel LhzbModel { get; set; }

        /// <summary>
        /// 审核人打分结果集合
        /// </summary>
        public virtual IList<KhJgbmdfbModel> KhJgbmdfbLst { get; set; }

        /// <summary>
        /// 量化指标基础数据表
        /// </summary>
        public virtual IList<KhLhzbjcsjModel> LhzbjcsjLst { get; set; }

        #endregion
    }

}
