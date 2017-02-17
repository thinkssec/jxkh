using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 考核节点表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhNodesModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///节点ID
        /// </summary>
        public virtual int JDID
        {
            get;
            set;
        }

        /// <summary>
        ///考核类型ID
        /// </summary>
        public virtual string LXID
        {
            get;
            set;
        }

        /// <summary>
        ///节点名称
        /// </summary>
        public virtual string JDMC
        {
            get;
            set;
        }

        /// <summary>
        ///节点关联表
        /// </summary>
        public virtual string JDGLB
        {
            get;
            set;
        }

        /// <summary>
        ///节点顺序号
        /// </summary>
        public virtual string JDXH
        {
            get;
            set;
        }

        /// <summary>
        ///节点进度值
        /// </summary>
        public virtual int? JDZ
        {
            get;
            set;
        }

        /// <summary>
        ///有效标志
        /// </summary>
        public virtual string YXBZ
        {
            get;
            set;
        }

        /// <summary>
        ///添加日期
        /// </summary>
        public virtual DateTime? TJRQ
        {
            get;
            set;
        }

        #endregion
    }

}
