using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 机关作风建设考核汇总表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/28 16:45:02
    /// </summary>
    [Serializable]
    public class KhJgzfbModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///机关作风ID
        /// </summary>
        public virtual string ZFID
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
        ///机构编码
        /// </summary>
        public virtual int JGBM
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
        ///指标名称
        /// </summary>
        public virtual string ZBMC
        {
            get;
            set;
        }

        /// <summary>
        ///统计时间
        /// </summary>
        public virtual DateTime? TJSJ
        {
            get;
            set;
        }

        /// <summary>
        ///操作人
        /// </summary>
        public virtual string CZR
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
        ///打分者数量
        /// </summary>
        public virtual int? DFZSL
        {
            get;
            set;
        }

        /// <summary>
        /// 作风考核得分
        /// </summary>
        public virtual decimal? ZFKHDF
        {
            get;
            set;
        }

        /// <summary>
        /// 总得分
        /// </summary>
        public virtual decimal? ZDF
        {
            get;
            set;
        }

        /// <summary>
        ///排名
        /// </summary>
        public virtual int? ZFPM
        {
            get;
            set;
        }

        /// <summary>
        ///加减分
        /// </summary>
        public virtual decimal? SJDF
        {
            get;
            set;
        }

        /// <summary>
        ///指标序号
        /// </summary>
        public virtual string DFZBXH
        {
            get;
            set;
        }

        /// <summary>
        ///汇总标志
        /// </summary>
        public virtual string HZBZ
        {
            get;
            set;
        }

        #endregion

        #region 自定义关联项

        /// <summary>
        /// 部门机构对象
        /// </summary>
        public virtual SysBmjgModel Bmjg { get; set; }

        #endregion
    }

}
