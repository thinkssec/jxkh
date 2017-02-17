using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khs
{
    /// <summary>
    /// 上级考核评分
	/// 创建人:代码生成器
	/// 创建时间:	2015/11/9 10:04:44
    /// </summary>
    [Serializable]
    public class KhsSjkhModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///uid
			/// </summary>
			public virtual string ID
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
			///考核人
			/// </summary>
			public virtual string LOGINID
			{
				get;
				set;
			}

			/// <summary>
			///被评机构
			/// </summary>
			public virtual int? JGBM
			{
				get;
				set;
			}

			/// <summary>
			///评分
			/// </summary>
			public virtual decimal? PF
			{
				get;
				set;
			}

			/// <summary>
			///备注
			/// </summary>
			public virtual string BZ
			{
				get;
				set;
			}

			/// <summary>
			///录入时间
			/// </summary>
			public virtual DateTime? LRSJ
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KHDZBID
			{
				get;
				set;
			}

        #endregion
    }

}
