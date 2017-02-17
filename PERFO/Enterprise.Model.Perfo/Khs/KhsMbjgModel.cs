using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Model.Perfo.Khs
{
    /// <summary>
    /// 模板机构
	/// 创建人:代码生成器
	/// 创建时间:	2015/11/12 18:44:38
    /// </summary>
    [Serializable]
    public class KhsMbjgModel : PerfoSuperModel
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
			///
			/// </summary>
			public virtual string KHDZBID
			{
				get;
				set;
			}

			/// <summary>
			///模板id
			/// </summary>
			public virtual string MBID
			{
				get;
				set;
			}

			/// <summary>
			///机构
			/// </summary>
			public virtual int? JGBM
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual decimal? PX
			{
				get;
				set;
			}

			/// <summary>
			///附件文件名称
			/// </summary>
			public virtual string FNAMES
			{
				get;
				set;
			}

			/// <summary>
			///附件保存名称
			/// </summary>
			public virtual string FVIEWNAMES
			{
				get;
				set;
			}

			/// <summary>
			///考核状态，0：配置中，1：已提交，2：评分中
			/// </summary>
			public virtual string STATUS
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KEY1
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KEY2
			{
				get;
				set;
			}

			/// <summary>
			///
			/// </summary>
			public virtual string KEY3
			{
				get;
				set;
			}

        #endregion
    }

}
