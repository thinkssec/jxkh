using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 机关部门打分表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:51
    /// </summary>
    [Serializable]
    public class KhJgbmdfbModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///打分表ID
        /// </summary>
        public virtual string DFBID
        {
            get;
            set;
        }

        /// <summary>
        ///量化考核表ID
        /// </summary>
        public virtual string ID
        {
            get;
            set;
        }

        /// <summary>
        ///打分指标ID
        /// </summary>
        public virtual string DFZBID
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
        ///考核ID
        /// </summary>
        public virtual int? KHID
        {
            get;
            set;
        }

        /// <summary>
        ///完成进度
        /// </summary>
        public virtual int? WCJD
        {
            get;
            set;
        }

        /// <summary>
        ///考核得分
        /// </summary>
        public virtual decimal? KHDF
        {
            get;
            set;
        }

        /// <summary>
        ///得分说明
        /// </summary>
        public virtual string DFSM
        {
            get;
            set;
        }

        /// <summary>
        ///打分者
        /// </summary>
        public virtual string DFZ
        {
            get;
            set;
        }

        /// <summary>
        ///打分时间
        /// </summary>
        public virtual DateTime? DFSJ
        {
            get;
            set;
        }

        /// <summary>
        ///打分者类型
        /// </summary>
        public virtual string DFZLX
        {
            get;
            set;
        }

        /// <summary>
        ///打分权重
        /// </summary>
        public virtual decimal? DFQZ
        {
            get;
            set;
        }

        #endregion
    }

}
