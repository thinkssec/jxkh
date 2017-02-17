using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khs
{
    /// <summary>
    /// 
	/// 创建人:代码生成器
	/// 创建时间:	2015/11/5 19:18:56
    /// </summary>
    [Serializable]
    public class KhsKhbzModel : PerfoSuperModel
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
			public virtual string KHDZBID
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string MC
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

			/// <summary>
			///负责部门
			/// </summary>
			public virtual int? FZBM
			{
				get;
				set;
			}

        #endregion
    }

}
