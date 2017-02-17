using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 通知签收表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhSigninModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///签收ID
			/// </summary>
			public virtual int QSID
			{
				get;
				set;
			}

			/// <summary>
			///通知ID
			/// </summary>
			public virtual string TZID
			{
				get;
				set;
			}

			/// <summary>
			///签收单位
			/// </summary>
			public virtual string QSDW
			{
				get;
				set;
			}

			/// <summary>
			///签收人
			/// </summary>
			public virtual string QSR
			{
				get;
				set;
			}

			/// <summary>
			///签收日期
			/// </summary>
			public virtual DateTime? QSRQ
			{
				get;
				set;
			}

        #endregion
    }

}
