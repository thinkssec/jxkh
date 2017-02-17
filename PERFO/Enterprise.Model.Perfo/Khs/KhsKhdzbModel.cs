using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khs
{
    /// <summary>
    /// 考核指标对照表
	/// 创建人:代码生成器
	/// 创建时间:	2015/11/4 23:29:41
    /// </summary>
    [Serializable]
    public class KhsKhdzbModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///
			/// </summary>
			public virtual string ID
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual int? KHID
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string DZBID
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual decimal? PX
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual int? STATUS
			{
				get;
				set;
			}

        #endregion
    }

}
