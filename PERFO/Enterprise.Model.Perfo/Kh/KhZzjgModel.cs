using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Kh
{
    /// <summary>
    /// 最终结果表
    /// 创建人:代码生成器
    /// 创建时间:2014/11/9 17:03:53
    /// </summary>
    [Serializable]
    public class KhZzjgModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///结果ID
			/// </summary>
			public virtual int JGID
			{
				get;
				set;
			}

			/// <summary>
			///考核ID
			/// </summary>
			public virtual int? KHID
			{
				get;
				set;
			}

			/// <summary>
			///机构编码
			/// </summary>
			public virtual int? JGBM
			{
				get;
				set;
			}

			/// <summary>
			///约束扣分
			/// </summary>
            public virtual decimal? YSZBKF
			{
				get;
				set;
			}

			/// <summary>
			///加分得分
			/// </summary>
            public virtual decimal? JFZBDF
			{
				get;
				set;
			}

			/// <summary>
			///加减分得分
			/// </summary>
            public virtual decimal? JJFDF
			{
				get;
				set;
			}

			/// <summary>
			///量化指标得分
			/// </summary>
			public virtual decimal? LHZBDF
			{
				get;
				set;
			}

			/// <summary>
			///原始得分
			/// </summary>
			public virtual decimal? YSDF
			{
				get;
				set;
			}

			/// <summary>
			///难度系数
			/// </summary>
			public virtual decimal? NDXS
			{
				get;
				set;
			}

			/// <summary>
			///最终得分
			/// </summary>
			public virtual decimal? ZZDF
			{
				get;
				set;
			}

			/// <summary>
			///得分等级
			/// </summary>
			public virtual string DFDJ
			{
				get;
				set;
			}

			/// <summary>
			///兑现倍数
			/// </summary>
			public virtual decimal? DXBS
			{
				get;
				set;
			}

			/// <summary>
			///生成日期
			/// </summary>
			public virtual DateTime? SCRQ
			{
				get;
				set;
			}

			/// <summary>
			///考核类型
			/// </summary>
			public virtual string KHLX
			{
				get;
				set;
			}

			/// <summary>
			///排名顺序
			/// </summary>
			public virtual int? PMSX
			{
				get;
				set;
			}

        #endregion
    }

}
