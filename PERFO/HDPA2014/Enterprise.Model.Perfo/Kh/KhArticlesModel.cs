using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 通知公告
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:50
    /// </summary>
    [Serializable]
    public class KhArticlesModel : PerfoSuperModel
    {
        #region 代码生成器

        /// <summary>
        ///通知ID
        /// </summary>
        public virtual string TZID
        {
            get;
            set;
        }

        /// <summary>
        ///标题
        /// </summary>
        public virtual string TZBT
        {
            get;
            set;
        }

        /// <summary>
        ///内容
        /// </summary>
        public virtual string TZNR
        {
            get;
            set;
        }

        /// <summary>
        ///作者
        /// </summary>
        public virtual string TZZZ
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

        /// <summary>
        ///附件
        /// </summary>
        public virtual string TZFJ
        {
            get;
            set;
        }

        /// <summary>
        ///浏览次数
        /// </summary>
        public virtual int? LLCS
        {
            get;
            set;
        }

        /// <summary>
        ///通知类型
        /// </summary>
        public virtual string TZLX
        {
            get;
            set;
        }

        #endregion

        #region 关联数据项

        /// <summary>
        /// 签收列表对象
        /// </summary>
        public virtual IList<KhSigninModel> SigninLst { get; set; }

        #endregion
    }

}
