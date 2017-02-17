using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 考核节点运行表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhNoderunModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///运行表ID
        /// </summary>
        public virtual string YXID
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
        ///节点ID
        /// </summary>
        public virtual int? JDID
        {
            get;
            set;
        }

        /// <summary>
        ///节点运行状态
        /// </summary>
        public virtual string YXZT
        {
            get;
            set;
        }

        /// <summary>
        ///节点开始时间
        /// </summary>
        public virtual DateTime? YXKSSJ
        {
            get;
            set;
        }

        /// <summary>
        ///节点完成时间
        /// </summary>
        public virtual DateTime? YXWCSJ
        {
            get;
            set;
        }

        /// <summary>
        ///节点进度值
        /// </summary>
        public virtual int? YXJDZ
        {
            get;
            set;
        }

        #endregion

        #region 关联数据项

        /// <summary>
        /// 节点对象实例
        /// </summary>
        public virtual KhNodesModel JdNode { get; set; }

        #endregion

    }

}
