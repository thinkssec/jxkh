using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Sys;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 年度绩效责任书指标
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhJxzrszbModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///责任书指标ID
        /// </summary>
        public virtual string ZRSZBID
        {
            get;
            set;
        }

        /// <summary>
        ///量化指标编码
        /// </summary>
        public virtual string LHZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///打分指标编码
        /// </summary>
        public virtual string DFZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///责任书ID
        /// </summary>
        public virtual int? ZRSID
        {
            get;
            set;
        }

        /// <summary>
        ///所在年度
        /// </summary>
        public virtual int? ZSZND
        {
            get;
            set;
        }

        /// <summary>
        ///指标权重
        /// </summary>
        public virtual decimal? ZZBQZ
        {
            get;
            set;
        }

        /// <summary>
        ///指标分值
        /// </summary>
        public virtual decimal? ZZBFZ
        {
            get;
            set;
        }

        /// <summary>
        ///显示序号
        /// </summary>
        public virtual int? ZXSXH
        {
            get;
            set;
        }

        /// <summary>
        ///年初目标值
        /// </summary>
        public virtual decimal? ZNCMBZ
        {
            get;
            set;
        }

        /// <summary>
        ///年中目标值
        /// </summary>
        public virtual decimal? ZNZMBZ
        {
            get;
            set;
        }

        /// <summary>
        ///年末目标值
        /// </summary>
        public virtual decimal? ZNMMBZ
        {
            get;
            set;
        }

        /// <summary>
        ///目标值
        /// </summary>
        public virtual decimal? ZMBZ
        {
            get;
            set;
        }

        /// <summary>
        ///指标代号
        /// </summary>
        public virtual string ZZBDH
        {
            get;
            set;
        }

        /// <summary>
        ///指标性质
        /// </summary>
        public virtual string ZZBXZ
        {
            get;
            set;
        }

        /// <summary>
        ///计算关系式
        /// </summary>
        public virtual string ZJSGXS
        {
            get;
            set;
        }

        /// <summary>
        ///上级指标
        /// </summary>
        public virtual string ZSJZB
        {
            get;
            set;
        }

        /// <summary>
        ///提交日期
        /// </summary>
        public virtual DateTime? ZTJRQ
        {
            get;
            set;
        }

        /// <summary>
        ///所属机构
        /// </summary>
        public virtual int? ZJGBM
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
        /// 目标值备注
        /// </summary>
        public virtual string MBZBZ
        {
            get;
            set;
        }

        /// <summary>
        /// 机关部门考核主要内容
        /// </summary>
        public virtual string JGKHNR
        {
            get;
            set;
        }

        /// <summary>
        /// 机关部门考核目标
        /// </summary>
        public virtual string JGKHMB
        {
            get;
            set;
        }

        /// <summary>
        /// 机关部门完成时间说明
        /// </summary>
        public virtual string JGWCSJ
        {
            get;
            set;
        }

        /// <summary>
        /// 机关部门考核评分标准
        /// </summary>
        public virtual string JGPFBZ
        {
            get;
            set;
        }

        #endregion

        #region 关联数据项

        /// <summary>
        /// 组织机构
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        /// <summary>
        /// 绩效责任书
        /// </summary>
        public virtual KhJxzrsModel Jxzrs { get; set; }

        /// <summary>
        /// 量化指标
        /// </summary>
        public virtual ZbkLhzbModel Lhzb { get; set; }

        /// <summary>
        /// 打分指标
        /// </summary>
        public virtual ZbkDfzbModel Dfzb { get; set; }

        #endregion
    }

}
