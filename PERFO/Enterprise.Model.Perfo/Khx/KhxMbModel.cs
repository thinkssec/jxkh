using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khx
{
    /// <summary>
    /// 考核模板
	/// 创建人:代码生成器
	/// 创建时间:	2015/11/5 20:46:51
    /// </summary>
    [Serializable]
    public class KhxMbModel : PerfoSuperModel
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
			///模板名称
			/// </summary>
			public virtual string NAME
			{
				get;
				set;
			}

			/// <summary>
			///备注
			/// </summary>
			public virtual string MEMO
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual DateTime ADDTIME
			{
				get;
				set;
			}

			/// <summary>
			///权重（%）
			/// </summary>
			public virtual int? QZ
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

        #endregion
    }

}
