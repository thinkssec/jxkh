using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khs
{
    /// <summary>
    /// 
	/// 创建人:代码生成器
	/// 创建时间:	2015/11/7 19:54:34
    /// </summary>
    [Serializable]
    public class KhsQzModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///
			/// </summary>
			public virtual string QZID
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

			/// <summary>
			///
			/// </summary>
			public virtual int? JGBM
			{
				get;
				set;
			}

			/// <summary>
			///权
			/// </summary>
			public virtual decimal? QZ
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

        #endregion
    }

}
