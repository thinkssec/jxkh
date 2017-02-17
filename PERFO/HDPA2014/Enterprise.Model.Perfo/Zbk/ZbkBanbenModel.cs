using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// 指标版本管理
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkBanbenModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///版本名称
        /// </summary>
        public virtual string BBMC
        {
            get;
            set;
        }

        /// <summary>
        ///启用时间
        /// </summary>
        public virtual DateTime? QYSJ
        {
            get;
            set;
        }

        #endregion
    }

}
