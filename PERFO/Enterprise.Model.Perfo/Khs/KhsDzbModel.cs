using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khs
{
    /// <summary>
    /// 考核大指标
	/// 创建人:代码生成器
	/// 创建时间:	2015/11/10 20:02:20
    /// </summary>
    [Serializable]
    public class KhsDzbModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///大指标ID
			/// </summary>
			public virtual string DZBID
			{
				get;
				set;
			}

			/// <summary>
			///指标名称
			/// </summary>
			public virtual string ZBMC
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

			/// <summary>
			///
			/// </summary>
			public virtual string KHXM
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KHNR
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KHFS
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KHBZ
			{
				get;
				set;
			}

			/// <summary>
			///考核周期
			/// </summary>
			public virtual string KHZQ
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual int? PX
			{
				get;
				set;
			}

        #endregion
    }

}
