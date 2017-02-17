using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// 被打分机构
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:46
    /// </summary>
    [Serializable]
    public class ZbkBdfjgModel : PerfoSuperModel
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
