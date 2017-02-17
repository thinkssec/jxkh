using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Sys
{
    /// <summary>
    /// 用户分管机构
    /// 创建人:代码生成器
    /// 创建时间:2014/11/1 0:35:45
    /// </summary>
    [Serializable]
    public class SysFgbmjgModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///机构编码
			/// </summary>
			public virtual int JGBM
			{
				get;
				set;
			}

			/// <summary>
			///登录ID
			/// </summary>
			public virtual string LOGINID
			{
				get;
				set;
			}

        #endregion
    }

}
