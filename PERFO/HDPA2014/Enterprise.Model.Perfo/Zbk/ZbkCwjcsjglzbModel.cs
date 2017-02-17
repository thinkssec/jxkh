using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Zbk
{
    /// <summary>
    /// 关联指标与财务基础数据对应表
    /// 创建人:代码生成器
    /// 创建时间:2014/12/14 9:19:20
    /// </summary>
    [Serializable]
    public class ZbkCwjcsjglzbModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///对应表ID
			/// </summary>
			public virtual string ID
			{
				get;
				set;
			}

			/// <summary>
			///指标项名称
			/// </summary>
			public virtual string ZBXMC
			{
				get;
				set;
			}

			/// <summary>
			///财务基础数据指标
			/// </summary>
			public virtual string JCSJZB
			{
				get;
				set;
			}

			/// <summary>
			///数据类型
			/// </summary>
			public virtual string JCSJLX
			{
				get;
				set;
			}

			/// <summary>
			///备用1
			/// </summary>
			public virtual string BY1
			{
				get;
				set;
			}

			/// <summary>
			///备用2
			/// </summary>
			public virtual string BY2
			{
				get;
				set;
			}

        #endregion
    }

}
