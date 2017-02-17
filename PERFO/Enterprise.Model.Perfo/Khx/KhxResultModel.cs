using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khx
{
    /// <summary>
    /// 考核得分
	/// 创建人:代码生成器
	/// 创建时间:	2015/11/12 19:37:11
    /// </summary>
    [Serializable]
    public class KhxResultModel : PerfoSuperModel
    {
        #region 代码生成器
        
			/// <summary>
			///得分对应指标项的ID
			/// </summary>
			public virtual string FORID
			{
				get;
				set;
			}

			/// <summary>
			///得分
			/// </summary>
			public virtual int? SCORE
			{
				get;
				set;
			}

			/// <summary>
			///单位、模板关系表ID
			/// </summary>
			public virtual string MBJGID
			{
				get;
				set;
			}

			/// <summary>
			///加减分描述
			/// </summary>
			public virtual string MS
			{
				get;
				set;
			}

			/// <summary>
			///考核流程表ID
			/// </summary>
			public virtual string KHID
			{
				get;
				set;
			}

			/// <summary>
			///评分类别,xm1,xm2,nr
			/// </summary>
			public virtual string TYPE
			{
				get;
				set;
			}

        #endregion
    }

}
