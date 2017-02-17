using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 考核类型表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhKindModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///考核类型ID
			/// </summary>
			public virtual string LXID
			{
				get;
				set;
			}

			/// <summary>
			///类型名称
			/// </summary>
			public virtual string LXMC
			{
				get;
				set;
			}

			/// <summary>
			///生效时间
			/// </summary>
			public virtual DateTime? SXSJ
			{
				get;
				set;
			}

			/// <summary>
			///当前状态
			/// </summary>
			public virtual string DQZT
			{
				get;
				set;
			}

        #endregion
    }

}
