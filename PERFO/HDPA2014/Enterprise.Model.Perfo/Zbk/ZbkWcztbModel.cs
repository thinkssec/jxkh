using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// 完成值填报
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkWcztbModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///量化指标编码
			/// </summary>
			public virtual string LHZBBM
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

        #endregion
    }

}
