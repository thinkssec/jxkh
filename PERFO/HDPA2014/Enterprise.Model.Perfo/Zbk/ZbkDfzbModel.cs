using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// 打分指标维护
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkDfzbModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///打分指标编码
        /// </summary>
        public virtual string DFZBBM
        {
            get;
            set;
        }

        /// <summary>
        ///指标ID
        /// </summary>
        public virtual int ZBID
        {
            get;
            set;
        }

        /// <summary>
        ///版本名称
        /// </summary>
        public virtual string BBMC
        {
            get;
            set;
        }

        /// <summary>
        ///极限分值
        /// </summary>
        public virtual decimal? JXFZ
        {
            get;
            set;
        }

        /// <summary>
        ///评分类型
        /// </summary>
        public virtual string PFLX
        {
            get;
            set;
        }

        /// <summary>
        ///是否否决项
        /// </summary>
        public virtual string SFFJX
        {
            get;
            set;
        }

        /// <summary>
        ///评分标准
        /// </summary>
        public virtual string PFBZ
        {
            get;
            set;
        }

        /// <summary>
        ///是否禁用 1=是
        /// </summary>
        public virtual string DISABLE
        {
            get;
            set;
        }

        /// <summary>
        ///表达式
        /// </summary>
        public virtual string DFBDS
        {
            get;
            set;
        }

        /// <summary>
        ///最大值
        /// </summary>
        public virtual decimal? MAXV
        {
            get;
            set;
        }

        /// <summary>
        ///最小值
        /// </summary>
        public virtual decimal? MINV
        {
            get;
            set;
        }

        /// <summary>
        /// 计算规则ID
        /// </summary>
        public virtual string GZID
        {
            get;
            set;
        }

        /// <summary>
        /// 旧表ID
        /// </summary>
        public virtual string OLDID
        {
            get;
            set;
        }

        #endregion

        #region 自定义属性

        /// <summary>
        /// 指标基础信息
        /// </summary>
        public virtual ZbkZbxxModel Zbxx { get; set; }
        /// <summary>
        /// 计算规则信息
        /// </summary>
        public virtual ZbkJsgzModel Jsgz { get; set; }

        //说明：关联的字段不是表中的主键，需要修改对应的hbm文件，人为新增主键才行
        /// <summary>
        /// 被打分机构集合
        /// </summary>
        public virtual IList<ZbkBdfjgModel> BdfjgLst { get; set; }
        /// <summary>
        /// 打分者集合
        /// </summary>
        public virtual IList<ZbkDfzModel> DfzLst { get; set; }

        #endregion

    }

}
