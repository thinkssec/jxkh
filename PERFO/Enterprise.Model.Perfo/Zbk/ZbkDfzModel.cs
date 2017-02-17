using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// 打分者
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkDfzModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///打分指标编码
			/// </summary>
			public virtual string DFZBBM
			{
				get;
				set;
			}

			/// <summary>
			///操作者
			/// </summary>
			public virtual string OPERATOR
			{
				get;
				set;
			}

			/// <summary>
			///操作者类型 1=机构 0=指定用户
			/// </summary>
			public virtual string OPERTYPE
			{
				get;
				set;
			}

        #endregion
    }

}
