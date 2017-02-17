using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 考核统计表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:52
    /// </summary>
    [Serializable]
    public class KhTjbModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///考核ID
			/// </summary>
			public virtual int KHID
			{
				get;
				set;
			}

			/// <summary>
			///未填报
			/// </summary>
			public virtual int? WTB
			{
				get;
				set;
			}

			/// <summary>
			///未提交
			/// </summary>
			public virtual int? WTJ
			{
				get;
				set;
			}

			/// <summary>
			///数据填报
			/// </summary>
			public virtual int? SJTB
			{
				get;
				set;
			}

			/// <summary>
			///文件提交
			/// </summary>
			public virtual int? WJTJ
			{
				get;
				set;
			}

			/// <summary>
			///已审核
			/// </summary>
			public virtual int? YSH
			{
				get;
				set;
			}

			/// <summary>
			///已审定
			/// </summary>
			public virtual int? YSD
			{
				get;
				set;
			}

			/// <summary>
			///考核进度
			/// </summary>
			public virtual int? KHJD
			{
				get;
				set;
			}

			/// <summary>
			///统计日期
			/// </summary>
			public virtual DateTime? TJRQ
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

			/// <summary>
			///备用3
			/// </summary>
			public virtual string BY3
			{
				get;
				set;
			}

			/// <summary>
			///备用4
			/// </summary>
			public virtual string BY4
			{
				get;
				set;
			}

        #endregion
    }

}
